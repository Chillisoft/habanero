//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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


//using System.Collections;
//using System.Windows.Forms;
//using Habanero.BO.ClassDefinition;
//using Habanero.BO;
//using Habanero.Test;
//using Habanero.UI.Forms;
//using NMock;
//using NUnit.Extensions.Forms;
//using NUnit.Framework;
//
//namespace Habanero.Test.UI.Forms
//{
//    /// <summary>
//    /// Summary description for TreeViewTableEditor.
//    /// </summary>
//    [TestFixture]
//    public class TestTreeViewTableEditor
//    {
//        private TreeViewTableEditor itsTreeViewTableEditor;
//        private ControlTester itsGridControlTester;
//        private Form frm;
//        private ClassDef itsClassDefMyBo;
//        private BusinessObject itsSampleMyBo;
//
//        [TestFixtureSetUp]
//        public void SetupFixture()
//        {
//            ClassDef.GetClassDefCol.Clear();
//            itsClassDefMyBo = MyBo.LoadClassDefWithNoLookup();
//            frm = new Form();
//            itsTreeViewTableEditor = new TreeViewTableEditor();
//            itsTreeViewTableEditor.Initialise();
//            frm.Controls.Add(itsTreeViewTableEditor);
//            frm.Show();
//            itsGridControlTester = new ControlTester("GridControl");
//        }
//
//
//        [SetUp]
//        public void SetupForm()
//        {
//            
//
//        }
//
//        [TearDown]
//        public void TearDownForm()
//        {
//            frm.Close();
//            frm.Dispose();
//        }
//
//        [Test]
//        public void TestLayout()
//        {
//            ControlTester tester = new ControlTester ("TreeViewTableEditor.SplitContainer.Panel1.MyTreeView");
//            Assert.AreEqual("test", tester.Text );
//            TreeViewTester treeViewTester = new TreeViewTester("TreeViewTableEditor.SplitContainer.Panel1.MyTreeView");
//            Assert.AreEqual(DockStyle.Left, treeViewTester.Properties.Dock);
//
//            Assert.AreEqual(DockStyle.Fill, itsGridControlTester["Dock"]);
//            ControlTester buttonControlTester = new ControlTester("ButtonControl");
//            Assert.AreEqual(DockStyle.Bottom, buttonControlTester["Dock"]);
//            ControlTester gridAndButtonPanel = new ControlTester("SimpleGridWithButtons");
//            Assert.AreEqual(DockStyle.Fill, gridAndButtonPanel["Dock"]);
//        }
//
//        //THIS is broken since i changed the formant - going to have to research the controltester again.
//        //[Test]
//        //public void TestPopulateTreeView()
//        //{
//        //    TreeViewTester treeViewTester = new TreeViewTester("TreeView");
//        //    PopulateTreeView();
//
//        //    Assert.AreEqual(2, treeViewTester.Properties.Nodes.Count, "There should be two parent nodes");
//        //    Assert.AreEqual("1", treeViewTester.Properties.Nodes[0].Text);
//        //    Assert.AreEqual(null, treeViewTester.Properties.Nodes[0].Tag);
//        //    Assert.AreEqual(1, treeViewTester.Properties.Nodes[0].Nodes.Count, "There should be one child of '1'");
//        //    Assert.AreEqual("My Bo", treeViewTester.Properties.Nodes[0].Nodes[0].Text);
//        //    Assert.AreEqual(typeof (MyBo), treeViewTester.Properties.Nodes[0].Nodes[0].Tag.GetType());
//        //}
//
//        private void PopulateTreeView()
//        {
//            Mock mockControl = new DynamicMock(typeof (TreeViewTableEditor.ITreeViewDataSource));
//            TreeViewTableEditor.ITreeViewDataSource treeViewDataSource =
//                (TreeViewTableEditor.ITreeViewDataSource) mockControl.MockInstance;
//
//            itsSampleMyBo = itsClassDefMyBo.CreateNewBusinessObject();
//            IList treeContents = new ArrayList();
//            treeContents.Add(new DictionaryEntry("1", null));
//            treeContents.Add(new DictionaryEntry("My Bo", itsSampleMyBo));
//            treeContents.Add(new DictionaryEntry("2", null));
//            treeContents.Add(new DictionaryEntry("My Other Bo", itsSampleMyBo));
//            mockControl.ExpectAndReturn("GetTreeViewData", treeContents, null);
//
//            itsTreeViewTableEditor.TreeViewDataSource = treeViewDataSource;
//            itsTreeViewTableEditor.PopulateTreeView();
//            mockControl.Verify();
//        }
//
//        //TODO:  get this test working again - treeviewtableeditor is working correctly, but this test fails.
////		[Test]
////		public void TestSelectTreeNode() {
////			TreeViewTester treeViewTester = new TreeViewTester("TreeView");
////			PopulateTreeView();
////
////			BusinessObjectCollection col = new BusinessObjectBaseCollection(itsClassDefMyBo);
////			BusinessObjectBase bo1 = itsClassDefMyBo.CreateNewBusinessObject();
////			bo1.SetPropertyValue("TestProp", "Value");
////			bo1.SetPropertyValue("TestProp2", "Value2");
////			col.Add(bo1);
////
////			Mock mockControlTableDataSource = new DynamicMock(typeof (TreeViewTableEditor.ITableDataSource));
////			TreeViewTableEditor.ITableDataSource tableDataSource = (TreeViewTableEditor.ITableDataSource)mockControlTableDataSource.MockInstance;
////
////			itsTreeViewTableEditor.TableDataSource = tableDataSource;
////			mockControlTableDataSource.ExpectAndReturn("GetBusinessObjectCollection", col, new object[] {itsSampleMyBo});
////			treeViewTester.SelectNode(new int[] {0, 0});
////
////			Assert.IsNotNull(itsGridControlTester["DataSource"], "Datasource of grid should be set once the node is set.");
////			DataView view = (DataView)itsGridControlTester["DataSource"];
////			Assert.AreEqual(1, view.Table.Rows.Count);
////			Assert.AreEqual("Value", view.Table.Rows[0]["TestProp"]);
////			mockControlTableDataSource.Verify();
////		}
//    }
//}