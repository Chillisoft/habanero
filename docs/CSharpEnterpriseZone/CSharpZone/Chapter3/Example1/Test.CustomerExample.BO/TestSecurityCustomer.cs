using System.Collections.Generic;
using CustomerExample.BO;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ObjectManager;
using NUnit.Framework;

namespace Test.CustomerExample.BO
{
    [TestFixture]
    public class TestSecurityCustomer : TestBase
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
        #region TestDelete
        [Test]
        public void Test_BusinessObjectAuthorisation_AllowDelete()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanDelete_True();
            Customer customer = new Customer();
            customer.SetAuthorisation(authorisationStub);

            //---------------Assert Precondition----------------
            Assert.IsTrue(authorisationStub.IsAuthorised(BusinessObjectActions.CanDelete));

            //---------------Execute Test ----------------------
            string message;
            bool isDeletable = customer.IsDeletable(out message);

            //---------------Test Result -----------------------
            Assert.IsTrue(isDeletable);
            Assert.AreEqual("", message);
        }
        [Test]
        public void Test_BusinessObjectAuthorisation_AllowDelete_False()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanDelete_False();
            Customer customer = new Customer();
            customer.SetAuthorisation(authorisationStub);
            //---------------Assert Precondition----------------
            Assert.IsFalse(authorisationStub.IsAuthorised(BusinessObjectActions.CanDelete));
            //---------------Execute Test ----------------------
            string message;
            bool isDeletable = customer.IsDeletable(out message);

            //---------------Test Result -----------------------
            Assert.IsFalse(isDeletable);
            StringAssert.Contains("The logged on user", message);
            StringAssert.Contains("is not authorised to delete ", message);
        }

        [Test]
        public void Test_DeleteBO_AllowDelete_True()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanDelete_True();

            Customer customer = CreateNewCustomerValid();
            customer.SetAuthorisation(authorisationStub);
            customer.Save();

            //---------------Assert Precondition----------------
            Assert.IsTrue(authorisationStub.IsAuthorised(BusinessObjectActions.CanDelete));
            Assert.IsFalse(customer.Status.IsNew);

            //---------------Execute Test ----------------------
            customer.Delete();
            customer.Save();

            //---------------Test Result -----------------------
            Assert.IsTrue(customer.Status.IsDeleted);
            Assert.IsTrue(customer.Status.IsNew);

        }

        private Customer CreateNewCustomerValid()
        {
            Customer customer = new Customer();
            customer.CustomerCode = "x" + GetRandomString();
            customer.CustomerName = "y" + GetRandomString();
            return customer;
        }

        [Test]
        public void Test_DeleteBO_Fail_AllowDelete_False()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanDelete_False();

            Customer customer = new Customer();
            customer.SetAuthorisation(authorisationStub);

            //---------------Assert Precondition----------------
            Assert.IsFalse(authorisationStub.IsAuthorised(BusinessObjectActions.CanDelete));

            //---------------Execute Test ----------------------
            try
            {
                customer.Delete();
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (BusObjDeleteException ex)
            {
                StringAssert.Contains("The logged on user", ex.Message);
                StringAssert.Contains("is not authorised to delete ", ex.Message);
            }
        }

        [Test]
        public void Test_SaveDeletedBO_Fail_AllowDelete_False()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanDelete_False();

            Customer customer = CreateNewCustomerValid();
            
            customer.Save();
            customer.Delete();
            customer.SetAuthorisation(authorisationStub);
            //---------------Assert Precondition----------------
            Assert.IsFalse(authorisationStub.IsAuthorised(BusinessObjectActions.CanDelete));
            Assert.IsTrue(customer.Status.IsDeleted);
            Assert.IsFalse(customer.Status.IsNew);
            //---------------Execute Test ----------------------
            try
            {
                customer.Save();
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (BusObjPersistException ex)
            {
                StringAssert.Contains("The logged on user", ex.Message);
                StringAssert.Contains("is not authorised to delete ", ex.Message);
            }
        }

        private static IBusinessObjectAuthorisation GetAuthorisationStub_CanDelete_False()
        {
            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanCreate);
            return authorisationStub;
        }

        private static IBusinessObjectAuthorisation GetAuthorisationStub_CanDelete_True()
        {
            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanDelete);
            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanCreate);
            return authorisationStub;
        }
        #endregion //TestDelete

        #region TestCreate
        [Test]
        public void Test_BusinessObjectAuthorisation_AllowCreate()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanCreate);
            Customer customer = new Customer();
            customer.SetAuthorisation(authorisationStub);

            //---------------Assert Precondition----------------
            Assert.IsTrue(authorisationStub.IsAuthorised(BusinessObjectActions.CanCreate));

            //---------------Execute Test ----------------------
            string message;
            bool isCreatable = customer.IsCreatable(out message);

            //---------------Test Result -----------------------
            Assert.IsTrue(isCreatable);
            Assert.AreEqual("", message);
        }
        [Test]
        public void Test_BusinessObjectAuthorisation_AllowCreate_False()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
            Customer customer = new Customer();
            customer.SetAuthorisation(authorisationStub);
            //---------------Assert Precondition----------------
            Assert.IsFalse(authorisationStub.IsAuthorised(BusinessObjectActions.CanCreate));
            //---------------Execute Test ----------------------
            string message;
            bool isCreatable = customer.IsCreatable(out message);

            //---------------Test Result -----------------------
            Assert.IsFalse(isCreatable);
            StringAssert.Contains("The logged on user", message);
            StringAssert.Contains("is not authorised to create ", message);
        }

        [Test]
        public void Test_SaveNewBO_AllowCreate_True()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanCreate_True();

            Customer customer = CreateNewCustomerValid();
            customer.SetAuthorisation(authorisationStub);

            //---------------Assert Precondition----------------
            Assert.IsTrue(authorisationStub.IsAuthorised(BusinessObjectActions.CanCreate));

            //---------------Execute Test ----------------------
            customer.Save();
            //---------------Test Result -----------------------
        }

        [Test]
        public void Test_SaveNewBO_Fail_AllowCreate_False()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanCreate_False();

            Customer customer = CreateNewCustomerValid();
            customer.SetAuthorisation(authorisationStub);

            //---------------Assert Precondition----------------
            Assert.IsFalse(authorisationStub.IsAuthorised(BusinessObjectActions.CanCreate));

            //---------------Execute Test ----------------------
            try
            {
                customer.Save();
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (BusObjPersistException ex)
            {
                StringAssert.Contains("The logged on user", ex.Message);
                StringAssert.Contains("is not authorised to create ", ex.Message);
            }
        }

        private static IBusinessObjectAuthorisation GetAuthorisationStub_CanCreate_True()
        {
            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanCreate);
            return authorisationStub;
        }

        private static IBusinessObjectAuthorisation GetAuthorisationStub_CanCreate_False()
        {
            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
            return authorisationStub;
        }
        #endregion /TestDelete

        #region TestUpdate
        [Test]
        public void Test_BusinessObjectAuthorisation_AllowUpdate()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanUpdate_True();
            Customer customer = new Customer();
            customer.SetAuthorisation(authorisationStub);

            //---------------Assert Precondition----------------
            Assert.IsTrue(authorisationStub.IsAuthorised(BusinessObjectActions.CanUpdate));

            //---------------Execute Test ----------------------
            string message;
            bool isEditable = customer.IsEditable(out message);

            //---------------Test Result -----------------------
            Assert.IsTrue(isEditable);
            Assert.AreEqual("", message);
        }

        [Test]
        public void Test_BusinessObjectAuthorisation_AllowUpdate_False()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanUpdate_False();
            Customer customer = new Customer();
            customer.SetAuthorisation(authorisationStub);
            //---------------Assert Precondition----------------
            Assert.IsFalse(authorisationStub.IsAuthorised(BusinessObjectActions.CanUpdate));
            //---------------Execute Test ----------------------
            string message;
            bool isEditable = customer.IsEditable(out message);

            //---------------Test Result -----------------------
            Assert.IsFalse(isEditable);
            StringAssert.Contains("The logged on user", message);
            StringAssert.Contains("is not authorised to update ", message);
        }

        [Test]
        public void Test_SetValue_AllowUpdate_True()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanUpdate_True();

            Customer customer = CreateNewCustomerValid();
            customer.SetAuthorisation(authorisationStub);
            customer.Save();

            //---------------Assert Precondition----------------
            Assert.IsTrue(authorisationStub.IsAuthorised(BusinessObjectActions.CanUpdate));
            Assert.IsFalse(customer.Status.IsNew);
            Assert.IsFalse(customer.Status.IsDirty);

            //---------------Execute Test ----------------------
            const string newPropValue = "1112";
            customer.CustomerName = newPropValue;

            //---------------Test Result -----------------------
            Assert.IsTrue(customer.Status.IsDirty);
            Assert.AreEqual(newPropValue, customer.CustomerName);
        }

        [Test]
        public void Test_SetValue_Fail_AllowUpdate_False()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanUpdate_False();

            Customer customer = CreateNewCustomerValid();
            customer.SetAuthorisation(authorisationStub);
            customer.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(authorisationStub.IsAuthorised(BusinessObjectActions.CanUpdate));
            Assert.IsFalse(customer.Status.IsNew);
            Assert.IsFalse(customer.Status.IsDirty);

            //---------------Execute Test ----------------------
            const string newPropValue = "1112";
            try
            {
                customer.CustomerName = newPropValue;
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (BusObjEditableException ex)
            {
                StringAssert.Contains("You cannot Edit ", ex.Message);
                StringAssert.Contains("as the IsEditable is set to false for the object", ex.Message);
            }
        }

        [Test]
        public void Test_SaveExistingBO_AllowUpdate_True()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanUpdate_True();

            Customer customer = CreateNewCustomerValid();
            customer.SetAuthorisation(authorisationStub);
            customer.Save();
            customer.CustomerName = "c11123";

            //---------------Assert Precondition----------------
            Assert.IsTrue(authorisationStub.IsAuthorised(BusinessObjectActions.CanUpdate));
            Assert.IsFalse(customer.Status.IsNew);
            Assert.IsTrue(customer.Status.IsDirty);

            //---------------Execute Test ----------------------
            customer.Save();

            //---------------Test Result -----------------------
            Assert.IsFalse(customer.Status.IsDirty);
        }

        [Test]
        public void Test_SaveExistingBO_Fail_AllowUpdate_False()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanUpdate_False();

            Customer customer = CreateNewCustomerValid();
            
            customer.Save();
            customer.CustomerName = "cc1112";
            customer.SetAuthorisation(authorisationStub);
            //---------------Assert Precondition----------------
            Assert.IsFalse(authorisationStub.IsAuthorised(BusinessObjectActions.CanUpdate));
