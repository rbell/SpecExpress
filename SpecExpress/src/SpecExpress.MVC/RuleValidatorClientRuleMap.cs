using System.Collections.Generic;

namespace SpecExpress.MVC
{
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