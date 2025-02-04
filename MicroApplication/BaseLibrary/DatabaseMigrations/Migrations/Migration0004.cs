using System.Reflection;

namespace BaseLibrary.DatabaseMigrations.Migrations
{
    public class Migration0004 : MigrationBase
    {
        public Migration0004(ISqlQueryExecutor sqlCommandExecutor, Assembly assembly) : base(sqlCommandExecutor, assembly)
        {
        }
        public override void Execute()
        {
            AddColumn("ApplicationUser", "Role", "nvarchar(max)", true);
            AddColumn("ApplicationUser", "PasswordResetCode", "nvarchar(6)", true);
            AddColumn("ApplicationUser", "PasswordResetCodeTimeStamp", "datetime", true);
            AddColumn("ApplicationUser", "DefaultStudyId", "uniqueidentifier", true);   
        }
    }
}
