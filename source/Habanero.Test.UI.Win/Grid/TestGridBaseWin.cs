using Habanero.Base;
using Habanero.BO;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Grid
{
    public class GridBaseWinStub : GridBaseWin
    {
        /// <summary>
        /// Creates a dataset provider that is applicable to this grid. For example, a readonly grid would
        /// return a read only datasetprovider, while an editable grid would return an editable one.
        /// </summary>
        /// <param name="col">The collection to create the datasetprovider for</param>
        /// <returns></returns>
        public override IDataSetProvider CreateDataSetProvider(IBusinessObjectCollection col)
        {
            ReadOnlyDataSetProvider dataSetProvider = new ReadOnlyDataSetProvider(col);
            dataSetProvider.RegisterForBusinessObjectPropertyUpdatedEvents = true;
            return dataSetProvider;
        }
    }

    [TestFixture]
    public class TestGridBaseWin : TestGridBase
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override IGridBase CreateGridBaseStub()
        {
            GridBaseWinStub gridBase = new GridBaseWinStub();
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add(gridBase);
            return gridBase;
        }

        [Test]
        public void TestWin_RowShowingBusinessObjectsValues()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            SetupGridColumnsForMyBo(gridBase);
            const string propName = "TestProp";
            const int rowIndex = 1;
            //---------------Execute Test ----------------------
#pragma warning disable 618,612 //Maintained for backward compatibility testing
            gridBase.SetBusinessObjectCollection(col);
#pragma warning restore 618,612

            //---------------Test Result -----------------------
            MyBO selectedBo = (MyBO)gridBase.GetBusinessObjectAtRow(rowIndex);
            IDataGridViewRow row = gridBase.Rows[rowIndex];
            IDataGridViewCell cell = row.Cells[propName];
            Assert.AreEqual(selectedBo.TestProp, cell.Value);
        }
        [Test]
        public void TestWin_Set_BusinessObjectCollection()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            SetupGridColumnsForMyBo(gridBase);
            const string propName = "TestProp";
            const int rowIndex = 1;
            //---------------Execute Test ----------------------
            gridBase.BusinessObjectCollection = col;

            //---------------Test Result -----------------------
            MyBO selectedBo = (MyBO)gridBase.GetBusinessObjectAtRow(rowIndex);
            IDataGridViewRow row = gridBase.Rows[rowIndex];
            IDataGridViewCell cell = row.Cells[propName];
            Assert.AreEqual(selectedBo.TestProp, cell.Value);
        }

        [Test]
        public void TestWinRowIsRefreshed()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            MyBO bo = col[0];

            //---------------Execute Test ----------------------
            const string propName = "TestProp";
            bo.SetPropertyValue(propName, "UpdatedValue");

            //---------------Test Result -----------------------
            gridBase.SelectedBusinessObject = bo;
            //System.Windows.Forms.DataGridViewRow row = (System.Windows.Forms.DataGridViewRow) gridBase.Rows[0];
            //System.Windows.Forms.DataGridViewCell cell = row.Cells[propName];
            System.Windows.Forms.DataGridViewCell cell = GetCell(0, propName, gridBase);
            Assert.AreEqual("UpdatedValue", cell.Value);
        }

        [Test]
        public void TestWinApplyFilterFiresFilterUpdatedEvent()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            GridBaseWin gridBase = (GridBaseWin)GetGridBaseWith_4_Rows(out col);
            string filterString = col[2].ID.ToString().Substring(5, 30);
            IFilterClauseFactory factory = new DataViewFilterClauseFactory();
            IFilterClause filterClause =
                factory.CreateStringFilterClause(_gridIdColumnName, FilterClauseOperator.OpLike, filterString);
            bool filterUpdatedFired = false;
            gridBase.FilterUpdated += delegate { filterUpdatedFired = true; };
            //---------------Execute Test ----------------------
            gridBase.ApplyFilter(filterClause);
            //---------------Test Result -----------------------
            Assert.IsTrue(filterUpdatedFired);
        }

        [Test]
        public void TestSelectedBusinessObject_SetsCurrentRow()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            SetupGridColumnsForMyBo(gridBase);
            MyBO firstBO = col[0];
            MyBO secondBO = col[1];
            //---------------Assert Precondition----------------
            Assert.AreEqual(firstBO, gridBase.SelectedBusinessObject);
            Assert.IsNull(gridBase.CurrentRow);
            //Assert.AreEqual(0, gridBase.Rows.IndexOf(gridBase.CurrentRow));   //surely the currentrow should be active on setCol?
            //---------------Execute Test ----------------------
            gridBase.SelectedBusinessObject = secondBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(secondBO, gridBase.SelectedBusinessObject);
            Assert.AreEqual(1, gridBase.Rows.IndexOf(gridBase.CurrentRow));
        }

        private static System.Windows.Forms.DataGridViewCell GetCell(int rowIndex, string propName,
                                                                     IGridBase gridBase)
        {
            System.Windows.Forms.DataGridView dgv = (System.Windows.Forms.DataGridView)gridBase;
            System.Windows.Forms.DataGridViewRow row = dgv.Rows[rowIndex];
            return row.Cells[propName];
        }

        protected override void AddControlToForm(IGridBase gridBase)
        {
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add((System.Windows.Forms.Control)gridBase);
        }
    }
}