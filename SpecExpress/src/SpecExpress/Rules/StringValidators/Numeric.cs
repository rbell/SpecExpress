using System;
using System.Text.RegularExpressions;

namespace SpecExpress.Rules.StringValidators
{
    public class Numeric<T> : RuleValidator<T, string>
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

            //use a regex over Convert.ToInt32 because the string could potentially be bigger than an integer
            Match m = new Regex(@"^\d+$").Match(context.PropertyValue);
            return Evaluate(m.Success, context);
        }
    }
}