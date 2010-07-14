using SpecExpress.Rules;

namespace SpecExpress.RuleTree
{
    public class GroupNode<T, TProperty> : NodeBase<T, TProperty>
    {
        public GroupNode(NodeBase<T, TProperty> groupRoot)
        {
            GroupRoot = groupRoot;
        }

        public NodeBase<T, TProperty> GroupRoot { get; set; }
    }
}