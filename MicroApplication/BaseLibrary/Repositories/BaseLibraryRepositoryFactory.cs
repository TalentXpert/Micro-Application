using BaseLibrary.Domain.PerformanceLogs;
using UserManagement.Services.Repositories;

namespace BaseLibrary.Repositories
{
    public interface IBaseLibraryRepositoryFactory
    {
        IUserRepository UserRepository { get; }
        IUserSettingRepository UserSettingRepository { get; }
        IFormDataStoreRepository PageDataStoreRepository { get; }
        IApplicationSettingRepository ApplicationSettingRepository { get; }
        IAppFormRepository AppFormRepository { get; }
        IAppControlRepository AppControlRepository { get; }
        IAppFormControlRepository AppFormControlRepository { get; }
        IAppPageRepository AppPageRepository { get; }

        IComponentSchemaRepository ComponentSchemaRepository { get; }
        IsqlDataSourceRepository SqlDataSourceRepository { get; }
        IApplicationRoleRepository RoleRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IApplicationRolePermissionRepository ApplicationRolePermissionRepository { get; }

        IOrganizationRepository OrganizationRepository { get; }
        IAuditLogRepository AuditLogRepository { get; }
        IPerformanceLogRepository PerformanceLogRepository { get; }
        LogPerformance LogPerformance { get; }
    }

    public class BaseLibraryRepositoryFactory : IBaseLibraryRepositoryFactory
    {
        public IBaseDatabase UnitOfWork { get; }
        public ILoggerFactory LoggerFactory { get; }
        public BaseLibraryRepositoryFactory(IBaseDatabase unitOfWork, ILoggerFactory loggerFactory)
        {
            UnitOfWork = unitOfWork;
            LoggerFactory = loggerFactory;
        }

        IUserRepository? userRepository;
        public IUserRepository UserRepository { get { return userRepository ?? (userRepository = new UserRepository(UnitOfWork, LoggerFactory)); } }

        IUserSettingRepository? userSettingRepository;
        public IUserSettingRepository UserSettingRepository { get { return userSettingRepository ?? (userSettingRepository = new UserSettingRepository(UnitOfWork, LoggerFactory)); } }

        IFormDataStoreRepository? pageDataRepository;
        public IFormDataStoreRepository PageDataStoreRepository { get { return pageDataRepository ?? (pageDataRepository = new FormDataStoreRepository(UnitOfWork, LoggerFactory)); } }

        IApplicationSettingRepository? applicationSettingRepository;
        public IApplicationSettingRepository ApplicationSettingRepository { get { return applicationSettingRepository ?? (applicationSettingRepository = new ApplicationSettingRepository(UnitOfWork, LoggerFactory)); } }

        IAppFormRepository? appFormRepository;
        public IAppFormRepository AppFormRepository { get { return appFormRepository ?? (appFormRepository = new AppFormRepository(UnitOfWork, LoggerFactory)); } }

        IAppControlRepository? appControlRepository;
        public IAppControlRepository AppControlRepository { get { return appControlRepository ?? (appControlRepository = new AppControlRepository(UnitOfWork, LoggerFactory)); } }

        IAppFormControlRepository? appFormControlRepository;
        public IAppFormControlRepository AppFormControlRepository { get { return appFormControlRepository ?? (appFormControlRepository = new AppFormControlRepository(UnitOfWork, LoggerFactory)); } }

        IAppPageRepository? appPageRepository;
        public IAppPageRepository AppPageRepository { get { return appPageRepository ?? (appPageRepository = new AppPageRepository(UnitOfWork, LoggerFactory)); } }


        IComponentSchemaRepository? componentSchemaRepository;
        public IComponentSchemaRepository ComponentSchemaRepository { get { return componentSchemaRepository ?? (componentSchemaRepository = new ComponentSchemaRepository(UnitOfWork, LoggerFactory)); } }

        IsqlDataSourceRepository? sqlDataSourceRepository;
        public IsqlDataSourceRepository SqlDataSourceRepository { get { return sqlDataSourceRepository ?? (sqlDataSourceRepository = new SqlDataSourceRepository(UnitOfWork, LoggerFactory)); } }

        IApplicationRoleRepository roleRepository;
        public IApplicationRoleRepository RoleRepository { get { return roleRepository ?? (roleRepository = new ApplicationRoleRepository(UnitOfWork, LoggerFactory)); } }

        IUserRoleRepository user_roleRepository;
        public IUserRoleRepository UserRoleRepository { get { return user_roleRepository ?? (user_roleRepository = new UserRoleRepository(UnitOfWork, LoggerFactory)); } }

        IApplicationRolePermissionRepository rolePermissionRepository;
        public IApplicationRolePermissionRepository ApplicationRolePermissionRepository { get { return rolePermissionRepository ?? (rolePermissionRepository = new ApplicationRolePermissionRepository(UnitOfWork, LoggerFactory)); } }

        IOrganizationRepository? organizationRepository;
        public IOrganizationRepository OrganizationRepository { get { return organizationRepository ?? (organizationRepository = new OrganizationRepository(UnitOfWork, LoggerFactory)); } }

        IAuditLogRepository? auditLogRepository;
        public IAuditLogRepository AuditLogRepository { get { return auditLogRepository ??= new AuditLogRepository(UnitOfWork, LoggerFactory); } }

        IPerformanceLogRepository? performanceLogRepository;
        public IPerformanceLogRepository PerformanceLogRepository { get { return performanceLogRepository ??= new PerformanceLogRepository(UnitOfWork, LoggerFactory); } }

        private LogPerformance _logPerformance;
        public LogPerformance LogPerformance
        {
            get
            {
                if (_logPerformance == null)
                    _logPerformance = new LogPerformance();
                return _logPerformance;
            }
        }
    }
}
