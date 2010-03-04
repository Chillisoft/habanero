//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestGridBase 
    {
        protected const string _gridIdColumnName = "HABANERO_OBJECTID";

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
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract IControlFactory GetControlFactory();
        protected abstract IGridBase CreateGridBaseStub();
        protected abstract void AddControlToForm(IGridBase gridBase);



        [Test]
        public void TestCreateGridBase()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero myGridBase = CreateGridBaseStub();

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
#pragma warning disable 618,612
            gridBase.SetBusinessObjectCollection(col);
#pragma warning restore 618,612
            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridBase.Rows.Count);
            //Assert.AreEqual(classDef.PropDefcol.Count, myGridBase.Columns.Count);//There are 8 columns in the collection BO
            Assert.IsNull(gridBase.SelectedBusinessObject);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_Set_BusinessCollectionOnGrid_EmptyCollection()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            IGridBase gridBase = CreateGridBaseStub();
            SetupGridColumnsForMyBo(gridBase);
            //---------------Execute Test ----------------------
            gridBase.BusinessObjectCollection = col;
            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridBase.Rows.Count);
            Assert.IsNull(gridBase.SelectedBusinessObject);
        }

        [Test]
        public void TestSetCollectionOnGrid_NullCollection()
        {
            //---------------Set up test pack-------------------
            IGridBase gridBase = CreateGridBaseStub();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
#pragma warning disable 618,612 //For backward compatibility testing
            gridBase.SetBusinessObjectCollection(null);
            //---------------Test Result -----------------------
            Assert.IsNull(gridBase.GetBusinessObjectCollection());
            Assert.AreEqual(0, gridBase.Rows.Count);
            Assert.AreEqual(0, gridBase.Columns.Count);
        }
#pragma warning restore 618,612

        [Test]
        public void Test_Set_BusinessCollectionOnGrid_NullCollection()
        {
            //---------------Set up test pack-------------------
            IGridBase gridBase = CreateGridBaseStub();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            gridBase.BusinessObjectCollection = null;
            //---------------Test Result -----------------------
            Assert.IsNull(gridBase.BusinessObjectCollection);
            Assert.AreEqual(0, gridBase.Rows.Count);
            Assert.AreEqual(0, gridBase.Columns.Count);
        }

        [Test]
        public void Test_SortMode()
        {
            //---------------Set up test pack-------------------
            IGridBase gridBase = CreateGridBaseStub();

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            gridBase.Columns.Add("TestProp", "TestProp");
            //---------------Test Result -----------------------
            Assert.AreEqual(DataGridViewColumnSortMode.Automatic , gridBase.Columns[0].SortMode);        
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
#pragma warning disable 618,612
            gridBase.SetBusinessObjectCollection(col);
#pragma warning restore 618,612
            //---------------Test Result -----------------------
            Assert.AreEqual(4, gridBase.Rows.Count);
        }
        [Test]
        public void Test_Set_BusinessObjectCollectionOnGrid_NoOfRows()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            SetupGridColumnsForMyBo(gridBase);
            //---------------Execute Test ----------------------
            gridBase.BusinessObjectCollection = col;
            //---------------Test Result -----------------------
            Assert.AreEqual(4, gridBase.Rows.Count);
        }

        [Test]
        public void TestRefreshBusinessObjectRow()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            SetupGridColumnsForMyBo(gridBase);
            gridBase.BusinessObjectCollection =col;
            MyBO myBO = col[0];
            string testPropValue = TestUtil.GetRandomString();
            myBO.TestProp = testPropValue;
            IDataGridViewRow row = gridBase.GetBusinessObjectRow(myBO);
            row.Cells["TestProp"].Value = "";
            //---------------Assert Precondition----------------
            Assert.AreEqual("", row.Cells["TestProp"].Value);
            Assert.AreEqual(testPropValue, myBO.TestProp);
            //---------------Execute Test ----------------------
            gridBase.RefreshBusinessObjectRow(myBO);
            //---------------Test Result -----------------------
            Assert.AreEqual(testPropValue, row.Cells["TestProp"].Value);
        }

        [Test]
        public void TestRefreshBusinessObjectRow_NullBo()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            SetupGridColumnsForMyBo(gridBase);
            gridBase.BusinessObjectCollection = col;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            gridBase.RefreshBusinessObjectRow(null);
            //---------------Test Result -----------------------
            Assert.IsTrue(true, "Error should not be thrown and this assert should be reached.");
        }

        [Test]
        public void Test_TryReturnAColumnThatDoesNotExist_ReturnsNull()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            SetupGridColumnsForMyBo(gridBase);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            IDataGridViewColumn column = gridBase.Columns["NonExistant"];
            //---------------Test Result -----------------------
            Assert.IsNull(column);      
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
            gridBase.BusinessObjectCollection  = col;
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

        [Test]
        public void TestSetSelectedBusinessObject_ToNull()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            gridBase.SelectedBusinessObject = col[2];
            //---------------Assert Precondition ---------------
            Assert.IsNotNull(gridBase.SelectedBusinessObject);
