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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestSelectQueryDB 
    {
        private SqlFormatter _sqlFormatter;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            
        }
        [SetUp]
        public void SetupTest()
        {
            _sqlFormatter = new SqlFormatter("[", "]", "TOP", "");
            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub();
            ClassDef.ClassDefs.Clear();
            new Address();
        }

        public class DatabaseConnectionStub : DatabaseConnection
        {
            public DatabaseConnectionStub()
                : base("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection")
            {
                _sqlFormatter = new SqlFormatter("","","", "");
            }
        }
 
        [Test]
        public void Test_Set_FirstRecordToLoad()
        {
            //---------------Set up test pack-------------------
            ISelectQuery selectQuery = new SelectQueryDB(new SelectQuery());
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, selectQuery.FirstRecordToLoad);
            //---------------Execute Test ----------------------
            selectQuery.FirstRecordToLoad = 10;
            //---------------Test Result -----------------------
            Assert.AreEqual(10, selectQuery.FirstRecordToLoad);
        }

        [Test]
        public void Test_SelectQueryDB_Set_FirstRecordToLoad()
        {
            //---------------Set up test pack-------------------
            ISelectQuery selectQuery = new SelectQueryDB(new SelectQuery());
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, selectQuery.FirstRecordToLoad);
            //---------------Execute Test ----------------------
            selectQuery.FirstRecordToLoad = 10;
            //---------------Test Result -----------------------
            Assert.AreEqual(10, selectQuery.FirstRecordToLoad);
        }

        [Test]
        public void TestCreateSqlStatement_NoCriteria()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.AreEqualIgnoringCase("SELECT MyBO.MyBoID, MyBO.TestProp, MyBO.TestProp2 FROM MyBO", statementString);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestCreateSqlStatement_NoCriteria_WithClassID()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            classDef.ClassID = Guid.NewGuid();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.AreEqualIgnoringCase("SELECT MyBO.MyBoID, MyBO.TestProp, MyBO.TestProp2 FROM MyBO WHERE DMClassID = ?Param0", statementString);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestCreateSqlStatement_NoSourceNameInQueryFields()
        {
            //---------------Set up test pack-------------------
            ISelectQuery selectQuery = new SelectQuery();
            selectQuery.Fields.Add("Surname", new QueryField("Surname", "Surname", null));
            selectQuery.Fields.Add("ContactPersonID", new QueryField("ContactPersonID", "ContactPersonID", null));
            selectQuery.Source = new Source("bob");
            //---------------Execute Test ----------------------
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);

            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.AreEqualIgnoringCase("SELECT [Surname], [ContactPersonID] FROM [bob]", statementString);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCreateSqlStatement_WithCriteria()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, "test");
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef, criteria);
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("WHERE [MyBO].[TestProp] = ?Param0", statementString);
            Assert.AreEqual("?Param0", statement.Parameters[0].ParameterName);
            Assert.AreEqual("test", statement.Parameters[0].Value);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestCreateSqlStatement_WithCriteria_WithClassID()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            classDef.ClassID = Guid.NewGuid();
            Criteria criteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, "test");
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef, criteria);
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.Contains("WHERE ([MyBO].[TestProp] = ?Param0)", statementString);
            Assert.AreEqual("?Param0", statement.Parameters[0].ParameterName);
            Assert.AreEqual("test", statement.Parameters[0].Value);
            StringAssert.EndsWith("([DMClassID] = ?Param1)", statementString);
            Assert.AreEqual("?Param1", statement.Parameters[1].ParameterName);
            Assert.AreEqual(classDef.ClassID.Value.ToString("B").ToUpper(), statement.Parameters[1].Value);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestCreateSqlStatement_WithCriteria_DateTimeToday()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWithDateTime();
            Criteria criteria = new Criteria("TestDateTime", Criteria.ComparisonOp.Equals, new DateTimeToday());
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef, criteria);
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            DateTime dateTimeBefore = DateTime.Today;
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            DateTime dateTimeAfter = DateTime.Today;
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("WHERE [MyBO].[TestDateTime] = ?Param0", statementString);
            Assert.AreEqual("?Param0", statement.Parameters[0].ParameterName);
            object value = statement.Parameters[0].Value;
            Assert.IsInstanceOf(typeof(DateTime), value);
            DateTime dateTimeValue = (DateTime) value;
            Assert.GreaterOrEqual(dateTimeBefore, dateTimeValue);
            Assert.LessOrEqual(dateTimeAfter, dateTimeValue);
        }
        [Test]
        public void TestCreateSqlStatement_WithCriteria_DateTimeNow()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWithDateTime();
            Criteria criteria = new Criteria("TestDateTime", Criteria.ComparisonOp.Equals, new DateTimeNow());
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef, criteria);
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            DateTime dateTimeBefore = DateTime.Today;
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            DateTime dateTimeAfter = DateTime.Today.AddDays(1);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("WHERE [MyBO].[TestDateTime] = ?Param0", statementString);
            Assert.AreEqual("?Param0", statement.Parameters[0].ParameterName);
            object value = statement.Parameters[0].Value;
            Assert.IsInstanceOf(typeof(DateTime), value);
            DateTime dateTimeValue = (DateTime)value;
            Assert.GreaterOrEqual(dateTimeValue, dateTimeBefore);
            Assert.LessOrEqual(dateTimeValue, dateTimeAfter);
        }


        [Test]
        public void TestCreateSqlStatement_WithCriteria_TwoLevels()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            Criteria criteria = CriteriaParser.CreateCriteria("MyRelationship.MyRelatedTestProp = 'test'");
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef, criteria);
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            StringAssert.Contains(
                "[MyBO] LEFT JOIN [MyRelatedBo] ON [MyBO].[RelatedID] = [MyRelatedBo].[MyRelatedBoID]", 
                statement.Statement.ToString());
            //---------------Tear Down -------------------------
        }
            
        [Test]
        public void TestCreateSqlStatement_WithOrderFields()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.OrderCriteria = QueryBuilder.CreateOrderCriteria(classDef, "MyBoID, TestProp");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("ORDER BY [MyBO].[MyBoID] ASC, [MyBO].[TestProp] ASC", statementString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCreateSqlStatement_WithOrderFields_WithoutBuilder()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.OrderCriteria = OrderCriteria.FromString("MyBoID, TestProp");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("ORDER BY [MyBO].[MyBoID] ASC, [MyBO].[TestProp] ASC", statementString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCreateSqlStatement_WithOrderFields_WithoutBuilder_DifferentFieldNames()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDefWithDifferentTableAndFieldNames();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.OrderCriteria = OrderCriteria.FromString("MyBoID, TestProp");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("ORDER BY [my_bo].[my_bo_id] ASC, [my_bo].[test_prop] ASC", statementString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCreateSqlStatement_WithEmptyOrderCriteria()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.OrderCriteria = new OrderCriteria();
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("FROM [MyBO]", statementString, "An empty OrderCriteria should be ignored");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCreateSqlStatement_WithOrderFields_Descending()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.OrderCriteria = QueryBuilder.CreateOrderCriteria(classDef, "MyBoID DESC, TestProp DESC");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("ORDER BY [MyBO].[MyBoID] DESC, [MyBO].[TestProp] DESC", statementString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCreateSqlStatement_WithOrderFields_MixedOrder()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.OrderCriteria = QueryBuilder.CreateOrderCriteria(classDef, "MyBoID DESC, TestProp ASC");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("ORDER BY [MyBO].[MyBoID] DESC, [MyBO].[TestProp] ASC", statementString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCreateSqlStatement_WithOrder_ThroughRelationship()
        {
            //---------------Set up test pack-------------------
            new Engine();//TO Load ClassDefs
            new Car();//TO Load ClassDefs
            new ContactPerson();//TO Load ClassDefs
            ClassDef addressClassDef = new Address().ClassDef;
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(addressClassDef);
            selectQuery.OrderCriteria = QueryBuilder.CreateOrderCriteria(addressClassDef, "ContactPerson.Surname");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
          
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.Contains("JOIN [contact_person] ON [contact_person_address].[ContactPersonID] = [contact_person].[ContactPersonID]", statementString);
            StringAssert.EndsWith("ORDER BY [contact_person].[Surname_field] ASC", statementString);
        }

        [Test]
        public void TestCreateSqlStatement_WithOrder_ThroughRelationship_TwoLevels()
        {
            //---------------Set up test pack-------------------
            new Engine();//TO Load ClassDefs
            new Car();//TO Load ClassDefs
            new ContactPerson();//TO Load ClassDefs
            ClassDef engineClassDef = new Engine().ClassDef;

            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(engineClassDef);
            selectQuery.OrderCriteria = QueryBuilder.CreateOrderCriteria(engineClassDef, "Car.Owner.Surname");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            string expectedJoinSql = "LEFT JOIN [car_table] ON [Table_Engine].[CAR_ID] = [car_table].[CAR_ID]";
            expectedJoinSql += " LEFT JOIN [contact_person] ON [car_table].[OWNER_ID] = [contact_person].[ContactPersonID]";
            StringAssert.Contains(expectedJoinSql, statementString);
            StringAssert.EndsWith("ORDER BY [contact_person].[Surname_field] ASC", statementString);
        }

        [Test]
        public void TestCreateSqlStatement_WithOrder_ThroughRelationship_CompositeKey()
        {
            //---------------Set up test pack-------------------
            Car car = new Car();
            new ContactPersonCompositeKey();

            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(car.ClassDef);
            selectQuery.OrderCriteria = QueryBuilder.CreateOrderCriteria(car.ClassDef, "Driver.Surname");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.Contains("JOIN [ContactPersonCompositeKey] ON [car_table].[Driver_FK1] = [ContactPersonCompositeKey].[PK1_Prop1] AND [car_table].[Driver_FK2] = [ContactPersonCompositeKey].[PK1_Prop2] ", statementString);
            StringAssert.EndsWith("ORDER BY [ContactPersonCompositeKey].[Surname] ASC", statementString);
        }

        [Test]
        public void TestCreateSqlStatement_WithLimit_AtBeginning()
        {
            //---------------Set up test pack-------------------
//            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub_LimitClauseAtBeginning();
            SelectQuery selectQuery = new SelectQuery {Limit = 10};
            const string fieldName = "Field1";
            selectQuery.Fields.Add(fieldName, new QueryField(fieldName, fieldName, null));
            selectQuery.Source = new Source("Table1");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            SqlFormatter sqlFormatter = new SqlFormatter("", "", "TOP ROWS", "");
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(sqlFormatter);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.StartsWith("SELECT TOP ROWS 10 ", statementString);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCreateSqlStatement_WithNoLimit_AtBeginning()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub_LimitClauseAtBeginning();
            SelectQuery selectQuery = new SelectQuery {Limit = (-1)};
            const string fieldName = "Field1";
            selectQuery.Fields.Add(fieldName, new QueryField(fieldName, fieldName, null));
            selectQuery.Source = new Source("Table1");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.StartsWith("SELECT [Field1]", statementString);
            //---------------Tear Down -------------------------
        }

        public class DatabaseConnectionStub_LimitClauseAtBeginning : DatabaseConnectionStub
        {
            public DatabaseConnectionStub_LimitClauseAtBeginning()
            {
                _sqlFormatter = new SqlFormatter("`", "`", "TOP ROWS", "");
            }
        }

        [Test]
        public void TestCreateSqlStatement_WithLimit_AtEnd()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub_LimitClauseAtEnd();
            SelectQuery selectQuery = new SelectQuery {Limit = 10};
            const string fieldName = "Field1";
            selectQuery.Fields.Add(fieldName, new QueryField(fieldName, fieldName, null));
            selectQuery.Source = new Source("Table1");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            SqlFormatter sqlFormatter = new SqlFormatter("", "", "", "LIMIT");
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(sqlFormatter);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith(" LIMIT 10", statementString);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCreateSqlStatement_WithOrder_WithLimit_AtEnd()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub_LimitClauseAtEnd();
            SelectQuery selectQuery = new SelectQuery {Limit = 10};
            const string fieldName = "Field1";
            selectQuery.Fields.Add(fieldName, new QueryField(fieldName, fieldName, null));
            selectQuery.Source = new Source("Table1");
            selectQuery.OrderCriteria = OrderCriteria.FromString(fieldName);
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            SqlFormatter sqlFormatter = new SqlFormatter("", "", "", "LIMIT");
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(sqlFormatter);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith(" LIMIT 10", statementString);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCreateSqlStatement_WithNoLimit_AtEnd()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub_LimitClauseAtEnd();
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.Limit = -1;
            const string fieldName = "Field1";
            selectQuery.Fields.Add(fieldName, new QueryField(fieldName, fieldName, null));
            selectQuery.Source = new Source("Table1");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("FROM [Table1]", statementString);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestClassTableInheritance_Join()
        {
            //---------------Set up test pack-------------------
            ClassDef circleClassDef = Circle.GetClassDef();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(circleClassDef);
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, selectQuery.Source.InheritanceJoins.Count);
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            StringAssert.Contains("([circle_table] JOIN [Shape_table] ON [circle_table].[CircleID_field] = [Shape_table].[ShapeID_field])", statement.Statement.ToString());
        }

        [Test]
        public void TestClassTableInheritanceHierarchy_Join()
        {
            //---------------Set up test pack-------------------
            ClassDef filledCircleClassDef = FilledCircle.GetClassDefWithClassInheritanceHierarchy();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(filledCircleClassDef);
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            StringAssert.Contains("(([FilledCircle_table] JOIN [circle_table] ON [FilledCircle_table].[FilledCircleID_field] = [circle_table].[CircleID_field])" +
                                  " JOIN [Shape_table] ON [circle_table].[CircleID_field] = [Shape_table].[ShapeID_field])", statement.Statement.ToString());
        }

        [Test]
        public void TestClassTable_LoadSubtypeWithCriteriaFromBaseType()
        {
            //---------------Set up test pack-------------------
            Structure.Entity.LoadDefaultClassDef();
            ClassDef classDef = Structure.Part.LoadClassDef_WithClassTableInheritance();
            string entityType = TestUtil.GetRandomString();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            Criteria criteria = new Criteria("EntityType", Criteria.ComparisonOp.Equals, entityType);
            QueryBuilder.PrepareCriteria(classDef, criteria);
            selectQuery.Criteria = criteria;
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            StringAssert.Contains("[table_Entity].[field_Entity_Type] = ?Param0", statement.Statement.ToString());

            //---------------Tear Down -------------------------

        }


        [Test]
        public void TestSingleTableInheritance_DiscriminatorInWhere()
        {
            //---------------Set up test pack-------------------
            ClassDef circleClassDef = CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(circleClassDef);
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(_sqlFormatter);
            //---------------Test Result -----------------------
            StringAssert.Contains("[Shape_table].[ShapeType_field] = ?Param0", statement.Statement.ToString());
            Assert.AreEqual(1, statement.Parameters.Count);
            Assert.AreEqual("CircleNoPrimaryKey", statement.Parameters[0].Value);
            //---------------Tear Down -------------------------

        }

        [Test]
        public void TestCreateSqlStatement_NonPersistableProperty()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();

            PropDef newPropDef =
                new PropDef("NonPeristableProp", typeof (string), PropReadWriteRule.ReadOnly, null);
            newPropDef.Persistable = false;
            classDef.PropDefcol.Add(newPropDef);

            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.AreEqualIgnoringCase("SELECT MyBO.MyBoID, MyBO.TestProp, MyBO.TestProp2 FROM MyBO", statementString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_CreateSQL_LoadWithLimit_PaginatedFind()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub();
            ClassDef classDef = MyBO.LoadClassDefs_OneProp();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.FirstRecordToLoad = 2;
            selectQuery.Limit = 4;
            selectQuery.OrderCriteria = OrderCriteria.FromString("MyBOID");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            SqlFormatter sqlFormatter = new SqlFormatter("", "", "TOP", "");
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, selectQuery.FirstRecordToLoad);
            Assert.AreEqual(4, selectQuery.Limit);
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(sqlFormatter);
            string actualStatement = statement.Statement.ToString();
            //---------------Test Result -----------------------
            const string expectedFirstSelect = "(SELECT TOP 6 MyBO.MyBoID FROM MyBO ORDER BY MyBO.MyBOID ASC) As FirstSelect";
            StringAssert.Contains(expectedFirstSelect, actualStatement);
            string expectedSecondSelect = string.Format("(SELECT TOP 4 FirstSelect.MyBoID FROM {0} ORDER BY FirstSelect.MyBOID DESC ) AS SecondSelect", expectedFirstSelect);
            StringAssert.Contains(expectedSecondSelect, actualStatement);
            string expectedMainSelect = string.Format("SELECT SecondSelect.MyBoID FROM {0} ORDER BY SecondSelect.MyBOID ASC", expectedSecondSelect);
            Assert.AreEqual(expectedMainSelect, actualStatement);
        }

        [Test]
        public void Test_CreateSQL_LoadWithLimit_PaginatedFind_HasCorrectFieldNames()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub();
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.FirstRecordToLoad = 3;
            selectQuery.Limit = 5;
            selectQuery.OrderCriteria = OrderCriteria.FromString("Surname");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            SqlFormatter sqlFormatter = new SqlFormatter("", "", "TOP", "");
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, selectQuery.FirstRecordToLoad);
            Assert.AreEqual(5, selectQuery.Limit);
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(sqlFormatter);
            string actualStatement = statement.Statement.ToString();
            //---------------Test Result -----------------------
            const string expectedFirstSelect = "(SELECT TOP 8 contact_person.ContactPersonID, contact_person.Surname_field, contact_person.FirstName_field, contact_person.DateOfBirth FROM contact_person ORDER BY contact_person.Surname_field ASC) As FirstSelect";
            StringAssert.Contains(expectedFirstSelect, actualStatement);
            string expectedSecondSelect = string.Format("(SELECT TOP 5 FirstSelect.ContactPersonID, FirstSelect.Surname_field, FirstSelect.FirstName_field, FirstSelect.DateOfBirth FROM {0} ORDER BY FirstSelect.Surname_field DESC ) AS SecondSelect", expectedFirstSelect);
            StringAssert.Contains(expectedSecondSelect, actualStatement);
            string expectedMainSelect = string.Format("SELECT SecondSelect.ContactPersonID, SecondSelect.Surname_field, SecondSelect.FirstName_field, SecondSelect.DateOfBirth FROM {0} ORDER BY SecondSelect.Surname_field ASC", expectedSecondSelect);
            Assert.AreEqual(expectedMainSelect, actualStatement);
        }

        [Test]
        public void Test_CreateSQL_LoadWithLimit_PaginatedFind_HasCorrectFieldNames_WithDelimiters()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub();
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.FirstRecordToLoad = 3;
            selectQuery.Limit = 5;
            selectQuery.OrderCriteria = OrderCriteria.FromString("Surname");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            SqlFormatter sqlFormatter = new SqlFormatter("[", "]", "TOP", "");
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, selectQuery.FirstRecordToLoad);
            Assert.AreEqual(5, selectQuery.Limit);
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(sqlFormatter);
            string actualStatement = statement.Statement.ToString();
            //---------------Test Result -----------------------
            const string expectedFirstSelect = "(SELECT TOP 8 [contact_person].[ContactPersonID], [contact_person].[Surname_field], [contact_person].[FirstName_field], [contact_person].[DateOfBirth] FROM [contact_person] ORDER BY [contact_person].[Surname_field] ASC) As [FirstSelect]";
            StringAssert.Contains(expectedFirstSelect, actualStatement);
            string expectedSecondSelect = string.Format("(SELECT TOP 5 [FirstSelect].[ContactPersonID], [FirstSelect].[Surname_field], [FirstSelect].[FirstName_field], [FirstSelect].[DateOfBirth] FROM {0} ORDER BY [FirstSelect].[Surname_field] DESC ) AS [SecondSelect]", expectedFirstSelect);
            StringAssert.Contains(expectedSecondSelect, actualStatement);
            string expectedMainSelect = string.Format("SELECT [SecondSelect].[ContactPersonID], [SecondSelect].[Surname_field], [SecondSelect].[FirstName_field], [SecondSelect].[DateOfBirth] FROM {0} ORDER BY [SecondSelect].[Surname_field] ASC", expectedSecondSelect);
            Assert.AreEqual(expectedMainSelect, actualStatement);
        }

        [Test]
        public void Test_CreateSQL_LoadWithLimit_AtEnd_PaginatedFind()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub();
            ClassDef classDef = MyBO.LoadClassDefs_OneProp();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.FirstRecordToLoad = 2;
            selectQuery.Limit = 4;
            selectQuery.OrderCriteria = OrderCriteria.FromString("MyBOID");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            SqlFormatter sqlFormatter = new SqlFormatter("", "", "", "LIMIT");
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, selectQuery.FirstRecordToLoad);
            Assert.AreEqual(4, selectQuery.Limit);
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(sqlFormatter);
            string actualStatement = statement.Statement.ToString();
            //---------------Test Result -----------------------
            const string expectedFirstSelect = "(SELECT MyBO.MyBoID FROM MyBO ORDER BY MyBO.MyBOID ASC LIMIT 6) As FirstSelect";
            StringAssert.Contains(expectedFirstSelect, actualStatement);
            string expectedSecondSelect = string.Format("(SELECT FirstSelect.MyBoID FROM {0} ORDER BY FirstSelect.MyBOID DESC LIMIT 4) AS SecondSelect", expectedFirstSelect);
            StringAssert.Contains(expectedSecondSelect, actualStatement);
            string expectedMainSelect = string.Format("SELECT SecondSelect.MyBoID FROM {0} ORDER BY SecondSelect.MyBOID ASC", expectedSecondSelect);
            Assert.AreEqual(expectedMainSelect, actualStatement);
        }

        [Test]
        public void Test_CreateSQL_LoadWithLimit_AtEnd_PaginatedFind_HasCorrectFieldNames()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub();
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.FirstRecordToLoad = 3;
            selectQuery.Limit = 5;
            selectQuery.OrderCriteria = OrderCriteria.FromString("Surname");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            SqlFormatter sqlFormatter = new SqlFormatter("", "", "", "LIMIT");
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, selectQuery.FirstRecordToLoad);
            Assert.AreEqual(5, selectQuery.Limit);
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(sqlFormatter);
            string actualStatement = statement.Statement.ToString();
            //---------------Test Result -----------------------
            const string expectedFirstSelect = "(SELECT contact_person.ContactPersonID, contact_person.Surname_field, contact_person.FirstName_field, contact_person.DateOfBirth FROM contact_person ORDER BY contact_person.Surname_field ASC LIMIT 8) As FirstSelect";
            StringAssert.Contains(expectedFirstSelect, actualStatement);
            string expectedSecondSelect = string.Format("(SELECT FirstSelect.ContactPersonID, FirstSelect.Surname_field, FirstSelect.FirstName_field, FirstSelect.DateOfBirth FROM {0} ORDER BY FirstSelect.Surname_field DESC LIMIT 5) AS SecondSelect", expectedFirstSelect);
            StringAssert.Contains(expectedSecondSelect, actualStatement);
            string expectedMainSelect = string.Format("SELECT SecondSelect.ContactPersonID, SecondSelect.Surname_field, SecondSelect.FirstName_field, SecondSelect.DateOfBirth FROM {0} ORDER BY SecondSelect.Surname_field ASC", expectedSecondSelect);
            Assert.AreEqual(expectedMainSelect, actualStatement);
        }

        [Test]
        public void Test_CreateSQL_LoadWithLimit_AtEnd_PaginatedFind_HasCorrectFieldNames_WithDelimiters()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub();
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.FirstRecordToLoad = 3;
            selectQuery.Limit = 5;
            selectQuery.OrderCriteria = OrderCriteria.FromString("Surname");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            SqlFormatter sqlFormatter = new SqlFormatter("[", "]", "", "LIMIT");
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, selectQuery.FirstRecordToLoad);
            Assert.AreEqual(5, selectQuery.Limit);
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement(sqlFormatter);
            string actualStatement = statement.Statement.ToString();
            //---------------Test Result -----------------------
            const string expectedFirstSelect = "(SELECT [contact_person].[ContactPersonID], [contact_person].[Surname_field], [contact_person].[FirstName_field], [contact_person].[DateOfBirth] FROM [contact_person] ORDER BY [contact_person].[Surname_field] ASC LIMIT 8) As [FirstSelect]";
            StringAssert.Contains(expectedFirstSelect, actualStatement);
            string expectedSecondSelect = string.Format("(SELECT [FirstSelect].[ContactPersonID], [FirstSelect].[Surname_field], [FirstSelect].[FirstName_field], [FirstSelect].[DateOfBirth] FROM {0} ORDER BY [FirstSelect].[Surname_field] DESC LIMIT 5) AS [SecondSelect]", expectedFirstSelect);
            StringAssert.Contains(expectedSecondSelect, actualStatement);
            string expectedMainSelect = string.Format("SELECT [SecondSelect].[ContactPersonID], [SecondSelect].[Surname_field], [SecondSelect].[FirstName_field], [SecondSelect].[DateOfBirth] FROM {0} ORDER BY [SecondSelect].[Surname_field] ASC", expectedSecondSelect);
            Assert.AreEqual(expectedMainSelect, actualStatement);
        }


        [Test]
        public void Test_CreateSQL_SelectCountQuery()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub();
            ClassDef classDef = MyBO.LoadClassDefs_OneProp();
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            ISelectQuery selectQuery = QueryBuilder.CreateSelectCountQuery(classDef);
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            Assert.AreEqual("SELECT Count(*) FROM MyBO", statement.Statement.ToString());
        }

        [Test]
        public void Test_CreateSQL_SelectCountQuery_WithCriteria()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub();
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, "test");
            SqlFormatter sqlFormatter = new SqlFormatter("[", "]", "", "LIMIT");
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            ISelectQuery selectQuery = QueryBuilder.CreateSelectCountQuery(classDef, criteria);
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            ISqlStatement statement = query.CreateSqlStatement(sqlFormatter);
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.Contains("SELECT [Count(*)] FROM [MyBO] WHERE ", statement.Statement.ToString());
            StringAssert.EndsWith("WHERE [MyBO].[TestProp] = ?Param0", statementString);
            Assert.AreEqual("?Param0", statement.Parameters[0].ParameterName);
            Assert.AreEqual("test", statement.Parameters[0].Value);

        }
 
        public class DatabaseConnectionStub_LimitClauseAtEnd : DatabaseConnectionStub
        {
#pragma warning disable 672 //Tests on backward compatibility are being maintained.
            public override string GetLimitClauseForEnd(int limit)

            {
                return "LIMIT " + limit;
            }
#pragma warning restore 672
        }
    }
}