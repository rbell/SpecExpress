using System;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace SpecExpress.Rules.StringValidators
{
    public class Matches<T> : RuleValidator<T,string>
    {
        public Matches(string regexPattern)
        {
            Params.Add(new RuleParameter("regexPattern", regexPattern));
        }

        public Matches(Expression<Func<T, string>> regexPattern)
        {
            Params.Add(new RuleParameter("regexPattern", regexPattern));
        }


        public override bool Validate(RuleValidatorContext<T, string> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            var regexPattern = (string)Params[0].GetParamValue(context);

            Regex regex = new Regex(regexPattern);
            bool isMatch = regex.IsMatch(context.PropertyValue);
            return Evaluate(isMatch, context, notification);
        }
    }
}