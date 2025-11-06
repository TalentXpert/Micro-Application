using System.Reflection;
using System.Data;

namespace BaseLibrary.DatabaseMigrations
{
    public class DatabaseColumnDefinition
    {
        public string Name { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public bool IsNullable { get; set; }
        public string TableName { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string DefaultValue { get; set; } = string.Empty;

        public DatabaseColumnDefinition(DataRow dr)
        {
            if (dr != null)
            {
                if (dr.IsNull("COLUMN_NAME") == false)
                    Name = dr["COLUMN_NAME"].ToString();
                if (dr.IsNull("DATA_TYPE") is false)
                    DataType = dr["DATA_TYPE"].ToString();
                if (dr.IsNull("IS_NULLABLE") is false)
                    IsNullable = dr["IS_NULLABLE"].ToString() == "YES";
                if (dr.IsNull("TABLE_NAME") is false)
                    TableName = dr["TABLE_NAME"].ToString();
                if (dr.IsNull("TABLE_CATALOG") is false)
                    DatabaseName = dr["TABLE_CATALOG"].ToString();
                if (dr.IsNull("COLUMN_DEFAULT") is false)
                    DefaultValue = dr["COLUMN_DEFAULT"] == DBNull.Value ? null : dr["COLUMN_DEFAULT"].ToString();
            }
            else
            {
                throw new ArgumentNullException(nameof(dr));
            }
        }
    }

    public class MigrationBase
    {
        protected ISqlQueryExecutor SqlCommandExecutor { get; }
        public Assembly Assembly { get; }
        private DbScriptReader DbScriptReader { get; }
        public IBaseDatabase UnitOfWork { get; }
        public MigrationBase(ISqlQueryExecutor sqlCommandExecutor, IBaseDatabase unitOfWork , Assembly assembly)
        {
            SqlCommandExecutor = sqlCommandExecutor;
            Assembly = assembly;
            DbScriptReader = new DbScriptReader();
            UnitOfWork = unitOfWork;
        }

        public virtual void Execute() { }
        public DatabaseColumnDefinition GetDatabaseColumnDefinition(string table, string column)
        {
            var query = @$"SELECT COLUMN_NAME, DATA_TYPE, TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, COLUMN_DEFAULT, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{table}' AND COLUMN_NAME = '{column}'";
            var dt = SqlCommandExecutor.GetDataTable(query);
            return new DatabaseColumnDefinition(dt.Rows[0]);
        }
        public string GetColumnDataType(string table, string column)
        {
            return GetDatabaseColumnDefinition(table, column).DataType;
        }
        public bool IsColumnNullable(string table, string column)
        {
            return GetDatabaseColumnDefinition(table, column).IsNullable;
        }
        public bool IsColumnDataTypeAndNullableMatching(string table, string column, string dataType, bool isNullable)
        {
            var columnDefination = GetDatabaseColumnDefinition(table, column);
            if (columnDefination != null)
            {
                return columnDefination.DataType.Equals(dataType, StringComparison.InvariantCultureIgnoreCase) && columnDefination.IsNullable == isNullable;
            }
            return false;
        }

        protected void DropTable(string table)
        {
            if (!HasTable(table)) return;
            var query = $"DROP TABLE {table}";
            ExecuteQuery(query);
        }
        public bool HasTable(string table)
        {
            var query = $@"SELECT count(*) FROM sys.objects WHERE OBJECT_ID = OBJECT_ID(N'{table}') AND type = (N'U')";
            return SqlCommandExecutor.ExecuteScalar(query) > 0;
        }

        public void RenameTable(string oldName, string newName)
        {
            if (!HasTable(oldName)) return;
            var query = $"EXEC sp_rename 'dbo.{oldName}', '{newName}';";
            ExecuteQuery(query);
        }

        public void RenameColumn(string table, string oldName, string newName)
        {
            if (!HasColumn(table, oldName)) return;
            var query = $"EXEC sp_rename 'dbo.{table}.{oldName}', '{newName}', 'COLUMN';";
            ExecuteQuery(query);
        }

        public void DropIndex(string table, string index)
        {
            if (HasIndex(table, index))
            {
                var query = $"DROP INDEX {table}.{index}";
                ExecuteQuery(query);
            }
        }

        protected void DropFK(string table, string FKConstraintName)
        {
            if (!HasTable(table)) return;
            var query = $"IF (OBJECT_ID('dbo.{FKConstraintName}', 'F') IS NOT NULL) BEGIN ALTER TABLE {table} DROP CONSTRAINT {FKConstraintName} END";
            ExecuteQuery(query);
        }

        protected void AddFK(string childTable, string parentTable, string FKConstraintName, string childColumns, string parentColumns)
        {
            if (!HasTable(childTable)) return;
            if (!HasTable(parentTable)) return;
            var query = $@"IF (OBJECT_ID('dbo.{FKConstraintName}', 'F') IS NULL) BEGIN ALTER TABLE {childTable} ADD CONSTRAINT {FKConstraintName} FOREIGN KEY ({childColumns}) REFERENCES {parentTable} ({parentColumns}) END";
            ExecuteQuery(query);
        }

        public void DeleteColumn(string table, string column)
        {
            if (HasColumn(table, column)) 
            {
                var query = $@"ALTER TABLE {table} DROP COLUMN {column};";
                ExecuteQuery(query);
            }
        }

        protected void DropUniqueIndex(string table, string index)
        {
            if (HasIndex(table, index))
            {
                var query = $@"ALTER TABLE {table} DROP CONSTRAINT {index};";
                ExecuteQuery(query);
            }

        }

        protected void DropDefaultConstraint(string table, string ConstraintName)
        {
            DropConstraint(table, ConstraintName, "D");
        }

        private void DropConstraint(string table, string constraintName, string constraintType)
        {
            var query = $"IF (OBJECT_ID('dbo.{constraintName}', '{constraintType}') IS NOT NULL) BEGIN ALTER TABLE {table} DROP CONSTRAINT {constraintName} END";
            ExecuteQuery(query);
        }

        protected void AddColumn(string table, string column, string dataType, bool isNullAllowed)
        {
            if (!HasColumn(table, column))
            {
                var query = $@"ALTER TABLE {table} ADD {column} {dataType} ";
                if (isNullAllowed)
                    query += " NULL";
                else
                    query += " NOT NULL";
                ExecuteQuery(query);
            }
        }

        protected void CreateIntColumnWithDefaultValueZero(string table, string column)
        {
            AddColumn(table, column, "int", true);
            ExecuteQuery($"Update {table} Set {column}=0 where {column} is null");
            ChangeColumnDataType(table, column, "int", false);
        }

        protected void CreateBooleanColumnWithDefaultValue(string table, string column, bool defaultValue)
        {
            AddColumn(table, column, DBTypes.Bit, true);
            if (defaultValue)
                ExecuteQuery($"Update {table} Set {column}=1 where {column} is null");
            else
                ExecuteQuery($"Update {table} Set {column}=0 where {column} is null");
            ChangeColumnDataType(table, column, DBTypes.Bit, false);
        }
        protected void CreateGuidColumnWithDefaultValue(string table, string column, Guid defaultValue)
        {
            AddColumn(table, column, DBTypes.Uniqueidentifier, true);
            ExecuteQuery($"Update {table} Set {column}='{defaultValue}' where {column} is null");
            ChangeColumnDataType(table, column, DBTypes.Uniqueidentifier, false);
        }

        protected void CreateTable(string fileName)
        {
            var script = GetScript(fileName);
            ExecuteQuery(script);
        }

        protected void ChangeColumnDataType(string table, string column, string dataType, bool isNullAllowed)
        {
            if (HasColumn(table, column))
            {
                if (IsColumnDataTypeAndNullableMatching(table, column, dataType, isNullAllowed))
                    return;
                var query = $@"ALTER TABLE {table} ALTER COLUMN {column} {dataType} ";
                if (isNullAllowed)
                    query += " NULL";
                else
                    query += " NOT NULL";
                ExecuteQuery(query);
            }
        }

        protected bool HasColumn(string table, string column)
        {
            string query = $@"SELECT count(*) FROM sys.columns WHERE Name = N'{column}' AND Object_ID = Object_ID(N'{table}')";
            return SqlCommandExecutor.ExecuteScalar(query) > 0;
        }

        protected bool HasIndex(string table, string index)
        {
            string query = $@"SELECT count(*) FROM sys.indexes WHERE name = '{index}' AND object_id = OBJECT_ID('{table}')";
            return SqlCommandExecutor.ExecuteScalar(query) > 0;
        }

        protected void ExecuteQuery(string query)
        {
            SqlCommandExecutor.ExecuteQuery(query);
        }

        protected string GetScript(string fileName)
        {
            return DbScriptReader.GetScript(fileName, Assembly);
        }

        protected Guid? GetId(string query)
        {
            var dt = SqlCommandExecutor.GetDataTable(query);
            if (dt.Rows.Count > 0)
            {
                if (Guid.TryParse(dt.Rows[0][0].ToString(), out Guid id))
                    return id;
            }
            return null;
        }
    }
}
