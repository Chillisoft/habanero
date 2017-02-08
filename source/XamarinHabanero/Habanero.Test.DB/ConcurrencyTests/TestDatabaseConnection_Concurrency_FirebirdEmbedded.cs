using System.Threading.Tasks;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB.ConcurrencyTests
{
    [TestFixture]
    public class TestDatabaseConnection_Concurrency_FirebirdEmbedded : FirebirdEmbeddedTestsBase
    {
        // TODO:
        [Test]
        public void Test_Concurrent_ConnectionOpenAndBeginTransaction()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Parallel.For(0, 1000, i =>
                                  {
                                      var databaseConnection = DatabaseConnection.CurrentConnection;
                                      var connection = databaseConnection.GetConnection();
                                      connection.Open();
                                      var transaction = databaseConnection.BeginTransaction(connection);
                                      transaction.Rollback();
                                  });

            //---------------Test Result -----------------------
        }
    }
}