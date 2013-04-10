using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.Collection
{
    public class CountGreaterThanEqualTo<T, TProperty> : RuleValidator<T, TProperty> where TProperty : IEnumerable
    {
        public CountGreaterThanEqualTo(int countGreaterThanEqualTo)
        {
            Params.Add(new RuleParameter("countGreaterThanEqualTo", countGreaterThanEqualTo));
        }

        public CountGreaterThanEqualTo(Expression<Func<T, int>> expression)
        {
            Params.Add(new RuleParameter("countGreaterThanEqualTo", expression));
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, ISpecificationContainer specificationContainer, ValidationNotification notification)
        {
            var countGreaterThanEqualTo = (int)Params[0].GetParamValue(context);

            int collectionCount = 0;
            if (context.PropertyValue != null)
            {
                foreach (var value in context.PropertyValue)
                {
                    collectionCount++;
                }
            }

            return Evaluate(collectionCount >= countGreaterThanEqualTo, context, notification);
        }
    }
}