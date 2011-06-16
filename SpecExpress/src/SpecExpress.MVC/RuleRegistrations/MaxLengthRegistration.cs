using System;
using SpecExpress.Rules.StringValidators;

namespace SpecExpress.MVC.RuleRegistrations
{
    public class MaxLengthRegistration : RuleRegistration
    {
        public override Type RuleType
        {
            get { return typeof(MaxLength<>); }
        }

        public override RuleValidatorClientRuleMap ClientRuleMap
        {
            get
            {
                var map = new RuleValidatorClientRuleMap()
                {
                    JQueryRuleName = "specmaxlength"
                };

                // Map the jquery "maxlength" parameter to the specexpress parameter keyed by an empty string.
                map.Parameters.Add("maxlength", "");
                return map;
            }
        }

    }
}