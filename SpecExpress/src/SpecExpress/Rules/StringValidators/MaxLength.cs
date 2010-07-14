using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SpecExpress.Rules.StringValidators
{   
    public class MaxLength<T> : RuleValidator<T, string>
    {
        private int _max;

        public MaxLength(int max)
        {
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException("max", "Max should be greater than 0");
            }
            _max = max;
        }

        public MaxLength(Expression<Func<T, int>> expression)
        {
            SetPropertyExpression(expression);
        }


        public override object[] Parameters
        {
            get { return new object[] { _max}; }
        }

        public override bool Validate(RuleValidatorContext<T, string> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            int length = String.IsNullOrEmpty(context.PropertyValue) ? 0 : context.PropertyValue.Length;

            var contextWithLength = new RuleValidatorContext<T, string>(context.Instance, context.PropertyName, length.ToString(),
                                                                           context.PropertyInfo, context.Level, null);
            
            if (PropertyExpressions.Any())
            {
                //Get value manually because Type of TProperty is int instead of string
                _max = (int)PropertyExpressions.First().Value.Invoke(context.Instance);
            }

            return Evaluate(length <= _max, contextWithLength, notification);
        }
    }
}
