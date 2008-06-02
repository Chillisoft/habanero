using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestGridBase : TestUsingDatabase
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            base.SetupDBConnection();
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract IControlFactory GetControlFactory();
        protected abstract IGridBase CreateGridBaseStub();
        protected abstract void AddControlToForm(IGridBase gridBase);

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

            //TODO: To be implemented in Win
            [Test, Ignore("To be implemented in win")]
            public void TestWin_RowShowingBusinessObjectsValues()
            {
                //---------------Set up test pack-------------------
                MyBO.LoadDefaultClassDef();
                BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
                IGridBase gridBase = CreateGridBaseStub();
                SetupGridColumnsForMyBo(gridBase);
                string propName = "TestProp";
                int rowIndex = 1;
                //---------------Execute Test ----------------------
                gridBase.SetBusinessObjectCollection(col);

                //---------------Test Result -----------------------
                MyBO selectedBo = (MyBO) gridBase.GetBusinessObjectAtRow(rowIndex);
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
                string propName = "TestProp";
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
                GridBaseWin gridBase = (GridBaseWin) GetGridBaseWith_4_Rows(out col);
                string filterString = col[2].ID.ToString().Substring(5, 30);
                IFilterClauseFactory factory = new DataViewFilterClauseFactory();
                IFilterClause filterClause =
                    factory.CreateStringFilterClause("ID", FilterClauseOperator.OpLike, filterString);
                bool filterUpdatedFired = false;
                gridBase.FilterUpdated += delegate { filterUpdatedFired = true; };
                //---------------Execute Test ----------------------

                gridBase.ApplyFilter(filterClause);
                //---------------Test Result -----------------------

                Assert.IsTrue(filterUpdatedFired);

                //---------------Tear Down -------------------------
            }

            private static System.Windows.Forms.DataGridViewCell GetCell(int rowIndex, string propName,
                                                                         IGridBase gridBase)
            {
                System.Windows.Forms.DataGridView dgv = (System.Windows.Forms.DataGridView) gridBase;
                System.Windows.Forms.DataGridViewRow row = dgv.Rows[rowIndex];
                return row.Cells[propName];
            }

            protected override void AddControlToForm(IGridBase gridBase)
            {
                throw new NotImplementedException();
            }
        }

        [TestFixture]
        public class TestGridBaseGiz : TestGridBase
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }

            protected override IGridBase CreateGridBaseStub()
            {
                GridBaseGizStub gridBase = new GridBaseGizStub();
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add(gridBase);
                return gridBase;
            }

            protected override void AddControlToForm(IGridBase gridBase)
            {
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add((Gizmox.WebGUI.Forms.Control) gridBase);
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
            public void TestGizRowIsRefreshed()
            {
                //---------------Set up test pack-------------------
                BusinessObjectCollection<MyBO> col;
                IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
                string propName = "TestProp";
                int rowIndex = 1;
                MyBO bo = col[rowIndex];
                AddControlToForm(gridBase);

                //---------------verify preconditions---------------
                object cellValue = GetCellValue(rowIndex, gridBase, propName);
                //DataGridViewCell cell;

                Assert.AreEqual(bo.GetPropertyValue(propName), cellValue);

                //---------------Execute Test ----------------------
                bo.SetPropertyValue(propName, "UpdatedValue");

                //---------------Test Result -----------------------
                //gridBase.SelectedBusinessObject = bo;

                //cell = GetCell(rowIndex, propName, gridBase);
                cellValue = GetCellValue(rowIndex, gridBase, propName);
                Assert.AreEqual("UpdatedValue", cellValue);
            }

            //This does not work since you cannot push changes to the grid based only on changes in the 
            ///underlying datasource. moved to doing a refresh grid.
            //[Test]
            //public void TestGiz_SelectedBusinessObjectEdited_FiresEventToUpdateGrid()
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

            //TODO: Test if it is the first one then will be auto selected then does it still fire.
            [Test]
            public void TestGiz_NonSelectedBusinessObjectEdited()
            {
                //---------------Set up test pack-------------------
                ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
                BusinessObjectCollection<ContactPersonTestBO> businessObjectCollection =
                    new BusinessObjectCollection<ContactPersonTestBO>();

                CreateBOAndAddToCollection(businessObjectCollection);
                CreateBOAndAddToCollection(businessObjectCollection);

                IGridBase gridBase = CreateGridBaseStub();

                string propName = "Surname";
                AddColumnsForContactPerson(businessObjectCollection, gridBase, propName);

                int rowIndex = 1;
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
            public void TestGiz_RowShowingBusinessObjectsValues()
            {
                //---------------Set up test pack-------------------
                MyBO.LoadDefaultClassDef();
                BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
                IGridBase gridBase = CreateGridBaseStub();

                SetupGridColumnsForMyBo(gridBase);
                string propName = "TestProp";
                int rowIndex = 1;
                //---------------Execute Test ----------------------
                gridBase.SetBusinessObjectCollection(col);

                //---------------Test Result -----------------------
                MyBO selectedBo = (MyBO) gridBase.GetBusinessObjectAtRow(rowIndex);
                IDataGridViewRow row = gridBase.Rows[rowIndex];
                IDataGridViewCell cell = row.Cells[propName];
                Assert.AreEqual(selectedBo.TestProp, cell.Value);
            }

            //Cannot Duplicate in grid
            //[Test]
            //public void Test_DeleteObjectInGridThenSetCollectionCausesInfiniteLoop_InGiz()
            //{
            //    //---------------Set up test pack-------------------
            //    MyBO.LoadDefaultClassDef();
            //    BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            //    IGridBase gridBase = CreateGridBaseStub();
            //    SetupGridColumnsForMyBo(gridBase);
            //    gridBase.SetBusinessObjectCollection(col);
            //    string propName = "TestProp";
            //    //---------------Execute Test ----------------------
            //    MyBO bo = col[1];
            //    gridBase.SelectedBusinessObject = bo;
            //    col.Remove(bo);
            //    gridBase.Sort(propName,true);
            //    col = CreateCollectionWith_4_Objects();
            //    gridBase.SetBusinessObjectCollection(col);
            //    //---------------Test Result -----------------------
            //}


            [Test]
            public void TestGiz_ChangeToPageOfRowNum()
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
            public void TestGiz_SetSelectedBusinessObjectChangesPage()
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
            //TODO: Implement in win _Port
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
                string columnName = "Name";
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

        [Test]
        public void TestCreateGridBase()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IControlChilli myGridBase = CreateGridBaseStub();

            //---------------Test Result -----------------------
            Assert.IsNotNull(myGridBase);
            Assert.IsTrue(myGridBase is IGridBase);
            //---------------Tear Down -------------------------   
        }

        [Test]
        public void TestSetCollectionOnGrid_EmptyCollection()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            IGridBase gridBase = CreateGridBaseStub();
            SetupGridColumnsForMyBo(gridBase);
            //---------------Execute Test ----------------------
            gridBase.SetBusinessObjectCollection(col);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridBase.Rows.Count);
            //Assert.AreEqual(classDef.PropDefcol.Count, myGridBase.Columns.Count);//There are 8 columns in the collection BO
            Assert.IsNull(gridBase.SelectedBusinessObject);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSetCollectionOnGrid_NoOfRows()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            SetupGridColumnsForMyBo(gridBase);
            //---------------Execute Test ----------------------
            gridBase.SetBusinessObjectCollection(col);
            //---------------Test Result -----------------------
            Assert.AreEqual(4, gridBase.Rows.Count);
            //---------------Tear Down -------------------------    
        }

        [Test]
        public void Test_SelectedBusinessObject_FirstRowIsSelected()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            SetupGridColumnsForMyBo(gridBase);

            //---------------Execute Test ----------------------
            gridBase.SetBusinessObjectCollection(col);
            IBusinessObject selectedBo = gridBase.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.AreSame(col[0], selectedBo);
            Assert.AreEqual(1, gridBase.SelectedBusinessObjects.Count);
        }

        [Test]
        public void TestSetSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            SetupGridColumnsForMyBo(gridBase);
            MyBO boToSelect = col[1];
            //---------------Execute Test ----------------------

            gridBase.SelectedBusinessObject = boToSelect;

            //---------------Test Result -----------------------
            Assert.AreEqual(boToSelect, gridBase.SelectedBusinessObject);
        }

        [Test]
        public void TestGetSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            MyBO boToSelect = col[1];
            gridBase.SelectedBusinessObject = boToSelect;
            //---------------Execute Test ----------------------
            IBusinessObject selectedBusinessObject = gridBase.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.AreEqual(boToSelect, selectedBusinessObject);
        }

        [Test, Ignore("The old tests tested this but no longer works for giz or windows")]
        public void TestSelectedBusinessObject_SetsCurrentRow()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            MyBO boToSelect = col[1];

            //---------------Execute Test ----------------------
            gridBase.SelectedBusinessObject = boToSelect;
            //---------------Test Result -----------------------
            Assert.IsNotNull(gridBase.CurrentRow);
        }

        [Test]
        public void TestSetSelectedBusinessObject_ToNull()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            //---------------Execute Test ----------------------
            gridBase.SelectedBusinessObject = col[2];
            gridBase.SelectedBusinessObject = null;

            //---------------Test Result -----------------------
            Assert.IsNull(gridBase.SelectedBusinessObject);
            Assert.IsNull(gridBase.CurrentRow);
            //The current row is never set see ignored test TestSelectedBusinessObject_SetsCurrentRow
        }

        [Test]
        public void TestGridFiringItemSelected()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            bool gridItemSelected = false;
            gridBase.SelectedBusinessObject = null;
            gridBase.BusinessObjectSelected += (delegate { gridItemSelected = true; });

            //---------------Execute Test ----------------------
            gridBase.SelectedBusinessObject = col[1];

            //---------------Test Result -----------------------
            Assert.IsTrue(gridItemSelected);
        }

        [Test]
        public void TestGrid_2GetSelectedObjects()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            MyBO boToSelect1 = col[1];
            gridBase.Rows[1].Selected = true;

            //---------------Execute Test ----------------------
            IList<BusinessObject> selectedObjects = gridBase.SelectedBusinessObjects;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, selectedObjects.Count); //The first row is auto selected and the second row
