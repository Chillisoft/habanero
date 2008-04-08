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
        public TestDatabaseConnectionFactory()
        {
        }

        [Test]
        public void TestCreateConnectionMySql()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.MySql, "test", "test", "test", "test", "1000");
            DatabaseConnection connection = DatabaseConnectionFactory.CreateConnection(config);
            Assert.AreEqual("MySql.Data.MySqlClient", connection.TestConnection.GetType().Namespace);
        }

        [Test]
        public void TestCreateConnectionSqlServer()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.SqlServer, "test", "test", "test", "test", "1000");
            DatabaseConnection connection = DatabaseConnectionFactory.CreateConnection(config);
            Assert.AreEqual("System.Data.SqlClient", connection.TestConnection.GetType().Namespace);
        }

        [Test]
        public void TestCreateConnectionOracle()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.Oracle, "test", "test", "test", "test", "1000");
            DatabaseConnection connection = DatabaseConnectionFactory.CreateConnection(config);
            Assert.AreEqual("System.Data.OracleClient", connection.TestConnection.GetType().Namespace);
            //Assert.AreEqual("Oracle.DataAccess.Client", connection.GetTestConnection().GetType().Namespace);
        }

        /*
        [Test]
        public void TestCreateConnectionOracleUsingMicrosoft()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.Oracle, "test", "test", "test", "test", "1000");
            DatabaseConnectionFactory factory = new DatabaseConnectionFactory();
            DatabaseConnection connection =
                factory.CreateConnection(config, "System.Data.OracleClient", "System.Data.OracleClient.OracleConnection");
            Assert.AreEqual("System.Data.OracleClient", connection.GetTestConnection().GetType().Namespace);
        }
        */

		[Test]
		public void TestCreateConnectionAccess()
		{
			DatabaseConfig config = new DatabaseConfig(DatabaseConfig.Access, "test", "test", "test", "test", "1000");
			DatabaseConnection connection = DatabaseConnectionFactory.CreateConnection(config);
			Assert.AreEqual("System.Data.OleDb", connection.TestConnection.GetType().Namespace);
		}

        [Test]
        public void TestCreateConnectionPostgreSql()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.PostgreSql, "test", "test", "test", "test", "1000");
            DatabaseConnection connection = DatabaseConnectionFactory.CreateConnection(config);
            Assert.AreEqual("Npgsql", connection.TestConnection.GetType().Namespace);
        }

        [Test, Ignore("Issue with SQLite 64-bit driver")]
        public void TestCreateConnectionSQLite()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.SQLite, "test", "test", "test", "test", "1000");
            DatabaseConnection connection = DatabaseConnectionFactory.CreateConnection(config);
            Assert.AreEqual("System.Data.SQLite", connection.TestConnection.GetType().Namespace);
        }
    }
}