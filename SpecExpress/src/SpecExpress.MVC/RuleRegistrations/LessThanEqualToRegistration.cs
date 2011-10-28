using System;
using SpecExpress.Rules.IComparableValidators;

namespace SpecExpress.MVC.RuleRegistrations
{
    public class LessThanEqualToRegistration : RuleRegistration
    {
        public override Type RuleType
        {
            get { return typeof(LessThanEqualTo<,>); }
        }

        public override RuleValidatorClientRuleMap ClientRuleMap
        {
            get
            {
                var map = new RuleValidatorClientRuleMap()
                {
                    JQueryRuleName = "speclessthanequalto"
                };

                // Map the jquery "maxlength" parameter to the specexpress parameter keyed by an empty string.
                map.Parameters.Add("lessthanequalto", "lessThanEqualTo");
                return map;
            }
        }
         
    }
}