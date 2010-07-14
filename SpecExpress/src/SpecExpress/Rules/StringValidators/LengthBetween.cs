using System;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.StringValidators
{
    public class LengthBetween<T> : RuleValidator<T, string>
    {
        private int _min;
        private int _max;

        public LengthBetween(int min, int max)
        {
            if (max < min)
            {
                throw new ArgumentOutOfRangeException("max", "Max should be larger than min.");
            }

            _max = max;
            _min = min;
        }

        public LengthBetween(Expression<Func<T, int>> min, Expression<Func<T, int>> max)
        {
            SetPropertyExpression("min", min);
            SetPropertyExpression("max", max);
        }

        public LengthBetween(Expression<Func<T, int>> min , int max)
        {
             SetPropertyExpression("min", min);
            _max = max;
        }

         public LengthBetween(int min, Expression<Func<T, int>> max)
        {
             _min = min;
             SetPropertyExpression("max",max);
        }

        public override object[] Parameters
        {
            get { return new object[] { _min, _max }; }
        }

        public override bool Validate(RuleValidatorContext<T, string> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {   
            int length = String.IsNullOrEmpty(context.PropertyValue)? 0 : context.PropertyValue.Length;

            var contextWithLength = new RuleValidatorContext<T, string>(context.Instance, context.PropertyName, length.ToString(),
                                                                           context.PropertyInfo, context.Level, null);

            if (PropertyExpressions.ContainsKey("min"))
            {
                _min = (int)PropertyExpressions["min"].Invoke(context.Instance);
            }

            if (PropertyExpressions.ContainsKey("max"))
            {
                _max = (int)PropertyExpressions["max"].Invoke(context.Instance);
            }

            return Evaluate(length >= _min && length <= _max, contextWithLength, notification);
        }
    }
}