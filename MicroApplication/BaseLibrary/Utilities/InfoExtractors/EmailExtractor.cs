using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BaseLibrary.Utilities
{
    public class EmailCorrector
    {
        public static List<string> CorrectEmail(List<string> emails)
        {
            var filteredEmails = new List<string>();
            foreach (var email in emails)
            {
                var tempEmail = GmailNotComplete(email);
                filteredEmails.Add(tempEmail);
            }

            return filteredEmails;
        }

        private static string GmailNotComplete(string email)
        {
            if (email.Contains("gmail", StringComparison.OrdinalIgnoreCase))
            {
                if (email.Contains("@gmail.com", StringComparison.OrdinalIgnoreCase)) return email;
                var parts = email.Split("@");
                return parts[0] + "@gmail.com";
            }
            return email;
        }
    }

    public class EmailExtractor : InfoExtractorBase
    {
        private const string _matchEmailPattern =
            @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
            + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";

        private static Regex _rx = new Regex(_matchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static List<string> ExtractEmail(string fileText)
        {
            var emails = new List<string>();
            if (string.IsNullOrWhiteSpace(fileText)) return emails;

            var domaintypes = new List<string> { "gmail.com", "rediffmail.com", "hotmail.com", "yahoo.com", "outlook.com", "mail.com" };

            foreach (var domain in domaintypes)
            {
                var email = new DomainMailExtractor(fileText, domain).GetDomainEmail();
                if (!string.IsNullOrWhiteSpace(email) && IsValidEmail(email)) return new List<string> { email };
            }

            MatchCollection matches = _rx.Matches(fileText);

            if (matches.Count > 0)
            {
                emails = matches.Select(m => m.Value).ToList();
            }
            else
            {
                emails = GetEmail(fileText);
            }

            var uniqueEmails = RemoveDuplicate(emails);
            var validEmails = RemoveInvalidEmails(uniqueEmails);
            var correctedEmails = EmailCorrector.CorrectEmail(validEmails);

            return correctedEmails;
        }

        private static List<string> GetEmail(string fileText)
        {
            var emails = new List<string>();
            var lines = GetLinesHavingWord(fileText, "@");
            foreach (var line in lines)
            {
                var temp = line.Replace(" ", "");
                MatchCollection matches = _rx.Matches(temp);
                if (matches.Count > 0)
                {
                    emails = matches.Select(m => m.Value).ToList();
                    return emails;
                }
            }
            return emails;
        }

        private static bool IsValidEmail(string email)
        {
            if (IsCorrectLength(email))
            {
                var validator = new EmailAddressAttribute();
                if (validator.IsValid(email)) return true;
                if (EmailIsValid(email)) return true;
                if (X.Validator.EmailValidator.IsValidEmail(email)) return true;
            }
            return false;
        }

        private static bool IsCorrectLength(string email)
        {
            if (email.Length > 64) return false;
            return true;
        }

        private static bool EmailIsValid(string email)
        {
            string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

            if (Regex.IsMatch(email, expression))
            {
                if (Regex.Replace(email, expression, string.Empty).Length == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private static List<string> RemoveInvalidEmails(List<string> uniqueEmails)
        {
            var validEmails = new List<string>();
            foreach (var email in uniqueEmails)
            {
                if (X.Validator.EmailValidator.IsValidEmail(email))
                    validEmails.Add(email);
            }
            return validEmails;
        }

        private static List<string> RemoveDuplicate(List<string> emails)
        {
            var filteredEmails = new List<string>();
            foreach (var email in emails)
            {
                if (filteredEmails.Any(e => email.Equals(e, System.StringComparison.OrdinalIgnoreCase))) continue;
                filteredEmails.Add(email);
            }
            return filteredEmails;
        }

    }

    public class DomainMailExtractor : InfoExtractorBase
    {
        public string CvText { get; }
        private string SearchWord { get; }  //gmail
        private string Domain { get; } //gmail.com
        public DomainMailExtractor(string cvText, string domain) //domain - gmail.com
        {
            CvText = cvText;
            SearchWord = StringBefore(domain, ".");
            Domain = "@" + domain;
        }

        public string GetDomainEmail()
        {
            var lines = GetLinesHavingWord(CvText, SearchWord);
            foreach (var line in lines)
            {
                var index = IndexOf(line, SearchWord);
                if (index > -1)
                {
                    return GetValidEmailFirstPart(line) + Domain;
                }
            }

            return string.Empty;
        }

        //extract valid email before Search Word
        private string GetValidEmailFirstPart(string line)
        {
            if (line.Contains("Email   :"))
            {
                line = line.Replace(" ", "");
            }
                
            var invalidChars = new char[] { ':', ' ', '-' };
            line = StringBefore(line, "@" + SearchWord);
            int nonLetterCount = 0;
            int indexOfLastLetter = 0;
            for (int i = line.Length - 1; i >= 0; i -= 1)
            {
                if (char.IsLetter(line[i]))
                {
                    nonLetterCount = 0;
                    indexOfLastLetter = i;
                }
                if (invalidChars.Contains(line[i])) break;
                if (nonLetterCount > 5) break;
            }

            return line.Substring(indexOfLastLetter);
        }
    }
}
