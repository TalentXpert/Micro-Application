
using System.Collections.Generic;

namespace BaseLibrary.Utilities
{
    public class WebsiteLinkExtractor : InfoExtractorBase
    {
        public List<string> Extract(string input)
        {
            var regEx = @"(?i)\b((?:[a-z][\w-]+:(?:/{1,3}|[a-z0-9%])|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'.,<>?«»“”'']))";
            return ExtractInfoByRegEx(input, regEx);
        }
    }
}
