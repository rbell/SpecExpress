using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.GeneralValidators
{
    public class IsInSet<T, TProperty> : RuleValidator<T,TProperty>
    {
        public IsInSet(IEnumerable<TProperty> set)
        {
            Params.Add(new RuleParameter("set", set));
        }

        public IsInSet(Func<T,IEnumerable<TProperty>> expression)
        {
            Params.Add(new RuleParameter("set", expression));
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            var set = (IEnumerable<TProperty>)Params[0].GetParamValue(context);

            return Evaluate(context.PropertyValue != null && set.Contains(context.PropertyValue), context, notification);
        }
    }
}