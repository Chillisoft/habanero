//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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

using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;
using Habanero.Util;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Habanero.Test.BO.ClassDefinition
{

    [TestFixture]
    public class TestPropDef
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
            _propDef = new PropDef("PropName", typeof (string), PropReadWriteRule.ReadOnly, null);
        }

        #endregion

        private PropDef _propDef;


        [Test]
        public void Test_SetPropDefUnitOfMeasure()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof (string), PropReadWriteRule.ReadOnly, null);
            //---------------Assert Precondition----------------
            Assert.AreEqual("", propDef.UnitOfMeasure);
            //---------------Execute Test ----------------------
            const string newUOM = "New UOM";
            propDef.UnitOfMeasure = newUOM;
            //---------------Test Result -----------------------
            Assert.AreEqual(newUOM, propDef.UnitOfMeasure);
        }
        [Test]
        public void Test_PropertyType_WhenTypeNullable_ShouldRetTypeToUnderlyingType()
        {
            //---------------Set up test pack-------------------
            Type propType = typeof (Guid?);
            IPropDef propDef = new PropDef("PropName", propType, PropReadWriteRule.ReadOnly, null);
            //---------------Assert Precondition----------------
            Assert.IsNotEmpty(propDef.PropertyTypeName);
            Assert.IsNotNull(propDef.PropertyType);
            Assert.IsTrue(ReflectionUtilities.IsNullableType(propType));
            Assert.AreSame(typeof(Guid), ReflectionUtilities.GetNullableUnderlyingType(propType));
            //---------------Execute Test ----------------------
            Type propertyType = propDef.PropertyType;
            //---------------Test Result -----------------------
            Assert.AreSame(typeof(Guid), propertyType);
        }
        [Test]
        public void Test_PropertyType_WhenTypeNotNullable_ShouldRetType()
        {
            //---------------Set up test pack-------------------
            Type propType = typeof (Guid);
            IPropDef propDef = new PropDef("PropName", propType, PropReadWriteRule.ReadOnly, null);
            //---------------Assert Precondition----------------
            Assert.IsNotEmpty(propDef.PropertyTypeName);
            Assert.IsNotNull(propDef.PropertyType);
            Assert.IsFalse(ReflectionUtilities.IsNullableType(propType));
            Assert.AreSame(typeof(Guid), ReflectionUtilities.GetNullableUnderlyingType(propType));
            //---------------Execute Test ----------------------
            Type propertyType = propDef.PropertyType;
            //---------------Test Result -----------------------
            Assert.AreSame(typeof(Guid), propertyType);
        }

        [Test]
        public void Test_SetPropType_ShouldSet()
        {
            //---------------Set up test pack-------------------
            Type origPropType = typeof(Guid);
            IPropDef propDef = new PropDef("PropName", origPropType, PropReadWriteRule.ReadOnly, null);
            var newPropType = typeof (string);
            //---------------Assert Precondition----------------
            Assert.AreSame(origPropType, propDef.PropertyType);
            //---------------Execute Test ----------------------
            propDef.PropertyType = newPropType;
            //---------------Test Result -----------------------
            Assert.AreNotSame(origPropType, propDef.PropertyType);
            Assert.AreSame(newPropType, propDef.PropertyType);
        }

        [Test]
        public void Test_AddPropRule()
        {
            //---------------Set up test pack-------------------
            IPropDef propDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, propDef.PropRules.Count);

            //---------------Execute Test ----------------------
            PropRuleString rule = new PropRuleString("StringRule", "My Message", 1, 3,"");
            propDef.AddPropRule(rule);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, propDef.PropRules.Count);
        }

        [Test]
        public void Test_AddTwoPropRules()
        {
            //---------------Set up test pack-------------------
            IPropDef propDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, propDef.PropRules.Count);

            //---------------Execute Test ----------------------
            PropRuleString rule = new PropRuleString("StringRule", "My Message", 1, 3, "");
            propDef.AddPropRule(rule);
            propDef.AddPropRule(new PropRuleString("StringRule", "My Message", 1, 3, ""));

            //---------------Test Result -----------------------
            Assert.AreEqual(2, propDef.PropRules.Count);
        }

        [Test]
        public void Test_IsValueValid_ValueInLookupList_Guid()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            MyBO.LoadDefaultClassDef();
            PropDef propDef = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null) ;
            MyBO validBusinessObject = new MyBO {TestProp = "ValidValue"};
            validBusinessObject.Save();
            propDef.LookupList = new BusinessObjectLookupList(typeof(MyBO));
            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            string errMsg = "";
            bool valid = propDef.IsValueValid(validBusinessObject.ID.GetAsGuid(), ref errMsg);

            //---------------Test Result -----------------------
            Assert.AreEqual("", errMsg);
            Assert.IsTrue(valid);
        }

        [Test]
        public void Test_IsValueValid_ValueNotInLookupList_Guid()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            MyBO.LoadDefaultClassDef();
            PropDef propDef = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null) { LookupList = new BusinessObjectLookupList(typeof(MyBO), "", "", true) };
            Guid invalidValue = Guid.NewGuid();
            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            string errMsg = "";
            
            bool valid = propDef.IsValueValid(invalidValue, ref errMsg);

            //---------------Test Result -----------------------
            string expectedErrorMessage = "Prop Name' invalid since '" + invalidValue + "' is not in the lookup list of available values.";
            StringAssert.Contains(expectedErrorMessage, errMsg);
