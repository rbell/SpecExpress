using System;
using System.Collections.Specialized;

namespace SpecExpress.Rules.Boolean
{
    public class IsFalse<T> : RuleValidator<T, bool>
    {
        public override bool Validate(RuleValidatorContext<T, bool> context, ISpecificationContainer specificationContainer, ValidationNotification notification)
        {
            return Evaluate(!context.PropertyValue, context, notification);
        }
    }
}