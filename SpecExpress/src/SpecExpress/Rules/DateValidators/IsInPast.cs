using System;

namespace SpecExpress.Rules.DateValidators
{
    public class IsInPast<T> : RuleValidator<T, DateTime>
    {
        public override object[] Parameters
        {
            get { return new object[]{}; }
        }

        public override bool Validate(RuleValidatorContext<T, DateTime> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            return Evaluate(context.PropertyValue < DateTime.Now, context, notification);
        }
    }

    public class IsInPastNullable<T> : RuleValidator<T, System.Nullable<DateTime>>
    {
        public override object[] Parameters
        {
            get { return new object[] { }; }
        }

        public override bool Validate(RuleValidatorContext<T, DateTime?> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            return Evaluate(!context.PropertyValue.HasValue || context.PropertyValue < DateTime.Now, context, notification);
        }
    }

}