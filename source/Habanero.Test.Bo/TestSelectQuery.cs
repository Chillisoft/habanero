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
    public class TestSelectQuery
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }
        [Test]
        public void TestConstruct()
        {
            //---------------Set up test pack-------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, DateTime.Now);
            //---------------Execute Test ----------------------
            SelectQuery<MyBO> query = new SelectQuery<MyBO>(criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, query.Criteria);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestFields()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            //---------------Execute Test ----------------------
            SelectQuery<MyBO> query = new SelectQuery<MyBO>();
            //---------------Test Result -----------------------
            Assert.AreEqual(3, query.Fields.Count);
            Assert.AreEqual("MyBoID", query.Fields["MyBoID"].PropertyName);
            Assert.AreEqual("MyBO.MyBoID", query.Fields["MyBoID"].FieldName);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSource()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            //---------------Execute Test ----------------------
            SelectQuery<MyBO> query = new SelectQuery<MyBO>();
            //---------------Test Result -----------------------
            Assert.AreEqual("MyBO", query.Source);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCreateSqlStatement_NoCriteria()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            SelectQueryDB<MyBO> query = new SelectQueryDB<MyBO>();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.AreEqualIgnoringCase("SELECT MyBO.MyBoID, MyBO.TestProp, MyBO.TestProp2 FROM MyBO", statementString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCreateSqlStatement_WithCriteria()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("TestProp", Criteria.Op.Equals, "test");
            SelectQueryDB<MyBO> query = new SelectQueryDB<MyBO>(criteria);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISqlStatement statement = query.CreateSqlStatement();
            //---------------Test Result -----------------------
            string statementString = statement.Statement.ToString();
            StringAssert.EndsWith("WHERE MyBO.TestProp = 'test'", statementString);
            //---------------Tear Down -------------------------          
        }
    }



    
}
