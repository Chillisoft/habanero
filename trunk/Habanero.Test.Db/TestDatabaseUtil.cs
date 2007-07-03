using System;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.Db
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