using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestSelectQueryDB : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.SetupDBConnection();
        }
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }



        [Test]
        public void TestCreateSqlStatement_NoCriteria()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            SelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.AreEqualIgnoringCase("SELECT `MyBO`.`MyBoID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` FROM `MyBO`", statementString);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestCreateSqlStatement_NoSourceNameInQueryFields()
        {
            //---------------Set up test pack-------------------
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.Fields.Add("Surname", new QueryField("Surname", "Surname", ""));
            selectQuery.Fields.Add("ContactPersonID", new QueryField("ContactPersonID", "ContactPersonID", ""));
            selectQuery.Source = "bob";
            //---------------Execute Test ----------------------
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            ISqlStatement statement = query.CreateSqlStatement();

            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.AreEqualIgnoringCase("SELECT `Surname`, `ContactPersonID` FROM `bob`", statementString);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCreateSqlStatement_WithCriteria()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("TestProp", Criteria.Op.Equals, "test");
            SelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef, criteria);
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("WHERE `MyBO`.`TestProp` = 'test'", statementString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCreateSqlStatement_WithOrderFields()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            SelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.OrderCriteria = new OrderCriteria().Add("MyBoID").Add("TestProp");
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("ORDER BY `MyBO`.`MyBoID` ASC, `MyBO`.`TestProp` ASC", statementString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCreateSqlStatement_WithOrderFields_Descending()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            SelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.OrderCriteria = new OrderCriteria().Add("MyBoID", OrderCriteria.SortDirection.Descending);
            selectQuery.OrderCriteria.Add("TestProp", OrderCriteria.SortDirection.Descending);
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("ORDER BY `MyBO`.`MyBoID` DESC, `MyBO`.`TestProp` DESC", statementString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCreateSqlStatement_WithOrderFields_MixedOrder()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadDefaultClassDef();
            SelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.OrderCriteria = new OrderCriteria();
            selectQuery.OrderCriteria.Add("MyBoID", OrderCriteria.SortDirection.Descending);
            selectQuery.OrderCriteria.Add("TestProp", OrderCriteria.SortDirection.Ascending);
            SelectQueryDB query = new SelectQueryDB(selectQuery);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("ORDER BY `MyBO`.`MyBoID` DESC, `MyBO`.`TestProp` ASC", statementString);
            //---------------Tear Down -------------------------          
        }


    }
}
