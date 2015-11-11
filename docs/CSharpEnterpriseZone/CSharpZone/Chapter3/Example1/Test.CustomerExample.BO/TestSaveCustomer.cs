using CustomerExample.BO;
using Habanero.BO;
using NUnit.Framework;

namespace Test.CustomerExample.BO
{
    [TestFixture]
    public class TestSaveCustomer : TestBase
    {
        [SetUp]
        public override void SetupTest()
        {
            //Runs every time that any testmethod is executed
            base.SetupTest();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public override void TearDownTest()
        {
            //runs every time any testmethod is complete
            base.TearDownTest();
        }

        [Test]
        public void Test_SaveValidCustomer()
        {
            //---------------Set up test pack-------------------
            Customer customer = new Customer();
            customer.CustomerName = "Valid Name";
            customer.CustomerCode = "Code";
            
            //---------------Assert Precondition----------------
            Assert.IsTrue(customer.Status.IsNew);
            Assert.IsTrue(customer.Status.IsDirty);

            //---------------Execute Test ----------------------
            customer.Save();

            //---------------Test Result -----------------------
            Assert.IsFalse(customer.Status.IsNew);
            Assert.IsFalse(customer.Status.IsDirty);
        }

        [Test]
        public void Test_Save_Invalid_ValidCustomer()
        {
            //---------------Set up test pack-------------------
            Customer customer = new Customer();
            customer.CustomerCode = "Code";

            //---------------Assert Precondition----------------
            Assert.IsTrue(customer.Status.IsNew);
            Assert.IsTrue(customer.Status.IsDirty);

            //---------------Execute Test ----------------------
            try
            {
                customer.Save();
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (BusObjectInAnInvalidStateException ex)
            {
                StringAssert.Contains("'Customer Name' is a compulsory field and has no value", ex.Message);
            }
        }
    }
}