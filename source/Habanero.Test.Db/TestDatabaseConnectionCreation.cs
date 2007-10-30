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

using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    /// <summary>
    /// Summary description for TestDatabaseConnectionCreation.
    /// </summary>
    [TestFixture]
    public class TestDatabaseConnectionCreation
    {
        public TestDatabaseConnectionCreation()
        {
        }

        [Test]
        public void TestCreateDatabaseConnectionMySql()
        {
            DatabaseConnection conn =
                new DatabaseConnectionMySql("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection");
            conn.ConnectionString =
                new DatabaseConfig(DatabaseConfig.MySql, "test", "test", "test", "test", "1000").GetConnectionString();
            Assert.AreEqual("MySql.Data.MySqlClient", conn.TestConnection.GetType().Namespace,
                            "Namespace of mysqlconnection is wrong.");
        }

        [Test]
        public void TestCreateDatabaseConnectionSqlServer()
        {
            DatabaseConnection conn =
                new DatabaseConnectionSqlServer("System.Data", "System.Data.SqlClient.SqlConnection");
            conn.ConnectionString =
                new DatabaseConfig(DatabaseConfig.SqlServer, "test", "test", "test", "test", "1000").GetConnectionString
                    ();
            Assert.AreEqual("System.Data.SqlClient", conn.TestConnection.GetType().Namespace,
                            "Namespace of Sql connection is wrong.");
        }

        //[Test]
        //public void TestCreateDatabaseConnectionOracle()
        //{

        //    DatabaseConnection conn =
        //        new DatabaseConnectionOracle("Oracle.DataAccess", "Oracle.DataAccess.Client.OracleConnection");
        //    conn.ConnectionString =
        //        new DatabaseConfig(DatabaseConfig.Oracle, "test", "test", "test", "test", "1000").GetConnectionString();
        //    Assert.AreEqual("Oracle.DataAccess.Client", conn.GetTestConnection().GetType().Namespace,
        //                    "Namespace of Oracle connection is wrong.");
        //}
        
        [Test]
        public void TestCreateDatabaseConnectionOracleMicrosoft()
        {

            DatabaseConnection conn =
                new DatabaseConnectionOracle("System.Data.OracleClient", "System.Data.OracleClient.OracleConnection");
            conn.ConnectionString =
                new DatabaseConfig(DatabaseConfig.Oracle, "test", "test", "test", "test", "1000").GetConnectionString();
            Assert.AreEqual("System.Data.OracleClient", conn.TestConnection.GetType().Namespace,
                            "Namespace of Oracle connection is wrong.");
        }

        [Test]
        public void TestCreateDatabaseConnectionAccess()
        {
            DatabaseConnection conn = new DatabaseConnectionAccess("System.Data", "System.Data.OleDb.OleDbConnection");
            conn.ConnectionString =
                new DatabaseConfig(DatabaseConfig.Access, "test", "test", "test", "test", "1000").GetConnectionString();
            Assert.AreEqual("System.Data.OleDb", conn.TestConnection.GetType().Namespace,
                            "Namespace of Access connection is wrong.");
        }

		[Test]
		public void TestCreateDatabaseConnectionPostgreSql()
		{
			DatabaseConnection conn = new DatabaseConnectionPostgreSql("Npgsql", "Npgsql.NpgsqlConnection");
			conn.ConnectionString =
				new DatabaseConfig(DatabaseConfig.Access, "test", "test", "test", "test", "1000").GetConnectionString();
			Assert.AreEqual("Npgsql", conn.TestConnection.GetType().Namespace,
							"Namespace of PostgreSql connection is wrong.");
		}

    }
}
