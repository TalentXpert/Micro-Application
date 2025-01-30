

namespace BaseLibrary.Configurations
{
    public abstract class BasePermission
    {
        protected abstract List<Permission> GetApplicationPermissions();
        public List<Permission> GetPermissions()
        {
            var permissions = new List<Permission>();
            permissions.AddRange(GetBasePermissions());
            permissions.AddRange(GetApplicationPermissions());
            return permissions;
        }
        private List<Permission> GetBasePermissions()
        {
            return new List<Permission>
            {
                ManageUsers,FormConfiguration, ManageCharts,  ManageFormControls, ManageRoles, ManageRolePermissions, ManageUserRoles, ManageDashboard, OrganizationConfiguration, ViewReports, ViewDashboards
            };
        }
        
        public virtual List<Permission> GetOrganizationAdminPermissions()
        {
            return new List<Permission> { ManageOrganization, ManageUsers, FormConfiguration, ManageCharts, ManageFormControls, ManageRoles, ManageRolePermissions, ManageUserRoles , ManageDashboard, OrganizationConfiguration, ViewReports, ViewDashboards };
        }
        public virtual List<Permission> GetWebsiteAdminPermissions()
        {
            return new List<Permission>
            {
               SystemConfiguration,
               ManageLookUp,
               BuildForm,
               ManageOrganizationAdmin,
               ManageSystem,
               ManageFormControls
            };
        }

        #region Permissions
        //Website Admin Permissions
        public static Permission ManageSystem = new Permission("041AADCD-2A3A-4972-BA8D-F583B0238487", "System Configuration", "");
        public static Permission SystemConfiguration = new Permission("36C31FD2-8CED-C39A-5E0E-08DAC6C79A9A", "System Configuration", "");
        public static Permission ManageLookUp = new Permission("F4424EAC-A630-C4D7-8EB2-08DAC6C659F9", "Manage LookUp", "");
        public static Permission BuildForm = new Permission("022F72B9-2C15-C56E-E639-08DAC6C73C35", "Form Builder", "");
        public static Permission ManageOrganizationAdmin = new Permission("F314B393-CA50-4BF3-9FDB-995F5510D607", "Manage Organization Admin", "");
        
        //Organization Admin Permissions
        public static Permission ManageUsers = new Permission("D387B73D-45AC-C116-A741-08D909447A8A", "Manage Users", "");
        public static Permission FormConfiguration = new Permission("9EFDE8E5-C64E-CA80-9974-08DAC6C65BF1", "Form Configuration", "");
        public static Permission ManageCharts = new Permission("DF0B1DF0-06C7-C4FF-8F75-08DAC6C661D5", "Manage Charts", "");
        public static Permission ManageFormControls = new Permission("69D5D849-295E-C649-7057-08DAC6C679CD", "Manage Form Controls", "");
        public static Permission ManageRoles = new Permission("61E47F60-874A-C14B-874E-08DAC6C67BDD", "Manage Roles", "");
        public static Permission ManageRolePermissions = new Permission("6C969054-45D7-C6F3-B9C8-08DAC6C6A72C", "Manage Role Permissions", "");
        public static Permission ManageUserRoles = new Permission("2A3A73A0-759D-C300-36F5-08DAC6C6DE77", "Manage User Roles", "");
        public static Permission ManageDashboard = new Permission("15FABDD6-8166-C990-7D3F-08DAC6C77D3F", "Manage Dashboard", "");
        public static Permission OrganizationConfiguration = new Permission("BA07069F-3659-C1B3-66DD-08DAC6C7B633", "Organization Configuration", "");
        public static Permission ViewReports = new Permission("561E3398-CD38-C500-9370-08DAC6C7DEB8", "View Reports", "");
        public static Permission ViewDashboards = new Permission("2E578AA1-E3AE-CE79-8BEE-08DAC6C84E89", "View Dashboards", "");

        //[Guid("")]
        //No permission
        public static Permission None = new Permission("97C1597A-F431-483D-AC60-BAA9105BF73D", "None", "");
        public static Permission ManageOrganization = new Permission("8A1C034C-9374-4EBE-931E-39B655C70E4D", "Manage Organization", "");
        #endregion
    }
}