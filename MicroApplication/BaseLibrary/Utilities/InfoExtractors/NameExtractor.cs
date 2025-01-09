
using System.Collections.Generic;

namespace BaseLibrary.Utilities
{


    public class NameExtractor
    {
        public static string ExtractName(string cvText, List<string> emails, string filename)
        {
            string name = new ExractFromLinesNearEmail().ExtractName(cvText, emails);
            if (!string.IsNullOrWhiteSpace(name)) return name;

            if (!string.IsNullOrWhiteSpace(filename))
                name = new ExtractFromFileName().ExtractName(filename, cvText);

            if (!string.IsNullOrWhiteSpace(name)) return name;

            name = new ExtractUsingLabel().ExtractName(cvText);

            return name;
        }
    }
}
