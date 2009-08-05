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

using System;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Controllers
{

    [TestFixture]
    public class TestComboBoxCollectionControllerVWG : TestComboBoxCollectionManagerWin
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.VWG.ControlFactoryVWG();
        }
    }
    [TestFixture]
    public class TestComboBoxCollectionManagerWin
    {
        protected virtual IControlFactory GetControlFactory()
        {
            return new Habanero.UI.Win.ControlFactoryWin();
        }

        [Test]
        public void TestCreateTestComboBoxCollectionController()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithBoolean();
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO> {{new MyBO(), new MyBO()}};
            IComboBox cmb = GetControlFactory().CreateComboBox();
            ComboBoxCollectionSelector selector = new ComboBoxCollectionSelector(cmb,GetControlFactory());
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            selector.SetCollection(myBOs, false);
            //---------------Verify Result -----------------------
            Assert.AreEqual(myBOs, selector.Collection);
            Assert.AreSame(cmb,selector.Control);
            //---------------Tear Down -------------------------   
        }
        [Test]
        public void TestSetCollectionNull()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithBoolean();

            IComboBox cmb = GetControlFactory().CreateComboBox();
            ComboBoxCollectionSelector selector = new ComboBoxCollectionSelector(cmb, GetControlFactory());
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            selector.SetCollection(null, false);
            //---------------Verify Result -----------------------
            Assert.IsNull(selector.Collection);
            Assert.AreSame(cmb,selector.Control);
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            //---------------Execute Test ----------------------
            ComboBoxCollectionSelector selectorManger = new ComboBoxCollectionSelector(cmbox, controlFactory);
            //---------------Test Result -----------------------
            Assert.IsNotNull(selectorManger);
            Assert.AreSame(cmbox, selectorManger.Control);
            Assert.AreSame(controlFactory, selectorManger.ControlFactory);

            //---------------Tear Down -------------------------
        }
        [Test]
        public void Test_Constructor_NullControlFactoryRaisesError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            //---------------Execute Test ----------------------
            try
            {
                new ComboBoxCollectionSelector(cmbox, null);
                Assert.Fail("expected ArgumentNullException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("controlFactory", ex.ParamName);
            }
        }

        [Test]
        public void TestConstructor_NullComboBoxRaisesError()
        {
            //---------------Set up test pack-------------------
            IControlFactory controlFactory = GetControlFactory();
            //---------------Execute Test ----------------------
            try
            {
                new ComboBoxCollectionSelector(null, controlFactory);
                Assert.Fail("expected ArgumentNullException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("comboBox", ex.ParamName);
            }
        }

        [Test]
        public void TestSetComboBoxCollection()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector selectorManger = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), new MyBO(), new MyBO()};
            //---------------Execute Test ----------------------
            selectorManger.SetCollection(myBoCol, false);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, selectorManger.Collection.Count);
            Assert.AreEqual(3, selectorManger.Control.Items.Count);
        }
        [Test]
        public void TestSetComboBoxCollection_AddNullItemTrue()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector selectorManger = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), new MyBO(), new MyBO()};
            //---------------Execute Test ----------------------
            selectorManger.SetCollection(myBoCol, true);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, selectorManger.Collection.Count);
            Assert.AreEqual(4, selectorManger.Control.Items.Count);
        }
        [Test]
        public void TestSetComboBoxCollection_IncludeBlankFalse_SetsFirstItem()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector selectorManger = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            MyBO firstBo = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO> 
                    { firstBo, new MyBO(), new MyBO() };
            //---------------Execute Test ----------------------
            selectorManger.SetCollection(myBoCol, false);
            //---------------Test Result -----------------------
            Assert.AreSame(firstBo, selectorManger.SelectedBusinessObject);
        }
        [Test]
        public void TestSetComboBoxCollection_IncludeBlank_True()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector selectorManger = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            MyBO firstBo = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {firstBo, new MyBO(), new MyBO()};
            //---------------Execute Test ----------------------
            selectorManger.SetCollection(myBoCol, true);
            //---------------Test Result -----------------------
            Assert.AreSame(firstBo, selectorManger.SelectedBusinessObject);
            Assert.AreEqual(4, cmbox.Items.Count);
        }

        [Test]
        public void TestSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector selectorManger = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            MyBO selectedBO = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), selectedBO, new MyBO()};
            //---------------Execute Test ----------------------
            selectorManger.SetCollection(myBoCol, false);
            selectorManger.Control.SelectedIndex = 1;
            //---------------Test Result -----------------------
            Assert.AreEqual(selectedBO, selectorManger.SelectedBusinessObject);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestBusinessObejctAddedToCollection()
        {
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector selectorManger = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            MyBO addedBo = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), new MyBO(), new MyBO()};
            selectorManger.SetCollection(myBoCol, false);
            //---------------Execute Test ----------------------
            selectorManger.Collection.Add(addedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(4, selectorManger.Control.Items.Count);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestBusinessObejctRemovedFromCollection()
        {
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector selectorManger = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            MyBO removedBo = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), removedBo, new MyBO()};
            selectorManger.SetCollection(myBoCol, false);
            //---------------Execute Test ----------------------
            selectorManger.Collection.Remove(removedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, selectorManger.Control.Items.Count);
        }

        [Test]
        public void Test_EditItemFromCollection_UpdatesItemInCombo()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector selectorManger = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO> { { new MyBO(), new MyBO() } };
            selectorManger.SetCollection(myBOs, false);
            MyBO myBO = myBOs[0];
            string origToString = myBO.ToString();
            Guid newValue = Guid.NewGuid();
            int index = cmbox.Items.IndexOf(myBO);
            cmbox.SelectedIndex = index;
            //---------------Assert precondition----------------
            Assert.AreEqual(2, cmbox.Items.Count);
            Assert.AreEqual(0, cmbox.SelectedIndex);
            Assert.AreSame(myBO, cmbox.Items[index]);
            Assert.AreEqual(origToString, cmbox.Items[index].ToString());
            Assert.AreEqual(origToString, cmbox.Text);
            //---------------Execute Test ----------------------
            myBO.MyBoID = newValue;
            //---------------Test Result -----------------------
            string newToString = myBO.ToString();
            Assert.AreNotEqual(origToString, newToString);
            Assert.AreEqual(index, cmbox.SelectedIndex);
            //Assert.AreNotEqual(origToString, cmbox.Text);
            //Assert.AreEqual(newToString, cmbox.Text);
        }
        [Test]
        public void Test_EditSecondItemFromCollection_UpdatesItemInCombo()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector selectorManger = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO> { { new MyBO(), new MyBO() } };
            selectorManger.SetCollection(myBOs, false);
            MyBO myBO = myBOs[1];
            string origToString = myBO.ToString();
            Guid newValue = Guid.NewGuid();
            int index = cmbox.Items.IndexOf(myBO);
            cmbox.SelectedIndex = index;
            //---------------Assert precondition----------------
            Assert.AreEqual(2, cmbox.Items.Count);
            Assert.AreEqual(1, cmbox.SelectedIndex);
            Assert.AreSame(myBO, cmbox.Items[index]);
            Assert.AreEqual(origToString, cmbox.Items[index].ToString());
            Assert.AreEqual(origToString, cmbox.Text);
            //---------------Execute Test ----------------------
            myBO.MyBoID = newValue;
            //---------------Test Result -----------------------
            string newToString = myBO.ToString();
            Assert.AreNotEqual(origToString, newToString);
            Assert.AreEqual(index, cmbox.SelectedIndex);
            //Assert.AreNotEqual(origToString, cmbox.Text);
            //Assert.AreEqual(newToString, cmbox.Text);
        }
        [Test]
        public void Test_EditUnselectedItemFromCollection_UpdatesItemInCombo_DoesNotSelectItem()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector selectorManger = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO> { { new MyBO(), new MyBO() } };
            selectorManger.SetCollection(myBOs, false);
            MyBO myBO = myBOs[1];
            string origToString = myBO.ToString();
            Guid newValue = Guid.NewGuid();
            int index = cmbox.Items.IndexOf(myBO);
            //---------------Assert precondition----------------
            Assert.AreEqual(2, cmbox.Items.Count);
            Assert.AreEqual(0, cmbox.SelectedIndex);
            Assert.AreSame(myBO, cmbox.Items[index]);
            Assert.AreEqual(origToString, cmbox.Items[index].ToString());
            Assert.AreNotEqual(index, cmbox.SelectedIndex);
            //---------------Execute Test ----------------------
            myBO.MyBoID = newValue;
            //---------------Test Result -----------------------
            string newToString = myBO.ToString();
            Assert.AreNotEqual(origToString, newToString);
            Assert.AreEqual(0, cmbox.SelectedIndex);
        }
        [Test]
        public void Test_EditUnselectedItemFromCollection_UpdatesItemInCombo_DoesNotSelectItem_WithBlank()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector selectorManger = new ComboBoxCollectionSelector(cmbox, controlFactory, false);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO> { { new MyBO(), new MyBO() } };
            selectorManger.SetCollection(myBOs, true);
            MyBO myBO = myBOs[1];
            string origToString = myBO.ToString();
            Guid newValue = Guid.NewGuid();
            int index = cmbox.Items.IndexOf(myBO);
            //---------------Assert precondition----------------
            Assert.AreEqual(3, cmbox.Items.Count);
            Assert.AreEqual(-1, cmbox.SelectedIndex);
            Assert.AreSame(myBO, cmbox.Items[index]);
            Assert.AreEqual(origToString, cmbox.Items[index].ToString());
            Assert.AreNotEqual(index, cmbox.SelectedIndex);
            //---------------Execute Test ----------------------
            myBO.MyBoID = newValue;
            //---------------Test Result -----------------------
            string newToString = myBO.ToString();
            Assert.AreNotEqual(origToString, newToString);
            Assert.AreEqual(-1, cmbox.SelectedIndex);
