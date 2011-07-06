using System;
using System.Collections.Specialized;

namespace SpecExpress.Rules.Boolean
{
    public class IsTrue<T> : RuleValidator<T, bool> 
    {
        public override OrderedDictionary Parameters
        {
            get { return new OrderedDictionary() { }; }
        }

        public override bool Validate(RuleValidatorContext<T, bool> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            return Evaluate(context.PropertyValue, context, notification);
        }
    }
}