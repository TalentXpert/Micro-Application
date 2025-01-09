using System.Globalization;
using System.Text.RegularExpressions;

namespace BaseLibrary.Utilities
{
    public class CityNameValidator : CleanCode
    {
        private static Regex regex = new Regex(@"^([a-zA-Z\u0080-\u024F]+(?:. |-| |'))*[a-zA-Z\u0080-\u024F]*$", RegexOptions.Compiled);
        public bool IsValidCityName(string cityName)
        {

            if (IsNullOrEmpty(cityName) || IsFalse(regex.IsMatch(cityName)))
                return false;
            return true;
        }
    }

    public class DesignationValidator : CleanCode
    {
        public bool IsValidDesignation(string designation)
        {
            if (IsNullOrEmpty(designation)) return false;
            foreach (var c in designation)
            {
                if (char.IsDigit(c))
                    return false;
            }

            return true;
        }
    }

    public class ContactNumbereValidator : CleanCode
    {
        //@"\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})"
        private static Regex regex = new Regex(@"^(\+\s?)?((?<!\+.*)\(\+?\d+([\s\-\.]?\d+)?\)|\d+)([\s\-\.]?(\(\d+([\s\-\.]?\d+)?\)|\d+))*(\s?(x|ext\.?)\s?\d+)?$", RegexOptions.Compiled);

        public bool IsValidContactNumber(string contactNumber)
        {
            if (IsNullOrEmpty(contactNumber) || IsFalse(regex.IsMatch(contactNumber)))
                return false;
            return true;
        }
    }

    public class NameValidator : CleanCode
    {
        private static Regex regex = new Regex(@"^[A-z][A-z|\.|\s]+$", RegexOptions.Compiled);

        public bool IsValidName(string name)
        {
            if (IsNullOrEmpty(name) || IsFalse(regex.IsMatch(name)))
                return false;
            return true;
        }
    }


    public class EmailValidator : CleanCode
    {
        public bool IsEmailBelongToDomain(string email, string domain)
        {
            var emailParts = email.Split('@');
            if (emailParts.Length > 1)
            {
                var emailDomain = emailParts.LastOrDefault();
                if (IsNotNull(emailDomain))
                    return domain.EndsWith(emailDomain);
            }
            return false;
        }

        public bool IsValidEmailBasicValidation(string email)
        {
            if (email.Contains("@") && email.Contains(".")) return true;
            return false;
        }

        public void MustNotBeNullOrEmpty(string email)
        {
            if (IsNullOrEmpty(email))
                throw new ValidationException("Email can not be null or empty.");
        }

        //https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format#:~:text=To%20verify%20that%20the%20email,name%20from%20the%20email%20address.
        public bool IsValidEmail(string email)
        {
            if (IsNullOrEmpty(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
