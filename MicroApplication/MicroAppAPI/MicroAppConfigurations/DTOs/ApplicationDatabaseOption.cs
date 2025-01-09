using BaseLibrary.DTOs;

namespace MicroAppAPI.MicroAppConfigurations.DTOs
{
    public class ApplicationDatabaseOption : DatabaseOption
    {
        protected ApplicationDatabaseOption(string value) : base(value)
        {
        }
        public static DatabaseOption ApplicationDatabase = new ApplicationDatabaseOption("ApplicationDatabase");
    }
}
