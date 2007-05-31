using Chillisoft.Db.v2;
using Chillisoft.Test.General.v2;

namespace Chillisoft.Test
{
    /// <summary>
    /// Summary description for TestUsingDatabase.
    /// </summary>
    public class TestUsingDatabase : ArchitectureTest
    {
        public void SetupDBConnection()
        {
            if (DatabaseConnection.CurrentConnection != null &&
                DatabaseConnection.CurrentConnection.GetType() == typeof (DatabaseConnectionMySQL))
            {
                return;
            }
            else
            {
                DatabaseConnection.CurrentConnection =
                    new DatabaseConnectionMySQL("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection");
                DatabaseConnection.CurrentConnection.ConnectionString =
                    MyDBConnection.GetDatabaseConfig().GetConnectionString();
                DatabaseConnection.CurrentConnection.GetConnection();
            }
        }

        public void SetupDBOracleConnection()
        {
            if (DatabaseConnection.CurrentConnection != null &&
                DatabaseConnection.CurrentConnection.GetType() == typeof(DatabaseConnectionOracle))
            {
                return;
            }
            else
            {
                DatabaseConnection.CurrentConnection =
                    new DatabaseConnectionOracle("System.Data.OracleClient", "System.Data.OracleClient.OracleConnection");
                ConnectionStringOracleFactory oracleConnectionString = new ConnectionStringOracleFactory();
                string connStr = oracleConnectionString.GetConnectionString("core1", "XE", "system", "system", "1521");
                DatabaseConnection.CurrentConnection.ConnectionString = connStr;
                DatabaseConnection.CurrentConnection.GetConnection();
            }
        }
    }
}