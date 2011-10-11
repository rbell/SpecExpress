using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.Collection
{
    public class CountEqualTo<T, TProperty> : RuleValidator<T, TProperty> where TProperty : IEnumerable
    {
        public CountEqualTo(int countEquals)
        {
            Params.Add(new RuleParameter("countEquals", countEquals));
        }

        public CountEqualTo(Expression<Func<T, int>> expression)
        {
            Params.Add(new RuleParameter("countEquals", expression));
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            var countEquals = (int)Params[0].GetParamValue(context);

            int collectionCount = 0;
            if (context.PropertyValue != null)
            {
                foreach (var value in context.PropertyValue)
                {
                    collectionCount++;
                }
            }

            return Evaluate(collectionCount == countEquals, context, notification);
        }
    }
}