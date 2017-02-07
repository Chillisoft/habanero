using Habanero.Base.Data;
using NUnit.Framework;

namespace Habanero.Test.Base.Data
{
    [TestFixture]
    public class TestQueryResultRow
    {
        [Test]
        public void Construct()
        {
            //---------------Execute Test ----------------------
            var row = new QueryResultRow();
            //---------------Test Result -----------------------
            Assert.IsNotNull(row.RawValues);
            Assert.IsNotNull(row.Values);
            Assert.AreEqual(0, row.RawValues.Count);
            Assert.AreEqual(0, row.Values.Count);
        }

        [Test]
        public void Construct_WithRawValues()
        {
            //---------------Set up test pack-------------------
            var rawValues = new object[] { "somevalues", 34 };
            //---------------Execute Test ----------------------
            var row = new QueryResultRow(rawValues);
            //---------------Test Result -----------------------
            Assert.AreEqual(rawValues.Length, row.RawValues.Count);
            Assert.AreEqual(rawValues[0], row.RawValues[0]);
            Assert.AreEqual(rawValues[1], row.RawValues[1]);
        }
    }
}