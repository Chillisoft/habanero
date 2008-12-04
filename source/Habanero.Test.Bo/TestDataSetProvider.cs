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
using Habanero.BO.Loaders;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>   
    //[TestFixture] 
    public abstract class TestDataSetProvider : TestUsingDatabase
    {
        protected XmlClassLoader _loader;
        protected ClassDef _classDef;
        protected IBusinessObjectCollection _collection;
        protected DataTable itsTable;
        protected IBusinessObject itsBo1;
        protected IBusinessObject itsBo2;
        protected IBusinessObject itsRelatedBo;
        protected IDataSetProvider _dataSetProvider;

        protected Mock itsDatabaseConnectionMockControl;
        protected IDatabaseConnection itsConnection;

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
            this.SetupDBConnection();
        }
        
        public virtual void SetupTestData()
        {
            
            ClassDef.ClassDefs.Clear();
            _loader = new XmlClassLoader();
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
            
            BOMapper mapper = new BOMapper((BusinessObject) _collection.SampleBo);
            itsTable = _dataSetProvider.GetDataTable(mapper.GetUIDef().GetUIGridProperties());
            itsDatabaseConnectionMockControl.Verify();
        }

        [TearDown]
        public void TearDown()
        {
//            OrderItem.ClearTable();
        }

        protected abstract IDataSetProvider CreateDataSetProvider(IBusinessObjectCollection col);

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

        [Test]
        public void TestCorrectColumnNames()
        {
            SetupTestData();
            Assert.AreEqual("ID", itsTable.Columns[0].Caption);
            Assert.AreEqual("ID", itsTable.Columns[0].ColumnName);

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
            Assert.AreEqual(itsBo1.ID.ToString(), row1["ID"]);
            Assert.AreEqual("s1", row1["TestProp2"]);
            Assert.AreEqual("bo2prop1", row2["TestProp"]);
            Assert.AreEqual("s2", row2["TestProp2"]);
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
            string dateTimeProp = "TestDateTime";
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
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyContactPerson.LoadClassDef();
            string columnName = "Father.DateOfBirth";
            UIGrid uiGrid = CreateUiGridWithColumn(classDef, columnName);
            
            MyContactPerson myContactPerson = new MyContactPerson();
            MyContactPerson myFather = new MyContactPerson();
            myContactPerson.Father = myFather;
            string fatherFirstName = "Father John";
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
//            DateTime startDate = DateTime.Now;
            ClassDef classDef = MyContactPerson.LoadClassDef();
            string columnName = "-DateProperty-";
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

        private static UIGrid CreateUiGridWithColumn(ClassDef classDef, string columnName)
        {
            UIGrid uiGrid = new UIGrid();
            UIGridColumn dateTimeUiGridColumn = new UIGridColumn(columnName, columnName, null, null, false, 100, UIGridColumn.PropAlignment.right, null);
            uiGrid.Add(dateTimeUiGridColumn);
            UIDef uiDef = new UIDef("TestUiDef", new UIForm(), uiGrid);
            classDef.UIDefCol.Add(uiDef);
            return uiGrid;
        }

        #region Internal Classes

        public class MyContactPerson : ContactPerson
        {
            private DateTime _dateTime = DateTime.Now;
            private static string fatherRelationshipName = "Father";

            public MyContactPerson() : base(LoadClassDef())
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

            public static ClassDef LoadClassDef()
            {
                ClassDef classDef = GetClassDef();
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

                public override Relationship CreateRelationship(IBusinessObject owningBo, BOPropCol lBOPropCol)
                {
                    return new MySingleRelationship(owningBo, this, lBOPropCol);
                }
            }

            public class MySingleRelationship : SingleRelationship
            {
                private MyContactPerson _myContactPerson;

                public MySingleRelationship(IBusinessObject owningBo, RelationshipDef lRelDef, BOPropCol lBOPropCol) : base(owningBo, lRelDef, lBOPropCol)
                {
                }


                /// <summary>
                /// Returns the related object 
                /// </summary>
                /// <returns>Returns the related business object</returns>
                public override T GetRelatedObject<T>() 
                {
                    return _myContactPerson as T;
                }

                /// <summary>
                /// Returns the related object 
                /// </summary>
                /// <returns>Returns the related business object</returns>
                public override IBusinessObject GetRelatedObject()
                {
                    return _myContactPerson;
                }
                /// <summary>
                /// Sets the related object to that provided
                /// </summary>
                /// <param name="relatedObject">The object to relate to</param>
                public override void SetRelatedObject(IBusinessObject relatedObject)
                {
                    _myContactPerson = relatedObject as MyContactPerson;
                }
            }
        }

        #endregion //Internal Classes


    }
}