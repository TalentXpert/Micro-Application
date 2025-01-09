using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BaseLibrary.Utilities
{

    public class InfoExtractorBase
    {
        

        public List<string> ExtractInfoByRegEx(string input, string regEx)
        {
            Regex rx = new Regex(regEx, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(input);
            if (matches.Count > 0)
                return matches.Select(m => m.Value).ToList();
            return null;
        }
    
        public static StringComparison StringComparisonRule = StringComparison.InvariantCultureIgnoreCase;
        public static IEnumerable<string> GetLinesHavingWord(string fileText, string word)
        {
            var lines = fileText.Split(new[] { '\r', '\n' });
            IEnumerable<string> selectLines = lines.Where(line => line.Contains(word, StringComparison.OrdinalIgnoreCase));
            return selectLines;
        }

        public static string GetStringOfLengthOrDefault(string text, int index, int length)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            int textLength = text.Length;
            if (textLength < index) return null;
            var result = text.Substring(index);
            if (result.Length >= length) return result.Substring(0, length);
            return result;
        }

        public static string GetStringAfterWordOfLengthOrDefault(string text, string word, int length)
        {
            var indexOfWord = text.IndexOf(word, StringComparison.OrdinalIgnoreCase);
            if (indexOfWord > -1)
            {
                return GetStringOfLengthOrDefault(text, indexOfWord + word.Length, length);
            }
            return null;
        }

        public static bool ContainWord(string input, string word)
        {
            return input.Contains(word, StringComparison.InvariantCultureIgnoreCase);
        }

        public static int CountOfccurrences(string input, string pattern)
        {
            int count = 0;
            int i = 0;
            while ((i = input.IndexOf(pattern, i, StringComparisonRule)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }

        public static int IndexOf(string input, string pattern)
        {
            return input.IndexOf(pattern, StringComparisonRule);
        }

        public static string StringBefore(string input, string pattern)
        {
            var index = IndexOf(input, pattern);
            if (index > 0)
            {
                return input.Substring(0, index);
            }
            return input;
        }
        public static string RemoveSpace(string input)
        {
            return input.Trim().Replace(" ", "");
        }
    }
}
