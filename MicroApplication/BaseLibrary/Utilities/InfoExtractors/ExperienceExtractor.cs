using System;
using System.Collections.Generic;

namespace BaseLibrary.Utilities
{
    public class ExperienceExtractor
    {
        
        public static string ExtractExperience(string cvText)
        {
            var experienceLine = GetExperienceLine(cvText);
            string experience = string.Empty;
            if (string.IsNullOrWhiteSpace(experienceLine))
                return "";

            foreach (var c in experienceLine)
            {
                if (char.IsDigit(c) || c == '.')
                {
                    if (c == '.' && string.IsNullOrWhiteSpace(experience)) continue;
                    experience += c.ToString();
                }
            }
            return experience;
        }

        private static string GetExperienceLine(string cvText)
        {
            var keywords = new List<string> { "year", "yrs" };
            foreach (var keyword in keywords)
            {
                var experienceLine = GetLineWithKeyword(keyword, cvText);
                if (!string.IsNullOrWhiteSpace(experienceLine)) return experienceLine;
            }
            return null;
        }

        private static string GetLineWithKeyword(string keyword, string cvText)
        {
            var index = cvText.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);
            if (index > 10) return cvText.Substring(index - 10, 11);
            return null;
        }
    }
}
