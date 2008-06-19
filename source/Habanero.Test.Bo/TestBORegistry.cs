using Habanero.Base;
using Habanero.BO;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBORegistry 
    {
        [SetUp]
        public void SetupTest()
        {
           
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
        
        }

 

        [Test]
        public void TestSetDataAccessor()
        {
            //---------------Set up test pack-------------------
            IDataAccessor dataAccessor = new DataAccessorDB();
            //---------------Execute Test ----------------------
            BORegistry.DataAccessor = dataAccessor;
            //---------------Test Result -----------------------
            Assert.AreSame(dataAccessor, BORegistry.DataAccessor);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestDataAccessorDBConstructor()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            IDataAccessor dataAccessor = new DataAccessorDB();
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(BusinessObjectLoaderDB), dataAccessor.BusinessObjectLoader);
            Assert.IsInstanceOfType(typeof(TransactionCommitterFactoryDB), dataAccessor.TransactionCommiterFactory);

            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDataAccessorInMemoryConstructor()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            IDataAccessor dataAccessor = new DataAccessorInMemory();
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(BusinessObjectLoaderInMemory), dataAccessor.BusinessObjectLoader);
            Assert.IsInstanceOfType(typeof(TransactionCommitterFactoryInMemory), dataAccessor.TransactionCommiterFactory);
            //---------------Tear Down -------------------------
        }


        
    }

   
}