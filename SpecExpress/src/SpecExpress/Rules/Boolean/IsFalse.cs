using System;

namespace SpecExpress.Rules.Boolean
{
    public class IsFalse<T> : RuleValidator<T, bool>
    {
        public override object[] Parameters
        {
            get { return new object[] { }; }
        }

        public override ValidationResult Validate(RuleValidatorContext<T, bool> context, SpecificationContainer specificationContainer)
        {
            return Evaluate(!context.PropertyValue, context);
        }
    }
}