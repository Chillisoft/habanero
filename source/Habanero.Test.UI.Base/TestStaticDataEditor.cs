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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.UI.Base;


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

        protected abstract IStaticDataEditor CreateEditorOnForm(out IFormHabanero frm);
        protected abstract IControlFactory GetControlFactory();
        protected abstract void TearDownForm(IFormHabanero frm);



        [Test]
        public void TestLayoutOfEditor()
        {
            //---------------Set up test pack-------------------
            IFormHabanero frm;

            //---------------Execute Test ----------------------
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            //---------------Test Result -----------------------

            Assert.AreEqual(2, editor.Controls.Count);
            Assert.IsInstanceOfType(typeof (IEditableGridControl), editor.Controls[0]);
            Assert.IsInstanceOfType(typeof (ITreeView), editor.Controls[1]);
            IEditableGridControl editableGridControl = (IEditableGridControl) editor.Controls[0];
            Assert.AreEqual(DockStyle.Fill, editor.Controls[0].Dock);
            Assert.AreEqual(DockStyle.Left, editor.Controls[1].Dock);
            Assert.IsFalse(editableGridControl.FilterControl.Visible);

            //---------------Tear down -------------------------
            TearDownForm(frm);
        }

        
        

        [Test]
        public void TestAddSection()
        {
            //---------------Set up test pack-------------------
            IFormHabanero frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            ITreeView treeView = (ITreeView) editor.Controls[1];
            string sectionName = TestUtil.GetRandomString();
            //---------------Execute Test ----------------------
            editor.AddSection(sectionName);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            Assert.AreEqual(sectionName, treeView.Nodes[0].Text);
            Assert.AreEqual(sectionName, treeView.TopNode.Text);
            //---------------Tear Down -------------------------
            TearDownForm(frm);
        }

        [Test]
        public void TestAddItem()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            IFormHabanero frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            ITreeView treeView = (ITreeView) editor.Controls[1];
            editor.AddSection(TestUtil.GetRandomString());
            string itemName = TestUtil.GetRandomString();
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

            TearDownForm(frm);
        }

        [Test]
        public void TestAddSecondSection()
        {
            //---------------Set up test pack-------------------
            IFormHabanero frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            ITreeView treeView = (ITreeView) editor.Controls[1];
            editor.AddSection(TestUtil.GetRandomString());
            string sectionName = TestUtil.GetRandomString();

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

            TearDownForm(frm);
        }

        [Test]
        public void TestAddItemToSecondSection()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            IFormHabanero frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            ITreeView treeView = (ITreeView) editor.Controls[1];
            editor.AddSection(TestUtil.GetRandomString());
            editor.AddSection(TestUtil.GetRandomString());
            string itemName = TestUtil.GetRandomString();
            //---------------Execute Test ----------------------
            editor.AddItem(itemName, classDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, treeView.Nodes.Count);
            Assert.AreEqual(0, treeView.Nodes[0].Nodes.Count);
            Assert.AreEqual(1, treeView.Nodes[1].Nodes.Count);
            Assert.AreEqual(itemName, treeView.Nodes[1].Nodes[0].Text);
            //---------------Tear Down -------------------------
            TearDownForm(frm);
        }

        [Test]
        public void TestSelectItem()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            IFormHabanero frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            IEditableGridControl gridControl = (IEditableGridControl) editor.Controls[0];
            editor.AddSection(TestUtil.GetRandomString());
            string itemName = TestUtil.GetRandomString();
            editor.AddItem(itemName, classDef);

            //---------------Execute Test ----------------------
            editor.SelectItem(itemName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(gridControl.Grid.BusinessObjectCollection);
            Assert.AreSame(classDef, gridControl.Grid.BusinessObjectCollection.ClassDef);
            //---------------Tear Down -------------------------
            TearDownForm(frm);
        }


        [Test]
        public void TestAddTwoItemsAndSelectSecondItem()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef2 = MyBO.LoadClassDefWithBoolean();
            IFormHabanero frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            IEditableGridControl gridControl = (IEditableGridControl)editor.Controls[0];
            editor.AddSection(TestUtil.GetRandomString());
            string itemName = TestUtil.GetRandomString();
            string itemName2 = TestUtil.GetRandomString();
            editor.AddItem(itemName, classDef);
            editor.AddItem(itemName2,classDef2);
            //---------------Execute Test ----------------------
            editor.SelectItem(itemName);
            editor.SelectItem(itemName2);

            //---------------Test Result -----------------------
            Assert.IsNotNull(gridControl.Grid.BusinessObjectCollection);
            Assert.AreSame(classDef2, gridControl.Grid.BusinessObjectCollection.ClassDef);
            //---------------Tear Down -------------------------
            TearDownForm(frm);
        }
        [Test]
        public virtual void TestSelectSection()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            IFormHabanero frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            IEditableGridControl gridControl = (IEditableGridControl)editor.Controls[0];
            editor.AddSection(TestUtil.GetRandomString());
            string itemName = TestUtil.GetRandomString();
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
            TearDownForm(frm);

        }


        [Test]
        public void TestSelectSectionThenItemNode()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            IFormHabanero frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            IEditableGridControl gridControl = (IEditableGridControl)editor.Controls[0];
            editor.AddSection(TestUtil.GetRandomString());
            string itemName = TestUtil.GetRandomString();
            editor.AddItem(itemName, classDef);
            ITreeView treeView = (ITreeView)editor.Controls[1];
            //---------------Execute Test ----------------------
            treeView.SelectedNode = treeView.Nodes[0];

            treeView.SelectedNode = treeView.Nodes[0].Nodes[0];

            //---------------Test Result -----------------------
            Assert.IsTrue(gridControl.Grid.Enabled);
            //---------------Tear Down -------------------------
            TearDownForm(frm);
        }

        [Test]
        public void TestSelectTreeNode()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            IFormHabanero frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            IEditableGridControl gridControl = (IEditableGridControl)editor.Controls[0];
            editor.AddSection(TestUtil.GetRandomString());
            string itemName = TestUtil.GetRandomString();
            editor.AddItem(itemName, classDef);
             ITreeView treeView = (ITreeView) editor.Controls[1];
            //---------------Execute Test ----------------------
            treeView.SelectedNode = treeView.Nodes[0].Nodes[0];

            //---------------Test Result -----------------------
            Assert.IsNotNull(gridControl.Grid.BusinessObjectCollection);
            Assert.AreSame(classDef, gridControl.Grid.BusinessObjectCollection.ClassDef);
            //---------------Tear Down -------------------------
            TearDownForm(frm);
        }




        [Test]
        public void TestSelectTwoTreeNodes()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IClassDef classDef1 = MyBO.LoadDefaultClassDef();
            IClassDef classDef2 = ContactPersonTestBO.LoadDefaultClassDefWithUIDef();

            IFormHabanero frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            IEditableGridControl gridControl = (IEditableGridControl) editor.Controls[0];
            editor.AddSection(TestUtil.GetRandomString());
            string itemName1 = TestUtil.GetRandomString();
            editor.AddItem(itemName1, classDef1);
            string itemName2 = TestUtil.GetRandomString();
            editor.AddItem(itemName2, classDef2);
            editor.SelectItem(itemName1);

            //---------------Assert Preconditions---------------
            Assert.IsNotNull(gridControl.Grid.BusinessObjectCollection);
            Assert.AreSame(classDef1, gridControl.Grid.BusinessObjectCollection.ClassDef);

            //---------------Execute Test ----------------------
            editor.SelectItem(itemName2);

            //---------------Test Result -----------------------
            Assert.IsNotNull(gridControl.Grid.BusinessObjectCollection);
            Assert.AreSame(classDef2, gridControl.Grid.BusinessObjectCollection.ClassDef);
            //---------------Tear Down -------------------------
            TearDownForm(frm);
        }

        [Ignore("Changing how this works 04 March 2009 bbb")] 
        [Test]
        public virtual void TestSaveChanges()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IClassDef classDef1 = MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            myBO.Save();
            IFormHabanero frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            IEditableGridControl gridControl = (IEditableGridControl)editor.Controls[0];
            editor.AddSection(TestUtil.GetRandomString());
            string itemName1 = TestUtil.GetRandomString();
            editor.AddItem(itemName1, classDef1);
            editor.SelectItem(itemName1);
            gridControl.Grid.SelectedBusinessObject = myBO;
            string newValue = TestUtil.GetRandomString();
            //---------------Execute Test ----------------------
            gridControl.Grid.CurrentRow.Cells["TestProp"].Value = newValue;
            bool result = editor.SaveChanges();
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
            BusinessObjectCollection<MyBO> collection = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<MyBO>("");
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(newValue, myBO.TestProp);
            Assert.IsFalse(myBO.Status.IsDirty);
            //---------------Tear Down -------------------------
            TearDownForm(frm);
        }

        [Test]
        public virtual void TestRejectChanges()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IClassDef classDef1 = MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            string originalValue = TestUtil.GetRandomString();
            myBO.TestProp = originalValue;
            myBO.Save();
            IFormHabanero frm;
            IStaticDataEditor editor = CreateEditorOnForm(out frm);
            IEditableGridControl gridControl = (IEditableGridControl)editor.Controls[0];
            editor.AddSection(TestUtil.GetRandomString());
            string itemName1 = TestUtil.GetRandomString();
            editor.AddItem(itemName1, classDef1);
            editor.SelectItem(itemName1);

            //---------------Execute Test ----------------------
            gridControl.Grid.SelectedBusinessObject = myBO;
            string newValue = TestUtil.GetRandomString();
            gridControl.Grid.CurrentRow.Cells["TestProp"].Value = newValue;
            bool result = editor.RejectChanges();
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
            BusinessObjectCollection<MyBO> collection = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<MyBO>("");
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(originalValue, myBO.TestProp);
            Assert.IsFalse(myBO.Status.IsDirty);
            //---------------Tear Down -------------------------
            TearDownForm(frm);
        }
    }
}