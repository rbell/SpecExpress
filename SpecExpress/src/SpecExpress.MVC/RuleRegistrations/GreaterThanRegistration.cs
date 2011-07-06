using System;
using SpecExpress.Rules.IComparableValidators;

namespace SpecExpress.MVC.RuleRegistrations
{
    public class GreaterThanRegistration : RuleRegistration
    {
        public override Type RuleType
        {
            get { return typeof (GreaterThan<,>); }
        }

        public override RuleValidatorClientRuleMap ClientRuleMap
        {
            get
            {
                var map = new RuleValidatorClientRuleMap()
                {
                    JQueryRuleName = "specgreaterthan"
                };

                // Map the jquery "maxlength" parameter to the specexpress parameter keyed by an empty string.
                map.Parameters.Add("greaterthan", "");
                return map;
            }
        }
    }
}