// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System.Data;
using Habanero.Base;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    /// <summary>
    /// Summary description for TestDatabaseConnectionFactory.
    /// </summary>
    [TestFixture]
    public class TestDatabaseConnectionFactory
    {
        #region Firebird

        [Test]
        public void TestCreateConnectionFireBird()
        {
            //---------------Set up test pack-------------------
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.Firebird, "test", "test", "test", "test", "1000");

            //---------------Execute Test ----------------------
            IDatabaseConnection connection = new DatabaseConnectionFactory().CreateConnection(config);
            IDbConnection dbConnection = connection.GetConnection();

            //---------------Test Result -----------------------
            Assert.AreEqual("FirebirdSql.Data.FirebirdClient", dbConnection.GetType().Namespace);
        }

        #endregion

        #region MySql

        [Test]
        public void TestCreateConnectionMySql()
        {
            //---------------Set up test pack-------------------
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.MySql, "test", "test", "test", "test", "1000");

            //---------------Execute Test ----------------------
            IDatabaseConnection connection = new DatabaseConnectionFactory().CreateConnection(config);
            IDbConnection dbConnection = connection.GetConnection();

            //---------------Test Result -----------------------
            Assert.AreEqual("MySql.Data.MySqlClient", dbConnection.GetType().Namespace);
        }

        #endregion

        #region SqlServer

        [Test]
        public void TestCreateConnectionSqlServer()
        {
            //---------------Set up test pack-------------------
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.SqlServer, "test", "test", "test", "test", "1000");

            //---------------Execute Test ----------------------
            IDatabaseConnection connection = new DatabaseConnectionFactory().CreateConnection(config);
            IDbConnection dbConnection = connection.GetConnection();

            //---------------Test Result -----------------------
            Assert.AreEqual("System.Data.SqlClient", dbConnection.GetType().Namespace);
        }

        #endregion

        #region Oracle

        [Test]
        public void TestCreateConnectionOracle()
        {
            //---------------Set up test pack-------------------
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.Oracle, "test", "test", "test", "test", "1000");

            //---------------Execute Test ----------------------
            IDatabaseConnection connection = new DatabaseConnectionFactory().CreateConnection(config);
            IDbConnection dbConnection = connection.GetConnection();

            //---------------Test Result -----------------------
            Assert.AreEqual("System.Data.OracleClient", dbConnection.GetType().Namespace);
        }
        
        #endregion

        #region Access

        [Test]
        public void TestCreateConnectionAccess()
        {
            //---------------Set up test pack-------------------
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.Access, "test", "test", "test", "test", "1000");

            //---------------Execute Test ----------------------
            IDatabaseConnection connection = new DatabaseConnectionFactory().CreateConnection(config);
            IDbConnection dbConnection = connection.GetConnection();

            //---------------Test Result -----------------------
            Assert.AreEqual("System.Data.OleDb", dbConnection.GetType().Namespace);
        }

        #endregion

        #region PostgreSql

        [Test]
        public void TestCreateConnectionPostgreSql()
        {
            //---------------Set up test pack-------------------
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.PostgreSql, "test", "test", "test", "test", "1000");

            //---------------Execute Test ----------------------
            IDatabaseConnection connection = new DatabaseConnectionFactory().CreateConnection(config);
            IDbConnection dbConnection = connection.GetConnection();

            //---------------Test Result -----------------------
            Assert.AreEqual("Npgsql", dbConnection.GetType().Namespace);
        }

        #endregion

        #region SQLite

        [Test, Ignore("Issue with SQLite 64-bit driver in Hudson")]
        public void TestCreateConnectionSQLite()
        {
            //---------------Set up test pack-------------------
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.SQLite, "test", "test", "test", "test", "1000");

            //---------------Execute Test ----------------------
            IDatabaseConnection connection = new DatabaseConnectionFactory().CreateConnection(config);
            IDbConnection dbConnection = connection.GetConnection();

            //---------------Test Result -----------------------
            Assert.AreEqual("System.Data.SQLite", dbConnection.GetType().Namespace);
        }

        #endregion

        #region all
        [Test]
        public void TestUsingCustomAssembly()
        {
            //---------------Set up test pack-------------------
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.Oracle, "test", "test", "test", "test", "1000");
            config.AssemblyName = "System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            config.FullClassName = "System.Data.SqlClient.SqlConnection";

            //---------------Execute Test ----------------------
            IDatabaseConnection connection = new DatabaseConnectionFactory().CreateConnection(config);
            IDbConnection dbConnection = connection.GetConnection();

            //---------------Test Result -----------------------
            Assert.AreEqual("System.Data.SqlClient", dbConnection.GetType().Namespace);
            StringAssert.Contains("System.Data, ", dbConnection.GetType().Assembly.FullName);
            StringAssert.Contains(", Culture=neutral, PublicKeyToken=b77a5c561934e089", dbConnection.GetType().Assembly.FullName);
            //---------------Tear Down -------------------------          
        }
        #endregion
    }
}