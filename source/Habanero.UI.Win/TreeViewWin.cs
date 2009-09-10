//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using System.Collections;
using System.Windows.Forms;
using Habanero.UI.Base;
using TreeViewAction=Habanero.UI.Base.TreeViewAction;
using TreeViewCancelEventArgs=System.Windows.Forms.TreeViewCancelEventArgs;
using TreeViewCancelEventHandler=Habanero.UI.Base.TreeViewCancelEventHandler;
using TreeViewEventArgs=System.Windows.Forms.TreeViewEventArgs;
using TreeViewEventHandler=Habanero.UI.Base.TreeViewEventHandler;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Displays a hierarchical collection of labeled items, each represented by a TreeNode
    /// </summary>
    public class TreeViewWin : TreeView, ITreeView
    {
        private readonly TreeNodeCollectionWin _nodes;

        #region Utility Methods

        private static ITreeNode GetITreeNode(TreeNode treeNode)
        {
            return (ITreeNode) treeNode;
        }

        private static TreeNode GetTreeNode(ITreeNode treeNode)
        {
            return (TreeNode)treeNode;
        }

        #endregion // Utility Methods

        ///<summary>
        /// Constructs a new <see cref="TreeViewWin"/>
        ///</summary>
        public TreeViewWin() : this(GlobalUIRegistry.ControlFactory) { }

        ///<summary>
        /// Constructs a new <see cref="TreeViewWin"/>
        ///</summary>
        ///<param name="controlFactory">The Control Factory to use to construct new nodes</param>
        public TreeViewWin(IControlFactory controlFactory)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            _nodes = new TreeNodeCollectionWin(base.Nodes, controlFactory);
        }

        #region Event Handling

        /// <summary>
        /// An event that is fired after the <see cref="ITreeNode"/>  is selected.
        /// </summary>
        public new event TreeViewEventHandler AfterSelect;

        /// <summary>
        /// An event that is fired just before the <see cref="ITreeNode"/> is selected.
        /// </summary>
        public new event TreeViewCancelEventHandler BeforeSelect;

        /// <summary>Occurs before the tree node is expanded.</summary>
        /// <filterpriority>1</filterpriority>
        public new event TreeViewCancelEventHandler BeforeExpand;

        /// <summary>Occurs before the tree node check box is checked.</summary>
        /// <filterpriority>1</filterpriority>
        public new event TreeViewCancelEventHandler BeforeCheck;

        /// <summary>Occurs before the tree node is collapsed.</summary>
        /// <filterpriority>1</filterpriority>
        public new event TreeViewCancelEventHandler BeforeCollapse;

        /// <summary>Occurs after the tree node check box is checked.</summary>
        /// <filterpriority>1</filterpriority>
        public new event TreeViewEventHandler AfterCheck;

        /// <summary>Occurs after the tree node is expanded.</summary>
        /// <filterpriority>1</filterpriority>
        public new event TreeViewEventHandler AfterExpand;

        /// <summary>Occurs after the tree node is collapsed.</summary>
        /// <filterpriority>1</filterpriority>
        public new event TreeViewEventHandler AfterCollapse;

        /// <summary>
        ///
        /// </summary>
        /// <param name="intX"></param>
        /// <param name="intY"></param>
        public new ITreeNode GetNodeAt(int intX, int intY)
        {
            return GetITreeNode(base.GetNodeAt(intX, intY));
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.TreeView.AfterSelect" /> event.
        ///</summary>
        ///<param name="e">A <see cref="T:System.Windows.Forms.TreeViewEventArgs" /> that contains the event data. </param>
        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            base.OnAfterSelect(e);
            if (this.AfterSelect == null) return;
            Base.TreeViewEventArgs treeViewEventArgs = new Base.TreeViewEventArgs(GetITreeNode(e.Node), (TreeViewAction)e.Action);
            this.AfterSelect(this, treeViewEventArgs);
        }
        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.TreeView.AfterCheck" /> event.
        ///</summary>
        ///<param name="e">A <see cref="T:System.Windows.Forms.TreeViewEventArgs" /> that contains the event data. </param>
        protected override void OnAfterCheck(TreeViewEventArgs e)
        {
            base.OnAfterCheck(e);
            if (this.AfterCheck == null) return;
            Base.TreeViewEventArgs treeViewEventArgs = new Base.TreeViewEventArgs(GetITreeNode(e.Node), (TreeViewAction)e.Action);
            this.AfterCheck(this, treeViewEventArgs);
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.TreeView.AfterExpand" /> event.
        ///</summary>
        ///<param name="e">A <see cref="T:System.Windows.Forms.TreeViewEventArgs" /> that contains the event data. </param>
        protected override void OnAfterExpand(TreeViewEventArgs e)
        {
            base.OnAfterExpand(e);
            if (this.AfterExpand == null) return;
            Base.TreeViewEventArgs treeViewEventArgs = new Base.TreeViewEventArgs(GetITreeNode(e.Node), (TreeViewAction)e.Action);
            this.AfterExpand(this, treeViewEventArgs);
        }
        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.TreeView.AfterCollapse" /> event.
        ///</summary>
        ///<param name="e">A <see cref="T:System.Windows.Forms.TreeViewEventArgs" /> that contains the event data. </param>
        protected override void OnAfterCollapse(TreeViewEventArgs e)
        {
            base.OnAfterCollapse(e);
            if (this.AfterCollapse == null) return;
            Base.TreeViewEventArgs treeViewEventArgs = new Base.TreeViewEventArgs(GetITreeNode(e.Node), (TreeViewAction)e.Action);
            this.AfterCollapse(this, treeViewEventArgs);
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.TreeView.BeforeSelect" /> event.
        ///</summary>
        ///<param name="e">A <see cref="T:System.Windows.Forms.TreeViewCancelEventArgs" /> that contains the event data. </param>
        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            base.OnBeforeSelect(e);
            if (this.BeforeSelect == null || e.Cancel) return;
            Base.TreeViewCancelEventArgs treeViewCancelEventArgs = new Base.TreeViewCancelEventArgs(GetITreeNode(e.Node), e.Cancel, (TreeViewAction)e.Action);
            this.BeforeSelect(this, treeViewCancelEventArgs);
            e.Cancel = treeViewCancelEventArgs.Cancel;
        }
        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.TreeView.BeforeCollapse" /> event.
        ///</summary>
        ///<param name="e">A <see cref="T:System.Windows.Forms.TreeViewCancelEventArgs" /> that contains the event data. </param>
        protected override void OnBeforeCollapse(TreeViewCancelEventArgs e)
        {
            base.OnBeforeCollapse(e);
            if (this.BeforeCollapse == null || e.Cancel) return;
            Base.TreeViewCancelEventArgs treeViewCancelEventArgs = new Base.TreeViewCancelEventArgs(GetITreeNode(e.Node), e.Cancel, (TreeViewAction)e.Action);
            this.BeforeCollapse(this, treeViewCancelEventArgs);
            e.Cancel = treeViewCancelEventArgs.Cancel;
        }
        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.TreeView.BeforeCheck" /> event.
        ///</summary>
        ///<param name="e">A <see cref="T:System.Windows.Forms.TreeViewCancelEventArgs" /> that contains the event data. </param>
        protected override void OnBeforeCheck(TreeViewCancelEventArgs e)
        {
            base.OnBeforeCheck(e);
            if (this.BeforeCheck == null || e.Cancel) return;
            Base.TreeViewCancelEventArgs treeViewCancelEventArgs = new Base.TreeViewCancelEventArgs(GetITreeNode(e.Node), e.Cancel, (TreeViewAction)e.Action);
            this.BeforeCheck(this, treeViewCancelEventArgs);
            e.Cancel = treeViewCancelEventArgs.Cancel;
        }
        ///<summary>
        ///Raises the <see cref="ITreeView.BeforeExpand" /> event.
        ///</summary>
        ///<param name="e">A <see cref="TreeViewCancelEventArgs" /> that contains the event data. </param>
        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            base.OnBeforeExpand(e);
            if (this.BeforeExpand == null || e.Cancel) return;
            Base.TreeViewCancelEventArgs treeViewCancelEventArgs = new Base.TreeViewCancelEventArgs(GetITreeNode(e.Node), e.Cancel, (TreeViewAction)e.Action);
            this.BeforeExpand(this, treeViewCancelEventArgs);
            e.Cancel = treeViewCancelEventArgs.Cancel;
        }

        #endregion // Event Handling
        
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

        /// <summary>
        /// The collection (<see cref="ITreeNodeCollection"/>) of <see cref="ITreeNode"/>s in this <see cref="ITreeView"/>.
        /// This collection only returns the <see cref="ITreeNode"/>s that are directly assigned to the <see cref="ITreeView"/>.
        /// i.e. all the nodes shown in this collection are Root Nodes.
        /// </summary>
        public new ITreeNodeCollection Nodes
        {
            get { return _nodes; }
        }

        /// <summary>
        /// The top <see cref="ITreeNode"/> or first node shown in the <see cref="ITreeView"/>.
        /// </summary>
        public new ITreeNode TopNode
        {
            get { return GetITreeNode(base.TopNode); }
            set { base.TopNode = GetTreeNode(value); }
        }

        /// <summary>
        /// The currently selected <see cref="ITreeNode"/> in the <see cref="ITreeView"/>.
        /// </summary>
        public new ITreeNode SelectedNode
        {
            get { return GetITreeNode(base.SelectedNode); }
            set { base.SelectedNode = GetTreeNode(value); }
        }

        ///<summary>
        /// An implementation of <see cref="ITreeView"/> for Windows Forms.
        ///</summary>
        public class TreeNodeWin : TreeNode, ITreeNode
        {
            private readonly TreeNodeCollectionWin _nodes;

            ///<summary>
            /// Constructs a new <see cref="TreeNodeWin"/>.
            ///</summary>
            public TreeNodeWin() : this("") { }

            ///<summary>
            /// Constructs a new <see cref="TreeNodeWin"/> with the specified text.
            ///</summary>
            ///<param name="text">The label text of the new Tree node</param>
            public TreeNodeWin(string text) : this(GlobalUIRegistry.ControlFactory, text) { }

            ///<summary>
            /// Constructs a new <see cref="TreeNodeWin"/> with the specified text.
            ///</summary>
            ///<param name="controlFactory">The Control Factory to use to construct new nodes</param>
            public TreeNodeWin(IControlFactory controlFactory) : this(controlFactory, "") { }

            ///<summary>
            /// Constructs a new <see cref="TreeNodeWin"/> with the specified text.
            ///</summary>
            ///<param name="controlFactory">The Control Factory to use to construct new nodes</param>
            ///<param name="text">The label text of the new Tree node</param>
            public TreeNodeWin(IControlFactory controlFactory, string text) : base(text)
            {
                _nodes = new TreeNodeCollectionWin(base.Nodes, controlFactory);
            }

            /// <summary>
            /// The parent <see cref="ITreeNode"/> if one exists null if this is the Root Node.
            /// </summary>
            public new ITreeNode Parent
            {
                get { return GetITreeNode(base.Parent); }
            }

            ///<summary>
            /// The <see cref="ITreeNodeCollection"/> of <see cref="ITreeNode"/>'s that are children of this <see cref="ITreeNode"/>.
            ///</summary>
            public new ITreeNodeCollection Nodes
            {
                get { return _nodes; }
            }


            /// <summary>Gets the first child tree node in the tree node collection.</summary>
            /// <returns>The first child <see cref="ITreeNode"></see> in the <see cref="Nodes"></see> collection.</returns>
            public new ITreeNode FirstNode
            {
                get { return GetITreeNode(base.FirstNode); }
            }

            /// <summary>Gets the last child tree node.</summary>
            /// <returns>A <see cref="ITreeNode"></see> that represents the last child tree node.</returns>
            public new ITreeNode LastNode
            {
                get { return GetITreeNode(base.LastNode); }
            }

            /// <summary>Gets the previous sibling tree node.</summary>
            /// <returns>A <see cref="ITreeNode"></see> that represents the previous sibling tree node.</returns>
            public new ITreeNode PrevNode
            {
                get { return GetITreeNode(base.PrevNode); }
            }

            /// <summary>Gets the previous visible tree node.</summary>
            /// <returns>A <see cref="ITreeNode"></see> that represents the previous visible tree node.</returns>  
            public new ITreeNode PrevVisibleNode
            {
                get { return GetITreeNode(base.PrevVisibleNode); }
            }

            /// <summary>Gets the next sibling tree node.</summary>
            /// <returns>A <see cref="ITreeNode"></see> that represents the next sibling tree node.</returns>
            public new ITreeNode NextNode
            {
                get { return GetITreeNode(base.NextNode); }
            }

            /// <summary>Gets the next visible tree node.</summary>
            /// <returns>A <see cref="ITreeNode"></see> that represents the next visible tree node.</returns>
            /// <filterpriority>1</filterpriority>        
            public new ITreeNode NextVisibleNode
            {
                get { return GetITreeNode(base.NextVisibleNode); }
            }

            /// <summary>Gets the parent tree view that the tree node is assigned to.</summary>
            /// <returns>A <see cref="ITreeView"></see> that represents the parent tree view that the tree node is assigned to, or null if the node has not been assigned to a tree view.</returns>
            /// <filterpriority>1</filterpriority>
            public new ITreeView TreeView
            {
                get { return (ITreeView)base.TreeView; }
            }
        }

        ///<summary>
        /// An implementation of <see cref="ITreeNodeCollection"/> for windows.
        /// This implements the wrapper pattern where the underlying windows TreeView control
        ///  is merely wrapped by this control.
        /// this control
        ///</summary>
        public class TreeNodeCollectionWin : ITreeNodeCollection
        {
            private readonly TreeNodeCollection _nodes;
            private readonly IControlFactory _controlFactory;

            ///<summary>
            /// constructs a <see cref="TreeNodeCollectionWin"/>
            ///</summary>
            ///<param name="nodes">The underlying Nodes collection</param>
            ///<param name="controlFactory">Control Factory used to Create new nodes for this collection</param>
            public TreeNodeCollectionWin(TreeNodeCollection nodes, IControlFactory controlFactory)
            {
                if (nodes == null) throw new ArgumentNullException("nodes");
                if (controlFactory == null) throw new ArgumentNullException("controlFactory");
                _nodes = nodes;
                _controlFactory = controlFactory;
            }

            /// <summary>
            /// Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
            /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins. </param>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="array" /> is null. </exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index" /> is less than zero. </exception>
            /// <exception cref="T:System.ArgumentException"><paramref name="array" /> is multidimensional.-or- <paramref name="index" /> is equal to or greater than the length of <paramref name="array" />.-or- The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />. </exception>
            /// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception><filterpriority>2</filterpriority>
            public void CopyTo(Array array, int index)
            {
                _nodes.CopyTo(array, index);
            }

            ///<summary>
            /// The number of items in this collection
            ///</summary>
            public int Count
            {
                get { return _nodes.Count; }
            }

            /// <summary>
            /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
            /// </summary>
            /// <returns>
            /// An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            object ICollection.SyncRoot
            {
                get { return ((ICollection)_nodes).SyncRoot; }
            }

            /// <summary>
            /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).
            /// </summary>
            /// <returns>
            /// true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            bool ICollection.IsSynchronized
            {
                get { return ((ICollection)_nodes).IsSynchronized; }
            }

            /// <summary>
            /// Adds an item to the <see cref="T:System.Collections.IList" />.
            /// </summary>
            /// <returns>
            /// The position into which the new element was inserted.
            /// </returns>
            /// <param name="value">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Collections.IList" />. </param>
            /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> is read-only.-or- The <see cref="T:System.Collections.IList" /> has a fixed size. </exception><filterpriority>2</filterpriority>
            int IList.Add(object value)
            {
                return ((IList)_nodes).Add(value);
            }

            /// <summary>
            /// Determines whether the <see cref="T:System.Collections.IList" /> contains a specific value.
            /// </summary>
            /// <returns>
            /// true if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Collections.IList" />; otherwise, false.
            /// </returns>
            /// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.IList" />. </param><filterpriority>2</filterpriority>
            bool IList.Contains(object value)
            {
                return ((IList)_nodes).Contains(value);
            }

            /// <summary>
            /// Removes all items from the <see cref="T:System.Collections.IList" />.
            /// </summary>
            /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> is read-only. </exception><filterpriority>2</filterpriority>
            public void Clear()
            {
                _nodes.Clear();
            }

            /// <summary>
            /// Determines the index of a specific item in the <see cref="T:System.Collections.IList" />.
            /// </summary>
            /// <returns>
            /// The index of <paramref name="value" /> if found in the list; otherwise, -1.
            /// </returns>
            /// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.IList" />. </param><filterpriority>2</filterpriority>
            int IList.IndexOf(object value)
            {
                return ((IList)_nodes).IndexOf(value);
            }

            /// <summary>
            /// Inserts an item to the <see cref="T:System.Collections.IList" /> at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted. </param>
            /// <param name="value">The <see cref="T:System.Object" /> to insert into the <see cref="T:System.Collections.IList" />. </param>
            /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.IList" />. </exception>
            /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> is read-only.-or- The <see cref="T:System.Collections.IList" /> has a fixed size. </exception>
            /// <exception cref="T:System.NullReferenceException"><paramref name="value" /> is null reference in the <see cref="T:System.Collections.IList" />.</exception><filterpriority>2</filterpriority>
            void IList.Insert(int index, object value)
            {
                _nodes.Insert(index, (TreeNode) value);
                    //(IList)_nodes).Insert(index, value);
            }

            /// <summary>
            /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList" />.
            /// </summary>
            /// <param name="value">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Collections.IList" />. </param>
            /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> is read-only.-or- The <see cref="T:System.Collections.IList" /> has a fixed size. </exception><filterpriority>2</filterpriority>
            void IList.Remove(object value)
            {
                ((IList)_nodes).Remove(value);
            }

            /// <summary>
            /// Removes the <see cref="T:System.Collections.IList" /> item at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the item to remove. </param>
            /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.IList" />. </exception>
            /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> is read-only.-or- The <see cref="T:System.Collections.IList" /> has a fixed size. </exception><filterpriority>2</filterpriority>
            public void RemoveAt(int index)
            {
                ((IList)_nodes).RemoveAt(index);
            }

            /// <summary>
            /// Gets or sets the element at the specified index.
            /// </summary>
            /// <returns>
            /// The element at the specified index.
            /// </returns>
            /// <param name="index">The zero-based index of the element to get or set. </param>
            /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.IList" />. </exception>
            /// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.IList" /> is read-only. </exception><filterpriority>2</filterpriority>
            object IList.this[int index]
            {
                get { return _nodes[index]; }
                set { ((IList)_nodes)[index] = value; }
            }

            /// <summary>
            /// Gets a value indicating whether the <see cref="T:System.Collections.IList" /> is read-only.
            /// </summary>
            /// <returns>
            /// true if the <see cref="T:System.Collections.IList" /> is read-only; otherwise, false.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            public bool IsReadOnly
            {
                get { return _nodes.IsReadOnly; }
            }

            /// <summary>
            /// Gets a value indicating whether the <see cref="T:System.Collections.IList" /> has a fixed size.
            /// </summary>
            /// <returns>
            /// true if the <see cref="T:System.Collections.IList" /> has a fixed size; otherwise, false.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            bool IList.IsFixedSize
            {
                get { return ((IList)_nodes).IsFixedSize; }
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
            public int Add(ITreeNode treeNode)
            {
                return _nodes.Add(GetTreeNode(treeNode));
            }

            /// <summary>
            /// Adds a new tree node to the end of the current tree node collection with the specified label text.
            /// </summary>
            /// <param name="text">The label text displayed by the TreeNode .</param>
            /// <returns>A TreeNode that represents the tree node being added to the collection.</returns>
            public ITreeNode Add(string text)
            {
                ITreeNode treeNode = _controlFactory.CreateTreeNode(text);
                Add(treeNode);
                return treeNode;
            }

            /// <summary>
            /// Adds a new tree node to the end of the current tree node collection with the specified label text.
            /// </summary>
            /// <param name="name">The name of the node(used as the key).</param>
            /// <param name="text">The label text displayed by the TreeNode .</param>
            /// <returns>A TreeNode that represents the tree node being added to the collection.</returns>
            public ITreeNode Add(string name, string text)
            {
                ITreeNode treeNode = this.Add(text);
                treeNode.Name = name;
                return treeNode;
            }

//            /// <summary>
//            ///
//            /// </summary>
//            /// <param name="objTreeViewNodes"></param>
//            public void AddRange(ITreeNode[] objTreeViewNodes)
//            {
//
//                _nodes.AddRange();
//            }

            /// <summary>
            /// Removes the specified tree view node.
            /// </summary>
            /// <param name="objTreeViewNode">Obj tree view node.</param>
            public void Remove(ITreeNode objTreeViewNode)
            {
                _nodes.Remove(GetTreeNode(objTreeViewNode));
            }

            #region Implementation of IEnumerable

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            public IEnumerator GetEnumerator()
            {
                return _nodes.GetEnumerator();
            }

            #endregion
        }
    }
}