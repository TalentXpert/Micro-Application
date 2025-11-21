using BaseLibrary.Repositories;

namespace BaseLibrary.Services
{
    public interface IBaseLibraryServiceFactory
    {
        ILoggerFactory LoggerFactory { get; }
        IBaseDatabase UnitOfWork { get; }
        IBaseLibraryRepositoryFactory RF { get; }
        IAppControlService AppControlService { get; }
        IAppFormService AppFormService { get; }
        IAppFormControlService AppFormControlService { get; }
        IUserService UserService { get; }
        ISecurityService SecurityService { get; }
        IUserSettingService UserSettingService { get; }
        IFormDataStoreService PageDataStoreService { get; }
        IApplicationSettingService ApplicationSettingService { get; }
        ISeederService SeederService { get; }
        IAppPageService AppPageService { get; }
        IComponentSchemaService ComponentSchemaService { get; }
        ISqlDataSourceService SqlDataSourceService { get; }
        UIControlBaseFactory ApplicationControlBaseFactory { get; set; }
        ILoginService LoginService { get; }
        IUserRoleService UserRoleService { get; }
        IRoleService RoleService { get; }
        IRolePermissionService RolePermissionService { get; }
        IMicroAppContract MicroAppContract { get; set; }
        IChartService ChartService { get; }
        IFormBuilderService FormBuilderService { get; }
        IFormConfigurationService FormConfigurationService { get; }
        IOrganizationService OrganizationService { get; }
        ApplicationUser? LoggedInUser { get; set; }
        IAuditEventBaseService AuditEventBaseService { get; }
    }
    public class BaseLibraryServiceFactory : IBaseLibraryServiceFactory
    {
        public IBaseDatabase UnitOfWork { get; }
        public IBaseLibraryRepositoryFactory RF { get; }
        public ILoggerFactory LoggerFactory { get; }
        
        public BaseLibraryServiceFactory(IBaseDatabase baseDatabase, ILoggerFactory loggerFactory)
        {
            UnitOfWork = baseDatabase;
            RF = new BaseLibraryRepositoryFactory(UnitOfWork, loggerFactory);
            LoggerFactory = loggerFactory;
        }

        IAppControlService? appControlService;
        public IAppControlService AppControlService { get { return appControlService ?? (appControlService = new AppControlService(this, LoggerFactory)); } }

        IAppFormService? appFormService;
        public IAppFormService AppFormService { get { return appFormService ?? (appFormService = new AppFormService(this, LoggerFactory)); } }

        IAppFormControlService? appFormControlService;
        public IAppFormControlService AppFormControlService { get { return appFormControlService ?? (appFormControlService = new AppFormControlService(this, LoggerFactory)); } }

        IUserService? userService;
        public IUserService UserService { get { return userService ?? (userService = new UserService(this, LoggerFactory)); } }

        ISecurityService? securityService;
        public ISecurityService SecurityService { get { return securityService ?? (securityService = new SecurityService(this, LoggerFactory)); } }

        IUserSettingService? userSettingService;
        public IUserSettingService UserSettingService { get { return userSettingService ?? (userSettingService = new UserSettingService(this, LoggerFactory)); } }

        IFormDataStoreService? pageDataStoreService;
        public IFormDataStoreService PageDataStoreService { get { return pageDataStoreService ?? (pageDataStoreService = new FormDataStoreService(this, LoggerFactory)); } }

        IApplicationSettingService? applicationSettingService;
        public IApplicationSettingService ApplicationSettingService { get { return applicationSettingService ?? (applicationSettingService = new ApplicationSettingService(this, LoggerFactory)); } }

        ISeederService? seederService;
        public ISeederService SeederService { get { return seederService ?? (seederService = new SeederService(this, LoggerFactory)); } }

        IAppPageService? appPageService;
        public IAppPageService AppPageService { get { return appPageService ?? (appPageService = new AppPageService(this, LoggerFactory)); } }
        
        IComponentSchemaService? componentSchemaService;
        public IComponentSchemaService ComponentSchemaService { get { return componentSchemaService ?? (componentSchemaService = new ComponentSchemaService(this, LoggerFactory)); } }

        ISqlDataSourceService? sqlDataSourceService;
        public ISqlDataSourceService SqlDataSourceService { get { return sqlDataSourceService ?? (sqlDataSourceService = new SqlDataSourceService(this, LoggerFactory)); } }

        public UIControlBaseFactory ApplicationControlBaseFactory { get; set; }
        public IMicroAppContract MicroAppContract { get; set; }
        ILoginService? loginService;
        public ILoginService LoginService { get { return loginService ?? (loginService = new LoginService(this, LoggerFactory)); } }

        IRoleService? roleService;
        public IRoleService RoleService { get { return roleService ?? (roleService = new RoleService(this, LoggerFactory)); } }



        IRolePermissionService? rolePermissionService;
        public IRolePermissionService RolePermissionService { get { return rolePermissionService ?? (rolePermissionService = new RolePermissionService(this, LoggerFactory)); } }


        IUserRoleService? userRoleService;

        public IUserRoleService UserRoleService { get { return userRoleService ?? (userRoleService = new UserRoleService(this, LoggerFactory)); } }


        IChartService? chartService;
        public IChartService ChartService { get { return chartService ?? (chartService = new ChartService(this, LoggerFactory)); } }

        IFormBuilderService ? formBuilderService;
        public IFormBuilderService FormBuilderService { get { return formBuilderService ?? (formBuilderService = new FormBuilderService(this, LoggerFactory)); } }

        IFormConfigurationService? formConfigurationService;
        public IFormConfigurationService FormConfigurationService { get  { return formConfigurationService ?? (formConfigurationService = new FormConfigurationService(this, LoggerFactory));  } }

        public IOrganizationService? organizationService;
        public IOrganizationService OrganizationService { get { return organizationService ?? (organizationService = new OrganizationService(this, LoggerFactory)); } }

        public IAuditEventBaseService? auditEventBaseService;
        public IAuditEventBaseService AuditEventBaseService { get { return auditEventBaseService ??= new AuditEventBaseService(this, LoggerFactory.CreateLogger<AuditEventBaseService>()); } }

        public ApplicationUser? LoggedInUser { get; set; }
    }
}
