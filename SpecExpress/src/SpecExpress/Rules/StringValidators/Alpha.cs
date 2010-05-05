using System;
using System.Text.RegularExpressions;

namespace SpecExpress.Rules.StringValidators
{
    public class Alpha<T> : RuleValidator<T, string>
    {
        public override object[] Parameters
        {
            get { return new object[] {}; }
        }

        public override ValidationResult Validate(RuleValidatorContext<T, string> context, SpecificationContainer specificationContainer)
        {   
            //Guard against null in Regex Match
            if (context.PropertyValue == null)
            {
                return Evaluate(false, context);
            }

            Match m = new Regex(@"^[a-zA-Z\s]+$").Match(context.PropertyValue);
            return Evaluate(m.Success, context);
        }
    }
}