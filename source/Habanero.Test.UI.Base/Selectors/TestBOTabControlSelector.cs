using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;


using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestBOSelectorBOTab : TestBOColSelector
    {
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

        protected abstract IBusinessObjectControl GetBusinessObjectControlStub();

        protected override int NumberOfLeadingBlankRows()
        {
            return 0;
        }

        protected override int NumberOfTrailingBlankRows()
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
            Assert.IsInstanceOf(typeof (IBOColTabControl), colSelector);
//            Assert.IsInstanceOf(typeof (IBOColTabControl), selector);
        }

        [Ignore(" Not Yet implemented : Brett 03 Mar 2009:")] //TODO  01 Mar 2009:
        [Test]
        public void TestEditItemFromCollectionUpdatesItemInSelector()
        {
            Assert.Fail("Not yet implemented");
        }

        [Test]
        public override void Test_SetBOCollection_WhenAutoSelectFalse_ShouldNot_AutoSelectsFirstItem()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject myBO;
            IBusinessObjectCollection collection = GetCollectionWithTowBOs(out myBO);
            colSelector.AutoSelectFirstItem = false;
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, colSelector.NoOfItems);
            Assert.AreEqual(0, SelectedIndex(colSelector));
            Assert.AreEqual(null, colSelector.SelectedBusinessObject);
            Assert.IsFalse(colSelector.AutoSelectFirstItem);
            //---------------Execute Test ----------------------
            colSelector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualNumberOfRows(collection.Count), colSelector.NoOfItems, "The blank item");
            Assert.IsNull(colSelector.SelectedBusinessObject);
        }

        [Test]
        public override void Test_SetBOCollection_WhenAutoSelectsFirstItem_ShouldSelectFirstItem()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject myBO;
            IBusinessObjectCollection collection = GetCollectionWithTowBOs(out myBO); 
            //---------------Assert Precondition----------------
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