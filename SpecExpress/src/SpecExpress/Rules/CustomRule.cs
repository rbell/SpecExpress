using System;
using System.Text.RegularExpressions;

namespace SpecExpress.Rules
{
    public class CustomRule<T,TProperty> : RuleValidator<T, TProperty>
    {
        private Func<T,TProperty,bool> _expression;
        public override object[] Parameters
        {
            get { return new object[] {}; }
        }

        public CustomRule(Func<T, TProperty, bool> rule)
        {
            _expression = rule;
        }

        public override ValidationResult Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer)
        {
            var result = (bool)(_expression.DynamicInvoke(new object[] {context.Instance, context.PropertyValue }));

            return Evaluate(result, context);
        }
    }
}
