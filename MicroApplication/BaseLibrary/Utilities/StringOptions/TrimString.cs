namespace BaseLibrary.Utilities
{
    public class TrimString
    {
        public static string Trim(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            return input.Trim();
        }
    }
}
