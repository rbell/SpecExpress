using System;
using SpecExpress.Rules.IComparableValidators;

namespace SpecExpress.MVC.RuleRegistrations
{
    public class EqualToRegistration : RuleRegistration
    {
        public override Type RuleType
        {
            get { return typeof (EqualTo<,>); }
        }

        public override RuleValidatorClientRuleMap ClientRuleMap
        {
            get
            {
                var map = new RuleValidatorClientRuleMap()
                {
                    JQueryRuleName = "specequalto"
                };

                // Map the jquery "maxlength" parameter to the specexpress parameter keyed by an empty string.
                map.Parameters.Add("equalto", "");
                return map;
            }
        }
    }
}