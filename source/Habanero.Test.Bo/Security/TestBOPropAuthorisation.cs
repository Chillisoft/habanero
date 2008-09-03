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
            BORegistry.DataAccessor = new DataAccessorInMemory();
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

        [Test]
        public void Test_ReadOnly_IsEditable_False()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("Name","NN", "String", PropReadWriteRule.ReadOnly, "DD", "", false, false);
            BOProp prop1 = new BOProp(propDef);

            //---------------Assert Precondition----------------
            Assert.AreEqual(PropReadWriteRule.ReadOnly, prop1.PropDef.ReadWriteRule);

            //---------------Execute Test ----------------------
            string message;
            bool isEditable = prop1.IsEditable(out message);

            //---------------Test Result -----------------------
            Assert.IsFalse(isEditable);
            StringAssert.Contains("The property 'Name' is not editable since it is set up as ReadOnly", message);
        }

        [Test]
        public void Test_WriteNew_NewObject_IsEditable_True()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("Name", "NN", "String", PropReadWriteRule.WriteNew, "DD", "", false, false);
            BOProp prop1 = new BOProp(propDef);
            prop1.IsObjectNew = true;

            //---------------Assert Precondition----------------
            Assert.AreEqual(PropReadWriteRule.WriteNew, prop1.PropDef.ReadWriteRule);
            Assert.IsTrue(prop1.IsObjectNew);

            //---------------Execute Test ----------------------
            string message;
            bool isEditable = prop1.IsEditable(out message);

            //---------------Test Result -----------------------
            Assert.IsTrue(isEditable);
            Assert.AreEqual("", message);
        }

        [Test]
        public void Test_WriteNew_SavedObject_IsEditable_False()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("Name", "NN", "String", PropReadWriteRule.WriteNew, "DD", "", false, false);
            BOProp prop1 = new BOProp(propDef);
            prop1.IsObjectNew = false;

            //---------------Assert Precondition----------------
            Assert.AreEqual(PropReadWriteRule.WriteNew, prop1.PropDef.ReadWriteRule);
            Assert.IsFalse(prop1.IsObjectNew);

            //---------------Execute Test ----------------------
            string message;
            bool isEditable = prop1.IsEditable(out message);

            //---------------Test Result -----------------------
            Assert.IsFalse(isEditable);
            StringAssert.Contains("The property 'Name' is not editable since it is set up as WriteNew and the object is not new", message);
        }

        [Test]
        public void Test_WriteNotNew_SavedObject_IsEditable_True()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("Name", "NN", "String", PropReadWriteRule.WriteNotNew, "DD", "", false, false);
            BOProp prop1 = new BOProp(propDef);
            prop1.IsObjectNew = false;

            //---------------Assert Precondition----------------
            Assert.AreEqual(PropReadWriteRule.WriteNotNew, prop1.PropDef.ReadWriteRule);
            Assert.IsFalse(prop1.IsObjectNew);

            //---------------Execute Test ----------------------
            string message;
            bool isEditable = prop1.IsEditable(out message);

            //---------------Test Result -----------------------
            Assert.IsTrue(isEditable);
            Assert.AreEqual("", message);
        }

        [Test]
        public void Test_WriteNotNew_NewObject_IsEditable_False()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("Name", "NN", "String", PropReadWriteRule.WriteNotNew, "DD", "", false, false);
            BOProp prop1 = new BOProp(propDef);
            prop1.IsObjectNew = true;

            //---------------Assert Precondition----------------
            Assert.AreEqual(PropReadWriteRule.WriteNotNew, prop1.PropDef.ReadWriteRule);
            Assert.IsTrue(prop1.IsObjectNew);

            //---------------Execute Test ----------------------
            string message;
            bool isEditable = prop1.IsEditable(out message);

            //---------------Test Result -----------------------
            Assert.IsFalse(isEditable);
            StringAssert.Contains("The property 'Name' is not editable since it is set up as WriteNew and the object is new", message);
        }

        [Test]
        public void Test_WriteOnce_PersistedValueSet_IsEditable_False()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("Name", typeof(string), PropReadWriteRule.WriteOnce, "DD", "", false, false);
            BOProp prop1 = new BOProp(propDef);
            prop1.Value = "new Value";
            prop1.IsObjectNew = false;
            prop1.BackupPropValue();

            //---------------Assert Precondition----------------
            Assert.AreEqual(PropReadWriteRule.WriteOnce, prop1.PropDef.ReadWriteRule);
            Assert.IsNotNull(prop1.PersistedPropertyValue);

            //---------------Execute Test ----------------------
            string message;
            bool isEditable = prop1.IsEditable(out message);

            //---------------Test Result -----------------------
            Assert.IsFalse(isEditable);
            StringAssert.Contains("The property 'Name' is not editable since it is set up as WriteOnce and the value has already been set", message);
        }

        [Test]
        public void Test_WriteOnce_PersistedValueNotSet_IsEditable_True()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("Name", typeof(string), PropReadWriteRule.WriteOnce, "DD", "", false, false);
            BOProp prop1 = new BOProp(propDef);
            prop1.Value = "new Value";
            prop1.IsObjectNew = false;

            //---------------Assert Precondition----------------
            Assert.AreEqual(PropReadWriteRule.WriteOnce, prop1.PropDef.ReadWriteRule);
            Assert.IsNull(prop1.PersistedPropertyValue);

            //---------------Execute Test ----------------------
            string message;
            bool isEditable = prop1.IsEditable(out message);

            //---------------Test Result -----------------------
            Assert.IsTrue(isEditable);
            Assert.AreEqual("", message);
        }

        [Test]
        public void Test_WriteOnce_NewObject_IsEditable_True()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("Name", typeof(string), PropReadWriteRule.WriteOnce, "DD", "", false, false);
            BOProp prop1 = new BOProp(propDef);
            prop1.Value = "new Value";
            prop1.IsObjectNew = true;
            prop1.BackupPropValue();

            //---------------Assert Precondition----------------
            Assert.AreEqual(PropReadWriteRule.WriteOnce, prop1.PropDef.ReadWriteRule);
            Assert.IsNotNull(prop1.PersistedPropertyValue);

            //---------------Execute Test ----------------------
            string message;
            bool isEditable = prop1.IsEditable(out message);

            //---------------Test Result -----------------------
            Assert.IsTrue(isEditable);
            Assert.AreEqual("", message);
        }

        [Test]
        public void Test_BOPropAuthorisation_AllowUpdate()
        {
            //---------------Set up test pack-------------------
            MyBoAuthenticationStub.LoadDefaultClassDef();
            IBOPropAuthorisation propAuthorisationStub = GetPropAuthorisationStub_CanUpdate_True();
            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
            BOProp prop1 = (BOProp) myBoStub.Props["Prop1"];
            prop1.SetAuthorisationRules(propAuthorisationStub);

            //---------------Assert Precondition----------------
            propAuthorisationStub.IsAuthorised(BOPropActions.CanUpdate);

            //---------------Execute Test ----------------------
            string message;
            bool isEditable = prop1.IsEditable(out message);

            //---------------Test Result -----------------------
            Assert.IsTrue(isEditable);
            Assert.AreEqual("", message);
        }

        [Test]
        public void Test_BOPropAuthorisation_AllowUpdate_False()
        {
            //---------------Set up test pack-------------------
            MyBoAuthenticationStub.LoadDefaultClassDef();
            IBOPropAuthorisation propAuthorisationStub = GetPropAuthorisationStub_CanUpdate_False();
            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
            BOProp prop1 = (BOProp)myBoStub.Props["Prop1"];
            prop1.SetAuthorisationRules(propAuthorisationStub);

            //---------------Assert Precondition----------------
            Assert.IsFalse(propAuthorisationStub.IsAuthorised(BOPropActions.CanUpdate));
            //---------------Execute Test ----------------------
            string message;
            bool isEditable = prop1.IsEditable(out message);

            //---------------Test Result -----------------------
            Assert.IsFalse(isEditable);
            StringAssert.Contains("The logged on user", message);
            StringAssert.Contains(" is not authorised to update ", message);
        }

        [Test]
        public void Test_SetValue_AllowUpdate_True()
        {
            //---------------Set up test pack-------------------
            MyBoAuthenticationStub.LoadDefaultClassDef();
            IBOPropAuthorisation propAuthorisationStub = GetPropAuthorisationStub_CanUpdate_True();
            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
            BOProp prop1 = (BOProp)myBoStub.Props["Prop1"];
            prop1.SetAuthorisationRules(propAuthorisationStub);
            myBoStub.Save();

            //---------------Assert Precondition----------------
            Assert.IsTrue(propAuthorisationStub.IsAuthorised(BOPropActions.CanUpdate));

            //---------------Execute Test ----------------------
            const string newPropValue = "1112";
            prop1.Value = newPropValue;

            //---------------Test Result -----------------------
            Assert.IsTrue(prop1.IsDirty);
            Assert.AreEqual(newPropValue, prop1.Value);
        }

        [Test, Ignore("Currently working on this")]
        public void Test_SetValue_Fail_AllowUpdate_False()
        {
            //---------------Set up test pack-------------------
            MyBoAuthenticationStub.LoadDefaultClassDef();
            IBOPropAuthorisation propAuthorisationStub = GetPropAuthorisationStub_CanUpdate_False();

            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
            BOProp prop1 = (BOProp)myBoStub.Props["Prop1"];
            prop1.SetAuthorisationRules(propAuthorisationStub);
            myBoStub.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(propAuthorisationStub.IsAuthorised(BOPropActions.CanUpdate));
            Assert.IsFalse(myBoStub.Status.IsNew);

            //---------------Execute Test ----------------------
            const string newPropValue = "1112";
            try
            {
                prop1.Value = newPropValue;
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (BusinessObjectReadWriteRuleException ex)
            {
                StringAssert.Contains("You cannot Edit ", ex.Message);
                StringAssert.Contains("as the IsEditable is set to false for the object", ex.Message);
            }
        }
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
        private static IBOPropAuthorisation GetPropAuthorisationStub_CanUpdate_True()
        {
            IBOPropAuthorisation authorisationStub = new BOPropAuthorisationStub();
            authorisationStub.AddAuthorisedRole("A Role", BOPropActions.CanUpdate);
            authorisationStub.AddAuthorisedRole("A Role", BOPropActions.CanRead);
            return authorisationStub;
        }

        private static IBOPropAuthorisation GetPropAuthorisationStub_CanUpdate_False()
        {
            IBOPropAuthorisation authorisationStub = new BOPropAuthorisationStub();
            authorisationStub.AddAuthorisedRole("A Role", BOPropActions.CanRead);
            return authorisationStub;
        }
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