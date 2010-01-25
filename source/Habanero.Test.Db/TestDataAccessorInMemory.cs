using Habanero.Base;
using Habanero.BO;
using Habanero.DB;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestDataAccessorInMemory
    {
        [Test]
        public void Test_UsesBusinessObjectLoaderInMemory()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            DataAccessorInMemory dataAccessorInMemory = new DataAccessorInMemory();
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(BusinessObjectLoaderInMemory), dataAccessorInMemory.BusinessObjectLoader);
        }

        [Test]
        public void Test_UsesTransactionCommitterInMemory()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            DataAccessorInMemory dataAccessorInMemory = new DataAccessorInMemory();
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(TransactionCommitterInMemory), dataAccessorInMemory.CreateTransactionCommitter());
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_StandardConstructor_CreatesADataStore()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            DataAccessorInMemory dataAccessorInMemory = new DataAccessorInMemory();
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataAccessorInMemory.DataStoreInMemory);
        }

        [Test]
        public void Test_Constructor_UsesPassedInDataStore()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = MockRepository.GenerateMock<DataStoreInMemory>();
            //---------------Execute Test ----------------------
            DataAccessorInMemory dataAccessorInMemory = new DataAccessorInMemory(dataStore);
            //---------------Test Result -----------------------
            Assert.AreSame(dataStore, dataAccessorInMemory.DataStoreInMemory);
        }

    }
}