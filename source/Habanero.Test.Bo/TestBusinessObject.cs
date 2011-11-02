// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO.ClassDefinition;
using Habanero.Test.Structure;
using NUnit.Framework;
using Rhino.Mocks;
// ReSharper disable InconsistentNaming
namespace Habanero.Test.BO
{

    [TestFixture]
    public class TestBusinessObject_InMemory : TestBusinessObject
    {
        [TestFixtureSetUp]
        public override void TestFixtureSetup()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [SetUp]
        public override void SetupTest()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();
            ClassDef.ClassDefs.Clear();
            new Address();
        }


        [Test]
        public void Test_ToString_WhenHasObjectID_ShouldReturnObjectIDToString()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            Car myBO = new Car();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string toString = myBO.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO.ID.GetAsValue().ToString(), toString);
        }
        [Test]
        public void Test_ToString_WhenHasCompositePrimaryKey_AndValueSet_ShouldReturnAggregateOfKeyProps()
        {
            //---------------Set up test pack-------------------
            BOWithCompositePK.LoadClassDefs();
            var myBO = new BOWithCompositePK();
            const string pk1Prop1Value = "somevalue";
            const string pk1Prop2Value = "anothervalue";
            myBO.PK1Prop1 = pk1Prop1Value;
            myBO.PK1Prop2 = pk1Prop2Value;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(myBO.PK1Prop1);
            Assert.IsNotNull(myBO.PK1Prop2);
            //---------------Execute Test ----------------------
            string actualToString = myBO.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO.ID.ToString(), actualToString);
        }
        [Test]
        public void Test_ToString_WhenHasCompositePrimaryKey_AndValueSet_ShouldReturnGuidIdToString()
        {
            //---------------Set up test pack-------------------
            BOWithCompositePK.LoadClassDefs();
            var myBO = new BOWithCompositePK();
            //---------------Assert Precondition----------------
            Assert.IsNullOrEmpty(myBO.PK1Prop1);
            Assert.IsNullOrEmpty(myBO.PK1Prop2);
            //---------------Execute Test ----------------------
            string actualToString = myBO.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO.ID.GetAsGuid().ToString(), actualToString);
        }
        
        [Test]
        public void Test_ToString_WhenHasStringPKProp_AndValueSet_ShouldReturnTheSingleValueAsAToString()
        {
            //---------------Set up test pack-------------------
            BOWithStringPKProp.LoadClassDefs();
            var myBO = new BOWithStringPKProp();
            const string pk1Prop1Value = "somevalue";
            myBO.PK1Prop1 = pk1Prop1Value;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(myBO.PK1Prop1);
            //---------------Execute Test ----------------------
            string actualToString = myBO.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(pk1Prop1Value, actualToString);
        }
        [Test]
        public void Test_ToString_WhenHasStringPKProp_AndValueSet_ShouldReturnGuidIDToString()
        {
            //---------------Set up test pack-------------------
            BOWithStringPKProp.LoadClassDefs();
            var myBO = new BOWithStringPKProp();
            //---------------Assert Precondition----------------
            Assert.IsNullOrEmpty(myBO.PK1Prop1);
            //---------------Execute Test ----------------------
            string actualToString = myBO.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO.ID.GetAsGuid().ToString(), actualToString);
        }       
        [Test]
        public void Test_ToString_WhenHasIntPKProp_AndValueSet_ShouldReturnTheSingleValueAsAToString()
        {
            //---------------Set up test pack-------------------
            BOWithIntPKProp.LoadClassDefs();
            var myBO = new BOWithIntPKProp();
            int pk1Prop1Value = RandomValueGen.GetRandomInt();
            myBO.PK1Prop1 = pk1Prop1Value;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(myBO.PK1Prop1);
            //---------------Execute Test ----------------------
            string actualToString = myBO.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(pk1Prop1Value.ToString(), actualToString);
        }
        [Test]
        public void Test_ToString_WhenHasIntPKProp_AndValueSet_ShouldReturnGuidIDToString()
        {
            //---------------Set up test pack-------------------
            BOWithIntPKProp.LoadClassDefs();
            var myBO = new BOWithIntPKProp();
            //---------------Assert Precondition----------------
            Assert.IsNull(myBO.PK1Prop1);
            //---------------Execute Test ----------------------
            string actualToString = myBO.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO.ID.GetAsGuid().ToString(), actualToString);
        }

    }


    /// <summary>
    /// Summary description for TestBusinessObject.
    /// </summary>
    [TestFixture]
    public class TestBusinessObject
    {
        [TestFixtureSetUp]
        public virtual void TestFixtureSetup()
        {
            TestUsingDatabase.SetupDBDataAccessor();
        }

        [TearDown]
        public void TearDown()
        {
            ContactPersonTestBO.DeleteAllContactPeople();
        }

        [SetUp]
        public virtual void SetupTest()
        {
            TestUsingDatabase.SetupDBDataAccessor();
            BORegistry.DataAccessor = new DataAccessorDB();
            ClassDef.ClassDefs.Clear();
            new Address();
        }

        [Test]
        public void TestInstantiate()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            bo.GetPropertyValueString("TestProp");
        }


        [Test]
        public void Test_Instantiate_SetsDefaults()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            string testPropDefault = TestUtil.GetRandomString();
            MyBO.LoadDefaultClassDefWithDefault(testPropDefault);
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            MyBO bo = new MyBO();
            //---------------Test Result -----------------------
            Assert.AreEqual(testPropDefault, bo.TestProp);
        }

        [Test]
        public void Test_Instantiate_DefaultValuesAreBackedUp()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            string testPropDefault = TestUtil.GetRandomString();
            MyBO.LoadDefaultClassDefWithDefault(testPropDefault);
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            MyBO bo = new MyBO();
            //---------------Test Result -----------------------
            Assert.AreEqual(testPropDefault, bo.Props["TestProp"].PersistedPropertyValue);
            Assert.AreEqual(null, bo.Props["TestProp2"].PersistedPropertyValue);
        }
        [Test]
        public void Test_Instantiate_ShouldSetBusinessObjectOnPrimaryKey()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            string testPropDefault = TestUtil.GetRandomString();
            MyBO.LoadDefaultClassDefWithDefault(testPropDefault);
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            MyBO bo = new MyBO();
            //---------------Test Result -----------------------
            Assert.AreSame(bo, bo.ID.BusinessObject);
        }

        [Test]
        public void Test_Instantiate_NewObjectIdIsBackedUp()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            string testPropDefault = TestUtil.GetRandomString();
            MyBO.LoadDefaultClassDefWithDefault(testPropDefault);
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            MyBO bo = new MyBO();
            //---------------Test Result -----------------------
            Assert.AreEqual(bo.MyBoID, bo.Props["MyBoID"].PersistedPropertyValue);
            Assert.IsTrue(bo.Props["MyBoID"].IsObjectNew);
        }

        [Test]
        public void Test_Instantiate_NewObjectIdRemainsAfterCancelEdit()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            string testPropDefault = TestUtil.GetRandomString();
            MyBO.LoadDefaultClassDefWithDefault(testPropDefault);
            MyBO bo = new MyBO();
            Guid id = bo.MyBoID.GetValueOrDefault();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            bo.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(id, bo.MyBoID);
        }

        //[Test]
        //public void TestIndexer()
        //{
        //    MyBO bo = new MyBO();
        //    bo["TestProp"] = "hello";
        //    Assert.AreEqual("hello", bo.GetPropertyValue("TestProp"));
        //}

        [Test]
        public void TestSettingLookupValueSetsGuid()
        {
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithLookup();
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", "s1");
            Assert.AreEqual("s1", bo.GetPropertyValueToDisplay("TestProp2"));
            Assert.AreEqual(new Guid("{E6E8DC44-59EA-4e24-8D53-4A43DC2F25E7}"), bo.GetPropertyValue("TestProp2"));
        }        
        
        [Test]
        public void Test_ChangeObjectID_FiresIDUpdatedEvent()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO bo = ContactPersonTestBO.CreateUnsavedContactPerson();
            bool updatedEventFired = false;
            bo.IDUpdated += ((sender, e) => updatedEventFired = true);
            //---------------Assert Precondition----------------
            Assert.IsFalse(updatedEventFired);
            //---------------Execute Test ----------------------
            bo.ContactPersonID = Guid.NewGuid();
            //---------------Test Result -----------------------
            Assert.IsTrue(updatedEventFired);
        }

        [Test]
        public void Test_BusinessObject_WithBrokenRules_ValidUntil_PropsIsValidCalled()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            IBOProp boProp = bo.Props["TestProp"];
            //---------------Assert Precondition----------------
            Assert.IsTrue(boProp.IsValid);
            //---------------Execute Test ----------------------
            string invalidReason;
            bo.Props.IsValid(out invalidReason);
            //---------------Test Result -----------------------
            StringAssert.Contains("Test Prop' is a compulsory field and has no value", boProp.InvalidReason);
            Assert.IsFalse(boProp.IsValid);
            StringAssert.Contains("Test Prop' is a compulsory field and has no value", invalidReason);
        }

        [Test]
        public void Test_BusinessObject_WithBrokenRules_ValidUntil_AfterBOProp_IsValidCalled()
        {
            //For performance reasons it was decided to not run the validation code 
            // every time Isvalid is called on the boProp instead the boProp has a validate method
            // which runs the validation code and sets the valid message and status on the BOProp.
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            IBOProp boProp = bo.Props["TestProp"];
            //---------------Assert Precondition----------------
            Assert.IsTrue(boProp.IsValid);
            //---------------Execute Test ----------------------
            bool valid = boProp.IsValid;
            //---------------Test Result -----------------------
            Assert.AreEqual("", boProp.InvalidReason);
            Assert.IsTrue(valid);
        }

        [Test]
        public void Test_BusinessObject_WithBrokenRules_ValidUntilIsValidCalled()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            IBOProp boProp = bo.Props["TestProp"];
            //---------------Assert Precondition----------------
            Assert.IsTrue(boProp.IsValid);
            //---------------Execute Test ----------------------
            bool isValid = bo.Status.IsValid();
            //---------------Test Result -----------------------
            StringAssert.Contains("Test Prop' is a compulsory field and has no value", boProp.InvalidReason);
            Assert.IsFalse(boProp.IsValid);
            StringAssert.Contains("Test Prop' is a compulsory field and has no value", bo.Status.IsValidMessage);
            Assert.IsFalse(isValid);
        }


        [Test]
        public void Test_IsValid_Valid_EmptyErrorDescriptions()
        {
            //--------------- Set up test pack ------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            IList<IBOError> errors;
            bool isValid = myBO.Status.IsValid(out errors);
            //--------------- Test Result -----------------------
            Assert.IsTrue(isValid);
            Assert.AreEqual(0, errors.Count);
        }


        [Test]
        public void Test_IsValid_Errors()
        {
            //--------------- Set up test pack ------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            MyBO myBO = new MyBO();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            IList<IBOError> errors;
            bool isValid = myBO.Status.IsValid(out errors);
            //--------------- Test Result -----------------------
            Assert.IsFalse(isValid);
            Assert.AreEqual(1, errors.Count);
            StringAssert.Contains("Test Prop' is a compulsory field and has no value", errors[0].Message);
            Assert.AreEqual(ErrorLevel.Error, errors[0].Level);
            Assert.AreSame(myBO, errors[0].BusinessObject);
        }

        [Test]
        public void Test_StatusIsDirty_WhenUpdateBOPropValue_ShouldBeSetToDirty()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            BusinessObject bo = (BusinessObject)classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", "somevlaue");
            bo.Save();
            IBOProp boProp = bo.Props["TestProp"];
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsDirty);
            //---------------Execute Test ----------------------
            boProp.Value = "New Value";
            //---------------Test Result -----------------------
            Assert.IsTrue(bo.Status.IsDirty);
        }


        private class BOWithCustomErrors : MockBO
        {
            protected override bool AreCustomRulesValid(out string customRuleErrors)
            {
                customRuleErrors = "ERROR";
                return false;
            }
        }
        [Test]
        public void Test_IsValid_CustomErrors_Errors()
        {
            //--------------- Set up test pack ------------------
            ClassDef.ClassDefs.Clear();
            BOWithCustomErrors myBO = new BOWithCustomErrors();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            IList<IBOError> errors;
            bool isValid = myBO.Status.IsValid(out errors);
            //--------------- Test Result -----------------------
            Assert.IsFalse(isValid);
            Assert.AreEqual(1, errors.Count);
            StringAssert.Contains("ERROR", errors[0].Message);
            Assert.AreEqual(ErrorLevel.Error, errors[0].Level);
            Assert.AreSame(myBO, errors[0].BusinessObject);
        }

        private class BOWithCustomErrors_Errors : MockBO
        {
            protected override bool AreCustomRulesValid(ref IList<IBOError> errors)
            {
                errors.Add(new BOError("ERROR1", ErrorLevel.Error));
                errors.Add(new BOError("ERROR2", ErrorLevel.Warning));
                return false;
            }
        }



        [Test]
        public void Test_IsValid_CustomErrors_ListOfErrors()
        {
            //--------------- Set up test pack ------------------
            ClassDef.ClassDefs.Clear();
            BOWithCustomErrors_Errors myBO = new BOWithCustomErrors_Errors();
            //--------------- Test Preconditions ----------------
            //--------------- Execute Test ----------------------
            IList<IBOError> errors;
            bool isValid = myBO.Status.IsValid(out errors);
            //--------------- Test Result -----------------------
            Assert.IsFalse(isValid);
            Assert.AreEqual(2, errors.Count);
            StringAssert.Contains("ERROR1", errors[0].Message);
            Assert.AreEqual(ErrorLevel.Error, errors[0].Level);
            Assert.AreSame(myBO, errors[0].BusinessObject);  
            StringAssert.Contains("ERROR2", errors[1].Message);
            Assert.AreEqual(ErrorLevel.Warning, errors[1].Level);
            Assert.AreSame(myBO, errors[1].BusinessObject);
        }


        private class BOWithCustomErrors_NullErrors : MockBO
        {
            protected override bool AreCustomRulesValid(ref IList<IBOError> errors)
            {
                errors = null;
                return true;
            }
        }



        [Test]
        public void Test_IsValid_CustomErors_NullErrors()
        {
            //--------------- Set up test pack ------------------
            ClassDef.ClassDefs.Clear();
            BOWithCustomErrors_NullErrors myBO = new BOWithCustomErrors_NullErrors();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            IList<IBOError> errors;
            bool isValid = myBO.Status.IsValid(out errors);
            //--------------- Test Result -----------------------
            Assert.IsTrue(isValid);
            Assert.AreEqual(0, errors.Count);

        }


