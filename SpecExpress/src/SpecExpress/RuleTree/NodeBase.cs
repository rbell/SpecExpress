using System;

namespace SpecExpress.RuleTree
{
    public abstract class NodeBase<T, TProperty>
    {
        public event EventHandler NodeAltered;

        private NodeBase<T, TProperty> _childNode;

        /// <summary>
        /// Reference to a RuleNode
        /// </summary>
        public NodeBase<T, TProperty> ChildNode
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

        public bool HasChild
        {
            get { return ChildNode != null;}
        }

        public void AndChild(NodeBase<T, TProperty> child)
        {
            _childHasAndRelationship = true;
            ChildNode = child;
        }

        public void OrChild(NodeBase<T, TProperty> child)
        {
            _childHasAndRelationship = false;
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