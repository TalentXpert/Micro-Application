using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BaseLibrary.Utilities
{
    public class StringExtension
    {
        public string RemoveEmptySpace(string input)
        {
            return input.Trim().Replace(" ", "");
        }

        public bool IsInteger(string input)
        {
            foreach (char c in input)
            {
                if (char.IsDigit(c)) continue;
                return false;
            }
            return true;
        }

        public string Join<T>(IEnumerable<T> items, string separator = ",", string prefix = "'", string sufix = "'")
        {
            string result = null;
            if (prefix == null) prefix = "";
            if (sufix == null) sufix = "";
            if (separator == null) separator = ",";

            foreach (var item in items)
            {
                if (result == null)
                    result = prefix + item + sufix;
                else
                    result += separator + prefix + item + sufix;
            }
            return result;
        }

        public string TitleCase(string input)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            var output = textInfo.ToTitleCase(input);
            return output;
        }

        public bool AreEqual(string input1,string input2)
        {
            return input1.Equals(input2, System.StringComparison.OrdinalIgnoreCase);
        }
        public string KeepOnlyLettersDigitsAndSpaces(string input)
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

        public List<string> GetAllWordStartWith(string inputText,string startWith="@")
        {
            string pattern = @$"\b{startWith}\w+";

            // Create a Regex object
            Regex regex = new Regex(pattern);

            // Find all matches in the input string
            MatchCollection matches = regex.Matches(inputText);

            // Store the found strings in a list
            List<string> foundStrings = new List<string>();

            // Iterate through the matches and extract the values
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    foundStrings.Add(match.Value);
                }
            }
            return foundStrings;
        }
    }
}
