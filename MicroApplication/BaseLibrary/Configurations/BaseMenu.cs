
using BaseLibrary.Controls.Menus;
using BaseLibrary.Domain;

namespace BaseLibrary.Configurations
{
    public abstract class BaseMenu : CleanCode
    {
        public static Guid SystemConfigurationMenuId = Guid.Parse("F9873BA4-FE58-C3A8-FFE7-08D90949354F");
        public static Guid ConfigurationMenuId = Guid.Parse("52173B32-B3A8-C227-EC5A-08D9089FBAB5");
        public static Guid ReportMenuId = Guid.Parse("6FD218A1-C307-C3D9-D941-08D90AD67EE2");

        //Top Menus
        public Menu SystemConfiguration = new Menu(SystemConfigurationMenuId, "Macro Configuration", "");
        public Menu Configuration = new Menu(ConfigurationMenuId, "Configuration", "");
        public Menu Report = new Menu(ReportMenuId, "Report", "");

        //Children Menus 
        public static Guid FormConfigurationMenuId = Guid.Parse("545A4CBB-6A62-C1C3-97E0-08D90A27551F");
        public static Guid FormBuilderMenuId = Guid.Parse("C9BCF025-B0C1-C81E-D6D2-08D90AD67EE2");
        public static Guid ChartBuilderMenuId = Guid.Parse("CF84521B-246B-C6ED-54B2-08D90BC55171");
        public static Guid DashboardBuilderMenuId = Guid.Parse("48EC0D29-41F0-CC3A-5505-08D90BC55171");

        public static Guid ManageConfigurationMenuId = Guid.Parse("30D669F3-D8DD-4448-B70E-CFF96010FE86");
        public static Guid PermissionMenuId = Guid.Parse("C8AFE5D7-1833-C61B-BBF9-08D90AD67EE2");
        public static Guid ManageRoleMenuId = Guid.Parse("52BFE4D8-86A8-C3F5-D7A8-08D90AD67EE2");
        public static Guid ManageRolePermissionMenuId = Guid.Parse("89A8306F-9D30-C831-D552-08D90AD67EE2");
        public static Guid NoneMenuId = Guid.Parse("66E7A586-195E-43BC-97C8-9EC83D21E35A");


        //[Guid("")]
        public Menu FormConfiguration = new Menu(FormConfigurationMenuId, "Form Configuration", $"appPage/fc");
        public Menu FormBuilder = new Menu(FormBuilderMenuId, "Form Builder", $"appPage/fb");
        public Menu ChartBuilder = new Menu(ChartBuilderMenuId, "Chart Builder", $"appPage/cb");
        public Menu DashboardBuilder = new Menu(DashboardBuilderMenuId, "Dashboard Builder", $"appPage/db");
        public Menu ManageConfiguration = new Menu(ManageConfigurationMenuId, "ManageConfiguration", $"mngconfig");
        public Menu Permission = new Menu(PermissionMenuId, "Permission", $"permission");
        public Menu ManageRole = new Menu(ManageRoleMenuId, "Role", $"role");
        public Menu ManageRolePermission = new Menu(ManageRolePermissionMenuId, "Manage Role and Permission", $"rp");
        public Menu None = new Menu(NoneMenuId, "None", $"");
        

        /// <summary>
        /// Implement this method returns top menus where website admin can add new dynamic page and forms to them
        /// </summary>
        public abstract List<Menu> GetFixedTopMenus();

        /// <summary>
        /// Override this method if to change menu structure
        /// </summary>
        protected abstract List<Menu> GetAllMenus(List<string> userPermissions);

        public List<Menu> GetMenus(List<AppPage> appPages, List<string> userPermissions)
        {
            var fixedMenus = GetFixedTopMenus();
            if (fixedMenus.Count == 0)
                throw new ValidationException("No top fixed menus found. Please add top menus.");

            var allMenus = GetAllMenus(userPermissions);

            appPages = appPages.OrderBy(f => f.Position).ToList();

            foreach (var appPage in appPages)
            {
                var topMenu = allMenus.FirstOrDefault(m => m.Id == appPage.MenuId);
                if (topMenu == null)
                    continue;
                if (topMenu.Id == appPage.Id || topMenu.Children.Any(c => c.Id == appPage.Id))
                    continue;
                if (HasPagePermission(appPage, userPermissions))
                    topMenu.Children.Add(new Menu(appPage.Id, appPage.Name, $"appPage/ap", false));
            }
            return allMenus;
        }

        private bool HasPagePermission(AppPage appPage, List<string> userPermissions)
        {
            if (HasPermission(userPermissions, appPage.ViewPermission))
                return true;
            if (appPage.ViewPermission == BasePermission.None.Name)
                return true;
            return false;
        }
    }
}
