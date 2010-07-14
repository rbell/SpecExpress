using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.GeneralValidators
{
    public class IsInSet<T, TProperty> : RuleValidator<T,TProperty>
    {
        private Func<T,IEnumerable<TProperty>> _expression;
        private IEnumerable<TProperty> _set;

        public IsInSet(IEnumerable<TProperty> set)
        {
            _set = set;
        }

        public IsInSet(Func<T,IEnumerable<TProperty>> expression)
        {
            _expression = expression;
        }

        public override object[] Parameters
        {
            get { return new object[] {}; }
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            if (_expression != null)
            {
                _set = _expression.Invoke(context.Instance);                
            }

            return Evaluate(context.PropertyValue != null && _set.Contains(context.PropertyValue), context, notification);
        }
    }
}