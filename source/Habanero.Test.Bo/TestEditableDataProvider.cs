//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using System.Data;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestEditableDataProvider.
    /// </summary>
    [TestFixture]
    public class TestEditableDataProvider : TestDataSetProvider
    {
        protected override IDataSetProvider CreateDataSetProvider(IBusinessObjectCollection col)
        {
            return new EditableDataSetProvider(col);
        }

        public override void TestUpdateBusinessObjectRowValues()
        {
            Assert.IsTrue(true, "This test cannot be conducted since edits to the grid update the BO and visa versa");
        }

        [Test]
        public void TestUpdateRowUpdatesBusinessObject()
        {
            SetupTestData();
            itsTable.Rows[0]["TestProp"] = "bo1prop1updated";
            Assert.AreEqual("bo1prop1updated", itsBo1.GetPropertyValue("TestProp"));
        }

        [Test]
        public void TestAcceptChangesSavesBusinessObjects()
        {
            SetupTestData();
            SetupSaveExpectation();
            itsTable.Rows[0]["TestProp"] = "bo1prop1updated";
            itsTable.AcceptChanges();
        }

        [Test]
        public void TestAddRowCreatesBusinessObjectThroughCollection()
        {
            SetupTestData();
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> boCollection = new BusinessObjectCollection<MyBO>();
            MyBO bo = new MyBO();
            bo.SetPropertyValue("TestProp", "bo1prop1");
            bo.SetPropertyValue("TestProp2", "s1");
            bo.Save();
            boCollection.Add(bo);

            MyBO bo2 = new MyBO();
            bo2.SetPropertyValue("TestProp", "bo2prop1");
            bo2.SetPropertyValue("TestProp2", "s2");
            bo2.Save();
            boCollection.Add(bo2);

            _dataSetProvider = new EditableDataSetProvider(boCollection);
            BOMapper mapper = new BOMapper(boCollection.ClassDef.CreateNewBusinessObject());
            itsTable = _dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, boCollection.Count);
            Assert.AreEqual(0, boCollection.CreatedBusinessObjects.Count, "Should be no created items to start");

            //---------------Execute Test ----------------------
            itsTable.Rows.Add(new object[] {null, "bo3prop1", "bo3prop2"});

            //---------------Test Result -----------------------
            Assert.AreEqual
                (1, boCollection.CreatedBusinessObjects.Count,
                 "Adding a row to the table should use the collection to create the object");
            //Assert.AreEqual(2, boCollection.Count, "Adding a row to the table should not add a bo to the main collection");
            Assert.AreEqual(3, boCollection.Count, "Adding a row to the table should add a bo to the main collection");
        }

        [Test]
        public void TestAddRowP_RowValueDBNull_BOProp_NUll_ShouldNotRaiseException_FIXBUG()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithUIAllDataTypes();
            BusinessObjectCollection<MyBO> boCollection = new BusinessObjectCollection<MyBO>();

            _dataSetProvider = new EditableDataSetProvider(boCollection);
            BOMapper mapper = new BOMapper(boCollection.ClassDef.CreateNewBusinessObject());
            itsTable = _dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, boCollection.Count);
            Assert.AreEqual(0, boCollection.CreatedBusinessObjects.Count, "Should be no created items to start");

            //---------------Execute Test ----------------------
            itsTable.Rows.Add(new object[] { DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value });
            //---------------Test Result -----------------------
            Assert.IsFalse(itsTable.Rows[0].HasErrors);
            Assert.AreEqual
                (1, boCollection.CreatedBusinessObjects.Count,
                 "Adding a row to the table should use the collection to create the object");
            Assert.AreEqual(1, boCollection.Count, "Adding a row to the table should add a bo to the main collection");
        }
        [Test]
        public void TestAddRowP_RowValueDBNull_BOProp_DateTime_NUll_ShouldNotRaiseException_FIXBUG()
        {
            //---------------Set up test pack-------------------
            SetupTestData();
            BusinessObjectCollection<MyBO> boCollection = new BusinessObjectCollection<MyBO>();

            _dataSetProvider = new EditableDataSetProvider(boCollection);
            BOMapper mapper = new BOMapper(boCollection.ClassDef.CreateNewBusinessObject());
            itsTable = _dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, boCollection.Count);
            Assert.AreEqual(0, boCollection.CreatedBusinessObjects.Count, "Should be no created items to start");

            //---------------Execute Test ----------------------
            itsTable.Rows.Add(new object[] { DBNull.Value, DBNull.Value, DBNull.Value });
            //---------------Test Result -----------------------
            Assert.AreEqual
                (1, boCollection.CreatedBusinessObjects.Count,
                 "Adding a row to the table should use the collection to create the object");
            Assert.AreEqual(1, boCollection.Count, "Adding a row to the table should add a bo to the main collection");
        }
        [Test]
        public void TestDeleteRowMarksBOAsDeleted()
        {
            SetupTestData();
            Assert.AreEqual(2, _collection.Count, "Before Deleting a row shouldn't remove any Bo's from the collection.");
            itsTable.Rows[0].Delete();
            Assert.AreEqual(1, _collection.Count, "Deleting a row shouldn't remove any Bo's from the collection.");
            int numDeleted = _collection.MarkedForDeleteBusinessObjects.Count;
            Assert.AreEqual(1, numDeleted, "BO should be marked as deleted.");
        }

        [Test]
        public void TestAddRowAddsBo()
        {
            //---------------Set up test pack-------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            SetupTestData();
            int originalCount = _collection.Count;
            //---------------Assert Precondition----------------
            Assert.AreEqual(originalCount, _collection.Count);
            Assert.AreEqual(1, _collection.CreatedBusinessObjects.Count);
            Assert.AreEqual(originalCount, itsTable.Rows.Count);
            //---------------Execute Test ----------------------
            itsTable.Rows.Add(new object[] { null, "bo1prop1", "s1" });
            //---------------Test Result ----------------
            Assert.AreEqual(originalCount + 1, _collection.Count);
            Assert.AreEqual(2 , _collection.CreatedBusinessObjects.Count);
            Assert.AreEqual(originalCount + 1, itsTable.Rows.Count);
//            itsTable.RejectChanges();
//            //---------------Test Result -----------------------
//            Assert.AreEqual(originalCount, itsTable.Rows.Count);
//            Assert.AreEqual(originalCount, _collection.Count);
        }
        [Test]
        public void TestRejectChangesRemovesNewRow()
        {
            //---------------Set up test pack-------------------
            BusinessObjectManager.Instance.ClearLoadedObjects();
            SetupTestData();
            int originalCount = _collection.Count;
            itsTable.Rows.Add(new object[] { null, "bo1prop1", "s1" });
            //---------------Assert Precondition----------------
            Assert.AreEqual(originalCount + 1, _collection.Count);
            Assert.AreEqual(originalCount, _collection.CreatedBusinessObjects.Count);
            Assert.AreEqual(originalCount + 1, itsTable.Rows.Count);
            //---------------Execute Test ----------------------
            itsTable.RejectChanges();
            //---------------Test Result -----------------------
            Assert.AreEqual(originalCount, itsTable.Rows.Count);
            Assert.AreEqual(originalCount -1, _collection.CreatedBusinessObjects.Count);
            Assert.AreEqual(originalCount, _collection.Count);
        }
        [Test]
