using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BaseLibrary.Utilities.DataCleaners
{
    public class DataCleaner
    {

        CVFontCleaner? _cvFontCleaner;
        public CVFontCleaner CVFontCleaner { get { return _cvFontCleaner ?? (_cvFontCleaner = new CVFontCleaner()); } }
    }

    public class CVFontCleaner:CleanCode
    {
        public string ReplaceFontSizeToCVFont(string text)
        {
            if (IsNullOrEmpty(text)) return text;
            string toReplace = "font-size: [0-9]+[a-zA-Z]*";
            string withThis = "font-size: 14px";

            var regEqu = Regex.Replace(text, toReplace, withThis);

            regEqu = regEqu.Replace("\"", "'");

            string toReplaceS = "style='[a-zA-Z0-9%*#&,-<> ();.!:]*'";
            string withThisS = "style='font-family: Roboto, sans-serif; font-size: 14px; font-style: normal;margin-bottom: 0;'";
            regEqu = Regex.Replace(regEqu, toReplaceS, withThisS);

            //replace header tags with p tag
            string toReplaceH = "<h[0-9]+";
            string withThisH = "<h6";

            regEqu = Regex.Replace(regEqu, toReplaceH, withThisH);

            string toReplaceHclose = "</h[0-9]+";
            string withThisHclose = "</h6";

            regEqu = Regex.Replace(regEqu, toReplaceHclose, withThisHclose);

            string returnString = regEqu.Replace("'", "\"");
            return returnString;
        }
    }
}
