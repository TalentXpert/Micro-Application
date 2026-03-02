using System.Text.RegularExpressions;

namespace BaseLibrary.Utilities
{
    public class TrimString
    {
        public static string Trim(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            return input.Trim();
        }

        public static string KeepOnlyLettersDigitsAndSpaces(string input)
        {
            try
            {
                var cleaned = new string(input
                        .Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                        .ToArray());
                return Regex.Replace(cleaned, @"\s+", " ").Trim();
            }
            catch
            {
                return input;
            }
        }
    }
}
