using System;
using SpecExpress.Rules.GeneralValidators;
using SpecExpress.Rules.StringValidators;

namespace SpecExpress.MVC.RuleRegistrations
{
    public class NumericRegistration : RuleRegistration
    {
        public override Type RuleType
        {
            get { return typeof(Numeric<>); }
        }

        public override RuleValidatorClientRuleMap ClientRuleMap
        {
            get { return new RuleValidatorClientRuleMap() { JQueryRuleName = "specnumeric" }; }
        }
    }
}