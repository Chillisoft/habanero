using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.BO.ObjectManager;
using NUnit.Framework;

namespace Habanero.Test.BO.Security
{
    [TestFixture]
    public class TestTestBOPropAuthorisation //:TestBase
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
             ClassDef.ClassDefs.Clear();
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
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }

        #region TestUpdate
        //[Test]
        //public void Test_BusinessObjectAuthorisation_AllowUpdate()
        //{
        //    //---------------Set up test pack-------------------
        //    MyBoAuthenticationStub.LoadDefaultClassDef();
        //    IBOPropAuthorisation propAuthorisationStub = GetPropAuthorisationStub_CanUpdate_True();
        //    MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
        //    BOProp prop1
        //    myBoStub.SetAuthorisation(propAuthorisationStub);

        //    //---------------Assert Precondition----------------
        //    Assert.IsTrue(propAuthorisationStub.IsAuthorised(BusinessObjectActions.CanUpdate));

        //    //---------------Execute Test ----------------------
        //    string message;
        //    bool isEditable = myBoStub.IsEditable(out message);

        //    //---------------Test Result -----------------------
        //    Assert.IsTrue(isEditable);
        //    Assert.AreEqual("", message);
        //}
//
//        [Test]
//        public void Test_BusinessObjectAuthorisation_AllowUpdate_False()
//        {
//            //---------------Set up test pack-------------------
//            MyBoAuthenticationStub.LoadDefaultClassDef();
//            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanUpdate_False();
//            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
//            myBoStub.SetAuthorisation(authorisationStub);
//            //---------------Assert Precondition----------------
//            Assert.IsFalse(authorisationStub.IsAuthorised(BusinessObjectActions.CanUpdate));
//            //---------------Execute Test ----------------------
//            string message;
//            bool isEditable = myBoStub.IsEditable(out message);
//
//            //---------------Test Result -----------------------
//            Assert.IsFalse(isEditable);
//            StringAssert.Contains("The logged on user", message);
//            StringAssert.Contains("is not authorised to update ", message);
//        }
//
//        [Test]
//        public void Test_SetValue_AllowUpdate_True()
//        {
//            //---------------Set up test pack-------------------
//            MyBoAuthenticationStub.LoadDefaultClassDef();
//            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanUpdate_True();
//
//            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
//            myBoStub.SetAuthorisation(authorisationStub);
//            myBoStub.Save();
//
//            //---------------Assert Precondition----------------
//            Assert.IsTrue(authorisationStub.IsAuthorised(BusinessObjectActions.CanUpdate));
//            Assert.IsFalse(myBoStub.Status.IsNew);
//            Assert.IsFalse(myBoStub.Status.IsDirty);
//
//            //---------------Execute Test ----------------------
//            const string newPropValue = "1112";
//            myBoStub.SetPropertyValue("Prop1", newPropValue);
//
//            //---------------Test Result -----------------------
//            Assert.IsTrue(myBoStub.Status.IsDirty);
//            Assert.AreEqual(newPropValue, myBoStub.GetPropertyValue("Prop1"));
//        }
//
//        [Test]
//        public void Test_SetValue_Fail_AllowUpdate_False()
//        {
//            //---------------Set up test pack-------------------
//            MyBoAuthenticationStub.LoadDefaultClassDef();
//            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanUpdate_False();
//
//            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
//            myBoStub.SetAuthorisation(authorisationStub);
//            myBoStub.Save();
//
//            //---------------Assert Precondition----------------
//            Assert.IsFalse(authorisationStub.IsAuthorised(BusinessObjectActions.CanUpdate));
//            Assert.IsFalse(myBoStub.Status.IsNew);
//            Assert.IsFalse(myBoStub.Status.IsDirty);
//
//            //---------------Execute Test ----------------------
//            const string newPropValue = "1112";
//            try
//            {
//                myBoStub.SetPropertyValue("Prop1", newPropValue);
//                Assert.Fail("expected Err");
//            }
//            //---------------Test Result -----------------------
//            catch (BusObjEditableException ex)
//            {
//                StringAssert.Contains("You cannot Edit ", ex.Message);
//                StringAssert.Contains("as the IsEditable is set to false for the object", ex.Message);
//            }
//        }
//
//        [Test]
//        public void Test_SaveExistingBO_AllowUpdate_True()
//        {
//            //---------------Set up test pack-------------------
//            MyBoAuthenticationStub.LoadDefaultClassDef();
//            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanUpdate_True();
//
//            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
//            myBoStub.SetAuthorisation(authorisationStub);
//            myBoStub.Save();
//            myBoStub.SetPropertyValue("Prop1", "1112");
//
//            //---------------Assert Precondition----------------
//            Assert.IsTrue(authorisationStub.IsAuthorised(BusinessObjectActions.CanUpdate));
//            Assert.IsFalse(myBoStub.Status.IsNew);
//            Assert.IsTrue(myBoStub.Status.IsDirty);
//
//            //---------------Execute Test ----------------------
//            myBoStub.Save();
//
//            //---------------Test Result -----------------------
//            Assert.IsFalse(myBoStub.Status.IsDirty);
//        }
//
//        [Test]
//        public void Test_SaveExistingBO_Fail_AllowUpdate_False()
//        {
//            //---------------Set up test pack-------------------
//            MyBoAuthenticationStub.LoadDefaultClassDef();
//            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanUpdate_False();
//
//            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
//            myBoStub.SetAuthorisation(authorisationStub);
//            myBoStub.Save();
//            IBOProp prop1Prop = myBoStub.Props["Prop1"];
//            prop1Prop.Value = "1112";
//            myBoStub.SetStatus(BOStatus.Statuses.isDirty, true);
//
//            //---------------Assert Precondition----------------
//            Assert.IsFalse(authorisationStub.IsAuthorised(BusinessObjectActions.CanUpdate));
//            Assert.IsTrue(prop1Prop.IsDirty);
//            Assert.IsTrue(myBoStub.Status.IsDirty);
//
//            //---------------Execute Test ----------------------
//            try
//            {
//                myBoStub.Save();
//                Assert.Fail("expected Err");
//            }
//            //---------------Test Result -----------------------
//            catch (BusObjPersistException ex)
//            {
//                StringAssert.Contains("The logged on user", ex.Message);
//                StringAssert.Contains("is not authorised to update ", ex.Message);
//            }
//        }
//
//        private static IBusinessObjectAuthorisation GetAuthorisationStub_CanUpdate_True()
//        {
//            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
//            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanUpdate);
//            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanCreate);
//            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanRead);
//            return authorisationStub;
//        }
//
//        private static IBusinessObjectAuthorisation GetAuthorisationStub_CanUpdate_False()
//        {
//            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
//            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanCreate);
//            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanRead);
//            return authorisationStub;
//        }
        #endregion /TestDelete

        #region TestRead
