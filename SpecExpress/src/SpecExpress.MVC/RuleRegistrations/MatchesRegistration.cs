using System;
using SpecExpress.Rules.StringValidators;

namespace SpecExpress.MVC.RuleRegistrations
{
    public class MatchesRegistration : RuleRegistration
    {
        public override Type RuleType
        {
            get { return typeof (Matches<>); }
        }

        public override RuleValidatorClientRuleMap ClientRuleMap
        {
            get
            {
                var map = new RuleValidatorClientRuleMap()
                {
                    JQueryRuleName = "specmatches"
                };

                // Map the jquery "maxlength" parameter to the specexpress parameter keyed by an empty string.
                map.Parameters.Add("match", "");
                return map;
            }
        }
    }
}