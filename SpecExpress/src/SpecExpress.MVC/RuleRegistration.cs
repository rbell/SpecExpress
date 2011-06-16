using System;

namespace SpecExpress.MVC
{
    /// <summary>
    /// Defines a mapping for a given SpecExpress Rule to a Client side enforce rule
    /// </summary>
    public abstract class RuleRegistration
    {
        public abstract Type RuleType { get; } 
        public abstract RuleValidatorClientRuleMap ClientRuleMap { get; }
    }

}