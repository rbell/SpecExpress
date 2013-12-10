using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpecExpress.Util
{
    public static class StringExtensions
    {
        public static string SplitPascalCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            return Regex.Replace(input, "([A-Z][A-Z]*)", " $1").Trim().Replace("  ", " ");
        }

        /// <summary>
        /// Takes a list of string, reverses them, and appends them together in a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToReverseString(this List<String> input)
        {
            if (input.Any())
            {
                input.Reverse();
                return input.Aggregate((x, y) => x.SplitPascalCase() + " " + y.SplitPascalCase()).Trim();
            }
            else
            {
                return input.FirstOrDefault();
            }
           
        }

    }
}