using System.Reflection;

namespace BaseLibrary.DatabaseMigrations.Migrations
{
    public class Migration0002 : MigrationBase
    {
        public Migration0002(ISqlQueryExecutor sqlCommandExecutor, Assembly assembly) : base(sqlCommandExecutor, assembly)
        {
        }

        public override void Execute()
        {
            AddColumn("AppPage", "EditPermission", "nvarchar(64)", true);
            AddColumn("AppPage", "DeletePermission", "nvarchar(64)", true);
            AddColumn("AppPage", "ViewPermission", "nvarchar(64)", true);
            ExecuteQuery("Update AppPage Set EditPermission='',DeletePermission='',ViewPermission='' where EditPermission is null");
            AddColumn("AppPage", "EditPermission", "nvarchar(64)", false);
            AddColumn("AppPage", "DeletePermission", "nvarchar(64)", false);
            AddColumn("AppPage", "ViewPermission", "nvarchar(64)", false);

            AddColumn("AppForm", "EditPermission", "nvarchar(64)", true);
            AddColumn("AppForm", "DeletePermission", "nvarchar(64)", true);
            AddColumn("AppForm", "ViewPermission", "nvarchar(64)", true);
            ExecuteQuery("Update AppForm Set EditPermission='',DeletePermission='',ViewPermission='' where EditPermission is null");
            AddColumn("AppForm", "EditPermission", "nvarchar(64)", false);
            AddColumn("AppForm", "DeletePermission", "nvarchar(64)", false);
            AddColumn("AppForm", "ViewPermission", "nvarchar(64)", false);
        }
    }
}
