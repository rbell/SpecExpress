using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SpecExpress.Util
{
    public static class ExpressionExtensions
    {
        public static MemberInfo GetMember<T, TProperty>(this Expression<Func<T, TProperty>> expression)
        {
            MemberExpression memberExp = RemoveUnary(expression.Body);

            if (memberExp == null)
            {
                return null;
            }

            return memberExp.Member;
        }

        private static MemberExpression RemoveUnary(Expression toUnwrap)
        {
            if (toUnwrap is UnaryExpression)
            {
                return ((UnaryExpression) toUnwrap).Operand as MemberExpression;
            }

            return toUnwrap as MemberExpression;
        }
    }
}