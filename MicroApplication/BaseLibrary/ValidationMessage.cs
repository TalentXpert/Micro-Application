using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary
{
    public class ValidationMessage
    {
        public static string Relogin = "Your session has expired. Please login again to continue.";

        public static string UserWithLoginIdNotFound = "User with this login id not found. Please try with valid login id.";

        public static string BlockedAccount(string? blockReason)
        {
            var m = $"Your account has been disabled. Please contact support team for more information.";
            if (string.IsNullOrWhiteSpace(blockReason))
                return m;
            return m + $"Message from disabler - {blockReason}";
        }

    }
}