//            Assert.IsNotNull(gridBase.CurrentRow); 
            //The current row is never set for VWG see ignored test TestSelectedBusinessObject_SetsCurrentRow
            //---------------Execute Test ----------------------
            gridBase.SelectedBusinessObject = null;

            //---------------Test Result -----------------------
            Assert.IsNull(gridBase.SelectedBusinessObject);
            Assert.IsNull(gridBase.CurrentRow);
            
        }

        [Test, Ignore("Works in real, but not in the tests")]
        public void TestSetSelectedBusinessObject_ScrollsGridWhenRowIsNotVisible()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            for (int i = 0; i < 200; i++)
            {
                MyBO newBO = new MyBO();
                newBO.TestProp = BOTestUtils.RandomString;
                col.Add(newBO);
            }
            AddControlToForm(gridBase);
            //---------------Assert Precondition----------------
            Assert.AreSame(col[0], gridBase.SelectedBusinessObject);
            Assert.AreEqual(204, gridBase.Rows.Count);
            Assert.IsFalse(gridBase.Rows[203].Displayed);
            //---------------Execute Test ----------------------
            gridBase.SelectedBusinessObject = col[203];
            //---------------Test Result -----------------------
            Assert.AreSame(col[203], gridBase.SelectedBusinessObject);
            Assert.IsTrue(gridBase.Rows[203].Displayed);
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
            Assert.IsNull(gridBase.BusinessObjectCollection);
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
            gridBase.BusinessObjectCollection  = col;
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
            IBusinessObjectCollection collection = gridBase.BusinessObjectCollection;
            //---------------Test Result -----------------------
            Assert.AreSame(col, collection);
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
        }

        [Test]
        public void TestGetBusinessObjectRow()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            MyBO myBO2 = col[2];
            const int expectedIndex = 2;
            //-------------Assert Preconditions -------------
            Assert.AreSame(myBO2, gridBase.GetBusinessObjectAtRow(expectedIndex));
            //---------------Execute Test ----------------------
            IDataGridViewRow dataGridViewRow = gridBase.GetBusinessObjectRow(myBO2);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedIndex, dataGridViewRow.Index);
            Assert.AreEqual(myBO2.ID.AsString_CurrentValue(), dataGridViewRow.Cells[_gridIdColumnName].Value);
        }

        [Test]
        public void TestGetBusinessObjectRow_NullBO()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            IDataGridViewRow dataGridViewRow = gridBase.GetBusinessObjectRow(null);
            //---------------Test Result -----------------------
            Assert.IsNull(dataGridViewRow);
        }

        [Test]
        public void TestGetBusinessObjectRow_ReturnsNullIfNotFound()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            MyBO notFoundBO = new MyBO();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            IDataGridViewRow dataGridViewRow = gridBase.GetBusinessObjectRow(notFoundBO);
            //---------------Test Result -----------------------
            Assert.IsNull(dataGridViewRow);
        }

        [Test]
        public void TestNoOfColumns()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IGridBase gridBase = CreateGridBaseStub();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridBase.Columns.Count);
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
            gridBase.BusinessObjectCollection  = col;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, gridBase.Columns.Count);
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
                gridBase.BusinessObjectCollection  = col;
                Assert.Fail();
                //---------------Test Result -----------------------
            }
            catch (GridBaseSetUpException ex)
            {
                StringAssert.Contains("cannot call SetBusinessObjectCollection if the grid's columns have not been set up", ex.Message);
            }
        }

        [Test]
        public void Test_SetCollection_NonDefaultGridLoader()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            gridBase.Columns.Add(_gridIdColumnName, _gridIdColumnName);
            gridBase.Columns.Add(Guid.NewGuid().ToString("N"), "");
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            gridBase.GridLoader = GridLoaderDelegateStub;
            gridBase.BusinessObjectCollection  = col;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, gridBase.Rows.Count);
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
        public void TestSetBusinessObject_NonDefaultGridLoader()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            gridBase.Columns.Add("HABANERO_OBJECTID", "HABANERO_OBJECTID");
            gridBase.Columns.Add(Guid.NewGuid().ToString("N"), "");
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            MyBO cp = col[2];
            gridBase.GridLoader = GridLoaderDelegateStub_LoadAllItems;
            gridBase.BusinessObjectCollection  = col;
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
            gridBase.Columns.Add(_gridIdColumnName, _gridIdColumnName);
            gridBase.Columns.Add(Guid.NewGuid().ToString("N"), "");
            gridBase.GridLoader = GridLoaderDelegateStub_LoadAllItems;
            gridBase.BusinessObjectCollection  = col;
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
        }

        [Test]
        public void TestEditItemFromCollectionUpdatesItemInGrid()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            const string propName = "TestProp";
            const int rowIndex = 1;
            MyBO bo = col[rowIndex];
            gridBase.BusinessObjectCollection  = col;
            MyBO selectedBo = (MyBO) gridBase.GetBusinessObjectAtRow(rowIndex);
            IDataGridViewCell cell = GetCell(rowIndex, gridBase, propName);
            //---------------Verify precondition----------------
            Assert.AreEqual(selectedBo.TestProp, cell.Value);
            //---------------Execute Test ----------------------
            const string newPropValue = "NewValue";
            bo.SetPropertyValue(propName, newPropValue);
            bo.Save();
            //---------------Test Result -----------------------
            Assert.AreEqual(newPropValue, cell.Value);
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
                factory.CreateStringFilterClause(_gridIdColumnName, FilterClauseOperator.OpLike, filterString);
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
                factory.CreateStringFilterClause(_gridIdColumnName, FilterClauseOperator.OpLike, filterString);
            MyBO bo = boColllection[2];
            //---------------Execute Test ----------------------

            gridBase.ApplyFilter(filterClause);
            gridBase.SelectedBusinessObject = bo;
            //---------------Test Result -----------------------

            Assert.AreSame(bo, gridBase.SelectedBusinessObject);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestApplyFilter_SetBusinessObject_ToAnObjectNoLongerInTheGrid_ReturnsNullSelectedBO()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> boColllection;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out boColllection);
            MyBO boRemainingInThisGrid = boColllection[2];
            string filterString = boRemainingInThisGrid.ID.ToString().Substring(5, 30);
            IFilterClauseFactory factory = new DataViewFilterClauseFactory();
            IFilterClause filterClause =
                factory.CreateStringFilterClause(_gridIdColumnName, FilterClauseOperator.OpLike, filterString);
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
                factory.CreateStringFilterClause(_gridIdColumnName, FilterClauseOperator.OpLike, filterString);
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
            Assert.AreSame(col, gridBase.BusinessObjectCollection);
            Assert.AreSame(selectedBo, gridBase.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            gridBase.RefreshGrid();

            //---------------Test Result -----------------------
            Assert.AreSame(col, gridBase.BusinessObjectCollection);
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
            Assert.IsNull(gridBase.BusinessObjectCollection);
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
            Assert.IsNull(gridBase.BusinessObjectCollection);
            //---------------Execute Test ----------------------


            gridBase.SelectedBusinessObject = null;
            //---------------Test Result -----------------------
            Assert.AreEqual(null, gridBase.SelectedBusinessObject);
            //---------------Tear Down -------------------------
        }
                
        [Test]
        public void Test_SetColumnWithCustomDateFormat()
        {
            //---------------Set up test pack-------------------
            IGridBase gridBase = CreateGridBaseStub();
            gridBase.Columns.Add("TestPropDate", "TestPropDate");
            IDataGridViewColumn col = gridBase.Columns[0];
            const string requiredFormat = "dd.MMM.yyyy";

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            col.DefaultCellStyle.Format = requiredFormat;

            //---------------Test Result -----------------------
            Assert.AreEqual(requiredFormat, col.DefaultCellStyle.Format);         
        }

        [Test]
        public void Test_SetDateToGridWithCustomDateFormat()
        {
            //---------------Set up test pack-------------------
            IDataGridViewColumn col;
            const string requiredFormat = "dd.MMM.yyyy";
            IGridBase gridBase = CreateGridBaseWithDateCustomFormatCol(out col,  requiredFormat);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(requiredFormat, col.DefaultCellStyle.Format);  

            //---------------Execute Test ----------------------
            DateTime expectedDate = DateTime.Now;
            gridBase.Rows.Add(expectedDate);

            //---------------Test Result -----------------------
            IDataGridViewCell dataGridViewCell = gridBase.Rows[0].Cells[0];
            //((DataGridViewCellVWG) dataGridViewCell).DataGridViewCell.HasStyle = false;

            Assert.AreEqual(expectedDate.ToString(requiredFormat), dataGridViewCell.FormattedValue);
//            Assert.AreEqual(currentDateTime.ToString("dd.MMM.yyyy") ,grid.Grid.Rows[0].Cells[formattedPropertyName].Value);
        }


        [Test]
        public void Test_SetDateToGridCustomFormat_LoadViaCollection()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadClassDefWithDateTime();
            IDataGridViewColumn column;
            const string requiredFormat = "dd.MMM.yyyy";
            IGridBase gridBase = CreateGridBaseWithDateCustomFormatCol(out column, requiredFormat);
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO bo = new MyBO();
            const string dateTimeProp = "TestDateTime";
            DateTime expectedDate = DateTime.Now;
            bo.SetPropertyValue(dateTimeProp, expectedDate);
            col.Add(bo);

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            gridBase.BusinessObjectCollection  = col;

            //---------------Test Result -----------------------

            IDataGridViewCell dataGridViewCell = gridBase.Rows[0].Cells[0];
            Assert.AreEqual(expectedDate.ToString(requiredFormat), dataGridViewCell.FormattedValue);

        }

        [Test]
        public void Test_SetDateToGridCustomFormat_LoadViaDataTable()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadClassDefWithDateTime();
            IDataGridViewColumn column;
            const string requiredFormat = "dd.MMM.yyyy";
            IGridBase gridBase = CreateGridBaseWithDateCustomFormatCol(out column, requiredFormat);
            DataTable dataTable = new DataTable();
            const string dateTimeProp = "TestDateTime";
            dataTable.Columns.Add(dateTimeProp, typeof (DateTime));
            DateTime expectedDate = DateTime.Now;
            dataTable.Rows.Add(expectedDate);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            gridBase.DataSource = dataTable.DefaultView;

            //---------------Test Result -----------------------

            IDataGridViewCell dataGridViewCell = gridBase.Rows[0].Cells[0];
            Assert.AreEqual(expectedDate.ToString(requiredFormat), dataGridViewCell.FormattedValue);

        }


        #region Utility Methods 
        
        private IGridBase CreateGridBaseWithDateCustomFormatCol(out IDataGridViewColumn col, string requiredFormat)
        {
            IGridBase gridBase = CreateGridBaseStub();
            gridBase.Columns.Add("TestDateTime", "TestDateTime");
            col = gridBase.Columns[0];
            col.DefaultCellStyle.Format = requiredFormat;
            return gridBase;
        }

        protected static void AddColumnsForContactPerson(IBusinessObjectCollection businessObjectCollection, IGridBase gridBase, string propName)
        {
            gridBase.Columns.Add(_gridIdColumnName, _gridIdColumnName);
            gridBase.Columns.Add(propName, propName);
            gridBase.BusinessObjectCollection =businessObjectCollection;
        }

        protected static void CreateBOAndAddToCollection(
            BusinessObjectCollection<ContactPersonTestBO> businessObjectCollection)
        {
            ContactPersonTestBO cp = new ContactPersonTestBO {Surname = Guid.NewGuid().ToString("N")};
            cp.Save();
            businessObjectCollection.Add(cp);
        }
        protected static BusinessObjectCollection<MyBO> CreateCollectionWith_4_Objects()
        {
            MyBO cp = new MyBO {TestProp = "b"};
            MyBO cp2 = new MyBO {TestProp = "d"};
            MyBO cp3 = new MyBO {TestProp = "c"};
            MyBO cp4 = new MyBO {TestProp = "a"};
            return new BusinessObjectCollection<MyBO> {{cp, cp2, cp3, cp4}};
        }

        private static IDataGridViewCell GetCell(int rowIndex, IDataGridView gridBase, string propName)
        {
            IDataGridViewRow row = gridBase.Rows[rowIndex];
            return row.Cells[propName];
        }

        protected IGridBase GetGridBaseWith_4_Rows(out BusinessObjectCollection<MyBO> col)
        {
            MyBO.LoadDefaultClassDef();
            col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            SetupGridColumnsForMyBo(gridBase);
            gridBase.BusinessObjectCollection  = col;
            return gridBase;
        }


        protected static void SetupGridColumnsForMyBo(IDataGridView gridBase)
        {
            gridBase.Columns.Add(_gridIdColumnName, _gridIdColumnName);
            gridBase.Columns.Add("TestProp", "TestProp");
        }


        

        #endregion
    }



    internal class DataGridViewColumnStub : IDataGridViewColumn
    {
        ///<summary>
        /// Returns the underlying control being wrapped by this decorator.
        ///</summary>
        public object Control
        {
            get { throw new System.NotImplementedException(); }
        }

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
        public string DataPropertyName { get; set; }

        /// <summary>Gets or sets the caption text on the column's header cell.</summary>
        /// <returns>A <see cref="T:System.String"></see> with the desired text. The default is an empty string ("").</returns>
        /// <filterpriority>1</filterpriority>
        public string HeaderText { get; set; }

        /// <summary>Gets or sets the name of the column.</summary>
        /// <returns>A <see cref="T:System.String"></see> that contains the name of the column. The default is an empty string ("").</returns>
        /// <filterpriority>1</filterpriority>
        public string Name { get; set; }

        /// <summary>Gets or sets a value indicating whether the user can edit the column's cells.</summary>
        /// <returns>true if the user cannot edit the column's cells; otherwise, false.</returns>
        /// <exception cref="T:System.InvalidOperationException">This property is set to false for a column that is bound to a read-only data source. </exception>
        /// <filterpriority>1</filterpriority>
        public bool ReadOnly { get; set; }

        /// <summary>Gets or sets the sort mode for the column.</summary>
        /// <returns>A <see cref="DataGridViewColumnSortMode"></see> that specifies the criteria used to order the rows based on the cell values in a column.</returns>
        /// <exception cref="System.InvalidOperationException">The value assigned to the property conflicts with IDataGridView.SelectionMode. </exception>
        /// <filterpriority>1</filterpriority>
        public DataGridViewColumnSortMode SortMode
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>Gets or sets the text used for ToolTips.</summary>
        /// <returns>The text to display as a ToolTip for the column.</returns>
        /// <filterpriority>1</filterpriority>
        public string ToolTipText { get; set; }

        /// <summary>Gets or sets the data type of the values in the column's cells.</summary>
        /// <returns>A <see cref="T:System.Type"></see> that describes the run-time class of the values stored in the column's cells.</returns>
        /// <filterpriority>1</filterpriority>
        public Type ValueType { get; set; }

        /// <summary>Gets or sets the current width of the column.</summary>
        /// <returns>The width, in pixels, of the column. The default is 100.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The specified value when setting this property is greater than 65536.</exception>
        /// <filterpriority>1</filterpriority>
        public int Width { get; set; }

        public bool Visible
        {
            get { return false; }
            set { }
        }

        /// <summary>Gets or sets the column's default cell style.</summary>
        /// <returns>A <see cref="IDataGridViewCellStyle"></see> that represents the default style of the cells in the column.</returns>
        /// <filterpriority>1</filterpriority>
        public IDataGridViewCellStyle DefaultCellStyle
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public object DataGridViewColumn
        {
            get { throw new NotImplementedException(); }
        }
    }
}