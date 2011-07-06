using System;
using SpecExpress.Rules.StringValidators;

namespace SpecExpress.MVC.RuleRegistrations
{
    public class LengthEqualToRegistration : RuleRegistration
    {
        public override Type RuleType
        {
            get { return typeof (LengthEqualTo<>); }
        }

        public override RuleValidatorClientRuleMap ClientRuleMap
        {
            get
            {
                var map = new RuleValidatorClientRuleMap()
                           {
                               JQueryRuleName = "speclengthequalto"
                           };
                // Map the jquery "minlength" parameter to the specexpress parameter keyed by an empty string.
                map.Parameters.Add("length", "");
                return map;
            }
        }
    }
}