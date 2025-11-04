using BaseLibrary.DTOs;
using System.Reflection;

namespace BaseLibrary.DatabaseMigrations
{
    public class DatabaseMigrator
    {
        private static void MigrateDatabaseSchema(int lastExecutedScriptSerialNumber, DatabaseOption database, Assembly assembly, ISqlQueryExecutor sqlCommandExecutor)
        {
            sqlCommandExecutor.StartTransaction();
            int migrationNumber = 0;
            try
            {
                var types = assembly.GetTypes().Where(x => x.BaseType == typeof(MigrationBase));
                types = types.OrderBy(t => t.Name);

                foreach (var type in types)
                {
                    if (type.Name == "MigrationBase") continue;

                    migrationNumber = Convert.ToInt32(type.Name.Substring(9));
                    if (migrationNumber <= lastExecutedScriptSerialNumber)
                        continue;

                    MigrationBase? migration = Activator.CreateInstance(type, sqlCommandExecutor, assembly) as MigrationBase;
                    if (migration != null)
                        migration.Execute();
                }

                UpdateDatabaseMigrationInformation(migrationNumber, assembly, database, sqlCommandExecutor);
                sqlCommandExecutor.CommitTransaction();
            }
            catch
            {
                sqlCommandExecutor.RollBackTransaction();
                throw;
            }
        }


        private static void UpdateDatabaseMigrationInformation(int maxExecutedScriptSerialNumber, Assembly assembly, DatabaseOption database, ISqlQueryExecutor sqlCommandExecutor)
        {
            var migration = new MigrationBase(sqlCommandExecutor, assembly);
            string query;
            if (migration.HasTable("DatabaseMigration"))
            {
                var id = sqlCommandExecutor.ExecuteScalar($"Select Id from DatabaseMigration WHERE DatabaseName = '{database.Value}'");
                if (id == 0)
                {
                    var maxId = sqlCommandExecutor.ExecuteScalar("Select MAx(Id) from DatabaseMigration");
                    maxId += 1;
                    query = $"INSERT INTO [DatabaseMigration] ([Id],[DatabaseName],[ScriptSerialNumber]) VALUES ({maxId},'{database.Value}',{maxExecutedScriptSerialNumber})";
                    sqlCommandExecutor.ExecuteQuery(query);
                }
                else
                {
                    var currentMigrationId = sqlCommandExecutor.ExecuteScalar($@"SELECT ScriptSerialNumber FROM DatabaseMigration WHERE DatabaseName = '{database.Value}'");
                    int currentVersion = currentMigrationId == 0 ? 1 : Convert.ToInt32(currentMigrationId);

                    if (maxExecutedScriptSerialNumber > currentVersion)
                    {
                        query = $"Update DatabaseMigration set [ScriptSerialNumber]={maxExecutedScriptSerialNumber} WHERE DatabaseName = '{database.Value}'";
                        sqlCommandExecutor.ExecuteQuery(query);
                    }
                }
            }
        }

        private static int GetLastExecutedScriptSerialNumberOrZero(ISqlQueryExecutor sqlCommandExecutor, DatabaseOption database)
        {
            try
            {
                var query = $"SELECT ScriptSerialNumber FROM DatabaseMigration WHERE DatabaseName = '{database.Value}'";
                int lastExecutedScriptSerialNumber = sqlCommandExecutor.ExecuteScalar(query);
                return lastExecutedScriptSerialNumber;
            }
            catch
            {
                return 0;
            }
        }

        public static void UpgradeDatabase(ISqlQueryExecutor sqlCommandExecutor, DatabaseOption database, Assembly assembly)
        {
            sqlCommandExecutor.GaurdForNonExistingDatabase();
            UpgradeMicroApplicationDatabase(sqlCommandExecutor);
            UpgradeApplicationDatabase(sqlCommandExecutor, database, assembly);
        }

        private static void UpgradeApplicationDatabase(ISqlQueryExecutor sqlCommandExecutor, DatabaseOption database, Assembly assembly)
        {
            try
            {
                int lastExecutedScriptSerialNumber = GetLastExecutedScriptSerialNumberOrZero(sqlCommandExecutor, database);
                MigrateDatabaseSchema(lastExecutedScriptSerialNumber, database, assembly, sqlCommandExecutor);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw new ValidationException("Failed to upgrade application database. Please contact your administrator." + message);
            }
        }
        private static void UpgradeMicroApplicationDatabase(ISqlQueryExecutor sqlCommandExecutor)
        {
            try
            {
                var database = DatabaseOption.MicroApplicationDatabase;
                int lastExecutedScriptSerialNumber = GetLastExecutedScriptSerialNumberOrZero(sqlCommandExecutor, database);
                var assembly = typeof(DatabaseMigrator).Assembly;
                MigrateDatabaseSchema(lastExecutedScriptSerialNumber, database, assembly, sqlCommandExecutor);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw new ValidationException("Failed to upgrade micro application database. Please contact your administrator." + message);
            }
        }
    }
}