//        [Test]
//        public void Test_BusinessObject_WithNoBrokenRules_Isvalid_AfterValidateCalled()
//        {
//            //---------------Set up test pack-------------------
//            ClassDef.ClassDefs.Clear();
//            ClassDef classDef = MyBO.LoadClassDefsNoUIDef();
//            BusinessObject bo = (BusinessObject)classDef.CreateNewBusinessObject();
//            IBOProp boProp = bo.Props["TestProp"];
//            //---------------Assert Precondition----------------
//            Assert.IsTrue(boProp.IsValid);
//            //---------------Execute Test ----------------------
//            bo.Validate();
//            //---------------Test Result -----------------------
//            Assert.AreEqual("", boProp.InvalidReason);
//            Assert.IsTrue(boProp.IsValid);
//            Assert.AreEqual("", bo.Status.IsValidMessage);
//            Assert.IsTrue(bo.IsValid());
//        }

        [Test]
        public void Test_BusinessObject_TrySaveThrowsUserError_IfWithBrokenRules()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            IBOProp boProp = bo.Props["TestProp"];
            //---------------Assert Precondition----------------
            Assert.IsTrue(boProp.IsValid);
            //---------------Execute Test ----------------------
            try
            {
                bo.Save();
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (BusObjectInAnInvalidStateException ex)
            {
                StringAssert.Contains("Test Prop' is a compulsory field and has no value", ex.Message);
            }
        }

        [Test]
        public void Test_CancelEdit_ShouldClearIsValidErrorMessage()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            MyBO myBO = (MyBO) classDef.CreateNewBusinessObject();
            myBO.TestProp = TestUtil.GetRandomString();
            myBO.Save();
            Assert.AreEqual("", myBO.Status.IsValidMessage);
            Assert.IsTrue(myBO.Status.IsValid());
            myBO.TestProp = "";
            IBOProp boProp = myBO.Props["TestProp"];
            //---------------Assert Precondition----------------
            Assert.IsFalse(myBO.Status.IsValid());
            Assert.AreNotEqual("", myBO.Status.IsValidMessage);
            Assert.IsFalse(boProp.IsValid);
            Assert.AreNotEqual("", boProp.InvalidReason);
            //---------------Execute Test ----------------------
            myBO.CancelEdits();
            //---------------Test Result -----------------------
            Assert.IsTrue(myBO.Status.IsValid());
            Assert.AreEqual("", myBO.Status.IsValidMessage);
            Assert.IsTrue(boProp.IsValid);
            Assert.AreEqual("", boProp.InvalidReason);
        }
        [Test]
        public void Test_CancelEdit_ShouldRestorePropertyValues()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            MyBO myBO = (MyBO) classDef.CreateNewBusinessObject();
            string origionalPropertyValue = myBO.TestProp = TestUtil.GetRandomString();
            myBO.Save();
            Assert.AreEqual("", myBO.Status.IsValidMessage);
            Assert.IsTrue(myBO.Status.IsValid());
            myBO.TestProp = "";
            IBOProp boProp = myBO.Props["TestProp"];
            //---------------Assert Precondition----------------

            Assert.AreNotEqual(origionalPropertyValue, myBO.TestProp);
            Assert.AreNotEqual(origionalPropertyValue, boProp.Value);
            //---------------Execute Test ----------------------
            myBO.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(origionalPropertyValue, myBO.TestProp);
            Assert.AreEqual(origionalPropertyValue, boProp.Value);
        }

        [Test]
        public void TestGetPropertyValueToDisplay()
        {
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithStringLookup();
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", "Started");
            Assert.AreEqual("S", bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual("Started", bo.GetPropertyValueToDisplay("TestProp2"));
        }


        // This test is duplicated in TestBOMapper.TestGetPropertyValueToDisplay_BusinessObjectLookupList()
        [Test]
        public void TestGetPropertyValueToDisplayWithBOLookupList()
        {
            ContactPersonTestBO.CreateSampleData();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithBOLookup();
            ContactPersonTestBO.LoadDefaultClassDef();

            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, "abc");
            ContactPersonTestBO cp =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", cp);
            Assert.IsNotNull(cp);
            Assert.AreEqual(cp.ContactPersonID, bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual("abc", bo.GetPropertyValueToDisplay("TestProp2"));
        }

        [Test]
        public void TestBOLookupListWithString()
        {
            ContactPersonTestBO.CreateSampleData();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithBOStringLookup();
            ContactPersonTestBO.LoadDefaultClassDef();

            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, "abc");
            ContactPersonTestBO cp =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();

            bo.SetPropertyValue("TestProp2", "abc");
            Assert.AreEqual(cp.ContactPersonID.ToString(), bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual("abc", bo.GetPropertyValueToDisplay("TestProp2"));
        }

        [Test]
        public void TestBOLookupListNull()
        {
            ContactPersonTestBO.CreateSampleData();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithBOStringLookup();
            ContactPersonTestBO.LoadDefaultClassDef();

            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, "abc");
            BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", null);
            Assert.AreEqual(null, bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual(null, bo.GetPropertyValueToDisplay("TestProp2"));
        }

        [Test]
        public void TestBORuleString()
        {
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithStringRule();
            Assert.IsTrue(classDef.PropDefcol.Contains("TestProp"), "TestProp must exist");
            IPropDef propDef = classDef.PropDefcol["TestProp"];
            Assert.GreaterOrEqual(1, propDef.PropRules.Count);
            Assert.AreEqual(1, propDef.PropRules.Count);
            Assert.IsNotNull(propDef.PropRules[0], "TestProp must have a rule");
            string errorMessage = "";
            Assert.IsTrue
                (propDef.PropRules[0].IsPropValueValid("TestProp", "abcde", ref errorMessage),
                 "Property value of length 5 must pass");
            Assert.IsFalse
                (propDef.PropRules[0].IsPropValueValid("TestProp", "abcdef", ref errorMessage),
                 "Property value of length 6 must not pass");
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", "abcde");
            string reason;
            Assert.IsTrue
                (bo.Status.IsValid(out reason), "BO should be valid with a TestProp value of 'abcde' but returned : " + reason);
            bo.SetPropertyValue("TestProp", "abcdef");
            Assert.IsFalse(bo.Status.IsValid(), "BO should not be valid with a TestProp value of 'abcdef'");
        }


        [Ignore("Cannot figure out what this is doing and why it is suddenly failing")] //TODO Brett 07 Dec 2009: Ignored Test - Cannot figure out what this is doing and why it is suddenly failing
        [Test]
        public void TestApplyEditResetsPreviousValues()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            MockRepository mock = new MockRepository();
            IDatabaseConnection itsConnection = mock.DynamicMock<IDatabaseConnection>();
            Expect.Call(itsConnection.GetConnection()).Return(DatabaseConnection.CurrentConnection.GetConnection()).
                Repeat.Times(2);
            Expect.Call(itsConnection.ExecuteSql(null, null)).IgnoreArguments().Return(1).Repeat.Times(1);
            mock.ReplayAll();
            MyBO bo = (MyBO) classDef.CreateNewBusinessObject();

            bo.SetPropertyValue("TestProp", "Goodbye");
            TransactionCommitterStub committer = new TransactionCommitterStub();
            committer.AddBusinessObject(bo);
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            bo.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual("Goodbye", bo.GetPropertyValueString("TestProp"));
        }

        [Test]
        public void Test_UpdatePropValueDirectly_ShouldChangeIsEditingStatusToTrue()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            IBOProp prop = bo.Props["TestProp"];
            const string newValue = "NewValue";
            //-------------Assert Preconditions -------------
            Assert.IsFalse(bo.Status.IsEditing);
            //---------------Execute Test ----------------------
            prop.Value = newValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(newValue, prop.Value);
            Assert.IsTrue(bo.Status.IsEditing);
        }

        [Test]
        public void Test_UpdatePropValueDirectly_ShouldChangeIsDirtyStatusToTrue()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            IBOProp prop = bo.Props["TestProp"];
            const string newValue = "NewValue";
            //-------------Assert Preconditions -------------
            Assert.IsFalse(bo.Status.IsDirty);
            //---------------Execute Test ----------------------
            prop.Value = newValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(newValue, prop.Value);
            Assert.IsTrue(bo.Status.IsDirty);
        }

        [Test]
        public void Test_UpdatePropValueDirectly_ShouldFireProperyUpdatedEvent()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bool propertyEventFired = false;
            IBusinessObject eventBusinessObject = null;
            IBOProp eventProp = null;
            bo.PropertyUpdated +=
                delegate(object sender, BOPropUpdatedEventArgs eventArgs)
                {
                    eventBusinessObject = eventArgs.BusinessObject;
                    eventProp = eventArgs.Prop;
                    propertyEventFired = true;
                };
            IBOProp prop = bo.Props["TestProp"];
            const string newValue = "NewValue";
            //-------------Assert Preconditions -------------
            Assert.IsFalse(propertyEventFired);
            Assert.IsNull(eventBusinessObject);
            Assert.IsNull(eventProp);
            //---------------Execute Test ----------------------
            prop.Value = newValue;
            //---------------Test Result -----------------------
            Assert.IsTrue(propertyEventFired);
            Assert.AreSame(bo, eventBusinessObject);
            Assert.AreSame(prop, eventProp);
        }

        [Test]
        public void Test_UpdatePropValueDirectly_WhenValueUnchanged_ShouldNotChangeIsEditingStatusToTrue()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            const string propValue = "PropValue";
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bo.TestProp = propValue;
            bo.Save();
            IBOProp prop = bo.Props["TestProp"];
            //-------------Assert Preconditions -------------
            Assert.IsFalse(bo.Status.IsEditing);
            //---------------Execute Test ----------------------
            prop.Value = propValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(propValue, prop.Value);
            Assert.IsFalse(bo.Status.IsEditing);
        }

        [Test]
        public void Test_UpdatePropValueDirectly_WhenValueUnchanged_ShouldNotChangeIsDirtyStatusToTrue()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            const string propValue = "PropValue";
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bo.TestProp = propValue;
            bo.Save();
            IBOProp prop = bo.Props["TestProp"];
            //-------------Assert Preconditions -------------
            Assert.IsFalse(bo.Status.IsDirty);
            //---------------Execute Test ----------------------
            prop.Value = propValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(propValue, prop.Value);
            Assert.IsFalse(bo.Status.IsDirty);
        }

        [Test]
        public void Test_UpdatePropValueDirectly_WhenValueUnchanged_ShouldNotFireProperyUpdatedEvent()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            const string propValue = "PropValue";
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bo.TestProp = propValue;
            bo.Save();
            bool propertyEventFired = false;
            bo.PropertyUpdated += ((sender, eventArgs) => propertyEventFired = true);
            IBOProp prop = bo.Props["TestProp"];
            //-------------Assert Preconditions -------------
            Assert.IsFalse(propertyEventFired);
            //---------------Execute Test ----------------------
            prop.Value = propValue;
            //---------------Test Result -----------------------
            Assert.IsFalse(propertyEventFired);
        }

        [Test]
        public void TestSaveUpdatesAutoIncrementingField()
        {
            ClassDef.ClassDefs.Clear();
            TestAutoInc.LoadClassDefWithAutoIncrementingID();

            TestAutoInc bo = new TestAutoInc();
            bo.SetPropertyValue("testfield", "testing 123");
            Assert.IsFalse(bo.TestAutoIncID.HasValue);
            bo.Save();
            Assert.IsNotNull(bo.TestAutoIncID);
            Assert.AreNotEqual(0, bo.TestAutoIncID);
            Assert.IsFalse(bo.Status.IsDirty);
        }

        [Test]
        public void TestSetPropertyValueFieldDoesNotExist()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            TestAutoInc.LoadClassDefWithAutoIncrementingID();

            TestAutoInc bo = new TestAutoInc();
            //---------------Execute Test ----------------------

            const string nonexistentPropName = "nonExistent";
            try
            {
                bo.SetPropertyValue(nonexistentPropName, "testing 123");
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (InvalidPropertyNameException ex)
            {
                string errMessage = String.Format
                    ("The given property name '{0}' does not exist in the "
                     + "collection of properties for the class '{1}'.", nonexistentPropName, "TestAutoInc");
                StringAssert.Contains(errMessage, ex.Message);
            }
        }

        [Test]
        public void Test_GetProperty_FieldDoesNotExist()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            TestAutoInc.LoadClassDefWithAutoIncrementingID();

            TestAutoInc bo = new TestAutoInc();
            //---------------Execute Test ----------------------

            const string nonexistentPropName = "nonExistent";
            try
            {
                bo.GetProperty(nonexistentPropName);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (InvalidPropertyNameException ex)
            {
                string errMessage = String.Format
                    ("The given property name '{0}' does not exist in the "
                     + "collection of properties for the class '{1}'.", nonexistentPropName, "TestAutoInc");
                StringAssert.Contains(errMessage, ex.Message);
            }
        }

        [Test]
        public void Test_AddNullBOProp()
        {
            //---------------Set up test pack-------------------
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            TestAutoInc.LoadClassDefWithAutoIncrementingID();

            TestAutoInc bo = new TestAutoInc();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            try
            {
                bo.Props.Add((IBOProp) null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("boProp", ex.ParamName);
            }
        }

        [Test]
        public void TestSaveWithBeforeSaveImplemented()
        {
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = BeforeSaveBo.LoadDefaultClassDef();
            BeforeSaveBo bo = (BeforeSaveBo) classDef.CreateNewBusinessObject();
            bo.FirstPart = "foo";
            bo.SecondPart = "bar";
            Assert.AreEqual("", bo.CombinedParts);
            TransactionCommitterStub committer = new TransactionCommitterStub();
            committer.AddBusinessObject(bo);
            committer.CommitTransaction();
            //bo.Save();
            Assert.AreEqual("foobar", bo.CombinedParts);
            //mock.VerifyAll();
        }

        [Test]
        public void TestSave_WithAfterSaveImplemented()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = AfterSaveBO.LoadDefaultClassDef();

            AfterSaveBO bo = (AfterSaveBO) classDef.CreateNewBusinessObject();
            bo.FirstPart = "foo";
            bo.SecondPart = "bar";

            //--------------Assert PreConditions----------------     
            Assert.AreEqual("", bo.CombinedParts);

            //---------------Execute Test ----------------------
            TransactionCommitterStub committer = new TransactionCommitterStub();
            committer.AddBusinessObject(bo);
            committer.CommitTransaction();

            //---------------Test Result -----------------------
            Assert.AreEqual("foobar", bo.CombinedParts);
        }
        [Test]
        public void TestSave_ShouldReturnUnderlyingObject()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = Shape.CreateClassDef();

            Shape bo = (Shape)classDef.CreateNewBusinessObject();

            //--------------Assert PreConditions----------------     

            //---------------Execute Test ----------------------
            IBusinessObject savedBusinessObject = bo.Save();

            //---------------Test Result -----------------------
            Assert.AreSame(bo, savedBusinessObject);
        }

        //[Test]
        //public void TestSave_ToDifferentDb()
        //{
        //    //---------------Set up test pack-------------------
        //    ClassDef.ClassDefs.Clear();
        //    ClassDef classDef = MyBO.LoadDefaultClassDef();
        //    MockRepository mock = new MockRepository();
        //    IDatabaseConnection mockDatabaseConnection;
        //    mockDatabaseConnection = TestTransactionCommitter.GetMockDatabaseConnectionWithExpectations(mock);
        //    MyBO myBO = new MyBO(mockDatabaseConnection);
        //    mock.ReplayAll();
        //    //---------------Test Preconditions ----------------

        //    //---------------Execute Test ----------------------
        //    myBO.Save();

        //    //---------------Test Result -----------------------
        //    mock.VerifyAll();
        //}

        [Test]
        public void TestDeleteObjce_WithAfterSaveImplemented()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = AfterSaveBO.LoadDefaultClassDef();

            AfterSaveBO bo = (AfterSaveBO) classDef.CreateNewBusinessObject();
            bo.FirstPart = "foo";
            bo.SecondPart = "bar";
            TransactionCommitterStub committer = new TransactionCommitterStub();
            committer.AddBusinessObject(bo);
            committer.CommitTransaction();
            //--------------Assert PreConditions----------------     
            Assert.AreEqual("foobar", bo.CombinedParts);

            //---------------Execute Test ----------------------
            bo.MarkForDelete();
            committer = new TransactionCommitterStub();
            committer.AddBusinessObject(bo);
            committer.CommitTransaction();

            //---------------Test Result -----------------------
            Assert.AreEqual("deleted", bo.CombinedParts);
        }

        [Test]
        public void TestCannotDelete_IsDeletable_False()
        {
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            IBusinessObject bo = classDef.CreateNewBusinessObject();
            try
            {
                bo.MarkForDelete();
                Assert.Fail("Expected to throw an BusObjDeleteException");
            }
                //---------------Test Result -----------------------
            catch (BusObjDeleteException ex)
            {
                StringAssert.Contains(string.Format("You cannot delete the 'MyBoNotEditableDeletable', as the IsDeletable is set to false for the object. ObjectID: {0}, also identified as {0}", bo.ID.ToString()), ex.Message);
            }
           // Assert.Throws<BusObjDeleteException>();
        }

        //[Test]
        //public void TestCannotDelete_IsDeletable_False_ExpectMessage()
        //{
        //    try
        //    {
        //        TestCannotDelete_IsDeletable_False();
        //        Assert.Fail();
        //    }
        //    catch (BusObjDeleteException ex)
        //    {
        //        Assert.IsTrue
        //            (ex.Message.Contains
        //                 ("You cannot delete the 'MyBoNotEditableDeletable', as the IsDeletable is set to false for the object"));
        //    }
        //}

        [Test]
        public void Test_MarkForDelete_WhenNew_DoesNotThrowException()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            IBusinessObject bo = classDef.CreateNewBusinessObject();
            bool markForDeleteEventFired = false;
            bo.MarkedForDeletion += delegate { markForDeleteEventFired = true; };
            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.Status.IsNew);
            //---------------Execute Test ----------------------

            bo.MarkForDelete();
            //---------------Test Result -----------------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.IsTrue(bo.Status.IsNew);
            Assert.IsFalse(bo.Status.IsDirty);
            Assert.IsTrue(bo.Status.IsEditing);
            Assert.IsTrue(markForDeleteEventFired);
        }

        [Test]
        public void Test_MarkForDelete()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            MyBO bo = (MyBO) classDef.CreateNewBusinessObject();
            bo.Save();
            bool markForDeleteEventFired = false;
            bo.MarkedForDeletion += delegate { markForDeleteEventFired = true; };

            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsDeleted);
            Assert.IsFalse(bo.Status.IsNew);
            Assert.IsFalse(bo.Status.IsDirty);
            Assert.IsFalse(bo.Status.IsEditing);
            Assert.IsFalse(markForDeleteEventFired);

            //---------------Execute Test ----------------------
            bo.MarkForDelete();

            //---------------Test Result -----------------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.IsFalse(bo.Status.IsNew);
            Assert.IsTrue(bo.Status.IsDirty);
            Assert.IsTrue(bo.Status.IsEditing);
            Assert.IsTrue(markForDeleteEventFired);
        }

        [Test]
        public void Test_MarkForDelete_WhenHasRelatedBO_WhenPreventDelete_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bo.Save();
            bo.MyMultipleRelationship.CreateBusinessObject();
            IRelationship relationship = bo.Relationships["MyMultipleRelationship"];

            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsDeleted);
            Assert.IsFalse(bo.Status.IsNew);
            Assert.IsTrue(bo.Status.IsDirty);
            Assert.IsFalse(bo.Status.IsEditing);
            Assert.AreEqual(1, bo.MyMultipleRelationship.Count);
            Assert.AreEqual(DeleteParentAction.Prevent, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            try
            {
                bo.MarkForDelete();
                Assert.Fail("expected Error");
            }
                //---------------Test Result -----------------------
            catch (BusObjDeleteException ex)
            {
                StringAssert.Contains("You cannot delete MyBO identified by", ex.Message);
                StringAssert.Contains("since it is related to 1 Business Objects via the MyMultipleRelationship relationship", ex.Message);
            }
        }
        [Test]
        public void Test_IsDeletable_WhenMultiple_WhenHasRelatedBO_WhenPreventDelete_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bo.Save();
            bo.MyMultipleRelationship.CreateBusinessObject();
            IRelationship relationship = bo.Relationships["MyMultipleRelationship"];

            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsDeleted);
            Assert.IsFalse(bo.Status.IsNew);
            Assert.IsTrue(bo.Status.IsDirty);
            Assert.IsFalse(bo.Status.IsEditing);
            Assert.AreEqual(1, bo.MyMultipleRelationship.Count);
            Assert.AreEqual(DeleteParentAction.Prevent, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            string message;
            bool isDeletable = bo.IsDeletable(out message);
            //---------------Test Result -----------------------
            Assert.IsFalse(isDeletable);
            StringAssert.Contains("You cannot delete MyBO identified by", message);
            StringAssert.Contains("since it is related to 1 Business Objects via the MyMultipleRelationship relationship", message);
        }
        [Test]
        public void Test_IsDeletable_WhenSingle_WhenHasRelatedBO_WhenPreventDelete_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bo.Save();
            SingleRelationship<MyRelatedBo> relationship = (SingleRelationship<MyRelatedBo>) bo.Relationships["MyRelationship"];
            relationship.SetRelatedObject(new MyRelatedBo());

            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsDeleted);
            Assert.IsFalse(bo.Status.IsNew);
            Assert.IsTrue(bo.Status.IsDirty);
            Assert.IsTrue(bo.Status.IsEditing);

            Assert.IsNotNull(relationship.GetRelatedObject());
            Assert.AreEqual(DeleteParentAction.Prevent, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            string message;
            bool isDeletable = bo.IsDeletable(out message);
            //---------------Test Result -----------------------
            Assert.IsFalse(isDeletable);
            StringAssert.Contains("You cannot delete MyBO identified by", message);
            StringAssert.Contains("since it is related to a Business Object via the MyRelationship relationship", message);
        }
        [Test]
        public void Test_IsDeletable_WhenSingle_WhenHasRelatedBO_WhenPreventDelete_WhenRelatedBOMarked4Delete_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bo.Save();
            SingleRelationship<MyRelatedBo> relationship = (SingleRelationship<MyRelatedBo>) bo.Relationships["MyRelationship"];
            MyRelatedBo myRelatedBO = new MyRelatedBo();
            relationship.SetRelatedObject(myRelatedBO);
            myRelatedBO.MarkForDelete();
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsDeleted);
            Assert.IsFalse(bo.Status.IsNew);
            Assert.IsTrue(bo.Status.IsDirty);
            Assert.IsTrue(bo.Status.IsEditing);
            Assert.IsNotNull(relationship.GetRelatedObject());
            Assert.AreEqual(DeleteParentAction.Prevent, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            string message;
            bool isDeletable = bo.IsDeletable(out message);
            //---------------Test Result -----------------------
            Assert.IsTrue(isDeletable);
        }
        [Test]
        public void Test_MarkForDelete_NewObjectDoesNotRaiseError()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            MyBO bo = (MyBO) classDef.CreateNewBusinessObject();
