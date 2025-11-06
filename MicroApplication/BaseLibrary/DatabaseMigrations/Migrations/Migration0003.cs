using System.Reflection;

namespace BaseLibrary.DatabaseMigrations.Migrations
{
    public class Migration0003 : MigrationBase
    {
        public Migration0003(ISqlQueryExecutor sqlCommandExecutor, IBaseDatabase unitOfWork, Assembly assembly) : base(sqlCommandExecutor, unitOfWork, assembly)
        {
            
        }

        public override void Execute()
        {
            AddColumn("AppControl", "OptionFormId", DBTypes.Uniqueidentifier, true);
        }
    }
}
