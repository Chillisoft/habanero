//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

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

            InsertSqlStatement sql = new InsertSqlStatement(DatabaseConnection.CurrentConnection, "insert into testautoinc (testfield) values ('testing')");
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
