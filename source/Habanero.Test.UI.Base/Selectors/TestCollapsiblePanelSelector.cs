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
//    [Ignore(" To be implemented")] //TODO Brett 02 Mar 2009:
//    [TestFixture]
//    public class TestCollapsiblePanelSelectorVWG : TestCollapsiblePanelSelectorWin
//    {
//        protected override IControlFactory GetControlFactory()
//        {
//            ControlFactoryVWG factory = new ControlFactoryVWG();
//            GlobalUIRegistry.ControlFactory = factory;
//            return factory;
//        }
//
//        protected override IBOSelector CreateSelector()
//        {
//            return GetControlFactory().CreateCollapsiblePanelSelector();
//        }
//
//        [Test]
//        public override void Test_Constructor_nullControlFactory_RaisesError()
//        {
//            //---------------Set up test pack-------------------
//
//            //---------------Assert Precondition----------------
//
//            //---------------Execute Test ----------------------
//            try
//            {
//                new CollapsiblePanelSelectorVWG(null);
//                Assert.Fail("expected ArgumentNullException");
//            }
//                //---------------Test Result -----------------------
//            catch (ArgumentNullException ex)
//            {
//                StringAssert.Contains("Value cannot be null", ex.Message);
//                StringAssert.Contains("controlFactory", ex.ParamName);
//            }
//        }
//    }


    /// <summary>
    /// This test class tests the CollapsiblePanelSelector class.
    /// </summary>
    [TestFixture]
    public class TestCollapsiblePanelSelectorWin : TestBOSelector
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }


        protected override void SetSelectedIndex(IBOSelector selector, int index)
        {
            ICollapsiblePanelGroupControl groupControl = ((ICollapsiblePanelGroupControl) selector);
            groupControl.AllCollapsed = true;
            ((ICollapsiblePanelGroupControl) selector).PanelsList[index].Collapsed = false;
        }

        protected override int SelectedIndex(IBOSelector selector)
        {
            ICollapsiblePanelGroupControl groupControl = ((ICollapsiblePanelGroupControl) selector);
            int count = 0;
            foreach (ICollapsiblePanel panel in groupControl.PanelsList)
            {
                if (!panel.Collapsed)
                {
                    return count;
                }
                count++;
            }
            return -1;
//            return groupControl. .SelectedIndex;
////            return 0;
        }

        protected override IBOSelector CreateSelector()
        {
            return GetControlFactory().CreateCollapsiblePanelSelector();
        }

        protected override int NumberOfLeadingBlankRows()
        {
            return 0;
        }

        [Test]
        public void Test_Constructor_CollapsiblePanelSet()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOSelector selector = CreateSelector();
            //---------------Test Result -----------------------
            Assert.IsNotNull(selector);
            Assert.IsInstanceOfType(typeof (IBOCollapsiblePanelSelector), selector);
            Assert.IsInstanceOfType(typeof (ICollapsiblePanelGroupControl), selector);
        }

        [Test]
        public virtual void Test_Constructor_nullControlFactory_RaisesError()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new CollapsiblePanelSelectorWin(null);
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
        }

        [Ignore(" Not Implemented yet")] //TODO  02 Mar 2009:
        [Test]
        public override void Test_RemoveBOToCol_UpdatesItems()
        {
            //---------------Set up test pack-------------------
            IBOSelector selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO newMyBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> {myBO, newMyBO};
            selector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreEqual
                (collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and one other");
            Assert.AreSame(myBO, selector.GetBusinessObjectAtRow(ActualIndex(0)));
            Assert.AreSame(newMyBO, selector.GetBusinessObjectAtRow(ActualIndex(1)));
            //---------------Execute Test ----------------------
            collection.Remove(myBO);
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualNumberOfRows(1), selector.NoOfItems, "The blank item and one other");
            Assert.AreSame(newMyBO, selector.GetBusinessObjectAtRow(ActualIndex(0)));
        }

        [Ignore(" Not Implemented")]
        [Test]
        public override void Test_SelectedBusinessObject_SecondItemSelected_ReturnsItem()
        {
            //---------------Set up test pack-------------------
            IBOSelector selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> {myBO, myBO2};
            selector.BusinessObjectCollection = collection;
            SetSelectedIndex(selector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual
                (collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(selector));
            //---------------Execute Test ----------------------
            IBusinessObject selectedBusinessObject = selector.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.AreSame(myBO2, selectedBusinessObject);
        }

        [Ignore(" Not Implemented")] 
        [Test]
        public override void Test_Set_SelectedBusinessObject_SetsItem()
        {
            //---------------Set up test pack-------------------
            IBOSelector selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> {myBO, myBO2};
            selector.BusinessObjectCollection = collection;
            SetSelectedIndex(selector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual
                (collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and others");
//            Assert.AreEqual(ActualIndex(1), SelectedIndex(selector));
            Assert.AreSame(myBO2, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.SelectedBusinessObject = myBO;
            //---------------Test Result -----------------------
            Assert.AreSame(myBO, selector.SelectedBusinessObject);
            Assert.AreEqual(ActualIndex(0), SelectedIndex(selector));
        }

        [Ignore(" Not Implemented")] 
        [Test]
        public override void Test_Set_SelectedBusinessObject_Null_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOSelector selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> {myBO, myBO2};
            selector.BusinessObjectCollection = collection;
            SetSelectedIndex(selector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual
                (collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(selector));
            Assert.AreEqual(myBO2, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.SelectedBusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(selector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(selector));
        }

        [Ignore(" Not Implemented")]
        [Test]
        public override void Test_Set_SelectedBusinessObject_ItemNotInList_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOSelector selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> {myBO, myBO2};
            selector.BusinessObjectCollection = collection;
            SetSelectedIndex(selector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual
                (collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item and others");
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
}