using Habanero.Db;
using NUnit.Framework;

namespace Chillisoft.Test.Db.v2
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
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.MySQL, "test", "test", "test", "test", "1000");
            DatabaseConnectionFactory factory = new DatabaseConnectionFactory();
            DatabaseConnection connection = factory.CreateConnection(config);
            Assert.AreEqual("MySql.Data.MySqlClient", connection.GetTestConnection().GetType().Namespace);
        }

        [Test]
        public void TestCreateConnectionSqlServer()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.SQLServer, "test", "test", "test", "test", "1000");
            DatabaseConnectionFactory factory = new DatabaseConnectionFactory();
            DatabaseConnection connection = factory.CreateConnection(config);
            Assert.AreEqual("System.Data.SqlClient", connection.GetTestConnection().GetType().Namespace);
        }

        [Test]
        public void TestCreateConnectionOracle()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.Oracle, "test", "test", "test", "test", "1000");
            DatabaseConnectionFactory factory = new DatabaseConnectionFactory();
            DatabaseConnection connection = factory.CreateConnection(config);
            Assert.AreEqual("System.Data.OracleClient", connection.GetTestConnection().GetType().Namespace);
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
            DatabaseConnectionFactory factory = new DatabaseConnectionFactory();
            DatabaseConnection connection = factory.CreateConnection(config);
            Assert.AreEqual("System.Data.OleDb", connection.GetTestConnection().GetType().Namespace);
        }
    }
}