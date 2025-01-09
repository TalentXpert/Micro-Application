namespace BaseLibrary.Utilities.Exceptions
{
    public class CustomValidationException : Exception
    {
        public string Code { get; set; }
        public string Group { get; set; }
        public object[] Args { get; set; }
        public CustomValidationException(string group, string code, params object?[] args)
        {
            Code = code;
            Group = group;
            Args = args;
        }
    }

}