//            Assert.AreNotEqual(origToString, cmbox.Text);
//            Assert.AreEqual(newToString, cmbox.Text);
        }

        [Test]
        public void Test_CancelEditsItemFromCollection_UpdatesItemInCombo()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector selectorManger = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO> { { new MyBO(), new MyBO() } };
            selectorManger.SetCollection(myBOs, false);
            MyBO myBO = myBOs[0];
            Guid newValue = Guid.NewGuid();
            int index = cmbox.Items.IndexOf(myBO);
            cmbox.SelectedIndex = index;
            myBO.MyBoID = newValue;
            myBO.Save();
            //---------------Assert precondition----------------
            Assert.AreEqual(2, cmbox.Items.Count);
            Assert.AreEqual(0, cmbox.SelectedIndex);
            Assert.AreSame(myBO, cmbox.Items[index]);
            Assert.AreEqual(index, cmbox.SelectedIndex);
            //---------------Execute Test ----------------------
            myBO.MyBoID = Guid.NewGuid();
            myBO.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cmbox.SelectedIndex);
            Assert.AreEqual(index, cmbox.SelectedIndex);
            Assert.AreSame(myBO, cmbox.Items[index]);
            string newToString = myBO.ToString();
            Assert.AreEqual(newToString, cmbox.Items[index].ToString());

        }

    }
}
