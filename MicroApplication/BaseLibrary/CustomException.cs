namespace BaseLibrary
{
    public class CustomException : Exception
    {
        public int Code { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        private CustomException(int code, string title, string body)
        {
            Code = code;
            Title = title;
            Body = body;
        }
        public ExceptionErrorVM ToErrorVM()
        {
            return new ExceptionErrorVM(Code, Title,Body) ;
        }

        public static CustomException IgnoreAndContinue(string title, string body)
        {
            return new CustomException(5, title, body);
        }

        public static CustomException CreateInformationMessage(string title, string body)
        {
            return new CustomException(3, title, body);
        }

        public static CustomException CreateWarningMessage(string title, string body)
        {
            return new CustomException(2, title, body);
        }

        public static CustomException CreateErrorMessage(string title, string body)
        {
            return new CustomException(1, title, body);
        }

    }

    public class ExceptionErrorVM
    {
        public int Code { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public ExceptionErrorVM(int code, string title, string body)
        {
            Code = code;
            Title = title;
            Body = body;
        }
    }
}
