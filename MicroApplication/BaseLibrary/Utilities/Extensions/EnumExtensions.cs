using System.Reflection;

namespace BaseLibrary.Utilities.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var attribute = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>();

            return attribute?.GetName() ?? enumValue.ToString();
        }

        public static T? GetEnumByDisplayName<T>(string displayName) where T : struct, Enum
        {
            foreach (var value in Enum.GetValues(typeof(T)).Cast<T>())
            {
                var name = ((Enum)(object)value).GetDisplayName();
                if (name.Equals(displayName, StringComparison.OrdinalIgnoreCase))
                    return value;
            }

            return null; // not found
        }
    }
}
