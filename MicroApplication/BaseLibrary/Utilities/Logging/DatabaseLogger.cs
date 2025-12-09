using System.Data.SqlClient;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;
using SqlParameter = Microsoft.Data.SqlClient.SqlParameter;
using SqlDataAdapter = Microsoft.Data.SqlClient.SqlDataAdapter;

namespace BaseLibrary.Utilities
{
    public interface IDatabaseLogger
    {
        void LogError(Exception exception, string method, Guid? userId, object? dataVM = null, bool isValidationException = false);
        void LogError(string message, string stackTrace, string detail, string method, Guid? userId, object? dataVM = null, bool isValidationException = false);
        List<DatabaseErrorVM> GetErrors(string methodInfo);
    }

    public class DatabaseLogger : IDisposable, IDatabaseLogger
    {
        public Microsoft.Data.SqlClient.SqlConnection SqlConnection;
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
                SqlConnection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
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

        public static DatabaseLogger GetDatabaseLogger(string connectionString, string logFileFolderPath = null)
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



        public void LogError(Exception exception, string method, Guid? userId, object? dataVM = null, bool isValidationException = false)
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
        public void LogError(string message, string? stackTrace, string detail, string method, Guid? userId, object? dataVM, bool isValidationException = false)
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
                        if (stackTrace == null)
                            stackTrace = string.Empty;
                        sqlCommand.Parameters.Add(new SqlParameter("@stackTrace", stackTrace));
                        if (userId.HasValue == false)
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
                    try
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
                    catch { }
                }
            }
        }

        private string InsertExceptionQuery()
        {
            string insertQuery = @"INSERT INTO [dbo].[ExceptionLog] ([Id],[Message],[StackTrace],[UserId],[DateTime],[ExceptionDump],[MethodInfo],[CreatedOn],[UpdatedOn],[MethodParams],[IsValidation])
            VALUES
           (@id,@message,@stackTrace, @userId,GETUTCDATE(),@exceptionDump,@methodInfo,GETUTCDATE(),GETUTCDATE(),@params,@isValidationException)";
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
                SqlDataAdapter da = new(query, SqlConnection);
                DataTable dt = new();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    string userId = "";
                    if (dr.IsNull("UserId") is false)
                        userId = dr["UserId"]?.ToString() ?? string.Empty;

                    var errrorVM = new DatabaseErrorVM(userId)
                    {
                        Id = (Guid)dr["Id"],
                        Message = dr["Message"]?.ToString() ?? string.Empty,
                        UserId = dr["UserId"]?.ToString() ?? string.Empty,
                        StackTrace = dr["StackTrace"]?.ToString() ?? string.Empty,
                        DateTime = (DateTime)dr["DateTime"],
                        ExceptionDump = dr["ExceptionDump"]?.ToString() ?? string.Empty,
                        MethodInfo = dr["MethodInfo"]?.ToString() ?? string.Empty,
                    };
                    result.Add(errrorVM);
                }
            }
            return result;
        }

    }

    public class DatabaseErrorVM(string userId)
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = userId;
        public string Message { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public string ExceptionDump { get; set; } = string.Empty;
        public String MethodInfo { get; set; } = string.Empty;
        public String MethodParams { get; set; } = string.Empty;
    }
}
