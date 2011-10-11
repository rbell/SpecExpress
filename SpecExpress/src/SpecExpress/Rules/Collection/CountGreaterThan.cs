using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.Collection
{
    public class CountGreaterThan<T, TProperty> : RuleValidator<T, TProperty> where TProperty : IEnumerable
    {

        public CountGreaterThan(int countGreaterThan)
        {
            Params.Add(new RuleParameter("countGreaterThan", countGreaterThan));
        }

        public CountGreaterThan(Expression<Func<T, int>> expression)
        {
            Params.Add(new RuleParameter("countGreaterThan", expression));
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
           var countGreaterThan = (int)Params[0].GetParamValue(context);

            int collectionCount = 0;
            if (context.PropertyValue != null)
            {
                foreach (var value in context.PropertyValue)
                {
                    collectionCount++;
                }
            }

            return Evaluate(collectionCount > countGreaterThan, context, notification);
        }
    }
}