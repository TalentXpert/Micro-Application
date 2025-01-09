using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseLibrary.Utilities
{



    public class SalaryExtractor : InfoExtractorBase
    {
        List<string> lacsWords = new List<string> { "l", "lac", "lacs" };
        List<string> thousandWords = new List<string> { "k", "t" };
        List<string> validWords = new List<string> { "k", "t", "l", "lac", "lacs" };

        public decimal? ExtractSalary(string text)
        {
            foreach (var salarySufix in validWords)
            {
                var lines = GetLinesHavingWord(text, salarySufix);
                foreach (var line in lines)
                {
                    var number = new ExtractNumberBeforeAWord().ExtractNumber(text, salarySufix);
                    var salary = X.Extension.Decimal.ConvertToDecimal(number);
                    if (salary.HasValue)
                        return ConvertToRealNumberIfRequred(salary, salarySufix);
                }
            }
            return null;

        }

        private decimal? ConvertToRealNumberIfRequred(decimal? salary, string sufix)
        {
            if (salary == null) return null;

            if (lacsWords.Any(l => l.Equals(sufix, StringComparison.OrdinalIgnoreCase))) return salary * 100 * 1000;
            if (thousandWords.Any(l => l.Equals(sufix, StringComparison.OrdinalIgnoreCase))) return salary * 1000;
            return salary;
        }
    }

    public class ExtractNumberBeforeAWord : InfoExtractorBase
    {
        public string ExtractNumber(string text, string word)
        {
            var lines = GetLinesHavingWord(text, word);

            foreach (var line in lines)
            {
                if (HasNumber(line, word))
                {
                    var index = line.IndexOf(word, StringComparison.OrdinalIgnoreCase);
                    string salaryWord = null;
                    if (index > 15)
                    {
                        salaryWord = line.Substring((index - 10), 10);
                    }
                    else
                    {
                        salaryWord = line.Substring(0, index);
                    }

                    var salary = X.Extension.Decimal.ConvertToDecimal(salaryWord);
                    if (salary.HasValue)
                        return salary.ToString();
                }
            }
            return null;
        }

        private bool HasNumber(string line, string word)
        {
            var index = line.IndexOf(word);
            for (int i = 0; i < 10; i++)
            {
                var newIndex = index - i;
                if (newIndex > 0)
                {
                    if (char.IsWhiteSpace(line[newIndex])) continue;
                    if (char.IsDigit(line[newIndex])) return true;
                    return false;
                }
            }
            return true;
        }
    }


}
