using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.Base
{
    [TestFixture]
    public class TestResultSet
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            var resultSet = new ResultSet();
            //---------------Test Result -----------------------
            Assert.IsNotNull(resultSet.Fields);
            Assert.IsNotNull(resultSet.Rows);
            Assert.AreEqual(0, resultSet.Fields.Count);
            Assert.AreEqual(0, resultSet.Rows.Count);
        }

        [Test]
        public void ConstructField()
        {
            //---------------Set up test pack-------------------
            string propName = TestUtil.GetRandomString();
            //---------------Execute Test ----------------------
            var field = new ResultSet.Field(propName);
            //---------------Test Result -----------------------
            Assert.AreEqual(propName, field.PropertyName);
        }

        [Test]
        public void AddResult_ShouldAddRowToRowsList()
        {
            //---------------Set up test pack-------------------
            var resultSet = new ResultSet();
            resultSet.Fields.Add(new ResultSet.Field("Name"));
            string name = TestUtil.GetRandomString();
            //---------------Execute Test ----------------------
            resultSet.AddResult(new object[] {name});
            //---------------Test Result -----------------------
            Assert.AreEqual(1, resultSet.Rows.Count);
            Assert.AreEqual(name, resultSet.Rows[0].RawValues[0]);
        }

        [Test]
        public void ConstructRow()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            var row = new ResultSet.Row();
            //---------------Test Result -----------------------
            Assert.IsNotNull(row.RawValues);
            Assert.IsNotNull(row.Values);
            Assert.AreEqual(0, row.RawValues.Count);
            Assert.AreEqual(0, row.Values.Count);

        }

        [Test]
        public void ConstructRow_WithRawValues()
        {
            //---------------Set up test pack-------------------
            object[] rawValues = new object[] {"somevalues", 34};
            //---------------Execute Test ----------------------
            var row = new ResultSet.Row(rawValues);
            //---------------Test Result -----------------------
            Assert.AreEqual(rawValues.Length, row.RawValues.Count);
            Assert.AreEqual(rawValues[0], row.RawValues[0]);
            Assert.AreEqual(rawValues[1], row.RawValues[1]);
        }
    }
}
