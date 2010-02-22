using System;

namespace SpecExpress.Rules.DateValidators
{
    public class IsInFuture<T> : RuleValidator<T, DateTime>
    {
        public override object[] Parameters
        {
            get { return new object[]{}; }
        }

        public override ValidationResult Validate(RuleValidatorContext<T, DateTime> context, SpecificationContainer specificationContainer)
        {
            return Evaluate(context.PropertyValue > DateTime.Now, context);
        }
    }

    public class IsInFutureNullable<T> : RuleValidator<T, System.Nullable<DateTime>>
    {
        public override object[] Parameters
        {
            get { return new object[] { }; }
        }

        public override ValidationResult Validate(RuleValidatorContext<T, DateTime?> context, SpecificationContainer specificationContainer)
        {
            return Evaluate(!context.PropertyValue.HasValue || context.PropertyValue.Value > DateTime.Now, context);
        }
    }

}