using System;
using SpecExpress.Rules.StringValidators;

namespace SpecExpress.MVC.RuleRegistrations
{
    public class LengthBetweenRegistration : RuleRegistration
    {
        public override Type RuleType
        {
            get { return typeof (LengthBetween<>); }
        }

        public override RuleValidatorClientRuleMap ClientRuleMap
        {
            get
            {
                var map = new RuleValidatorClientRuleMap()
                {
                    JQueryRuleName = "speclengthbetween"
                };

                // Map the jquery "maxlength" parameter to the specexpress parameter keyed by an empty string.
                map.Parameters.Add("minlength", "min");
                map.Parameters.Add("maxlength", "max");
                return map;
            }
        }
    }
}