using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.Collection
{
    public class CountLessThan<T, TProperty> : RuleValidator<T, TProperty> where TProperty : IEnumerable
    {
        public CountLessThan(int countLessThan)
        {
            Params.Add(new RuleParameter("countLessThan", countLessThan));
        }

        public CountLessThan(Expression<Func<T, int>> expression)
        {
            Params.Add(new RuleParameter("countLessThan", expression));
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, ISpecificationContainer specificationContainer, ValidationNotification notification)
        {
            var countLessThan = (int)Params[0].GetParamValue(context);

            int collectionCount = 0;
            if (context.PropertyValue != null)
            {
                foreach (var value in context.PropertyValue)
                {
                    collectionCount++;
                }
            }

            return Evaluate(collectionCount < countLessThan, context, notification);
        }
    }
}