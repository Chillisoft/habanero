//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using Habanero.Base;
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
        [TestFixtureSetUp]
        public void SetUpDBCon()
        {
            this.SetupDBConnection();
            //DatabaseConnection.CurrentConnection.ConnectionString =
            //@"data source=Core;database=WorkShopManagement;uid=sa;pwd=" + mPassWord;
        }

        [Test]
        public void Test_NoColumnName_DoesntError()
        {
            //---------------Set up test pack-------------------
            string sql = "Select FirstName + ', ' + Surname from contactpersoncompositekey";
            SqlStatement sqlStatement = new SqlStatement(DatabaseConnection.CurrentConnection,sql);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            DataTable dataTable = DatabaseConnection.CurrentConnection.LoadDataTable(sqlStatement, "", "");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, dataTable.Columns.Count);
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
            IDatabaseConnection databaseConnection = DatabaseConnection.CurrentConnection;
            databaseConnection.ExecuteRawSql(sql);

            //Create transaction with Error
            Console.WriteLine("beginning transaction");
            IDbConnection dbConnection = databaseConnection.GetConnection();
            dbConnection.Open();
            IDbTransaction dbTransaction = dbConnection.BeginTransaction(databaseConnection.IsolationLevel);
            //insert first record
            bool rolledBack = false;
            int statementsExecutedPriorToRollBack = 0;
            try
            {
                Console.WriteLine("doing first insert.");
                sql = "Insert into TestTableRead (TestTableReadData) VALUES ('Test')";
                databaseConnection.ExecuteRawSql(sql, dbTransaction);
                statementsExecutedPriorToRollBack++;
                //insert second record
                Console.WriteLine("doing second insert.");
                sql = "Insert into TestTableRead (TestTableReadData) VALUES ('Test')";
                databaseConnection.ExecuteRawSql(sql, dbTransaction);
                statementsExecutedPriorToRollBack++;
                Console.WriteLine("committing.");
                dbTransaction.Commit();
                dbConnection.Close();
                statementsExecutedPriorToRollBack++;
            }
            catch
            {
                Console.WriteLine("error. rolling back. ");
                dbTransaction.Rollback();
                dbConnection.Close();
                rolledBack = true;
            }
            Console.WriteLine("creating second datareader.");
            IDataReader dr2 =
                databaseConnection.LoadDataReader(
                    new SqlStatement(databaseConnection,
                                     "SELECT * FROM TestTableRead Order By TestTableReadData"));
            Assert.IsTrue(rolledBack);
            Console.WriteLine("reading from second datareader.");
            Assert.IsFalse(dr2.Read());

            Assert.IsTrue(statementsExecutedPriorToRollBack == 1);

            //Create transaction without Error
            dbConnection = databaseConnection.GetConnection();
            dbConnection.Open();
            dbTransaction = dbConnection.BeginTransaction(databaseConnection.IsolationLevel);
            //insert first record
            try
            {
                sql = "Insert into TestTableRead (TestTableReadData) VALUES ('Test')";
                databaseConnection.ExecuteRawSql(sql, dbTransaction);
                //insert second record
                sql = "Insert into TestTableRead (TestTableReadData) VALUES ('Test2')";
                databaseConnection.ExecuteRawSql(sql, dbTransaction);

                dbTransaction.Commit();
                dbConnection.Close();
            }
            catch
            {
                dbTransaction.Rollback();
                dbConnection.Close();
            }
            dr2.Close();
            dr2 =
                databaseConnection.LoadDataReader(
                    new SqlStatement(databaseConnection,
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

        [Test]
        public void Test_CreateSqlFormatter()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection dbConn = new DatabaseConnection_Stub();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            SqlFormatter sqlFormatter = dbConn.SqlFormatter;
            //---------------Test Result -----------------------
            Assert.IsNotNull(sqlFormatter);
            Assert.AreEqual("[", sqlFormatter.LeftFieldDelimiter);
            Assert.AreEqual("]", sqlFormatter.RightFieldDelimiter);
            Assert.AreEqual("TOP", sqlFormatter.LimitClauseAtBeginning);
            Assert.AreEqual("", sqlFormatter.LimitClauseAtEnd);
            Assert.AreEqual("[", dbConn.LeftFieldDelimiter);
            Assert.AreEqual("]", dbConn.RightFieldDelimiter);
//            StringAssert.Contains("TOP", dbConn.GetLimitClauseForBeginning(1));
//            Assert.AreEqual("", dbConn.GetLimitClauseForEnd(1));
        }

        [Test]
        public void Test_NoColumnName_DoesntError_SqlServer()
        {
            //---------------Set up test pack-------------------
            DatabaseConfig databaseConfig = new DatabaseConfig("SqlServer", "localhost", "habanero_test_branch_2_3", "sa", "sa", null);
            DatabaseConnection.CurrentConnection = databaseConfig.GetDatabaseConnection();
            //DatabaseConnection.CurrentConnection = new DatabaseConnectionSqlServer("System.Data", "System.Data.SqlClient.SqlConnection","server=localhost;database=habanero_test_branch_2_3;user=sa;password=sa");
            const string sql = "Select FirstName + ', ' + Surname from tbPersonTable";
            SqlStatement sqlStatement = new SqlStatement(DatabaseConnection.CurrentConnection, sql);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            DataTable dataTable = DatabaseConnection.CurrentConnection.LoadDataTable(sqlStatement, "", "");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, dataTable.Columns.Count);
            this.SetupDBConnection();
        }
        internal class DatabaseConnection_Stub : DatabaseConnection
        {
        }
    }

}

