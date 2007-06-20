using Habanero.Db;
using NUnit.Framework;

namespace Habanero.Test.Db
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
                new DatabaseConnectionMySQL("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection");
            conn.ConnectionString =
                new DatabaseConfig(DatabaseConfig.MySQL, "test", "test", "test", "test", "1000").GetConnectionString();
            Assert.AreEqual("MySql.Data.MySqlClient", conn.GetTestConnection().GetType().Namespace,
                            "Namespace of mysqlconnection is wrong.");
        }

        [Test]
        public void TestCreateDatabaseConnectionSqlServer()
        {
            DatabaseConnection conn =
                new DatabaseConnectionSQLServer("System.Data", "System.Data.SqlClient.SqlConnection");
            conn.ConnectionString =
                new DatabaseConfig(DatabaseConfig.SQLServer, "test", "test", "test", "test", "1000").GetConnectionString
                    ();
            Assert.AreEqual("System.Data.SqlClient", conn.GetTestConnection().GetType().Namespace,
                            "Namespace of SQL connection is wrong.");
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
            Assert.AreEqual("System.Data.OracleClient", conn.GetTestConnection().GetType().Namespace,
                            "Namespace of Oracle connection is wrong.");
        }

        [Test]
        public void TestCreateDatabaseConnectionAccess()
        {
            DatabaseConnection conn = new DatabaseConnectionAccess("System.Data", "System.Data.OleDb.OleDbConnection");
            conn.ConnectionString =
                new DatabaseConfig(DatabaseConfig.Access, "test", "test", "test", "test", "1000").GetConnectionString();
            Assert.AreEqual("System.Data.OleDb", conn.GetTestConnection().GetType().Namespace,
                            "Namespace of Access connection is wrong.");
        }
    }
}
