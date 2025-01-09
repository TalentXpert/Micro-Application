

namespace MicroAppAPI.Services
{
    public class ServiceBase
    {
        protected IServiceFactory SF { get; }
        public ILoggerFactory LoggerFactory { get; }
        public IRepositoryFactory RF { get; }

        public ServiceBase(IServiceFactory serviceFactory, ILoggerFactory loggerFactory)
        {
            SF = serviceFactory; LoggerFactory = loggerFactory;
            RF = serviceFactory.RepositoryFactory;
        }
        public DataTable GetDataTable(string query)
        {
            using (var db = new SqlCommandExecutor())
            {
                var dt = db.GetDataTable(query);
                return dt;
            }
        }
    }
}
