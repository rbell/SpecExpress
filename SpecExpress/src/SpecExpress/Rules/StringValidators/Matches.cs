using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace SpecExpress.Rules.StringValidators
{
    public class Matches<T> : RuleValidator<T,string>
    {
        private string _regexPattern;

        public Matches(string regexPattern)
        {
            _regexPattern = regexPattern;
        }

        public Matches(Expression<Func<T, string>> regexPattern)
        {
            SetPropertyExpression(regexPattern);
        }

        public override object[] Parameters
        {
            get { return new string[] {_regexPattern}; }
        }

        public override bool Validate(RuleValidatorContext<T, string> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            if (PropertyExpressions.Any())
            {
                _regexPattern = (string) GetExpressionValue(context);
            }

            Regex regex = new Regex(_regexPattern);
            bool isMatch = regex.IsMatch(context.PropertyValue);
            return Evaluate(isMatch, context, notification);
        }
    }
}