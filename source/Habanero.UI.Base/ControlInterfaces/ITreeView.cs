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
    public interface ITreeView : IControlChilli
    {

        ITreeNodeCollection Nodes { get; }
        ITreeNode TopNode { set; get; }
        ITreeNode SelectedNode { get; set; }
    }

    public interface ITreeNode
    {
        string Text { get; set; }
        ITreeNode Parent { get; }
        ITreeNodeCollection Nodes { get; }
        object OriginalNode { get; }
    }

    public interface ITreeNodeCollection
    {
        int Count { get; }
        ITreeNode this[int index] { get; }
    }
}