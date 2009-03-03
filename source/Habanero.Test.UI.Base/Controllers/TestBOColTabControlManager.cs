using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Controllers
{
    [TestFixture]
    public class TestBOColTabControlManagerVWG:TestBOColTabControlManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.VWG.ControlFactoryVWG();
        }
        protected override IBusinessObjectControl GetBusinessObjectControl()
        {
            return new BusinessObjectControlStubVWG();
        }
    }
    [TestFixture]
    public class TestBOColTabControlManager
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }
        protected virtual IControlFactory GetControlFactory()
        {
            return new Habanero.UI.Win.ControlFactoryWin();
        }

        protected virtual IBusinessObjectControl GetBusinessObjectControl()
        {
            return new BusinessObjectControlStubWin();
        }

        [Test]
        public void Test_Create_tabControlNull_ExpectException()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new BOColTabControlManager(null, GetControlFactory());
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("tabControl", ex.ParamName);
            }
        }

        [Test]
        public void Test_Create_controlFactoryNull_ExpectException()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new BOColTabControlManager(tabControl, null);
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
        public void TestCreateTestTabControlCollectionController()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO> {{new MyBO(), new MyBO()}};
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, GetControlFactory());
            selectorManager.BusinessObjectControl = GetBusinessObjectControl();
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectCollection = myBOs;
            //---------------Verify Result -----------------------
            Assert.AreEqual(myBOs, selectorManager.BusinessObjectCollection);
            Assert.AreSame(tabControl, selectorManager.TabControl);
            //---------------Tear Down -------------------------   
        }

        [Test]
        public void TestSetCollectionNull()
        {
            //---------------Set up test pack-------------------

            ITabControl tabControl = GetControlFactory().CreateTabControl();
            BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, GetControlFactory());
            selectorManager.BusinessObjectControl = GetBusinessObjectControl();
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectCollection = null;
            //---------------Verify Result -----------------------
            Assert.IsNull(selectorManager.BusinessObjectCollection);
            Assert.AreSame(tabControl, selectorManager.TabControl);
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            //---------------Execute Test ----------------------
            BOColTabControlManager selectorManger = new BOColTabControlManager(tabControl, controlFactory);
            //---------------Test Result -----------------------
            Assert.IsNotNull(selectorManger);
            Assert.AreSame(tabControl, selectorManger.TabControl);
