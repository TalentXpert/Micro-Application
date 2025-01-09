using Microsoft.EntityFrameworkCore;

namespace BaseLibrary.Database
{
    public class QueryableUnitOfWork : DbContext, IQueryableUnitOfWork
    {

        public QueryableUnitOfWork(DbContextOptions options, IEntityValidator entityValidator) : base(options)
        {
            this.EntityValidator = entityValidator;
        }

        #region IQueryableUnitOfWork Members

        public DbSet<TEntity> CreateSet<TEntity>() where TEntity : class
        {

            return Set<TEntity>();
        }

        public new void Attach<TEntity>(TEntity item) where TEntity : class
        {
            //attach and set as unchanged
            Entry<TEntity>(item).State = EntityState.Unchanged;
        }

        public void SetModified<TEntity>(TEntity item) where TEntity : class
        {
            //this operation also attach item in object state manager
            Entry<TEntity>(item).State = EntityState.Modified;
        }

        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : class
        {
            //if not is attached, attach original and set current values
            Entry<TEntity>(original).CurrentValues.SetValues(current);
        }

        public void Commit()
        {
            base.SaveChanges();
        }

        public void CommitAndRefreshChanges()
        {
            bool saveFailed = false;

            do
            {
                try
                {
                    base.SaveChanges();

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList()
                      .ForEach(entry =>
                      {
                          entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                      });

                }
            } while (saveFailed);

        }

        public void RollbackChanges()
        {
            // set all entities in change tracker // as 'unchanged state'
            base.ChangeTracker.Entries()
                .ToList()
                .ForEach(entry => entry.State = EntityState.Unchanged);
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return null;
            // return Database..SqlQuery<TEntity>(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return 0;// Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        public IEntityValidator EntityValidator { get; set; }

        #endregion
    }
}
