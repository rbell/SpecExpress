using System;
using SpecExpress.Rules.IComparableValidators;

namespace SpecExpress.MVC.RuleRegistrations
{
    public class GreaterThanEqualToRegistration : RuleRegistration
    {
        public override Type RuleType
        {
            get { return typeof (GreaterThanEqualTo<,>); }
        }

        public override RuleValidatorClientRuleMap ClientRuleMap
        {
            get
            {
                var map = new RuleValidatorClientRuleMap()
                {
                    JQueryRuleName = "specgreaterthanequalto"
                };

                // Map the jquery "maxlength" parameter to the specexpress parameter keyed by an empty string.
                map.Parameters.Add("greaterthanequalto", "greaterThanEqualTo");
                return map;
            }
        }
    }
}