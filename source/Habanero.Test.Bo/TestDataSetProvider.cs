// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.BO
{

    /// <summary>
    /// Tests the DataSet provider base behaviour via the ReadOnlyDataSetProvider
    /// </summary
   [TestFixture]
    public class TestDataSetProvider : TestUsingDatabase
    {
        protected XmlClassLoader _loader;
        protected IClassDef _classDef;
        protected IBusinessObjectCollection _collection;
        protected DataTable itsTable;
        protected IBusinessObject itsBo1;
        protected IBusinessObject itsBo2;
        protected IBusinessObject itsRelatedBo;
        protected IDataSetProvider _dataSetProvider;

        protected Mock itsDatabaseConnectionMockControl;
        protected IDatabaseConnection itsConnection;
        protected const string _dataTableIdColumnName = "HABANERO_OBJECTID";

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            this.SetupDBConnection();
            OrderItem.CreateTable();
        }

        [TestFixtureTearDown]
        public void TearDownFixure()
        {
            OrderItem.DropTable();
        }

        [SetUp]
        public void SetupTest()
        {
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
            this.SetupDBConnection();
            ClassDef.ClassDefs.Clear();
            new Address();
        }

        public virtual void SetupTestData()
        {
            
            ClassDef.ClassDefs.Clear();
            _loader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            _classDef = MyBO.LoadClassDefWithLookup();
            OrderItem.LoadDefaultClassDef();

            TransactionCommitterStub committer = new TransactionCommitterStub();
			itsDatabaseConnectionMockControl = new DynamicMock(typeof (IDatabaseConnection));
			itsConnection = (IDatabaseConnection) itsDatabaseConnectionMockControl.MockInstance;
            _collection = new BusinessObjectCollection<MyBO>(_classDef);
            //itsBo1 = _classDef.CreateNewBusinessObject(itsConnection);
            itsBo1 = _classDef.CreateNewBusinessObject();
            itsBo1.SetPropertyValue("TestProp", "bo1prop1");
            itsBo1.SetPropertyValue("TestProp2", "s1");
            _collection.Add(itsBo1);
            //itsBo2 = _classDef.CreateNewBusinessObject(itsConnection);
            itsBo2 = _classDef.CreateNewBusinessObject();
            itsBo2.SetPropertyValue("TestProp", "bo2prop1");
            itsBo2.SetPropertyValue("TestProp2", "s2");
            _collection.Add(itsBo2);
        	SetupSaveExpectation();
            committer.AddBusinessObject(itsBo1);
            committer.CommitTransaction();
            //itsBo1.Save();
            //itsBo1.Save();
            _dataSetProvider = CreateDataSetProvider(_collection);
            
            BOMapper mapper = new BOMapper( _collection.ClassDef.CreateNewBusinessObject());
            itsTable = _dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);
            itsDatabaseConnectionMockControl.Verify();
        }

        [TearDown]
        public void TearDown()
        {
//            OrderItem.ClearTable();
        }

        protected virtual IDataSetProvider CreateDataSetProvider(IBusinessObjectCollection col)
        {
            _dataSetProvider = new ReadOnlyDataSetProvider(col);
            ((ReadOnlyDataSetProvider)_dataSetProvider).RegisterForBusinessObjectPropertyUpdatedEvents = true;
            return _dataSetProvider;
        }
		protected void SetupSaveExpectation()
		{
//            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
//                  DatabaseConnection.CurrentConnection.GetConnection());
//            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
//                  DatabaseConnection.CurrentConnection.GetConnection());
//            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
//                  DatabaseConnection.CurrentConnection.GetConnection());
//            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
//                  DatabaseConnection.CurrentConnection.GetConnection());
		}

        [Test]
        public void Test_Construct_WithNullCollection_RaisesError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                CreateDataSetProvider(null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("collection", ex.ParamName);
            }

        }
        [Test]
        public void Test_GetDataTable_WithNullCollection_RaisesError()
        {
            //---------------Set up test pack-------------------

            BusinessObjectCollection<MyBO> col = null;
            IDataSetProvider provider = GetDataSetProviderWithCollection(ref col);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                provider.GetDataTable(null);
                Assert.Fail("expected ArgumentNullException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("uiGrid", ex.ParamName);
            }

        }

        protected IDataSetProvider GetDataSetProviderWithCollection(ref BusinessObjectCollection<MyBO> col)
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithLookup();
            col = CreateCollectionWith_4_Objects();
            return CreateDataSetProvider(col);
        }

        [Test]
        public void Test_RegisterForBusinessObjectPropertyUpdatedEvents()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col = null;
            IDataSetProvider dataSetProvider = GetDataSetProviderWithCollection(ref col);
            //---------------Assert Precondition----------------
            Assert.IsTrue(dataSetProvider.RegisterForBusinessObjectPropertyUpdatedEvents);
            //---------------Execute Test ----------------------
            dataSetProvider.RegisterForBusinessObjectPropertyUpdatedEvents = false;  
            //---------------Test Result -----------------------
            Assert.IsFalse(dataSetProvider.RegisterForBusinessObjectPropertyUpdatedEvents);

        }
        [Test]
        public void TestCorrectNumberOfRows()
        {
            SetupTestData();
            Assert.AreEqual(2, itsTable.Rows.Count);
        }

        [Test]
        public void TestCorrectNumberOfColumns()
        {
            SetupTestData();
            Assert.AreEqual(3, itsTable.Columns.Count);
        }
        protected static BusinessObjectCollection<MyBO> CreateCollectionWith_4_Objects()
        {
            MyBO cp = new MyBO {TestProp = "b"};
            MyBO cp2 = new MyBO {TestProp = "d"};
            MyBO cp3 = new MyBO {TestProp = "c"};
            MyBO cp4 = new MyBO {TestProp = "a"};
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO> {{cp, cp2, cp3, cp4}};
            return col;
        }
        [Test]
        public void Test_UpdateBusinessObjectUpdatesRow()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col = null;
            IDataSetProvider dataSetProvider = GetDataSetProviderWithCollection(ref col);
            BOMapper mapper = new BOMapper( col.ClassDef.CreateNewBusinessObject());
            DataTable table = dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);
            MyBO bo1 = col[0];
            //---------------Assert Precondition----------------
            Assert.IsTrue(dataSetProvider.RegisterForBusinessObjectPropertyUpdatedEvents);
            //---------------Execute Test ----------------------
            bo1.SetPropertyValue("TestProp", "UpdatedValue");
            //---------------Test Result -----------------------
            Assert.AreEqual("UpdatedValue", table.Rows[0][1]);
        }
        [Test]
        public void Test_UpdateBusinessObject_DoesNotUpdatesRow_NotRegisteredForPropUpdated()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col = null;
            IDataSetProvider dataSetProvider = GetDataSetProviderWithCollection(ref col);
            dataSetProvider.RegisterForBusinessObjectPropertyUpdatedEvents = false;
            BOMapper mapper = new BOMapper(col.ClassDef.CreateNewBusinessObject());
            DataTable table = dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);
            MyBO bo1 = col[0];
            object origValue = bo1.GetPropertyValue("TestProp");
            //---------------Assert Precondition----------------
            Assert.IsFalse(dataSetProvider.RegisterForBusinessObjectPropertyUpdatedEvents);
            Assert.AreEqual(origValue, table.Rows[0][1]);
            //---------------Execute Test ----------------------
            bo1.SetPropertyValue("TestProp", "UpdatedValue");
            //---------------Test Result -----------------------
            Assert.AreNotEqual("UpdatedValue", table.Rows[0][1]);
            Assert.AreEqual(origValue, table.Rows[0][1]);
        }

        [Test]
        public void Test_UpdateBusinessObjectUpdatedDataTable()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col = null;
            IDataSetProvider dataSetProvider = GetDataSetProviderWithCollection(ref col);
            dataSetProvider.RegisterForBusinessObjectPropertyUpdatedEvents = false;
            BOMapper mapper = new BOMapper(col.ClassDef.CreateNewBusinessObject());
            DataTable table = dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);
            MyBO bo1 = col[0];
            object origValue = bo1.GetPropertyValue("TestProp");
            //---------------Assert Precondition----------------
            Assert.IsFalse(dataSetProvider.RegisterForBusinessObjectPropertyUpdatedEvents);
            Assert.AreEqual(origValue, table.Rows[0][1]);
            //---------------Execute Test ----------------------
            bo1.SetPropertyValue("TestProp", "UpdatedValue");
            bo1.Save();
            //---------------Test Result -----------------------
            Assert.AreNotEqual(origValue, table.Rows[0][1]);
            Assert.AreEqual("UpdatedValue", table.Rows[0][1]);
        }

        [Test]
        public void TestAddBusinessObjectAddsRow()
        {
            SetupTestData();
            //IBusinessObject bo3 = _classDef.CreateNewBusinessObject(itsConnection);
            IBusinessObject bo3 = _classDef.CreateNewBusinessObject();
            bo3.SetPropertyValue("TestProp", "bo3prop1");
            bo3.SetPropertyValue("TestProp2", "s1");
            //---------------Execute Test---------------------
            _collection.Add(bo3);
            //---------------Test Result-----------------------
            Assert.AreEqual(3, itsTable.Rows.Count);
            Assert.AreEqual("bo3prop1", itsTable.Rows[2][1]);
        }
        [Test]
        public void TestAddBusinessObjectAndUpdateUpdatesNewRow()
        {
            SetupTestData();
            //IBusinessObject bo3 = _classDef.CreateNewBusinessObject(itsConnection);
            IBusinessObject bo3 = _classDef.CreateNewBusinessObject();
            bo3.SetPropertyValue("TestProp", "bo3prop1");
            bo3.SetPropertyValue("TestProp2", "s2");
            const string updatedvalue = "UpdatedValue";
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, itsTable.Rows.Count);
            Assert.AreNotEqual(updatedvalue,bo3.GetPropertyValue("TestProp"));
            //---------------Execute Test ----------------------
            _collection.Add(bo3);
            bo3.SetPropertyValue("TestProp", updatedvalue);
            //---------------Test Result -----------------------
            Assert.AreEqual(updatedvalue, itsTable.Rows[2][1]);
            Assert.AreEqual(3, itsTable.Rows.Count);
        }
       [Test]
        public void TestAddBusinessObjectAndUpdateUpdatesAnotherRow()
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
            itsBo1.SetPropertyValue("TestProp", updatedvalue);
            //---------------Test Result -----------------------
           Assert.AreEqual(3, itsTable.Rows.Count);
           Assert.AreEqual(updatedvalue, itsTable.Rows[0][1]);
           Assert.AreNotEqual(origionalPropValue, itsTable.Rows[0][1]);
        }

        [Test]
        public void TestRemoveBusinessObjectRemovesRow()
        {
            SetupTestData();
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, itsTable.Rows.Count);
            //---------------Execute Test-----------
            _collection.Remove(itsBo1);
            //---------------Test Result -----------
            Assert.AreEqual(1, itsTable.Rows.Count);
        }

        [Test]
        public void TestOrderItemAddAndFindBO()
        {
            OrderItem car;
            OrderItem chair;
            BusinessObjectCollection<OrderItem> col = GetCollection(out car, out chair);
            Assert.AreEqual(car, col[0]);
            Assert.AreEqual(chair, col[1]);

            ReadOnlyDataSetProvider provider = new ReadOnlyDataSetProvider(col);
            IUIGrid uiGrid = ClassDef.ClassDefs[typeof(OrderItem)].UIDefCol["default"].UIGrid;
            Assert.AreEqual(2, provider.GetDataTable(uiGrid).Rows.Count);
            Assert.AreEqual(0, provider.FindRow(car));
            Assert.AreEqual(1, provider.FindRow(chair));
            Assert.AreEqual(car, provider.Find(0));
            Assert.AreEqual(chair, provider.Find(1));
            //Assert.AreEqual(car, provider.Find("OrderItem.OrderNumber=1;OrderItem.Product=car"));
            //Assert.AreEqual(chair, provider.Find("OrderItem.OrderNumber=2;OrderItem.Product=chair"));
            Assert.AreEqual(car, provider.Find(car.ID.ObjectID));
            Assert.AreEqual(chair, provider.Find(chair.ID.ObjectID));

            OrderItem roof = OrderItem.AddOrder3Roof();
            Assert.AreEqual(2, provider.GetDataTable(uiGrid).Rows.Count);
            Assert.AreEqual(-1, provider.FindRow(roof));
            bool causedException = false;
            try
            {
                provider.Find(2);
            }
            catch (Exception e)
            {
                causedException = true;
                Assert.AreEqual(typeof(IndexOutOfRangeException), e.GetType());
            }
            Assert.IsTrue(causedException);
            //Assert.IsNull(provider.Find("OrderItem.OrderNumber=3;OrderItem.Product=roof"));
            Assert.IsNull(provider.Find(roof.ID.ObjectID));

            col.Add(roof);
            Assert.AreEqual(3, provider.GetDataTable(uiGrid).Rows.Count);
            Assert.AreEqual(2, provider.FindRow(roof));
            Assert.AreEqual(roof, provider.Find(2));
            //Assert.AreEqual(roof, provider.Find("OrderItem.OrderNumber=3;OrderItem.Product=roof"));
            Assert.AreEqual(roof, provider.Find(roof.ID.ObjectID));
        }

        private BusinessObjectCollection<OrderItem> GetCollection(out OrderItem car, out OrderItem chair)
        {
            SetupTestData();
            OrderItem.ClearTable();
            car = OrderItem.AddOrder1Car();
            chair = OrderItem.AddOrder2Chair();
            BusinessObjectCollection<OrderItem> col = new BusinessObjectCollection<OrderItem>();
            col.LoadAll();
            col.Sort("OrderNumber", true, true);
            return col;
        }

        [Ignore("Peter: TODO 13 Feb 2009: This is hanging the build on the server")]
        [Test]
        public void TestOrderItemRemove()
        {
            SetupTestData();
            OrderItem.ClearTable();
            OrderItem.AddOrder1Car();
            OrderItem chair = OrderItem.AddOrder2Chair();
            BusinessObjectCollection<OrderItem> col = new BusinessObjectCollection<OrderItem>();
            col.LoadAll();
            col.Sort("OrderNumber", true, true);
            ReadOnlyDataSetProvider provider = new ReadOnlyDataSetProvider(col);
            IUIGrid uiGrid = ClassDef.ClassDefs[typeof(OrderItem)].UIDefCol["default"].UIGrid;
            DataTable table = provider.GetDataTable(uiGrid);
            Assert.AreEqual(2, table.Rows.Count);
            col.Remove(chair);
            Assert.AreEqual(1, table.Rows.Count);
            Assert.AreEqual(-1, provider.FindRow(chair));
            bool causedException = false;
            try
            {
                provider.Find(1);
            }
            catch (Exception e)
            {
                causedException = true;
                Assert.AreEqual(typeof(IndexOutOfRangeException), e.GetType());
            }
            Assert.IsTrue(causedException);
            //Assert.IsNull(provider.Find("OrderNumber=2 AND Product=chair"));
            Assert.IsNull(provider.Find(chair.ID.ObjectID));
        }

        [Test]
        public void Test_Find_UsingRow_ShouldReturnBusinessObject()
        {
            //--------------- Set up test pack ------------------
            OrderItem car;
            OrderItem chair;
            BusinessObjectCollection<OrderItem> col = GetCollection(out car, out chair);
            IDataSetProvider provider = new ReadOnlyDataSetProvider(col);
            IUIGrid uiGrid = ClassDef.ClassDefs[typeof(OrderItem)].UIDefCol["default"].UIGrid;
            DataTable dataTable = provider.GetDataTable(uiGrid);
            
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(2, col.Count, "Should b 2 BO's in col");
            Assert.AreEqual(2, dataTable.Rows.Count, "Should be 2 rows in datatable");
            Assert.AreSame(car, col[0]);
            Assert.AreSame(chair, col[1]);
            //--------------- Execute Test ----------------------
            DataRow row1 = dataTable.Rows[0];
            DataRow row2 = dataTable.Rows[1];
            IBusinessObject bo1 = provider.Find(row1);
            IBusinessObject bo2 = provider.Find(row2);
            //--------------- Test Result -----------------------
            Assert.AreSame(car, bo1);
            Assert.AreSame(chair, bo2);
        }

        [Test]
        public void TestOrderItemChangeItemAndFind()
        {
            SetupTestData();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            OrderItem.ClearTable();
            BusinessObjectCollection<OrderItem> col = new BusinessObjectCollection<OrderItem>();
            col.LoadAll();
            col.Sort("OrderNumber", true, true);
            Assert.AreEqual(0, col.Count);

            OrderItem car = OrderItem.AddOrder1Car();
            OrderItem.AddOrder2Chair();
            col = new BusinessObjectCollection<OrderItem>();
            col.LoadAll();
            col.Sort("OrderNumber", true, true);
            Assert.AreEqual(2, col.Count);
            ReadOnlyDataSetProvider provider = new ReadOnlyDataSetProvider(col);
            IUIGrid uiGrid = ClassDef.ClassDefs[typeof(OrderItem)].UIDefCol["default"].UIGrid;
            DataTable table = provider.GetDataTable(uiGrid);
            Assert.AreEqual(2, table.Rows.Count);

            car.OrderNumber = 11;
            Assert.AreEqual(0, provider.FindRow(car));
            Assert.AreEqual(car, provider.Find(0));
            //Assert.AreEqual(car, provider.Find("OrderItem.OrderNumber=11;OrderItem.Product=car"));
            Assert.AreEqual(car, provider.Find(car.ID.ObjectID));

            car.Save();
            Assert.AreEqual(0, provider.FindRow(car));
            Assert.AreEqual(car, provider.Find(0));
            //Assert.AreEqual(car, provider.Find("OrderItem.OrderNumber=11;OrderItem.Product=car"));
            Assert.AreEqual(car, provider.Find(car.ID.ObjectID));

            car.OrderNumber = 12;
            Assert.AreEqual(0, provider.FindRow(car));
            Assert.AreEqual(car, provider.Find(0));
            //Assert.AreEqual(car, provider.Find("OrderItem.OrderNumber=12;OrderItem.Product=car"));
            Assert.AreEqual(car, provider.Find(car.ID.ObjectID));

            car.OrderNumber = 13;
            Assert.AreEqual(0, provider.FindRow(car));
            Assert.AreEqual(car, provider.Find(0));
            //Assert.AreEqual(car, provider.Find("OrderItem.OrderNumber=13;OrderItem.Product=car"));
            Assert.AreEqual(car, provider.Find(car.ID.ObjectID));
        }

        [Test]
        public void TestCorrectColumnNames()
        {
            SetupTestData();
            Assert.AreEqual(_dataTableIdColumnName, itsTable.Columns[0].Caption);
            Assert.AreEqual(_dataTableIdColumnName, itsTable.Columns[0].ColumnName);

            Assert.AreEqual("Test Prop", itsTable.Columns[1].Caption);
            Assert.AreEqual("TestProp", itsTable.Columns[1].ColumnName);
            Assert.AreEqual("Test Prop 2", itsTable.Columns[2].Caption);
            Assert.AreEqual("TestProp2", itsTable.Columns[2].ColumnName);
        }

        [Test]
        public void TestCorrectRowValues()
        {
            SetupTestData();
            DataRow row1 = itsTable.Rows[0];
            DataRow row2 = itsTable.Rows[1];
            Assert.AreEqual("bo1prop1", row1["TestProp"]);
            Assert.AreEqual(itsBo1.ID.ToString(), row1[_dataTableIdColumnName]);
            Assert.AreEqual("s1", row1["TestProp2"]);
            Assert.AreEqual("bo2prop1", row2["TestProp"]);
            Assert.AreEqual("s2", row2["TestProp2"]);
        }

        [Test]
        public virtual void TestUpdateBusinessObjectRowValues()
        {
            //---------------Set up test pack-------------------
            SetupTestData();
            DataRow row1 = itsTable.Rows[0];
            MyBO myBO = (MyBO)_dataSetProvider.Find(0);
            string testPropValue = myBO.TestProp;
            row1["TestProp"] = "";
            //-------------Assert Preconditions -------------
            //Assert.AreEqual(row1[_dataTableIdColumnName], myBO.ID.ObjectID);
            Assert.AreEqual(row1[_dataTableIdColumnName], myBO.ID.ObjectID.ToString());
            Assert.AreEqual(testPropValue, myBO.TestProp);
            Assert.AreNotEqual(testPropValue, row1["TestProp"]);
            //---------------Execute Test ----------------------
            _dataSetProvider.UpdateBusinessObjectRowValues(myBO);
            //---------------Test Result -----------------------
            Assert.AreEqual(testPropValue, myBO.TestProp);
            Assert.AreEqual(testPropValue, row1["TestProp"]);
        }
        [Test]
        public virtual void TestAddBOToCol_WhenBOInvalid_UpdatesErrorProvider()
        {
            //---------------Set up test pack-------------------
            //XmlClassLoader loader = new XmlClassLoader();
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = (ClassDef) MyBO.LoadClassDefWithLookup();
            classDef.PropDefcol["TestProp"].Compulsory = true; 
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO>(_classDef);
            IDataSetProvider dataSetProvider = CreateDataSetProvider(collection);
            DataTable table = dataSetProvider.GetDataTable(classDef.GetUIDef("default").UIGrid);
            string errMessage;
            //-------------Assert Preconditions -------------
            Assert.AreEqual(0, table.Rows.Count);
            //---------------Execute Test ----------------------
            MyBO myBO = collection.CreateBusinessObject();
            myBO.Status.IsValid(out errMessage);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, table.Rows.Count);
            DataRow row1 = table.Rows[0];
            Assert.AreEqual(null, myBO.TestProp);
            Assert.AreEqual(DBNull.Value, row1["TestProp"]);
            Assert.AreEqual(errMessage, row1.RowError);
        }

        [Test]
        public virtual void TestUpdateBusinessObjectRowValues_WhenBOInvalid_UpdatesErrorProvider()
        {
            //---------------Set up test pack-------------------
            //XmlClassLoader loader = new XmlClassLoader();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithLookup();
            classDef.PropDefcol["TestProp"].Compulsory = true; 
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO>(_classDef);
            IDataSetProvider dataSetProvider = CreateDataSetProvider(collection);
            MyBO myBo = new MyBO();
            myBo.TestProp = TestUtil.GetRandomString();
            collection.Add(myBo);
            BOMapper mapper = new BOMapper(myBo);
            DataTable table = dataSetProvider.GetDataTable(mapper.GetUIDef().UIGrid);
            DataRow row1 = table.Rows[0];
            MyBO foundBO = (MyBO)dataSetProvider.Find(0);
            string testPropValueOrigValue = foundBO.TestProp;
            string errMessage;
            
            //-------------Assert Preconditions -------------
            Assert.AreEqual(testPropValueOrigValue, row1["TestProp"]);
            Assert.IsTrue(foundBO.Status.IsValid());
            Assert.AreEqual("", row1.RowError);
            //---------------Execute Test ----------------------
            myBo.TestProp = "";
            foundBO.Status.IsValid(out errMessage);
            dataSetProvider.UpdateBusinessObjectRowValues(foundBO);
            //---------------Test Result -----------------------
            Assert.AreNotEqual(testPropValueOrigValue, foundBO.TestProp);
            Assert.IsFalse(foundBO.Status.IsValid());
            Assert.AreEqual(errMessage,row1.RowError);
        }
        [Test]
        public virtual void TestUpdateBusinessObjectRowValues_BOPropNull()
        {
            //---------------Set up test pack-------------------
            SetupTestData();
            DataRow row1 = itsTable.Rows[0];
            MyBO myBO = (MyBO)_dataSetProvider.Find(0);
            string testPropValue = myBO.TestProp;
            _dataSetProvider.UpdateBusinessObjectRowValues(myBO);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(testPropValue, myBO.TestProp);
            Assert.AreEqual(testPropValue, row1["TestProp"]);
            //---------------Execute Test ----------------------
            myBO.TestProp = null;
            _dataSetProvider.UpdateBusinessObjectRowValues(myBO);
            //---------------Test Result -----------------------
            Assert.IsNull( myBO.TestProp);
            Assert.AreEqual(DBNull.Value, row1["TestProp"]);
        }

        [Test]
        public void TestUpdateBusinessObjectRowValues_NullBo()
        {
            //---------------Set up test pack-------------------
            SetupTestData();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            _dataSetProvider.UpdateBusinessObjectRowValues(null);
            //---------------Test Result -----------------------
            Assert.IsTrue(true, "Error should not be thrown and this assert should be reached.");
        }

        [Test]
        public void TestLookupListPopulated()
        {
            SetupTestData();
            Object prop = itsTable.Columns["TestProp2"].ExtendedProperties["LookupList"];
            Assert.AreSame(typeof (SimpleLookupList), prop.GetType());
        }

        [Test]
        public void TestDateTimeColumnType()
        {
            //-------------Setup Test Pack ------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDateTime();
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO bo = new MyBO();
            const string dateTimeProp = "TestDateTime";
            DateTime expectedDate = DateTime.Now;
            bo.SetPropertyValue(dateTimeProp, expectedDate);

            col.Add(bo);
            IDataSetProvider dataSetProvider = CreateDataSetProvider(col);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            DataTable dataTable = dataSetProvider.GetDataTable(bo.ClassDef.GetUIDef("default").UIGrid);
            //---------------Test Result -----------------------
            Assert.AreSame(typeof(DateTime), dataTable.Columns[dateTimeProp].DataType);
            Assert.IsInstanceOfType(typeof(DateTime), dataTable.Rows[0][dateTimeProp]);
            //---------------Tear Down -------------------------          
        }

