using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.IComparableValidators
{
    public class GreaterThan<T, TProperty> : RuleValidator<T, TProperty> 
    {
        public GreaterThan(TProperty greaterThan)
        {
            Params.Add(new RuleParameter("greaterThan", greaterThan));
        }

        public GreaterThan(Expression<Func<T, TProperty>> expression)
        {
            Params.Add(new RuleParameter("greaterThan", expression));
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            var greaterThan = (TProperty)Params[0].GetParamValue(context);

            Comparer<TProperty> comparer = System.Collections.Generic.Comparer<TProperty>.Default;
            return Evaluate( comparer.Compare(context.PropertyValue, greaterThan)  > 0, context, notification);
        }
    }
}