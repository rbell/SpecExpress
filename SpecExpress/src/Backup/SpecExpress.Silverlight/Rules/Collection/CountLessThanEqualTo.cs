using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.Collection
{
    public class CountLessThanEqualTo<T, TProperty> : RuleValidator<T, TProperty> where TProperty : IEnumerable
    {
        public CountLessThanEqualTo(int countLessThanEqualTo)
        {
            Params.Add(new RuleParameter("countLessThanEqualTo", countLessThanEqualTo));
        }

        public CountLessThanEqualTo(Expression<Func<T, int>> expression)
        {
            Params.Add(new RuleParameter("countLessThanEqualTo", expression));
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            var countLessThanEqualTo = (int)Params[0].GetParamValue(context);

            int collectionCount = 0;
            if (context.PropertyValue != null)
            {
                foreach (var value in context.PropertyValue)
                {
                    collectionCount++;
                }
            }

            return Evaluate(collectionCount <= countLessThanEqualTo, context, notification);
        }
    }
}