//            Assert.AreEqual(expectedErrorMessage, errMsg);
            Assert.IsFalse(valid);
        }

        [Test]
        public void Test_IsValueValid_ValueInLookupList_Int()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOWithIntID.LoadClassDefWithIntID();
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null) ;
            BOWithIntID validBusinessObject = new BOWithIntID { IntID = 3, TestField = "ValidValue" };
            validBusinessObject.Save();
            propDef.LookupList = new BusinessObjectLookupList(typeof(BOWithIntID));
            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            string errMsg = "";
            bool valid = propDef.IsValueValid(validBusinessObject.IntID, ref errMsg);

            //---------------Test Result -----------------------
            Assert.AreEqual("", errMsg);
            Assert.IsTrue(valid);
        }

        [Test]
        public void Test_IsValueValid_ValueNotInLookupList_Int()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOWithIntID.LoadClassDefWithIntID();
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null) { LookupList = new BusinessObjectLookupList(typeof(BOWithIntID), "", "", true) };
            const int invalidValue = 4555;
            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            string errMsg = "";
            
            bool valid = propDef.IsValueValid(invalidValue, ref errMsg);

            //---------------Test Result -----------------------

            string expectedErrorMessage = "Prop Name' invalid since '" + invalidValue + "' is not in the lookup list of available values.";
            StringAssert.Contains(expectedErrorMessage, errMsg);
