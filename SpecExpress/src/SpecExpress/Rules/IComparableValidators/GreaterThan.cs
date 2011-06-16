using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.IComparableValidators
{
    public class GreaterThan<T, TProperty> : RuleValidator<T, TProperty> 
    {
        private TProperty _greaterThan;

        public GreaterThan(TProperty greaterThan)
        {
            _greaterThan = greaterThan;
        }

        public GreaterThan(Expression<Func<T, TProperty>> expression)
        {
            SetPropertyExpression(expression);
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            if (PropertyExpressions.Any())
            {
                _greaterThan = (TProperty)GetExpressionValue(context);
            }

            Comparer<TProperty> comparer = System.Collections.Generic.Comparer<TProperty>.Default;
            return Evaluate( comparer.Compare(context.PropertyValue, _greaterThan)  > 0, context, notification);
        }

        public override OrderedDictionary Parameters
        {
            get { return new OrderedDictionary() {{"", _greaterThan}}; }
        }
    }
}