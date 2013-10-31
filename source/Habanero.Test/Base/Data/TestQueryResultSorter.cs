using System.Linq;
using Habanero.Base;
using Habanero.Base.Data;
using NUnit.Framework;

namespace Habanero.Test.Base.Data
{
    [TestFixture]
    public class TestQueryResultSorter
    {
        [Test]
        public void Sort_OneFieldAscending()
        {
            //---------------Set up test pack-------------------
            var resultSet = new QueryResult();
            resultSet.AddField("Name");
            const string lastValue = "zzzzz";
            const string firstValue = "aaaaa";
            resultSet.AddResult(new object[] { lastValue });
            resultSet.AddResult(new object[] { firstValue });
            var orderCriteria = new OrderCriteria();
            orderCriteria.Add("Name");
            var sorter = new QueryResultSorter();
            //---------------Execute Test ----------------------
            sorter.Sort(resultSet, orderCriteria);
            //---------------Test Result -----------------------
            var rows = resultSet.Rows.ToList();
            Assert.AreEqual(firstValue, rows[0].Values[0]);
            Assert.AreEqual(lastValue, rows[1].Values[0]);
        }

        [Test]
        public void Sort_TwoFieldsAscending()
        {
            //---------------Set up test pack-------------------
            var resultSet = new QueryResult();
            resultSet.AddField("Name");
            resultSet.AddField("Age");
            resultSet.AddResult(new object[] { "Bob", 21 });
            resultSet.AddResult(new object[] { "Bob", 19 });
            resultSet.AddResult(new object[] { "Peter", 40 });
            resultSet.AddResult(new object[] { "Peter", 30 });
            var orderCriteria = OrderCriteria.FromString("Name, Age");
            var sorter = new QueryResultSorter();
            //---------------Execute Test ----------------------
            sorter.Sort(resultSet, orderCriteria);
            //---------------Test Result -----------------------
            var rows = resultSet.Rows.ToList();
            Assert.AreEqual("Bob", rows[0].Values[0]);
            Assert.AreEqual(19, rows[0].Values[1]);
            Assert.AreEqual("Bob", rows[1].Values[0]);
            Assert.AreEqual(21, rows[1].Values[1]);
            Assert.AreEqual("Peter", rows[2].Values[0]);
            Assert.AreEqual(30, rows[2].Values[1]);
            Assert.AreEqual("Peter", rows[3].Values[0]);
            Assert.AreEqual(40, rows[3].Values[1]);
        }

        [Test]
        public void Sort_OneFieldDescending()
        {
            //---------------Set up test pack-------------------
            var resultSet = new QueryResult();
            resultSet.AddField("Name");
            resultSet.AddResult(new object[] { "aaaaa" });
            resultSet.AddResult(new object[] { "zzzzz" });
            var orderCriteria = OrderCriteria.FromString("Name DESC");
            var sorter = new QueryResultSorter();
            //---------------Execute Test ----------------------
            sorter.Sort(resultSet, orderCriteria);
            //---------------Test Result -----------------------
            var rows = resultSet.Rows.ToList();
            Assert.AreEqual("zzzzz", rows[0].Values[0]);
            Assert.AreEqual("aaaaa", rows[1].Values[0]);
        }
    }
}