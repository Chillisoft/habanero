using CustomerExample.BO;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ObjectManager;
using NUnit.Framework;

namespace Test.CustomerExample.BO
{
    [TestFixture]
    public class TestSaveLoadingAndRestoringCustomerBOProp : TestBase
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
        public void Test_SaveBOProp()
        {
            //Testing that when a customer is saved the Persisted Property
            // Value is updated to the PropertyValue
            //---------------Set up test pack-------------------
            Customer customer = new Customer();
            const string newCustomerName = "Valid Name";
            customer.CustomerName = newCustomerName;
            customer.CustomerCode = "Code";
            IBOProp customerNameProp = customer.Props["CustomerName"];

            //---------------Assert Precondition----------------
            Assert.IsNull(customerNameProp.PersistedPropertyValue, "A new object should not have a persisted value");
            Assert.AreEqual(newCustomerName, customerNameProp.Value);
            Assert.IsTrue(customerNameProp.IsDirty);

            //---------------Execute Test ----------------------
            customer.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(newCustomerName, customerNameProp.Value);
            Assert.AreEqual(newCustomerName, customerNameProp.PersistedPropertyValue, 
                    "After saving the PersistedPropertyValue should be backed up to the property value");
            Assert.IsFalse(customerNameProp.IsDirty);
        }

        [Test]
        public void Test_LoadBOProp()
        {
            //Testing that when a customer is saved the Persisted Property
            // Value is updated to the PropertyValue
            //---------------Set up test pack-------------------
            Customer customer = CreateSavedCustomer();
            IBOProp customerNameProp = customer.Props["CustomerName"];

            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Assert Precondition----------------


            //---------------Execute Test ----------------------
            Customer customer2 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Customer>(customer.ID);

            //---------------Test Result -----------------------
//            Assert.AreEqual(newCustomerName, customerNameProp.Value);
//            Assert.AreEqual(newCustomerName, customerNameProp.PersistedPropertyValue,
//                    "After saving the PersistedPropertyValue should be backed up to the property value");

        }
    }
}