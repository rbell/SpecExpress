using SpecExpress.Rules;

namespace SpecExpress.RuleTree
{
    public class RuleNode<T, TProperty> : NodeBase<T, TProperty>
    {
        public RuleNode(RuleValidator<T, TProperty> rule)
        {
            Rule = rule;
        }

        public RuleValidator<T, TProperty> Rule { get; set; }
    }
}