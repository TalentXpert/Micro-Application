using System.Data.SqlClient;
using System.Data;

namespace BaseLibrary.Utilities
{
    public interface IDatabaseLogger
    {
        void LogError(Exception exception, string method, Guid? userId, object dataVM = null, bool isValidationException = false);
        void LogError(string message, string stackTrace, string detail, string method, Guid? userId, object dataVM = null, bool isValidationException = false);
        List<DatabaseErrorVM> GetErrors(string methodInfo);
    }

    public class DatabaseLogger : IDisposable, IDatabaseLogger
    {
        public SqlConnection SqlConnection;
        public bool IsValidConnection = false;

        private static DatabaseLogger? _databaseLogger;
        private static string? LogFileFolderPath;
        private DatabaseLogger(string connectionString)
        {
            InitialiseConnection(connectionString);
        }

        private void InitialiseConnection(string connectionString)
        {
            if (SqlConnection == null)
            {
                SqlConnection = new SqlConnection(connectionString);
                try
                {
                    SqlConnection.Open();
                    IsValidConnection = true;
                }
                catch
                {
                    IsValidConnection = false;
                }
            }
        }

        public static DatabaseLogger GetDatabaseLogger(string connectionString,string logFileFolderPath = null)
        {
            LogFileFolderPath = logFileFolderPath;
            if (_databaseLogger == null)
                _databaseLogger = new DatabaseLogger(connectionString);
            return _databaseLogger;
        }

        public void Dispose()
        {
            try
            {
                if (IsValidConnection && SqlConnection.State == ConnectionState.Open)
                    SqlConnection.Close();
            }
            catch { }
        }



        public void LogError(Exception exception, string method, Guid? userId, object dataVM = null, bool isValidationException = false)
        {
            LogError(exception.Message, exception.StackTrace, exception.ToString(), method, userId, dataVM, isValidationException);
        }

        private object lockObject = new object();
        private string GetParams(object dataVM)
        {
            if (dataVM == null) return string.Empty;
            var dataJson = JsonConvert.SerializeObject(dataVM);
            if (string.IsNullOrWhiteSpace(dataJson)) return string.Empty;
            return dataJson;

        }
        public void LogError(string message, string stackTrace, string detail, string method, Guid? userId, object dataVM, bool isValidationException = false)
        {
            if (IsValidConnection)
            {
                try
                {
                    lock (lockObject)
                    {
                        SqlCommand sqlCommand = new SqlCommand(InsertExceptionQuery(), SqlConnection);
                        sqlCommand.Parameters.Add(new SqlParameter("@id", IdentityGenerator.NewSequentialGuid()));
                        sqlCommand.Parameters.Add(new SqlParameter("@message", message));
                        sqlCommand.Parameters.Add(new SqlParameter("@stackTrace", stackTrace));
                        if (userId.HasValue ==false)
                            sqlCommand.Parameters.Add(new SqlParameter("@userId", Guid.Empty));
                        else
                            sqlCommand.Parameters.Add(new SqlParameter("@userId", userId));
                        sqlCommand.Parameters.Add(new SqlParameter("@exceptionDump", detail));
                        if (method == null) method = string.Empty;
                        sqlCommand.Parameters.Add(new SqlParameter("@methodInfo", method));
                        sqlCommand.Parameters.Add(new SqlParameter("@params", GetParams(dataVM)));
                        sqlCommand.Parameters.Add(new SqlParameter("@isValidationException", isValidationException));
                        sqlCommand.ExecuteNonQuery();
                    }
                }
                catch
                {
                    if (userId.HasValue == false)
                        userId = Guid.Empty;
                    if (string.IsNullOrWhiteSpace(LogFileFolderPath) == false)
                    {
                        var filePath = Path.Combine(LogFileFolderPath, "ApplicationError", "ErrorLog.txt");
                        var error = $"user-{userId}, method-{method}, message-{message}, stack strace-{stackTrace}, details-{detail}";
                        File.AppendAllText(filePath, error);
                    }
                }
            }
        }

        private string InsertExceptionQuery()
        {
            string insertQuery = @"INSERT INTO [dbo].[ExceptionLog] ([Id],[Message],[StackTrace],[UserId],[DateTime],[ExceptionDump],[MethodInfo],[CreatedOn],[UpdatedOn],[MethodParams],[IsValidation])
            VALUES
           (@id,@message,@stackTrace, @userId,getdate(),@exceptionDump,@methodInfo,getdate(),getdate(),@params,@isValidationException)";
            return insertQuery;
        }

        public List<DatabaseErrorVM> GetErrors(string methodInfo)
        {
            string query = $"select top 100 [Id],[Message],[StackTrace],[UserId],[DateTime],[ExceptionDump],[MethodInfo],[MethodParams] from [ExceptionLog]";
            if (!string.IsNullOrWhiteSpace(methodInfo))
            {
                query += "where MethodInfo='{methodInfo}' order by[DateTime] desc";
            }

            List<DatabaseErrorVM> result = GetErrorData(query);
            return result;
        }

        private List<DatabaseErrorVM> GetErrorData(string query)
        {
            var result = new List<DatabaseErrorVM>();

            lock (lockObject)
            {
                SqlDataAdapter da = new SqlDataAdapter(query, SqlConnection);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    var errrorVM = new DatabaseErrorVM((string)dr["UserId"])
                    {
                        Id = (Guid)dr["Id"],
                        Message = dr["Message"].ToString(),
                        UserId = (string)dr["UserId"],
                        StackTrace = dr["StackTrace"].ToString(),
                        DateTime = (DateTime)dr["DateTime"],
                        ExceptionDump = dr["ExceptionDump"].ToString(),
                        MethodInfo = dr["MethodInfo"].ToString(),
                    };
                    result.Add(errrorVM);
                }
            }
            return result;
        }

    }





    public class DatabaseErrorVM
    {
        public DatabaseErrorVM(string userId)
        {
            UserId = userId;
        }
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime DateTime { get; set; }
        public string ExceptionDump { get; set; }
        public String MethodInfo { get; set; }
        public String MethodParams { get; set; }
    }
}
