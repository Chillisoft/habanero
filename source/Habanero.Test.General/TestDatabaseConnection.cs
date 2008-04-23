//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Data;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.General
{
    /// <summary>
    /// Summary description for TestDatabaseConnection.
    /// </summary>
    [TestFixture]
    public class TestDatabaseConnection : TestUsingDatabase
    {
        public TestDatabaseConnection()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        [TestFixtureSetUp]
        public void SetUpDBCon()
        {
            this.SetupDBConnection();
            //DatabaseConnection.CurrentConnection.ConnectionString =
            //@"data source=Core;database=WorkShopManagement;uid=sa;pwd=" + mPassWord;
        }


        [Test]
        public void TestDataReader()
        {
            //DatabaseConnection.CurrentConnection.ConnectionString =
            //	@"data source=Core;database=WorkShopManagement;uid=sa;pwd=;";

            string sql = "DELETE from TestTableRead";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);

            sql = "Insert into TestTableRead (TestTableReadData) VALUES ('aaa')";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);

            sql = "Insert into TestTableRead (TestTableReadData) VALUES ('abb')";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);


            using (
                IDataReader dr =
                    DatabaseConnection.CurrentConnection.LoadDataReader(
                        new SqlStatement(DatabaseConnection.CurrentConnection,
                                         "SELECT * FROM TestTableRead Order By TestTableReadData")))
            {
                try
                {
                    int i = 0;
                    while (dr.Read())
                    {
                        i++;
                        if (i == 1)
                        {
                            Assert.AreEqual("aaa", dr["TestTableReadData"]);
                        }
                        else
                        {
                            Assert.AreEqual("abb", dr["TestTableReadData"]);
                        }
                    }
                    Assert.AreEqual(2, i);
                }
                finally
                {
                    if (dr != null && !(dr.IsClosed))
                    {
                        dr.Close();
                    }
                }
            }
        }

        [Test]
        public void TestMultipleDataReaderperConnection()
        {
            //DatabaseConnection.CurrentConnection.ConnectionString =
            //	@"data source=Core;database=WorkShopManagement;uid=sa;pwd=;";
            IDataReader dr =
                DatabaseConnection.CurrentConnection.LoadDataReader(
                    new SqlStatement(DatabaseConnection.CurrentConnection,
                                     "SELECT * FROM TestTableRead Order By TestTableReadData"));
            IDataReader dr2 =
                DatabaseConnection.CurrentConnection.LoadDataReader(
                    new SqlStatement(DatabaseConnection.CurrentConnection,
                                     "SELECT * FROM TestTableRead Order By TestTableReadData"));
            Assert.IsNotNull(dr);
            Assert.IsNotNull(dr2);
            dr.Close();
            dr.Close();
        }

        [Test]
        public void TestExecuteSqlTransaction()
        {
            //DatabaseConnection.CurrentConnection.ConnectionString =
            //	@"data source=Core;database=WorkShopManagement;uid=sa;pwd=;";
            //Clean all data from table before starting

            Console.WriteLine("deleting from testtableread");
            string sql = "DELETE from TestTableRead";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);

            //Create transaction with Error
            Console.WriteLine("beginning transaction");
            IDbConnection connection = DatabaseConnection.CurrentConnection.GetConnection();
            connection.Open();
            IDbTransaction dbTransaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            //insert first record
            bool rolledBack = false;
            int statementsExecutedPriorToRollBack = 0;
            try
            {
                Console.WriteLine("doing first insert.");
                sql = "Insert into TestTableRead (TestTableReadData) VALUES ('Test')";
                DatabaseConnection.CurrentConnection.ExecuteRawSql(sql, dbTransaction);
                statementsExecutedPriorToRollBack++;
                //insert second record
                Console.WriteLine("doing second insert.");
                sql = "Insert into TestTableRead (TestTableReadData) VALUES ('Test')";
                DatabaseConnection.CurrentConnection.ExecuteRawSql(sql, dbTransaction);
                statementsExecutedPriorToRollBack++;
                Console.WriteLine("committing.");
                dbTransaction.Commit();
                connection.Close();
                statementsExecutedPriorToRollBack++;
            }
            catch
            {
                Console.WriteLine("error. rolling back. ");
                dbTransaction.Rollback();
                connection.Close();
                rolledBack = true;
            }
            Console.WriteLine("creating second datareader.");
            IDataReader dr2 =
                DatabaseConnection.CurrentConnection.LoadDataReader(
                    new SqlStatement(DatabaseConnection.CurrentConnection,
                                     "SELECT * FROM TestTableRead Order By TestTableReadData"));
            Assert.IsTrue(rolledBack);
            Console.WriteLine("reading from second datareader.");
            Assert.IsFalse(dr2.Read());

            Assert.IsTrue(statementsExecutedPriorToRollBack == 1);

            //Create transaction without Error
            connection = DatabaseConnection.CurrentConnection.GetConnection();
            connection.Open();
            dbTransaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            //insert first record
            try
            {
                sql = "Insert into TestTableRead (TestTableReadData) VALUES ('Test')";
                DatabaseConnection.CurrentConnection.ExecuteRawSql(sql, dbTransaction);
                //insert second record
                sql = "Insert into TestTableRead (TestTableReadData) VALUES ('Test2')";
                DatabaseConnection.CurrentConnection.ExecuteRawSql(sql, dbTransaction);

                dbTransaction.Commit();
                connection.Close();
            }
            catch
            {
                dbTransaction.Rollback();
                connection.Close();
            }
            dr2.Close();
            dr2 =
                DatabaseConnection.CurrentConnection.LoadDataReader(
                    new SqlStatement(DatabaseConnection.CurrentConnection,
                                     "SELECT * FROM TestTableRead Order By TestTableReadData"));
            Assert.IsTrue(dr2.Read());
            Assert.IsTrue(dr2.Read());
            Assert.IsFalse(dr2.Read());
            dr2.Close();
        }

        [Test]
        public void TestPrepareValueWithGuid()
        {
            Guid g = Guid.NewGuid();
            string strg = g.ToString("B").ToUpper();
            Assert.AreEqual(strg, DatabaseUtil.PrepareValue(g), "PrepareValue is not preparing guids correctly.");
        }
    }
}