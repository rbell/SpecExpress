using System;
using SpecExpress.Rules.IComparableValidators;

namespace SpecExpress.MVC.RuleRegistrations
{
    public class BetweenRegistration : RuleRegistration
    {
        public override Type RuleType
        {
            get { return typeof (Between<,>); }
        }

        public override RuleValidatorClientRuleMap ClientRuleMap
        {
            get
            {
                var map = new RuleValidatorClientRuleMap()
                {
                    JQueryRuleName = "specbetween"
                };

                // Map the jquery "maxlength" parameter to the specexpress parameter keyed by an empty string.
                map.Parameters.Add("floor", "floor");
                map.Parameters.Add("ceiling", "ceiling");
                return map;
            }
        }
    }
}