//        [Test, Ignore("Not yet implemented critical to implement immediately")]
        [Test]
        public void Test_CustomDefined_Property()
        {
            //Assert.Fail("Not yet implemented");
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDateTime();
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDateTime();
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO bo = new MyBO();
            const string dateTimeProp = "TestDateTime";
            DateTime expectedDate = DateTime.Now;
            bo.SetPropertyValue(dateTimeProp, expectedDate);

            col.Add(bo);
            IDataSetProvider dataSetProvider = CreateDataSetProvider(col);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            DataTable dataTable = dataSetProvider.GetDataTable(bo.ClassDef.GetUIDef("default").UIGrid);
            //---------------Test Result -----------------------
            Assert.AreSame(typeof(DateTime), dataTable.Columns[dateTimeProp].DataType);
            Assert.IsInstanceOfType(typeof(DateTime), dataTable.Rows[0][dateTimeProp]);
            //---------------Tear Down -------------------------          
        }

        //		[Test]
        //		public void TestUpdateBusinessObjectUpdatesRow() {
        //			 itsBo1.SetPropertyValue("TestProp", "bo1prop1updated");
        //			Assert.AreEqual("bo1prop1updated", _table.Rows[0]["TestProp"]);
        //		}

        [Test]
        public void TestRelatedPropColumn()
        {
            //-------------Setup Test Pack ------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            new Address();//TO Load ClassDefs

            new Engine();//TO Load ClassDefs
            new Car();//TO Load ClassDefs
            IClassDef classDef = MyContactPerson.LoadClassDef();
            const string columnName = "Father.DateOfBirth";
            UIGrid uiGrid = CreateUiGridWithColumn(classDef, columnName);
            
            MyContactPerson myContactPerson = new MyContactPerson();
            MyContactPerson myFather = new MyContactPerson();
            myContactPerson.Father = myFather;
            const string fatherFirstName = "Father John";
            myFather.FirstName = fatherFirstName;
            DateTime fatherDOB = DateTime.Now;
            myFather.DateOfBirth = fatherDOB;
            BusinessObjectCollection<ContactPerson> contactPersons = new BusinessObjectCollection<ContactPerson>();
            contactPersons.Add(myContactPerson);
            IDataSetProvider dataSetProvider = CreateDataSetProvider(contactPersons);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            DataTable dataTable = dataSetProvider.GetDataTable(uiGrid);

            //---------------Test Result -----------------------
            Assert.IsTrue(dataTable.Columns.Contains(columnName), "DataTable should have the related property column");
            DataColumn dataColumn = dataTable.Columns[columnName];
            Assert.AreSame(typeof(DateTime), dataColumn.DataType);
            Assert.AreEqual(1, dataTable.Rows.Count);
            DataRow dataRow = dataTable.Rows[0];
            object value = dataRow[columnName];
            Assert.IsInstanceOfType(typeof(DateTime), value);
            Assert.AreEqual(myFather.DateOfBirth, value);
        }

        [Test]
        public void TestVirtualPropColumn()
        {
            //-------------Setup Test Pack ------------------
            ClassDef.ClassDefs.Clear();
            new Address();//TO Load ClassDefs
            new Engine();//TO Load ClassDefs
            new Car();//TO Load ClassDefs
//            DateTime startDate = DateTime.Now;
            IClassDef classDef = MyContactPerson.LoadClassDef();
            const string columnName = "-DateProperty-";
            UIGrid uiGrid = CreateUiGridWithColumn(classDef, columnName);
            BusinessObjectCollection<ContactPerson> contactPersons = new BusinessObjectCollection<ContactPerson>();
            MyContactPerson bo = new MyContactPerson();
            contactPersons.Add(bo);
            IDataSetProvider dataSetProvider = CreateDataSetProvider(contactPersons);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            DataTable dataTable = dataSetProvider.GetDataTable(uiGrid);
            //---------------Test Result -----------------------
            Assert.IsTrue(dataTable.Columns.Contains(columnName), "DataTable should have the virtual column");
            DataColumn dataColumn = dataTable.Columns[columnName];
            Assert.AreSame(typeof(object), dataColumn.DataType);
            Assert.AreEqual(1, dataTable.Rows.Count);
            DataRow dataRow = dataTable.Rows[0];
            object value = dataRow[columnName];
            Assert.IsInstanceOfType(typeof(DateTime), value);
            Assert.AreEqual(bo.DateProperty, value);
        }

        [Test]
        public void Test_GetDataTable_WhenMultipleLevelProp_ShouldLoadCorrectly()
        {
            //---------------Set up test pack-------------------
            RecordingExceptionNotifier recordingExceptionNotifier = new RecordingExceptionNotifier();
            GlobalRegistry.UIExceptionNotifier = recordingExceptionNotifier;
            ClassDef.ClassDefs.Clear();
            AddressTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.LoadClassDefWithOrganisationAndAddressRelationships();
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<AddressTestBO> addresses = contactPersonTestBO.Addresses;
            AddressTestBO address = AddressTestBO.CreateUnsavedAddress(contactPersonTestBO);

            UIGrid uiGrid = new UIGrid();
            const string propertyName = "ContactPersonTestBO.FirstName";
            uiGrid.Add(new UIGridColumn("Contact First Name", propertyName, typeof(DataGridViewTextBoxColumn), true, 100, PropAlignment.left, new Hashtable()));

            IDataSetProvider dataSetProvider = CreateDataSetProvider(addresses);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, addresses.Count);
            //---------------Execute Test ----------------------
            DataTable table = dataSetProvider.GetDataTable(uiGrid);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, table.Rows.Count);
            Assert.AreEqual(contactPersonTestBO.FirstName, table.Rows[0][propertyName]);
            recordingExceptionNotifier.RethrowRecordedException();
        }
        private static UIGrid CreateUiGridWithColumn(IClassDef classDef, string columnName)
        {
            UIGrid uiGrid = new UIGrid();
            UIGridColumn dateTimeUiGridColumn = new UIGridColumn(columnName, columnName, null, null, false, 100, PropAlignment.right, null);
            uiGrid.Add(dateTimeUiGridColumn);
            UIDef uiDef = new UIDef("TestUiDef", new UIForm(), uiGrid);
            classDef.UIDefCol.Add(uiDef);
            return uiGrid;
        }

        #region Internal Classes

        public class MyContactPerson : ContactPerson
        {
            private readonly DateTime _dateTime = DateTime.Now;
            private const string fatherRelationshipName = "Father";

            public MyContactPerson() : base((ClassDef) LoadClassDef())
            {
                
            }

            public DateTime DateProperty
            {
                get { return _dateTime; }
            }

            public ContactPerson Father
            {
//                get { return Relationships.GetRelatedObject<ContactPerson>(fatherRelationshipName); }
                set { Relationships.SetRelatedObject(fatherRelationshipName, value); }
            }

            public static IClassDef LoadClassDef()
            {
                IClassDef classDef = GetClassDef();
                if (!classDef.RelationshipDefCol.Contains(fatherRelationshipName))
                {
                    RelKeyDef relKeyDef = new RelKeyDef();
                    IPropDef idPropDef = classDef.GetPropDef("ContactPersonID");
                    relKeyDef.Add(new RelPropDef(idPropDef, "ContactPersonID"));
                    MySingleRelationshipDef singleRelationshipDef = new MySingleRelationshipDef(
                        fatherRelationshipName, typeof (ContactPerson), relKeyDef, true, DeleteParentAction.DoNothing);
                    classDef.RelationshipDefCol.Add(singleRelationshipDef);
                }
                return classDef;
            }

            private class MySingleRelationshipDef: SingleRelationshipDef
            {
                public MySingleRelationshipDef(string relationshipName, Type relatedObjectClassType, RelKeyDef relKeyDef, bool keepReferenceToRelatedObject, DeleteParentAction deleteParentAction) : base(relationshipName, relatedObjectClassType, relKeyDef, keepReferenceToRelatedObject, deleteParentAction)
                {
                }

                public override IRelationship CreateRelationship(IBusinessObject owningBo, IBOPropCol lBOPropCol)
                {
                    return new MySingleRelationship(owningBo, this, lBOPropCol);
                }
            }

            public class MySingleRelationship : SingleRelationship<MyContactPerson>, ISingleRelationship
            {
                private MyContactPerson _myContactPerson;

                public MySingleRelationship(IBusinessObject owningBo, RelationshipDef lRelDef, IBOPropCol lBOPropCol) : base(owningBo, lRelDef, lBOPropCol)
                {
                }

                /// <summary>
                /// Returns the related object 
                /// </summary>
                /// <returns>Returns the related business object</returns>
                public override MyContactPerson GetRelatedObject()
                {
                    return _myContactPerson;
                }
                /// <summary>
                /// Sets the related object to that provided
                /// </summary>
                /// <param name="relatedObject">The object to relate to</param>
                    void ISingleRelationship.SetRelatedObject(IBusinessObject relatedObject)
                {
                    _myContactPerson = relatedObject as MyContactPerson;
                }
            }
        }

        #endregion //Internal Classes


    }
}