//        public void TestRejectChangesUnDoesDeletedRow()
//        {
//            //---------------Set up test pack-------------------
//            BusinessObjectManager.Instance.ClearLoadedObjects();
//            SetupTestData();
//            _collection.SaveAll();
//            int originalCount = _collection.Count;
////            itsTable.Rows.Add(new object[] { null, "bo1prop1", "s1" });
//            DataRow row = itsTable.Rows[1];
//            row.Delete();
//            //---------------Assert Precondition----------------
//            Assert.AreEqual(1, _collection.MarkedForDeleteBusinessObjects.Count);
//            Assert.AreEqual(originalCount, itsTable.Rows.Count);
//            Assert.AreEqual(DataRowState.Deleted,  row.RowState);
//            Assert.AreEqual(originalCount -1, _collection.Count);
//            //---------------Execute Test ----------------------
//            itsTable.RejectChanges();
//            //---------------Test Result -----------------------
//            Assert.AreEqual(originalCount, itsTable.Rows.Count);
//            Assert.AreEqual(0, _collection.MarkedForDeleteBusinessObjects.Count);
//            Assert.AreEqual(originalCount, _collection.Count);
//        }

        [Test]
        public void TestAcceptChangesSavesNewBusinessObjects()
        {
            SetupTestData();
            SetupSaveExpectation();
            ((EditableDataSetProvider) _dataSetProvider).Connection = itsConnection;
            itsTable.Rows.Add(new object[] {null, "bo3prop1", "bo3prop2"});
            itsTable.AcceptChanges();
        }

        [Test]
        public void TestDeleteRowDeletesBOOnSave()
        {
            SetupTestData();
            SetupSaveExpectation();

            itsTable.AcceptChanges();
            itsTable.Rows[0].Delete();
            itsTable.AcceptChanges();
        }

        [Test]
        public void TestDeleteRow_ThenRemoveBusinessObjectRemovesRow()
        {
            SetupTestData();
            _collection.SaveAll();
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, itsTable.Rows.Count);
            Assert.AreEqual(2, _collection.Count);
            //---------------Execute Test-----------
            itsTable.Rows[1].Delete();
            _collection.Remove(itsBo1);
            //---------------Test Result -----------
            Assert.AreEqual(0, _collection.Count);
            Assert.AreEqual(1, _collection.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(1, _collection.RemovedBusinessObjects.Count);
            Assert.AreEqual(DataRowState.Deleted, itsTable.Rows[0].RowState);
            Assert.AreEqual(1, itsTable.Rows.Count);
        }
        [Test]
        public void TestRemoveBusinessObject_ThenDeleteRow_UpdatesCollection()
        {
            SetupTestData();
            _collection.SaveAll();
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, itsTable.Rows.Count);
            Assert.AreEqual(2, _collection.Count);
            //---------------Execute Test-----------
            _collection.Remove(itsBo1);
            itsTable.Rows[0].Delete();
            //---------------Test Result -----------
            Assert.AreEqual(0, _collection.Count);
            Assert.AreEqual(1, _collection.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(1, _collection.RemovedBusinessObjects.Count);
            Assert.AreEqual(DataRowState.Deleted, itsTable.Rows[0].RowState);
            Assert.AreEqual(1, itsTable.Rows.Count);
        }
        [Test]
        public void TestDeleteRow_UpdatesColAndTable()
        {
            SetupTestData();
            _collection.SaveAll();
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, itsTable.Rows.Count);
            Assert.AreEqual(2, _collection.Count);
            Assert.AreEqual(0, _collection.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test-----------
            itsTable.Rows[1].Delete();
            //---------------Test Result -----------
            Assert.AreEqual(1, _collection.Count);
            Assert.AreEqual(1, _collection.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(DataRowState.Deleted, itsTable.Rows[1].RowState);
        }
        [Test]
        public void TestRemoveItem_UpdatesTable()
        {
            SetupTestData();
            _collection.SaveAll();
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, itsTable.Rows.Count);
            Assert.AreEqual(2, _collection.Count);
            Assert.AreEqual(0, _collection.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test-----------
            _collection.Remove(itsBo1);
            //---------------Test Result -----------
            Assert.AreEqual(1, _collection.Count);
            Assert.AreEqual(1, _collection.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, itsTable.Rows.Count);
//            Assert.AreEqual(DataRowState.Deleted, itsTable.Rows[1].RowState);
        }

//        [Test,
//         Ignore(
//             "Brett - to consult with peter by fundamentally changing the way BO's respond to edits we are fundamentally altering the way these data providers work"
//             )]
//        [Test]
//        public void TestRevertChangesRevertsBoValues()
//        {
//            SetupTestData();
//            itsTable.Rows[0]["TestProp"] = "bo1prop1updated";
//            itsTable.RejectChanges();
//            Assert.AreEqual("bo1prop1", itsBo1.GetPropertyValue("TestProp"));
//            itsTable.AcceptChanges();
//            itsTable.Rows[0].Delete();
//            Assert.IsTrue(itsBo1.Status.IsDeleted);
//            itsTable.RejectChanges();
//            Assert.IsFalse(itsBo1.Status.IsDeleted);
//        }

        [Test]
        public void TestAddBOToCollectionAddsRow()
        {
            SetupTestData();
            //IBusinessObject newBo = _classDef.CreateNewBusinessObject(itsConnection);
            IBusinessObject newBo = _classDef.CreateNewBusinessObject();
            _collection.Add(newBo);
            Assert.AreEqual(3, itsTable.Rows.Count);
        }

        [Test]
        public void TestAddBOToCollectionAddsCorrectValues()
        {
            SetupTestData();
            //IBusinessObject newBo = _classDef.CreateNewBusinessObject(itsConnection);
            IBusinessObject newBo = _classDef.CreateNewBusinessObject();
            newBo.SetPropertyValue("TestProp", "TestVal");
            _collection.Add(newBo);
            Assert.AreEqual("TestVal", itsTable.Rows[2][1]);
        }

        [Test, ExpectedException(typeof (DuplicateNameException))]
        public void TestDuplicateColumnNames()
        {
            SetupTestData();
            BOMapper mapper = new BOMapper(_collection.ClassDef.CreateNewBusinessObject());
            itsTable = _dataSetProvider.GetDataTable(mapper.GetUIDef("duplicateColumns").UIGrid);
        }

        [Test]
        public void TestFind()
        {
            SetupTestData();
            IBusinessObject bo = ((EditableDataSetProvider) _dataSetProvider).Find(0);
            Assert.AreEqual(_collection[0], bo);

            MyBO unlistedBO = new MyBO();
            Assert.AreEqual(-1, ((EditableDataSetProvider) _dataSetProvider).FindRow(unlistedBO));
        }
        [Test]
        public void TestAddBusinessObjectAndUpdatesAnotherRow_UpdatesBO()
        {
            SetupTestData();
            //IBusinessObject bo3 = _classDef.CreateNewBusinessObject(itsConnection);
            IBusinessObject bo3 = _classDef.CreateNewBusinessObject();
            bo3.SetPropertyValue("TestProp", "bo3prop1");
            bo3.SetPropertyValue("TestProp2", "s2");
            const string updatedvalue = "UpdatedValue";
            object origionalPropValue = itsBo1.GetPropertyValue("TestProp");
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, itsTable.Rows.Count);
            Assert.AreNotEqual(updatedvalue, itsTable.Rows[0][1]);
            Assert.AreEqual(origionalPropValue, itsTable.Rows[0][1]);
            //---------------Execute Test ----------------------
            _collection.Add(bo3);
            itsTable.Rows[0]["TestProp"] = updatedvalue;
            //---------------Test Result -----------------------
//            Assert.AreEqual(3, itsTable.Rows.Count);
            Assert.AreEqual(updatedvalue, itsTable.Rows[0][1]);
            Assert.AreNotEqual(origionalPropValue, itsTable.Rows[0][1]);
            Assert.AreEqual(itsTable.Rows[0][0], itsBo1.ID.AsString_CurrentValue());
            Assert.AreEqual(updatedvalue, itsBo1.GetPropertyValue("TestProp"));
            Assert.AreNotEqual(origionalPropValue, itsBo1.GetPropertyValue("TestProp"));
        }
        [Test]
        public void TestAddBusinessObject_ThenAddRow_UpdatesCollection()
        {
            BusinessObjectCollection<MyBO> col = null;
            IDataSetProvider dataSetProvider = GetDataSetProviderWithCollection(ref col);
            BOMapper mapper = new BOMapper(col.ClassDef.CreateNewBusinessObject());
            DataTable table = dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);
            MyBO boNew = new MyBO();
            boNew.SetPropertyValue("TestProp", "bo3prop1");
            boNew.SetPropertyValue("TestProp2", "s2");
            //---------------Assert Precondition----------------
            Assert.AreEqual(4, table.Rows.Count);
            Assert.AreEqual(4, col.Count);
            Assert.AreEqual(4, col.CreatedBusinessObjects.Count);
            //---------------Execute Test ----------------------
            col.Add(boNew);
            table.Rows.Add(new object[] { null, "bo3prop1", "bo3prop2" });
            //---------------Test Result -----------------------
            Assert.AreEqual(6, col.Count);
            Assert.AreEqual(6, col.CreatedBusinessObjects.Count);
            Assert.AreEqual(6, table.Rows.Count);
        }

        [Test]
        public void TestAddRow_UpdatesCollection()
        {
            BusinessObjectCollection<MyBO> col = null;
            IDataSetProvider dataSetProvider = GetDataSetProviderWithCollection(ref col);
            BOMapper mapper = new BOMapper(col.ClassDef.CreateNewBusinessObject());
            DataTable table = dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);
            MyBO boNew = new MyBO();
            boNew.SetPropertyValue("TestProp", "bo3prop1");
            boNew.SetPropertyValue("TestProp2", "s2");
            //---------------Assert Precondition----------------
            Assert.AreEqual(4, table.Rows.Count);
            Assert.AreEqual(4, col.Count);
            Assert.AreEqual(4, col.CreatedBusinessObjects.Count);
            //---------------Execute Test ----------------------
            table.Rows.Add(new object[] { null, "bo3prop1", "bo3prop2" });
            //---------------Test Result -----------------------
            Assert.AreEqual(5, col.Count);
            Assert.AreEqual(5, col.CreatedBusinessObjects.Count);
            Assert.AreEqual(5, table.Rows.Count);
        }

        [Test]
        public void TestAddRow_ThenAddBO_UpdatesTable()
        {
            BusinessObjectCollection<MyBO> col = null;
            IDataSetProvider dataSetProvider = GetDataSetProviderWithCollection(ref col);
            BOMapper mapper = new BOMapper(col.ClassDef.CreateNewBusinessObject());
            DataTable table = dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);
            MyBO boNew = new MyBO();
            boNew.SetPropertyValue("TestProp", "bo3prop1");
            boNew.SetPropertyValue("TestProp2", "s2");
            //---------------Assert Precondition----------------
            Assert.AreEqual(4, table.Rows.Count);
            Assert.AreEqual(4, col.Count);
            Assert.AreEqual(4, col.CreatedBusinessObjects.Count);
            //---------------Execute Test ----------------------
            table.Rows.Add(new object[] { null, "bo3prop1", "bo3prop2" });
            col.Add(boNew);
            //---------------Test Result -----------------------
            Assert.AreEqual(6, col.Count);
            Assert.AreEqual(6, col.CreatedBusinessObjects.Count);
            Assert.AreEqual(6, table.Rows.Count);
        }

