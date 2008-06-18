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
        //TODO: make the fields and order fields case insensitive
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
        public void TestOrderCriteria()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            SelectQuery<MyBO> query = new SelectQuery<MyBO>();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            query.OrderCriteria = new OrderCriteria("TestProp");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, query.OrderCriteria.Count);
            Assert.IsTrue(query.OrderCriteria.Fields.Contains("TestProp"));
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestOrderCriteria_Multiple()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            SelectQuery<MyBO> query = new SelectQuery<MyBO>();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            query.OrderCriteria = new OrderCriteria("TestProp");
            query.OrderCriteria.Add("TestProp2");
            //---------------Test Result -----------------------
            Assert.AreEqual(2, query.OrderCriteria.Count);
            Assert.IsTrue(query.OrderCriteria.Fields.Contains("TestProp"));
            Assert.IsTrue(query.OrderCriteria.Fields.Contains("TestProp2"));
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestOrderCriteria_Compare()
        {
            //---------------Set up test pack-------------------
            OrderCriteria orderCriteria = new OrderCriteria("TestProp");
            orderCriteria.Add("TestProp2");
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------

            //---------------Tear Down -------------------------
        }


    }
}
