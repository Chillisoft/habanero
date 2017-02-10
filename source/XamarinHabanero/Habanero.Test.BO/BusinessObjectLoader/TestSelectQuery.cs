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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Habanero.Test.BO.BusinessObjectLoader
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
            var selectQuery = new SelectQuery();
            //---------------Test Result -----------------------
            Assert.IsNull(selectQuery.Criteria);
            Assert.AreEqual(0, selectQuery.Fields.Count);
            Assert.AreEqual(0, selectQuery.OrderCriteria.Fields.Count);
            Assert.IsNull(selectQuery.Source);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCriteria()
        {
            //---------------Set up test pack-------------------
            var selectQuery = new SelectQuery {Source = new Source("bob")};
            //---------------Execute Test ----------------------
            var criteria = new Criteria("test", Criteria.ComparisonOp.Equals, "testValue");
            selectQuery.Criteria = criteria;
            //---------------Test Result -----------------------
            Assert.AreSame(criteria, selectQuery.Criteria);
            //---------------Tear Down -------------------------
        }

        
        [Test]
        public void TestSetCriteria_AddsJoins()
        {
            //---------------Set up test pack-------------------
            var selectQuery = new SelectQuery();
            const string sourceName = "mysource";
            selectQuery.Source = new Source(sourceName);
            var fieldSource = new Source(sourceName);
            var expectedSourceName = TestUtil.GetRandomString();
            fieldSource.JoinToSource(new Source(expectedSourceName));
            var field = new QueryField("testfield", "testfield", fieldSource);
            var criteria = new Criteria(field, Criteria.ComparisonOp.Equals, "value");

            //---------------Execute Test ----------------------
            selectQuery.Criteria = criteria;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, selectQuery.Source.Joins.Count);
            Assert.AreEqual(selectQuery.Source, selectQuery.Source.Joins[0].FromSource);
            Assert.AreEqual(expectedSourceName, selectQuery.Source.Joins[0].ToSource.Name);
        }

        [Test]
        public void TestSetCompoundCriteria_AddsJoins()
        {
            //---------------Set up test pack-------------------
            var selectQuery = new SelectQuery();
            const string sourceName = "mysource";
            selectQuery.Source = new Source(sourceName);
            var field1Source = new Source(sourceName);
            var field2Source = new Source(sourceName); 
            var expectedSourceName1 = TestUtil.GetRandomString();
            var expectedSourceName2 = TestUtil.GetRandomString();
            field1Source.JoinToSource(new Source(expectedSourceName1));
            field2Source.JoinToSource(new Source(expectedSourceName2));
            var field1 = new QueryField("testfield", "testfield", field1Source);
            var field2 = new QueryField("testfield", "testfield", field2Source);
            var criteria1 = new Criteria(field1, Criteria.ComparisonOp.Equals, "value");
            var criteria2 = new Criteria(field2, Criteria.ComparisonOp.Equals, "value");
            var criteria = new Criteria(criteria1, Criteria.LogicalOp.And, criteria2);
            //---------------Execute Test ----------------------
            selectQuery.Criteria = criteria;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, selectQuery.Source.Joins.Count);
            Assert.AreEqual(selectQuery.Source, selectQuery.Source.Joins[0].FromSource);
            Assert.AreEqual(expectedSourceName1, selectQuery.Source.Joins[0].ToSource.Name);
            Assert.AreEqual(selectQuery.Source, selectQuery.Source.Joins[1].FromSource);
            Assert.AreEqual(expectedSourceName2, selectQuery.Source.Joins[1].ToSource.Name);
        }

        [Test]
        public void Test_SetOrderCriteriaToNull_NoError()
        {
            //---------------Set up test pack-------------------
            var selectQuery = new SelectQuery
                            {
                                Source = new Source(TestUtil.GetRandomString()),
                                OrderCriteria = null
                            };
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsNull(selectQuery.OrderCriteria);

        }

        [Test]
        public void TestSetOrderCriteria()
        {
            //---------------Set up test pack-------------------
            var selectQuery = new SelectQuery {Source = new Source(TestUtil.GetRandomString())};
            //---------------Execute Test ----------------------
            var orderCriteria = new OrderCriteria().Add("testfield");
            selectQuery.OrderCriteria = orderCriteria;
            //---------------Test Result -----------------------
            Assert.AreSame(orderCriteria, selectQuery.OrderCriteria);
            Assert.AreEqual(0, selectQuery.Source.Joins.Count);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetOrderCriteria_AddsJoinToSource()
        {
            //---------------Set up test pack-------------------
            var selectQuery = new SelectQuery();
            const string sourceName = "mysource";
            selectQuery.Source = new Source(sourceName);
            var orderSource = new Source(sourceName);
            var expectedSourceName = TestUtil.GetRandomString();
            orderSource.JoinToSource(new Source(expectedSourceName));
            var orderOrderCriteriaField = new OrderCriteriaField("testfield", "testfield", orderSource, SortDirection.Ascending);
            var orderCriteria = new OrderCriteria().Add(orderOrderCriteriaField);

            //---------------Execute Test ----------------------
            selectQuery.OrderCriteria = orderCriteria;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, selectQuery.Source.Joins.Count);
            Assert.AreEqual(selectQuery.Source, selectQuery.Source.Joins[0].FromSource);
            Assert.AreEqual(expectedSourceName, selectQuery.Source.Joins[0].ToSource.Name);
        }

        [Test]
        public void TestOrderCriteriaWithNoSource()
        {
            try
            {
                //---------------Set up test pack-------------------
                var selectQuery = new SelectQuery();
                //---------------Execute Test ----------------------
                var orderCriteria = new OrderCriteria().Add("testfield");
                selectQuery.OrderCriteria = orderCriteria;
                //---------------Test Result -----------------------
                Assert.AreSame(orderCriteria, selectQuery.OrderCriteria);
                //---------------Tear Down -------------------------
                Assert.Fail("Expected to throw an HabaneroApplicationException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroApplicationException ex)
            {
                StringAssert.Contains("You cannot set an OrderCriteria for a SelectQuery if no Source has been set", ex.Message);
            }
        }

        [Test]
        public void TestSource()
        {
            //---------------Set up test pack-------------------
            var selectQuery = new SelectQuery();
            //---------------Execute Test ----------------------
            Source source = new Source("testsource");
            selectQuery.Source = source;
            //---------------Test Result -----------------------
            Assert.AreSame(source, selectQuery.Source);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestLimit()
        {
            //---------------Set up test pack-------------------
            var selectQuery = new SelectQuery();
            //---------------Execute Test ----------------------
            const int limit = 40;
            selectQuery.Limit = limit;
            //---------------Test Result -----------------------
            Assert.AreEqual(limit, selectQuery.Limit);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestFirstRecordToLoad()
        {
            //---------------Set up test pack-------------------
            var selectQuery = new SelectQuery();
            //---------------Execute Test ----------------------
            const int firstRecordToLoad = 40;
            selectQuery.FirstRecordToLoad = firstRecordToLoad;
            //---------------Test Result -----------------------
            Assert.AreEqual(firstRecordToLoad, selectQuery.FirstRecordToLoad);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestLimit_NotSet()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            var selectQuery = new SelectQuery();
            //---------------Test Result -----------------------
            Assert.AreEqual(-1, selectQuery.Limit);
            Assert.AreEqual(0, selectQuery.FirstRecordToLoad);
            //---------------Tear Down -------------------------
        }
    }
}