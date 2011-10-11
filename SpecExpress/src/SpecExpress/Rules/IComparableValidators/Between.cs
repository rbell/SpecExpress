using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.IComparableValidators
{
    public class Between<T, TProperty> : RuleValidator<T, TProperty>
    {
        public Between(TProperty floor, TProperty ceiling) : base()
        {
            Params.Add(new RuleParameter("floor",floor));
            Params.Add(new RuleParameter("ceiling",ceiling));
        }

        public Between(Expression<Func<T, TProperty>> floor, TProperty ceiling)
            : base()
        {
            Params.Add(new RuleParameter("floor",floor));
            Params.Add(new RuleParameter("ceiling",ceiling));
        }

        public Between(TProperty floor, Expression<Func<T, TProperty>> ceiling)
            : base()
        {
            Params.Add(new RuleParameter("floor", floor));
            Params.Add(new RuleParameter("ceiling", ceiling));
        }

        public Between(Expression<Func<T, TProperty>> floor, Expression<Func<T, TProperty>> ceiling)
            : base()
        {
            Params.Add(new RuleParameter("floor", floor));
            Params.Add(new RuleParameter("ceiling", ceiling));
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            var floor = (TProperty)Params[0].GetParamValue(context);
            var ceiling = (TProperty)Params[1].GetParamValue(context);
            Comparer<TProperty> comparer = System.Collections.Generic.Comparer<TProperty>.Default;
           
            return Evaluate(comparer.Compare(context.PropertyValue, ceiling) <= 0 && comparer.Compare(context.PropertyValue, floor) >= 0 , context, notification);
        }
    }
}