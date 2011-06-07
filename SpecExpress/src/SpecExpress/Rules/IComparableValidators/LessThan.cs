using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.IComparableValidators
{
    public class LessThan<T, TProperty> : RuleValidator<T, TProperty> 
    {
        private TProperty _lessThan;

        public LessThan(TProperty greaterThan)
        {
            _lessThan = greaterThan;
        }

        public LessThan(Expression<Func<T, TProperty>> expression)
        {
            SetPropertyExpression(expression);
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            if (PropertyExpressions.Any())
            {
                _lessThan = (TProperty)GetExpressionValue(context);
            }

            Comparer<TProperty> comparer = System.Collections.Generic.Comparer<TProperty>.Default;
            return Evaluate(comparer.Compare(context.PropertyValue, _lessThan) < 0, context, notification);
        }

        public override OrderedDictionary Parameters
        {
            get { return new OrderedDictionary() {{"", _lessThan }}; }
        }
    }
}