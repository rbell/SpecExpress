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
            IsDelegate = false;
        }

        public RuleParameter(string propName, LambdaExpression expression)
        {
            PropertyName = propName;
            this.CompiledExpression = new CompiledExpression(expression);
            IsExpressionParam = true;
            IsDelegate = false;
        }

        public RuleParameter(string propName, Delegate dDelegate)
        {
            PropertyName = propName;
            Delegate = dDelegate;
            IsExpressionParam = false;
            IsDelegate = true;
        }

        public string PropertyName { get; private set; }
        public bool IsExpressionParam { get; private set; }
        public bool IsDelegate { get; private set; }
        private object paramValue { get; set; }
        public CompiledExpression CompiledExpression { get; private set; }
        public Delegate Delegate { get; private set; }

        public object GetParamValue()
        {
            if (IsExpressionParam)
            {
                throw new System.ArgumentException("Cannot get Param Value for an Expression Param without context.");
            }
            return paramValue;
        }

        public object GetParamValue<T,TProperty>(RuleValidatorContext<T, TProperty> context = null)
        {
            if (IsExpressionParam)
            {
                if (context == null)
                {
                    throw new System.ArgumentException(
                        "Cannot get Param Value for an Expression Param without context.");
                }

                return CompiledExpression.Invoke(context.Instance);
            }
            else
            {
                if (IsDelegate)
                {
                    return Delegate.DynamicInvoke(new object[] {context.Instance});
                }
                else
                {
                    return paramValue;
                }
            }
        }

        public object GetParamValue2(RuleValidatorContext context = null)
        {
            if (IsExpressionParam)
            {
                if (context == null)
                {
                    throw new System.ArgumentException(
                        "Cannot get Param Value for an Expression Param without context.");
                }

                return CompiledExpression.Invoke(context.Instance);
            }
            else
            {
                if (IsDelegate)
                {
                    return Delegate.DynamicInvoke(new object[] { context.Instance });
                }
                else
                {
                    return paramValue;
                }
            }
        }

    }
}