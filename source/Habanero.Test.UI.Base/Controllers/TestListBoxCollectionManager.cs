using System;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Controllers
{
    [TestFixture]
        public class TestListBoxCollectionManagerVWG : TestListBoxCollectionManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.VWG.ControlFactoryVWG();
        }
    }

    [TestFixture]
    public class TestListBoxCollectionManager
    {
        protected virtual IControlFactory GetControlFactory()
        {
            return new Habanero.UI.Win.ControlFactoryWin();
        }

        [Test]
        public void TestCreateTestListBoxCollectionController()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithBoolean();
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO>(); 
            MyBO myBO1 = new MyBO();
            MyBO myBO2 = new MyBO();
            myBOs.Add(myBO1,myBO2);
            IListBox cmb = GetControlFactory().CreateListBox();
            ListBoxCollectionManager selector = new ListBoxCollectionManager(cmb, GetControlFactory());
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

            IListBox cmb = GetControlFactory().CreateListBox();
            ListBoxCollectionManager selector = new ListBoxCollectionManager(cmb, GetControlFactory());
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
            IListBox listBox = GetControlFactory().CreateListBox();
            IControlFactory controlFactory = GetControlFactory();
            //---------------Execute Test ----------------------
            ListBoxCollectionManager manager = new ListBoxCollectionManager(listBox, controlFactory);
            //---------------Test Result -----------------------
            Assert.IsNotNull(manager);
            Assert.AreSame(listBox, manager.Control);
            Assert.AreSame(controlFactory, manager.ControlFactory);

            //---------------Tear Down -------------------------
        }
        [Test]
        public void Test_Constructor_NullControlFactoryRaisesError()
        {
            //---------------Set up test pack-------------------
            IListBox listBox = GetControlFactory().CreateListBox();
            //---------------Execute Test ----------------------
            try
            {
                new ListBoxCollectionManager(listBox, null);
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
        public void TestConstructor_NullListBoxRaisesError()
        {
            //---------------Set up test pack-------------------
            IControlFactory controlFactory = GetControlFactory();
            //---------------Execute Test ----------------------
            try
            {
                new ListBoxCollectionManager(null, controlFactory);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("listBox", ex.ParamName);
            }
        }

        [Test]
        public void TestSetListBoxCollection()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IListBox listBox = GetControlFactory().CreateListBox();
            IControlFactory controlFactory = GetControlFactory();
            ListBoxCollectionManager manager = new ListBoxCollectionManager(listBox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), new MyBO(), new MyBO()};
            //---------------Execute Test ----------------------
            manager.SetCollection(myBoCol, false);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, manager.Collection.Count);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IListBox listBox = GetControlFactory().CreateListBox();
            IControlFactory controlFactory = GetControlFactory();
            ListBoxCollectionManager manager = new ListBoxCollectionManager(listBox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO selectedBO = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(selectedBO);
            myBoCol.Add(new MyBO());
            //---------------Execute Test ----------------------
            manager.SetCollection(myBoCol, false);
            manager.Control.SelectedIndex = 1;
            //---------------Test Result -----------------------
            Assert.AreEqual(selectedBO, manager.SelectedBusinessObject);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestBusinessObejctAddedToCollection()
        {
            ClassDef.ClassDefs.Clear();
            IListBox listBox = GetControlFactory().CreateListBox();
            IControlFactory controlFactory = GetControlFactory();
            ListBoxCollectionManager manager = new ListBoxCollectionManager(listBox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO addedBo = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            manager.SetCollection(myBoCol, false);
            //---------------Execute Test ----------------------
            manager.Collection.Add(addedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(4, manager.Control.Items.Count);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestBusinessObejctRemovedFromCollection()
        {
            ClassDef.ClassDefs.Clear();
            IListBox listBox = GetControlFactory().CreateListBox();
            IControlFactory controlFactory = GetControlFactory();
            ListBoxCollectionManager manager = new ListBoxCollectionManager(listBox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO removedBo = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(removedBo);
            myBoCol.Add(new MyBO());
            manager.SetCollection(myBoCol, false);
            //---------------Execute Test ----------------------
            manager.Collection.Remove(removedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, manager.Control.Items.Count);
        }
    }
}