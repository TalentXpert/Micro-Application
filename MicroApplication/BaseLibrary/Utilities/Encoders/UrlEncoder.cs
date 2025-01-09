
using System.Text.RegularExpressions;

namespace BaseLibrary.Utilities.Encode
{

    public class WebsiteEncoder
    {
        public string GetPlainWebsiteName(string website)
        {
            if (website.Contains("https"))
                website = website.Replace("https", "");
            if (website.Contains("http"))
                website = website.Replace("http", "");
            if (website.Contains("www"))
                website = website.Replace("www", "");
            var plainWebsite = string.Empty;
            foreach (var c in website)
            {
                if (char.IsLetter(c) || char.IsDigit(c))
                    plainWebsite += c.ToString();
            }
            return plainWebsite;
        }
    }
    public class UrlEncoder
    {
        public string CreateUrlSafeString(string input)
        {
            string result = Regex.Replace(input, "[^a-zA-Z0-9_]+", "-");
            return result;
        }
    }

    public class FileNameEncoder
    {
        public string GetSafeDownloadFileName(string input)
        {
            var safeFileName = string.Empty;
            foreach (var c in input)
            {
                if (char.IsLetter(c) || char.IsDigit(c) || c=='.')
                    safeFileName += c.ToString();
            }
            return safeFileName;
        }
    }

}
