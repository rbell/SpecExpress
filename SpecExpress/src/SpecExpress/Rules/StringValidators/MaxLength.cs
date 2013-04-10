using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SpecExpress.Rules.StringValidators
{
    public class MaxLength<T> : RuleValidator<T, string>
    {
        public MaxLength(int max)
        {
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException("max", "Max should be greater than 0");
            }
            Params.Add(new RuleParameter("max", max));
        }

        public MaxLength(Expression<Func<T, int>> expression)
        {
            Params.Add(new RuleParameter("max", expression));
        }

        public override bool Validate(RuleValidatorContext<T, string> context, ISpecificationContainer specificationContainer, ValidationNotification notification)
        {
            int length = String.IsNullOrEmpty(context.PropertyValue) ? 0 : context.PropertyValue.Length;

            var contextWithLength = new RuleValidatorContext<T, string>(context.Instance, context.PropertyName, length.ToString(),
                                                                           context.PropertyInfo, context.Level, null);

            var max = (int)Params[0].GetParamValue(context);

            return Evaluate(length <= max, contextWithLength, notification);
        }
    }
}
