using CustomerExample.BO;
using NUnit.Framework;

namespace Test.CustomerExample.BO
{
    [TestFixture]
    public class TestDeleteCustomer : TestBase
    {
        [SetUp]
        public override void SetupTest()
        {
            //Runs every time that any testmethod is executed
            base.SetupTest();
            
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
        public void Test_MarkCustomerAsDeleted()
        {
            //---------------Set up test pack-------------------
            Customer customer = CreateSavedCustomer();

            //---------------Assert Precondition----------------
            Assert.IsFalse(customer.Status.IsNew);
            Assert.IsFalse(customer.Status.IsDeleted);
            Assert.IsFalse(customer.Status.IsDirty);

            //---------------Execute Test ----------------------
            customer.Delete();

            //---------------Test Result -----------------------
            Assert.IsFalse(customer.Status.IsNew);
            Assert.IsTrue(customer.Status.IsDeleted);
            Assert.IsTrue(customer.Status.IsDirty);
        }

        [Test]
        public void Test_DeleteCustomer()
        {
            //---------------Set up test pack-------------------
            Customer customer = CreateSavedCustomer();
            customer.Delete();

            //---------------Assert Precondition----------------
            Assert.IsFalse(customer.Status.IsNew);
            Assert.IsTrue(customer.Status.IsDeleted);
            Assert.IsTrue(customer.Status.IsDirty);

            //---------------Execute Test ----------------------
            customer.Save();

            //---------------Test Result -----------------------
            Assert.IsTrue(customer.Status.IsNew);
            Assert.IsTrue(customer.Status.IsDeleted);
            Assert.IsFalse(customer.Status.IsDirty);
        }

    }
}