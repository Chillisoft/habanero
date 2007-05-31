using System;
using System.Data;
using Chillisoft.Db.v2;
using Chillisoft.Generic.v2;
using NMock;
using NUnit.Framework;

namespace Chillisoft.Test.Generic.v2
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    [TestFixture]
    public class TestStringGuidPairCollection : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.SetupDBConnection();
        }

        [Test]
        public void TestLoad()
        {
            StringGuidPairCollection col = new StringGuidPairCollection();
            Mock dbConnMock = new DynamicMock(typeof (IDatabaseConnection));
            IDatabaseConnection conn = (IDatabaseConnection) dbConnMock.MockInstance;
            DataTable dt = new DataTable();
            dt.Columns.Add();
            dt.Columns.Add();
            DataRow row = dt.NewRow();
            row[0] = Guid.NewGuid();
            row[1] = "Test1";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = Guid.NewGuid();
            row[1] = "Test2";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = Guid.NewGuid();
            row[1] = "Test3";
            dt.Rows.Add(row);
            Assert.AreEqual(3, dt.Rows.Count);

            ISqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            statement.Statement.Append("select SampleLookupID, SampleLookup from tbsamplelookup");
            dbConnMock.ExpectAndReturn("LoadDataTable", dt, new object[] {statement, "", ""});
            col.Load(conn, statement);
            Assert.AreEqual(3, col.Count);
            dbConnMock.Verify();
        }

        [Test]
        public void TestSorting()
        {
            StringGuidPairCollection col = new StringGuidPairCollection();
            col.Add(new StringGuidPair("Hello", Guid.NewGuid()));
            col.Add(new StringGuidPair("Goodbye", Guid.NewGuid()));
            Assert.AreEqual("Goodbye", col[0].Str);
        }
    }
}