//            Assert.AreSame(controlFactory, selectorManger.ControlFactory);

        }

        [Test]
        public void TestSetTabControlCollection()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManger = new BOColTabControlManager(tabControl, controlFactory);
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), new MyBO(), new MyBO()};
            selectorManger.BusinessObjectControl = GetBusinessObjectControl();
            //---------------Execute Test ----------------------
            selectorManger.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreEqual(3, selectorManger.BusinessObjectCollection.Count);
            Assert.AreEqual(3, selectorManger.TabControl.TabPages.Count);
        }

        [Test]
        public void Test_Set_Collection_WhenBOControlNotSet_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManger = new BOColTabControlManager(tabControl, controlFactory);
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), new MyBO(), new MyBO()};
            //---------------Execute Test ----------------------
            try
            {
                selectorManger.BusinessObjectCollection = myBoCol;
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                const string expectedMessage = "You cannot set the 'BusinessObjectCollection' for BOColTabControlManager since the BusinessObjectControl has not been set";
                StringAssert.Contains(expectedMessage, ex.Message);
            }
        }

        [Test]
        public void TestSetTabControlCollection_AddNullItemTrue()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManger = new BOColTabControlManager(tabControl, controlFactory);
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), new MyBO(), new MyBO()};
            selectorManger.BusinessObjectControl = GetBusinessObjectControl();
            //---------------Execute Test ----------------------
            selectorManger.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreEqual(3, selectorManger.BusinessObjectCollection.Count);
            Assert.AreEqual(3, selectorManger.TabControl.TabPages.Count);
        }

        [Test]
        public void TestSetTabControlCollection_IncludeBlankFalse_SetsFirstItem()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManger = new BOColTabControlManager(tabControl, controlFactory);
            MyBO firstBo = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {firstBo, new MyBO(), new MyBO()};
            selectorManger.BusinessObjectControl = GetBusinessObjectControl();
            //---------------Execute Test ----------------------
            selectorManger.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreSame(firstBo, selectorManger.CurrentBusinessObject);
        }

        [Test]
        public void TestSetTabControlCollection_IncludeBlank_True()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManger = new BOColTabControlManager(tabControl, controlFactory);
            MyBO firstBo;
            BusinessObjectCollection<MyBO> myBoCol = GetMyBoCol_3Items(out firstBo);
            selectorManger.BusinessObjectControl = GetBusinessObjectControl();
            //---------------Execute Test ----------------------
            selectorManger.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreSame(firstBo, selectorManger.CurrentBusinessObject);
        }

        private static BusinessObjectCollection<MyBO> GetMyBoCol_3Items(out MyBO firstBo)
        {
            firstBo = new MyBO();
            return new BusinessObjectCollection<MyBO>
                       {firstBo, new MyBO(), new MyBO()};
        }

        [Test]
        public void TestSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManger = new BOColTabControlManager(tabControl, controlFactory);
            MyBO selectedBO = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), selectedBO, new MyBO()};
            selectorManger.BusinessObjectControl = GetBusinessObjectControl();
            //---------------Execute Test ----------------------
            selectorManger.BusinessObjectCollection = myBoCol;
            selectorManger.TabControl.SelectedIndex = 1;
            //---------------Test Result -----------------------
            Assert.AreEqual(selectedBO, selectorManger.CurrentBusinessObject);
        }
        [Test]
        public void Test_Set_SelectedBusinessObject_BOColNull_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManger = new BOColTabControlManager(tabControl, controlFactory);
            MyBO selectedBO = new MyBO();
            selectorManger.BusinessObjectControl = this.GetBusinessObjectControl();
            selectorManger.BusinessObjectCollection = null;
            //---------------Assert Precondition----------------
            Assert.IsNull(selectorManger.BusinessObjectCollection);
            Assert.IsNotNull(selectorManger.BusinessObjectControl);
            //---------------Execute Test ----------------------
            try
            {
                selectorManger.CurrentBusinessObject = selectedBO;
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                const string expectedMessage = "You cannot set the 'CurrentBusinessObject' for BOColTabControlManager since the BusinessObjectCollection has not been set";
                StringAssert.Contains(expectedMessage, ex.Message);
            }
        }
        [Test]
        public void Test_Set_SelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManger = new BOColTabControlManager(tabControl, controlFactory);
            MyBO selectedBO = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), selectedBO, new MyBO()};
            selectorManger.BusinessObjectControl = GetBusinessObjectControl();
            //---------------Execute Test ----------------------
            selectorManger.BusinessObjectCollection = myBoCol;
            selectorManger.CurrentBusinessObject = selectedBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(selectedBO, selectorManger.CurrentBusinessObject);
        }

        [Test]
        public void TestBusinessObejctAddedToCollection()
        {
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            BOColTabControlManager selectorManger = CreateBOTabControlManager(tabControl);
            MyBO addedBo = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), new MyBO(), new MyBO()};
            selectorManger.BusinessObjectCollection = myBoCol;
            //---------------Execute Test ----------------------
            selectorManger.BusinessObjectCollection.Add(addedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(4, selectorManger.TabControl.TabPages.Count);
            //---------------Tear down -------------------------
        }

        private BOColTabControlManager CreateBOTabControlManager(ITabControl tabControl)
        {
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManger = new BOColTabControlManager(tabControl, controlFactory);
            selectorManger.BusinessObjectControl = GetBusinessObjectControl();
            return selectorManger;
        }

        [Test]
        public void TestBusinessObejctRemovedFromCollection()
        {
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            BOColTabControlManager selectorManger = CreateBOTabControlManager(tabControl);
            MyBO removedBo = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), removedBo, new MyBO()};
            selectorManger.BusinessObjectCollection = myBoCol;
            //---------------Execute Test ----------------------
            selectorManger.BusinessObjectCollection.Remove(removedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, selectorManger.TabControl.TabPages.Count);
        }
        [Test]
        public virtual void Test_SelectedBusinessObject_Null_DoesNothing()
        {
            //---------------Set up test pack-------------------
            //The control is being swapped out 
            // onto each tab i.e. all the tabs use the Same BusinessObjectControl
            // setting the selected Bo to null is therefore not a particularly 
            // sensible action on a BOTabControl.t up test pack-------------------
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            BOColTabControlManager selectorManger = CreateBOTabControlManager(tabControl);
            MyBO myBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO };
            selectorManger.BusinessObjectCollection = collection;
            selectorManger.CurrentBusinessObject = null;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IBusinessObject selectedBusinessObject = selectorManger.CurrentBusinessObject;
            //---------------Test Result -----------------------
            Assert.IsNotNull(selectedBusinessObject);
        }
        [Test]
        public void Test_Selector_Clear_ClearsItems()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            BOColTabControlManager selectorManger = CreateBOTabControlManager(tabControl);
            MyBO bo;
            BusinessObjectCollection<MyBO> col= GetMyBoCol_3Items(out bo);
            selectorManger.BusinessObjectCollection = col;
            //---------------Assert Preconditions --------------
            Assert.IsNotNull(selectorManger.CurrentBusinessObject);
            Assert.IsNotNull(selectorManger.BusinessObjectCollection);
            //---------------Execute Test ----------------------
            selectorManger.Clear();
            //---------------Test Result -----------------------
            Assert.IsNull(selectorManger.BusinessObjectCollection);
            Assert.IsNull(selectorManger.CurrentBusinessObject);
            Assert.AreEqual(0, selectorManger.NoOfItems);
        }

    }
}