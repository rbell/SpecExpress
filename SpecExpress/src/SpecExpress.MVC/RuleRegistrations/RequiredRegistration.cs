using System;
using SpecExpress.Rules.GeneralValidators;

namespace SpecExpress.MVC.RuleRegistrations
{
    public class RequiredRegistration : RuleRegistration
    {
        public override Type RuleType
        {
            get { return typeof (Required<,>); }
        }

        public override RuleValidatorClientRuleMap ClientRuleMap
        {
            get { return new RuleValidatorClientRuleMap() {JQueryRuleName = "specrequired"}; }
        }
    }
}