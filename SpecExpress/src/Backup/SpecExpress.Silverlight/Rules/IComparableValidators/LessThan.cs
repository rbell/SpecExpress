using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.IComparableValidators
{
    public class LessThan<T, TProperty> : RuleValidator<T, TProperty> 
    {
        public LessThan(TProperty lessThan)
        {
            Params.Add(new RuleParameter("lessThan", lessThan));
        }

        public LessThan(Expression<Func<T, TProperty>> expression)
        {
            Params.Add(new RuleParameter("lessThan", expression));
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            var lessThan = (TProperty)Params[0].GetParamValue(context);

            Comparer<TProperty> comparer = System.Collections.Generic.Comparer<TProperty>.Default;
            return Evaluate(comparer.Compare(context.PropertyValue, lessThan) < 0, context, notification);
        }

    }
}