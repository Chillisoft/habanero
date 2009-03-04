using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public class TestEditableGridSelectorVWG : TestEditableGridSelectorWin
    {
        //        private const string _gridIdColumnName = "HABANERO_OBJECTID";
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
        protected override IBOSelectorControl CreateSelector()
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
    public class TestEditableGridSelectorWin : TestBOSelector
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

        protected override void SetSelectedIndex(IBOSelectorControl selector, int index)
        {
            int count = 0;

            IEditableGrid readOnlyGrid = ((IEditableGridControl)selector).Grid;
            foreach (IDataGridViewRow row in readOnlyGrid.Rows)
            {
                if (row == null) continue;//This is done to stop the Pragma warning.
                if (count == index)
                {
                    IBusinessObject businessObjectAtRow = readOnlyGrid.GetBusinessObjectAtRow(count);
                    selector.SelectedBusinessObject = businessObjectAtRow;
                }
                count++;
            }
        }

        protected override int SelectedIndex(IBOSelectorControl selector)
        {
            IEditableGrid gridSelector = ((IEditableGridControl)selector).Grid;
            IDataGridViewRow currentRow = null;
            if (gridSelector.SelectedRows.Count > 0)
            {
                currentRow = gridSelector.SelectedRows[0];
            }

            if (currentRow == null) return -1;

            return gridSelector.Rows.IndexOf(currentRow);
        }

        protected override IBOSelectorControl CreateSelector()
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
        public void Test_Constructor_ReadOnlyGridControlSet()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOSelectorControl selector = CreateSelector();
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(IEditableGridControl), selector);
        }

          [Test]
          public override void Test_ResetBOCol_ToNullClearsItems()
          {
              //---------------Set up test pack-------------------
              IBOSelectorControl selector = CreateSelector();
              MyBO myBO = new MyBO();

              BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO };
              selector.BusinessObjectCollection = collection;
              //---------------Assert Precondition----------------
              Assert.AreEqual(ActualNumberOfRows(collection.Count), selector.NoOfItems, "The blank item and one other");
              Assert.AreSame(myBO, selector.SelectedBusinessObject);
              //---------------Execute Test ----------------------
              selector.BusinessObjectCollection = null;
              //---------------Test Result -----------------------
              Assert.IsNull(selector.SelectedBusinessObject);
              Assert.IsNull(selector.BusinessObjectCollection);
              Assert.AreEqual(0, selector.NoOfItems, "The blank item");
          }

        [Ignore(" Not sure how to implement this in grids.")] //TODO  01 Mar 2009:
        [Test]
        public override void Test_Set_SelectedBusinessObject_ItemNotInList_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
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

        [Ignore(" Not sure how to implement this in grids")] //TODO  01 Mar 2009:
        [Test]
        public override void Test_AutoSelectsFirstItem()
        {
            //---------------Set up test pack-------------------
            IBOSelectorControl selector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, myBO2 };
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, selector.NoOfItems);
            Assert.AreEqual(-1, SelectedIndex(selector));
            Assert.AreEqual(null, selector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selector.BusinessObjectCollection = collection;
            //---------------Test Result -----------------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), selector.NoOfItems, "The blank item");
            Assert.AreSame(myBO, selector.SelectedBusinessObject);
            Assert.AreEqual(ActualIndex(0), SelectedIndex(selector));
        }
        [Ignore(" Not Yet implemented")] //TODO  01 Mar 2009:
        [Test]
        public void TestEditItemFromCollectionUpdatesItemInSelector()
        {
        }
    }
}