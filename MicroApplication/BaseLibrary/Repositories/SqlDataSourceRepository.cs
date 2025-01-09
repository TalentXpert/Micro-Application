using BaseLibrary.Domain.DataSources;

namespace BaseLibrary.Repositories
{
    public interface IsqlDataSourceRepository : IRepository<SqlDataSource>
    {

    }

    public class SqlDataSourceRepository : Repository<SqlDataSource>, IsqlDataSourceRepository
    {
        IBaseDatabase unitOfWork;

        public SqlDataSourceRepository(IBaseDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<SqlDataSourceRepository>())
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
