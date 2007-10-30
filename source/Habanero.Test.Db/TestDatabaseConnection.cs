using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Habanero.Base;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestDatabaseConnection : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            this.SetupDBConnection();
        }
        [Test]
        public void TestAutoIncrementAfterInsert()
        {

            // MockRepository mockRepository = new MockRepository();
            MockSupportsAutoIncrementingID mockSupportsAutoIncrementingID = new MockSupportsAutoIncrementingID();

            InsertSqlStatement sql = new InsertSqlStatement(DatabaseConnection.CurrentConnection.GetConnection(), "insert into testautoinc (testfield) values ('testing')");
            sql.TableName = "testautoinc";
            sql.SupportsAutoIncrementingField = mockSupportsAutoIncrementingID;

            DatabaseConnection.CurrentConnection.ExecuteSql(sql);

            int maxNum = 0;
            using (IDataReader reader = DatabaseConnection.CurrentConnection.LoadDataReader("select max(testautoincid) from testautoinc"))
            {
                while (reader.Read())
                {
                    maxNum = reader.GetInt32(0);
                }
            }
            Assert.AreEqual(maxNum, mockSupportsAutoIncrementingID.AutoValue);
        }

        private class MockSupportsAutoIncrementingID : ISupportsAutoIncrementingField
        {
            private long _autoValue = 0;
            public void SetAutoIncrementingFieldValue(long value)
            {
                _autoValue = value;
            }

            public long AutoValue
            {
                get { return _autoValue; }
            }
        }

    }
}
