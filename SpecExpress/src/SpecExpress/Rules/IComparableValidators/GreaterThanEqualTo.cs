using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.IComparableValidators
{
    public class GreaterThanEqualTo<T, TProperty> : RuleValidator<T, TProperty>
    {
        private TProperty _greaterThanEqualTo;

        public GreaterThanEqualTo(TProperty greaterThanEqualTo)
        {
            _greaterThanEqualTo = greaterThanEqualTo;
        }

        public GreaterThanEqualTo(Expression<Func<T, TProperty>> expression)
        {
            SetPropertyExpression(expression);
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            if (PropertyExpressions.Any())
            {
                _greaterThanEqualTo = (TProperty)GetExpressionValue(context);
            }

            Comparer<TProperty> comparer = System.Collections.Generic.Comparer<TProperty>.Default;

            return Evaluate(comparer.Compare(context.PropertyValue, _greaterThanEqualTo) >= 0, context, notification);
        }

        public override object[] Parameters
        {
            get { return new object[] { _greaterThanEqualTo }; }
        }
    }
}