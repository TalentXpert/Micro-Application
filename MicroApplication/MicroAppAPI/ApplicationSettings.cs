namespace MicroAppAPI
{
    public class ApplicationSettings : ApplicationSettingBase
    {
        public static AuthOptions? AuthOptions { get; set; }
        public static string? WebsiteAdminLoginId { get; set; }
        public static void SetApplicationSettingConfiguration(ApplicationSettingConfiguration configuration)
        {
            WebsiteAdminLoginId = configuration.WebsiteAdminLoginId;
            DatabaseConnectionString = configuration.DatabaseConnectionString;
        }
        public bool IsWebsiteAdmin(string websiteAdminLoginId)
        {
            if(WebsiteAdminLoginId == null) 
                return false;
            if(string.IsNullOrWhiteSpace(websiteAdminLoginId)) 
                return false;
            var admins = WebsiteAdminLoginId.Split(",", StringSplitOptions.RemoveEmptyEntries);
            return admins.ToList().Any(a=> a.Equals(websiteAdminLoginId,StringComparison.InvariantCultureIgnoreCase));
        }
    }

    public class ApplicationSettingConfiguration
    {
        public string WebsiteAdminLoginId { get; set; }
        public string DatabaseConnectionString { get; set; }
    }
}
