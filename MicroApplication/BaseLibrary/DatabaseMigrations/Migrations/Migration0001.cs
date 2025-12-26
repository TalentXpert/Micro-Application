
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
            CreateUserTables();
            CreateRoleTables();
            CreatePageAndForms();
            CreateOtherTables();
            //CreateTable("AuditLog.txt");
        }
        private void CreateUserTables()
        {
            CreateTable("ApplicationUserTable.txt");
            CreateTable("ApplicationUserSettingTable.txt");
            CreateTable("ApplicationSettingTable.txt");
        }

        private void CreateRoleTables()
        {
            CreateTable("ApplicationPermission.txt");
            CreateTable("ApplicationRoleTable.txt");
            CreateTable("ApplicationRolePermissionTable.txt");
            CreateTable("UserRoleTable.txt");
        }
        private void CreatePageAndForms()
        {
            CreateTable("AppPageTable.txt");
            CreateTable("AppControlTable.txt");
            CreateTable("AppFormTable.txt");
            CreateTable("AppFormControlTable.txt");
        }
        private void CreateOtherTables()
        {
            CreateTable("SQLDataSourceTable.txt");
            CreateTable("ComponentSchemaTable.txt");
            CreateTable("FormDataStoreTable.txt");
            CreateTable("ExceptionLogTable.txt");
            CreateTable("ApplicationMenu.txt");
            CreateTable("AuditLog.txt");
        }


    }
}
