using System.Globalization;

namespace BaseLibrary.Utilities
{
    public class TimeZoneValidator
    {
        public void CheckForValidTimeZone(string input, string FieldName, List<ValidationResult> validationResults)
        {
            try
            {
                var result = TimeZoneInfo.FindSystemTimeZoneById(input);
            }
            catch
            {
                validationResults.Add(new ValidationResult(FieldName + " is not supported time zone.", new string[] { FieldName }));
            }
        }
    }

    public class LanguageValidator
    {
        public void CheckForValidLanguage(string input, string FieldName, List<ValidationResult> validationResults)
        {
            var result = CultureInfo.GetCultures(CultureTypes.AllCultures).Any(ci => ci.Name == input);
            if (!result)
                validationResults.Add(new ValidationResult(FieldName + " is not supported culture/language.", new string[] { FieldName }));
        }
    }

    public class CountryPhoneNumberValidator
    {
        public void ValidatePhone(Guid countryId, string phone, string FieldName, List<ValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(phone))
                validationResults.Add(new ValidationResult("Phone number can not be empty.", new string[] { FieldName }));

            long p;
            if (long.TryParse(phone.Trim(), out p)==false)
                validationResults.Add(new ValidationResult("Phone number should contain digits.", new string[] { FieldName }));

           // if (ApplicationConstants.IndiaCountryId== countryId)
                if (phone.Length != 10)
                    validationResults.Add(new ValidationResult("Indian phone number should be of 10 digits. Please remove country code 91 if added.", new string[] { FieldName }));
        }
    }
}
