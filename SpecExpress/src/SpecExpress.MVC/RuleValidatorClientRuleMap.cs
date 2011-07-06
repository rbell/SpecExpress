using System.Collections.Generic;

namespace SpecExpress.MVC
{
    /// <summary>
    /// Defines how a SpecExpress Rule is mapped to a Client Side Rule - inlcuding any parameters.
    /// </summary>
    public class RuleValidatorClientRuleMap
    {
        public RuleValidatorClientRuleMap()
        {
            Parameters = new Dictionary<string, string>();
        }

        public string JQueryRuleName { get; set; }
        public Dictionary<string, string > Parameters { get; set; }
        
    }
}