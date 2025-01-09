using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BaseLibrary.Utilities
{
    public class MobileNumberExtractor
    {
        static bool IsIndia(string country)
        {
            if (string.IsNullOrWhiteSpace(country)) return false;
            if (country.Equals("INDIA", StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

        const string MatchMobileNumberPattern2 = @"(\d{3}[-\.\s]??\d{3}[-\.\s]??\d{4}|\(\d{3}\)\s*\d{3}[-\.\s]??\d{4}|\d{3}[-\.\s]??\d{4})";

        const string MatchMobileNumberPattern1 = @"\(?([1-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})";  // new fix for bug 480

        const string MatchMobileNumberPattern3 = @"^(\(?\d{3}\)?[\s,\-]?\d{3}\-?\d{4})$";

        const string MatchMobileNumberPattern = @"^(0|91|\+91)?-?[789]\d{9}$";

        private static List<string> invalidSkilCharacters = new List<string> { ",", ".", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "_", "+", "=", "<", ">", "/", "?", " " };

        public static List<string> ExtractMobileNumber(string fileText, List<string> emails, string country)
        {
            bool hasEmail = emails.Count > 0;
            var result = new List<string>();

            if (country != null && IsIndia(country))
            {
                result = ExtractIndianMobileNumber(fileText);
                if (result.Count > 0) return RemoveDuplicate(result);
            }

            MatchCollection matches = GetMatchedItems(MatchMobileNumberPattern, fileText);

            if (GotPhoneNumber(matches) == null)
                matches = GetMatchedItems(MatchMobileNumberPattern1, fileText);
            if (GotPhoneNumber(matches) == null)
                matches = GetMatchedItems(MatchMobileNumberPattern2, fileText);
            if (GotPhoneNumber(matches) == null)
                matches = GetMatchedItems(MatchMobileNumberPattern3, fileText);
            if (matches.Count > 0 && matches.Count < 3)
            {
                result = matches.Select(m => m.Value).ToList();
                if (result.Count() > 2)
                {
                    result = RemoveInvalidNumber(result, country);
                    if (result.Count() > 2 && hasEmail)
                    {
                        result = ExtractNumberNearEmail(fileText, emails[0]);
                    }
                }
            }
            else
            {
                if (hasEmail)
                {
                    result = ExtractNumberNearEmail(fileText, emails[0]);
                }
            }

            result = RemoveDuplicate(result);
            return result.Take(3).ToList();

        }

        private static string GotPhoneNumber(MatchCollection matches)
        {
            if (matches.Count == 0) return null;
            if (matches.Count > 2) return null;
            foreach (Match match in matches)
            {
                if (IsValidNumber(match.Value)) return match.Value;
            }
            return null;
        }

        private static bool IsValidNumber(string phone)
        {
            return phone.Length >= 10 && phone.Length <= 12;
        }

        private static MatchCollection GetMatchedItems(string regEx, string fileText)
        {
            Regex rx = new Regex(regEx, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(fileText);
            return matches;
        }

        public static List<string> ExtractMobileNumberUsingRegEx(string text)
        {
            text = RemoveSpace(text);
            var result = new List<string>();
            Regex rx = new Regex(MatchMobileNumberPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(text);
            if (matches.Count > 0)
            {
                result = matches.Select(m => m.Value).ToList();
            }
            return result;
        }

        private static string RemoveSpace(string input)
        {
            return input.Trim().Replace(" ", "");
        }

        private static char[] invalidcharacters = new char[] { '/', '+' };

        private static List<string> ExtractIndianMobileNumber(string fileText)
        {
            var result = new List<string>();
            var index = fileText.IndexOf("+91");

            if (index == -1)
                index = fileText.IndexOf("+ 91");

            if (index == -1)
                index = fileText.IndexOf("+ 9 1");

            if (index > 0)
            {
                var mobileString = GetMobileNumberSubstring(fileText, index);
                mobileString = mobileString.Replace("+91", "");
                var mobile = "";
                foreach (var character in mobileString)
                {
                    if (char.IsDigit(character))
                        mobile += character.ToString();
                    if (InvalidPhoneCharacters(character) || mobile.Length >= 10) break;
                }
                result.Add(mobile);
            }
            return result;
        }

        private static string GetMobileNumberSubstring(string fileText, int index)
        {
            var substring = fileText.Substring(index);
            if (substring.Length > 50)
                substring = substring.Substring(0, 50);
            substring = RemoveSpace(substring);
            if (substring.Length > 20)
                substring = substring.Substring(0, 20);
            return substring;
        }

        private static List<string> ExtractNumberNearEmail(string cvText, string email)
        {
            var lines = new List<string>();
            bool emailFound = false;
            using (StringReader sr = new StringReader(cvText))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    lines.Add(line);
                    if (emailFound) break;
                    if (line.Contains(email))
                    {
                        emailFound = true;
                    };
                }
            }

            if (lines.Count() > 5)
                lines = lines.TakeLast(5).ToList();

            var phones = GetContactNumbers(lines);
            if (phones.Count > 0) return phones;
            var phone = GetContactNumbersFrom(lines);
            if (phone == null) return new List<string>();
            return new List<string> { phone };
        }

        private static List<string> GetContactNumbers(List<string> lines)
        {
            var result = new List<string>();
            foreach (var line in lines)
            {
                Regex rx = new Regex(MatchMobileNumberPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection matches = rx.Matches(RemoveSpace(line));
                if (matches.Count > 0)
                {
                    result.AddRange(matches.Select(m => m.Value).ToList());
                }
            }
            return result;
        }

        private static string GetContactNumbersFrom(List<string> lines)
        {
            foreach (var line in lines)
            {
                string contactNumber = string.Empty;
                foreach (char c in line)
                {
                    if (char.IsLetter(c) || InvalidPhoneCharacters(c))
                    {
                        if (contactNumber.Length >= 10) return contactNumber;
                        contactNumber = string.Empty;
                    }
                    if (char.IsDigit(c))
                        contactNumber += c;
                }

                if (contactNumber.Length >= 10 && contactNumber.Length <= 12) return contactNumber;
            }
            return null;
        }

        private static bool InvalidPhoneCharacters(char c)
        {
            string invalid = "!@$%^&*_|{}:;<>,?/";
            return invalid.Contains(c);
        }


        private static List<string> RemoveInvalidNumber(List<string> numbers, string country)
        {
            var validNumbers = new List<string>();
            foreach (var num in numbers)
            {
                if (string.IsNullOrWhiteSpace(num)) continue;
                var number = num;
                foreach (var invalidChar in invalidSkilCharacters)
                {
                    number = number.Replace(invalidChar, "");
                }
                if (number.StartsWith("0"))
                    number = number.Substring(1);

                if (country != null && IsIndia(country) && number.Length != 10) continue;
                if (number.Length <= 8) continue;
                validNumbers.Add(number);
            }

            return validNumbers;
        }

        public static List<string> RemoveDuplicate(List<string> contactNumbers)
        {
            var mobile = string.Empty;
            var validNumber = string.Empty;
            var filteredNumbers = new List<string>();
            foreach (var number in contactNumbers)
            {
                if (number.Length <= 7) continue;
                validNumber = string.Empty;
                mobile = number.Replace(" ", "");

                foreach (var mob in mobile)
                {
                    if (char.IsDigit(mob))
                        validNumber += mob;
                }
                if (filteredNumbers.Any(e => validNumber.Equals(e, StringComparison.OrdinalIgnoreCase))) continue;
                filteredNumbers.Add(validNumber);

            }
            return filteredNumbers;
        }
    }
}
