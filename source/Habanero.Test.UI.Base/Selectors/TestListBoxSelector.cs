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
    /// This test class tests the base inherited methods of the ListBoxSelector class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_ListBoxSelector : TestBaseMethods.TestBaseMethodsWin
    {
        [STAThread]
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateListBoxSelector();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the ListBoxSelector class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_ListBoxSelector : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateListBoxSelector();
        }
    }

    public class TestListBoxSelectorVWG : TestListBoxSelectorWin
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
        protected override IBOSelector CreateSelector()
        {
            return GetControlFactory().CreateListBoxSelector();
        }
        [Test]
        public override void Test_Constructor_nullControlFactory_RaisesError()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new ListBoxSelectorVWG(null);
                Assert.Fail("expected ArgumentNullException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("controlFactory", ex.ParamName);
            }
        }

        [Ignore(" Not working in VWG")]
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

        [Ignore(" Not working in VWG")] 
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

        [Ignore(" Not working in VWG")] 
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


        [Ignore(" Not working in VWG")]
        [Test]
        public override void Test_SelectorFiringItemSelected()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IBOSelector boSelector = GetSelectorWith_4_Rows(out col);
            bool itemSelected = false;
            boSelector.SelectedBusinessObject = null;
            boSelector.BusinessObjectSelected += (delegate { itemSelected = true; });
            //---------------Execute Test ----------------------
            boSelector.SelectedBusinessObject = col[1];
            //---------------Test Result -----------------------
            Assert.IsTrue(itemSelected);
        }

    }

    /// <summary>
    /// This test class tests the ListBoxSelector class.
    /// </summary>
    [TestFixture]
    public class TestListBoxSelectorWin : TestBOSelector
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override void SetSelectedIndex(IBOSelector selector, int index)
        {
            ((IBOListBoxSelector)selector).ListBox.SelectedIndex = index;
        }

        protected override int SelectedIndex(IBOSelector selector)
        {
            return ((IBOListBoxSelector)selector).ListBox.SelectedIndex;
        }

        protected override IBOSelector CreateSelector()
        {
            return GetControlFactory().CreateListBoxSelector();
        }

        protected override int NumberOfLeadingBlankRows()
        {
            return 0;
        }

        [Test]
        public void Test_Constructor_ListBoxSet()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOListBoxSelector selector = (IBOListBoxSelector) CreateSelector();
            //---------------Test Result -----------------------
            Assert.IsNotNull(selector.ListBox);
            Assert.IsInstanceOfType(typeof(IListBox), selector.ListBox);
        }
        [Test]
        public virtual void Test_Constructor_nullControlFactory_RaisesError()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new ListBoxSelectorWin(null);
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

    }
}