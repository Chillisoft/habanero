using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public class TestReadOnlyGridControlSelectorVWG : TestReadOnlyGridControlSelectorWin
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
            IReadOnlyGridControl readOnlyGridControl = GetControlFactory().CreateReadOnlyGridControl();
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add((Gizmox.WebGUI.Forms.Control)readOnlyGridControl);
            return readOnlyGridControl;
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
    public class TestReadOnlyGridControlSelectorWin : TestBOColSelector
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

            IReadOnlyGrid readOnlyGrid = ((IReadOnlyGridControl)colSelector).Grid;
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
            IReadOnlyGrid gridSelector = ((IReadOnlyGridControl)colSelector).Grid;
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
            IReadOnlyGridControl readOnlyGridControl = GetControlFactory().CreateReadOnlyGridControl();
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add((System.Windows.Forms.Control)readOnlyGridControl);
            return readOnlyGridControl;
        }

        protected override int NumberOfLeadingBlankRows()
        {
            return 0;
        }

        [Test]
        public void Test_Constructor_ReadOnlyGridControlSet()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOColSelectorControl colSelector = CreateSelector();
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(IReadOnlyGridControl), colSelector);
        }

        [Ignore(" Not sure how to implement this in grids.")] //TODO  01 Mar 2009:
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

        [Ignore(" Not sure how to implement this in grids")] //TODO  01 Mar 2009:
        [Test]
        public override void Test_AutoSelectsFirstItem()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, myBO2 };
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
        [Ignore(" Not Yet implemented")] //TODO  01 Mar 2009:
        [Test]
        public void TestEditItemFromCollectionUpdatesItemInSelector()
        {
        }

    }
}