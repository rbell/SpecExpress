using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace SpecExpress.Rules
{
    public class CustomRule<T,TProperty> : RuleValidator<T, TProperty>
    {
        private Func<T,TProperty,bool> _expression;

        public CustomRule(Func<T, TProperty, bool> rule)
        {
            _expression = rule;
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            var result = (bool)(_expression.DynamicInvoke(new object[] {context.Instance, context.PropertyValue }));

            return Evaluate(result, context, notification);
        }
    }
}
