using System;
using Chillisoft.Db.v2;
using NUnit.Framework;

namespace Chillisoft.Test.Db.v2
{
    /// <summary>
    /// Summary description for TestDatabaseUtil.
    /// </summary>
    [TestFixture]
    public class TestDatabaseUtil
    {
        public TestDatabaseUtil()
        {
        }

        [Test]
        public void TestFormatDatabaseDateTime()
        {
            DateTime dte = new DateTime(2004, 12, 15, 5, 10, 20);
            Assert.AreEqual("2004-12-15 05:10:20", DatabaseUtil.FormatDatabaseDateTime(dte));
        }
    }
}