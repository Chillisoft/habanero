// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestTreeView
    {
        protected abstract IControlFactory GetControlFactory();

        [Test]
        public void Test_TreeNode_TreeView()
        {
            //--------------- Set up test pack ------------------
            ITreeView treeView = GetControlFactory().CreateTreeView("test");
            ITreeNode node = GetControlFactory().CreateTreeNode("TestNode");
            treeView.Nodes.Add(node);
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            //--------------- Execute Test ----------------------
            ITreeView returnedTreeView = node.TreeView;
            //--------------- Test Result -----------------------
            Assert.AreSame(treeView, returnedTreeView);
        }
    }


}
