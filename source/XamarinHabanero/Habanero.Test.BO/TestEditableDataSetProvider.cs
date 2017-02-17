#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion

using NUnit.Framework;

namespace Habanero.Test.BO
{
#pragma warning disable 612,618
//pragma warning disable 612,618 - Although obselete the tests are still required.
    /// <summary>
    /// Summary description for TestEditableDataSetProvider.
    /// </summary>
    [TestFixture]
    [Ignore("Xamarin Port - UI Elements Not Needed")]
    public class TestEditableDataSetProvider : TestDataSetProvider
    {
//        protected override IDataSetProvider CreateDataSetProvider(IBusinessObjectCollection col)
//        {
//            return new EditableDataSetProvider(col);
//        }
//        [Test]
//        public override void TestUpdateBusinessObjectRowValues()
//        {
//            Assert.IsTrue(true, "This test cannot be conducted since edits to the grid update the BO and visa versa");
//        }

//        [Test]
//        public void TestUpdateRowUpdatesBusinessObject()
//        {
//            SetupTestData();
//            itsTable.Rows[0]["TestProp"] = "bo1prop1updated";
//            Assert.AreEqual("bo1prop1updated", itsBo1.GetPropertyValue("TestProp"));
//        }

//        [Test]
//        public void TestAcceptChangesSavesBusinessObjects()
//        {
//            SetupTestData();
//            SetupSaveExpectation();
//            itsTable.Rows[0]["TestProp"] = "bo1prop1updated";
//            itsTable.AcceptChanges();
//        }

//        [Test]
//        public void TestAddRowCreatesBusinessObjectThroughCollection()
//        {
//            SetupTestData();
//            //---------------Set up test pack-------------------
//            BusinessObjectCollection<MyBO> boCollection = new BusinessObjectCollection<MyBO>();
//            MyBO bo = new MyBO();
//            bo.SetPropertyValue("TestProp", "bo1prop1");
//            bo.SetPropertyValue("TestProp2", "s1");
//            bo.Save();
//            boCollection.Add(bo);

//            MyBO bo2 = new MyBO();
//            bo2.SetPropertyValue("TestProp", "bo2prop1");
//            bo2.SetPropertyValue("TestProp2", "s2");
//            bo2.Save();
//            boCollection.Add(bo2);

//            _dataSetProvider = new EditableDataSetProvider(boCollection);
//            BOMapper mapper = new BOMapper(boCollection.ClassDef.CreateNewBusinessObject());

//            itsTable = _dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);


//            //--------------Assert PreConditions----------------            
//            Assert.AreEqual(2, boCollection.Count);
//            Assert.AreEqual(0, boCollection.CreatedBusinessObjects.Count, "Should be no created items to start");

//            //---------------Execute Test ----------------------
//            itsTable.Rows.Add(new object[] {null, "bo3prop1", "bo3prop2"});

//            //---------------Test Result -----------------------
//            Assert.AreEqual
//                (1, boCollection.CreatedBusinessObjects.Count,
//                 "Adding a row to the table should use the collection to create the object");
//            //Assert.AreEqual(2, boCollection.Count, "Adding a row to the table should not add a bo to the main collection");
//            Assert.AreEqual(3, boCollection.Count, "Adding a row to the table should add a bo to the main collection");
//        }

//        [Test]
//        public void TestAddRowP_RowValueDBNull_BOProp_NUll_ShouldNotRaiseException_FIXBUG()
//        {
//            //---------------Set up test pack-------------------
//            BORegistry.DataAccessor = new DataAccessorInMemory();
//            ClassDef.ClassDefs.Clear();
//            MyBO.LoadClassDefWithUIAllDataTypes();
//            BusinessObjectCollection<MyBO> boCollection = new BusinessObjectCollection<MyBO>();

//            _dataSetProvider = new EditableDataSetProvider(boCollection);
//            BOMapper mapper = new BOMapper(boCollection.ClassDef.CreateNewBusinessObject());
//            itsTable = _dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);

//            //--------------Assert PreConditions----------------            
//            Assert.AreEqual(0, boCollection.Count);
//            Assert.AreEqual(0, boCollection.PersistedBusinessObjects.Count, "Should be no created items to start");
//            //            Assert.AreEqual(0, boCollection.CreatedBusinessObjects.Count, "Should be no created items to start");

//            //---------------Execute Test ----------------------
//            itsTable.Rows.Add(new object[] { DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value });
//            //---------------Test Result -----------------------
//            Assert.IsFalse(itsTable.Rows[0].HasErrors);
//            //Behaviour has been changed to persist the business object
//            Assert.AreEqual
//                (1, boCollection.PersistedBusinessObjects.Count,
//                 "Adding a row to the table should use the collection to create the object");
//            //            Assert.AreEqual
//            //                (1, boCollection.CreatedBusinessObjects.Count,
//            //                 "Adding a row to the table should use the collection to create the object");
//            Assert.AreEqual(1, boCollection.Count, "Adding a row to the table should add a bo to the main collection");
//        }

//        [Test]
//        public void TestAddRowP_RowValueDBNull_VirtualProp_NUll_ShouldNotRaiseException_FIXBUG()
//        {
//            //---------------Set up test pack-------------------
//            BORegistry.DataAccessor = new DataAccessorInMemory();
//            ClassDef.ClassDefs.Clear();
//            IClassDef classDef = MyBO.LoadClassDefWithUIAllDataTypes();
//            classDef.UIDefCol["default"].UIGrid.Add(new UIGridColumn("VirtualProp", "-TestProp-", typeof(DataGridViewTextBoxColumn), true, 100, PropAlignment.left, new Hashtable()));
//            BusinessObjectCollection<MyBO> boCollection = new BusinessObjectCollection<MyBO>();

//            _dataSetProvider = new EditableDataSetProvider(boCollection);
//            BOMapper mapper = new BOMapper(boCollection.ClassDef.CreateNewBusinessObject());
//            itsTable = _dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);

//            //--------------Assert PreConditions----------------            
//            Assert.AreEqual(0, boCollection.Count);
//            Assert.AreEqual(0, boCollection.PersistedBusinessObjects.Count, "Should be no created items to start");
//            //            Assert.AreEqual(0, boCollection.CreatedBusinessObjects.Count, "Should be no created items to start");

//            //---------------Execute Test ----------------------
//            itsTable.Rows.Add(new object[] { DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value });
//            //---------------Test Result -----------------------
//            Assert.IsFalse(itsTable.Rows[0].HasErrors);
//            //Behaviour has been changed to persist the business object
//            Assert.AreEqual
//                (1, boCollection.PersistedBusinessObjects.Count,
//                 "Adding a row to the table should use the collection to create the object");
//            //            Assert.AreEqual
//            //                (1, boCollection.CreatedBusinessObjects.Count,
//            //                 "Adding a row to the table should use the collection to create the object");
//            Assert.AreEqual(1, boCollection.Count, "Adding a row to the table should add a bo to the main collection");
//        }
//        [Test]
//        public void TestAddRowP_RowValueDBNull_BOProp_DateTime_NUll_ShouldNotRaiseException_FIXBUG()
//        {
//            //---------------Set up test pack-------------------
//            BORegistry.DataAccessor = new DataAccessorInMemory();
//            SetupTestData();
//            BusinessObjectCollection<MyBO> boCollection = new BusinessObjectCollection<MyBO>();

//            _dataSetProvider = new EditableDataSetProvider(boCollection);
//            BOMapper mapper = new BOMapper(boCollection.ClassDef.CreateNewBusinessObject());
//            itsTable = _dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);

//            //--------------Assert PreConditions----------------            
//            Assert.AreEqual(0, boCollection.Count);
//            Assert.AreEqual(0, boCollection.PersistedBusinessObjects.Count, "Should be no created items to start");
////            Assert.AreEqual(0, boCollection.CreatedBusinessObjects.Count, "Should be no created items to start");

//            //---------------Execute Test ----------------------
//            itsTable.Rows.Add(new object[] { DBNull.Value, DBNull.Value, DBNull.Value });
//            //---------------Test Result -----------------------
//            Assert.AreEqual
//                (1, boCollection.PersistedBusinessObjects.Count,
//                 "Adding a row to the table should use the collection to create the object");
////            Assert.AreEqual
////                (1, boCollection.CreatedBusinessObjects.Count,
////                 "Adding a row to the table should use the collection to create the object");
//            Assert.AreEqual(1, boCollection.Count, "Adding a row to the table should add a bo to the main collection");
//        }
//        [Test]
//        public void TestDeleteRowMarksBOAsDeleted()
//        {
//            SetupTestData();
//            Assert.AreEqual(2, _collection.Count, "Before Deleting a row shouldn't remove any Bo's from the collection.");
//            itsTable.Rows[0].Delete();
//            Assert.AreEqual(1, _collection.Count, "Deleting a row should remove any BO from the collection.");
//            int numDeleted = _collection.MarkedForDeleteBusinessObjects.Count;
//            Assert.AreEqual(0, numDeleted, "BO should be marked as deleted.");
//            //This has been changed to permanently delete the Business Object
////            int numDeleted = _collection.MarkedForDeleteBusinessObjects.Count;
////            Assert.AreEqual(1, numDeleted, "BO should be marked as deleted.");
//        }

//        [Test]
//        public void TestAddRowAddsBo()
//        {
//            //---------------Set up test pack-------------------
//            FixtureEnvironment.ClearBusinessObjectManager();
//            SetupTestData();
//            int originalCount = _collection.Count;
//            //---------------Assert Precondition----------------
//            Assert.AreEqual(originalCount, _collection.Count);
//            Assert.AreEqual(1, _collection.CreatedBusinessObjects.Count);
//            Assert.AreEqual(originalCount, itsTable.Rows.Count);
//            //---------------Execute Test ----------------------
//            itsTable.Rows.Add(new object[] { null, "bo1prop1", "s1" });
//            //---------------Test Result ----------------
//            Assert.AreEqual(originalCount + 1, _collection.Count);
//            //Changed to auto persist
//            Assert.AreEqual(1, _collection.CreatedBusinessObjects.Count);
////            Assert.AreEqual(2 , _collection.CreatedBusinessObjects.Count);
//            Assert.AreEqual(originalCount + 1, itsTable.Rows.Count);
////            itsTable.RejectChanges();
////            //---------------Test Result -----------------------
////            Assert.AreEqual(originalCount, itsTable.Rows.Count);
////            Assert.AreEqual(originalCount, _collection.Count);
//        }

//        [Ignore("This is has been removed because the system automatically saves valid new objects")] //0 May 2009: This is has been removed because the system automatically saves valid new objects
//        [Test]
//        public void TestRejectChangesRemovesNewRow()
//        {
//            //---------------Set up test pack-------------------
//            FixtureEnvironment.ClearBusinessObjectManager();
//            SetupTestData();
//            int originalCount = _collection.Count;
//            itsTable.Rows.Add(new object[] { null, "bo1prop1", "s1" });
//            //---------------Assert Precondition----------------
//            Assert.AreEqual(originalCount + 1, _collection.Count);
//            //Now Deletes automatically Brett May 2009
////            Assert.AreEqual(originalCount, _collection.CreatedBusinessObjects.Count);
//            Assert.AreEqual(originalCount - 1, _collection.CreatedBusinessObjects.Count);
//            Assert.AreEqual(originalCount + 1, itsTable.Rows.Count);
//            //---------------Execute Test ----------------------
//            itsTable.RejectChanges();
//            //---------------Test Result -----------------------
//            Assert.AreEqual(originalCount, itsTable.Rows.Count);
//            Assert.AreEqual(originalCount -1, _collection.CreatedBusinessObjects.Count);
//            Assert.AreEqual(originalCount, _collection.Count);
//        }
////        [Test]
////        public void TestRejectChangesUnDoesDeletedRow()
////        {
////            //---------------Set up test pack-------------------
////            FixtureEnvironment.ClearBusinessObjectManager();
////            SetupTestData();
////            _collection.SaveAll();
////            int originalCount = _collection.Count;
//////            itsTable.Rows.Add(new object[] { null, "bo1prop1", "s1" });
////            DataRow row = itsTable.Rows[1];
////            row.Delete();
////            //---------------Assert Precondition----------------
////            Assert.AreEqual(1, _collection.MarkedForDeleteBusinessObjects.Count);
////            Assert.AreEqual(originalCount, itsTable.Rows.Count);
////            Assert.AreEqual(DataRowState.Deleted,  row.RowState);
////            Assert.AreEqual(originalCount -1, _collection.Count);
////            //---------------Execute Test ----------------------
////            itsTable.RejectChanges();
////            //---------------Test Result -----------------------
////            Assert.AreEqual(originalCount, itsTable.Rows.Count);
////            Assert.AreEqual(0, _collection.MarkedForDeleteBusinessObjects.Count);
////            Assert.AreEqual(originalCount, _collection.Count);
////        }

//        [Test]
//        public void TestAcceptChangesSavesNewBusinessObjects()
//        {
//            SetupTestData();
//            SetupSaveExpectation();
//            ((EditableDataSetProvider) _dataSetProvider).Connection = itsConnection;
//            itsTable.Rows.Add(new object[] {null, "bo3prop1", "bo3prop2"});
//            itsTable.AcceptChanges();
//        }

//        [Test]
//        public void TestDeleteRowDeletesBOOnSave()
//        {
//            SetupTestData();
//            SetupSaveExpectation();

//            itsTable.AcceptChanges();
//            itsTable.Rows[0].Delete();
//            itsTable.AcceptChanges();
//        }

//        [Test]
//        public void TestDeleteRow_ThenRemoveBusinessObjectRemovesRow()
//        {
//            SetupTestData();
//            _collection.SaveAll();
//            //---------------Assert Precondition----------------
//            Assert.AreEqual(2, itsTable.Rows.Count);
//            Assert.AreEqual(2, _collection.Count);
//            //---------------Execute Test-----------
//            itsTable.Rows[1].Delete();
//            _collection.Remove(itsBo1);
//            //---------------Test Result -----------
//            Assert.AreEqual(0, _collection.Count);
//            //Now Deletes automatically Brett May 2009
////            Assert.AreEqual(1, _collection.MarkedForDeleteBusinessObjects.Count);
//            Assert.AreEqual(0, _collection.MarkedForDeleteBusinessObjects.Count);
//            Assert.AreEqual(1, _collection.RemovedBusinessObjects.Count);
//            Assert.AreEqual(DataRowState.Deleted, itsTable.Rows[0].RowState);
//            Assert.AreEqual(1, itsTable.Rows.Count);
//        }
//        [Test]
//        public void TestRemoveBusinessObject_ThenDeleteRow_UpdatesCollection()
//        {
//            SetupTestData();
//            _collection.SaveAll();
//            //---------------Assert Precondition----------------
//            Assert.AreEqual(2, itsTable.Rows.Count);
//            Assert.AreEqual(2, _collection.Count);
//            //---------------Execute Test-----------
//            _collection.Remove(itsBo1);
//            itsTable.Rows[0].Delete();
//            //---------------Test Result -----------
//            Assert.AreEqual(0, _collection.Count);
//            //Now Deletes automatically Brett May 2009
////            Assert.AreEqual(1, _collection.MarkedForDeleteBusinessObjects.Count);
//            Assert.AreEqual(0, _collection.MarkedForDeleteBusinessObjects.Count);
//            Assert.AreEqual(1, _collection.RemovedBusinessObjects.Count);
//            Assert.AreEqual(DataRowState.Deleted, itsTable.Rows[0].RowState);
//            Assert.AreEqual(1, itsTable.Rows.Count);
//        }
//        [Test]
//        public void TestDeleteRow_UpdatesColAndTable()
//        {
//            SetupTestData();
//            _collection.SaveAll();
//            //---------------Assert Precondition----------------
//            Assert.AreEqual(2, itsTable.Rows.Count);
//            Assert.AreEqual(2, _collection.Count);
//            Assert.AreEqual(0, _collection.MarkedForDeleteBusinessObjects.Count);
//            //---------------Execute Test-----------
//            itsTable.Rows[1].Delete();
//            //---------------Test Result -----------
//            Assert.AreEqual(1, _collection.Count);
//            //Now Deletes automatically Brett May 2009
//            Assert.AreEqual(0, _collection.MarkedForDeleteBusinessObjects.Count);
////            Assert.AreEqual(1, _collection.MarkedForDeleteBusinessObjects.Count);
//            Assert.AreEqual(DataRowState.Deleted, itsTable.Rows[1].RowState);
//        }
//        [Test]
//        public void TestRemoveItem_UpdatesTable()
//        {
//            SetupTestData();
//            _collection.SaveAll();
//            //---------------Assert Precondition----------------
//            Assert.AreEqual(2, itsTable.Rows.Count);
//            Assert.AreEqual(2, _collection.Count);
//            Assert.AreEqual(0, _collection.MarkedForDeleteBusinessObjects.Count);
//            //---------------Execute Test-----------
//            _collection.Remove(itsBo1);
//            //---------------Test Result -----------
//            Assert.AreEqual(1, _collection.Count);
//            Assert.AreEqual(1, _collection.RemovedBusinessObjects.Count);
//            Assert.AreEqual(1, itsTable.Rows.Count);
////            Assert.AreEqual(DataRowState.Deleted, itsTable.Rows[1].RowState);
//        }

////        [Test,
////         Ignore(
////             "Brett - to consult with peter by fundamentally changing the way BO's respond to edits we are fundamentally altering the way these data providers work"
////             )]
////        [Test]
////        public void TestRevertChangesRevertsBoValues()
////        {
////            SetupTestData();
////            itsTable.Rows[0]["TestProp"] = "bo1prop1updated";
////            itsTable.RejectChanges();
////            Assert.AreEqual("bo1prop1", itsBo1.GetPropertyValue("TestProp"));
////            itsTable.AcceptChanges();
////            itsTable.Rows[0].Delete();
////            Assert.IsTrue(itsBo1.Status.IsDeleted);
////            itsTable.RejectChanges();
////            Assert.IsFalse(itsBo1.Status.IsDeleted);
////        }

//        [Test]
//        public void TestAddBOToCollectionAddsRow()
//        {
//            SetupTestData();
//            //IBusinessObject newBo = _classDef.CreateNewBusinessObject(itsConnection);
//            IBusinessObject newBo = _classDef.CreateNewBusinessObject();
//            _collection.Add(newBo);
//            Assert.AreEqual(3, itsTable.Rows.Count);
//        }

//        [Test]
//        public void TestAddBOToCollectionAddsCorrectValues()
//        {
//            SetupTestData();
//            //IBusinessObject newBo = _classDef.CreateNewBusinessObject(itsConnection);
//            IBusinessObject newBo = _classDef.CreateNewBusinessObject();
//            newBo.SetPropertyValue("TestProp", "TestVal");
//            _collection.Add(newBo);
//            Assert.AreEqual("TestVal", itsTable.Rows[2][1]);
//        }

//        [Test]
//        public void TestDuplicateColumnNames()
//        {
//            //---------------Set up test pack-------------------
//            SetupTestData();
//            BOMapper mapper = new BOMapper(_collection.ClassDef.CreateNewBusinessObject());
//            //---------------Execute Test ----------------------
//            try
//            {
//                itsTable = _dataSetProvider.GetDataTable(mapper.GetUIDef("duplicateColumns").UIGrid);
//                Assert.Fail("Expected to throw an DuplicateNameException");
//            }
//                //---------------Test Result -----------------------
//            catch (DuplicateNameException ex)
//            {
//                StringAssert.Contains("Only one column per property can be specified", ex.Message);
//            }
//        }

//        [Test]
//        public void TestFind()
//        {
//            SetupTestData();
//            IBusinessObject bo = ((EditableDataSetProvider) _dataSetProvider).Find(0);
//            Assert.AreEqual(_collection[0], bo);

//            MyBO unlistedBO = new MyBO();
//            Assert.AreEqual(-1, ((EditableDataSetProvider) _dataSetProvider).FindRow(unlistedBO));
//        }
//        [Test]
//        public void TestAddBusinessObjectAndUpdatesAnotherRow_UpdatesBO()
//        {
//            SetupTestData();
//            //IBusinessObject bo3 = _classDef.CreateNewBusinessObject(itsConnection);
//            IBusinessObject bo3 = _classDef.CreateNewBusinessObject();
//            bo3.SetPropertyValue("TestProp", "bo3prop1");
//            bo3.SetPropertyValue("TestProp2", "s2");
//            const string updatedvalue = "UpdatedValue";
//            object origionalPropValue = itsBo1.GetPropertyValue("TestProp");
//            //---------------Assert Precondition----------------
//            Assert.AreEqual(2, itsTable.Rows.Count);
//            Assert.AreNotEqual(updatedvalue, itsTable.Rows[0][1]);
//            Assert.AreEqual(origionalPropValue, itsTable.Rows[0][1]);
//            //---------------Execute Test ----------------------
//            _collection.Add(bo3);
//            itsTable.Rows[0]["TestProp"] = updatedvalue;
//            //---------------Test Result -----------------------
////            Assert.AreEqual(3, itsTable.Rows.Count);
//            Assert.AreEqual(updatedvalue, itsTable.Rows[0][1]);
//            Assert.AreNotEqual(origionalPropValue, itsTable.Rows[0][1]);
//            Assert.AreEqual(itsTable.Rows[0][0], itsBo1.ID.AsString_CurrentValue());
//            Assert.AreEqual(updatedvalue, itsBo1.GetPropertyValue("TestProp"));
//            Assert.AreNotEqual(origionalPropValue, itsBo1.GetPropertyValue("TestProp"));
//        }
//        [Test]
//        public void TestAddBusinessObject_ThenAddRow_UpdatesCollection()
//        {
//            BusinessObjectCollection<MyBO> col = null;
//            IDataSetProvider dataSetProvider = GetDataSetProviderWithCollection(ref col);
//            BOMapper mapper = new BOMapper(col.ClassDef.CreateNewBusinessObject());
//            DataTable table = dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);
//            MyBO boNew = new MyBO();
//            boNew.SetPropertyValue("TestProp", "bo3prop1");
//            boNew.SetPropertyValue("TestProp2", "s2");
//            //---------------Assert Precondition----------------
//            Assert.AreEqual(4, table.Rows.Count);
//            Assert.AreEqual(4, col.Count);
//            Assert.AreEqual(4, col.CreatedBusinessObjects.Count);
//            //---------------Execute Test ----------------------
//            col.Add(boNew);
//            table.Rows.Add(new object[] { null, "bo3prop1", "bo3prop2" });
//            //---------------Test Result -----------------------
//            Assert.AreEqual(6, col.Count);
//            Assert.AreEqual(6, col.CreatedBusinessObjects.Count);
//            Assert.AreEqual(6, table.Rows.Count);
//        }

//        [Test]
//        public void TestAddRow_UpdatesCollection()
//        {
//            BusinessObjectCollection<MyBO> col = null;
//            IDataSetProvider dataSetProvider = GetDataSetProviderWithCollection(ref col);
//            BOMapper mapper = new BOMapper(col.ClassDef.CreateNewBusinessObject());
//            DataTable table = dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);
//            MyBO boNew = new MyBO();
//            boNew.SetPropertyValue("TestProp", "bo3prop1");
//            boNew.SetPropertyValue("TestProp2", "s2");
//            //---------------Assert Precondition----------------
//            Assert.AreEqual(4, table.Rows.Count);
//            Assert.AreEqual(4, col.Count);
//            Assert.AreEqual(4, col.CreatedBusinessObjects.Count);
//            //---------------Execute Test ----------------------
//            table.Rows.Add(new object[] { null, "bo3prop1", "bo3prop2" });
//            //---------------Test Result -----------------------
//            Assert.AreEqual(5, col.Count);
//            Assert.AreEqual(5, col.CreatedBusinessObjects.Count);
//            Assert.AreEqual(5, table.Rows.Count);
//        }

//        [Test]
//        public void TestAddRow_ThenAddBO_UpdatesTable()
//        {
//            BusinessObjectCollection<MyBO> col = null;
//            IDataSetProvider dataSetProvider = GetDataSetProviderWithCollection(ref col);
//            BOMapper mapper = new BOMapper(col.ClassDef.CreateNewBusinessObject());
//            DataTable table = dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);
//            MyBO boNew = new MyBO();
//            boNew.SetPropertyValue("TestProp", "bo3prop1");
//            boNew.SetPropertyValue("TestProp2", "s2");
//            //---------------Assert Precondition----------------
//            Assert.AreEqual(4, table.Rows.Count);
//            Assert.AreEqual(4, col.Count);
//            Assert.AreEqual(4, col.CreatedBusinessObjects.Count);
//            //---------------Execute Test ----------------------
//            table.Rows.Add(new object[] { null, "bo3prop1", "bo3prop2" });
//            col.Add(boNew);
//            //---------------Test Result -----------------------
//            Assert.AreEqual(6, col.Count);
//            Assert.AreEqual(6, col.CreatedBusinessObjects.Count);
//            Assert.AreEqual(6, table.Rows.Count);
//        }

////                    
////
////            //---------------Test Result -----------------------
////            Assert.AreEqual
////                (1, boCollection.CreatedBusinessObjects.Count,
////                 "Adding a row to the table should use the collection to create the object");

//        [Test]
//        public void Test_EditDataTableEditsBo()
//        {
//            //---------------Set up test pack-------------------
//            BusinessObjectCollection<MyBO> col = null;
//            IDataSetProvider dataSetProvider = GetDataSetProviderWithCollection(ref col);
//            BOMapper mapper = new BOMapper(col.ClassDef.CreateNewBusinessObject());
//            DataTable table = dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);
//            MyBO bo1 = col[0];
//            const string updatedvalue = "UpdatedValue";
//            const string columnName = "TestProp";
//            object origionalPropValue = bo1.GetPropertyValue(columnName);
//            //---------------Assert Precondition----------------
//            Assert.IsTrue(dataSetProvider is EditableDataSetProvider);
//            Assert.AreEqual(4, table.Rows.Count);
//            Assert.AreEqual(bo1.ID.AsString_CurrentValue(), table.Rows[0][_dataTableIdColumnName]);
//            Assert.AreEqual(origionalPropValue, table.Rows[0][columnName]);
//            //---------------Execute Test ----------------------
//            table.Rows[0][columnName] = updatedvalue;
//            //---------------Test Result -----------------------
//            Assert.AreEqual(bo1.ID.AsString_CurrentValue(), table.Rows[0][_dataTableIdColumnName]);
//            Assert.AreEqual(updatedvalue, table.Rows[0][columnName]);
//            Assert.AreEqual(updatedvalue, bo1.GetPropertyValue(columnName));
//        }

//        [Test]
//        public void Test_EditDataTable_ToDBNull_EditsBo()
//        {
//            //---------------Set up test pack-------------------
//            BusinessObjectCollection<MyBO> col = null;
//            IDataSetProvider dataSetProvider = GetDataSetProviderWithCollection(ref col);
//            BOMapper mapper = new BOMapper(col.ClassDef.CreateNewBusinessObject());
//            DataTable table = dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);
//            MyBO bo1 = col[0];
////            const string updatedvalue = "UpdatedValue";
//            const string columnName = "TestProp";
//            object origionalPropValue = bo1.GetPropertyValue(columnName);
//            //---------------Assert Precondition----------------
//            Assert.IsTrue(dataSetProvider is EditableDataSetProvider);
//            Assert.AreEqual(4, table.Rows.Count);
//            Assert.AreEqual(bo1.ID.AsString_CurrentValue(), table.Rows[0][_dataTableIdColumnName]);
//            Assert.AreEqual(origionalPropValue, table.Rows[0][columnName]);
//            //---------------Execute Test ----------------------
//            table.Rows[0][columnName] = DBNull.Value;
//            //---------------Test Result -----------------------
//            Assert.AreEqual(bo1.ID.AsString_CurrentValue(), table.Rows[0][_dataTableIdColumnName]);
//            Assert.AreEqual(DBNull.Value, table.Rows[0][columnName]);
//            Assert.AreEqual(null, bo1.GetPropertyValue(columnName));
//        }

//        [Test]
//        public void Test_EditDataTable_WhenMultipleLevelProp_ShouldEditRelatedBO()
//        {
//            //---------------Set up test pack-------------------
//            RecordingExceptionNotifier recordingExceptionNotifier = new RecordingExceptionNotifier();
//            GlobalRegistry.UIExceptionNotifier = recordingExceptionNotifier;
//            AddressTestBO.LoadDefaultClassDef();
//            ContactPersonTestBO.LoadClassDefWithOrganisationAndAddressRelationships();
//            OrganisationTestBO.LoadDefaultClassDef();
//            BusinessObjectCollection<AddressTestBO> addresses = new BusinessObjectCollection<AddressTestBO>();
//            addresses.Add(new AddressTestBO { ContactPersonTestBO = new ContactPersonTestBO() });
//            addresses.Add(new AddressTestBO { ContactPersonTestBO = new ContactPersonTestBO() });
//            addresses.Add(new AddressTestBO { ContactPersonTestBO = new ContactPersonTestBO() });

//            OrganisationTestBO organisation = new OrganisationTestBO();

//            UIGrid uiGrid = new UIGrid();
//            const string propertyName = "ContactPersonTestBO.OrganisationID";
//            uiGrid.Add(new UIGridColumn("Contact Organisation", propertyName, typeof(DataGridViewTextBoxColumn), true, 100, PropAlignment.left, new Hashtable()));

//            IDataSetProvider dataSetProvider = CreateDataSetProvider(addresses);
//            DataTable table = dataSetProvider.GetDataTable(uiGrid);

//            //---------------Assert Precondition----------------
//            Assert.IsTrue(dataSetProvider is EditableDataSetProvider);
//            Assert.AreEqual(3, table.Rows.Count);
//            Assert.AreEqual(DBNull.Value, table.Rows[0][propertyName]);
//            //---------------Execute Test ----------------------
//            table.Rows[0][propertyName] = organisation.OrganisationID;
//            //---------------Test Result -----------------------
//            Assert.AreEqual(organisation.OrganisationID, table.Rows[0][propertyName]);
//            Assert.AreEqual(organisation.OrganisationID, addresses[0].ContactPersonTestBO.OrganisationID);
//            recordingExceptionNotifier.RethrowRecordedException();
//        }

//        [Test]
//        public void Test_AddRow_WhenMultipleLevelProp_ShouldAddBOWithRelatedPropSet()
//        {
//            //---------------Set up test pack-------------------
//            RecordingExceptionNotifier recordingExceptionNotifier = new RecordingExceptionNotifier();
//            GlobalRegistry.UIExceptionNotifier = recordingExceptionNotifier;
//            ClassDef.ClassDefs.Clear();
//            AddressTestBO.LoadDefaultClassDef();
//            ContactPersonTestBO.LoadClassDefWithOrganisationAndAddressRelationships();
//            OrganisationTestBO.LoadDefaultClassDef();
//            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPerson();
//            BusinessObjectCollection<AddressTestBO> addresses = contactPersonTestBO.Addresses;

//            OrganisationTestBO organisation = new OrganisationTestBO();

//            UIGrid uiGrid = new UIGrid();
//            const string propertyName = "ContactPersonTestBO.OrganisationID";
//            uiGrid.Add(new UIGridColumn("Contact Organisation", propertyName, typeof(DataGridViewTextBoxColumn), true, 100, PropAlignment.left, new Hashtable()));

//            IDataSetProvider dataSetProvider = CreateDataSetProvider(addresses);
//            DataTable table = dataSetProvider.GetDataTable(uiGrid);

//            //---------------Assert Precondition----------------
//            Assert.IsTrue(dataSetProvider is EditableDataSetProvider);
//            Assert.AreEqual(0, table.Rows.Count);
//            Assert.AreEqual(0, addresses.Count);
//            //---------------Execute Test ----------------------
//            table.Rows.Add(new object[] { null, organisation.OrganisationID });
//            //---------------Test Result -----------------------
//            Assert.AreEqual(1, table.Rows.Count);
//            Assert.AreEqual(1, addresses.Count);
//            Assert.AreEqual(organisation.OrganisationID.ToString(), table.Rows[0][propertyName].ToString());
//            Assert.AreEqual(organisation.OrganisationID, contactPersonTestBO.OrganisationID);
//            recordingExceptionNotifier.RethrowRecordedException();
//        }

//        [Test]
//        public void Test_EditDataTable_WhenVirtualProp_ShouldEditRelatedVirtualProp()
//        {
//            //---------------Set up test pack-------------------
//            RecordingExceptionNotifier recordingExceptionNotifier = new RecordingExceptionNotifier();
//            GlobalRegistry.UIExceptionNotifier = recordingExceptionNotifier;
//            AddressTestBO.LoadDefaultClassDef();
//            var contactPersonClassDef = ContactPersonTestBO.LoadClassDefWithOrganisationAndAddressRelationships();
//            OrganisationTestBO.LoadDefaultClassDef();
//            BusinessObjectCollection<ContactPersonTestBO> contactPersonTestBOS = new BusinessObjectCollection<ContactPersonTestBO>();
//            contactPersonTestBOS.Add(new ContactPersonTestBO(), new ContactPersonTestBO(), new ContactPersonTestBO());

//            OrganisationTestBO organisation = new OrganisationTestBO();

//            UIGrid uiGrid = new UIGrid();
//            new UIDef("fdafdas", new UIForm(), uiGrid) { ClassDef = contactPersonClassDef };
//            const string propertyName = "-Organisation-";
//            uiGrid.Add(new UIGridColumn("Contact Organisation", propertyName, typeof(DataGridViewTextBoxColumn), true, 100, PropAlignment.left, new Hashtable()));

//            IDataSetProvider dataSetProvider = CreateDataSetProvider(contactPersonTestBOS);
//            DataTable table = dataSetProvider.GetDataTable(uiGrid);

//            //---------------Assert Precondition----------------
//            Assert.IsTrue(dataSetProvider is EditableDataSetProvider);
//            Assert.AreEqual(3, table.Rows.Count);
//            Assert.AreEqual(DBNull.Value, table.Rows[0][propertyName]);
//            Assert.AreEqual(null, contactPersonTestBOS[0].Organisation);
//            //---------------Execute Test ----------------------
//            table.Rows[0][propertyName] = organisation;
//            //---------------Test Result -----------------------
//            Assert.AreSame(organisation, table.Rows[0][propertyName]);
//            Assert.AreSame(organisation, contactPersonTestBOS[0].Organisation);
//            recordingExceptionNotifier.RethrowRecordedException();
//        }

//        [Test]
//        public void Test_AddRow_WhenVirtualProp_ShouldAddBOWithRelatedVirtualPropSet()
//        {
//            //---------------Set up test pack-------------------
//            RecordingExceptionNotifier recordingExceptionNotifier = new RecordingExceptionNotifier();
//            GlobalRegistry.UIExceptionNotifier = recordingExceptionNotifier;
//            AddressTestBO.LoadDefaultClassDef();
//            var contactPersonClassDef = ContactPersonTestBO.LoadClassDefWithOrganisationAndAddressRelationships();
//            OrganisationTestBO.LoadDefaultClassDef();
//            BusinessObjectCollection<ContactPersonTestBO> contactPersonTestBOS = new BusinessObjectCollection<ContactPersonTestBO>();

//            OrganisationTestBO organisation = new OrganisationTestBO();

//            UIGrid uiGrid = new UIGrid();
//            new UIDef("fdafdas", new UIForm(), uiGrid) {ClassDef = contactPersonClassDef};
//            const string propertyName = "-Organisation-";
//            uiGrid.Add(new UIGridColumn("Contact Organisation", propertyName, typeof(DataGridViewTextBoxColumn), true, 100, PropAlignment.left, new Hashtable()));

//            IDataSetProvider dataSetProvider = CreateDataSetProvider(contactPersonTestBOS);
//            DataTable table = dataSetProvider.GetDataTable(uiGrid);

//            //---------------Assert Precondition----------------
//            Assert.IsTrue(dataSetProvider is EditableDataSetProvider);
//            Assert.AreEqual(0, table.Rows.Count);
//            Assert.AreEqual(0, contactPersonTestBOS.Count);
//            //---------------Execute Test ----------------------
//            table.Rows.Add(new object[] { null, organisation });
//            //---------------Test Result -----------------------
//            Assert.AreEqual(1, table.Rows.Count);
//            Assert.AreEqual(1, contactPersonTestBOS.Count);
//            Assert.AreSame(organisation, table.Rows[0][propertyName]);
//            Assert.AreSame(organisation, contactPersonTestBOS[0].Organisation);
//        }

//        [Test]
//        public void TestGetConnection()
//        {
//            SetupTestData();
//            Assert.IsNull(((EditableDataSetProvider) _dataSetProvider).Connection);
//        }

    }
#pragma warning restore 612,618
}