//            StringAssert.Contains();
            Assert.IsFalse(valid);
        }
        [Test]
        public void Test_GetBusinessObjectFromObjectManager()
        {
            //---------------Set up test pack-------------------
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOWithIntID.LoadClassDefWithIntID();
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null);
            BOWithIntID expectedBO = new BOWithIntID { IntID = 3, TestField = "ValidValue" };
            expectedBO.Save();
            propDef.LookupList = new BusinessObjectLookupList(typeof(BOWithIntID));
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBusinessObject returnedBO = propDef.GetlookupBusinessObjectFromObjectManager(expectedBO.IntID);
            //---------------Test Result -----------------------
            Assert.AreSame(expectedBO, returnedBO );
        }
        [Test]
        public void Test_GetBusinessObjectFromObjectManager_IdNotInObjectManager()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOWithIntID.LoadClassDefWithIntID();
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null);
            BOWithIntID expectedBO = new BOWithIntID { IntID = 3, TestField = "ValidValue" };
            expectedBO.Save();
            propDef.LookupList = new BusinessObjectLookupList(typeof(BOWithIntID));
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, BORegistry.BusinessObjectManager.Count);
            //---------------Execute Test ----------------------
            IBusinessObject returnedBO = propDef.GetlookupBusinessObjectFromObjectManager(expectedBO.IntID);
            //---------------Test Result -----------------------
            Assert.IsNull(returnedBO);
        }
        [Test]
        public void Test_GetBusinessObjectFromObjectManager_IdInObjectManager_ButWrongType()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID_DifferentType.LoadClassDefWithIntID();
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null);
            BOWithIntID expectedBO = new BOWithIntID { IntID = 3, TestField = "ValidValue" };
            expectedBO.Save();
            propDef.LookupList = new BusinessObjectLookupList(typeof(BOWithIntID_DifferentType));

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, BORegistry.BusinessObjectManager.Count);
            //---------------Execute Test ----------------------
            IBusinessObject returnedBO = propDef.GetlookupBusinessObjectFromObjectManager(expectedBO.IntID);
            //---------------Test Result -----------------------
            Assert.IsNull(returnedBO);
        }

        [Test]
        public void Test_GetBusinessObjectFromObjectManager_WriteNewProp()
        {
            //---------------Set up test pack-------------------
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefWithSurnameAsPrimaryKey_WriteNew();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            string surname = TestUtil.GetRandomString();
            contactPersonTestBO.Surname = surname;
            PropDef propDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadWrite, null);
            propDef.LookupList = new BusinessObjectLookupList(typeof(ContactPersonTestBO));
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBusinessObject returnedBO = propDef.GetlookupBusinessObjectFromObjectManager(contactPersonTestBO.Surname);
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, returnedBO);
        }

        [Test]
        public void Test_IsValueValid_OnePropRule_ValidValue()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);
            propDef.AddPropRule(new PropRuleString("StringRule", "My Message", 1, 3, ""));

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, propDef.PropRules.Count);

            //---------------Execute Test ----------------------
            string errMsg = "";
            bool valid = propDef.IsValueValid("AB",ref errMsg);

            //---------------Test Result -----------------------
            Assert.IsTrue(valid);
            Assert.AreEqual("", errMsg);
        }

        [Test]
        public void Test_IsValueValid_OnePropRule_InValidValue()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);
            propDef.AddPropRule(new PropRuleString("StringRule", "My Message", 1, 3, ""));

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, propDef.PropRules.Count);

            //---------------Execute Test ----------------------
            string errMsg = "";
            bool valid = propDef.IsValueValid("Long String", ref errMsg);

            //---------------Test Result -----------------------
            Assert.IsFalse(valid);
            StringAssert.Contains("'Long String' for property 'Prop Name' is not valid for the rule 'StringRule'", errMsg);
        }

        [Test]
        public void Test_IsValueValid_TwoPropRule_ValidValue()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);
            propDef.AddPropRule(new PropRuleString("StringRule", "Rule 1", 1, 3, ""));
            propDef.AddPropRule(new PropRuleString("StringRule", "Rule 2", 3, 10, ""));

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, propDef.PropRules.Count);

            //---------------Execute Test ----------------------
            string errMsg = "";
            bool valid = propDef.IsValueValid("ABC", ref errMsg);

            //---------------Test Result -----------------------
            Assert.IsTrue(valid);
            Assert.AreEqual("", errMsg);
        }
        [Test]
        public void Test_IsValueValid_TwoPropRule_InValidValue_FailBothRules()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);
            propDef.AddPropRule(new PropRuleString("StringRule", "Rule 1", 1, 3, ""));
            propDef.AddPropRule(new PropRuleString("StringRule", "Rule 2", 3, 10, ""));


            //---------------Assert Precondition----------------
            Assert.AreEqual(2, propDef.PropRules.Count);

            //---------------Execute Test ----------------------
            string errMsg = "";
            bool valid = propDef.IsValueValid("Too Long String by far", ref errMsg);

            //---------------Test Result -----------------------
            Assert.IsFalse(valid);
            StringAssert.Contains("'Too Long String by far' for property 'Prop Name' is not valid for the rule 'StringRule'", errMsg);
            StringAssert.Contains("Rule 1", errMsg);
            StringAssert.Contains("Rule 2", errMsg);
        }
        [Test]
        public void Test_IsValueValid_TwoPropRule_InValidValue_FirstRulePass_SecondRuleFail()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);
            propDef.AddPropRule(new PropRuleString("StringRule", "Rule 1", 1, 3, ""));
            propDef.AddPropRule(new PropRuleString("StringRule", "Rule 2", 3, 10, ""));

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, propDef.PropRules.Count);

            //---------------Execute Test ----------------------
            string errMsg = "";
            bool valid = propDef.IsValueValid("AB", ref errMsg);

            //---------------Test Result -----------------------
            Assert.IsFalse(valid);
            StringAssert.Contains("'AB' for property 'Prop Name' is not valid for the rule 'StringRule'", errMsg);
            StringAssert.Contains("Rule 2", errMsg);
        }
        [Test]
        public void Test_IsValueValid_TwoPropRule_InValidValue_FirstRulePass_FirstRuleFail()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);
            propDef.AddPropRule(new PropRuleString("StringRule", "Rule 1", 1, 3, ""));
            propDef.AddPropRule(new PropRuleString("StringRule", "Rule 2", 3, 10, ""));

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, propDef.PropRules.Count);

            //---------------Execute Test ----------------------
            string errMsg = "";
            bool valid = propDef.IsValueValid("Long String", ref errMsg);

            //---------------Test Result -----------------------
            Assert.IsFalse(valid);
            StringAssert.Contains("'Long String' for property 'Prop Name' is not valid for the rule 'StringRule'", errMsg);
            StringAssert.Contains("Rule 1", errMsg);
        }
        [Test]
        public void Test_AddPropRule_Null()
        {
            //---------------Set up test pack-------------------
            IPropDef propDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, propDef.PropRules.Count);

            //---------------Execute Test ----------------------

            try
            {
                propDef.AddPropRule(null);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroApplicationException ex)
            {
                StringAssert.Contains("You cannot add a null property rule to a property def", ex.Message);
            }
        }

        [Test]
        public void Test_SetDisplayName()
        {
            //---------------Set up test pack-------------------
            IPropDef propDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string displayName = propDef.DisplayName;

            //---------------Test Result -----------------------
            Assert.AreEqual("Prop Name", displayName);
        }

        [Test]
        public void TestConvertValueToPropertyType_DateTimeAcceptsDateTimeNow()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("a", typeof (DateTime), PropReadWriteRule.ReadWrite, null);
            DateTimeNow dateTimeToday = new DateTimeNow();

            //---------------Execute Test ----------------------
            object convertedDateTimeValue;
            bool parsed = propDef.TryParsePropValue(dateTimeToday, out convertedDateTimeValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parsed);
            Assert.IsInstanceOf(typeof (DateTimeNow), convertedDateTimeValue);
            Assert.AreSame(dateTimeToday, convertedDateTimeValue);
        }

        [Test]
        public void TestConvertValueToPropertyType_DateTimeAcceptsDateTimeToday()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("a", typeof (DateTime), PropReadWriteRule.ReadWrite, null);
            DateTimeToday dateTimeToday = new DateTimeToday();

            //---------------Execute Test ----------------------
            object convertedDateTimeValue;
            bool parsed = propDef.TryParsePropValue(dateTimeToday, out convertedDateTimeValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parsed);
            Assert.IsInstanceOf(typeof (DateTimeToday), convertedDateTimeValue);
            Assert.AreSame(dateTimeToday, convertedDateTimeValue);
        }

        [Test]
        public void TestConvertValueToPropertyType_IntToString()
        {
            //-------------Setup Test Pack ------------------
            PropDef propDef = new PropDef("a", typeof (string), PropReadWriteRule.ReadWrite, null);
            const int valueToParse = 100;

            //-------------Execute test ---------------------
            object convertedIntValue;
            bool parsed = propDef.TryParsePropValue(valueToParse, out convertedIntValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parsed);
            Assert.IsInstanceOf(typeof (String), convertedIntValue);
            Assert.AreEqual(valueToParse.ToString(), convertedIntValue);
        }

        [Test]
        public void TestConvertValueToPropertyType_StringToDateTime()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("a", typeof (DateTime), PropReadWriteRule.ReadWrite, null);
            const string dateTimeString = "01 Jan 2000 01:30:45";
            DateTime dateTime = DateTime.Parse(dateTimeString);

            //---------------Execute Test ----------------------
            object convertedDateTimeValue;
            bool parsed = propDef.TryParsePropValue(dateTimeString, out convertedDateTimeValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parsed);
            Assert.IsInstanceOf(typeof (DateTime), convertedDateTimeValue);
            Assert.AreEqual(dateTime, convertedDateTimeValue);
        }

        [Test]
        public void TestConvertValueToPropertyType_StringToGuid()
        {
            //-------------Setup Test Pack ------------------
            PropDef propDef = new PropDef("a", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            string guidString = Guid.NewGuid().ToString("B");
            //-------------Execute test ---------------------
            object convertedGuid;
            bool parsed = propDef.TryParsePropValue(guidString, out convertedGuid);

            //---------------Test Result -----------------------
            Assert.IsTrue(parsed);
            Assert.IsInstanceOf(typeof (Guid), convertedGuid);
            Assert.AreEqual(new Guid(guidString), convertedGuid);
        }

        [Test]
        public void TestConvertValueToPropertyType_NowStringToDateTimeNow()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("a", typeof (DateTime), PropReadWriteRule.ReadWrite, null);
            const string dateTimeString = "Now";
            DateTimeNow dateTimeToday = new DateTimeNow();

            //---------------Execute Test ----------------------
            object convertedDateTimeValue;
            bool parsed = propDef.TryParsePropValue(dateTimeString, out convertedDateTimeValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parsed);
            Assert.IsInstanceOf(typeof (DateTimeNow), convertedDateTimeValue);
            Assert.AreEqual(dateTimeToday.ToString(), convertedDateTimeValue.ToString());
        }

        [Test]
        public void TestConvertValueToPropertyType_NowStringToDateTimeNow_VariedCase()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("a", typeof (DateTime), PropReadWriteRule.ReadWrite, null);
            const string dateTimeString = "NoW";
            DateTimeNow dateTimeToday = new DateTimeNow();

            //---------------Execute Test ----------------------
            object convertedDateTimeValue;
            bool parsed = propDef.TryParsePropValue(dateTimeString, out convertedDateTimeValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parsed); 
            Assert.IsInstanceOf(typeof(DateTimeNow), convertedDateTimeValue);
            Assert.AreEqual(dateTimeToday.ToString(), convertedDateTimeValue.ToString());
        }

        [Test]
        public void TestConvertValueToPropertyType_TodayStringToDateTimeToday()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("a", typeof (DateTime), PropReadWriteRule.ReadWrite, null);
            const string dateTimeString = "Today";
            DateTimeToday dateTimeToday = new DateTimeToday();

            //---------------Execute Test ----------------------
            object convertedDateTimeValue;
            bool parsed = propDef.TryParsePropValue(dateTimeString, out convertedDateTimeValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parsed); 
            Assert.IsInstanceOf(typeof(DateTimeToday), convertedDateTimeValue);
            Assert.AreEqual(dateTimeToday, convertedDateTimeValue);
        }

        [Test]
        public void TestConvertValueToPropertyType_TodayStringToDateTimeToday_VariedCase()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("a", typeof (DateTime), PropReadWriteRule.ReadWrite, null);
            const string dateTimeString = "ToDaY";
            DateTimeToday dateTimeToday = new DateTimeToday();

            //---------------Execute Test ----------------------
            object convertedDateTimeValue;
            bool parsed = propDef.TryParsePropValue(dateTimeString, out convertedDateTimeValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parsed);
            Assert.IsInstanceOf(typeof(DateTimeToday), convertedDateTimeValue);
            Assert.AreEqual(dateTimeToday, convertedDateTimeValue);
        }

        [Test]
        public void TestCreateBOProp()
        {
            IBOProp prop = _propDef.CreateBOProp(false);
            Assert.AreEqual("PropName", prop.PropertyName);
            Assert.AreEqual("PropName", prop.DatabaseFieldName);
        }
