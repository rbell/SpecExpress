using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.Collection
{
    public class CountGreaterThan<T, TProperty> : RuleValidator<T, TProperty> where TProperty : IEnumerable
    {
        private int _countGreaterThan;

        public CountGreaterThan(int countGreaterThan)
        {
            _countGreaterThan = countGreaterThan;
        }

        public CountGreaterThan(Expression<Func<T, int>> expression)
        {
            SetPropertyExpression(expression);
        }

        public override object[] Parameters
        {
            get { return new object[] {_countGreaterThan}; }
        }

        public override ValidationResult Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer)
        {
            if (PropertyExpressions.Any())
            {
                _countGreaterThan = (int)GetExpressionValue(context);
            }

            int collectionCount = 0;
            if (context.PropertyValue != null)
            {
                foreach (var value in context.PropertyValue)
                {
                    collectionCount++;
                }
            }

            return Evaluate(collectionCount > _countGreaterThan, context);
        }
    }
}