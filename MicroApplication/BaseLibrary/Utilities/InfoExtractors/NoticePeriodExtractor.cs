using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseLibrary.Utilities
{
    public class NoticePeriodExtractor : InfoExtractorBase
    {
        List<string> monthWords = new List<string> { "mon", "month", "months", "mnths" };
        List<string> validWords = new List<string> { "days", "d", "mon", "month", "months", "mnths" };

        public int? ExtractNoticePeriod(string text)
        {
            var noticePeriod = GetNoticePeriodByNoticePeriodWord(text);

            if (noticePeriod == null)
                noticePeriod = GetNoticePeriodWithValidSufix(text);

            if (noticePeriod.HasValue)
                return Convert.ToInt32(noticePeriod);

            return null;
        }

        /// <summary>
        /// Extract notice period which is in this format - Notice Period 15 Days or less";
        /// </summary>
        private decimal? GetNoticePeriodByNoticePeriodWord(string text)
        {
            var noticePeriodString = GetStringAfterWordOfLengthOrDefault(text, "Notice Period", 12);
            if (noticePeriodString != null)
            {
                var noticePeriod = X.Extension.Decimal.ConvertToDecimal(noticePeriodString);
                if (monthWords.Any(w => noticePeriodString.Contains(w, StringComparison.OrdinalIgnoreCase)))
                    return noticePeriod.Value * 30;
                return noticePeriod;
            }
            return null;
        }

        private decimal? GetNoticePeriodWithValidSufix(string text)
        {
            foreach (var np in validWords)
            {
                var lines = GetLinesHavingWord(text, np);
                foreach (var line in lines)
                {
                    if (!IsValidLine(line)) continue;
                    var number = new ExtractNumberBeforeAWord().ExtractNumber(line, np);
                    var noticePeriod = X.Extension.Decimal.ConvertToDecimal(number);
                    if (noticePeriod.HasValue)
                    {
                        noticePeriod = ConvertToRealNumberIfRequred(noticePeriod, np);
                        if (noticePeriod.HasValue && noticePeriod.Value < 100) return Convert.ToInt32(noticePeriod);
                    }
                }
            }
            return null;
        }

        private bool IsValidLine(string line)
        {
            var invalidWords = new List<string> { "years", "yrs", "yr" };
            return !invalidWords.Any(w => line.Contains(w, StringComparison.OrdinalIgnoreCase));
        }

        private decimal? ConvertToRealNumberIfRequred(decimal? noticePeriod, string np)
        {
            if (noticePeriod == null) return null;
            if (monthWords.Any(l => l.Equals(np, StringComparison.OrdinalIgnoreCase))) return noticePeriod * 30;
            return noticePeriod;
        }

    }


}
