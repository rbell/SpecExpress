using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.IComparableValidators
{
    public class GreaterThanEqualTo<T, TProperty> : RuleValidator<T, TProperty>
    {
        public GreaterThanEqualTo(TProperty greaterThanEqualTo)
        {
            Params.Add(new RuleParameter("greaterThanEqualTo", greaterThanEqualTo));
        }

        public GreaterThanEqualTo(Expression<Func<T, TProperty>> expression)
        {
            Params.Add(new RuleParameter("greaterThanEqualTo", expression));
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
       {
            var greaterThanEqualTo = (TProperty)Params[0].GetParamValue(context);
            
            Comparer<TProperty> comparer = System.Collections.Generic.Comparer<TProperty>.Default;

            return Evaluate(comparer.Compare(context.PropertyValue, greaterThanEqualTo) >= 0, context, notification);
        }

    }
}