//        [Test]
//        public void Test_BusinessObjectAuthorisation_AllowRead()
//        {
//            //---------------Set up test pack-------------------
//            MyBoAuthenticationStub.LoadDefaultClassDef();
//            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
//            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanRead);
//            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
//            myBoStub.SetAuthorisation(authorisationStub);
//
//            //---------------Assert Precondition----------------
//            Assert.IsTrue(authorisationStub.IsAuthorised(BusinessObjectActions.CanRead));
//
//            //---------------Execute Test ----------------------
//            string message;
//            bool isReadable = myBoStub.IsReadable(out message);
//
//            //---------------Test Result -----------------------
//            Assert.IsTrue(isReadable);
//            Assert.AreEqual("", message);
//        }
//        [Test]
//        public void Test_BusinessObjectAuthorisation_AllowRead_False()
//        {
//            //---------------Set up test pack-------------------
//            MyBoAuthenticationStub.LoadDefaultClassDef();
//            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
//            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
//            myBoStub.SetAuthorisation(authorisationStub);
//
//            //---------------Assert Precondition----------------
//            Assert.IsFalse(authorisationStub.IsAuthorised(BusinessObjectActions.CanRead));
//
//            //---------------Execute Test ----------------------
//            string message;
//            bool isReadable = myBoStub.IsReadable(out message);
//
//            //---------------Test Result -----------------------
//            Assert.IsFalse(isReadable);
//            StringAssert.Contains("The logged on user", message);
//            StringAssert.Contains("is not authorised to read ", message);
//        }
//
//        [Test]
//        public void Test_LoadExistingBO_AllowRead_True()
//        {
//            //---------------Set up test pack-------------------
//            MyBoAuthenticationStub.LoadDefaultClassDef();
//            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanCreate_True();
//            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
//            myBoStub.SetAuthorisation(authorisationStub);
//            myBoStub.Save();
//
//            authorisationStub = GetAuthorisationStub_CanRead_True();
//            myBoStub.SetAuthorisation(authorisationStub);
//            IPrimaryKey id = myBoStub.ID;
//            BusinessObjectManager.Instance.ClearLoadedObjects();
//
//            //---------------Assert Precondition----------------
//            Assert.IsTrue(authorisationStub.IsAuthorised(BusinessObjectActions.CanRead));
//            Assert.IsFalse(myBoStub.Status.IsNew);
//
//            //---------------Execute Test ----------------------
//            myBoStub = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<MyBoAuthenticationStub>(id);
//            object value = myBoStub.GetPropertyValue("Prop1");
//            //---------------Test Result -----------------------
//            Assert.IsNull(value);
//            Assert.IsFalse(myBoStub.Status.IsDirty);
//        }
//        [Test]
//        public void Test_LoadExistingBO_Fail_AllowRead_False()
//        {
//            //---------------Set up test pack-------------------
//            MyBoAuthenticationStub.LoadDefaultClassDef();
//            IBusinessObjectAuthorisation authorisationStub = GetAuthorisationStub_CanCreate_True();
//            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
//            myBoStub.SetAuthorisation(authorisationStub);
//            myBoStub.Save();
//
//            authorisationStub = GetAuthorisationStub_CanRead_False();
//            myBoStub.SetAuthorisation(authorisationStub);
//            IPrimaryKey id = myBoStub.ID;
//            BusinessObjectManager.Instance.ClearLoadedObjects();
//
//            //---------------Assert Precondition----------------
//            Assert.IsFalse(authorisationStub.IsAuthorised(BusinessObjectActions.CanRead));
//            Assert.IsFalse(myBoStub.Status.IsNew);
//
//            //---------------Execute Test ----------------------
//            myBoStub = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<MyBoAuthenticationStub>(id);
//            try
//            {
//                myBoStub.GetPropertyValue("Prop1");
//                Assert.Fail("expected Err");
//            }
//            //---------------Test Result -----------------------
//            catch (BusObjReadException ex)
//            {
//                StringAssert.Contains("The logged on user", ex.Message);
//                StringAssert.Contains("is not authorised to read ", ex.Message);
//            }
//        }
//
//        private static IBusinessObjectAuthorisation GetAuthorisationStub_CanRead_True()
//        {
//            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
//            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanRead);
//            authorisationStub.AddAuthorisedRole("A Role", BusinessObjectActions.CanCreate);
//            return authorisationStub;
//        }
//        private static IBusinessObjectAuthorisation GetAuthorisationStub_CanRead_False()
//        {
//            IBusinessObjectAuthorisation authorisationStub = new AuthorisationStub();
//            return authorisationStub;
//        }
        #endregion /TestRead
    }
    internal class MyBo_PropAuthenticationStub : BusinessObject
    {
        public static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBoAuthenticationStub"" assembly=""Habanero.Test.BO"">
					<property  name=""MyBoAuthenticationStubID"" type=""Guid""/>
                    <property  name=""Prop1""/>
					<primaryKey>
						<prop name=""MyBoAuthenticationStubID"" />
					</primaryKey>
					<ui>
					</ui>                   
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

    }

    internal class BOPropAuthorisationStub : IBOPropAuthorisation
    {
        private readonly List<BOPropActions> actionsAllowed = new List<BOPropActions>();

        public void AddAuthorisedRole(string authorisedRole, BOPropActions actionToPerform)
        {
            if (actionsAllowed.Contains(actionToPerform)) return;

            actionsAllowed.Add(actionToPerform);
        }

        public bool IsAuthorised(BOPropActions actionToPerform)
        {
            return (actionsAllowed.Contains(actionToPerform));
        }
    }
}