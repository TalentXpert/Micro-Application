using BaseLibrary.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Data;
using SqlParameter = Microsoft.Data.SqlClient.SqlParameter;

namespace BaseLibrary.Database
{
    public class Repository<TEntity> : CleanCode, IRepository<TEntity> where TEntity : Entity
    {
        readonly IQueryableUnitOfWork _unitOfWork;


        /// <summary>
        /// Create a new instance of repository
        /// </summary>
        /// <param name="unitOfWork">Associated Unit Of Work</param>
        public Repository(IQueryableUnitOfWork unitOfWork, ILogger logger)
        {

            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");

            _unitOfWork = unitOfWork;
            Logger = logger;
        }

        #region IRepository Members


        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _unitOfWork;
            }
        }

        public ILogger Logger { get; }

        public virtual void Add(TEntity item)
        {
            if (item is Entity)
            {
                ((Entity)item).SetCreatedOn();
                ((Entity)item).SetUpdatedOn();
            }
            if (item != null)
            {
                var validationMessages = _unitOfWork.EntityValidator.GetInvalidMessages(item);
                if (validationMessages.Any())
                    throw new ValidationException(string.Join(" ", validationMessages));
                GetSet().Add(item); // add new item in this set
            }
            else
            {
                throw new ValidationException($"Can not add null entity into {typeof(TEntity).ToString()} repository.");
            }
        }

        public virtual void Remove(TEntity item)
        {
            if (item != null)
            {
                //attach item if not exist
                _unitOfWork.Attach(item);

                //set as "removed"
                GetSet().Remove(item);
            }
            else
            {
                this.Logger.LogInformation("Messages.info_CannotRemoveNullEntity", typeof(TEntity).ToString());
            }
        }


        public virtual void TrackItem(TEntity item)
        {
            if (item != null)
                _unitOfWork.Attach(item);
            else
            {
                this.Logger.LogInformation("Messages.info_CannotRemoveNullEntity", typeof(TEntity).ToString());
            }
        }


        public virtual void Modify(TEntity item)
        {
            if (item is Entity)
            {
                ((Entity)item).SetUpdatedOn();
            }
            if (item != null)
            {
                var validationMessages = _unitOfWork.EntityValidator.GetInvalidMessages(item);
                if (validationMessages.Any())
                    throw new ValidationException(string.Join(" ", validationMessages));
                _unitOfWork.SetModified(item);
            }
            else
            {
                this.Logger.LogInformation("Messages.info_CannotRemoveNullEntity", typeof(TEntity).ToString());
            }
        }


        public virtual TEntity? Get(Guid id)
        {
            if (id != Guid.Empty)
                return GetSet().Find(id);
            return null;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return GetSet().AsEnumerable();
        }
        public virtual TEntity? FirstOrDefault()
        {
            return GetSet().FirstOrDefault();
        }
        //public virtual IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification)
        //{
        //    return GetSet().Where(specification.SatisfiedBy())
        //                   .AsEnumerable();
        //}

        public virtual IEnumerable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, System.Linq.Expressions.Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending)
        {
            var set = GetSet();

            if (ascending)
            {
                return set.OrderBy(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount)
                          .AsEnumerable();
            }

            return set.OrderByDescending(orderByExpression)
                      .Skip(pageCount * pageIndex)
                      .Take(pageCount)
                      .AsEnumerable();
        }

        public virtual IEnumerable<TEntity> GetFiltered(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)
        {
            return GetSet().Where(filter)
                           .AsEnumerable();
        }


        public virtual void Merge(TEntity persisted, TEntity current)
        {
            _unitOfWork.ApplyCurrentValues(persisted, current);
        }

        #endregion

        #region Private Methods

        DbSet<TEntity> GetSet()
        {
            return _unitOfWork.CreateSet<TEntity>();
        }

        //public DateTime TodayStartTime(string timezone)
        //{
        //    var userDate = H.Utility.ConvertDateTime.ConvertToUserTimeZone(DateTime.UtcNow, timezone);
        //    return new DateTime(userDate.Year, userDate.Month, userDate.Day, 0, 0, 0);
        //}
        public DateTime GetDateTime(long ticks)
        {
            return new DateTime(ticks);
        }

        //public DateTime GetLastUpdatedDateOrDefault(long lastUpdated)
        //{
        //    if (lastUpdated > 0)
        //        return new DateTime(lastUpdated);
        //    return ApplicationConstants.HireXpertMinDate;
        //}
        public List<TEntity> GetDistinctEntities(List<TEntity> entities)
        {
            var dictionary = new Dictionary<Guid, TEntity>();
            foreach (var entity in entities)
            {
                if (dictionary.ContainsKey(entity.Id)) continue;
                dictionary.Add(entity.Id, entity);
            }
            return dictionary.Values.ToList();
        }

        //protected static Guid? GetOrganizationId(ApplicationUser loggedInUser)
        //{
        //    if (loggedInUser.HasOrgnization()) return loggedInUser.OrganizationId;
        //    return null;
        //}
        #endregion

        #region base query builder methods

        protected string PrepareWhereClause(List<ControlFilter> filters, string controlIdentifer, string where, string dbColumnName, SqlDbType sqlDbType, List<SqlParameter> sqlParameters)
        {
            var filter = filters.FirstOrDefault(f => f.ControlIdentifier == controlIdentifer);
            if (filter != null && string.IsNullOrWhiteSpace(filter.Value) == false)
            {
                var param = $"@{dbColumnName}";
                if (string.IsNullOrWhiteSpace(where))
                    where = $"{dbColumnName} {filter.Operator} {param}";
                else
                    where += $" and {dbColumnName} {filter.Operator} {param}";
                var aqlParameter = new SqlParameter(param, sqlDbType);
                aqlParameter.Value = filter.Value;
                sqlParameters.Add(aqlParameter);
            }
            return where;
        }

        protected string ReadText(DataRow dr, string column)
        {
            try
            {
                if (dr.IsNull(column))
                    return string.Empty;
                return dr[column].ToString();
            }
            catch { return string.Empty; }
        }

        #endregion 
    }
}
