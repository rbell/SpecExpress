using SpecExpress.Rules;

namespace SpecExpress.RuleTree
{
    public class RuleNode : NodeBase
    {
        public RuleNode(RuleValidator rule)
        {
            Rule = rule;
        }

        public RuleValidator Rule { get; set; }
    }
}