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
	}
}