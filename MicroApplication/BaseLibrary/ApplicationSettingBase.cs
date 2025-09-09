
namespace BaseLibrary
{
    public class ApplicationSettingBase 
    {
        public static string Environment { get; set; } = string.Empty;
        public static string DatabaseConnectionString { get; set; } = string.Empty;
        public static bool IsDatabaseExist { get; set; } 
        public static string DefaultTimeZone { get; set; } = string.Empty;
    }

}
