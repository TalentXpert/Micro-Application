using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BaseLibrary.Utilities
{
    public class IntegerValidator
    {
        public void CheckForEmpltyOrDefaulValue(int input, string FieldName, List<ValidationResult> validationResults)
        {
            if(IsDefaultValue(input))
                validationResults.Add(new ValidationResult(FieldName + " can not be empty or 0.", new string[] { FieldName }));
        }

        public bool IsDefaultValue(int input)
        {
            return default(int) == input;
        }
    }

    public class DecimalValidator
    {
        public void CheckForEmpltyOrDefaulValue(decimal input, string FieldName, List<ValidationResult> validationResults)
        {
            if (IsDefaultValue(input))
                validationResults.Add(new ValidationResult(FieldName + " can not be empty or 0.", new string[] { FieldName }));
        }

        public bool IsDefaultValue(decimal input)
        {
            return default(decimal) == input;
        }
    }

    public class GuidValidator
    {
        public void CheckForEmpltyOrDefaulValue(Guid? input, string FieldName, List<ValidationResult> validationResults)
        {
            if (input is null || input.Value.IsEmpty())
                validationResults.Add(new ValidationResult(FieldName + " can not be empty.", new string[] { FieldName }));
        }
        
    }
    public class DoubleValidator
    {
        public void CheckForEmpltyOrDefaulValue(double input, string FieldName, List<ValidationResult> validationResults)
        {
            if (IsDefaultValue(input))
                validationResults.Add(new ValidationResult(FieldName + " can not be empty.", new string[] { FieldName }));
        }

        public bool IsDefaultValue(double input)
        {
            return default(double) == input;
        }
    }
}
