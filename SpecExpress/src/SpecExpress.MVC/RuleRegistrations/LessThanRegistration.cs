using System;
using SpecExpress.Rules.IComparableValidators;

namespace SpecExpress.MVC.RuleRegistrations
{
    public class LessThanRegistration : RuleRegistration
    {
        public override Type RuleType
        {
            get { return typeof(LessThan<,>); }
        }

        public override RuleValidatorClientRuleMap ClientRuleMap
        {
            get
            {
                var map = new RuleValidatorClientRuleMap()
                {
                    JQueryRuleName = "speclessthan"
                };

                // Map the jquery "maxlength" parameter to the specexpress parameter keyed by an empty string.
                map.Parameters.Add("lessthan", "lessThan");
                return map;
            }
        }
         
    }
}