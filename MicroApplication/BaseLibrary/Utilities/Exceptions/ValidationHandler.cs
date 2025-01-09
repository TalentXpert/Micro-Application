namespace BaseLibrary.Utilities.Exceptions
{
    public class ValidationHandler
    {
        public ValidationHandler(string userLanguage)
        {
            UserLanguage = userLanguage;
        }

        public string UserLanguage { get; }

        public string GetCustomValidationExceptionMessage(Exception exception)
        {
            var message = exception.Message;
            //var customValidationException = exception as CustomValidationException;
            //if (customValidationException != null)
            //{
            //    message = Messages.GetMessage(UserLanguage, customValidationException.Group, customValidationException.Code);
            //    if (customValidationException.Args != null)
            //        message = Messages.FormatMessage(message, customValidationException.Args);
            //}
            return message;
        }
    }

}
