//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Security
{
    [TestFixture]
    public class TestBOPropAuthorisation //:TestBase
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
        public void Test_BOPropAuthorisation_AllowUpdate_True()
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

        [Test]
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
            string message;
            Assert.IsFalse(prop1.IsEditable(out message));

            //---------------Execute Test ----------------------
            const string newPropValue = "1112";
            try
            {
                prop1.Value = newPropValue;
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (BOPropWriteException ex)
            {
                StringAssert.Contains("The logged on user  is not authorised to update the Prop1 ", ex.Message);
            }
        }

        [Test]
        public void Test_SetValue_Fail_ReadOnly_False()
        {
            //---------------Set up test pack-------------------
            MyBoAuthenticationStub.LoadDefaultClassDef_ReadOnlyProp1();

            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
            BOProp prop1 = (BOProp)myBoStub.Props["Prop1"];

            //---------------Assert Precondition----------------
            string message;
            Assert.IsFalse(prop1.IsEditable(out message));

            //---------------Execute Test ----------------------
            const string newPropValue = "1112";
            try
            {
                prop1.Value = newPropValue;
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (BOPropWriteException ex)
            {
                StringAssert.Contains("The property 'Prop 1' is not editable since it is set up as ReadOnly", ex.Message);
            }
        }

        [Test]
        public void Test_SetPropertyValue_Fail_AllowUpdate_False()
        {
            //---------------Set up test pack-------------------
            MyBoAuthenticationStub.LoadDefaultClassDef();
            IBOPropAuthorisation propAuthorisationStub = GetPropAuthorisationStub_CanUpdate_False();

            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
            BOProp prop1 = (BOProp)myBoStub.Props["Prop1"];
            prop1.SetAuthorisationRules(propAuthorisationStub);

            //---------------Assert Precondition----------------
            Assert.IsFalse(propAuthorisationStub.IsAuthorised(BOPropActions.CanUpdate));
            string message;
            Assert.IsFalse(prop1.IsEditable(out message));

            //---------------Execute Test ----------------------
            const string newPropValue = "1112";
            try
            {
                myBoStub.SetPropertyValue("Prop1", newPropValue);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (BOPropWriteException ex)
            {
                StringAssert.Contains("The logged on user  is not authorised to update the Prop1 ", ex.Message);
            }
        }

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

        [Test]
        public void Test_BOPropAuthorisation_AllowRead_True()
        {
            //---------------Set up test pack-------------------
            MyBoAuthenticationStub.LoadDefaultClassDef();
            IBOPropAuthorisation propAuthorisationStub = GetPropAuthorisationStub_CanRead_True();
            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
            BOProp prop1 = (BOProp)myBoStub.Props["Prop1"];
            prop1.SetAuthorisationRules(propAuthorisationStub);

            //---------------Assert Precondition----------------
            propAuthorisationStub.IsAuthorised(BOPropActions.CanRead);

            //---------------Execute Test ----------------------
            string message;
            bool isReadable = prop1.IsReadable(out message);

            //---------------Test Result -----------------------
            Assert.IsTrue(isReadable);
            Assert.AreEqual("", message);
        }

        [Test]
        public void Test_BOPropAuthorisation_AllowRead_False()
        {
            //---------------Set up test pack-------------------
            MyBoAuthenticationStub.LoadDefaultClassDef();
            IBOPropAuthorisation propAuthorisationStub = GetPropAuthorisationStub_CanRead_False();
            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
            BOProp prop1 = (BOProp)myBoStub.Props["Prop1"];
            prop1.SetAuthorisationRules(propAuthorisationStub);

            //---------------Assert Precondition----------------
            Assert.IsFalse(propAuthorisationStub.IsAuthorised(BOPropActions.CanRead));

            //---------------Execute Test ----------------------
            string message;
            bool isReadable = prop1.IsReadable(out message);

            //---------------Test Result -----------------------
            Assert.IsFalse(isReadable);
            StringAssert.Contains("The logged on user", message);
            StringAssert.Contains(" is not authorised to read ", message);
        }

        [Test]
        public void Test_GetValue_AllowRead_True()
        {
            //---------------Set up test pack-------------------
            MyBoAuthenticationStub.LoadDefaultClassDef();
            IBOPropAuthorisation propAuthorisationStub = GetPropAuthorisationStub_CanRead_True();
            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
            BOProp prop1 = (BOProp)myBoStub.Props["Prop1"];
            prop1.SetAuthorisationRules(propAuthorisationStub);
            myBoStub.Save();

            //---------------Assert Precondition----------------
            Assert.IsTrue(propAuthorisationStub.IsAuthorised(BOPropActions.CanRead));
            string message;
            Assert.IsTrue(prop1.IsReadable(out message));

            //---------------Execute Test ----------------------
            const string newPropValue = "1112";
            prop1.Value = newPropValue;

            //---------------Test Result -----------------------
            Assert.IsTrue(prop1.IsDirty);
            Assert.AreEqual(newPropValue, prop1.Value);
        }
#pragma warning disable 168
        [Test]
        public void Test_GetValue_Fail_AllowRead_False()
        {
            //---------------Set up test pack-------------------
            MyBoAuthenticationStub.LoadDefaultClassDef();
            IBOPropAuthorisation propAuthorisationStub = GetPropAuthorisationStub_CanRead_False();

            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
            BOProp prop1 = (BOProp)myBoStub.Props["Prop1"];
            prop1.SetAuthorisationRules(propAuthorisationStub);

            //---------------Assert Precondition----------------
            Assert.IsFalse(propAuthorisationStub.IsAuthorised(BOPropActions.CanRead));
            string message;
            Assert.IsFalse(prop1.IsReadable(out message));

            //---------------Execute Test ----------------------
            try
            {
                object value = prop1.Value;
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (BOPropReadException ex)
            {
                StringAssert.Contains("The logged on user  is not authorised to read the Prop1 ", ex.Message);
            }
        }
#pragma warning restore 168

        [Test]
        public void Test_BO_GetPropertyValue_Fail_AllowRead_False()
        {
            //---------------Set up test pack-------------------
            MyBoAuthenticationStub.LoadDefaultClassDef();
            IBOPropAuthorisation propAuthorisationStub = GetPropAuthorisationStub_CanRead_False();

            MyBoAuthenticationStub myBoStub = new MyBoAuthenticationStub();
            BOProp prop1 = (BOProp)myBoStub.Props["Prop1"];
            prop1.SetAuthorisationRules(propAuthorisationStub);

            //---------------Assert Precondition----------------
            Assert.IsFalse(propAuthorisationStub.IsAuthorised(BOPropActions.CanRead));
            string message;
            Assert.IsFalse(prop1.IsReadable(out message));

            //---------------Execute Test ----------------------
            try
            {
                myBoStub.GetPropertyValue("Prop1");
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (BOPropReadException ex)
            {
                StringAssert.Contains("The logged on user  is not authorised to read the Prop1 ", ex.Message);
            }
        }
        private static IBOPropAuthorisation GetPropAuthorisationStub_CanRead_True()
        {
            IBOPropAuthorisation authorisationStub = new BOPropAuthorisationStub();
            authorisationStub.AddAuthorisedRole("A Role", BOPropActions.CanUpdate);
            authorisationStub.AddAuthorisedRole("A Role", BOPropActions.CanRead);
            return authorisationStub;
        }

        private static IBOPropAuthorisation GetPropAuthorisationStub_CanRead_False()
        {
            IBOPropAuthorisation authorisationStub = new BOPropAuthorisationStub();
            return authorisationStub;
        }
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