using System;

namespace SpecExpress.Rules.Boolean
{
    public class IsTrue<T> : RuleValidator<T, bool> 
    {
        public override object[] Parameters
        {
            get { return new object[]{}; }
        }

        public override bool Validate(RuleValidatorContext<T, bool> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            return Evaluate(context.PropertyValue, context, notification);
        }
    }
}