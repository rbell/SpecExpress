using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Util
{
    public static class PredicateExtensions
    {
        public static bool Try<T>(this Predicate<T> predicate, T instance)
        {
            try
            {
                return predicate(instance);
            }
            catch (Exception)
            {

                return false;
            }
          

            //MemberExpression memberExp = RemoveUnary(expression.Body);

            //if (memberExp == null)
            //{
            //    return null;
            //}

            //return memberExp.Member;
        }

    }
   
}
