
using System.Reflection;

namespace BaseLibrary.DatabaseMigrations.Migrations
{
    public class Migration0001 : MigrationBase
    {
        public Migration0001(ISqlQueryExecutor sqlCommandExecutor, IBaseDatabase unitOfWork, Assembly assembly) : base(sqlCommandExecutor, unitOfWork, assembly)
        {
        }

        public override void Execute()
        {
            CreateTable("DatabaseMigrationTable.txt");
            CreateTable("ApplicationUserTable.txt");
            CreateTable("ApplicationUserSettingTable.txt");
            CreateTable("ApplicationSettingTable.txt");

            //form and controls
            CreateTable("AppPageTable.txt");
            CreateTable("AppControlTable.txt");
            CreateTable("AppFormTable.txt");
            CreateTable("AppFormControlTable.txt");

            //role 
            CreateTable("ApplicationRoleTable.txt");
            CreateTable("ApplicationRolePermissionTable.txt");
            CreateTable("UserRoleTable.txt");
            
            CreateTable("SQLDataSourceTable.txt");
            CreateTable("ComponentSchemaTable.txt");
            CreateTable("FormDataStoreTable.txt");
            CreateTable("ExceptionLogTable.txt");
            //CreateTable("Table.txt");
        }
    }
}
