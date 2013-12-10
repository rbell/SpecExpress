using System;
using SpecExpress.Rules;

namespace SpecExpress.RuleTree
{
    /// <summary>
    /// Tree structure that describes RuleValidators and their relations to one another (i.e. And / Or)
    /// </summary>
    public class RuleTree
    {
        private NodeBase _root;

        public RuleTree()
        {
        }

        public RuleTree(NodeBase rootNode)
        {
            Root = rootNode;
        }

        public virtual NodeBase Root
        {
            get { return _root; }
            set
            {
                _root = value;
                //_root.NodeAltered += new EventHandler(_root_NodeAltered);
            }
        }

        public NodeBase EndNode
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

        public RuleNode LastRuleNode
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

        private NodeBase getEndNode(NodeBase current)
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

        private RuleNode getLastRuleNode(NodeBase current)
        {
            if (!current.HasChild)
            {
                if (current is RuleNode)
                {
                    return current as RuleNode;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (current.ChildNode is GroupNode)
                {
                    return getLastRuleNode(((GroupNode)current.ChildNode).GroupRoot);
                }
                else
                {
                    return getLastRuleNode(current.ChildNode);
                }
            }
        }

    }


    /// <summary>
    /// Tree structure that describes RuleValidators and their relations to one another (i.e. And / Or)
    /// </summary>
    public class RuleTree<T, TProperty> : RuleTree
    {
       
        private Func<RuleValidatorContext<T, TProperty>, SpecificationContainer, ValidationNotification, bool> _lambda;

        public RuleTree()
        {
        }

        public RuleTree(NodeBase rootNode)
        {
            Root = rootNode;
        }

        public NodeBase Root
        {
            get { return base.Root; }
            set
            {
                base.Root = value;
                base.Root.NodeAltered += new EventHandler(_root_NodeAltered);
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
    }

}