using CustomerExample.BO;
using Habanero.Base;
using Habanero.BO;
using NUnit.Framework;

namespace Test.CustomerExample.BO
{
    [TestFixture]
    public class TestSetCustomerBOProp : TestBase
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
        public void Test_SetCustomerName_ToValidValue()
        {
            //When a property is set to a valid value for a compulsory field
            // that has a broken rule the rule is set to no longer broken.
            //---------------Set up test pack-------------------
            Customer customer = new Customer();
            IBOProp customerNameBoProp = customer.Props["CustomerName"];

            //---------------Assert Precondition----------------
            Assert.IsFalse(customerNameBoProp.IsDirty);
            Assert.IsFalse(customerNameBoProp.IsValid);
            StringAssert.Contains("'Customer Name' is a compulsory field and has no value", customerNameBoProp.InvalidReason);

            //---------------Execute Test ----------------------
            customer.CustomerName = "Valid Name";

            //---------------Test Result -----------------------
            Assert.IsTrue(customerNameBoProp.IsDirty);
            Assert.IsTrue(customerNameBoProp.IsValid);
            Assert.AreEqual("", customerNameBoProp.InvalidReason);
        }

        [Test]
        public void Test_SetCustomerName_ToInvalidValue()
        {
            //When a property is set to an In Valid Value for a compulsory field
            // that has no value the broken rule is changed from compulsory to
            // Invalid Value.
            //---------------Set up test pack-------------------
            Customer customer = new Customer();
            IBOProp customerNameBoProp = customer.Props["CustomerName"];

            //---------------Assert Precondition----------------
            Assert.IsFalse(customerNameBoProp.IsDirty);
            Assert.IsFalse(customerNameBoProp.IsValid);
            StringAssert.Contains("'Customer Name' is a compulsory field and has no value", customerNameBoProp.InvalidReason);


            //---------------Execute Test ----------------------
            customer.CustomerName = "Inv";

            //---------------Test Result -----------------------
            Assert.IsTrue(customerNameBoProp.IsDirty);
            Assert.IsFalse(customerNameBoProp.IsValid);
            StringAssert.Contains("'Inv' for property 'Customer Name' is not valid for the rule 'CustomerName'. The length cannot be less than 5 character", customerNameBoProp.InvalidReason);
        }

        [Test]
        public void Test_SetCustomerName_ToInvalidValue_FromValidValue()
        {
            //When a property is set to an In Valid Value for a compulsory field
            // that has no value the broken rule is changed from compulsory to
            // Invalid Value.
            //---------------Set up test pack-------------------
            Customer customer = new Customer();
            IBOProp customerNameBoProp = customer.Props["CustomerName"];
            customer.CustomerName = "Valid Name";

            //---------------Assert Precondition----------------
            Assert.AreEqual("Valid Name", customerNameBoProp.Value);
            Assert.IsTrue(customerNameBoProp.IsValid);
            Assert.AreEqual("", customerNameBoProp.InvalidReason);

            //---------------Execute Test ----------------------
            customer.CustomerName = "Inv";

            //---------------Test Result -----------------------
            Assert.AreEqual("Inv", customerNameBoProp.Value);
            Assert.IsFalse(customerNameBoProp.IsValid);
            StringAssert.Contains("'Inv' for property 'Customer Name' is not valid for the rule 'CustomerName'. The length cannot be less than 5 character", customerNameBoProp.InvalidReason);
        }

        [Test]
        public void Test_SetCustomerCode_ForNewCustomer()
        {
            //A WriteNew property can be written to when the business object is new.
            //---------------Set up test pack-------------------
            Customer customer = new Customer();
            IBOProp customerCodeBoProp = customer.Props["CustomerCode"];

            //---------------Assert Precondition----------------
            Assert.AreEqual(PropReadWriteRule.WriteNew, customerCodeBoProp.PropDef.ReadWriteRule);

            //---------------Execute Test ----------------------
            customer.CustomerCode = "Code";

            //---------------Test Result ----------------------- 
            Assert.AreEqual("Code",customer.CustomerCode);

        }

        [Test]
        public void Test_SetCustomerCode_ForPersistedCustomer()
        {
            //A WriteNew property can not be written to when the business object already persisted.
            //---------------Set up test pack-------------------
            Customer customer = CreateSavedCustomer();
            IBOProp customerCodeBoProp = customer.Props["CustomerCode"];

            //---------------Assert Precondition----------------
            Assert.AreEqual(PropReadWriteRule.WriteNew, customerCodeBoProp.PropDef.ReadWriteRule);
            Assert.IsFalse(customer.Status.IsNew);

            //---------------Execute Test ----------------------
            try
            {
                customer.CustomerCode = "Code New";
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (BOPropWriteException ex)
            {
                StringAssert.Contains("The property 'Customer Code' is not editable since it is set up as WriteNew and the object is not new", ex.Message);
            }
        }
    }
}