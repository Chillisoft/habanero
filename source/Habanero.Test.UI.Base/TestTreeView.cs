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

using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the TreeView class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_TreeView : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateTreeView("");
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the TreeView class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_TreeView : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateTreeView("");
        }
    }

    /// <summary>
    /// This test class tests the TreeView class.
    /// </summary>
    [TestFixture]
    public class TestTreeViewVWG
    {
        protected virtual IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
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
    /// <summary>
    /// This test class tests the TreeView class.
    /// </summary>
    [TestFixture]
    public class TestTreeViewWin : TestTreeViewVWG
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
    }

}