//                    
//
//            //---------------Test Result -----------------------
//            Assert.AreEqual
//                (1, boCollection.CreatedBusinessObjects.Count,
//                 "Adding a row to the table should use the collection to create the object");



        [Test]
        public void Test_EditDataTableEditsBo()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col = null;
            IDataSetProvider dataSetProvider = GetDataSetProviderWithCollection(ref col);
            BOMapper mapper = new BOMapper(col.ClassDef.CreateNewBusinessObject());
            DataTable table = dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);
            MyBO bo1 = col[0];
            const string updatedvalue = "UpdatedValue";
            const string columnName = "TestProp";
            object origionalPropValue = bo1.GetPropertyValue(columnName);
            //---------------Assert Precondition----------------
            Assert.IsTrue(dataSetProvider is EditableDataSetProvider);
            Assert.AreEqual(4, table.Rows.Count);
            Assert.AreEqual(bo1.ID.AsString_CurrentValue(), table.Rows[0][_dataTableIdColumnName]);
            Assert.AreEqual(origionalPropValue, table.Rows[0][columnName]);
            //---------------Execute Test ----------------------
            table.Rows[0][columnName] = updatedvalue;
            //---------------Test Result -----------------------
            Assert.AreEqual(bo1.ID.AsString_CurrentValue(), table.Rows[0][_dataTableIdColumnName]);
            Assert.AreEqual(updatedvalue, table.Rows[0][columnName]);
            Assert.AreEqual(updatedvalue, bo1.GetPropertyValue(columnName));
        }

        [Test]
        public void TestGetConnection()
        {
            SetupTestData();
            Assert.IsNull(((EditableDataSetProvider) _dataSetProvider).Connection);
        }
    }
}