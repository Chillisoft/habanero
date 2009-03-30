using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public class TestEditableGridSelectorVWG : TestBOSelectorEditableGridWin
    {
        //        private const string _gridIdColumnName = "HABANERO_OBJECTID";
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
        protected override IBOColSelectorControl CreateSelector()
        {
            IEditableGridControl editableGridControl = GetControlFactory().CreateEditableGridControl();
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add((Gizmox.WebGUI.Forms.Control)editableGridControl);
            return editableGridControl;
        }
        //protected override IBOSelectorControl CreateSelector()
        //{
        //    TestGridBase.GridBaseVWGStub gridBase = new TestGridBase.GridBaseVWGStub();
        //    Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
        //    frm.Controls.Add(gridBase);
        //    SetupGridColumnsForMyBo(gridBase);
        //    return gridBase;
        //}
        //        [Test]
        //        public virtual void Test_Constructor_nullControlFactory_RaisesError()
        //        {
        //            //---------------Set up test pack-------------------
        //
        //            //---------------Assert Precondition----------------
        //
        //            //---------------Execute Test ----------------------
        //            try
        //            {
        //                new GridSelectorVWG(null);
        //                Assert.Fail("expected ArgumentNullException");
        //            }
        //            //---------------Test Result -----------------------
        //            catch (ArgumentNullException ex)
        //            {
        //                StringAssert.Contains("Value cannot be null", ex.Message);
        //                StringAssert.Contains("controlFactory", ex.ParamName);
        //            }
        //        }
    }

      /// <summary>
    /// This test class tests the GridSelector class.
    /// </summary>
    [TestFixture]
    public class TestBOSelectorEditableGridWin : TestBOColSelector
    {
        //        private const string _gridIdColumnName = "HABANERO_OBJECTID";

        //[TestFixtureSetUp]
        //private void TestFixtureSetUp()
        //{
        //    ClassDef.ClassDefs.Clear();
        //    BORegistry.DataAccessor = new DataAccessorInMemory();
        //    ContactPersonTestBO.LoadDefaultClassDef();
        //}

        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override void SetSelectedIndex(IBOColSelectorControl colSelector, int index)
        {
            int count = 0;

            IEditableGrid readOnlyGrid = ((IEditableGridControl)colSelector).Grid;
            foreach (IDataGridViewRow row in readOnlyGrid.Rows)
            {
                if (row == null) continue;//This is done to stop the Pragma warning.
                if (count == index)
                {
                    IBusinessObject businessObjectAtRow = readOnlyGrid.GetBusinessObjectAtRow(count);
                    colSelector.SelectedBusinessObject = businessObjectAtRow;
                }
                count++;
            }
        }

        protected override int SelectedIndex(IBOColSelectorControl colSelector)
        {
            IEditableGrid gridSelector = ((IEditableGridControl)colSelector).Grid;
            IDataGridViewRow currentRow = null;
            if (gridSelector.SelectedRows.Count > 0)
            {
                currentRow = gridSelector.SelectedRows[0];
            }

            if (currentRow == null) return -1;

            return gridSelector.Rows.IndexOf(currentRow);
        }

        protected override IBOColSelectorControl CreateSelector()
        {
            IEditableGridControl editableGridControl = GetControlFactory().CreateEditableGridControl();
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add((System.Windows.Forms.Control)editableGridControl);
            return editableGridControl;
        }

        protected override int NumberOfLeadingBlankRows()
        {
            return 1;
        }

          protected override int ActualIndex(int index)
          {
              return index;
          }

          [Test]
        public virtual void Test_Constructor_ReadOnlyGridControlSet()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOColSelectorControl colSelector = CreateSelector();
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(IEditableGridControl), colSelector);
        }

          [Test]
          public override void Test_ResetBOCol_ToNullClearsItems()
          {
              //---------------Set up test pack-------------------
              IBOColSelectorControl colSelector = CreateSelector();
              IBusinessObject myBO;
              IBusinessObjectCollection collection = GetCollectionWithOneBO(out myBO); 
              colSelector.BusinessObjectCollection = collection;
              //---------------Assert Precondition----------------
              Assert.AreEqual(ActualNumberOfRows(collection.Count), colSelector.NoOfItems, "The blank item and one other");
              Assert.AreSame(myBO, colSelector.SelectedBusinessObject);
              //---------------Execute Test ----------------------
              colSelector.BusinessObjectCollection = null;
              //---------------Test Result -----------------------
              Assert.IsNull(colSelector.SelectedBusinessObject);
              Assert.IsNull(colSelector.BusinessObjectCollection);
              Assert.AreEqual(0, colSelector.NoOfItems, "The blank item");
          }

        [Ignore(" Not sure how to implement this in grids.")] //TODO  01 Mar 2009:
        [Test]
        public override void Test_Set_SelectedBusinessObject_ItemNotInList_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject myBO;
            IBusinessObjectCollection collection = GetCollectionWithTowBOs(out myBO); 
            colSelector.BusinessObjectCollection = collection;
            SetSelectedIndex(colSelector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(colSelector));
            Assert.AreEqual(collection[1], colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.SelectedBusinessObject = new MyBO();
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualIndex(2), colSelector.NoOfItems, "The blank item");
            Assert.IsNull(colSelector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
        }

        [Ignore(" Not sure how to implement this in grids")] //TODO  01 Mar 2009:
        [Test]
        public override void Test_SetBOCollection_WhenAutoSelectsFirstItem_ShouldSelectFirstItem()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObject myBO;
            IBusinessObjectCollection collection = GetCollectionWithTowBOs(out myBO);
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, colSelector.NoOfItems);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
            Assert.AreEqual(null, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item");
            Assert.AreSame(myBO, colSelector.SelectedBusinessObject);
            Assert.AreEqual(ActualIndex(0), SelectedIndex(colSelector));
        }

          public override void Test_AutoSelectsFirstItem_NoItems()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            IBusinessObjectCollection collection = GetCollectionWithNoItems();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, colSelector.NoOfItems);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
            Assert.AreEqual(null, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreEqual(NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item");
            Assert.AreSame(null, colSelector.SelectedBusinessObject);
            Assert.AreEqual(0, SelectedIndex(colSelector));
        }

          [Ignore(" Not Yet implemented")] //TODO  01 Mar 2009:
        [Test]
        public void TestEditItemFromCollectionUpdatesItemInSelector()
        {
        }
    }
}