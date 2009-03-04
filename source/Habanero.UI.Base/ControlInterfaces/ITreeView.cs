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
        ITreeNodeCollection Nodes { get; }
        /// <summary>
        /// The top <see cref="ITreeNode"/> or first node shown in the <see cref="ITreeView"/>.
        /// </summary>
        ITreeNode TopNode { set; get; }
        /// <summary>
        /// The currently selected <see cref="ITreeNode"/> in the <see cref="ITreeView"/>.
        /// </summary>
        ITreeNode SelectedNode { get; set; }
        /// <summary>
        /// An event that is fired after the <see cref="ITreeNode"/>  is selected.
        /// </summary>
        event TreeViewEventHandler AfterSelect;
        /// <summary>
        /// An event that is fired just before the <see cref="ITreeNode"/> is selected.
        /// </summary>
        event TreeViewCancelEventHandler BeforeSelect;
    }

    ///<summary>
    /// An individual node in the <see cref="ITreeView"/>.
    ///</summary>
    public interface ITreeNode
    {
        ///<summary>
        /// The t   ext shown in the <see cref="ITreeNode"/>
        ///</summary>
        string Text { get; set; }
        /// <summary>
        /// The parent <see cref="ITreeNode"/> if one exists null if this is the Root Node.
        /// </summary>
        ITreeNode Parent { get; }
        ///<summary>
        /// The <see cref="ITreeNodeCollection"/> of <see cref="ITreeNode"/>'s that are children of this <see cref="ITreeNode"/>.
        ///</summary>
        ITreeNodeCollection Nodes { get; }
        /// <summary>
        /// The underlying Node i.e. If you are wrapping a Windows TreeView then this method will return the Windows Node.
        /// If you are wrapping a VWG Node then this method will return the underlying VWG Node.
        /// </summary>
        object OriginalNode { get; }
    }
    /// <summary>
    /// A collection of nodes. This is used a children of a Node or all nodes in the tree.
    /// </summary>
    public interface ITreeNodeCollection
    {
        ///<summary>
        /// The number of items in this collection
        ///</summary>
        int Count { get; }
        /// <summary>
        /// Returns the item identified by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        ITreeNode this[int index] { get; }
        /// <summary>
        /// Adds a new <paramref name="treeNode"/> to the collection of <see cref="ITreeNode"/>s
        /// </summary>
        /// <param name="treeNode">the <see cref="ITreeNode"/> that is being added to the collection</param>
        void Add(ITreeNode treeNode);
    }
}