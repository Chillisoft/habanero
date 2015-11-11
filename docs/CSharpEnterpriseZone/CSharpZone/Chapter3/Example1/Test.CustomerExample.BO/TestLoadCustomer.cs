using CustomerExample.BO;
using Habanero.Base;
using Habanero.BO.ObjectManager;
using NUnit.Framework;

namespace Test.CustomerExample.BO
{
    [TestFixture]
    public class TestLoadCustomer : TestBase
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
        public void Test_LoadCustomerUsingID()
        {
            ///This test shows that if a persisted object is loaded from the 
            /// dataStore using the BusinessObjectLoader.GetBusinessObject. 
            /// Then an object with the exact same status and data as 
            /// the persisted object is loaded.
            //---------------Set up test pack-------------------
            Customer customer = CreateSavedCustomer();
            
            //---------------Assert Precondition----------------
            Assert.IsFalse(customer.Status.IsDirty);
            Assert.IsFalse(customer.Status.IsNew);
            Assert.IsTrue(customer.Status.IsValid());

            //---------------Execute Test ----------------------
            Customer customer2 = GetBusinessObjectLoader().GetBusinessObject<Customer>(customer.ID);

            //---------------Test Result -----------------------
            Assert.IsFalse(customer2.Status.IsDirty);
            Assert.IsFalse(customer2.Status.IsNew);
            Assert.IsTrue(customer2.Status.IsValid());

            Assert.AreEqual(customer2.CustomerCode, customer.CustomerCode);
            Assert.AreEqual(customer2.CustomerName, customer.CustomerName);
            Assert.AreEqual(customer2.CustomerID, customer.CustomerID);
            Assert.AreSame(customer, customer2);
        }

        [Test]
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
            Customer customer2 = GetBusinessObjectLoader().GetBusinessObject<Customer>(criteria);

            //---------------Test Result -----------------------
            Assert.IsFalse(customer2.Status.IsNew);
            Assert.AreSame(customer, customer2);
        }

        [Test]
        public void Test_LoadCustomer_LoadsBOProps()
        {
            ///This test shows that if a persisted object is loaded from the 
            /// dataStore using the BusinessObjectLoader.GetBusinessObject. 
            /// Then an object with the exact same status and data as 
            /// the persisted object is loaded.
            //---------------Set up test pack-------------------
            Customer customer = CreateSavedCustomer();
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Assert Precondition----------------
            Assert.IsFalse(customer.Status.IsDirty);
            Assert.IsFalse(customer.Status.IsNew);
            Assert.IsTrue(customer.Status.IsValid());

            //---------------Execute Test ----------------------
            Customer customer2 = GetBusinessObjectLoader().GetBusinessObject<Customer>(customer.ID);

            //---------------Test Result -----------------------
            Assert.IsFalse(customer2.Status.IsDirty);
            Assert.IsFalse(customer2.Status.IsNew);
            Assert.IsTrue(customer2.Status.IsValid());

            Assert.AreEqual(customer2.CustomerCode, customer.CustomerCode);
            Assert.AreEqual(customer2.CustomerName, customer.CustomerName);
            Assert.AreEqual(customer2.CustomerID, customer.CustomerID);
            Assert.AreSame(customer, customer2);
        }


        [Test]
        public void Test_LoadCustomerBOPropsUsingID()
        {
            ///This test shows that if a persisted object is loaded from the 
            /// dataStore using the BusinessObjectLoader.GetBusinessObject
            /// then the objects properteis are loaded with the appropriate persisted
            /// values.
            //---------------Set up test pack-------------------
            Customer customer = CreateSavedCustomer();

            //---------------Assert Precondition----------------
            Assert.IsFalse(customer.Status.IsNew);

            //---------------Execute Test ----------------------
            Customer customer2 = GetBusinessObjectLoader().GetBusinessObject<Customer>(customer.ID);
            //---------------Test Result -----------------------
            IBOProp customerProp = customer2.Props["CustomerName"];
            Assert.AreEqual(customer.CustomerName, customerProp.Value);
            Assert.AreEqual(customerProp.Value, customerProp.PersistedPropertyValue, 
                "the newly loaded properties Persisted Property value = Value");
        }
    }
}