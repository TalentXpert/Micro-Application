
namespace BaseLibrary
{
    public class ApplicationSettingBase 
    {
        public static string Environment { get; set; } = string.Empty;
        public static string DatabaseConnectionString { get; set; } = string.Empty;
        public static bool IsDatabaseExist { get; set; } 
        public static string DefaultTimeZone { get; set; } = string.Empty;

        public static bool IsEmailEnabled { get; set; } = false;
        public static string EmailHost { get; set; } = string.Empty;
        public static string EmailHostPort { get; set; } = string.Empty;
        public static bool EnableSsl { get; set; } = false;
        public static string FromEmailAddress { get; set; } = string.Empty;
        public static string FromEmailPassword { get; set; } = string.Empty;

        
    }

}
