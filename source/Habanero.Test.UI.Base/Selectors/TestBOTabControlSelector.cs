using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Currently Ignored
    /// </summary>
    /// 
    [TestFixture]
    public class TestBOSelectorBOTabVwg : TestBOSelectorBOTabWin
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override IBOColSelectorControl CreateSelector()
        {
            IBOColTabControl control = GetControlFactory().CreateBOColTabControl();
            control.BusinessObjectControl = this.GetBusinessObjectControlStub();
            return control;
        }
        protected override IBusinessObjectControl GetBusinessObjectControlStub()
        {
            return new TestBOColTabControl.BusinessObjectControlVWGStub();
        }
        [Test]
        public override void Test_Constructor_nullControlFactory_RaisesError()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new BOColTabControlVWG(null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("controlFactory", ex.ParamName);
            }
        }

        [Ignore(" This test is not working on VWG: Brett 03 Mar 2009:")] //TODO 
        [Test]
        public override void Test_SelectedBusinessObject_ReturnsNullIfNoItemSelected()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            MyBO myBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO };
            colSelector.BusinessObjectCollection = collection;
            colSelector.SelectedBusinessObject = null;
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            IBusinessObject selectedBusinessObject = colSelector.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.IsNull(selectedBusinessObject);
        }
        [Ignore(" This test is not working on VWG : Brett 03 Mar 2009:")] //TODO Brett 03 Mar 2009:
        [Test]
        public override void Test_Set_SelectedBusinessObject_Null_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, myBO2 };
            colSelector.BusinessObjectCollection = collection;
            SetSelectedIndex(colSelector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(colSelector));
            Assert.AreEqual(myBO2, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.SelectedBusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(colSelector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
        }
        [Ignore(" This test is not working on VWG Brett 03 Mar 2009:")] //TODO Brett 03 Mar 2009:
        [Test]
        public override void Test_Set_SelectedBusinessObject_ItemNotInList_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, myBO2 };
            colSelector.BusinessObjectCollection = collection;
            SetSelectedIndex(colSelector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(colSelector));
            Assert.AreEqual(myBO2, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.SelectedBusinessObject = new MyBO();
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualIndex(2), colSelector.NoOfItems, "The blank item");
            Assert.IsNull(colSelector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
        }
    }


    /// <summary>
    /// This test class tests the BOTabControlSelector class.
    /// </summary>
    [TestFixture]
    public class TestBOSelectorBOTabWin : TestBOColSelector
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override void SetSelectedIndex(IBOColSelectorControl colSelector, int index)
        {
            IBOColTabControl groupControl = ((IBOColTabControl)colSelector);
            groupControl.TabControl.SelectedIndex = index;
        }

        protected override int SelectedIndex(IBOColSelectorControl colSelector)
        {

            IBOColTabControl groupControl = ((IBOColTabControl) colSelector);
            return groupControl.TabControl.SelectedIndex;
        }

        protected override IBOColSelectorControl CreateSelector()
        {
            IBOColTabControl control = GetControlFactory().CreateBOColTabControl();
            control.BusinessObjectControl = this.GetBusinessObjectControlStub();
            return control;
        }
        protected virtual IBusinessObjectControl GetBusinessObjectControlStub()
        {
            return new TestBOColTabControl.BusinessObjectControlWinStub();
        }

        protected override int NumberOfLeadingBlankRows()
        {
            return 0;
        }

        [Test]
        public virtual void Test_Constructor_BOTabControlSet()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOColSelectorControl colSelector = CreateSelector();
            //---------------Test Result -----------------------
            Assert.IsNotNull(colSelector);
            Assert.IsInstanceOfType(typeof (IBOColTabControl), colSelector);
//            Assert.IsInstanceOfType(typeof (IBOColTabControl), selector);
        }

        [Test]
        public virtual void Test_Constructor_nullControlFactory_RaisesError()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new BOColTabControlWin(null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("controlFactory", ex.ParamName);
            }
        }

        [Ignore(" Not Yet implemented : Brett 03 Mar 2009:")] //TODO  01 Mar 2009:
        [Test]
        public void TestEditItemFromCollectionUpdatesItemInSelector()
        {
            Assert.Fail("Not yet implemented");
        }
        [Test]
        public override void Test_AutoSelectsFirstItem()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject myBO;
            IBusinessObjectCollection collection = GetCollectionWithTowBOs(out myBO);            //---------------Assert Precondition----------------
            Assert.AreEqual(1, colSelector.NoOfItems);
            Assert.AreEqual(0, SelectedIndex(colSelector));
            Assert.AreEqual(null, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item");
            Assert.AreSame(myBO, colSelector.SelectedBusinessObject);
            Assert.AreEqual(ActualIndex(0), SelectedIndex(colSelector));

        }
        [Test]
        public override void Test_AutoSelectsFirstItem_NoItems()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObjectCollection collection = GetCollectionWithNoItems();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, colSelector.NoOfItems);
            Assert.AreEqual(0, SelectedIndex(colSelector));
            Assert.AreEqual(null, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreEqual(NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item");
            Assert.AreSame(null, colSelector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
        }
        [Test]
        public override void Test_SelectedBusinessObject_ReturnsNullIfNoItemSelected()
        {
            //---------------Set up test pack-------------------
            //The control is being swapped out 
                    // onto each tab i.e. all the tabs use the Same BusinessObjectControl
                    // setting the selected Bo to null is therefore not a particularly 
                    // sensible action on a BOTabControl.
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject bo;
            IBusinessObjectCollection collection = GetCollectionWithOneBO(out bo);
            colSelector.BusinessObjectCollection = collection;
            colSelector.SelectedBusinessObject = null;
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            IBusinessObject selectedBusinessObject = colSelector.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.IsNotNull(selectedBusinessObject);
        }
        [Test]
        public override void Test_Set_SelectedBusinessObject_Null_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            //The control is being swapped out 
            // onto each tab i.e. all the tabs use the Same BusinessObjectControl
            // setting the selected Bo to null is therefore not a particularly 
            // sensible action on a BOTabControl.
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject myBO;
            IBusinessObjectCollection collection = GetCollectionWithTowBOs(out myBO);
            IBusinessObject myBO2 = collection[1]; colSelector.BusinessObjectCollection = collection;
            SetSelectedIndex(colSelector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(colSelector));
            Assert.AreEqual(myBO2, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.SelectedBusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNotNull(colSelector.SelectedBusinessObject);
            Assert.AreEqual(ActualIndex(1), SelectedIndex(colSelector));
        }
    }
}