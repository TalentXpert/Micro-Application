using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLibrary.Utilities.Extensions
{
    public class LinqExtension
    {

    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            return enumerable.GroupBy(keySelector).Select(grp => grp.First());
        }
    }
}
