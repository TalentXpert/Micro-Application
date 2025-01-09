
namespace MicroAppAPI.Services
{
    public class ServiceFactory : BaseLibraryServiceFactory, IServiceFactory
    {
        public IUserManagementDatabase UnitOfWork { get; private set; }
        public ILoggerFactory LoggerFactory { get; }
        public IRepositoryFactory RepositoryFactory { get; set; }
        public ServiceFactory(IUserManagementDatabase unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork, loggerFactory)
        {
            UnitOfWork = unitOfWork;
            LoggerFactory = loggerFactory;
            RepositoryFactory = new RepositoryFactory(UnitOfWork, LoggerFactory);
            //ApplicationControlBaseFactory = new UIControlFactory(this);
            //MicroAppContract = new MicroAppContract(this);
        }

        public void StartNewUnitOfWork()
        {
            UnitOfWork = UserManagementDatabase.GetDatabase(ApplicationSettingBase.DatabaseConnectionString);
            RepositoryFactory = new RepositoryFactory(UnitOfWork, LoggerFactory);
        }





    }
}
