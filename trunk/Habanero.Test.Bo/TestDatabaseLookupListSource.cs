using System;
using System.Data;
using Chillisoft.Test;
using Habanero.Bo;
using Habanero.Db;
using Habanero.Generic;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.Bo
{
    /// <summary>
    /// Summary description for DatabaseLookupListSource.
    /// </summary>
    [TestFixture]
    public class TestDatabaseLookupListSource : TestUsingDatabase
    {
        Guid g1 = Guid.NewGuid();
        Guid g2 = Guid.NewGuid();
        Guid g3 = Guid.NewGuid();
        DataTable dt;
        Mock dbConnMock;
        IDatabaseConnection conn;
        ISqlStatement statement;

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            this.SetupDBConnection();

            dt = new DataTable();
            dt.Columns.Add();
            dt.Columns.Add();
            DataRow row = dt.NewRow();
            row[0] = g1;
            row[1] = "Test1";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = g2;
            row[1] = "Test2";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = g3;
            row[1] = "Test3";
            dt.Rows.Add(row);
        }

        [SetUp]
        public void SetupTest()
        {
            dbConnMock = new DynamicMock(typeof (IDatabaseConnection));
            conn = (IDatabaseConnection) dbConnMock.MockInstance;
            statement = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            statement.Statement.Append("select MyBoID, TestProp from tbMyBo");
            dbConnMock.ExpectAndReturn("LoadDataTable", dt, new object[] {statement, "", ""});
            dbConnMock.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection(),
                                       new object[] {});
        }

        [Test]
        public void TestGetLookupList()
        {
            DatabaseLookupListSource source = new DatabaseLookupListSource("select MyBoID, TestProp from tbMyBo");
            StringGuidPairCollection col = source.GetLookupList(conn);
            Assert.AreEqual(3, col.Count);
            Assert.AreEqual("Test1", col.FindByGuid(g1).Str);
            dbConnMock.Verify();
        }


        [Test]
        public void TestCallingGetLookupListTwiceOnlyAccessesDbOnce()
        {
            DatabaseLookupListSource source = new DatabaseLookupListSource("select MyBoID, TestProp from tbMyBo");
            StringGuidPairCollection col = source.GetLookupList(conn);
            StringGuidPairCollection col2 = source.GetLookupList(conn);
            Assert.AreSame(col2, col);
            dbConnMock.Verify();
        }

        [Test]
        public void TestLookupListTimeout()
        {
            dbConnMock.ExpectAndReturn("LoadDataTable", dt, new object[] {statement, "", ""});
            dbConnMock.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection(),
                                       new object[] {});

            DatabaseLookupListSource source = new DatabaseLookupListSource("select MyBoID, TestProp from tbMyBo", 100);
            StringGuidPairCollection col = source.GetLookupList(conn);
            System.Threading.Thread.Sleep(250);
            StringGuidPairCollection col2 = source.GetLookupList(conn);
            Assert.AreNotSame(col2, col);
            dbConnMock.Verify();
        }
    }
}