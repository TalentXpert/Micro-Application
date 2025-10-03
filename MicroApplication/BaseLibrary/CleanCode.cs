namespace BaseLibrary
{
    public abstract class C : CleanCode
    {

    }
    public abstract class CleanCode
    {
        public static bool IsNotNull(object? input)
        {
            return input != null;
        }
        public static bool IsNull(object? input)
        {
            return input == null;
        }
        public static bool IsNotNullOrEmpty(string? input)
        {
            return !IsNullOrEmpty(input);
        }
        public static bool IsNullOrEmpty(string? input)
        {
            return string.IsNullOrWhiteSpace(input);
        }
        public static bool IsNullOrEmpty(Guid? input)
        {
            return input == null || input.Value == Guid.Empty;
        }
        public static bool ContainsIgnoreCase(string content, string input)
        {
            return content.Contains(input,StringComparison.InvariantCultureIgnoreCase);
        }
        public static bool AreEqualsIgnoreCase(string? left, string? right)
        {
            return string.Equals(left, right, StringComparison.InvariantCultureIgnoreCase);
        }
        public static bool AreNotEqualsIgnoreCase(string? left, string? right)
        {
            return string.Equals(left, right, StringComparison.InvariantCultureIgnoreCase)==false;
        }
        public static bool IsMoreThanZero(int? number)
        {
            if (number.HasValue && number.Value > 0)
                return true;
            return false;
        }
        public static bool IsTrue(bool input)
        {
            return input == true;
        }
        public static bool IsFalse(bool input)
        {
            return input == false;
        }
        public static bool HasChildren<T>(IList<T> list)
        {
            if (list == null)
                return false;
            if (list.Count == 0)
                return false;
            return true;
        }
        public static bool HasNoChild<T>(IList<T> list)
        {
            return !HasChildren(list);
        }

        public static bool IsNotNullOrEmpty(Guid? input)
        {
            if (IsNull(input))
                return false;
            if (input == Guid.Empty)
                return false;
            return true;
        }

        public static string Trim(string input)
        {
            if (IsNotNullOrEmpty(input)) return input;
            return input.Trim();
        }
        public static bool IsDBNull(object? value)
        {
            return Convert.IsDBNull(value);
        }
        public static bool IsNotDBNull(object? value)
        {
            return !Convert.IsDBNull(value);
        }

        public static bool HasContents(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;
            return true;
        }
        public static bool HasNoContents(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return true;
            return false;
        }
        public static bool IsMoreThanMaxLength(string input, int maxLength)
        {
            if (input != null && input.Length > maxLength)
                return true;
            return false;
        }
        public static Guid GetLoggedInUserOrganization(ApplicationUser? user)
        {
            if (user is null)
                throw new ValidationException($"You must login to perform this operation.");
            if(user.OrganizationId.HasValue==false)
                throw new ValidationException($"You must be part of an organization to perform this operation.");
            return user.OrganizationId.Value;   
        }
        public static ApplicationUser GetLoggedInUser(ApplicationUser? user)
        {
            if (user is null)
                throw new ValidationException($"You must login to perform this operation.");
            return user;
        }
        public bool HasPermission(List<string> userPermissions, Permission permission)
        {
            return userPermissions.Any(p => AreEqualsIgnoreCase(p, permission.Code));
        }
        public bool HasPermission(List<string> userPermissions, string permissionCode)
        {
            return userPermissions.Any(p => AreEqualsIgnoreCase(p, permissionCode));
        }

        public string GetFormattedDate(DateTime? date)
        {
            string result = string.Empty;
            if (date.HasValue)
            {
                result = date.Value.ToString("dd-MMM-yyyy");
            }
            return result;
        }
        public string FormatNumber(int? number)
        {
            if (number is not null && number.HasValue) return number.Value.ToString();
            return "";
        }

        public string FormatDecimal(decimal? number)
        {
            if (number is not null && number.HasValue) return number.Value.ToString();
            return "";
        }

        public string FormatBoolean(bool? boolean)
        {
            if (boolean.HasValue)
                return boolean.Value ? "Yes" : "No";
            return "";
        }
        public string RemoveSpace(string input)
        {
            return input.Replace(" ", "");
        }
    }
}