//            bo.Save();
            bool markForDeleteEventFired = false;
            bo.MarkedForDeletion += delegate { markForDeleteEventFired = true; };

            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsDeleted);
            Assert.IsTrue(bo.Status.IsNew);
            Assert.IsFalse(bo.Status.IsDirty);
            Assert.IsFalse(bo.Status.IsEditing);
            Assert.IsFalse(markForDeleteEventFired);
            //---------------Execute Test ----------------------
            bo.MarkForDelete();
            //---------------Test Result -----------------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.IsTrue(bo.Status.IsNew);
            Assert.IsFalse(bo.Status.IsDirty);
            Assert.IsTrue(bo.Status.IsEditing);
            Assert.IsTrue(markForDeleteEventFired);
        }

        [Test]
        public void Test_MarkForDelete_Save_NewObjectDoesNotRaiseError()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            MyBO bo = (MyBO) classDef.CreateNewBusinessObject();
            bo.MarkForDelete();
            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.IsTrue(bo.Status.IsNew);
            Assert.IsFalse(bo.Status.IsDirty);
            Assert.IsTrue(bo.Status.IsEditing);
            //---------------Execute Test ----------------------
            bo.Save();
            //---------------Test Result -----------------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.IsTrue(bo.Status.IsNew);
            Assert.IsFalse(bo.Status.IsDirty);
            Assert.IsTrue(bo.Status.IsEditing);
        }


        [Test]
        public void Test_MarkForDelete_WhenHasRelatedBO_WhenDeleteRelated_ShouldMarkForDeleteRelated()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bo.Save();
            MyRelatedBo myRelatedBO = bo.MyMultipleRelationship.CreateBusinessObject();
            IRelationship relationship = bo.Relationships["MyMultipleRelationship"];
            ((RelationshipDef)relationship.RelationshipDef).DeleteParentAction = DeleteParentAction.DeleteRelated;
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsDeleted);
            Assert.IsFalse(myRelatedBO.Status.IsDeleted);
            Assert.AreEqual(1, bo.MyMultipleRelationship.Count);
            Assert.AreEqual(DeleteParentAction.DeleteRelated, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
             bo.MarkForDelete();
            //---------------Test Result -----------------------
             Assert.IsTrue(bo.Status.IsDeleted);
             Assert.IsTrue(myRelatedBO.Status.IsDeleted);
        }

        [Test]
        public void TestCannotEdit_IsEditable_False()
        {
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            MyBoNotEditableDeletable bo = (MyBoNotEditableDeletable) classDef.CreateNewBusinessObject();
            try
            {
                bo.TestProp = "new";
                Assert.Fail("Expected to throw an BusObjEditableException");
            }
                //---------------Test Result -----------------------
            catch (BusObjEditableException ex)
            {
                StringAssert.Contains(string.Format("You cannot Edit the 'MyBoNotEditableDeletable', as the IsEditable is set to false for the object. ObjectID: {0}, also identified as {0}", bo.ID.ToString()), ex.Message);
            }
            //Assert.Throws<BusObjEditableException>(() => bo.TestProp = "new");
        }

        //[Test]
        //public void TestCannotEdit_IsEditable_False_ExpectMessage()
        //{
        //    try
        //    {
        //        TestCannotEdit_IsEditable_False();
        //        Assert.Fail();
        //    }
        //    catch (BusObjEditableException ex)
        //    {
        //        Assert.IsTrue
        //            (ex.Message.Contains
        //                 ("You cannot Edit the 'MyBoNotEditableDeletable', as the IsEditable is set to false for the object"));
        //    }
        //}

        [Test]
        public void TestCanDelete_IsDeletable_True()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            MyBoNotEditableDeletable bo = (MyBoNotEditableDeletable) classDef.CreateNewBusinessObject();
            bo.Save();
            bo.Deletable = true;
            bo.Editable = true;
            bo.MarkForDelete();
        }

        [Test]
        public void TestCanEdit_IsEditable_True()
        {
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            MyBoNotEditableDeletable bo = (MyBoNotEditableDeletable) classDef.CreateNewBusinessObject();
            bo.Editable = true;
            bo.TestProp = "new";
        }

        [Test]
        public void TestCanEdit_IsDeletable_False()
        {
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            MyBoNotEditableDeletable bo = (MyBoNotEditableDeletable) classDef.CreateNewBusinessObject();
            bo.Editable = true;
            bo.Deletable = false;
            bo.TestProp = "new";
        }

        [Test]
        public void TestCanDelete_IsEditable_False()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBoNotEditableDeletable.LoadDefaultClassDef();
            MyBoNotEditableDeletable bo = (MyBoNotEditableDeletable) classDef.CreateNewBusinessObject();
            bo.Save();
            bo.Editable = false;
            bo.Deletable = true;
            bo.MarkForDelete();
        }
