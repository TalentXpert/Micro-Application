
namespace BaseLibrary
{
    public class ApplicationSettingBase 
    {
        public static string Environment { get; set; }
        public static string DatabaseConnectionString { get; set; }
        public static bool IsDatabaseExist { get; set; }
    }

}
