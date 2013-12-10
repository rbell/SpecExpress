using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecExpress.Util
{
    public static class ArrayExtensions
    {
        public static TOutput[] ConvertAll<TInput, TOutput>(this Array array, Converter<TInput, TOutput> converter)
        {
            if (array == null)
                throw new ArgumentException();

            var lst = new List<TInput>(array as IEnumerable<TInput>);

            return (from item in lst select converter(item)).ToArray();
        }
    }

}