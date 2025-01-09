using System.Reflection;

namespace BaseLibrary.DatabaseMigrations.Migrations
{
    public class Migration0003 : MigrationBase
    {
        public Migration0003(ISqlQueryExecutor sqlCommandExecutor, Assembly assembly) : base(sqlCommandExecutor, assembly)
        {
            
        }

        public override void Execute()
        {
            AddColumn("AppControl", "OptionFormId", DBTypes.Uniqueidentifier, true);
        }
    }
}
