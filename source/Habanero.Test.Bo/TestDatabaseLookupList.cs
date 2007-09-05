using System;
using System.Collections.Generic;
using System.Data;
using Habanero.BO;
using Habanero.DB;
using Habanero.Base;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for DatabaseLookupList.
    /// </summary>
    [TestFixture]
    public class TestDatabaseLookupList : TestUsingDatabase
    {
        private readonly string Sql = "select MyBoID, TestProp from tbMyBo";
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
            row[1] = "Test1";
            row[0] = g1;
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[1] = "Test2";
            row[0] = g2;
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[1] = "Test3";
            row[0] = g3;
            dt.Rows.Add(row);
        }

        [SetUp]
        public void SetupTest()
        {
            dbConnMock = new DynamicMock(typeof (IDatabaseConnection));
            conn = (IDatabaseConnection) dbConnMock.MockInstance;
            statement = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            statement.Statement.Append(Sql);
            dbConnMock.ExpectAndReturn("LoadDataTable", dt, new object[] {statement, "", ""});
            dbConnMock.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection(),
                                       new object[] {});
        }

        [Test]
        public void TestGetLookupList()
        {
            DatabaseLookupList source = new DatabaseLookupList(Sql);
            Dictionary<string, object> col = source.GetLookupList(conn);
            Assert.AreEqual(3, col.Count);
            string str = "";
            foreach (KeyValuePair<string, object> pair in col)
            {
                if (pair.Value != null && pair.Value.Equals(g1))
                {
                    str = pair.Key;
                }
            }
            Assert.AreEqual("Test1", str);
            dbConnMock.Verify();
        }


        [Test]
        public void TestCallingGetLookupListTwiceOnlyAccessesDbOnce()
        {
            DatabaseLookupList source = new DatabaseLookupList(Sql);
            Dictionary<string, object> col = source.GetLookupList(conn);
            Dictionary<string, object> col2 = source.GetLookupList(conn);
            Assert.AreSame(col2, col);
            dbConnMock.Verify();
        }

        [Test]
        public void TestLookupListTimeout()
        {
            dbConnMock.ExpectAndReturn("LoadDataTable", dt, new object[] {statement, "", ""});
            dbConnMock.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection(),
                                       new object[] {});

            DatabaseLookupList source = new DatabaseLookupList(Sql, 100);
            Dictionary<string, object> col = source.GetLookupList(conn);
            System.Threading.Thread.Sleep(250);
            Dictionary<string, object> col2 = source.GetLookupList(conn);
            Assert.AreNotSame(col2, col);
            dbConnMock.Verify();
        }
    }
}