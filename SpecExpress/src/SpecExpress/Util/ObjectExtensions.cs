using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Util
{
    public static class ObjectExtensions
    {
        public static bool IsNullOrDefault<TProperty>(this TProperty input)
        {
            if (input == null)
            {
                return true;
            }

            //Ignore default values for booleans
            if (input.GetType() == typeof(bool) || input.GetType().IsEnum)
            {
                return false;
            }

            //Evaluate special cases
            return (input.Equals(string.Empty)
                         || Equals(input, default(TProperty))
                         || !(!(input is IEnumerable) ||
                                (input is IEnumerable && ((IEnumerable)(input)).GetEnumerator().MoveNext()))
                     );

        }

    }
}