#pragma warning disable 168
        [Test]
        public void TestCreatePropDefInvalidDefault_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            PropDef lPropDef = new PropDef("prop", "System", "Int32", PropReadWriteRule.ReadWrite, null, "ddd", false,
                                           false);
            //---------------Execute Test ----------------------
            try
            {

                var defaultValue = lPropDef.DefaultValue;
                Assert.Fail("Expected to throw an FormatException");
            }
                //---------------Test Result -----------------------
            catch (InvalidCastException ex)
            {
                StringAssert.Contains("for property 'prop' is not valid. It is not a type of", ex.Message);
            }
        }
#pragma warning restore 168
        [Test]
        public void TestCreateLatePropDefInvalidDefaultNotAccessed()
        {
            new PropDef("prop", "System", "Int32", PropReadWriteRule.ReadWrite, null, "", false, false);
            //No error
        }

        [Test]
        public void TestCreateLatePropDefInvalidType()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("prop", "NonExistentAssembly", "NonExistentType", PropReadWriteRule.ReadWrite,
                                          null, "", false, false);
            //---------------Execute Test ----------------------
            try
            {
                Type propType = propDef.PropertyType;
                Assert.Fail("Expected to throw an UnknownTypeNameException");
            }
                //---------------Test Result -----------------------
            catch (UnknownTypeNameException ex)
            {
                StringAssert.Contains("Unable to load the property type while attempting to load a property definition", ex.Message);
            }
        }

        [Test]
        public void TestCreateLatePropDefInvalidTypeNotAccessed()
        {
            new PropDef("prop", "NonExistentAssembly", "NonExistentType", PropReadWriteRule.ReadWrite, null, "", false,
                        false);
        }

        [Test]
        public void TestCreatePropDef()
        {
            Assert.AreEqual("PropName", _propDef.PropertyName);
            Assert.AreEqual("PropName", _propDef.DatabaseFieldName);
            Assert.AreEqual(typeof (string), _propDef.PropType);
            new PropDef("prop", typeof (int), PropReadWriteRule.ReadWrite, 1);
        }

        [Test]
        public void TestCreatePropDefInvalidDefault()
        {
            //---------------Execute Test ----------------------
            try
            {
                new PropDef("prop", typeof (int), PropReadWriteRule.ReadWrite, "fdsafasd");
                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("for property 'prop' is not valid. It is not a type of System.Int32", ex.Message);
            }
        }

        [Test]
        public void Test_ConstructEnumProp_WithDefaultValueString_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            var type = typeof(ContactPersonTestBO.ContactType);
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var propDef = new PropDef("EnumProp", type.Assembly.FullName, type.Name,
                              PropReadWriteRule.ReadWrite,"EnumField", "Family", false, false);
            //---------------Test Result -----------------------
            Assert.AreEqual("Family", propDef.DefaultValueString);
            Assert.AreEqual(ContactPersonTestBO.ContactType.Family, propDef.DefaultValue);
        }
        [Test]
        public void TestCreatePropDefWithEnumType()
        {
            PropDef propDef = new PropDef("EnumProp", typeof (ContactPersonTestBO.ContactType),
                                          PropReadWriteRule.ReadWrite, ContactPersonTestBO.ContactType.Family);
            Assert.AreEqual("Habanero.Test.BO", propDef.PropertyTypeAssemblyName);
            Assert.AreEqual("Habanero.Test.BO.ContactPersonTestBO+ContactType", propDef.PropertyTypeName);
            Assert.AreEqual("Family", propDef.DefaultValueString);
            Assert.AreEqual(ContactPersonTestBO.ContactType.Family, propDef.DefaultValue);
        }

        [Test]
        public void TestCreatePropDefWithEnumTypeString()
        {
            PropDef propDef = new PropDef("EnumProp", "Habanero.Test.BO", "ContactPersonTestBO+ContactType",
                                          PropReadWriteRule.ReadWrite, "EnumProp", "Family", false, false);
            Assert.AreEqual(typeof (ContactPersonTestBO.ContactType), propDef.PropertyType);
            Assert.AreEqual("Habanero.Test.BO", propDef.PropertyTypeAssemblyName);
            Assert.AreEqual("Habanero.Test.BO.ContactPersonTestBO+ContactType", propDef.PropertyTypeName);
            Assert.AreEqual("Family", propDef.DefaultValueString);
            Assert.AreEqual(ContactPersonTestBO.ContactType.Family, propDef.DefaultValue);
        }

        [Test]
        public void TestDashIsNotAllowedInName()
        {
            //---------------Execute Test ----------------------
            try
            {
                new PropDef("This-That", typeof (string), PropReadWriteRule.ReadWrite, "");
                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("A property name cannot contain any of the following characters: [.-|]  Invalid property name This-That", ex.Message);
            }
        }

        [Test]
        public void TestDotIsNotAllowedInName()
        {
            //---------------Execute Test ----------------------
            try
            {
                new PropDef("This.That", typeof (string), PropReadWriteRule.ReadWrite, "");
                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("A property name cannot contain any of the following characters: [.-|]  Invalid property name This.That", ex.Message);
            }
        }

        [Test]
        public void TestGetComparer()
        {
            PropDef propDef = new PropDef("prop", typeof (string), PropReadWriteRule.ReadWrite, null);
            Assert.AreEqual("PropertyComparer`2", propDef.GetPropertyComparer<MyBO>().GetType().Name);
        }

        [Test]
        public void TestPipeIsNotAllowedInName()
        {
            //---------------Execute Test ----------------------
            try
            {
                new PropDef("This|That", typeof (string), PropReadWriteRule.ReadWrite, "");
                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("A property name cannot contain any of the following characters: [.-|]  Invalid property name This|That", ex.Message);
            }
        }

        [Test]
        public void TestProtectedSets()
        {
            PropDefFake propDef = new PropDefFake();

            Assert.AreEqual("prop", propDef.PropertyName);
            propDef.SetPropertyName("myprop");
            Assert.AreEqual("myprop", propDef.PropertyName);

            Assert.AreEqual(typeof (MyBO), propDef.PropertyType);
            propDef.SetPropertyType(typeof (MyRelatedBo));
            Assert.AreEqual(typeof (MyRelatedBo), propDef.PropertyType);

            Assert.AreEqual("Habanero.Test.MyRelatedBo", propDef.PropertyTypeName);
            propDef.SetPropertyTypeName("myproptype");
            Assert.AreEqual("myproptype", propDef.PropertyTypeName);

            Assert.AreEqual("Habanero.Test", propDef.PropertyTypeAssemblyName);
            propDef.SetPropertyTypeAssemblyName("myassembly");
            Assert.AreEqual("myassembly", propDef.PropertyTypeAssemblyName);
            Assert.IsNull(propDef.PropertyTypeName);
            Assert.IsNull(propDef.PropertyType);

            Assert.AreEqual("prop", propDef.DatabaseFieldName);
            propDef.SetDatabaseFieldName("propfield");
            Assert.AreEqual("propfield", propDef.DatabaseFieldName);

            Assert.IsNull(propDef.DefaultValue);
            propDef.SetPropertyType(typeof (String));
            propDef.SetDefaultValue("default");
            Assert.AreEqual("default", propDef.DefaultValue);

            Assert.AreEqual("default", propDef.DefaultValueString);
            propDef.SetDefaultValueString("none");
            Assert.AreEqual("none", propDef.DefaultValueString);

            Assert.IsFalse(propDef.Compulsory);
            propDef.SetCompulsory(true);
            Assert.IsTrue(propDef.Compulsory);

            Assert.AreEqual(PropReadWriteRule.ReadWrite, propDef.ReadWriteRule);
            propDef.SetReadWriteRule(PropReadWriteRule.ReadOnly);
            Assert.AreEqual(PropReadWriteRule.ReadOnly, propDef.ReadWriteRule);

            Assert.IsTrue(propDef.IsPropValueValid("somestring"));

            Assert.AreEqual(typeof (String), propDef.PropType);
            propDef.SetPropType(typeof (DateTime));
            Assert.AreEqual(typeof (DateTime), propDef.PropType);

            PropDefParameterSQLInfo propDefParameterSQLInfo = new PropDefParameterSQLInfo(propDef);
            Assert.AreEqual(ParameterType.Date, propDefParameterSQLInfo.ParameterType);
        }

        // Used to access protected properties

        [Test]
        public void Test_Clone()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("TestPropDef","System","String",PropReadWriteRule.ReadWrite, "TestPropDef","TestString",true,false);
            
            //---------------Execute Test ----------------------
            PropDef clonedPropDef = (PropDef) propDef.Clone();
            //-----Test PreCondition--------------------------
            Assert.AreEqual(propDef.PropertyTypeAssemblyName, clonedPropDef.PropertyTypeAssemblyName);
            Assert.AreEqual(propDef.PropertyType, clonedPropDef.PropertyType);
            Assert.AreEqual(propDef.PropertyTypeName, clonedPropDef.PropertyTypeName);
            Assert.AreEqual(propDef.Compulsory, clonedPropDef.Compulsory);
            Assert.AreEqual(propDef.DefaultValueString, clonedPropDef.DefaultValueString);
            Assert.AreEqual(propDef.ReadWriteRule, clonedPropDef.ReadWriteRule);
            Assert.IsTrue(propDef.Equals(clonedPropDef), "Cloned prop def should equal orig propdef");

            //-----------Execute------------------------------
            clonedPropDef.Compulsory = false;
            clonedPropDef.DefaultValueString = "ClonedString";
            clonedPropDef.ReadWriteRule = PropReadWriteRule.WriteOnce;

            //---------------Test Result -----------------------
            Assert.AreNotEqual(propDef.Compulsory,clonedPropDef.Compulsory);
            Assert.AreNotEqual(propDef.DefaultValueString,clonedPropDef.DefaultValueString);
            Assert.AreNotEqual(propDef.ReadWriteRule,clonedPropDef.ReadWriteRule);
            Assert.IsFalse(propDef.Equals(clonedPropDef));
        }

        [Test]
        public void Test_Cloned_Equals_True()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("TestPropDef", "System", "String", PropReadWriteRule.ReadWrite, "TestPropDef", "TestString", true, false);
            IPropDef clonedPropDef = propDef.Clone();

            //-----Test PreCondition--------------------------
            //---------------Execute Test ----------------------
            bool equals = propDef.Equals(clonedPropDef);
            //---------------Test Result -----------------------
            Assert.IsTrue(equals, "Cloned def should be equal");
        }
        [Test]
        public void Test_Equals_False()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("TestPropDef", "System", "String", PropReadWriteRule.ReadWrite, "TestPropDef", "TestString", true, false);
            IPropDef clonedPropDef = propDef.Clone();

            //-----Test PreCondition--------------------------
            //---------------Execute Test ----------------------
            clonedPropDef.Compulsory = false;
            clonedPropDef.DefaultValueString = "ClonedString";
            clonedPropDef.ReadWriteRule = PropReadWriteRule.WriteOnce;
            bool equals = clonedPropDef.Equals(propDef);
            //---------------Test Result -----------------------
            Assert.IsFalse(equals);
        }

        [Test]
        public void TestCreateBOProp_CorrectPropCreated()
        {            
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null);

            //-----Test PreCondition--------------------------
            Assert.IsFalse(propDef.HasLookupList());

            //---------------Execute Test ----------------------

            IBOProp prop = propDef.CreateBOProp(false);
            //---------------Test Result -----------------------

            Assert.IsInstanceOf(typeof(BOProp), prop);

        }

        [Test]
        public void TestCreateBOPropWithLookupList_CorrectPropCreated()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null);
            Dictionary<string, string> collection = new Dictionary<string, string>();
            SimpleLookupList simpleLookupList = new SimpleLookupList(collection);
            propDef.LookupList = simpleLookupList;
            //-----Test PreCondition--------------------------
            Assert.IsTrue(propDef.HasLookupList());
            //---------------Execute Test ----------------------

            IBOProp prop = propDef.CreateBOProp(false);
            //---------------Test Result -----------------------

            Assert.IsInstanceOf(typeof(BOPropLookupList), prop);
        }  

        [Test]
        public void TestCreateBOProp_CorrectPropCreated_WithDefaultValue()
        {            
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null,99);

            //-----Test PreCondition--------------------------
            Assert.IsFalse(propDef.HasLookupList());

            //---------------Execute Test ----------------------

            IBOProp prop = propDef.CreateBOProp(true);
            //---------------Test Result -----------------------

            Assert.IsInstanceOf(typeof(BOProp), prop);
        }

        [Test]
        public void TestCreateBOPropWithLookupList_CorrectPropCreated_WithDefaultValue()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null,99);
            Dictionary<string, string> collection = new Dictionary<string, string>();
            SimpleLookupList simpleLookupList = new SimpleLookupList(collection);
            propDef.LookupList = simpleLookupList;
            //-----Test PreCondition--------------------------
            Assert.IsTrue(propDef.HasLookupList());
            //---------------Execute Test ----------------------
            IBOProp prop = propDef.CreateBOProp(true);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(BOPropLookupList), prop);
        }

        [Test]
        public void Test_TestLookupListCriteria()
        {
            //---------------Set up test pack-------------------
            PropDefStub propDef = new PropDefStub("PropName", typeof(int), PropReadWriteRule.ReadWrite, null, 99);
            Dictionary<string, string> collection = new Dictionary<string, string>();
            SimpleLookupList simpleLookupList = new SimpleLookupList(collection);
            propDef.LookupList = simpleLookupList;

            //---------------Assert Precondition----------------
            Assert.IsTrue(propDef.HasLookupList());
            Assert.AreEqual(0, propDef.LookupList.GetLookupList().Count);
            Assert.IsFalse(propDef.LookupList.LimitToList);
            //---------------Execute Test ----------------------
            string errorMessage = "";
            bool isItemInList = propDef.CallIsLookupListItemValid(TestUtil.GetRandomInt(), ref errorMessage);
            //---------------Test Result -----------------------
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "The error message should be null since limit to list is false");
            Assert.IsTrue(isItemInList);
        }

        [Test]
        public void Test_GetNewValue_WhenValueNull_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            PropDefStub propDef = new PropDefStub("PropName", typeof(int), PropReadWriteRule.ReadWrite, null, 99);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var newValue = propDef.GetNewValue(null);
            //---------------Test Result -----------------------
            Assert.IsNull(newValue);
        }

        [Test]
        public void Test_SetLookupList_ToNull_ShouldNotCauseError()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null, 99)
                                  {
                                      LookupList = new SimpleLookupList(new Dictionary<string, string>())
                                  };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(propDef.LookupList);
            //---------------Execute Test ----------------------
            propDef.LookupList = null;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<NullLookupList>(propDef.LookupList);
        }

        [Test]
        public void Test_ClassName_WhenHasClassDef_ShouldReturnClassDef_ClassName()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDefFake {ClassDef = new FakeClassDef{ClassName = GetRandomString()}};
            //---------------Assert Precondition----------------
            Assert.IsNotNull(propDef.ClassDef);
            //---------------Execute Test ----------------------
            string className = propDef.ClassName;
            //---------------Test Result -----------------------
            Assert.AreEqual(propDef.ClassDef.ClassName, className);
        }

        [Test]
        public void Test_ClassName_WhenNotHasClassDef_ShouldReturnEmptyString()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDefFake();
            //---------------Assert Precondition----------------
            Assert.IsNull(propDef.ClassDef);
            //---------------Execute Test ----------------------
            string className = propDef.ClassName;
            //---------------Test Result -----------------------
            Assert.IsEmpty(className);
        }
        [Test]
        public void Test_ClassNameViaInterface_WhenNotHasClassDef_ShouldReturnEmptyString()
        {
            //---------------Set up test pack-------------------
            IPropDef propDef = new PropDefFake();
            //---------------Assert Precondition----------------
            Assert.IsNull(propDef.ClassDef);
            //---------------Execute Test ----------------------
            string className = propDef.ClassName;
            //---------------Test Result -----------------------
            Assert.IsEmpty(className);
        }
        [Test]
        public void Test_ConstructPropDef_WithDefaultValueForCustomType_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            const string expectedValue = "xxxx.yyyy@ccc.aa.zz";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var propDef = new PropDef("Name", typeof (EMailAddressAsCustomProperty), PropReadWriteRule.ReadWrite, "DD",
                                      expectedValue, false, false);
            //---------------Assert Precondition----------------
            Assert.AreEqual(expectedValue, propDef.DefaultValue.ToString());
            Assert.AreEqual(expectedValue, propDef.DefaultValueString);
        }

        // ReSharper restore InconsistentNaming
        private static string GetRandomString()
        {
            return TestUtil.GetRandomString();
        }

        class PropDefStub : PropDef
        {
            public PropDefStub(string propertyName, Type propType, PropReadWriteRule propRWStatus, string databaseFieldName, object defaultValue) : base(propertyName, propType, propRWStatus, databaseFieldName, defaultValue)
            {
            }

            public PropDefStub(string propertyName, Type propType, PropReadWriteRule propRWStatus, object defaultValue) : base(propertyName, propType, propRWStatus, defaultValue)
            {
            }

            public PropDefStub(string propertyName, string assemblyName, string typeName, PropReadWriteRule propRWStatus, string databaseFieldName, string defaultValueString, bool compulsory, bool autoIncrementing) : base(propertyName, assemblyName, typeName, propRWStatus, databaseFieldName, defaultValueString, compulsory, autoIncrementing)
            {
            }

            internal PropDefStub(string propertyName, string assemblyName, string typeName, PropReadWriteRule propRWStatus, string databaseFieldName, string defaultValueString, bool compulsory, bool autoIncrementing, int length) : base(propertyName, assemblyName, typeName, propRWStatus, databaseFieldName, defaultValueString, compulsory, autoIncrementing, length)
            {
            }

            public PropDefStub(string propertyName, string assemblyName, string typeName, PropReadWriteRule propRWStatus, string databaseFieldName, string defaultValueString, bool compulsory, bool autoIncrementing, int length, string displayName, string description, bool keepValuePrivate) : base(propertyName, assemblyName, typeName, propRWStatus, databaseFieldName, defaultValueString, compulsory, autoIncrementing, length, displayName, description, keepValuePrivate)
            {
            }

            public PropDefStub(string propertyName, Type propType, PropReadWriteRule propRWStatus, string databaseFieldName, object defaultValue, bool compulsory, bool autoIncrementing, int length, string displayName, string description) : base(propertyName, propType, propRWStatus, databaseFieldName, defaultValue, compulsory, autoIncrementing, length, displayName, description)
            {
            }

            public PropDefStub(string propertyName, Type propType, PropReadWriteRule propRWStatus, string databaseFieldName, object defaultValue, bool compulsory, bool autoIncrementing) : base(propertyName, propType, propRWStatus, databaseFieldName, defaultValue, compulsory, autoIncrementing)
            {
            }

            internal PropDefStub(string propertyName, Type propType, PropReadWriteRule propRWStatus, string databaseFieldName, object defaultValue, bool compulsory, bool autoIncrementing, int length, string displayName, string description, bool keepValuePrivate) : base(propertyName, propType, propRWStatus, databaseFieldName, defaultValue, compulsory, autoIncrementing, length, displayName, description, keepValuePrivate)
            {
            }
            public bool CallIsLookupListItemValid(object propValue, ref string errorMessage)
            {
                return this.IsLookupListItemValid(propValue, ref errorMessage);
            }
        }
    }

    public class PropDefFake : PropDef
    {
        public PropDefFake()
            : base("prop", typeof(MyBO), PropReadWriteRule.ReadWrite, null)
        {
        }
        public PropDefFake(string propName)
            : base(propName, typeof(MyBO), PropReadWriteRule.ReadWrite, null)
        {
        }
        public void SetPropertyName(string name)
        {
            PropertyName = name;
        }

        public void SetPropertyTypeAssemblyName(string name)
        {
            PropertyTypeAssemblyName = name;
        }

        public void SetPropertyTypeName(string name)
        {
            PropertyTypeName = name;
        }

        public void SetPropertyType(Type type)
        {
            PropertyType = type;
        }

        public void SetDatabaseFieldName(string name)
        {
            DatabaseFieldName = name;
        }

        public void SetDefaultValue(object value)
        {
            DefaultValue = value;
        }

        public void SetDefaultValueString(string value)
        {
            DefaultValueString = value;
        }

        public void SetCompulsory(bool value)
        {
            Compulsory = value;
        }

        public void SetReadWriteRule(PropReadWriteRule rule)
        {
            ReadWriteRule = rule;
        }

#pragma warning disable 168
        public bool IsPropValueValid(object value)
        {
            string errors = "";
            return IsValueValid("test", ref errors);
        }
#pragma warning restore 168

        public void SetPropType(Type type)
        {
            PropType = type;
        }
    }
}