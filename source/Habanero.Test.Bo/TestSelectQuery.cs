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

using Habanero.Base;
using Habanero.Base.Exceptions;
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
            Assert.AreEqual(0, selectQuery.OrderCriteria.Fields.Count);
            Assert.IsNull(selectQuery.Source);
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
        public void Test_SetOrderCriteriaToNull_NoError()
        {
            //---------------Set up test pack-------------------
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.Source = new Source(TestUtil.CreateRandomString());
            //---------------Execute Test ----------------------
            selectQuery.OrderCriteria = null;

            //---------------Test Result -----------------------
            Assert.IsNull(selectQuery.OrderCriteria);

        }

        [Test]
        public void TestSetOrderCriteria()
        {
            //---------------Set up test pack-------------------
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.Source = new Source(TestUtil.CreateRandomString());
            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = new OrderCriteria().Add("testfield");
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
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.Source = new Source(TestUtil.CreateRandomString());
            const string sourceName = "mysource";
            OrderCriteria.Field orderField = new OrderCriteria.Field("testfield", "testfield", new Source(sourceName), OrderCriteria.SortDirection.Ascending);
            OrderCriteria orderCriteria = new OrderCriteria().Add(orderField);

            //---------------Execute Test ----------------------
            selectQuery.OrderCriteria = orderCriteria;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, selectQuery.Source.Joins.Count);
            Assert.AreEqual(selectQuery.Source, selectQuery.Source.Joins[0].FromSource);
            Assert.AreEqual(sourceName, selectQuery.Source.Joins[0].ToSource.Name);
        }


        [Test, ExpectedException(typeof(HabaneroApplicationException), ExpectedMessage="You cannot set an OrderCriteria for a SelectQuery if no Source has been set")]
        public void TestOrderCriteriaWithNoSource()
        {
            //---------------Set up test pack-------------------
            SelectQuery selectQuery = new SelectQuery();
            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = new OrderCriteria().Add("testfield");
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
            SelectQuery selectQuery = new SelectQuery();
            //---------------Execute Test ----------------------
            const int limit = 40;
            selectQuery.Limit = limit;
            //---------------Test Result -----------------------
            Assert.AreEqual(limit, selectQuery.Limit);
            //---------------Tear Down -------------------------
        }
    }
}