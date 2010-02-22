using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.Collection
{
    public class CountLessThanEqualTo<T, TProperty> : RuleValidator<T, TProperty> where TProperty : IEnumerable
    {
        private int _countLessThanEqualTo;

        public CountLessThanEqualTo(int countLessThanEqualTo)
        {
            _countLessThanEqualTo = countLessThanEqualTo;
        }

        public CountLessThanEqualTo(Expression<Func<T, int>> expression)
        {
            SetPropertyExpression(expression);
        }

        public override object[] Parameters
        {
            get { return new object[] { _countLessThanEqualTo }; }
        }

        public override ValidationResult Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer)
        {
            if (PropertyExpressions.Any())
            {
                _countLessThanEqualTo = (int)GetExpressionValue(context);
            }

            int collectionCount = 0;
            if (context.PropertyValue != null)
            {
                foreach (var value in context.PropertyValue)
                {
                    collectionCount++;
                }
            }

            return Evaluate(collectionCount <= _countLessThanEqualTo, context);
        }
    }
}