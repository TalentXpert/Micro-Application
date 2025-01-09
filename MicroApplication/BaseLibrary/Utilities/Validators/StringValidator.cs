
namespace BaseLibrary.Utilities
{
    public class StringValidator : CleanCode
    {
        public void MustNotBeNullOrEmpty(string input, string fieldName = "Input string", string message = "")
        {
            if (IsNullOrEmpty(input))
            {
                if (IsNotNullOrEmpty(message))
                    throw new ValidationException(message);
                throw new ValidationException(fieldName + " can not be null or empty.");
            }
        }

        public void CheckForNullOrEmpty(string input, string FieldName, List<ValidationResult> validationResults)
        {
            if (IsNullOrEmpty(input))
                validationResults.Add(new ValidationResult(FieldName + " can not be empty.", new string[] { FieldName }));
        }

        public void CheckForMaxLength(string input, string FieldName, int maxlength, List<ValidationResult> validationResults)
        {
            if (IsMoreThanMaxLength(input,maxlength))
                validationResults.Add(new ValidationResult($"({FieldName} '{input}' can not be more than {maxlength} characters.", new string[] { FieldName }));
        }
    }
}
