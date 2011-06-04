using System;

namespace SpecExpress.RuleTree
{
    public abstract class NodeBase
    {
        public event EventHandler NodeAltered;

        private NodeBase _childNode;

        /// <summary>
        /// Reference to a RuleNode
        /// </summary>
        public NodeBase ChildNode
        {
            get { return _childNode; }
            private set 
            {
                _childNode = value;
                _childNode.NodeAltered += new EventHandler(_childNode_NodeAltered);
                OnNodeAltered();
            }
        }

        private bool _childHasAndRelationship;
        private bool _childRelationshipIsConditional = false;

        /// <summary>
        /// True if the child has an And relationship with the current node
        /// </summary>
        public bool ChildHasAndRelationship
        {
            get { return _childHasAndRelationship; }
            private set
            {
                if (value != _childHasAndRelationship)
                {
                    _childHasAndRelationship = value;
                   OnNodeAltered();  
                }
            }
        }

        public bool ChildRelationshipIsConditional
        {
            get { return _childRelationshipIsConditional; }
            set
            {
                if (value != _childRelationshipIsConditional)
                {
                    _childRelationshipIsConditional = value;
                    OnNodeAltered();
                }
            }
        }

        public bool HasChild
        {
            get { return ChildNode != null;}
        }

        public void AndChild(NodeBase child)
        {
            _childHasAndRelationship = true;
            ChildNode = child;
        }

        public void ConditionalAndChild(NodeBase child)
        {
            _childHasAndRelationship = true;
            _childRelationshipIsConditional = true;
            ChildNode = child;
        }

        public void OrChild(NodeBase child)
        {
            _childHasAndRelationship = false;
            ChildNode = child;
        }

        public void ConditionalOrChild(NodeBase child)
        {
            _childHasAndRelationship = false;
            _childRelationshipIsConditional = true;
            ChildNode = child;
        }

        protected void OnNodeAltered()
        {
            if (NodeAltered != null)
            {
                NodeAltered(this,new EventArgs());
            }
        }

        private void _childNode_NodeAltered(object sender, EventArgs e)
        {
            // Raise event on up the tree
            OnNodeAltered();
        }
    }
}