using System;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Displays a hierarchical collection of labeled items, each represented by a TreeNode
    /// </summary>
    public class TreeViewWin : TreeView, ITreeView
    {       
        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>  
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlChilli.Dock
        {
            get { return (Base.DockStyle)Enum.Parse(typeof(Base.DockStyle), base.Dock.ToString()); }
            set { base.Dock = (System.Windows.Forms.DockStyle)Enum.Parse(typeof(System.Windows.Forms.DockStyle), value.ToString()); }
        }

        public new ITreeNodeCollection Nodes
        {
            get { return new TreeNodeCollectionWin(base.Nodes); }
        }

        public new ITreeNode TopNode
        {
            get { return new TreeNodeWin(base.TopNode); }
            set { base.TopNode = (TreeNode)value.OriginalNode; }
        }

        public ITreeNode SelectedNode
        {
            get { return new TreeNodeWin(base.SelectedNode); }
            set { base.SelectedNode = (TreeNode) value.OriginalNode; }
        }

        public class TreeNodeWin : TreeNode, ITreeNode
        {
            private readonly TreeNode _originalNode;

            public TreeNodeWin(TreeNode node)
            {
                _originalNode = node;
            }

            string ITreeNode.Text
            {
                get { return _originalNode.Text; }
                set { _originalNode.Text = value; }
            }

            public ITreeNode Parent
            {
                get { return new TreeNodeWin(_originalNode.Parent); }
            }

            public ITreeNodeCollection Nodes
            {
                get { return new TreeNodeCollectionWin(_originalNode.Nodes); }
            }

            public object OriginalNode
            {
                get { return _originalNode; }
            }
        }

        public class TreeNodeCollectionWin :  ITreeNodeCollection
        {
            private readonly TreeNodeCollection _nodes;

            public TreeNodeCollectionWin(TreeNodeCollection nodes) 
            {
                _nodes = nodes;
            }

            public int Count
            {
                get { return _nodes.Count; }
            }

            public ITreeNode this[int index]
            {
                get { return new TreeNodeWin(_nodes[index]); }
            }
        }
    }
}