using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.IComparableValidators
{
    public class LessThanEqualTo<T, TProperty> : RuleValidator<T, TProperty> 
    {
        public LessThanEqualTo(TProperty lessThanEqualTo)
        {
            Params.Add(new RuleParameter("lessThanEqualTo", lessThanEqualTo));
        }

        public LessThanEqualTo(Expression<Func<T, TProperty>> expression)
        {
            Params.Add(new RuleParameter("lessThanEqualTo", expression));
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, ISpecificationContainer specificationContainer, ValidationNotification notification)
        {
            var lessThanEqualTo = (TProperty)Params[0].GetParamValue(context);

            Comparer<TProperty> comparer = System.Collections.Generic.Comparer<TProperty>.Default;
            return Evaluate(comparer.Compare(context.PropertyValue, lessThanEqualTo) <= 0, context, notification);
        }
    }
}