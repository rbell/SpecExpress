using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace SpecExpress.Rules.StringValidators
{
    public class Alpha<T> : RuleValidator<T, string>
    {
        public override bool Validate(RuleValidatorContext<T, string> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {   
            //Guard against null in Regex Match
            if (context.PropertyValue == null)
            {
                return Evaluate(false, context, notification);
            }

            Match m = new Regex(@"^[a-zA-Z\s]+$").Match(context.PropertyValue);
            return Evaluate(m.Success, context, notification);
        }
    }
}