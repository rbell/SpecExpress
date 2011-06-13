using System;

namespace SpecExpress.MVC
{
    public abstract class RuleRegistration
    {
        public abstract Type RuleType { get; } 
        public abstract RuleValidatorClientRuleMap ClientRuleMap { get; }
    }

}