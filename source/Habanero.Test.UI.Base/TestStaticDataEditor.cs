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

using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.Test;
using Habanero.Test.BO;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Summary description for TreeViewTableEditor.
    /// </summary>
    public abstract class TestStaticDataEditor
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
        }


        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }

        protected abstract IStaticDataEditor CreateEditorOnForm(out IFormChilli frm);
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestStaticDataEditorWin : TestStaticDataEditor
        {
            protected override IStaticDataEditor CreateEditorOnForm(out IFormChilli frm)
            {
                frm = GetControlFactory().CreateForm();
                IStaticDataEditor editor = GetControlFactory().CreateStaticDataEditor();
                frm.Controls.Add(editor);
                frm.Show();
                return editor;
            }

            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }
        }

        //[TestFixture]
        //public class TestTreeViewGridViewEditorGiz : TestStaticDataEditor
        //{
        //    protected override ITreeViewGridViewEditor CreateEditorOnForm(out IFormChilli frm)
        //    {
        //        throw new System.NotImplementedException();
        //    }

        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new ControlFactoryGizmox();
        //    }
        //}

        [Test]
        public void TestLayoutOfEditor()
        {
            //---------------Set up test pack-------------------
            IFormChilli frm;

            //---------------Execute Test ----------------------
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            //---------------Test Result -----------------------

            Assert.AreEqual(2, editor.Controls.Count);
            Assert.IsInstanceOfType(typeof (IEditableGridControl), editor.Controls[0]);
            Assert.IsInstanceOfType(typeof (ITreeView), editor.Controls[1]);
            IEditableGridControl editableGridControl = (IEditableGridControl) editor.Controls[0];
            Assert.AreEqual(Habanero.UI.Base.DockStyle.Fill, editor.Controls[0].Dock);
            Assert.AreEqual(Habanero.UI.Base.DockStyle.Left, editor.Controls[1].Dock);
            Assert.IsFalse(editableGridControl.FilterControl.Visible);

            //---------------Tear down -------------------------
            frm.Close();
            frm.Dispose();
        }

        [Test]
        public void TestAddSection()
        {
            //---------------Set up test pack-------------------
            IFormChilli frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            ITreeView treeView = (ITreeView) editor.Controls[1];
            string sectionName = TestUtil.CreateRandomString();
            //---------------Execute Test ----------------------
            editor.AddSection(sectionName);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            Assert.AreEqual(sectionName, treeView.Nodes[0].Text);
            Assert.AreEqual(sectionName, treeView.TopNode.Text);
            //---------------Tear Down -------------------------
            frm.Close();
            frm.Dispose();
        }

        [Test]
        public void TestAddItem()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            IFormChilli frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            ITreeView treeView = (ITreeView) editor.Controls[1];
            editor.AddSection(TestUtil.CreateRandomString());
            string itemName = TestUtil.CreateRandomString();
            //---------------Assert Preconditions---------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            //---------------Execute Test ----------------------

            editor.AddItem(itemName, classDef);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode topNode = treeView.Nodes[0];
            Assert.AreEqual(1, topNode.Nodes.Count);
            Assert.AreEqual(itemName, topNode.Nodes[0].Text);
            Assert.AreEqual(treeView.TopNode.Text, topNode.Nodes[0].Parent.Text);
            //---------------Tear Down -------------------------

            frm.Close();
            frm.Dispose();
        }

        [Test]
        public void TestAddSecondSection()
        {
            //---------------Set up test pack-------------------
            IFormChilli frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            ITreeView treeView = (ITreeView) editor.Controls[1];
            editor.AddSection(TestUtil.CreateRandomString());
            string sectionName = TestUtil.CreateRandomString();

            //---------------Assert Preconditions---------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            //---------------Execute Test ----------------------

            editor.AddSection(sectionName);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, treeView.Nodes.Count);
            Assert.AreEqual(sectionName, treeView.Nodes[1].Text);
            Assert.AreEqual(treeView.TopNode.Text, treeView.Nodes[0].Text);
            Assert.AreNotEqual(sectionName, treeView.TopNode.Text);
            //---------------Tear Down -------------------------

            frm.Close();
            frm.Dispose();
        }

        [Test]
        public void TestAddItemToSecondSection()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            IFormChilli frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            ITreeView treeView = (ITreeView) editor.Controls[1];
            editor.AddSection(TestUtil.CreateRandomString());
            editor.AddSection(TestUtil.CreateRandomString());
            string itemName = TestUtil.CreateRandomString();
            //---------------Execute Test ----------------------
            editor.AddItem(itemName, classDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, treeView.Nodes.Count);
            Assert.AreEqual(0, treeView.Nodes[0].Nodes.Count);
            Assert.AreEqual(1, treeView.Nodes[1].Nodes.Count);
            Assert.AreEqual(itemName, treeView.Nodes[1].Nodes[0].Text);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSelectItem()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            IFormChilli frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            IEditableGridControl gridControl = (IEditableGridControl) editor.Controls[0];
            editor.AddSection(TestUtil.CreateRandomString());
            string itemName = TestUtil.CreateRandomString();
            editor.AddItem(itemName, classDef);

            //---------------Execute Test ----------------------
            editor.SelectItem(itemName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(gridControl.Grid.GetBusinessObjectCollection());
            Assert.AreSame(classDef, gridControl.Grid.GetBusinessObjectCollection().ClassDef);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSelectSection()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            IFormChilli frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            IEditableGridControl gridControl = (IEditableGridControl)editor.Controls[0];
            editor.AddSection(TestUtil.CreateRandomString());
            string itemName = TestUtil.CreateRandomString();
            editor.AddItem(itemName, classDef);
            ITreeView treeView = (ITreeView)editor.Controls[1];
            treeView.SelectedNode = treeView.Nodes[0].Nodes[0];

            //---------------Assert Preconditions---------------
            Assert.IsTrue(gridControl.Grid.Enabled);

            //---------------Execute Test ----------------------
            treeView.SelectedNode = treeView.Nodes[0];

            //---------------Test Result -----------------------
            Assert.IsFalse(gridControl.Grid.Enabled);

            //---------------Tear Down -------------------------

        }


        [Test]
        public void TestSelectSectionThenItemNode()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            IFormChilli frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            IEditableGridControl gridControl = (IEditableGridControl)editor.Controls[0];
            editor.AddSection(TestUtil.CreateRandomString());
            string itemName = TestUtil.CreateRandomString();
            editor.AddItem(itemName, classDef);
            ITreeView treeView = (ITreeView)editor.Controls[1];
            //---------------Execute Test ----------------------
            treeView.SelectedNode = treeView.Nodes[0];

            treeView.SelectedNode = treeView.Nodes[0].Nodes[0];

            //---------------Test Result -----------------------
            Assert.IsTrue(gridControl.Grid.Enabled);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSelectTreeNode()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            IFormChilli frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            IEditableGridControl gridControl = (IEditableGridControl)editor.Controls[0];
            editor.AddSection(TestUtil.CreateRandomString());
            string itemName = TestUtil.CreateRandomString();
            editor.AddItem(itemName, classDef);
             ITreeView treeView = (ITreeView) editor.Controls[1];
            //---------------Execute Test ----------------------
            treeView.SelectedNode = treeView.Nodes[0].Nodes[0];

            //---------------Test Result -----------------------
            Assert.IsNotNull(gridControl.Grid.GetBusinessObjectCollection());
            Assert.AreSame(classDef, gridControl.Grid.GetBusinessObjectCollection().ClassDef);
            //---------------Tear Down -------------------------
        }




        [Test]
        public void TestSelectTwoTreeNodes()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef classDef1 = MyBO.LoadDefaultClassDef();
            ClassDef classDef2 = ContactPersonTestBO.LoadDefaultClassDefWithUIDef();

            IFormChilli frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            IEditableGridControl gridControl = (IEditableGridControl) editor.Controls[0];
            editor.AddSection(TestUtil.CreateRandomString());
            string itemName1 = TestUtil.CreateRandomString();
            editor.AddItem(itemName1, classDef1);
            string itemName2 = TestUtil.CreateRandomString();
            editor.AddItem(itemName2, classDef2);
            editor.SelectItem(itemName1);

            //---------------Assert Preconditions---------------
            Assert.IsNotNull(gridControl.Grid.GetBusinessObjectCollection());
            Assert.AreSame(classDef1, gridControl.Grid.GetBusinessObjectCollection().ClassDef);

            //---------------Execute Test ----------------------
            editor.SelectItem(itemName2);

            //---------------Test Result -----------------------
            Assert.IsNotNull(gridControl.Grid.GetBusinessObjectCollection());
            Assert.AreSame(classDef2, gridControl.Grid.GetBusinessObjectCollection().ClassDef);
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestSaveChanges()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef classDef1 = MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            myBO.Save();
            IFormChilli frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            IEditableGridControl gridControl = (IEditableGridControl)editor.Controls[0];
            editor.AddSection(TestUtil.CreateRandomString());
            string itemName1 = TestUtil.CreateRandomString();
            editor.AddItem(itemName1, classDef1);
            editor.SelectItem(itemName1);

            //---------------Execute Test ----------------------
            gridControl.Grid.SelectedBusinessObject = myBO;
            string newValue = TestUtil.CreateRandomString();
            gridControl.Grid.CurrentCell.Value = newValue;
            bool result = editor.SaveChanges();
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
            BusinessObjectCollection<MyBO> collection = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<MyBO>("");
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(newValue, myBO.TestProp);
            Assert.IsFalse(myBO.Status.IsDirty);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestRejectChanges()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef classDef1 = MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            string originalValue = TestUtil.CreateRandomString();
            myBO.TestProp = originalValue;
            myBO.Save();
            IFormChilli frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            IEditableGridControl gridControl = (IEditableGridControl)editor.Controls[0];
            editor.AddSection(TestUtil.CreateRandomString());
            string itemName1 = TestUtil.CreateRandomString();
            editor.AddItem(itemName1, classDef1);
            editor.SelectItem(itemName1);

            //---------------Execute Test ----------------------
            gridControl.Grid.SelectedBusinessObject = myBO;
            string newValue = TestUtil.CreateRandomString();
            gridControl.Grid.CurrentCell.Value = newValue;
            bool result = editor.RejectChanges();
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
            BusinessObjectCollection<MyBO> collection = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<MyBO>("");
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(originalValue, myBO.TestProp);
            Assert.IsFalse(myBO.Status.IsDirty);
            //---------------Tear Down -------------------------
        }
    }
}