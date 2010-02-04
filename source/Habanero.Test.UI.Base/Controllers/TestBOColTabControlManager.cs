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
        protected override IBusinessObjectControl BusinessObjectControlCreator()
        {
            return new BusinessObjectControlStubVWG();
        }
        protected override Type TypeOfBusienssObjectControl()
        {
            return typeof(BusinessObjectControlStubVWG);
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
        public void Test_BusinessObejctAddedToCollection()
        {
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            BOColTabControlManager selectorManger = CreateBOTabControlManager(tabControl);
            MyBO addedBo = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), new MyBO(), new MyBO()};
            selectorManger.BusinessObjectCollection = myBoCol;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, selectorManger.TabControl.TabPages.Count);
            //---------------Execute Test ----------------------
            selectorManger.BusinessObjectCollection.Add(addedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(4, selectorManger.TabControl.TabPages.Count);
            Assert.AreEqual(addedBo.ToString(), selectorManger.TabControl.TabPages[3].Text);
            Assert.AreEqual(addedBo.ToString(), selectorManger.TabControl.TabPages[3].Name);
        }

        [Test]
        public void Test_WhenUsingCreator_WhenBusinessObejctAddedToCollection_ShouldAddTab()
        {
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManger = new BOColTabControlManager(tabControl, controlFactory);
            BusinessObjectControlCreatorDelegate creator = BusinessObjectControlCreator;
            selectorManger.BusinessObjectControlCreator = creator;

            MyBO addedBo = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO> { new MyBO(), new MyBO(), new MyBO() };
            selectorManger.BusinessObjectCollection = myBoCol;
            bool pageAddedEventFired = false;
            TabPageEventArgs ex = null;
            selectorManger.TabPageAdded += (sender,e) =>
                                           {
                                               pageAddedEventFired = true;
                                               ex = e;
                                           };
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, selectorManger.TabControl.TabPages.Count);
            Assert.IsFalse(pageAddedEventFired);
            //---------------Execute Test ----------------------
            selectorManger.BusinessObjectCollection.Add(addedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(4, selectorManger.TabControl.TabPages.Count);
            ITabPage tabPage = selectorManger.TabControl.TabPages[3];
            Assert.AreEqual(addedBo.ToString(), tabPage.Text);
            Assert.AreEqual(addedBo.ToString(), tabPage.Name);
            Assert.AreEqual(1,tabPage.Controls.Count);
            IControlHabanero boControl = tabPage.Controls[0];
            Assert.IsTrue(pageAddedEventFired);
            Assert.IsNotNull(ex);
            Assert.AreSame(tabPage, ex.TabPage);
            Assert.AreSame(boControl, ex.BOControl);
        }

        [Test]
        public void Test_WhenUsingCreator_WhenBusinessObejctRemovedToCollection_ShouldRemoveTab()
        {
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManger = new BOColTabControlManager(tabControl, controlFactory);
            BusinessObjectControlCreatorDelegate creator = BusinessObjectControlCreator;
            selectorManger.BusinessObjectControlCreator = creator;

            MyBO removedBo = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO> { removedBo, new MyBO(), new MyBO() };
            selectorManger.BusinessObjectCollection = myBoCol;
            bool pageRemovedEventFired = false;
            TabPageEventArgs ex = null;
            selectorManger.TabPageRemoved += (sender, e) =>
            {
                pageRemovedEventFired = true;
                ex = e;
            };
            ITabPage tabPage = selectorManger.TabControl.TabPages[0];
            IControlHabanero boControlToBeRemoved = tabPage.Controls[0];
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, selectorManger.TabControl.TabPages.Count);
            Assert.IsFalse(pageRemovedEventFired);
            //---------------Execute Test ----------------------
            selectorManger.BusinessObjectCollection.Remove(removedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, selectorManger.TabControl.TabPages.Count);
            
            Assert.AreEqual(removedBo.ToString(), tabPage.Text);
            Assert.AreEqual(removedBo.ToString(), tabPage.Name);
            Assert.AreEqual(1, tabPage.Controls.Count);
            
            Assert.IsTrue(pageRemovedEventFired);
            Assert.IsNotNull(ex);
            Assert.AreSame(tabPage, ex.TabPage);
            Assert.AreSame(boControlToBeRemoved, ex.BOControl);
        }

        [Test]
        public void Test_WhenUsingCreator_SetUpBOTabColManagerWithDelegateForCreating_aBOControl()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManger = new BOColTabControlManager(tabControl, controlFactory);
            BusinessObjectControlCreatorDelegate creator = BusinessObjectControlCreator;
            //---------------Assert Precondition----------------
            Assert.IsNull(selectorManger.BusinessObjectControlCreator);
            //---------------Execute Test ----------------------
            selectorManger.BusinessObjectControlCreator = creator;
            //---------------Test Result -----------------------
            Assert.IsNotNull(selectorManger.BusinessObjectControlCreator);
            Assert.AreEqual(creator, selectorManger.BusinessObjectControlCreator);
        }

        [Test]
        public void Test_WhenUsingCreator_WhenSetBOCol_ShouldCreateTabPageWithControlFromCreator()
        {
            //---------------Set up test pack-------------------
            BusinessObjectControlCreatorDelegate creator;
            BOColTabControlManager selectorManger = GetSelectorManger(out creator);
            MyBO expectedBO = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {expectedBO};
            //---------------Assert Precondition----------------
            Assert.IsNotNull(selectorManger.BusinessObjectControlCreator);
            Assert.AreEqual(creator, selectorManger.BusinessObjectControlCreator);
            Assert.AreEqual(0, selectorManger.TabControl.TabPages.Count);
            //---------------Execute Test ----------------------
            selectorManger.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, selectorManger.TabControl.TabPages.Count);
            ITabPage page = selectorManger.TabControl.TabPages[0];
            Assert.AreEqual(1, page.Controls.Count);
            IControlHabanero boControl = page.Controls[0];
            Assert.IsInstanceOfType(TypeOfBusienssObjectControl(), boControl);
            IBusinessObjectControl businessObjectControl = (IBusinessObjectControl) boControl;
            Assert.AreSame(expectedBO, businessObjectControl.BusinessObject);
            Assert.AreSame(boControl, selectorManger.BusinessObjectControl);
        }

        protected virtual Type TypeOfBusienssObjectControl()
        {
            return typeof(BusinessObjectControlStubWin);
        }

        private BOColTabControlManager GetSelectorManger(out BusinessObjectControlCreatorDelegate creator)
        {
            IControlFactory controlFactory = GetControlFactory();
            ITabControl tabControl = controlFactory.CreateTabControl();
            BOColTabControlManager selectorManger = new BOColTabControlManager(tabControl, controlFactory);
            creator = BusinessObjectControlCreator;
            selectorManger.BusinessObjectControlCreator = creator;
            return selectorManger;
        }

        [Test]
        public void Test_WhenChangeTabIndex_ShouldNotRecreateTheBOControl()
        {
            //---------------Set up test pack-------------------
            BusinessObjectControlCreatorDelegate creator;
            BOColTabControlManager selectorManger = GetSelectorManger(out creator);

            MyBO firstBO = new MyBO();
            MyBO secondBO = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO> { firstBO, secondBO };
            selectorManger.BusinessObjectCollection = myBoCol;
            ITabPage firstTabPage = selectorManger.TabControl.TabPages[0];
            IBusinessObjectControl firstBOControl = (IBusinessObjectControl)firstTabPage.Controls[0];
            ITabPage secondTabPage = selectorManger.TabControl.TabPages[1];
            IBusinessObjectControl secondBOControl = (IBusinessObjectControl)secondTabPage.Controls[0];
            //---------------Assert Precondition----------------
            Assert.AreSame(secondBO, secondBOControl.BusinessObject);
            Assert.AreSame(firstBOControl, selectorManger.BusinessObjectControl);
            Assert.AreEqual(2, selectorManger.TabControl.TabPages.Count);
            Assert.AreEqual(0, selectorManger.TabControl.SelectedIndex);
            //---------------Execute Test ----------------------
            selectorManger.CurrentBusinessObject = secondBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, selectorManger.TabControl.SelectedIndex);
            Assert.AreSame(secondBOControl, secondTabPage.Controls[0]);
            Assert.AreSame(secondBOControl, selectorManger.BusinessObjectControl);
        }

        protected virtual IBusinessObjectControl BusinessObjectControlCreator()
        {
            return new BusinessObjectControlStubWin();
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