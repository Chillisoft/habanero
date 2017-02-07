#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Util;
using NSubstitute;
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
        readonly Guid _guid1 = Guid.NewGuid();
        readonly Guid _guid2 = Guid.NewGuid();
        readonly Guid _guid3 = Guid.NewGuid();
        DataTable dt;
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
            row[0] = _guid1;
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[1] = "Test2";
            row[0] = _guid2;
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[1] = "Test3";
            row[0] = _guid3;
            dt.Rows.Add(row);
        }

        [SetUp]
        public void SetupTest()
        {
            conn = Substitute.For<IDatabaseConnection>();
            statement = new SqlStatement(DatabaseConnection.CurrentConnection);
            statement.Statement.Append(Sql);
            conn.LoadDataTable(statement, "", "").Returns(dt);
            conn.GetConnection().Returns(DatabaseConnection.CurrentConnection.GetConnection());
        }

        [Test]
        public void Test_NoPropDefSet_ThrowsError()
        {
            //---------------Set up test pack-------------------
            DatabaseLookupList source = new DatabaseLookupList(Sql);
            
            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            try
            {
                source.GetLookupList(conn);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (Exception ex)
            {
                StringAssert.Contains("There is an application setup error. There is no propdef set for the database lookup list.", ex.Message);
            }
            //---------------Test Result -----------------------

        }

        [Test]
        public void TestGetLookupList()
        {
            PropDef propDef = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            DatabaseLookupList source = new DatabaseLookupList(Sql) {PropDef = propDef};
            Dictionary<string, string> col = source.GetLookupList(conn);
            Assert.AreEqual(3, col.Count);
            string str = "";
            foreach (KeyValuePair<string, string> pair in col)
            {
                if (pair.Value != null && pair.Value.Equals(_guid1.ToString()))
                {
                    str = pair.Key;
                }
            }
            Assert.AreEqual("Test1", str);
        }

        private static string GuidToUpper(Guid guid)
        {
            return StringUtilities.GuidToUpper(guid);
        }


        [Test]
        public void TestCallingGetLookupListTwiceOnlyAccessesDbOnce()
        {
            PropDef propDef = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            DatabaseLookupList source = new DatabaseLookupList(Sql);
            source.PropDef = propDef;
            Dictionary<string, string> col = source.GetLookupList(conn);
            Dictionary<string, string> col2 = source.GetLookupList(conn);
            Assert.AreSame(col2, col);
        }


        [Test]
        public void Test_SetTimeOut_ShouldUpdateNewTimeOut()
        {
            //---------------Set up test pack-------------------
            DatabaseLookupList source = new DatabaseLookupList(Sql,10000,  null, null, false);
            const int expectedTimeout = 200000;

            //---------------Assert Precondition----------------
            Assert.AreEqual(10000, source.TimeOut);
            //---------------Execute Test ----------------------
            source.TimeOut = expectedTimeout;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedTimeout, source.TimeOut);
        }

        [Test]
        public void TestLookupListTimeout()
        {
            PropDef propDef = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            DatabaseLookupList source = new DatabaseLookupList(Sql, 100, null, null, false);
            source.PropDef = propDef;
            Dictionary<string, string> col = source.GetLookupList(conn);
            Thread.Sleep(250);
            Dictionary<string, string> col2 = source.GetLookupList(conn);
            Assert.AreNotSame(col2, col);
        }

        [Test]
        public void Test_LimitToList_Attribute_Default()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DatabaseLookupList source = new DatabaseLookupList("");
            //---------------Test Result -----------------------
            Assert.IsFalse(source.LimitToList);
        }

        [Test]
        public void Test_Constructor_WithLimitToList_AsTrue()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DatabaseLookupList source = new DatabaseLookupList("", 10000, null, null, true);
            //---------------Test Result -----------------------
            Assert.IsTrue(source.LimitToList);
        }

        [Test]
        public void Test_Constructor_WithLimitToList_AsFalse()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DatabaseLookupList source = new DatabaseLookupList("", 10000, null, null, false);

            //---------------Test Result -----------------------
            Assert.IsFalse(source.LimitToList);
        }

    }
}