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

using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;
using TreeViewAction=Habanero.UI.Base.TreeViewAction;
using TreeViewCancelEventArgs = Gizmox.WebGUI.Forms.TreeViewCancelEventArgs;
using TreeViewCancelEventHandler = Habanero.UI.Base.TreeViewCancelEventHandler;
using TreeViewEventArgs=Gizmox.WebGUI.Forms.TreeViewEventArgs;
using TreeViewEventHandler=Habanero.UI.Base.TreeViewEventHandler;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Displays a hierarchical collection of labeled items, each represented by a TreeNode
    /// </summary>
    public class TreeViewVWG : TreeView, ITreeView
    {
        private event TreeViewEventHandler _afterSelect;
        private event TreeViewCancelEventHandler _beforeSelect;

        #region Utility Methods

        private static ITreeNode GetITreeNode(TreeNode treeNode)
        {
            return treeNode == null ? null : new TreeNodeVWG(treeNode);
        }

        private static TreeNode GetTreeNode(ITreeNode treeNode)
        {
            if (treeNode == null) return null;
            return (TreeNode)treeNode. OriginalNode;
        }

        #endregion // Utility Methods

        #region Event Handling

        event TreeViewEventHandler ITreeView.AfterSelect
        {
            add { _afterSelect += value; }
            remove { _afterSelect -= value; }
        }

        event TreeViewCancelEventHandler ITreeView.BeforeSelect
        {
            add { _beforeSelect += value; }
            remove { _beforeSelect -= value; }
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            base.OnAfterSelect(e);
            if (this._afterSelect != null)
            {
                Base.TreeViewEventArgs treeViewEventArgs = new Base.TreeViewEventArgs(
                    GetITreeNode(e.Node), (TreeViewAction)e.Action);
                this._afterSelect(this, treeViewEventArgs);
            }
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            base.OnBeforeSelect(e);
            if (this._beforeSelect != null && !e.Cancel)
            {
                Base.TreeViewCancelEventArgs treeViewCancelEventArgs = new Base.TreeViewCancelEventArgs(
                    GetITreeNode(e.Node), e.Cancel, (TreeViewAction)e.Action);
                this._beforeSelect(this, treeViewCancelEventArgs);
                e.Cancel = treeViewCancelEventArgs.Cancel;
            }
        }

        #endregion // Event Handling

        /// <summary>
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        Base.AnchorStyles IControlHabanero.Anchor
        {
            get { return (Base.AnchorStyles)base.Anchor; }
            set { base.Anchor = (Gizmox.WebGUI.Forms.AnchorStyles)value; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionVWG(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlHabanero.Dock
        {
            get { return DockStyleVWG.GetDockStyle(base.Dock); }
            set { base.Dock = DockStyleVWG.GetDockStyle(value); }
        }

        public new ITreeNodeCollection Nodes
        {
            get { return new TreeNodeCollectionVWG(base.Nodes); }
        }

        /// <summary>
        /// The top <see cref="ITreeNode"/> or first node shown in the <see cref="ITreeView"/>.
        /// <remarks>
        /// This is custom implemented for Visual Web Guid since it was not implemeted by them.</remarks>
        /// </summary>
        public ITreeNode TopNode { get; set; }

        public new ITreeNode SelectedNode
        {
            get { return GetITreeNode(base.SelectedNode); }
            set { base.SelectedNode = GetTreeNode(value); }
        }
        /// <summary>
        /// An Implementation of <see cref="ITreeNode"/> for Visual Web Gui.
        /// </summary>
        public class TreeNodeVWG : TreeNode, ITreeNode
        {
            private readonly TreeNode _originalNode;

            ///<summary>
            /// A constructor for <see cref="TreeNodeVWG"/> that takes the underlying Visual Web Gui Node as its 
            /// arguement. I.e. This control acts as a wrapper to the Visual Web Gui TreeNode Control.
            ///</summary>
            ///<param name="node"></param>
            public TreeNodeVWG(TreeNode node)
            {
                _originalNode = node;
            }

            ///<summary>
            /// The text shown in the <see cref="ITreeNode"/>
            ///</summary>
            string ITreeNode.Text
            {
                get { return _originalNode.Text; }
                set { _originalNode.Text = value; }
            }

            /// <summary>
            /// The parent <see cref="ITreeNode"/> if one exists null if this is the Root Node.
            /// </summary>
            public new ITreeNode Parent
            {
                get { return GetITreeNode(_originalNode.Parent); }
            }

            ///<summary>
            /// The <see cref="ITreeNodeCollection"/> of <see cref="ITreeNode"/>'s that are children of this <see cref="ITreeNode"/>.
            ///</summary>
            public new ITreeNodeCollection Nodes
            {
                get { return new TreeNodeCollectionVWG(_originalNode.Nodes); }
            }

            /// <summary>
            /// The underlying Node i.e. If you are wrapping a Windows TreeView then this method will return the Windows Node.
            /// If you are wrapping a VWG Node then this method will return the underlying VWG Node.
            /// </summary>
            public object OriginalNode
            {
                get { return _originalNode; }
            }
        }
        /// <summary>
        /// An implementation of the <see cref="ITreeNodeCollection"/> for Visual Web Gui.
        /// </summary>
        public class TreeNodeCollectionVWG : ITreeNodeCollection
        {
            private readonly TreeNodeCollection _nodes;

            ///<summary>
            /// Constructs a <see cref="TreeNodeCollectionVWG"/>
            ///</summary>
            ///<param name="nodes"></param>
            public TreeNodeCollectionVWG(TreeNodeCollection nodes)
            {
                _nodes = nodes;
            }

            ///<summary>
            /// The number of items in this collection
            ///</summary>
            public int Count
            {
                get { return _nodes.Count; }
            }

            /// <summary>
            /// Returns the item identified by index.
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public ITreeNode this[int index]
            {
                get { return GetITreeNode(_nodes[index]); }
            }

            /// <summary>
            /// Adds a new <paramref name="treeNode"/> to the collection of <see cref="ITreeNode"/>s
            /// </summary>
            /// <param name="treeNode">the <see cref="ITreeNode"/> that is being added to the collection</param>
            public void Add(ITreeNode treeNode)
            {
                _nodes.Add(GetTreeNode(treeNode));
            }
        }
    }
}