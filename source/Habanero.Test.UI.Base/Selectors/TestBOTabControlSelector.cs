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
    public class TestBOTabControlSelectorVWG : TestBOTabControlSelectorWin
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override IBOSelector CreateSelector()
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

        [Ignore(" This test is not working on VWG")] //TODO Brett 03 Mar 2009:
        [Test]
        public override void Test_SelectedBusinessObject_ReturnsNullIfNoItemSelected()
        {
            //---------------Set up test pack-------------------
            IBOSelector selector = CreateSelector();
            MyBO myBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO };
            selector.BusinessObjectCollection = collection;
            selector.SelectedBusinessObject = null;
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            IBusinessObject selectedBusinessObject = selector.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.IsNull(selectedBusinessObject);
        }
        [Ignore(" This test is not working on VWG")] //TODO Brett 03 Mar 2009:
        [Test]
        public override void Test_Set_SelectedBusinessObject_Null_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOSelector selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, myBO2 };
            selector.BusinessObjectCollection = collection;
            SetSelectedIndex(selector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(selector));
            Assert.AreEqual(myBO2, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.SelectedBusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(selector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(selector));
        }
        [Ignore(" This test is not working on VWG")] //TODO Brett 03 Mar 2009:
        [Test]
        public override void Test_Set_SelectedBusinessObject_ItemNotInList_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOSelector selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, myBO2 };
            selector.BusinessObjectCollection = collection;
            SetSelectedIndex(selector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(selector));
            Assert.AreEqual(myBO2, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.SelectedBusinessObject = new MyBO();
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualIndex(2), selector.NoOfItems, "The blank item");
            Assert.IsNull(selector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(selector));
        }
    }


    /// <summary>
    /// This test class tests the BOTabControlSelector class.
    /// </summary>
    [TestFixture]
    public class TestBOTabControlSelectorWin : TestBOSelector
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override void SetSelectedIndex(IBOSelector selector, int index)
        {
            IBOColTabControl groupControl = ((IBOColTabControl)selector);
            groupControl.TabControl.SelectedIndex = index;
        }

        protected override int SelectedIndex(IBOSelector selector)
        {

            IBOColTabControl groupControl = ((IBOColTabControl) selector);
            return groupControl.TabControl.SelectedIndex;
        }

        protected override IBOSelector CreateSelector()
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
        public void Test_Constructor_BOTabControlSet()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOSelector selector = CreateSelector();
            //---------------Test Result -----------------------
            Assert.IsNotNull(selector);
            Assert.IsInstanceOfType(typeof (IBOColTabControl), selector);
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

        [Ignore(" Not Yet implemented")] //TODO  01 Mar 2009:
        [Test]
        public void TestEditItemFromCollectionUpdatesItemInSelector()
        {
            Assert.Fail("Not yet implemented");
        }
        [Test]
        public override void Test_AutoSelectsFirstItem()
        {
            //---------------Set up test pack-------------------
            IBOSelector selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, myBO2 };
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, selector.NoOfItems);
            Assert.AreEqual(0, SelectedIndex(selector));
            Assert.AreEqual(null, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item");
            Assert.AreSame(myBO, selector.SelectedBusinessObject);
            Assert.AreEqual(ActualIndex(0), SelectedIndex(selector));

        }
        [Test]
        public override void Test_AutoSelectsFirstItem_NoItems()
        {
            //---------------Set up test pack-------------------
            IBOSelector selector = CreateSelector();
            new MyBO();//Purely to load the ClassDefs.
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, selector.NoOfItems);
            Assert.AreEqual(0, SelectedIndex(selector));
            Assert.AreEqual(null, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreEqual(NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item");
            Assert.AreSame(null, selector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(selector));
        }
        [Test]
        public override void Test_SelectedBusinessObject_ReturnsNullIfNoItemSelected()
        {
            //---------------Set up test pack-------------------
            //The control is being swapped out 
                    // onto each tab i.e. all the tabs use the Same BusinessObjectControl
                    // setting the selected Bo to null is therefore not a particularly 
                    // sensible action on a BOTabControl.
            IBOSelector selector = CreateSelector();
            MyBO myBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO };
            selector.BusinessObjectCollection = collection;
            selector.SelectedBusinessObject = null;
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            IBusinessObject selectedBusinessObject = selector.SelectedBusinessObject;
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
            IBOSelector selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, myBO2 };
            selector.BusinessObjectCollection = collection;
            SetSelectedIndex(selector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(selector));
            Assert.AreEqual(myBO2, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.SelectedBusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNotNull(selector.SelectedBusinessObject);
            Assert.AreEqual(ActualIndex(1), SelectedIndex(selector));
        }
    }
}