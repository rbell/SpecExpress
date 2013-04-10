using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.Collection
{
    public class Contains<T, TProperty> : RuleValidator<T, TProperty> where TProperty:IEnumerable
    {
        public Contains(object contains) : base()
        {
            Params.Add(new RuleParameter("contains", contains));
        }

        public Contains(Expression<Func<T, object>> expression)
            : base()
        {
            Params.Add(new RuleParameter("contains", expression));
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, ISpecificationContainer specificationContainer, ValidationNotification notification)
        {
            bool contains = false;

            var containsParamVal = Params[0].GetParamValue(context);

            foreach (var value in context.PropertyValue)
            {
                if (value.Equals(containsParamVal))
                {
                    contains = true;
                    break;
                }
            }

            return Evaluate(contains, context, notification);
        }
    }
}