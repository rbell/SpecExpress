using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace SpecExpress.Rules.StringValidators
{
    public class Numeric<T> : RuleValidator<T, string>
    {
        public override bool Validate(RuleValidatorContext<T, string> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {   
            //Guard against null in Regex Match
            if (context.PropertyValue == null)
            {
                return Evaluate(false, context, notification);
            }

            //use a regex over Convert.ToInt32 because the string could potentially be bigger than an integer
            Match m = new Regex(@"^\d+$").Match(context.PropertyValue);
            return Evaluate(m.Success, context, notification);
        }
    }
}