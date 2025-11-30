using System.Reflection;
using XLParser;

namespace BaseLibrary.Utilities
{
    public class ReflectionExtension
    {
        /// <summary>
        /// Return all public static fields of class T those of R type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <returns></returns>
        public static List<R> GetAllPublicStaticFields<T, R>() where T : class where R : class
        {
            var result = new List<R>();
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).ToList();
            var items = fields.Where(f => f.FieldType == typeof(R));

            foreach (var item in items)
            {
                var itemValue = item.GetValue(null) as R;
                if (itemValue is not null)
                    result.Add(itemValue);
            }
            return result;
        }
        /// <summary>
        /// Return all public static fields of class T those of T type
        /// </summary>
        public static List<T> GetAllPublicStaticFields<T>() where T : class 
        {
            return GetAllPublicStaticFields<T, T>();
        }

        /// <summary>
        /// Return all public static fields of class T those of R type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <returns></returns>
        public static List<R> GetAllPublicStaticProperties<T, R>() where T : class where R : class
        {
            var result = new List<R>();
            var fields = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).ToList();
            var items = fields.Where(f => f.PropertyType == typeof(R));

            foreach (var item in items)
            {
                var itemValue = item.GetValue(null) as R;
                if (itemValue is not null)
                    result.Add(itemValue);
            }
            return result;
        }
        /// <summary>
        /// Return all public static fields of class T those of T type
        /// </summary>
        public static List<T> GetAllPublicStaticProperties<T>() where T : class
        {
            return GetAllPublicStaticProperties<T, T>();
        }

        /// <summary>
        /// Return all public static fields of class T those of R type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <returns></returns>
        public static List<R> GetAllPublicStaticFieldsAndProperties<T, R>() where T : class where R : class
        {
            var result = new List<R>();
            var fields = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).ToList();
            var items = fields.Where(f => f.PropertyType == typeof(R));

            foreach (var item in items)
            {
                var itemValue = item.GetValue(null) as R;
                if (itemValue is not null)
                    result.Add(itemValue);
            }
            return result;
        }
        /// <summary>
        /// Return all public static fields of class T those of T type
        /// </summary>
        public static List<T> GetAllPublicStaticFieldsAndProperties<T>() where T : class
        {
            return GetAllPublicStaticFieldsAndProperties<T, T>();
        }
    }
}
