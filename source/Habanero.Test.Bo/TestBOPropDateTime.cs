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

using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBoPropDateTime 
    {
        private PropDef _propDef;
        private const string _standardDateTimeFormat = "dd MMM yyyy HH:mm:ss:fff";

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            _propDef = new PropDef("PropName", typeof(DateTime), PropReadWriteRule.ReadWrite, null);
        }

        [Test]
        public void Test_InitialiseProp_NullValue()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(DateTime), boProp.PropertyType);
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(null);
            //---------------Test Result -----------------------
            Assert.IsNull(boProp.Value);
        }

        [Test]
        public void Test_InitialiseProp_ValidDateTime()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            DateTime value = DateTime.MinValue.AddDays(1);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(value);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(value, boProp.Value);
        }

        [Test]
        public void Test_InitialiseProp_ValidDateTimeString()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(expectedDateTime.ToString("d"));
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(expectedDateTime, boProp.Value);
            Assert.IsTrue(boProp.Value is DateTime, "Value should be a expectedDateTime");
        }
        //If try initialise a property with invalid property types is set to the property then
        //  An error is raised. Stating the error reason.
        //  The property value will be set to the previous property value.
        //  The property is not changed to be in an invalid state. The prop invalid reason is not set.
        [Test]
        public void Test_Initialise_InvalidDateTimeString()
        {
            BOProp boProp = new BOProp(_propDef);
            const string invalid = "Invalid";
            PropDef propDef = (PropDef) boProp.PropDef;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(DateTime), propDef.PropertyType);
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            try
            {
                boProp.InitialiseProp(invalid);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains(boProp.PropertyName + " cannot be set to " + invalid, ex.Message);
                StringAssert.Contains("It is not a type of ", ex.Message);
                StringAssert.Contains("DateTime", ex.Message);
                Assert.AreEqual(null, boProp.Value);
                Assert.IsTrue(boProp.IsValid);
            }
        }

        //If try construct a property with invalid property default value type then
        //  An error is raised. Stating the error reason.
        //  The property value will be set to the previous property value.
        //  The property is not set to be invalid.
        [Test]
        public void Test_ConstructWithDefault_InvalidDateTimeString()
        {
            string invalid = "";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                invalid = "Invalid";
                new BOProp(_propDef, invalid);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains(" cannot be set to " + invalid, ex.Message);
                StringAssert.Contains("It is not a type of ", ex.Message);
                StringAssert.Contains("DateTime", ex.Message);
            }
        }

        [Test]
        public void Test_InialiseProp_DBNUll()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(DBNull.Value);
            //---------------Test Result -----------------------
            Assert.IsNull(boProp.Value);
        }

        [Test]
        public void Test_SetValue_DBNull()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            //---------------Execute Test ----------------------
            boProp.Value = DBNull.Value;
            //---------------Test Result -----------------------
            Assert.IsNull(boProp.Value);
            Assert.IsTrue(boProp.IsValid);
        }

        [Test]
        public void Test_SetValue_Null()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            const string nullValue = null;
            //---------------Execute Test ----------------------
            boProp.Value = nullValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(nullValue, boProp.Value);
            Assert.IsTrue(boProp.IsValid);
        }

        [Test]
        public void Test_SetValue_ValidDateTime()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);

            //---------------Execute Test ----------------------
            boProp.Value = expectedDateTime;
            //---------------Test Result -----------------------
            Assert.IsTrue(boProp.IsValid);
            Assert.AreEqual(expectedDateTime, boProp.Value);
        }

        [Test]
        public void Test_SetValue_ValidDateTimeString_d()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);
            //---------------Execute Test ----------------------
            boProp.Value = expectedDateTime.ToString("d");
            //---------------Test Result -----------------------
            Assert.IsTrue(boProp.IsValid);
            Assert.AreEqual(expectedDateTime, boProp.Value);
            Assert.IsTrue(boProp.Value is DateTime);
        }

        //If an invalid property types is set to the property then
        //  An error is raised. Stating the error reason.
        //  The property value will be set to the previous property value.
        //  The property is not changed to be in an invalid state. The prop invalid reason is not set.
        [Test]
        public void Test_SetValue_InvalidDateTimeString()
        {
            BOProp boProp = new BOProp(_propDef);
            const string invalid = "Invalid";
            object originalPropValue = DateTime.MinValue.AddDays(1);
            boProp.Value = originalPropValue;
            PropDef propDef = (PropDef) boProp.PropDef;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(DateTime), propDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            try
            {
                boProp.Value = invalid; //expectedGuid.ToString("B");
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains(boProp.PropertyName + " cannot be set to " + invalid, ex.Message);
                StringAssert.Contains("It is not a type of ", ex.Message);
                StringAssert.Contains("DateTime", ex.Message);
                Assert.AreEqual(originalPropValue, boProp.Value);
                Assert.IsTrue(boProp.IsValid);
            }
        }

        [Test]
        public void Test_PersistedPropertyValueString_Null()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            boProp.InitialiseProp(DBNull.Value);
            boProp.Value = DateTime.MaxValue;

            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.PersistedPropertyValue);

            //---------------Execute Test ----------------------
            string persistedPropertyValueString = boProp.PersistedPropertyValueString;
            //---------------Test Result -----------------------
            Assert.AreEqual("", persistedPropertyValueString, "Null persisted prop value should return null string");
        }

        [Test]
        public void Test_PersistedPropertyValueString_ValidDateTime()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);
            boProp.InitialiseProp(expectedDateTime);
            boProp.Value = DateTime.MaxValue;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsNotNull(boProp.PersistedPropertyValue);

            //---------------Execute Test ----------------------
            string persistedPropertyValueString = boProp.PersistedPropertyValueString;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDateTime.ToString(_standardDateTimeFormat), persistedPropertyValueString);
        }

        [Test]
        public void Test_PropertyValueString_Null()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            boProp.InitialiseProp(DBNull.Value);

            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);

            //---------------Execute Test ----------------------
            string propertyValueString = boProp.PropertyValueString;

            //---------------Test Result -----------------------
            Assert.AreEqual("", propertyValueString, "Null persisted prop value should return null string");
        }

        [Test]
        public void Test_PropertyValueString_ValidDateTime()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);
            boProp.InitialiseProp(expectedDateTime);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);

            //---------------Execute Test ----------------------
            string propertyValueString = boProp.PropertyValueString;

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDateTime.ToString(_standardDateTimeFormat), propertyValueString);
        }

        [Test]
        public void Test_PropertyValueString_ValidDateTimeString()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);
            boProp.Value = expectedDateTime.ToString("d");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsTrue(boProp.Value is DateTime);
            //---------------Execute Test ----------------------
            string propertyValueString = boProp.PropertyValueString;

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDateTime.ToString(_standardDateTimeFormat), propertyValueString);
        }


        
    }
}