using System;
using SpecExpress.Rules.GeneralValidators;
using SpecExpress.Rules.StringValidators;

namespace SpecExpress.MVC.RuleRegistrations
{
    public class AlphaRegistration : RuleRegistration
    {
        public override Type RuleType
        {
            get { return typeof(Alpha<>); }
        }

        public override RuleValidatorClientRuleMap ClientRuleMap
        {
            get { return new RuleValidatorClientRuleMap() {JQueryRuleName = "specalpha"}; }
        }
    }
}