using System;
using System.Linq.Expressions;

namespace SpecExpress.Rules
{
    public class RuleParameter
    {
        public RuleParameter(string propName)
        {
            PropertyName = propName;
        }

        public RuleParameter(string propName, object paramValue)
        {
            PropertyName = propName;
            this.paramValue = paramValue;
            IsExpressionParam = false;
        }

        public RuleParameter(string propName, LambdaExpression expression)
        {
            PropertyName = propName;
            this.CompiledExpression = new CompiledExpression(expression);
            IsExpressionParam = true;
        }

        public string PropertyName { get; private set; }
        public bool IsExpressionParam { get; private set; }
        private object paramValue { get; set; }
        public CompiledExpression CompiledExpression { get; private set; }

        public object GetParamValue()
        {
            if (IsExpressionParam)
            {
                throw new System.ApplicationException("Cannot get Param Value for an Expression Param without context.");
            }
            return paramValue;
        }

        public object GetParamValue<T,TProperty>(RuleValidatorContext<T, TProperty> context = null)
        {
            if (IsExpressionParam)
            {
                if (context == null)
                {
                    throw new System.ApplicationException(
                        "Cannot get Param Value for an Expression Param without context.");
                }

                return CompiledExpression.Invoke(context.Instance);
            }
            else
            {
                return paramValue;
            }
        }

    }
}