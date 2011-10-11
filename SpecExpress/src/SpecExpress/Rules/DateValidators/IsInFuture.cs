using System;
using System.Collections.Specialized;

namespace SpecExpress.Rules.DateValidators
{
    public class IsInFuture<T> : RuleValidator<T, DateTime>
    {
        public override bool Validate(RuleValidatorContext<T, DateTime> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            return Evaluate(context.PropertyValue > DateTime.Now, context, notification);
        }
    }

    public class IsInFutureNullable<T> : RuleValidator<T, System.Nullable<DateTime>>
    {
        public override bool Validate(RuleValidatorContext<T, DateTime?> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            return Evaluate(!context.PropertyValue.HasValue || context.PropertyValue.Value > DateTime.Now, context, notification);
        }
    }

}