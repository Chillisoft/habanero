using System;
using NUnit.Framework;
using CustomerExample.BO;
namespace Test.CustomerExample.BO
{
    [TestFixture]
    public class TestCreateCustomer : TestBase
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

        [Test, Ignore("This test setup when there are no rules for customer name")]
        public void TestCreateNewCustomer_NoPropertyRules()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Customer customer = new Customer();
            //---------------Test Result -----------------------
            Assert.IsTrue(customer.Status.IsNew);
            Assert.IsNotNull(customer.CustomerID);

            Assert.IsFalse(customer.Status.IsDeleted);
            Assert.IsFalse(customer.Status.IsDirty);
            Assert.IsFalse(customer.Status.IsEditing);
            Assert.IsTrue(customer.Status.IsValid());
            Assert.AreEqual("", customer.Status.IsValidMessage);
        }

        [Test]
        public void TestCreateNewCustomer_CustomerNameCompusory()
        {
            //--------------------------------------------------
            //When a new customer is created it is created with any 
            //  broken rules for any 
            //  compulsory properties that do not have a default value set
            //  (In this case Customer name).
            //--------------------------------------------------
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Customer customer = new Customer();
            //---------------Test Result -----------------------
            Assert.IsTrue(customer.Status.IsNew);
            Assert.IsNotNull(customer.CustomerID);

            Assert.IsFalse(customer.Status.IsDeleted);
            Assert.IsFalse(customer.Status.IsDirty);
            Assert.IsFalse(customer.Status.IsEditing);
            Assert.IsFalse(customer.Status.IsValid());
            StringAssert.Contains("'Customer Name' is a compulsory field and has no value", 
                customer.Status.IsValidMessage);
        }

        [Test]
        public void TestNewCustomer_SetCustomerName_ToValidValue()
        {
            //When a property is set to a valid value for a compulsory field
            // that has a broken rule the rule is set to no longer broken.
            //---------------Set up test pack-------------------
            Customer customer = new Customer();
            //---------------Assert Precondition----------------
            Assert.IsTrue(customer.Status.IsNew);
            Assert.IsFalse(customer.Status.IsDirty);
            Assert.IsFalse(customer.Status.IsEditing);
            Assert.IsFalse(customer.Status.IsValid());
            StringAssert.Contains("'Customer Name' is a compulsory field and has no value",
    customer.Status.IsValidMessage);

            //---------------Execute Test ----------------------
            customer.CustomerName = "Valid Name";
            customer.CustomerCode = "Code";

            //---------------Test Result -----------------------
            Assert.IsTrue(customer.Status.IsDirty);
            Assert.IsTrue(customer.Status.IsEditing);
            Assert.AreEqual("Valid Name", customer.CustomerName);

            Assert.IsTrue(customer.Status.IsValid());
            Assert.AreEqual("", customer.Status.IsValidMessage);
        }


        [Test]
        public void TestNewCustomer_SetCustomerName_ToInvalidValidValue()
        {
            //When a property is set to an In Valid Value for a compulsory field
            // that has no value the broken rule is changed from compulsory to
            // Invalid Value.
            //---------------Set up test pack-------------------
            Customer customer = new Customer();
            //---------------Assert Precondition----------------
            Assert.IsTrue(customer.Status.IsNew);
            Assert.IsFalse(customer.Status.IsDirty);
            Assert.IsFalse(customer.Status.IsEditing);
            StringAssert.Contains("'Customer Name' is a compulsory field and has no value",
    customer.Status.IsValidMessage);

            //---------------Execute Test ----------------------
            customer.CustomerName = "Inv";

            //---------------Test Result -----------------------
            Assert.IsTrue(customer.Status.IsDirty);
            Assert.IsTrue(customer.Status.IsEditing);
            StringAssert.Contains("'Inv' for property 'Customer Name' is not valid for the rule 'CustomerName'. The length cannot be less than 5 character",
    customer.Status.IsValidMessage);
        }


        [Test]
        public void TestNewCustomer_SetCustomerName_ToInvalidValidValue_FromValidValue()
        {
            //When a property is set to an In Valid Value for a compulsory field
            // that has no value the broken rule is changed from compulsory to
            // Invalid Value.
            //---------------Set up test pack-------------------
            Customer customer = new Customer();
            customer.CustomerName = "Valid Name";
            customer.CustomerCode = "Code";

            //---------------Assert Precondition----------------
            Assert.IsTrue(customer.Status.IsValid());
            Assert.AreEqual("", customer.Status.IsValidMessage);

            //---------------Execute Test ----------------------
            customer.CustomerName = "Inv";

            //---------------Test Result -----------------------
            Assert.IsTrue(customer.Status.IsDirty);
            Assert.AreEqual("Inv", customer.CustomerName);

            Assert.IsFalse(customer.Status.IsValid());
            StringAssert.Contains("'Inv' for property 'Customer Name' is not valid for the rule 'CustomerName'. The length cannot be less than 5 character",
    customer.Status.IsValidMessage);
        }
        [Test]
        public void Test_CreateCustomer_WithDefaultValue()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            Customer customer = new Customer();

            //---------------Test Result -----------------------
            Assert.IsNotNull(customer.DateCustomerApproved);
            Assert.AreEqual(DateTime.Today, customer.DateCustomerApproved);
        }
    }
}
