using System;
using CustomerExample.BO;
using NUnit.Framework;

namespace Test.CustomerExample.BO
{
    [TestFixture]
    public class TestRestoreCustomer : TestBase
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
        public void Test_RestoreNewCustomer()
        {
            //---------------Set up test pack-------------------
            Customer customer = new Customer();
            customer.DateCustomerApproved = DateTime.Today.AddDays(-1);

            //---------------Assert Precondition----------------
            Assert.AreNotEqual(DateTime.Today, customer.DateCustomerApproved);

            //---------------Execute Test ----------------------
            customer.Restore();

            //---------------Test Result -----------------------
            Assert.AreEqual(DateTime.Today, customer.DateCustomerApproved);
        }

        [Test]
        public void Test_RestoreLoadedCustomer()
        {
            //Tests Restoring a loaded business object.
            // This test shows that when the customer's Restore
            // method is called. The customers Status and Data is 
            // restored so as to be the same as an object just loaded
            // from the DataStore.
            //---------------Set up test pack-------------------
            Customer customer = CreateSavedCustomer();
            string origCustomerName = customer.CustomerName;
            customer.CustomerName = "New customer Name";

            //---------------Assert Precondition----------------
            Assert.IsTrue(customer.Status.IsDirty);
            Assert.AreNotEqual(origCustomerName, customer.CustomerName);

            //---------------Execute Test ----------------------
            customer.Restore();

            //---------------Test Result -----------------------
            Assert.IsFalse(customer.Status.IsDirty);
            Assert.AreEqual(origCustomerName, customer.CustomerName);
        }
    }
}