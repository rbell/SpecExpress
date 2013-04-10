using System;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.StringValidators
{
    public class LengthBetween<T> : RuleValidator<T, string>
    {
        public LengthBetween(int min, int max)
        {
            if (max < min)
            {
                throw new ArgumentOutOfRangeException("max", "Max should be larger than min.");
            }

            Params.Add(new RuleParameter("min", min));
            Params.Add(new RuleParameter("max", max));
        }

        public LengthBetween(Expression<Func<T, int>> min, Expression<Func<T, int>> max)
        {
            Params.Add(new RuleParameter("min", min));
            Params.Add(new RuleParameter("max", max));
        }

        public LengthBetween(Expression<Func<T, int>> min , int max)
        {
            Params.Add(new RuleParameter("min", min));
            Params.Add(new RuleParameter("max", max));
        }

         public LengthBetween(int min, Expression<Func<T, int>> max)
        {
            Params.Add(new RuleParameter("min", min));
            Params.Add(new RuleParameter("max", max));
        }

        public override bool Validate(RuleValidatorContext<T, string> context, ISpecificationContainer specificationContainer, ValidationNotification notification)
        {   
            int length = String.IsNullOrEmpty(context.PropertyValue)? 0 : context.PropertyValue.Length;

            var contextWithLength = new RuleValidatorContext<T, string>(context.Instance, context.PropertyName, length.ToString(),
                                                                           context.PropertyInfo, context.Level, null);

            var min = (int)Params[0].GetParamValue(context);
            var max = (int)Params[1].GetParamValue(context);

            return Evaluate(length >= min && length <= max, contextWithLength, notification);
        }
    }
}