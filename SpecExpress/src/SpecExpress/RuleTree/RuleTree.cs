using System;
using SpecExpress.Rules;

namespace SpecExpress.RuleTree
{
    /// <summary>
    /// Tree structure that describes RuleValidators and their relations to one another (i.e. And / Or)
    /// </summary>
    public class RuleTree<T, TProperty>
    {
        private NodeBase<T, TProperty> _root;
        private Func<RuleValidatorContext<T, TProperty>, SpecificationContainer, ValidationNotification, bool> _lambda;

        public RuleTree()
        {
        }

        public RuleTree(NodeBase<T, TProperty> rootNode)
        {
            Root = rootNode;
        }

        public NodeBase<T, TProperty> Root
        {
            get { return _root; }
            set
            {
                _root = value;
                _root.NodeAltered += new EventHandler(_root_NodeAltered);
            }
        }

        public NodeBase<T, TProperty> EndNode
        {
            get
            {
                if (Root == null)
                {
                    return null;
                }

                return getEndNode(Root);
            }
        }

        public RuleNode<T, TProperty> LastRuleNode
        {
            get
            {
                if (Root == null)
                {
                    return null;
                }

                return getLastRuleNode(Root);
            }
        }

        public Func<RuleValidatorContext<T, TProperty>, SpecificationContainer, ValidationNotification, bool> LambdaExpression
        {
            get
            {
                if (_lambda == null)
                {
                    _lambda = new RuleExpressionFactory<T, TProperty>().CreateExpression(this);
                }
                return _lambda;
            }
        }

        private void _root_NodeAltered(object sender, EventArgs e)
        {
            // Force recalculation of LambaExpression
            _lambda = null;
        }

        private NodeBase<T, TProperty> getEndNode(NodeBase<T, TProperty> current)
        {
            if (!current.HasChild)
            {
                return current;
            }
            else
            {
                return getEndNode(current.ChildNode);
            }
        }

        private RuleNode<T, TProperty> getLastRuleNode(NodeBase<T, TProperty> current)
        {
            if (!current.HasChild)
            {
                if (current is RuleNode<T, TProperty>)
                {
                    return current as RuleNode<T, TProperty>;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (current.ChildNode is GroupNode<T, TProperty>)
                {
                    return getLastRuleNode(((GroupNode<T,TProperty>)current.ChildNode).GroupRoot);
                }
                else
                {
                    return getLastRuleNode(current.ChildNode);
                }
            }
        }

    }
}