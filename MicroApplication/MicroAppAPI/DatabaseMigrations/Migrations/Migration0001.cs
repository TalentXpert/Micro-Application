using BaseLibrary.Database;
using BaseLibrary.DatabaseMigrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MicroAppAPI.DatabaseMigrations.Migrations
{
    public class Migration0001 : MigrationBase
    {
        public Migration0001(ISqlQueryExecutor sqlCommandExecutor, Assembly assembly) : base(sqlCommandExecutor, assembly)
        {
        }

        public override void Execute()
        {
            CreateTable("AssetTable.txt");
            
            
            //CreateTable("ApplicationUserTable.txt");
            //CreateTable("ComponentSchemaTable.txt");
            
            //CreateTable("ApplicationRolePermissionTable.txt");


            //CreateTable("AppPageTable.txt");
            //CreateTable("BenutzerRolleTable.txt");
            //CreateTable("KostenstelleTable.txt");
        }
    }
}
