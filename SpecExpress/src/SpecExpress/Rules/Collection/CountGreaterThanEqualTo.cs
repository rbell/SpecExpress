using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.Collection
{
    public class CountGreaterThanEqualTo<T, TProperty> : RuleValidator<T, TProperty> where TProperty : IEnumerable
    {
        private int _countGreaterThanEqualTo;

        public CountGreaterThanEqualTo(int countGreaterThanEqualTo)
        {
            _countGreaterThanEqualTo = countGreaterThanEqualTo;
        }

        public CountGreaterThanEqualTo(Expression<Func<T, int>> expression)
        {
            SetPropertyExpression(expression);
        }

        public override object[] Parameters
        {
            get { return new object[] { _countGreaterThanEqualTo }; }
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            if (PropertyExpressions.Any())
            {
                _countGreaterThanEqualTo = (int)GetExpressionValue(context);
            }

            int collectionCount = 0;
            if (context.PropertyValue != null)
            {
                foreach (var value in context.PropertyValue)
                {
                    collectionCount++;
                }
            }

            return Evaluate(collectionCount >= _countGreaterThanEqualTo, context, notification);
        }
    }
}