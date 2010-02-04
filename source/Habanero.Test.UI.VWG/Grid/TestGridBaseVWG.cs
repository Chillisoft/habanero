using Habanero.BO;
using Habanero.Test.BO;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Grid
{
    [TestFixture]
    public class TestGridBaseVWG : TestGridBase
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override IGridBase CreateGridBaseStub()
        {
            GridBaseVWGStub gridBase = new GridBaseVWGStub();
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add(gridBase);
            return gridBase;
        }

        protected override void AddControlToForm(IGridBase gridBase)
        {
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add((Gizmox.WebGUI.Forms.Control)gridBase);
        }

        private static Gizmox.WebGUI.Forms.DataGridViewCell GetCell(int rowIndex, string propName,
                                                                    IGridBase gridBase)
        {
            Gizmox.WebGUI.Forms.DataGridView dgv = (Gizmox.WebGUI.Forms.DataGridView)gridBase;
            Gizmox.WebGUI.Forms.DataGridViewRow row = dgv.Rows[rowIndex];
            return row.Cells[propName];
        }

        private static object GetCellValue(int rowIndex, IGridBase gridBase, string propName)
        {
            Gizmox.WebGUI.Forms.DataGridViewCell cell = GetCell(rowIndex, propName, gridBase);
            return cell.Value;
        }

        //Both these tests work but the final result does not i.e. the row in the grid
        // does not get updated.
        [Test]
        public void TestVWGRowIsRefreshed()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            const string propName = "TestProp";
            const int rowIndex = 1;
            MyBO bo = col[rowIndex];
            AddControlToForm(gridBase);

            //---------------verify preconditions---------------
            object cellValue = GetCellValue(rowIndex, gridBase, propName);
            //DataGridViewCell cell;

            Assert.AreEqual(bo.GetPropertyValue(propName), cellValue);

            //---------------Execute Test ----------------------
            bo.SetPropertyValue(propName, "UpdatedValue");
            bo.Save();
            //---------------Test Result -----------------------
            //gridBase.SelectedBusinessObject = bo;

            //cell = GetCell(rowIndex, propName, gridBase);
            cellValue = GetCellValue(rowIndex, gridBase, propName);
            Assert.AreEqual("UpdatedValue", cellValue);
        }

        //This does not work since you cannot push changes to the grid based only on changes in the 
        ///underlying datasource. moved to doing a refresh grid.
        //[Test]
        //public void TestVWG_SelectedBusinessObjectEdited_FiresEventToUpdateGrid()
        //{
        //    //---------------Set up test pack-------------------
        //    ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
        //    BusinessObjectCollection<ContactPersonTestBO> businessObjectCollection =
        //        new BusinessObjectCollection<ContactPersonTestBO>();

        //    CreateBOAndAddToCollection(businessObjectCollection);
        //    CreateBOAndAddToCollection(businessObjectCollection);

        //    IGridBase gridBase = CreateGridBaseStub();

        //    string propName = "Surname";
        //    AddColumnsForContactPerson(businessObjectCollection, gridBase, propName);

        //    int rowIndex = 1;
        //    ContactPersonTestBO bo = businessObjectCollection[rowIndex];
        //    gridBase.SelectedBusinessObject = bo;
        //    bool _boUpdated = false;
        //    gridBase.BusinessObjectEdited += delegate { _boUpdated = true; };
        //    //---------------verify preconditions---------------
        //    object cellValue = GetCellValue(rowIndex, gridBase, propName);
        //    Assert.AreEqual(bo.GetPropertyValue(propName), cellValue);

        //    Assert.AreEqual(bo, gridBase.SelectedBusinessObject);
        //    Assert.IsFalse(_boUpdated);
        //    //---------------Execute Test ----------------------
        //    bo.SetPropertyValue(propName, "UpdatedValue");
        //    bo.Save();
        //    //---------------Test Result -----------------------
        //    Assert.IsTrue(_boUpdated);


        //}

        [Test]
        public void TestVWG_NonSelectedBusinessObjectEdited()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
            BusinessObjectCollection<ContactPersonTestBO> businessObjectCollection =
                new BusinessObjectCollection<ContactPersonTestBO>();

            CreateBOAndAddToCollection(businessObjectCollection);
            CreateBOAndAddToCollection(businessObjectCollection);

            IGridBase gridBase = CreateGridBaseStub();

            const string propName = "Surname";
            AddColumnsForContactPerson(businessObjectCollection, gridBase, propName);

            const int rowIndex = 1;
            ContactPersonTestBO bo = businessObjectCollection[rowIndex];
            gridBase.SelectedBusinessObject = bo;
            bool _boUpdated = false;
            gridBase.BusinessObjectEdited += delegate { _boUpdated = true; };

            //---------------verify preconditions---------------
            object cellValue = GetCellValue(rowIndex, gridBase, propName);
            Assert.AreEqual(bo.GetPropertyValue(propName), cellValue);

            Assert.AreEqual(bo, gridBase.SelectedBusinessObject);
            Assert.IsFalse(_boUpdated);

            //---------------Execute Test ----------------------
            //set a different object as the selected object
            gridBase.SelectedBusinessObject = businessObjectCollection[rowIndex - 1];
            //edit its value
            bo.SetPropertyValue(propName, "UpdatedValue");
            bo.Save();
            //---------------Test Result -----------------------
            //Should not cause an update
            Assert.IsFalse(_boUpdated);

        }



        [Test]
        public void TestVWG_RowShowingBusinessObjectsValues()
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
        public void TestVWG_Set_BusinessObjectCollection()
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

        //Cannot Duplicate in grid
        //[Test]
        //public void Test_DeleteObjectInGridThenSetCollectionCausesInfiniteLoop_InVWG()
        //{
        //    //---------------Set up test pack-------------------
        //    MyBO.LoadDefaultClassDef();
        //    BusinessObjectCollection<MyBO> col = GetCollectionWith_4_Objects();
        //    IGridBase gridBase = CreateGridBaseStub();
        //    SetupGridColumnsForMyBo(gridBase);
        //    gridBase.BusinessObjectCollection  = col;
        //    string propName = "TestProp";
        //    //---------------Execute Test ----------------------
        //    MyBO bo = col[1];
        //    gridBase.SelectedBusinessObject = bo;
        //    col.Remove(bo);
        //    gridBase.Sort(propName,true);
        //    col = GetCollectionWith_4_Objects();
        //    gridBase.SetBusinessObjectCollection(col);
        //    //---------------Test Result -----------------------
        //}


        [Test]
        public void TestVWG_ChangeToPageOfRowNum()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            gridBase.ItemsPerPage = 3;

            //---------------Assert preconditions---------------
            Assert.AreEqual(1, gridBase.CurrentPage);
            //---------------Execute Test ----------------------
            gridBase.ChangeToPageOfRow(4);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, gridBase.CurrentPage);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestVWG_SetSelectedBusinessObjectChangesPage()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            gridBase.ItemsPerPage = 3;

            //---------------Execute Test ----------------------
            gridBase.SelectedBusinessObject = col[3];
            //---------------Test Result -----------------------
            Assert.AreEqual(2, gridBase.CurrentPage);
            //---------------Tear Down -------------------------
        }
        [Test]
        public void Test_AddImageColumn()
        {
            //---------------Set up test pack-------------------
            IGridBase gridBase = CreateGridBaseStub();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            IDataGridViewImageColumn imgColumn = GetControlFactory().CreateDataGridViewImageColumn();
            gridBase.Columns.Add(imgColumn);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, gridBase.Columns.Count);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_IndexerReturnsImageColumn()
        {
            //---------------Set up test pack-------------------
            IGridBase gridBase = CreateGridBaseStub();
            IDataGridViewImageColumn imgColumn = GetControlFactory().CreateDataGridViewImageColumn();
            gridBase.Columns.Add(imgColumn);
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, gridBase.Columns.Count);
            //---------------Execute Test ----------------------
            IDataGridViewColumn col = gridBase.Columns[0];
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(IDataGridViewImageColumn), col);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_StringIndexerReturnsImageColumn()
        {
            //---------------Set up test pack-------------------
            IGridBase gridBase = CreateGridBaseStub();
            IDataGridViewImageColumn imgColumn = GetControlFactory().CreateDataGridViewImageColumn();
            const string columnName = "Name";
            imgColumn.Name = columnName;
            gridBase.Columns.Add(imgColumn);
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, gridBase.Columns.Count);
            //---------------Execute Test ----------------------
            IDataGridViewColumn col = gridBase.Columns[columnName];
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(IDataGridViewImageColumn), col);
            //---------------Tear Down -------------------------          
        }
    }
}