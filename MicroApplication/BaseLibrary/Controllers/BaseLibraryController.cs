using BaseLibrary.Configurations;

namespace BaseLibrary.Controllers
{
    public class BaseLibraryController : Controller
    {
        protected ILogger Logger { get; private set; }
        protected IUserService UserService { get; }
        protected ISecurityService SecurityService { get; }
        public IBaseLibraryServiceFactory BaseLibraryServiceFactory { get; }
        protected IBaseLibraryServiceFactory BSF { get; }
        public BaseLibraryController(IBaseLibraryServiceFactory baseLibraryServiceFactory, ILogger logger)
        {
            BaseLibraryServiceFactory = baseLibraryServiceFactory;
            BSF = baseLibraryServiceFactory;
            Logger = logger;
            UserService = baseLibraryServiceFactory.UserService;
            SecurityService = baseLibraryServiceFactory.SecurityService;
            BaseLibraryServiceFactory.LoggedInUser = GetSafeCurrentUser();
        }

        protected void CommitTransaction()
        {
            BaseLibraryServiceFactory.UnitOfWork.Commit();
        }
        protected void RollbackTransaction()
        {
            BaseLibraryServiceFactory.UnitOfWork.RollbackChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Guid GetCurrentUserId()
        {
            var userClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var issueTime = User?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Iat);
            if (issueTime is not null)
            {
                TokenIssueTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(issueTime.Value)).DateTime;
            }
            if (userClaim != null)
                return Guid.Parse(userClaim.Value);
            return Guid.Empty;
        }
        private DateTime? TokenIssueTime { get; set; }

        [NonAction]
        public Guid GetSafeCurrentUserId()
        {
            try
            {
                return GetCurrentUserId();
            }
            catch
            {
                return Guid.Empty;
            }
        }


        protected ApplicationUser? GetSafeCurrentUser()
        {
            try
            {
                return LoggedInUser;
            }
            catch
            {
                return null;
            }
        }

        private Guid _signInUserId;

        private Guid SignInUserId
        {
            get
            {
                if (C.IsNullOrEmpty(_signInUserId))
                    _signInUserId = GetCurrentUserId();
                return _signInUserId;
            }
            set { _signInUserId = value; }
        }

        ApplicationUser? _currentUser;

        protected ApplicationUser LoggedInUser
        {
            get
            {
                if (C.IsNull(_currentUser))
                {
                    if(SignInUserId == Guid.Empty)
                        throw new ValidationException("Login session has expired. Please login again.");
                    _currentUser = UserService.GetUser(SignInUserId);
                    if (_currentUser is not null && _currentUser.IsBlocked)
                        throw new ValidationException($"Your account has been disabled. Please contact support team for more information. Message from disabler - {_currentUser.BlockReason}");
                }

                if (_currentUser is null)
                    throw new ValidationException("Login session has expired. Please login again.");

                var releaseDate = ApplicationConstants.ReleaseDate;
                if (TokenIssueTime.HasValue && TokenIssueTime < releaseDate)
                    throw new ValidationException("Login session has expired. Please login again.");
               
                return _currentUser;
            }
        }
        protected Guid LoggedInUserOrganizationId
        {
            get
            {
                return C.GetLoggedInUserOrganization(LoggedInUser);
            }
        }
        protected void GaurdForDisableUser()
        {
            if (C.IsNull(LoggedInUser))
                throw new ValidationException("Login session has expired. Please login again.");
            if (LoggedInUser.IsBlocked)
                throw new ValidationException($"Your account has been blocked. Please contact support team for more information. Message from disabler - {_currentUser.BlockReason}");
        }

        protected void HasActiveUserSession()
        {
            var loggedInUser = GetSafeCurrentUser();
            if (loggedInUser is null)
                throw new ValidationException(ValidationMessage.Relogin);
        }

