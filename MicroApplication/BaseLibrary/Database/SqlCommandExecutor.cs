
namespace BaseLibrary.Database
{
    public interface ISqlQueryExecutor
    {
        void ExecuteQuery(string query);
        int ExecuteScalar(string query);
        DataTable GetDataTable(string query);
        void GaurdForNonExistingDatabase();

        bool IsDatabaseTablesExits(string tableName);
        void StartTransaction();
        void CommitTransaction();
        void RollBackTransaction();
    }


    public interface ISqlCommandExecutor : ISqlQueryExecutor, IDisposable
    {
        void ExecuteStoredProcedure(string spName, List<SqlParameter> sqlParameters);
        DataTable GetDataTable(string sql, List<SqlParameter> sqlParameters);
    }

    public class SqlCommandExecutor : ISqlCommandExecutor
    {

        private readonly SqlConnection sqlConnection;
        private SqlTransaction sqlTransaction;

        public SqlCommandExecutor()
        {
            this.sqlConnection = new SqlConnection(ApplicationSettingBase.DatabaseConnectionString);
            
            this.sqlConnection.Open();
        }

        public void StartTransaction()
        {
            this.sqlTransaction = this.sqlConnection.BeginTransaction();
        }

        public void Dispose()
        {
            try
            {
                this.sqlConnection.Close();
            }
            catch { }
        }

        public int ExecuteScalar(string query)
        {
            SqlCommand command = GetSqlCommand(query);
            var result = command.ExecuteScalar();
            if (string.IsNullOrEmpty(result?.ToString()))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(result);
            }
        }

        public int ExecuteScalar(string query, List<SqlParameter> sqlParameters)
        {
            SqlCommand command = GetSqlCommand(query);
            command.Parameters.AddRange(sqlParameters.ToArray());
            var result = command.ExecuteScalar();
            if (string.IsNullOrEmpty(result.ToString()))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(result);
            }
        }

        public static Guid? GetGuidResult(string query)
        {
            DataTable dt;
            using (var database = new SqlCommandExecutor())
            {
                dt = database.GetDataTable(query);
            }
            if (dt.Rows.Count > 0) return (Guid)dt.Rows[0][0];
            return null;
        }

        public string CommaSeparatedValue(string query)
        {
            var result = GetListValue(query);
            return string.Join(",", result);
        }

        public List<string> GetListValue(string query)
        {
            var dataTable = GetDataTable(query);
            var result = new List<string>();
            foreach (DataRow dr in dataTable.Rows)
                result.Add(dr[0].ToString());
            return result;
        }

        public void ExecuteQuery(string query)
        {
            SqlCommand command = GetSqlCommand(query);
            command.ExecuteNonQuery();
        }

        public void ExecuteQuery(string query, List<SqlParameter> sqlParameters)
        {
            SqlCommand command = GetSqlCommand(query);
            command.Parameters.AddRange(sqlParameters.ToArray());
            command.ExecuteNonQuery();
        }

        public DataTable GetDataTable(string query, List<SqlParameter> sqlParameters)
        {
            SqlCommand command = GetSqlCommand(query);
            command.Parameters.AddRange(sqlParameters.ToArray());
            SqlDataAdapter da = new SqlDataAdapter(command);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable GetDataTable(string query)
        {
            SqlCommand command = GetSqlCommand(query);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }


        public void RollBackTransaction()
        {
            if (sqlTransaction == null) throw new ValidationException("You have not started transaction on this object so unable to commit");
            sqlTransaction.Rollback();
        }

        public void CommitTransaction()
        {
            if (sqlTransaction == null) throw new ValidationException("You have not started transaction on this object so unable to commit");
            sqlTransaction.Commit();
        }

        public void GaurdForNonExistingDatabase()
        {
            try
            {
                if (ApplicationSettingBase.IsDatabaseExist) return;
                var sqlConnection = new SqlConnection(ApplicationSettingBase.DatabaseConnectionString);
                sqlConnection.Open();
                ApplicationSettingBase.IsDatabaseExist = true;
                sqlConnection.Close();
            }
            catch
            {
                throw new ValidationException("Database doesn't exist. Please contact administrator.");
            }
        }

        public bool IsDatabaseTablesExits(string tableName)
        {
            try
            {
                var query = $"SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{tableName}'";
                var record = ExecuteScalar(query);
                return record > 0;
            }
            catch
            {
                return false;
            }
        }

        public void ExecuteStoredProcedure(string spName, List<SqlParameter> sqlParameters)
        {
            SqlCommand command = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.StoredProcedure,
                CommandText = spName
            };

            if (sqlTransaction != null)
                command.Transaction = sqlTransaction;

            command.Parameters.AddRange(sqlParameters.ToArray());
            command.ExecuteNonQuery();
        }

        private SqlCommand GetSqlCommand(string query)
        {
            var command = new SqlCommand(query, sqlConnection);
            if (sqlTransaction != null)
                command.Transaction = sqlTransaction;
            return command;
        }

        private void SetContext(Guid applicationId)
        {
            SqlCommand command = new SqlCommand("SETCONTEXTAPPLICATION", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            var param = new SqlParameter("@applicationId", SqlDbType.UniqueIdentifier);
            param.Value = applicationId;
            command.Parameters.Add(param);
            command.ExecuteNonQuery();
        }
    }
}
