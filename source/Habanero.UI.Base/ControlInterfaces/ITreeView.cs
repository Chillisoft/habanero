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

using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Displays a hierarchical collection of labeled items, each represented by a TreeNode
    /// </summary>
    public interface ITreeView : IControlHabanero
    {
        /// <summary>
       /// The collection (<see cref="ITreeNodeCollection"/>) of <see cref="ITreeNode"/>s in this <see cref="ITreeView"/>.
        /// This collection only returns the <see cref="ITreeNode"/>s that are directly assigned to the <see cref="ITreeView"/>.
        /// i.e. all the nodes shown in this collection are Root Nodes.
        /// </summary>
        [ListBindable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        ITreeNodeCollection Nodes { get; }
        
        /// <summary>
        /// The top <see cref="ITreeNode"/> or first node shown in the <see cref="ITreeView"/>.
        /// </summary>
        ITreeNode TopNode { get; set; }  //This is not supported by VWG

        /// <summary>
        /// The currently selected <see cref="ITreeNode"/> in the <see cref="ITreeView"/>.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        ITreeNode SelectedNode { get; set; }

        /// <summary>
        /// An event that is fired after the <see cref="ITreeNode"/>  is selected.
        /// </summary>
        event TreeViewEventHandler AfterSelect;
        /// <summary>
        /// An event that is fired just before the <see cref="ITreeNode"/> is selected.
        /// </summary>
        event TreeViewCancelEventHandler BeforeSelect;

        #region Events commented out

//        /// <summary>Occurs when the user clicks a <see cref="ITreeNode"></see> with the mouse. </summary>
//        /// <filterpriority>1</filterpriority>
//        event TreeNodeMouseClickEventHandler NodeMouseClick;
//
//        /// <summary>Occurs when the user double-clicks a <see cref="ITreeNode"></see> with the mouse.</summary>
//        /// <filterpriority>1</filterpriority>
//        event TreeNodeMouseClickEventHandler NodeMouseDoubleClick;
//
//        /// <summary>Occurs after the tree node label text is edited.</summary>
//        /// <filterpriority>1</filterpriority>
//        event NodeLabelEditEventHandler AfterLabelEdit;
//
//        /// <summary>Occurs before the tree node label text is edited.</summary>
//        /// <filterpriority>1</filterpriority>
//        event NodeLabelEditEventHandler BeforeLabelEdit;

        #endregion


        /// <summary>Occurs before the tree node is expanded.</summary>
        /// <filterpriority>1</filterpriority>
        event TreeViewCancelEventHandler BeforeExpand;

        /// <summary>Occurs before the tree node check box is checked.</summary>
        /// <filterpriority>1</filterpriority>
        event TreeViewCancelEventHandler BeforeCheck;

        /// <summary>Occurs before the tree node is collapsed.</summary>
        /// <filterpriority>1</filterpriority>
        event TreeViewCancelEventHandler BeforeCollapse;

        /// <summary>Occurs after the tree node check box is checked.</summary>
        /// <filterpriority>1</filterpriority>
        event TreeViewEventHandler AfterCheck;

        /// <summary>Occurs after the tree node is expanded.</summary>
        /// <filterpriority>1</filterpriority>
        event TreeViewEventHandler AfterExpand;

        /// <summary>Occurs after the tree node is collapsed.</summary>
        /// <filterpriority>1</filterpriority>
        event TreeViewEventHandler AfterCollapse;

        /// <summary>Gets or sets the implementation of <see cref="T:System.Collections.IComparer"></see> to perform a custom sort of the <see cref="ITreeView"></see> nodes.</summary>
        /// <returns>The <see cref="T:System.Collections.IComparer"></see> to perform the custom sort.</returns>
        /// <filterpriority>2</filterpriority>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        IComparer TreeViewNodeSorter { get; set; }

        /// <summary>Gets or sets a value indicating whether the tree nodes in the tree view are sorted.</summary>
        /// <returns>true if the tree nodes in the tree view are sorted; otherwise, false. The default is false.</returns>
        [DefaultValue(false), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        bool Sorted { get; set; }

        /// <summary>
        ///
        /// </summary>
        [System.ComponentModel.DefaultValue(false)]
        bool HideSelection { get; set; }

//        [System.ComponentModel.DefaultValue(null)]
//        /// <summary>
//        ///
//        /// </summary>
//        ImageList ImageList { get; set; } // Need to implement ImageList interface if we want this to work

        /// <summary>
        /// Gets or sets a value indicating whether check boxes are displayed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if check boxes are displayed; otherwise, <c>false</c>.
        /// </value>
        [System.ComponentModel.DefaultValue(false)]
        bool CheckBoxes { get; set; }





        /// <summary>
        /// Gets or sets a value indicating whether to show plus minus.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [to show plus minus]; otherwise, <c>false</c>.
        /// </value>
        [System.ComponentModel.DefaultValue(true)]
        bool ShowPlusMinus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show lines.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if to show lines; otherwise, <c>false</c>.
        /// </value>
        [System.ComponentModel.DefaultValue(true)]
        bool ShowLines { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance can focus.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can focus; otherwise, <c>false</c>.
        /// </value>
        bool CanFocus { get; }

        /// <summary>
        ///
        /// </summary>
        [System.ComponentModel.DefaultValue(-1)]
        int ImageIndex { get; set; }

        /// <summary>
        ///
        /// </summary>
        [System.ComponentModel.DefaultValue(-1)]
        int SelectedImageIndex { get; set; }

        /// <summary>
        ///
        /// </summary>
        [System.ComponentModel.DefaultValue(@"\")]
        string PathSeparator { get; set; }

//        /// <summary>Sorts the items if the value of the <see cref="P:Gizmox.WebGUI.Forms.TreeView.TreeViewNodeSorter"></see> property is not null.</summary>
//        void Sort();

        /// <summary>Expands all the tree nodes.</summary>
        void ExpandAll();

        /// <summary>Collapses all the tree nodes.</summary>
        void CollapseAll();

        /// <summary>Retrieves the number of tree nodes, optionally including those in all subtrees, assigned to the tree view control.</summary>
        /// <returns>The number of tree nodes, optionally including those in all subtrees, assigned to the tree view control.</returns>
        int GetNodeCount(bool includeSubTrees);

        /// <summary>
        /// Disables any redrawing of the tree view.
        /// </summary>
        void BeginUpdate();

        /// <summary>
        /// Enables the redrawing of the tree view.
        /// </summary>
        void EndUpdate();

        /// <summary>
        ///
        /// </summary>
        /// <param name="intX"></param>
        /// <param name="intY"></param>
        ITreeNode GetNodeAt(int intX, int intY);
    }

    ///<summary>
    /// An individual node in the <see cref="ITreeView"/>.
    ///</summary>
    public interface ITreeNode
    {
        ///<summary>
        /// The text shown in the <see cref="ITreeNode"/>
        ///</summary>
        [System.ComponentModel.DefaultValue("")]
        string Text { get; set; }

        ///<summary>
        /// Gets or sets the object that contains data about the <see cref="ITreeNode"/>
        ///</summary>
        object Tag { get; set; }

        /// <summary>
        /// The parent <see cref="ITreeNode"/> if one exists null if this is the Root Node.
        /// </summary>
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        [System.ComponentModel.Browsable(false)]
        ITreeNode Parent { get; }

        /// <summary>
        /// Expands this node to show the child nodes.
        /// </summary>
        void Expand();

        /// <summary>Gets the collection of <see cref="ITreeNode"></see> objects assigned to the current tree node.</summary>
        /// <returns>A <see cref="ITreeNodeCollection"></see> that represents the tree nodes assigned to the current tree node.</returns>
        /// <filterpriority>1</filterpriority>
        [ListBindable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        ITreeNodeCollection Nodes { get; }

        /// <summary>Gets a value indicating whether the tree node is in the expanded state.</summary>
        /// <returns>true if the tree node is in the expanded state; otherwise, false.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        bool IsExpanded { get; } // set; - This exists for VWG

        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <value></value>
        string FullPath { get; }

        /// <summary>Gets or sets the background color of the tree node.</summary>
        /// <returns>The background <see cref="T:System.Drawing.Color"></see> of the tree node. The default is <see cref="F:System.Drawing.Color.Empty"></see>.</returns>
        Color BackColor { get; set; }

        /// <summary>Gets the first child tree node in the tree node collection.</summary>
        /// <returns>The first child <see cref="ITreeNode"></see> in the <see cref="Nodes"></see> collection.</returns>
        [Browsable(false)]
        ITreeNode FirstNode { get; }

        /// <summary>Gets or sets the foreground color of the tree node.</summary>
        /// <returns>The foreground <see cref="T:System.Drawing.Color"></see> of the tree node.</returns>
        Color ForeColor { get; set; }

        /// <summary>Gets the position of the tree node in the tree node collection.</summary>
        /// <returns>A zero-based index value that represents the position of the tree node in the <see cref="Nodes"></see> collection.</returns>
        int Index { get; }

        /// <summary>Gets a value indicating whether the tree node is in an editable state.</summary>
        /// <returns>true if the tree node is in editable state; otherwise, false.</returns>
        [Browsable(false)]
        bool IsEditing { get; }

        /// <summary>Gets a value indicating whether the tree node is in the selected state.</summary>
        /// <returns>true if the tree node is in the selected state; otherwise, false.</returns>
        [Browsable(false)]
        bool IsSelected { get; }

        /// <summary>Gets a value indicating whether the tree node is visible or partially visible.</summary>
        /// <returns>true if the tree node is visible or partially visible; otherwise, false.</returns>
        [Browsable(false)]
        bool IsVisible { get; }

        /// <summary>Gets the last child tree node.</summary>
        /// <returns>A <see cref="ITreeNode"></see> that represents the last child tree node.</returns>
        [Browsable(false)]
        ITreeNode LastNode { get; }

        /// <summary>Gets the zero-based depth of the tree node in the <see cref="ITreeView"></see> control.</summary>
        /// <returns>The zero-based depth of the tree node in the <see cref="ITreeView"></see> control.</returns>
        [Browsable(false)]
        int Level { get; }

        /// <summary>Gets the previous sibling tree node.</summary>
        /// <returns>A <see cref="ITreeNode"></see> that represents the previous sibling tree node.</returns>
        [Browsable(false)]
        ITreeNode PrevNode { get; }

        /// <summary>Gets the previous visible tree node.</summary>
        /// <returns>A <see cref="ITreeNode"></see> that represents the previous visible tree node.</returns>  
        [Browsable(false)]
        ITreeNode PrevVisibleNode { get; }

        /// <summary>Gets the next sibling tree node.</summary>
        /// <returns>A <see cref="ITreeNode"></see> that represents the next sibling tree node.</returns>
        [Browsable(false)]
        ITreeNode NextNode { get; }

        /// <summary>Gets the next visible tree node.</summary>
        /// <returns>A <see cref="ITreeNode"></see> that represents the next visible tree node.</returns>
        /// <filterpriority>1</filterpriority>        
        [Browsable(false)]
        ITreeNode NextVisibleNode { get; }

        /// <summary>
        /// Gets or sets the node font.
        /// </summary>
        /// <value></value>
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        [System.ComponentModel.Browsable(false)]
        Font NodeFont { get; set; }

        /// <summary>
        /// Gets or sets the selected image index.
        /// </summary>
        /// <value></value>
        [System.ComponentModel.DefaultValue(-1)]
        int SelectedImageIndex { get; set; }

        /// <summary>
        /// Gets or sets the image index.
        /// </summary>
        /// <value></value>
        [System.ComponentModel.DefaultValue(-1)]
        int ImageIndex { get; set; }

        /// <summary>Gets the parent tree view that the tree node is assigned to.</summary>
        /// <returns>A <see cref="ITreeView"></see> that represents the parent tree view that the tree node is assigned to, or null if the node has not been assigned to a tree view.</returns>
        /// <filterpriority>1</filterpriority>
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        [System.ComponentModel.Browsable(false)]
        ITreeView TreeView { get; }

        /// <summary>
        /// Is this node checked or not.
        /// </summary>
        [System.ComponentModel.DefaultValue(false)]
        bool Checked { get; set; }

//        /// <summary>
//        /// Gets or sets a value indicating whether this <see cref="ITreeNode"/> is loaded.
//        /// </summary>
//        /// <value>
//        /// 	<c>true</c> if loaded; otherwise, <c>false</c>.
//        /// </value>
//        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
//        [System.ComponentModel.Browsable(false)]
//        bool Loaded { get; set; }

//        /// <summary>
//        /// Gets or sets a value indicating whether this instance has nodes.
//        /// </summary>
//        /// <value>
//        /// 	<c>true</c> if this instance has nodes; otherwise, <c>false</c>.
//        /// </value>
//        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
//        [System.ComponentModel.Browsable(false)]
//        bool HasNodes { get; set; }

        /// <summary>
        /// The tree node name
        /// </summary>
        string Name { get; set; }

//        /// <summary>
//        /// The tree node label
//        /// </summary>
//        [System.ComponentModel.Browsable(false)]
//        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
//        string Label { get; set; }



//        /// <summary>
//        /// The tree node icon
//        /// </summary>
//        [System.ComponentModel.DefaultValue(null)]
//        [System.ComponentModel.Browsable(false)]
//        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
//        ResourceHandle Image { get; set; }
//
//        /// <summary>
//        /// The selected tree node icon
//        /// </summary>
//        [System.ComponentModel.DefaultValue(null)]
//        [System.ComponentModel.Browsable(false)]
//        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
//        ResourceHandle SelectedImage { get; set; }
//
//        /// <summary>
//        /// The expanded tree node icon
//        /// </summary>
//        [System.ComponentModel.DefaultValue(null)]
//        [System.ComponentModel.Browsable(false)]
//        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
//        ResourceHandle ExpandedImage { get; set; }

//        /// <summary>
//        /// Gets or sets the expanded image index.
//        /// </summary>
//        /// <value></value>
//        [System.ComponentModel.DefaultValue(-1)]
//        int ExpandedImageIndex { get; set; }

        /// <summary>Toggles the tree node to either the expanded or collapsed state.</summary>
        void Toggle();

        /// <summary>Removes the current tree node from the tree view control.</summary>
        void Remove();


        /// <summary>Expands all the child tree nodes.</summary>
        void ExpandAll();

        /// <summary>Ensures that the tree node is visible, expanding tree nodes and scrolling the tree view control as necessary.</summary>
        void EnsureVisible();

        /// <summary>Collapses the tree node.</summary>
        void Collapse();

        /// <summary>Collapses the <see cref="ITreeNode"></see> and optionally collapses its children.</summary>
        /// <param name="ignoreChildren">true to leave the child nodes in their current state; false to collapse the child nodes.</param>
        void Collapse(bool ignoreChildren);

        /// <summary>
        /// Returns the number of child tree nodes.
        /// </summary>
        /// <param name="blnIncludeSubTrees">true if the resulting count includes all tree nodes indirectly rooted at this tree node; otherwise, false . </param>
        /// <returns></returns>
        int GetNodeCount(bool blnIncludeSubTrees);
    }


    /// <summary>
    /// A collection of nodes. This is used a children of a Node or all nodes in the tree.
    /// </summary>
    public interface ITreeNodeCollection:IList
    {

        /// <summary>
        /// Returns the item identified by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        new ITreeNode this[int index] { get; }

        /// <summary>
        /// Adds a new <paramref name="treeNode"/> to the collection of <see cref="ITreeNode"/>s
        /// </summary>
        /// <param name="treeNode">the <see cref="ITreeNode"/> that is being added to the collection</param>
        /// <returns>index of the added item</returns>
        int Add(ITreeNode treeNode);

        /// <summary>
        /// Adds a new tree node to the end of the current tree node collection with the specified label text.
        /// </summary>
        /// <param name="text">The label text displayed by the TreeNode .</param>
        /// <returns>A TreeNode that represents the tree node being added to the collection.</returns>
        ITreeNode Add(string text);

        /// <summary>
        /// Adds a new tree node to the end of the current tree node collection with the specified label text.
        /// </summary>
        /// <param name="name">The name of the node(used as the key).</param>
        /// <param name="text">The label text displayed by the TreeNode .</param>
        /// <returns>A TreeNode that represents the tree node being added to the collection.</returns>
        ITreeNode Add(string name, string text);

//        /// <summary>
//        /// </summary>
//        /// <param name="objTreeViewNodes"></param>
//        void AddRange(ITreeNode[] objTreeViewNodes); //Commented out too much effort to implement implement when required :Brett 18/03/2009

        /// <summary>
        /// Removes the specified tree view node.
        /// </summary>
        /// <param name="objTreeViewNode">Obj tree view node.</param>
        void Remove(ITreeNode objTreeViewNode);
    }
}