//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------
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
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        Base.AnchorStyles IControlHabanero.Anchor
        {
            get { return (Base.AnchorStyles)base.Anchor; }
            set { base.Anchor = (System.Windows.Forms.AnchorStyles)value; }
        }
     
        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>  
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlHabanero.Dock
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