/*
        [Test]
        public void TestPropValueHasChanged()
        {
            int? x = 1;
            int? y = 1;
            Assert.IsFalse(BusinessObject.PropValueHasChanged(x, y));

            object z = 1;
            Assert.IsFalse(BusinessObject.PropValueHasChanged(x, z));

            Assert.IsFalse(BusinessObject.PropValueHasChanged(null, null));
            Assert.IsTrue(BusinessObject.PropValueHasChanged(null, x));
            Assert.IsTrue(BusinessObject.PropValueHasChanged(x, null));
            x = null;
            Assert.IsTrue(BusinessObject.PropValueHasChanged(x, y));
        }

        [Test]
        public void TestPropValueHasChanged_DiffTypes()
        {
            int? x = 1;
            int? y = 1;
            Assert.IsFalse(BusinessObject.PropValueHasChanged(x, y));

            const string z = "1";
            Type type = x.GetType();
            Assert.IsFalse(BusinessObject.PropValueHasChanged(Convert.ChangeType(x, type), Convert.ChangeType(z, type)));

            Assert.IsFalse(BusinessObject.PropValueHasChanged(null, null));
            Assert.IsTrue(BusinessObject.PropValueHasChanged(null, x));
            Assert.IsTrue(BusinessObject.PropValueHasChanged(x, null));
            x = null;
            Assert.IsTrue(BusinessObject.PropValueHasChanged(x, y));
        }*/

        [Test]
        public void TestSaveUsesFactoryGeneratedTransactionCommitter()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");

            BORegistry.DataAccessor = new DataAccessorInMemory();
            //---------------Execute Test ----------------------
            cp.Save();
            //---------------Test Result -----------------------
            ContactPersonTestBO loadedCP =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(cp.ID);
            Assert.IsNotNull(loadedCP);
            Assert.AreSame(cp, loadedCP);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetPropertyValue_NullSource()
        {
            //---------------Set up test pack-------------------
            Engine engine1 = new Engine();
            engine1.EngineNo = "20";
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            object engineNo = engine1.GetPropertyValue(null, "EngineNo");
            //---------------Test Result -----------------------
            Assert.AreEqual(engine1.EngineNo, engineNo);
            //---------------Tear Down -------------------------     
        }

        [Test]
        public void TestGetPropertyValue_ThroughRelationship()
        {
            //---------------Set up test pack-------------------
            Car car1 = new Car();
            car1.CarRegNo = "5";

            Engine engine1 = new Engine();
            engine1.CarID = car1.CarID;
            engine1.EngineNo = "20";

            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            committer.AddBusinessObject(car1);
            committer.AddBusinessObject(engine1);
            committer.CommitTransaction();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            object carregno = engine1.GetPropertyValue(new Source("Car"), "CarRegNo");
            //---------------Test Result -----------------------
            Assert.AreEqual(car1.CarRegNo, carregno);
            //---------------Tear Down -------------------------     
        }

        [Test]
        public void TestGetPropertyValue_ThroughRelationship_TwoLevels()
        {
            //---------------Set up test pack-------------------
            string surname = TestUtil.GetRandomString();
            new Engine();
            new Car();
            new ContactPerson();
            ContactPerson owner = ContactPerson.CreateSavedContactPerson(surname);
            Car car = Car.CreateSavedCar("5", owner);
            Engine engine = Engine.CreateSavedEngine(car, "20");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            object fetchedSurname = engine.GetPropertyValue(Source.FromString("Car.Owner"), "Surname");
            //---------------Test Result -----------------------
            Assert.AreEqual(surname, fetchedSurname);
            //---------------Tear Down -------------------------     
        }

        [Test]
        public void Test_GetPropertyValue_WithExpression()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            var originalFirstName = TestUtil.GetRandomString();
            contactPersonTestBO.SetPropertyValue(bo => bo.FirstName, originalFirstName);
            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            var firstName = contactPersonTestBO.GetPropertyValue(testBo => testBo.FirstName);
            //---------------Test Result -----------------------
            Assert.AreEqual(originalFirstName, firstName);
        }


        [Test]
        public void Test_GetPropertyValue_WithExpression_ValueType()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            var originalDateTime = DateTime.Today.AddDays(-TestUtil.GetRandomInt(365));
            contactPersonTestBO.SetPropertyValue(bo => bo.DateOfBirth, originalDateTime);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var dateOfBirth = contactPersonTestBO.GetPropertyValue(testBo => testBo.DateOfBirth);
            //---------------Test Result -----------------------
            Assert.AreEqual(originalDateTime, dateOfBirth);
        }

        [Test]
        public void Test_GetPropertyValue_WithExpression_WhenInvalidProperty()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                var firstName = contactPersonTestBO.GetPropertyValue(testBo => testBo.FirstName + "123");
            }
            //---------------Test Result -----------------------
            catch (Exception ex)
            {
                Assert.IsInstanceOf<ArgumentException>(ex);
                StringAssert.Contains("testBo => (testBo.FirstName + \"123\") is not a valid property on ContactPersonTestBO", ex.Message);
            }
        }


        [Test]
        public void TestBoStatusEqual()
        {
            //---------------Set up test pack-------------------
            BOStatus boStatus1 = new BOStatus(new Car());
            boStatus1.SetBOFlagValue(BOStatus.Statuses.isDeleted, true);
            //boStatus1.SetBOFlagValue(BOStatus.Statuses.isDirty, false);
            boStatus1.SetBOFlagValue(BOStatus.Statuses.isEditing, true);
            boStatus1.SetBOFlagValue(BOStatus.Statuses.isNew, false);

            BOStatus boStatus2 = new BOStatus(new Car());
            boStatus2.SetBOFlagValue(BOStatus.Statuses.isDeleted, true);
            //boStatus2.SetBOFlagValue(BOStatus.Statuses.isDirty, false);
            boStatus2.SetBOFlagValue(BOStatus.Statuses.isEditing, true);
            boStatus2.SetBOFlagValue(BOStatus.Statuses.isNew, false);
            //---------------Execute Test ----------------------
            bool equal = boStatus1.Equals(boStatus2);
            //---------------Test Result -----------------------
            Assert.IsTrue(equal);
            Assert.AreEqual(boStatus1.GetHashCode(), boStatus2.GetHashCode());
        }

        [Test]
        public void TestBoStatusEqual_Null()
        {
            //---------------Set up test pack-------------------
            BOStatus boStatus1 = new BOStatus(new Car());
            //---------------Execute Test ----------------------
            bool equal = boStatus1.Equals(null);
            //---------------Test Result -----------------------
            Assert.IsFalse(equal);
        }

        [Test]
        public void TestBoStatusNotEqual()
        {
            //---------------Set up test pack-------------------
            BOStatus boStatus1 = new BOStatus(new Car());
            boStatus1.SetBOFlagValue(BOStatus.Statuses.isDeleted, true);

            BOStatus boStatus2 = new BOStatus(new Car());
            boStatus2.SetBOFlagValue(BOStatus.Statuses.isDeleted, false);
            //---------------Execute Test ----------------------
            bool equal = boStatus1.Equals(boStatus2);
            //---------------Test Result -----------------------
            Assert.IsFalse(equal);
            Assert.AreNotEqual(boStatus1.GetHashCode(), boStatus2.GetHashCode());
        }

        [Test]
        public void Test_UpdatePropValue_ShouldFirePropertyUpdatedEvent()
        {
            //---------------Set up test pack-------------------
            Engine engine1 = new Engine();
            bool propertyEventFired = false;
            IBusinessObject eventBusinessObject = null;
            IBOProp eventProp = null;
            engine1.PropertyUpdated +=
                delegate(object sender, BOPropUpdatedEventArgs eventArgs)
                {
                    eventBusinessObject = eventArgs.BusinessObject;
                    eventProp = eventArgs.Prop;
                    propertyEventFired = true;
                };
            //-------------Assert Preconditions -------------
            Assert.IsFalse(propertyEventFired);
            Assert.IsNull(eventBusinessObject);
            Assert.IsNull(eventProp);
            //---------------Execute Test ----------------------
            engine1.EngineNo = "20";
            //---------------Test Result -----------------------
            Assert.IsTrue(propertyEventFired);
            Assert.AreSame(engine1, eventBusinessObject);
            Assert.AreSame(engine1.Props["EngineNo"], eventProp);
        }

        [Test]
        public void Test_UpdatePropValue_ShouldNotFireUpdatedEvent()
        {
            //---------------Set up test pack-------------------
            Engine engine1 = new Engine();
            bool updatedEventFired = false;
            //-------------Assert Preconditions -------------
            Assert.IsFalse(updatedEventFired);
            //---------------Execute Test ----------------------
            engine1.Updated += delegate { updatedEventFired = true; };
            engine1.EngineNo = "20";
            //---------------Test Result -----------------------
            Assert.IsFalse(updatedEventFired);
        }

        [Test]
        public void Test_SaveFiresUpdatedEvent()
        {
            //---------------Set up test pack-------------------
            Engine engine1 = new Engine();
            bool updatedEventFired = false;

            //-------------Assert Preconditions -------------
            Assert.IsFalse(updatedEventFired);
            //---------------Execute Test ----------------------
            engine1.Updated += delegate { updatedEventFired = true; };
            engine1.EngineNo = "20";
            engine1.Save();

            //---------------Test Result -----------------------
            Assert.IsTrue(updatedEventFired);
        }


        [Test]
        public void Test_CancelEdit_ShouldFirePropertyUpdatedEvent()
        {
            //---------------Set up test pack-------------------
            Engine engine1 = new Engine();
            bool propertyEventFired = false;
            IBusinessObject eventBusinessObject = null;
            IBOProp eventProp = null;
            engine1.EngineNo = "20";
            engine1.PropertyUpdated +=
                delegate(object sender, BOPropUpdatedEventArgs eventArgs)
                {
                    eventBusinessObject = eventArgs.BusinessObject;
                    eventProp = eventArgs.Prop;
                    propertyEventFired = true;
                };
            //-------------Assert Preconditions -------------
            Assert.IsFalse(propertyEventFired);
            Assert.IsNull(eventBusinessObject);
            Assert.IsNull(eventProp);
            //---------------Execute Test ----------------------
            engine1.CancelEdits();
            //---------------Test Result -----------------------
            Assert.IsTrue(propertyEventFired);
            Assert.AreSame(engine1, eventBusinessObject);
        }

        [Test]
        public void Test_SetPropertyValue_WithDateTime()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            DateTime newDateTime = DateTime.Now;
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            contactPersonTestBO.SetPropertyValue("DateOfBirth", newDateTime);
            //---------------Test Result -----------------------
            object value = contactPersonTestBO.GetPropertyValue("DateOfBirth");
            Assert.IsInstanceOf(typeof (DateTime), value);
            Assert.AreEqual(newDateTime, value);
        }

        [Test]
        public void Test_SetPropertyValue_WithDateTimeString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            DateTime newDateTime = DateTime.Today.Add(new TimeSpan(6, 3, 2));
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            contactPersonTestBO.SetPropertyValue("DateOfBirth", newDateTime.ToString());
            //---------------Test Result -----------------------
            object value = contactPersonTestBO.GetPropertyValue("DateOfBirth");
            Assert.IsInstanceOf(typeof (DateTime), value);
            Assert.AreEqual(newDateTime, value);
        }


        [Test]
        public void Test_SetPropertyValue_WithDateTimeString_Invalid()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string newDateTime = "31/11/2008";
            IBOProp prop = contactPersonTestBO.Props["DateOfBirth"];
            //-------------Assert Preconditions -------------
            Assert.IsNull(prop.Value);
            Assert.IsTrue(prop.IsValid);
            //---------------Execute Test ----------------------
            try
            {
                contactPersonTestBO.SetPropertyValue("DateOfBirth", newDateTime);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                string message = string.Format("{0} cannot be set to {1}. It is not a type of"
                                               , "DateOfBirth", newDateTime);
                StringAssert.Contains(message, ex.Message);
                StringAssert.Contains("DateTime", ex.Message);
                object value = contactPersonTestBO.GetPropertyValue("DateOfBirth");
                Assert.IsNull(value);
                Assert.IsTrue(prop.IsValid);
            }
        }

        [Test]
        public void Test_SetPropertyValue_WithEnumString()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDefWithEnum();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            contactPersonTestBO.SetPropertyValue("ContactType", "Business");
            //---------------Test Result -----------------------
            object value = contactPersonTestBO.GetPropertyValue("ContactType");
            Assert.IsInstanceOf(typeof (ContactPersonTestBO.ContactType), value);
            Assert.AreEqual(ContactPersonTestBO.ContactType.Business, value);
        }

        [Test]
        public void Test_SetPropertyValue_WithEnumString_Invalid()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDefWithEnum();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string newValue = "InvalidOption";
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            InvalidPropertyValueException exception = null;
            try
            {
                contactPersonTestBO.SetPropertyValue("ContactType", newValue);
            }
            catch (InvalidPropertyValueException ex)
            {
                exception = ex;
            }
            //---------------Test Result -----------------------
            Assert.IsNotNull(exception, "Expected exception of type InvalidPropertyValueException");
            StringAssert.Contains(
                "An error occurred while attempting to convert the loaded property value of 'ContactType' to its specified type of 'Habanero.Test.BO.ContactPersonTestBO+ContactType'. The property value is 'InvalidOption'. See log for details",
                exception.Message);

