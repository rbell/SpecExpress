using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SpecExpress.Rules.StringValidators
{   
    public class MinLength<T> : RuleValidator<T, string>
    {
        private int _min;

        public MinLength(int min)
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException("min", "Min should be greater than 0");
            }
            _min = min;
        }

        public MinLength(Expression<Func<T, int>> expression)
        {
            SetPropertyExpression(expression);
        }

        public override object[] Parameters
        {
            get { return new object[] { _min}; }
        }

        public override ValidationResult Validate(RuleValidatorContext<T, string> context, SpecificationContainer specificationContainer)
        {

            int length = String.IsNullOrEmpty(context.PropertyValue) ? 0 : context.PropertyValue.Trim().Length;

            var contextWithLength = new RuleValidatorContext<T, string>(context.Instance, context.PropertyName, length.ToString(),
                                                                           context.PropertyInfo, context.Level, null);

            if (PropertyExpressions.Any())
            {
                //Get value manually because Type of TProperty is int instead of string
                _min = (int)PropertyExpressions.First().Value.Invoke(context.Instance);
            }
            
            return Evaluate(length >= _min, contextWithLength);
        }
    }
}
