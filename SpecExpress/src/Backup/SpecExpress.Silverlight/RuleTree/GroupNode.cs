using SpecExpress.Rules;

namespace SpecExpress.RuleTree
{
    public class GroupNode : NodeBase
    {
        public GroupNode(NodeBase groupRoot)
        {
            GroupRoot = groupRoot;
        }

        public NodeBase GroupRoot { get; set; }
    }
}