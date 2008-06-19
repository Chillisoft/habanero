using System;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
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
            
            //---------------Execute Test ----------------------
            SelectQuery selectQuery = new SelectQuery();
            //---------------Test Result -----------------------
            Assert.IsNull(selectQuery.Criteria);
            Assert.AreEqual(0, selectQuery.Fields.Count);
            Assert.IsNull(selectQuery.OrderCriteria);
            Assert.IsTrue(String.IsNullOrEmpty(selectQuery.Source));
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCriteria()
        {
            //---------------Set up test pack-------------------
            SelectQuery selectQuery = new SelectQuery();
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("test", Criteria.Op.Equals, "testValue");
            selectQuery.Criteria = criteria;
            //---------------Test Result -----------------------
            Assert.AreSame(criteria, selectQuery.Criteria);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestOrderCriteria()
        {
            //---------------Set up test pack-------------------
            SelectQuery selectQuery = new SelectQuery();
            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = new OrderCriteria("testfield");
            selectQuery.OrderCriteria = orderCriteria;
            //---------------Test Result -----------------------
            Assert.AreSame(orderCriteria, selectQuery.OrderCriteria);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSource()
        {
            //---------------Set up test pack-------------------
            SelectQuery selectQuery = new SelectQuery();
            //---------------Execute Test ----------------------
            string source = "testsource";
            selectQuery.Source = source;
            //---------------Test Result -----------------------
            Assert.AreSame(source, selectQuery.Source);
            //---------------Tear Down -------------------------
        }
    }
}