        protected static JsonSerializerSettings DefaultJsonSettings
        {
            get
            {
                return new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                };
            }
        }

        protected static void GaurdForNullOrEmpty(Guid? input, string field)
        {
            if (input.IsNullOrEmpty()) throw new ValidationException($"{field} can not be empty or null.");
        }

        #region File Handling

        protected IActionResult Download(string filePath, string fileName)
        {
            var memory = new MemoryStream();
            using (var stream = new FileStream(@filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            fileName = X.Encoder.FileNameEncoder.GetSafeDownloadFileName(fileName);
            Response.Headers.Add("X-FileName", fileName);
            Response.Headers.Add("Access-Control-Expose-Headers", "Access-Control-Expose-Headers:X-FileName");
            return File(memory, GetContentType(filePath), fileName);
        }

        protected IActionResult DownloadCSV(MemoryStream memory, string fileName)
        {
            memory.Position = 0;
            fileName = X.Encoder.FileNameEncoder.GetSafeDownloadFileName(fileName);
            Response.Headers.Add("X-FileName", fileName);
            Response.Headers.Add("Access-Control-Expose-Headers", "Access-Control-Expose-Headers:X-FileName");
            var file = File(memory, GetContentType(fileName), fileName);
            return file;
        }

        protected static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            if (types.ContainsKey(ext)) return types[ext];
            return "application/octet-stream";
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/msword"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".pptx","application/vnd.ms-powerpoint" },
                {".zip","application/zip" }
            };
        }
        #endregion

        #region Permissions
        protected void IsOperationAllowed(string code, string operation)
        {
            if (C.IsNullOrEmpty(code)) throw new ValidationException("Operation code can not be null");
            if (C.IsNullOrEmpty(operation)) throw new ValidationException("Operation can not be null");
            if (C.IsNull(LoggedInUser)) throw new AuthenticationException($"To perform this operation {operation} you need to login in to system.");
            if (HasOperationPermission(code))
                return;
            throw new ValidationException($"You do not have permission to perform this operation {operation}.");
        }
        private bool HasOperationPermission(string code)
        {
            var permission = BSF.MicroAppContract.GetApplicationPermission().GetPermissions().FirstOrDefault(x => x.Code == code);
            if (BSF.UserRoleService.IsOperationAllowed(LoggedInUser, permission))
                return true;
            return false;
        }
        //protected void IsInRole(ApplicationRole role, string operation)
        //{
        //    if (LoggedInUser == null)
        //        throw new ValidationException($"Login is required to perform this {operation}.");
        //    if (LoggedInUser.OrganizationId.HasValue == false)
        //        throw new ValidationException($"Organization is required to perform this {operation}.");
        //    if (BSF.UserRoleService.IsInRole(role, LoggedInUser))
        //        return;
        //    throw new ValidationException($"You must have {role.Name} to perform this operation {operation}.");
        //}
        protected void IsInOrganizationAdminRole(string operation)
        {
            if (LoggedInUser.IsOrgAdmin)
                return;
            IsOperationAllowed(BasePermission.OrganizationConfiguration.Code, BasePermission.OrganizationConfiguration.Name);
        }
        protected void IsInWebsiteAdminRole(string operation)
        {
            if (BSF.UserRoleService.IsInWebsiteAdminRole(LoggedInUser))
                return;
            throw new ValidationException($"You must have website admin role to perform this operation {operation}.");
        }

        #endregion

        protected IActionResult GeneratePdfInvoice(byte[] bytes)
        {
            Response.Headers.Add("X-FileName", "invoice.pdf");
            Response.Headers.Add("Access-Control-Expose-Headers", "Access-Control-Expose-Headers:X-FileName");
            return File(bytes, GetContentType("invoice.pdf"), "invoice.pdf");
        }


        protected static DateTime GetLastCallTimeInUtc(long lastCallTime, int pastDays = 1)
        {
            if (lastCallTime == 0)
                return DateTime.UtcNow.AddDays(-pastDays);
            return new DateTime(lastCallTime, DateTimeKind.Utc);
        }


        #region Argument Validation
        protected static void GaurdForNullOrEmptyGuid(Guid? input, string argName)
        {
            if (input.IsNullOrEmpty())
                throw new ValidationException($"Provided value for {argName} is null or empty. {argName} can not be null or empty.");
        }

        protected static void GaurdForNullOrZeroInt(int? input, string argName)
        {
            if (!input.HasValue)
                throw new ValidationException($"Provided value for {argName} is null or zero. {argName} can not be null or zero.");

            if (input.Value == 0)
                throw new ValidationException($"Provided value for {argName} is null or zero. {argName} can not be null or zero.");
        }

        protected static void GaurdForNullEmptyString(string input, string argName)
        {
            if (C.IsNullOrEmpty(input))
                throw new ValidationException($"Provided value for {argName} is null or empty. {argName} can not be null or empty.");
            if (input == "undefined")
                throw new ValidationException($"Provided value for {argName} isundefined. {argName} can not be undefined.");
        }

        protected static void GaurdForNullArgument(object input, string argName)
        {
            if (C.IsNull(input))
                throw new ValidationException($"Provided value for {argName} is null. {argName} can not be null.");
        }
        protected static void GaurdForNullInputModel(object model)
        {
            if (model is null)
                throw new ValidationException("Input model is null. Please verify model binding.");
        }

        protected static void GaurdForUploadFile(IFormFile file, string argName, string extension, int fileSize = 5)
        {
            if (C.HasNoContents(file))
                throw new ValidationException($"No file uploaded to {argName}. Please upload file to process.");

            var extensions = extension.Split(",");
            var fileExtension = Path.GetExtension(file.FileName);
            if (!string.IsNullOrWhiteSpace(file.FileName) && !extensions.Contains(fileExtension))
                throw new ValidationException($"Only {extension} format files are supported.");

            if (file != null && file.Length >= (fileSize * 1024 * 1024))
                throw new ValidationException($"File more than {fileSize}mb size is not supported. Please split and upload a file of less than {fileSize}mb size.");
        }

        #endregion

        #region Exception 

        protected BadRequestObjectResult HandleException(Exception exception, string method, object dataVM = null)
        {
            if (C.IsNotNull(exception))
            {
                var userId = GetCurrentUserId();

                if (exception is ValidationException || exception is CustomException)
                {
                    X.Logger.LogError(exception, method, userId, dataVM, true);
                    var badRequest = GetBadRequest(exception);
                    if (C.IsNotNull(badRequest))
                        return badRequest;
                }

                if (C.IsNotNull(exception))
                    Logger.LogError(exception.ToString());


                X.Logger.LogError(exception.Message, exception.StackTrace, exception.ToString(), method, userId, dataVM, false);
            }

            return BadRequest(new ExceptionErrorVM(1, "Error Information", CommonErrorMessage));

        }

        protected string CommonErrorMessage => "An unhandled error has occurred while serving your request. Concern team has been informed to look into it. Please try again. If you face this error frequently please contact support.";

        private BadRequestObjectResult GetBadRequest(Exception exception)
        {
            try
            {
                if (exception is CustomException)
                    return BadRequest(((CustomException)exception).ToErrorVM());

                if (exception is ValidationException)
                {
                    if (exception.InnerException != null)
                        return BadRequest(new ExceptionErrorVM(4, "Validation Information", exception.Message)); // for check for overlap exception.
                    else
                        return BadRequest(new ExceptionErrorVM(3, "Validation Information", exception.Message));
                }
            }
            catch { }
            return null;
        }

        protected void LogException(Exception exception, string method, object data)
        {
            try
            {
                var userId = GetCurrentUserId();
                X.Logger?.LogError(exception, method, userId, data);
            }
            catch { }
        }

        #endregion


    }
}
