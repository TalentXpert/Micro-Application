using System;

namespace BaseLibrary.Utilities
{
    public static class GuidExtension
    {
        public static bool IsEmpty(this Guid input)
        {
            return input == Guid.Empty;
        }

        public static bool IsNullOrEmpty(this Guid? input)
        {
            if (input == null) return true;
            if (input.HasValue && input.Value == Guid.Empty) return true;
            return false;
        }

        
    }

    public class Convertor
    {
        public static Guid? ToGuid(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;
            if (Guid.TryParse(input, out Guid result))
                return result;
            return null;
        }
    }
}
