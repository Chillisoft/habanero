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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBOPropInteger
    {
        private PropDef _propDef;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            _propDef = new PropDef("PropName", typeof(Int32), PropReadWriteRule.ReadWrite, null);
        }

        [Test]
        public void Test_InitialiseProp_NullValue()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(Int32), boProp.PropertyType);
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(null);
            //---------------Test Result -----------------------
            Assert.IsNull(boProp.Value);
        }

        [Test]
        public void Test_InitialiseProp_ValidInteger()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Int32 value = TestUtil.GetRandomInt();
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(value);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(value, boProp.Value);
        }

        [Test]
        public void Test_InitialiseProp_ValidIntegerString()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Int32 expectedInteger = TestUtil.GetRandomInt();
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(expectedInteger.ToString("d"));
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(expectedInteger, boProp.Value);
            Assert.IsTrue(boProp.Value is Int32, "Value should be an Integer");
        }

        //If try initialise a property with invalid property types is set to the property then
        //  An error is raised. Stating the error reason.
        //  The property value will be set to the previous property value.
        //  The property is not changed to be in an invalid state. The prop invalid reason is not set.
        [Test]
        public void Test_Initialise_InvalidIntegerString()
        {
            BOProp boProp = new BOProp(_propDef);
            const string invalid = "Invalid";
            PropDef propDef = (PropDef) boProp.PropDef;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(Int32), propDef.PropertyType);
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
                StringAssert.Contains("Int32", ex.Message);
                Assert.AreEqual(null, boProp.Value);
                Assert.IsTrue(boProp.IsValid);
            }
        }

        [Test]
        public void Test_InitialiseProp_WithDecimal_Max()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            const decimal value = decimal.MaxValue;
            decimal expectedInteger = Math.Round(value);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            try
            {
                boProp.InitialiseProp(value);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains(boProp.PropertyName + " cannot be set to " + value, ex.Message);
                StringAssert.Contains("It is not a type of ", ex.Message);
                StringAssert.Contains("Int32", ex.Message);
                Assert.AreEqual(null, boProp.Value);
                Assert.IsTrue(boProp.IsValid);
            }
        }

        [Test]
        public void Test_InitialiseProp_WithDecimal_NoRoundingNecessary()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            const decimal value = 100.00m;
            decimal expectedInteger = Math.Round(value);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(value);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(expectedInteger, boProp.Value);
            Assert.IsTrue(boProp.Value is Int32, "Value should be an Integer");
        }

        [Test]
        public void Test_InitialiseProp_WithDecimal_RoundUp()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            const decimal value = 123.5m;
            decimal expectedInteger = Math.Round(value);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(value);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(expectedInteger, boProp.Value);
            Assert.IsTrue(boProp.Value is Int32, "Value should be an Integer");
        }

        [Test]
        public void Test_InitialiseProp_WithDecimal_RoundDown()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            const decimal value = 321.49m;
            decimal expectedInteger = Math.Round(value);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(value);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(expectedInteger, boProp.Value);
            Assert.IsTrue(boProp.Value is Int32, "Value should be an Integer");
        }

        //If try construct a property with invalid property default value type then
        //  An error is raised. Stating the error reason.
        //  The property value will be set to the previous property value.
        //  The property is not set to be invalid.
        [Test]
        public void Test_ConstructWithDefault_InvalidIntegerString()
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
                StringAssert.Contains("Int32", ex.Message);
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
        public void Test_SetValue_ValidInteger()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Int32 expectedInteger = TestUtil.GetRandomInt();

            //---------------Execute Test ----------------------
            boProp.Value = expectedInteger;
            //---------------Test Result -----------------------
            Assert.IsTrue(boProp.IsValid);
            Assert.AreEqual(expectedInteger, boProp.Value);
        }

        [Test]
        public void Test_SetValue_ValidIntegerString_d()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Int32 expectedInteger = TestUtil.GetRandomInt();
            //---------------Execute Test ----------------------
            boProp.Value = expectedInteger.ToString("d");
            //---------------Test Result -----------------------
            Assert.IsTrue(boProp.IsValid);
            Assert.AreEqual(expectedInteger, boProp.Value);
            Assert.IsTrue(boProp.Value is Int32);
        }

        //If an invalid property types is set to the property then
        //  An error is raised. Stating the error reason.
        //  The property value will be set to the previous property value.
        //  The property is not changed to be in an invalid state. The prop invalid reason is not set.
        [Test]
        public void Test_SetValue_InvalidIntegerString()
        {
            BOProp boProp = new BOProp(_propDef);
            const string invalid = "Invalid";
            object originalPropValue = TestUtil.GetRandomInt();
            boProp.Value = originalPropValue;
            PropDef propDef = (PropDef) boProp.PropDef;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(Int32), propDef.PropertyType);
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
                StringAssert.Contains("Int32", ex.Message);
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
            boProp.Value = TestUtil.GetRandomInt();

            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.PersistedPropertyValue);

            //---------------Execute Test ----------------------
            string persistedPropertyValueString = boProp.PersistedPropertyValueString;
            //---------------Test Result -----------------------
            Assert.AreEqual("", persistedPropertyValueString, "Null persisted prop value should return null string");
        }

        [Test]
        public void Test_PersistedPropertyValueString_ValidInteger()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Int32 expectedInteger = TestUtil.GetRandomInt();
            boProp.InitialiseProp(expectedInteger);
            boProp.Value = TestUtil.GetRandomInt();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsNotNull(boProp.PersistedPropertyValue);

            //---------------Execute Test ----------------------
            string persistedPropertyValueString = boProp.PersistedPropertyValueString;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedInteger.ToString(), persistedPropertyValueString);
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
        public void Test_PropertyValueString_ValidInteger()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Int32 expectedInteger = TestUtil.GetRandomInt();
            boProp.InitialiseProp(expectedInteger);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);

            //---------------Execute Test ----------------------
            string propertyValueString = boProp.PropertyValueString;

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedInteger.ToString(), propertyValueString);
        }

        [Test]
        public void Test_PropertyValueString_ValidIntegerString()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Int32 expectedInteger = TestUtil.GetRandomInt();
            boProp.Value = expectedInteger.ToString("d");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsTrue(boProp.Value is Int32);
            //---------------Execute Test ----------------------
            string propertyValueString = boProp.PropertyValueString;

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedInteger.ToString(), propertyValueString);
        }
    }
}
