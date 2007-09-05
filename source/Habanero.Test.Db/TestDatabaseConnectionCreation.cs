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
    }
}
