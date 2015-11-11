using CustomerExample.BO;
using Habanero.Base;
using NUnit.Framework;

namespace Test.CustomerExample.BO
{
    [TestFixture]
    public class TestLoadCustomerCollection : TestBase
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

        public void Test_LoadCustomerUsingStringCriteria()
        {
            ///This test shows that if a persisted object is loaded from the 
            /// dataStore using the BusinessObjectLoader.GetBusinessObject. 
            /// Then an object with the exact same status and data as 
            /// the persisted object is loaded.
            //---------------Set up test pack-------------------
            Customer customer = CreateSavedCustomer();

            //---------------Assert Precondition----------------
            Assert.IsFalse(customer.Status.IsNew);

            //---------------Execute Test ----------------------
            string loadCriteria = "CustomerCode = " + customer.CustomerCode;
            Customer customer2 = GetBusinessObjectLoader().GetBusinessObject<Customer>(loadCriteria);

            //---------------Test Result -----------------------
            Assert.IsFalse(customer2.Status.IsNew);
            Assert.AreSame(customer, customer2);
        }

        [Test]
        public void Test_LoadCustomerUsingCriteriaObject()
        {
            ///This test shows that if a persisted object is loaded from the 
            /// dataStore using the BusinessObjectLoader.GetBusinessObject. 
            /// Then an object with the exact same status and data as 
            /// the persisted object is loaded.
            //---------------Set up test pack-------------------
            Customer customer = CreateSavedCustomer();

            //---------------Assert Precondition----------------
            Assert.IsFalse(customer.Status.IsNew);

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("CustomerCode", Criteria.ComparisonOp.Equals, customer.CustomerCode);
            IBusinessObjectCollection customerCol = GetBusinessObjectLoader().GetBusinessObjectCollection<Customer>(criteria);

            //---------------Test Result -----------------------
            Assert.IsFalse(customer2.Status.IsNew);
            Assert.AreSame(customer, customer2);
        }
    }
}