//            Assert.IsTrue(prop1Prop.IsDirty);
            Assert.IsTrue(customer.Status.IsDirty);

            //---------------Execute Test ----------------------
            try
            {
                customer.Save();
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (BusObjPersistException ex)
            {
                StringAssert.Contains("The logged on user", ex.Message);
                StringAssert.Contains("is not authorised to update ", ex.Message);
            }
        }

        private static IBusinessObjectAuthorisation GetAuthorisationStub_CanUpdate_True()
        {
            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanUpdate);
            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanCreate);
            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanRead);
            return authorisationStub;
        }

        private static IBusinessObjectAuthorisation GetAuthorisationStub_CanUpdate_False()
        {
            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanCreate);
            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanRead);
            return authorisationStub;
        }
        #endregion /TestDelete

        #region TestRead
        [Test]
        public void Test_BusinessObjectAuthorisation_AllowRead()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanRead);
            Customer customer = new Customer();
            customer.SetAuthorisation(authorisationStub);

            //---------------Assert Precondition----------------
            Assert.IsTrue(authorisationStub.IsAuthorised(BusinessObjectActions.CanRead));

            //---------------Execute Test ----------------------
            string message;
            bool isReadable = customer.IsReadable(out message);

            //---------------Test Result -----------------------
            Assert.IsTrue(isReadable);
            Assert.AreEqual("", message);
        }
        [Test]
        public void Test_BusinessObjectAuthorisation_AllowRead_False()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
            Customer customer = new Customer();
            customer.SetAuthorisation(authorisationStub);

            //---------------Assert Precondition----------------
            Assert.IsFalse(authorisationStub.IsAuthorised(BusinessObjectActions.CanRead));

            //---------------Execute Test ----------------------
            string message;
            bool isReadable = customer.IsReadable(out message);

            //---------------Test Result -----------------------
            Assert.IsFalse(isReadable);
            StringAssert.Contains("The logged on user", message);
            StringAssert.Contains("is not authorised to read ", message);
        }

        [Test]
        public void Test_LoadExistingBO_AllowRead_True()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanCreate_True();
            Customer customer = CreateNewCustomerValid();
            customer.SetAuthorisation(authorisationStub);
            customer.Save();

            authorisationStub = GetAuthorisationStub_CanRead_True();
            customer.SetAuthorisation(authorisationStub);
            IPrimaryKey id = customer.ID;
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Assert Precondition----------------
            Assert.IsTrue(authorisationStub.IsAuthorised(BusinessObjectActions.CanRead));
            Assert.IsFalse(customer.Status.IsNew);

            //---------------Execute Test ----------------------
            customer = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Customer>(id);
            object value = customer.CustomerName;
            //---------------Test Result -----------------------
            Assert.IsNotNull(value);
            Assert.IsFalse(customer.Status.IsDirty);
        }

        [Test]
        public void Test_LoadExistingBO_Fail_AllowRead_False()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanCreate_True();
            Customer customer = CreateNewCustomerValid();
            customer.SetAuthorisation(authorisationStub);
            customer.Save();

            authorisationStub = GetAuthorisationStub_CanRead_False();
            customer.SetAuthorisation(authorisationStub);
            IPrimaryKey id = customer.ID;
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Assert Precondition----------------
            Assert.IsFalse(authorisationStub.IsAuthorised(BusinessObjectActions.CanRead));
            Assert.IsFalse(customer.Status.IsNew);

            //---------------Execute Test ----------------------
            customer = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<Customer>(id);
            try
            {
                customer.GetPropertyValue("Prop1");
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (BusObjReadException ex)
            {
                StringAssert.Contains("The logged on user", ex.Message);
                StringAssert.Contains("is not authorised to read ", ex.Message);
            }
        }

        private static IBusinessObjectAuthorisation GetAuthorisationStub_CanRead_True()
        {
            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanRead);
            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanCreate);
            return authorisationStub;
        }
        private static IBusinessObjectAuthorisation GetAuthorisationStub_CanRead_False()
        {
            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
            return authorisationStub;
        }
        #endregion //TestRead

    }
    /// <summary>
    /// This class is a very simple stub implementation of IBusinessObjectAuthorisation.
    /// It is not intended to represent how a read authorisation policy would be implemented by is
    ///  instead used only for testing.
    /// </summary>
    internal class AuthorisationStub : IBusinessObjectAuthorisation
    {
        private readonly List<BusinessObjectActions> actionsAllowed = new List<BusinessObjectActions>();
        /// <summary>
        /// This is a public method to be used for setting up tests.
        /// </summary>
        /// <param name="role">the role that the user must be a member of</param>
        /// <param name="actionToPerform">The action that can be performed by a user of this role</param>
        public void AddAuthorisedRole(string role, BusinessObjectActions actionToPerform)
        {
            if (actionsAllowed.Contains(actionToPerform)) return;

            actionsAllowed.Add(actionToPerform);
        }
        /// <summary>
        /// Whether the user is authorised to perform this action or not.
        /// This is an implementation of the IBusinessObjectAuthorisation method.
        /// </summary>
        /// <param name="actionToPerform"></param>
        /// <returns></returns>
        public bool IsAuthorised(BusinessObjectActions actionToPerform)
        {
            return (actionsAllowed.Contains(actionToPerform));
        }
    }
}