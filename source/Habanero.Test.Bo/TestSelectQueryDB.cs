using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestSelectQueryDB 
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            
        }
        [SetUp]
        public void SetupTest()
        {
            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub();
            ClassDef.ClassDefs.Clear();
        }

        public class DatabaseConnectionStub : DatabaseConnection
        {
            public DatabaseConnectionStub()
                : base("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection")
            {
            }

            /// <summary>
            /// Returns a left square bracket
            /// </summary>
            public override string LeftFieldDelimiter
            {
                get { return "["; }
            }

            /// <summary>
            /// Returns a right square bracket
            /// </summary>
            public override string RightFieldDelimiter
            {
                get { return "]"; }
            }
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
            StringAssert.AreEqualIgnoringCase("SELECT [MyBO].[MyBoID], [MyBO].[TestProp], [MyBO].[TestProp2] FROM [MyBO]", statementString);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestCreateSqlStatement_NoSourceNameInQueryFields()
        {
            //---------------Set up test pack-------------------
            ISelectQuery selectQuery = new SelectQuery();
            selectQuery.Fields.Add("Surname", new QueryField("Surname", "Surname", ""));
            selectQuery.Fields.Add("ContactPersonID", new QueryField("ContactPersonID", "ContactPersonID", ""));
            selectQuery.Source = "bob";
            //---------------Execute Test ----------------------
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            ISqlStatement statement = query.CreateSqlStatement();

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
            Criteria criteria = new Criteria("TestProp", Criteria.Op.Equals, "test");
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef, criteria);
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("WHERE [MyBO].[TestProp] = ?Param0", statementString);
            Assert.AreEqual("?Param0", statement.Parameters[0].ParameterName);
            Assert.AreEqual( "test", statement.Parameters[0].Value);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestCreateSqlStatement_WithOrderFields()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.OrderCriteria = OrderCriteria.FromString("MyBoID, TestProp");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("ORDER BY [MyBO].[MyBoID] ASC, [MyBO].[TestProp] ASC", statementString);
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
            ISqlStatement statement = query.CreateSqlStatement();
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
            selectQuery.OrderCriteria = new OrderCriteria().Add("MyBoID", OrderCriteria.SortDirection.Descending);
            selectQuery.OrderCriteria.Add("TestProp", OrderCriteria.SortDirection.Descending);
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement();
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
            selectQuery.OrderCriteria = new OrderCriteria();
            selectQuery.OrderCriteria.Add("MyBoID", OrderCriteria.SortDirection.Descending);
            selectQuery.OrderCriteria.Add("TestProp", OrderCriteria.SortDirection.Ascending);
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("ORDER BY [MyBO].[MyBoID] DESC, [MyBO].[TestProp] ASC", statementString);
            //---------------Tear Down -------------------------          
        }

        //[Test]
        //public void TestCreateJoinStatement_WithRelationship()
        //{
        //    //---------------Set up test pack-------------------
        //    new ContactPerson();
        //    Address address = new Address();
        //    //---------------Execute Test ----------------------
        //    string joinStatement = SelectQueryDB.CreateJoinFromRelationship(address.Relationships["ContactPerson"]);
        //    //---------------Test Result -----------------------
        //    StringAssert.AreEqualIgnoringCase("JOIN [ContactPerson] ON [Address].[ContactPersonID] = [ContactPerson].[ContactPersonID]", joinStatement);

        //    //---------------Tear Down -------------------------
        //}

        [Test]
        public void TestCreateSqlStatement_WithOrder_IncludingSource()
        {
            //---------------Set up test pack-------------------
            new ContactPerson();
            
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(new Address().ClassDef);
            selectQuery.OrderCriteria = OrderCriteria.FromString("ContactPerson.Surname");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.Contains("JOIN [contact_person] ON [contact_person_address].[ContactPersonID] = [contact_person].[ContactPersonID]", statementString);
            StringAssert.EndsWith("ORDER BY [contact_person].[Surname] ASC", statementString);
        }

        [Test]
        public void TestCreateSqlStatement_WithOrder_IncludingSource_CompositeKey()
        {
            //---------------Set up test pack-------------------
            Car car = new Car();
            ContactPersonCompositeKey person = new ContactPersonCompositeKey();

            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(car.ClassDef);
            selectQuery.OrderCriteria = OrderCriteria.FromString("Driver.Surname");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.Contains("JOIN [ContactPersonCompositeKey] ON [Car].[Driver_FK1] = [ContactPersonCompositeKey].[PK1_Prop1] AND [Car].[Driver_FK2] = [ContactPersonCompositeKey].[PK1_Prop2] ", statementString);
            StringAssert.EndsWith("ORDER BY [ContactPersonCompositeKey].[Surname] ASC", statementString);
        }

        [Test]
        public void TestCreateSqlStatement_WithLimit_AtBeginning()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub_LimitClauseAtBeginning();
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.Limit = 10;
            string fieldName = "Field1";
            selectQuery.Fields.Add(fieldName, new QueryField(fieldName, fieldName, ""));
            selectQuery.Source = "Table1";
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.StartsWith("SELECT TOP 10 ", statementString);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCreateSqlStatement_WithNoLimit_AtBeginning()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub_LimitClauseAtBeginning();
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.Limit = 0;
            string fieldName = "Field1";
            selectQuery.Fields.Add(fieldName, new QueryField(fieldName, fieldName, ""));
            selectQuery.Source = "Table1";
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.StartsWith("SELECT [Field1]", statementString);
            //---------------Tear Down -------------------------
        }

        public class DatabaseConnectionStub_LimitClauseAtBeginning : DatabaseConnection
        {
            public DatabaseConnectionStub_LimitClauseAtBeginning()
                : base("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection")
            {
            }

            public override string GetLimitClauseForBeginning(int limit)
            {
                return "TOP " + limit;
            }
        }

        [Test]
        public void TestCreateSqlStatement_WithLimit_AtEnd()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection = new DatabaseConnectionStub_LimitClauseAtEnd();
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.Limit = 10;
            string fieldName = "Field1";
            selectQuery.Fields.Add(fieldName, new QueryField(fieldName, fieldName, ""));
            selectQuery.Source = "Table1";
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement();
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
            selectQuery.Limit = 0;
            string fieldName = "Field1";
            selectQuery.Fields.Add(fieldName, new QueryField(fieldName, fieldName, ""));
            selectQuery.Source = "Table1";
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement();
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

            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            StringAssert.Contains("JOIN [Shape] ON [Circle].[CircleID] = [Shape].[ShapeID]", statement.Statement.ToString());
        }

        public class DatabaseConnectionStub_LimitClauseAtEnd : DatabaseConnection
        {
            public DatabaseConnectionStub_LimitClauseAtEnd()
                : base("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection")
            {
            }

            public override string GetLimitClauseForEnd(int limit)
            {
                return "LIMIT " + limit;
            }
        }
    }
}
