namespace BaseLibrary.Configurations
{
    public class PageGridHeader
    {
        public Guid FormId { get; set; }
        public Guid FormControlId { get; set; }
        public string HeaderText { get; set; }
        public List<SmartAction> Actions { get; protected set; }
        public PageGridHeader(Guid formId, Guid formControlId, string headerText)
        {
            FormId = formId;
            FormControlId = formControlId;
            HeaderText = headerText;
            Actions= new List<SmartAction>();
        }
    }

    public abstract class BasePage
    {
        /// <summary>
        /// Implement this method to return your application pages
        /// </summary>
        protected abstract List<AppPage> GetApplicationPages();

        /// <summary>
        /// Override this method if you want to change base pages or their sequence. You can return empty list and return all pages from GetApplicationPages method.
        /// </summary>
        protected virtual List<AppPage> GetBasePages()
        {
            return new List<AppPage>()
            {
                UserManagementPage,ApplicationControlPage,ManageOrganizationPage,ManageOrganizationAdminPage
            };
        }
        public List<AppPage> GetPages()
        {
            var pages = new List<AppPage>();
            pages.AddRange(GetApplicationPages());
            pages.AddRange(GetBasePages());
            return pages;
        }

        public const string UserManagementPageId = "1C2319A3-F291-C117-7AB0-08D9065B20B4";
        public static AppPage UserManagementPage = AppPage.Create(UserManagementPageId, "Users", BaseMenu.ConfigurationMenuId, 1, BasePermission.ManageUsers);

        public const string ApplicationControlPageId = "EC60607F-BB81-C5E2-3159-08D90869EAB7";
        public static AppPage ApplicationControlPage = AppPage.Create(ApplicationControlPageId, "Application Controls", BaseMenu.SystemConfigurationMenuId, 1, BasePermission.ManageFormControls);

        public const string ReportPageId = "4C8EB7CB-E8C6-C93A-C00A-08D90877CA3F";
        public static AppPage ReportPage = AppPage.Create(ReportPageId, "Report", BaseMenu.ReportMenuId, 1, BasePermission.ViewReports);

        public const string ManageOrganizationAdminPageId = "A5443C1E-C8BF-4D57-B527-C1949C97716F";
        public static AppPage ManageOrganizationAdminPage = AppPage.Create(ManageOrganizationAdminPageId, "Manage Organization Admin", BaseMenu.SystemConfigurationMenuId, 1, BasePermission.ManageSystem);

        public const string ManageOrganizationPageId = "0122D161-C76E-45CD-A5FE-74CD83F58223";
        public static AppPage ManageOrganizationPage = AppPage.Create(ManageOrganizationPageId, "Manage Organization", BaseMenu.SystemConfigurationMenuId, 1, BasePermission.ManageSystem);
    }
}

