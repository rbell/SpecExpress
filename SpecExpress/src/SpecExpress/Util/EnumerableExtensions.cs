using System;
using System.Collections;
using System.Collections.Generic;

namespace SpecExpress.Util
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<string> ToStringEnum(this IEnumerable source)
        {
            IEnumerable<string> enumerable = source as IEnumerable<string>;
            if (enumerable != null)
                return enumerable;
            if (source == null)
                throw new ArgumentNullException("source");
            else
                return EnumerableExtensions.ToStringIterator(source);
        }

        private static IEnumerable<string> ToStringIterator(IEnumerable source)
        {
            foreach (object obj in source)
                yield return obj == null ? string.Empty : obj.ToString();
        }
    }
}