using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.IComparableValidators
{
    public class EqualTo<T, TProperty> : RuleValidator<T, TProperty> 
    {
        public EqualTo(TProperty equalTo)
        {
            Params.Add(new RuleParameter("equalTo", equalTo));
        }

        public EqualTo(Expression<Func<T, TProperty>> expression)
        {
            Params.Add(new RuleParameter("equalTo", expression));
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            var equalTo = (TProperty)Params[0].GetParamValue(context);

            Comparer<TProperty> comparer = System.Collections.Generic.Comparer<TProperty>.Default;

            return Evaluate(comparer.Compare(context.PropertyValue, equalTo) == 0, context, notification);
        }

    }
}