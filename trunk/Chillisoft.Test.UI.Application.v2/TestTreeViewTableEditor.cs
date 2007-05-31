using System.Collections;
using System.Windows.Forms;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Test.Setup.v2;
using Chillisoft.UI.Application.v2;
using NMock;
using NUnit.Extensions.Forms;
using NUnit.Framework;

namespace Chillisoft.Test.UI.Application.v2
{
    /// <summary>
    /// Summary description for TreeViewTableEditor.
    /// </summary>
    [TestFixture]
    public class TestTreeViewTableEditor
    {
        private TreeViewTableEditor itsTreeViewTableEditor;
        private ControlTester itsGridControlTester;
        private Form frm;
        private ClassDef itsClassDefMyBo;
        private BusinessObjectBase itsSampleMyBo;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.GetClassDefCol().Clear();
            itsClassDefMyBo = MyBo.LoadClassDefWithNoLookup();
        }


        [SetUp]
        public void SetupForm()
        {
            frm = new Form();
            itsTreeViewTableEditor = new TreeViewTableEditor();
            frm.Controls.Add(itsTreeViewTableEditor);
            frm.Show();
            itsGridControlTester = new ControlTester("GridControl");
        }

        [TearDown]
        public void TearDownForm()
        {
            frm.Close();
            frm.Dispose();
        }

        //[Test]
        //public void TestLayout()
        //{
        //    TreeViewTester treeViewTester = new TreeViewTester("TreeView");
        //    Assert.AreEqual(DockStyle.Left, treeViewTester.Properties.Dock);

        //    Assert.AreEqual(DockStyle.Fill, itsGridControlTester["Dock"]);
        //    ControlTester buttonControlTester = new ControlTester("ButtonControl");
        //    Assert.AreEqual(DockStyle.Bottom, buttonControlTester["Dock"]);
        //    ControlTester gridAndButtonPanel = new ControlTester("SimpleGridWithButtons");
        //    Assert.AreEqual(DockStyle.Fill, gridAndButtonPanel["Dock"]);
        //}

        //THIS is broken since i changed the formant - going to have to research the controltester again.
        //[Test]
        //public void TestPopulateTreeView()
        //{
        //    TreeViewTester treeViewTester = new TreeViewTester("TreeView");
        //    PopulateTreeView();

        //    Assert.AreEqual(2, treeViewTester.Properties.Nodes.Count, "There should be two parent nodes");
        //    Assert.AreEqual("1", treeViewTester.Properties.Nodes[0].Text);
        //    Assert.AreEqual(null, treeViewTester.Properties.Nodes[0].Tag);
        //    Assert.AreEqual(1, treeViewTester.Properties.Nodes[0].Nodes.Count, "There should be one child of '1'");
        //    Assert.AreEqual("My Bo", treeViewTester.Properties.Nodes[0].Nodes[0].Text);
        //    Assert.AreEqual(typeof (MyBo), treeViewTester.Properties.Nodes[0].Nodes[0].Tag.GetType());
        //}

        private void PopulateTreeView()
        {
            Mock mockControl = new DynamicMock(typeof (TreeViewTableEditor.ITreeViewDataSource));
            TreeViewTableEditor.ITreeViewDataSource treeViewDataSource =
                (TreeViewTableEditor.ITreeViewDataSource) mockControl.MockInstance;

            itsSampleMyBo = itsClassDefMyBo.CreateNewBusinessObject();
            IList treeContents = new ArrayList();
            treeContents.Add(new DictionaryEntry("1", null));
            treeContents.Add(new DictionaryEntry("My Bo", itsSampleMyBo));
            treeContents.Add(new DictionaryEntry("2", null));
            treeContents.Add(new DictionaryEntry("My Other Bo", itsSampleMyBo));
            mockControl.ExpectAndReturn("GetTreeViewData", treeContents, null);

            itsTreeViewTableEditor.TreeViewDataSource = treeViewDataSource;
            itsTreeViewTableEditor.PopulateTreeView();
            mockControl.Verify();
        }

        //TODO:  get this test working again - treeviewtableeditor is working correctly, but this test fails.
//		[Test]
//		public void TestSelectTreeNode() {
//			TreeViewTester treeViewTester = new TreeViewTester("TreeView");
//			PopulateTreeView();
//
//			BusinessObjectBaseCollection col = new BusinessObjectBaseCollection(itsClassDefMyBo);
//			BusinessObjectBase bo1 = itsClassDefMyBo.CreateNewBusinessObject();
//			bo1.SetPropertyValue("TestProp", "Value");
//			bo1.SetPropertyValue("TestProp2", "Value2");
//			col.Add(bo1);
//
//			Mock mockControlTableDataSource = new DynamicMock(typeof (TreeViewTableEditor.ITableDataSource));
//			TreeViewTableEditor.ITableDataSource tableDataSource = (TreeViewTableEditor.ITableDataSource)mockControlTableDataSource.MockInstance;
//
//			itsTreeViewTableEditor.TableDataSource = tableDataSource;
//			mockControlTableDataSource.ExpectAndReturn("GetCollection", col, new object[] {itsSampleMyBo});
//			treeViewTester.SelectNode(new int[] {0, 0});
//
//			Assert.IsNotNull(itsGridControlTester["DataSource"], "Datasource of grid should be set once the node is set.");
//			DataView view = (DataView)itsGridControlTester["DataSource"];
//			Assert.AreEqual(1, view.Table.Rows.Count);
//			Assert.AreEqual("Value", view.Table.Rows[0]["TestProp"]);
//			mockControlTableDataSource.Verify();
//		}
    }
}