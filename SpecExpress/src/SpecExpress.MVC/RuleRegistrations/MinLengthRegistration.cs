using System;
using SpecExpress.Rules.StringValidators;

namespace SpecExpress.MVC.RuleRegistrations
{
    public class MinLengthRegistration : RuleRegistration
    {
        public override Type RuleType
        {
            get { return typeof (MinLength<>); }
        }

        public override RuleValidatorClientRuleMap ClientRuleMap
        {
            get
            {
                var map = new RuleValidatorClientRuleMap()
                           {
                               JQueryRuleName = "specminlength"
                           };
                // Map the jquery "minlength" parameter to the specexpress parameter keyed by an empty string.
                map.Parameters.Add("minlength", "");
                return map;
            }
        }
    }
}