//            is being manually selected
            //Test that the correct items where returned
            Assert.AreSame(boToSelect1, selectedObjects[0]);
            Assert.AreSame(col[0], selectedObjects[1]);
        }

        [Test]
        public void TestGrid_GetSelectedObjects_3SelectedObjects()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            gridBase.Rows[1].Selected = true;
            gridBase.Rows[3].Selected = true;
            //---------------Execute Test ----------------------
            IList<BusinessObject> selectedObjects = gridBase.SelectedBusinessObjects;
            //---------------Test Result -----------------------
            Assert.AreEqual(3, selectedObjects.Count); //the first row plus the two new selected rows
        }

        [Test]
        public void TestGrid_Clear()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            //---------------Execute Test ----------------------
            gridBase.Clear();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridBase.Rows.Count); //The first row is auto selected and the second row
            //is being manually selected
            //Test that the correct items where returned
            Assert.AreEqual(0, gridBase.SelectedBusinessObjects.Count);
            Assert.AreEqual(null, gridBase.SelectedBusinessObject);
        }

        [Test]
        public void TestCollectionChanged()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            SetupGridColumnsForMyBo(gridBase);
            bool hasCollectionChangedFired = false;
            gridBase.CollectionChanged += delegate { hasCollectionChangedFired = true; };
            //---------------Execute Test ----------------------
            gridBase.SetBusinessObjectCollection(col);
            //---------------Test Result -----------------------
            Assert.IsTrue(hasCollectionChangedFired, "CollectionChanged event should have fired.");
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetCollection()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            //---------------Execute Test ----------------------
            IBusinessObjectCollection collection = gridBase.GetBusinessObjectCollection();
            //---------------Test Result -----------------------
            Assert.AreSame(col, collection);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetBusinessObjectAtRow()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            //---------------Execute Test ----------------------
            IBusinessObject businessObject2 = gridBase.GetBusinessObjectAtRow(2);
            IBusinessObject businessObject3 = gridBase.GetBusinessObjectAtRow(3);
            //---------------Test Result -----------------------
            Assert.AreSame(col[2], businessObject2);
            Assert.AreSame(col[3], businessObject3);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestNoOfColumns()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IGridBase gridBase = CreateGridBaseStub();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridBase.Columns.Count);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestOneColumnAdded()
        {
            //---------------Set up test pack-------------------
            IGridBase gridBase = CreateGridBaseStub();
            //---------------Execute Test ----------------------
            //IDataGridViewColumn column = new DataGridViewColumnStub();
            gridBase.Columns.Add(Guid.NewGuid().ToString("N"), "");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, gridBase.Columns.Count);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestTwoColumnsAdded()
        {
            //---------------Set up test pack-------------------
            IGridBase gridBase = CreateGridBaseStub();
            //---------------Execute Test ----------------------
            //IDataGridViewColumn column = new DataGridViewColumnStub();
            gridBase.Columns.Add(Guid.NewGuid().ToString("N"), "");
            gridBase.Columns.Add(Guid.NewGuid().ToString("N"), "");
            //---------------Test Result -----------------------
            Assert.AreEqual(2, gridBase.Columns.Count);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestTwoColumnsAdded_still2columns_AfterSetCollection()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            //---------------Execute Test ----------------------
            //IDataGridViewColumn column = new DataGridViewColumnStub();
            gridBase.Columns.Add(Guid.NewGuid().ToString("N"), "");
            gridBase.Columns.Add(Guid.NewGuid().ToString("N"), "");
            gridBase.SetBusinessObjectCollection(col);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, gridBase.Columns.Count);
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestSetCollectionNoColumnsAddedShouldReturnError()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            //---------------Execute Test ----------------------
            try
            {
                gridBase.SetBusinessObjectCollection(col);
                Assert.Fail();
                //---------------Test Result -----------------------
            }
            catch (GridBaseSetUpException ex)
            {
                StringAssert.Contains("cannot call SetBusinessObjectCollection if the grid's columns have not been set up", ex.Message);
            }

            //---------------Tear Down -------------------------
        }

        [Test]
        public void Test_SetCollection_NonDefaultGridLoader()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            gridBase.Columns.Add(Guid.NewGuid().ToString("N"), "");
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            gridBase.GridLoader = GridLoaderDelegateStub;
            gridBase.SetBusinessObjectCollection(col);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, gridBase.Rows.Count);
            //---------------Tear Down -------------------------          
        }
        public void GridLoaderDelegateStub(IGridBase grid, IBusinessObjectCollection col)
        {
            MyBO cp = new MyBO();
            cp.TestProp = "b";
            grid.Rows.Add(cp.ID,cp.TestProp);
        }
        public void GridLoaderDelegateStub_LoadAllItems(IGridBase grid, IBusinessObjectCollection col)
        {
            if (col == null)
            {
                grid.Rows.Clear();
                return;
            }
            foreach (IBusinessObject businessObject in col)
            {
                MyBO cp = (MyBO) businessObject;
                grid.Rows.Add(cp.ID, cp.TestProp);               
            }
        }
        [Test]
        public void Test_SetBusinessObject_NonDefaultGridLoader()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            gridBase.Columns.Add("ID", "ID");
            gridBase.Columns.Add(Guid.NewGuid().ToString("N"), "");
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            MyBO cp = col[2];
            gridBase.GridLoader = GridLoaderDelegateStub_LoadAllItems;
            gridBase.SetBusinessObjectCollection(col);
            gridBase.SelectedBusinessObject = cp;
            //---------------Test Result -----------------------
            Assert.AreEqual(cp, gridBase.SelectedBusinessObject);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_ClearGrid_NonDefaultGridLoader()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            gridBase.Columns.Add("ID", "ID");
            gridBase.Columns.Add(Guid.NewGuid().ToString("N"), "");
            gridBase.GridLoader = GridLoaderDelegateStub_LoadAllItems;
            gridBase.SetBusinessObjectCollection(col);
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(4, gridBase.Rows.Count);
            //---------------Execute Test ----------------------
            gridBase.Clear();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridBase.Rows.Count);
        }
        [Test]
        public void TestAddItemToCollectionAddsItemToGrid()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            //---------------Verify precondition----------------
            Assert.AreEqual(4, col.Count);
            Assert.AreEqual(4, gridBase.Rows.Count);
            //---------------Execute Test ----------------------
            col.Add(new MyBO());
            //---------------Test Result -----------------------
            Assert.AreEqual(5, col.Count);
            Assert.AreEqual(5, gridBase.Rows.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestRemoveItemFromCollectionRemovesItemFromGrid()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            MyBO bo = col[1];
            //---------------Verify precondition----------------
            Assert.AreEqual(4, col.Count);
            Assert.AreEqual(4, gridBase.Rows.Count);
            //---------------Execute Test ----------------------
            col.Remove(bo);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.AreEqual(3, gridBase.Rows.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestEditItemFromCollectionUpdatesItemInGrid()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            string propName = "TestProp";
            int rowIndex = 1;
            MyBO bo = col[rowIndex];
            gridBase.SetBusinessObjectCollection(col);
            MyBO selectedBo = (MyBO) gridBase.GetBusinessObjectAtRow(rowIndex);
            IDataGridViewCell cell = GetCell(rowIndex, gridBase, propName);
            //---------------Verify precondition----------------
            Assert.AreEqual(selectedBo.TestProp, cell.Value);
            //---------------Execute Test ----------------------
            string newPropValue = "NewValue";
            bo.SetPropertyValue(propName, newPropValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(newPropValue, cell.Value);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSetSortColumn()
        {
            //---------------Set up test pack-------------------

            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            MyBO bo_b = col[0];
            MyBO bo_d = col[1];
            MyBO bo_c = col[2];
            MyBO bo_a = col[3];
            //---------------Verify PreConditions --------------
            Assert.AreEqual("b", bo_b.TestProp);
            Assert.AreEqual("d", bo_d.TestProp);
            Assert.AreEqual("c", bo_c.TestProp);
            Assert.AreEqual("a", bo_a.TestProp);
            Assert.AreEqual(bo_b, gridBase.GetBusinessObjectAtRow(0));
            Assert.AreEqual(bo_d, gridBase.GetBusinessObjectAtRow(1));
            Assert.AreEqual(bo_c, gridBase.GetBusinessObjectAtRow(2));
            Assert.AreEqual(bo_a, gridBase.GetBusinessObjectAtRow(3));
            //---------------Execute Test ----------------------
            gridBase.Sort("TestProp", true);
            //---------------Test Result -----------------------
            Assert.AreEqual(bo_a, gridBase.GetBusinessObjectAtRow(0));
            Assert.AreEqual(bo_b, gridBase.GetBusinessObjectAtRow(1));
            Assert.AreEqual(bo_c, gridBase.GetBusinessObjectAtRow(2));
            Assert.AreEqual(bo_d, gridBase.GetBusinessObjectAtRow(3));
        }
        //TODO: Peter we need to finalise decisions on enums 
        //[Test]
        //public void TestSortColumnAttributeDefault()
        //{
        //    Assert.IsNull(_grid.SortedColumn);
        //    Assert.AreEqual(SortOrder.None, _grid.SortOrder);
        //}

        //[Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        //public void TestSortColumnAttributeExceptionColumnName()
        //{
        //    _grid.SetBusinessObjectCollection(_grid.GetCollection(), "Error1");
        //}

        //[Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        //public void TestSortColumnAttributeExceptionColumnNameAndOrder()
        //{
        //    _grid.SetBusinessObjectCollection(_grid.GetCollection(), "Error2");
        //}

        //[Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        //public void TestSortColumnAttributeExceptionOrder()
        //{
        //    _grid.SetBusinessObjectCollection(_grid.GetCollection(), "Error3");
        //}
        [Test]
        public void TestApplyFilter()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            string filterString = col[2].ID.ToString().Substring(5, 30);
            IFilterClauseFactory factory = new DataViewFilterClauseFactory();
            IFilterClause filterClause =
                factory.CreateStringFilterClause("ID", FilterClauseOperator.OpLike, filterString);
            //---------------Execute Test ----------------------

            gridBase.ApplyFilter(filterClause);
            //---------------Test Result -----------------------

            Assert.AreEqual(1, gridBase.Rows.Count);
            Assert.AreSame(col[2], gridBase.GetBusinessObjectAtRow(0));
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestApplyFilter_SetBusinessObject_SetsTheCorrectBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> boColllection;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out boColllection);
            string filterString = boColllection[2].ID.ToString().Substring(5, 30);
            IFilterClauseFactory factory = new DataViewFilterClauseFactory();
            IFilterClause filterClause =
                factory.CreateStringFilterClause("ID", FilterClauseOperator.OpLike, filterString);
            MyBO bo = boColllection[2];
            //---------------Execute Test ----------------------

            gridBase.ApplyFilter(filterClause);
            gridBase.SelectedBusinessObject = bo;
            //---------------Test Result -----------------------

            Assert.AreSame(bo, gridBase.SelectedBusinessObject);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestApplyFilter_SetBusinessObject_ToAnObjectNoLongerInTheGrid_ReturnsTheCorretRow()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> boColllection;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out boColllection);
            MyBO boRemainingInThisGrid = boColllection[2];
            string filterString = boRemainingInThisGrid.ID.ToString().Substring(5, 30);
            IFilterClauseFactory factory = new DataViewFilterClauseFactory();
            IFilterClause filterClause =
                factory.CreateStringFilterClause("ID", FilterClauseOperator.OpLike, filterString);
            MyBO boNotInGrid = boColllection[1];
            //---------------Execute Test ----------------------

            gridBase.ApplyFilter(filterClause);
            gridBase.SelectedBusinessObject = boNotInGrid;
            //---------------Test Result -----------------------

            Assert.AreSame(null, gridBase.SelectedBusinessObject);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestClearFilter()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            string filterString = col[2].ID.ToString().Substring(5, 30);
            IFilterClauseFactory factory = new DataViewFilterClauseFactory();
            IFilterClause filterClause =
                factory.CreateStringFilterClause("ID", FilterClauseOperator.OpLike, filterString);
            gridBase.ApplyFilter(filterClause);

            //---------------Verify PreConditions --------------
            Assert.AreEqual(1, gridBase.Rows.Count);

            //---------------Execute Test ----------------------
            gridBase.ApplyFilter(null);

            //---------------Test Result -----------------------

            Assert.AreEqual(4, gridBase.Rows.Count);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestRefreshGrid_ResetsSelectedBusinessObjectAndCollection()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            MyBO selectedBo = col[2];
            gridBase.SelectedBusinessObject = selectedBo;
            //---------------Verify PreConditions --------------
            Assert.AreSame(col, gridBase.GetBusinessObjectCollection());
            Assert.AreSame(selectedBo, gridBase.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            gridBase.RefreshGrid();

            //---------------Test Result -----------------------
            Assert.AreSame(col, gridBase.GetBusinessObjectCollection());
            Assert.AreSame(selectedBo, gridBase.SelectedBusinessObject);
            //---------------Tear Down -------------------------
        }
        [Test]
        public void TestSetSelectedBusinessObjectWhenColIsNull_Fail()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            IGridBase gridBase = CreateGridBaseStub();
            //---------------Verify PreConditions --------------
            Assert.IsNull(gridBase.GetBusinessObjectCollection());
            //---------------Execute Test ----------------------
            try
            {
                gridBase.SelectedBusinessObject = new MyBO();
                Assert.Fail("Should throw an error here");
            }
            //---------------Test Result -----------------------
            catch (GridBaseInitialiseException ex)
            {
                StringAssert.Contains("You cannot call SelectedBusinessObject if the collection is not set", ex.Message);
            }
            //---------------Tear Down -------------------------
        }
        [Test]
        public void TestSetSelectedBusinessObject_ToNull_WhenColIsNull()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            IGridBase gridBase = CreateGridBaseStub();
            //---------------Verify PreConditions --------------
            Assert.IsNull(gridBase.GetBusinessObjectCollection());
            //---------------Execute Test ----------------------


            gridBase.SelectedBusinessObject = null;
            //---------------Test Result -----------------------
            Assert.AreEqual(null, gridBase.SelectedBusinessObject);
            //---------------Tear Down -------------------------
        }

        #region Utility Methods 
        private static void AddColumnsForContactPerson(BusinessObjectCollection<ContactPersonTestBO> businessObjectCollection, IGridBase gridBase, string propName)
        {
            gridBase.Columns.Add("ID", "ID");
            gridBase.Columns.Add(propName, propName);
            gridBase.SetBusinessObjectCollection(businessObjectCollection);
        }

        private static void CreateBOAndAddToCollection(
            BusinessObjectCollection<ContactPersonTestBO> businessObjectCollection)
        {
            ContactPersonTestBO cp;
            cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();
            businessObjectCollection.Add(cp);
        }
        private static BusinessObjectCollection<MyBO> CreateCollectionWith_4_Objects()
        {
            MyBO cp = new MyBO();
            cp.TestProp = "b";
            MyBO cp2 = new MyBO();
            cp2.TestProp = "d";
            MyBO cp3 = new MyBO();
            cp3.TestProp = "c";
            MyBO cp4 = new MyBO();
            cp4.TestProp = "a";
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            col.Add(cp, cp2, cp3, cp4);
            return col;
        }

        private static IDataGridViewCell GetCell(int rowIndex, IGridBase gridBase, string propName)
        {
            IDataGridViewRow row = gridBase.Rows[rowIndex];
            return row.Cells[propName];
        }

        private IGridBase GetGridBaseWith_4_Rows(out BusinessObjectCollection<MyBO> col)
        {
            MyBO.LoadDefaultClassDef();
            col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            SetupGridColumnsForMyBo(gridBase);
            gridBase.SetBusinessObjectCollection(col);
            return gridBase;
        }


        private static void SetupGridColumnsForMyBo(IGridBase gridBase)
        {
            gridBase.Columns.Add("ID", "ID");
            gridBase.Columns.Add("TestProp", "TestProp");
        }

        internal class GridBaseGizStub : GridBaseGiz
        {

        }

        internal class GridBaseWinStub : GridBaseWin
        {
        }

        #endregion
    }



    internal class DataGridViewColumnStub : IDataGridViewColumn
    {
        private string _dataPropertyName;
        private string _headerText;
        private string _name;
        private bool _readOnly;
        private string _toolTipText;
        private Type _valueType;
        private int _width;

        /// <summary>Gets or sets the name of the data source property or database column to which the <see cref="IDataGridViewColumn"></see> is bound.</summary>
        /// <returns>The name of the property or database column associated with the <see cref="IDataGridViewColumn"></see>.</returns>
        /// <filterpriority>1</filterpriority>
        //Editor(
        //    "Gizmox.WebGUI.Forms.Design.DataGridViewColumnDataPropertyNameEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
        //    , typeof (UITypeEditor)), Gizmox.WebGUI.Forms.SRDescription("DataGridView_ColumnDataPropertyNameDescr"),
        //DefaultValue(""),
        //TypeConverter(
        //    "IForms.Design.DataMemberFieldConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
        //    ), Browsable(true)]
        public string DataPropertyName
        {
            get { return _dataPropertyName; }
            set { _dataPropertyName = value; }
        }

        /// <summary>Gets or sets the caption text on the column's header cell.</summary>
        /// <returns>A <see cref="T:System.String"></see> with the desired text. The default is an empty string ("").</returns>
        /// <filterpriority>1</filterpriority>
        public string HeaderText
        {
            get { return _headerText; }
            set { _headerText = value; }
        }

        /// <summary>Gets or sets the name of the column.</summary>
        /// <returns>A <see cref="T:System.String"></see> that contains the name of the column. The default is an empty string ("").</returns>
        /// <filterpriority>1</filterpriority>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>Gets or sets a value indicating whether the user can edit the column's cells.</summary>
        /// <returns>true if the user cannot edit the column's cells; otherwise, false.</returns>
        /// <exception cref="T:System.InvalidOperationException">This property is set to false for a column that is bound to a read-only data source. </exception>
        /// <filterpriority>1</filterpriority>
        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        /// <summary>Gets or sets the text used for ToolTips.</summary>
        /// <returns>The text to display as a ToolTip for the column.</returns>
        /// <filterpriority>1</filterpriority>
        public string ToolTipText
        {
            get { return _toolTipText; }
            set { _toolTipText = value; }
        }

        /// <summary>Gets or sets the data type of the values in the column's cells.</summary>
        /// <returns>A <see cref="T:System.Type"></see> that describes the run-time class of the values stored in the column's cells.</returns>
        /// <filterpriority>1</filterpriority>
        public Type ValueType
        {
            get { return _valueType; }
            set { _valueType = value; }
        }

        /// <summary>Gets or sets the current width of the column.</summary>
        /// <returns>The width, in pixels, of the column. The default is 100.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The specified value when setting this property is greater than 65536.</exception>
        /// <filterpriority>1</filterpriority>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public bool Visible
        {
            get { return false; }
            set { }
        }

        public object DataGridViewColumn
        {
            get { throw new NotImplementedException(); }
        }
    }
}