//            object value = contactPersonTestBO.GetPropertyValue("ContactType");
//            Assert.IsInstanceOf(typeof (string), value);
//            Assert.AreEqual(newValue, value);
//            IBOProp prop = contactPersonTestBO.Props["ContactType"];
//            Assert.IsFalse(prop.IsValid);
//            StringAssert.Contains(
//                "for property 'Contact Type' is not valid. It is not a type of ContactPersonTestBO+ContactType.",
//                prop.InvalidReason);

            //Habanero.BO.InvalidPropertyValueException: An error occurred while attempting to convert the loaded property value of 'ContactType' to its specified type of 'Habanero.Test.BO.ContactPersonTestBO+ContactType'. The property value is 'InvalidOption'. See log for details
        }

        [Test]
        public void Test_SetPropertyValue_WithExpression()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            var firstName = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            Assert.IsNullOrEmpty(contactPersonTestBO.FirstName);
            //---------------Execute Test ----------------------
            contactPersonTestBO.SetPropertyValue(bo => bo.FirstName, firstName);
            //---------------Test Result -----------------------
            Assert.AreEqual(firstName, contactPersonTestBO.FirstName);
        }

        [Test]
        public void Test_SetPropertyValue_WithExpression_ValueType()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            var dateOfBirth = DateTime.Today;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            contactPersonTestBO.DateOfBirthAsExpression = dateOfBirth;
            //---------------Test Result -----------------------
            Assert.AreEqual(dateOfBirth, contactPersonTestBO.DateOfBirth);
        }
        
        [Test]
        public void Test_SetPropertyValue_WithExpression_WhenInvalidProperty()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            var firstName = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                contactPersonTestBO.SetPropertyValue(testBo => testBo.FirstName + "123", firstName);
            }
            //---------------Test Result -----------------------
            catch (Exception ex)
            {
                Assert.IsInstanceOf<ArgumentException>(ex);
                StringAssert.Contains("testBo => (testBo.FirstName + \"123\") is not a valid property on ContactPersonTestBO", ex.Message);
            }
        }


    }
}