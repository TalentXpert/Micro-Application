using System.Text.RegularExpressions;

namespace BaseLibrary.Utilities
{
    public class RegExExtension
    {
        public List<string> GetAllOccuranceOfWordMatchingStartAndEndPatter(string text,string startPattern,string endPattern)
        {
            var pattern = $"(?<={startPattern})(.*?)(?={endPattern})";
            Regex reg = new Regex(pattern);
            var matches = reg.Matches(text);
            return matches.Select(i => i.Value).ToList();
        }
    }
}
