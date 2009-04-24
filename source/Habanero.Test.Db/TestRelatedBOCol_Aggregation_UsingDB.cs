using Habanero.BO;
using Habanero.DB;
using Habanero.Test.BO.BusinessObjectCollection;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestRelatedBOCol_Aggregation_UsingDB : TestRelatedBOCol_Aggregation
    {
        [TestFixtureSetUp]
        public override void TestFixtureSetup()
        {
            if (DatabaseConnection.CurrentConnection != null &&
                DatabaseConnection.CurrentConnection.GetType() == typeof(DatabaseConnectionMySql))
            {
                return;
            }
            DatabaseConnection.CurrentConnection =
                new DatabaseConnectionMySql("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection");
            DatabaseConnection.CurrentConnection.ConnectionString =
                MyDBConnection.GetDatabaseConfig().GetConnectionString();
            DatabaseConnection.CurrentConnection.GetConnection();

            BORegistry.DataAccessor = new DataAccessorDB();
        }
    }
}