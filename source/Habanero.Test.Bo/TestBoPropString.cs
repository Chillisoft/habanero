#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBoPropString
    {
        private PropDef _propDef;
        private const string _standardDateTimeFormat = "dd MMM yyyy HH:mm:ss:fff";

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            _propDef = new PropDef("PropName", typeof (string), PropReadWriteRule.ReadWrite, null);
        }

        [Test]
// ReSharper disable InconsistentNaming
        public void Test_InitialiseProp_NullValue()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof (string), boProp.PropertyType);
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(null);
            //---------------Test Result -----------------------
            Assert.IsNull(boProp.Value);
        }

        [Test]
        public void Test_InitialiseProp_Valid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            const string value = "Valid";
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(value);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(value, boProp.Value);
        }

        [Test]
        public void Test_InitialiseProp_ValidGuidString_B()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            string expectedString = Guid.NewGuid().ToString("B");
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(expectedString);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(expectedString, boProp.Value);
            Assert.IsTrue(boProp.Value is string, "Value should be a expectedString");
        }

        [Test]
        public void Test_InitialiseProp_ValidGuid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Guid expectedGuid = Guid.NewGuid();
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(expectedGuid);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(expectedGuid.ToString("B").ToUpperInvariant(), boProp.Value);
            Assert.IsTrue(boProp.Value is string, "Value should be a expectedString");
        }

        [Test]
        public void Test_InitialiseProp_ValidDateTime()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(expectedDateTime);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(expectedDateTime.ToString(_standardDateTimeFormat), boProp.Value);
            Assert.IsTrue(boProp.Value is string, "Value should be a expectedString");
        }

        [Test]
        public void Test_InitialiseProp_ValidInt()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            int expectedInt = BOTestUtils.RandomInt;
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(expectedInt);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(expectedInt.ToString(), boProp.Value);
            Assert.IsTrue(boProp.Value is string, "Value should be a expectedString");
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
        public void Test_SetValue_Valid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            const string expectedString = "Valid";

            //---------------Execute Test ----------------------
            boProp.Value = expectedString;
            //---------------Test Result -----------------------
            Assert.IsTrue(boProp.IsValid);
            Assert.AreEqual(expectedString, boProp.Value);
        }

        [Test]
        public void Test_SetValue_NullString()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            const string expectedString = "";
            //---------------Execute Test ----------------------
            boProp.Value = expectedString;
            //---------------Test Result -----------------------
            Assert.IsTrue(boProp.IsValid);
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
        public void Test_SetValue_ValidGuid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Guid expectedGuid = Guid.NewGuid();
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.Value = expectedGuid;
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(expectedGuid.ToString("B").ToUpperInvariant(), boProp.Value);
            Assert.IsTrue(boProp.Value is string, "Value should be a expectedString");
        }

        [Test]
        public void Test_SetValue_ValidDateTime()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.Value = expectedDateTime;
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(expectedDateTime.ToString(_standardDateTimeFormat), boProp.Value);
            Assert.IsTrue(boProp.Value is string, "Value should be a expectedString");
        }

        [Test]
        public void Test_SetValue_ValidInt()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            int expectedInt = BOTestUtils.RandomInt;
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.Value = expectedInt;
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(expectedInt.ToString(), boProp.Value);
            Assert.IsTrue(boProp.Value is string, "Value should be a expectedString");
        }

        [Test]
        public void Test_PersistedPropertyValueString_Null()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            boProp.InitialiseProp(DBNull.Value);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            Assert.IsNull(boProp.PersistedPropertyValue);

            //---------------Execute Test ----------------------
            string persistedPropertyValueString = boProp.PersistedPropertyValueString;
            //---------------Test Result -----------------------
            Assert.AreEqual("", persistedPropertyValueString, "Null persisted prop value should return null string");
        }

        [Test]
        public void Test_PersistedPropertyValueString_Valid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            const string expectedString = "Valid";
            boProp.InitialiseProp(expectedString);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsNotNull(boProp.PersistedPropertyValue);

            //---------------Execute Test ----------------------
            string persistedPropertyValueString = boProp.PersistedPropertyValueString;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedString, persistedPropertyValueString);
        }

        [Test]
        public void Test_PersistedPropertyValueString_ValidGuid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Guid expectedGuid = Guid.NewGuid();
            boProp.Value = expectedGuid;
            boProp.BackupPropValue();
            boProp.Value = "new value";
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsNotNull(boProp.PersistedPropertyValue);
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            string persistedPropertyValueString = boProp.PersistedPropertyValueString;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedGuid.ToString("B").ToUpperInvariant(), persistedPropertyValueString);
        }

        [Test]
        public void Test_PersistedPropertyValueString_ValidDateTime()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);
            boProp.Value = expectedDateTime;
            boProp.BackupPropValue();
            boProp.Value = "new value";

            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsNotNull(boProp.PersistedPropertyValue);
            //---------------Execute Test ----------------------
            string persistedPropertyValueString = boProp.PersistedPropertyValueString;

            //---------------Test Result -----------------------
            Assert.IsTrue(boProp.Value is string, "Value should be a expectedString");
            Assert.AreEqual(expectedDateTime.ToString(_standardDateTimeFormat), persistedPropertyValueString);
        }

        [Test]
        public void Test_PersistedPropertyValueString_ValidInt()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            int expectedInt = BOTestUtils.RandomInt;
            boProp.Value = expectedInt;
            boProp.BackupPropValue();
            boProp.Value = "new value";

            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsNotNull(boProp.PersistedPropertyValue);
            //---------------Execute Test ----------------------
            string persistedPropertyValueString = boProp.PersistedPropertyValueString;
            //---------------Test Result -----------------------
            Assert.IsTrue(boProp.Value is string, "Value should be a expectedString");
            Assert.AreEqual(expectedInt.ToString(), persistedPropertyValueString);
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
        public void Test_PropertyValueString_Valid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            const string expectedString = "Valid";
            boProp.InitialiseProp(expectedString);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);

            //---------------Execute Test ----------------------
            string propertyValueString = boProp.PropertyValueString;

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedString, propertyValueString);
        }

        [Test]
        public void Test_PropertyValueString_ValidGuid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Guid expectedGuid = Guid.NewGuid();
            boProp.Value = expectedGuid;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            string persistedPropertyValueString = boProp.PropertyValueString;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedGuid.ToString("B").ToUpperInvariant(), persistedPropertyValueString);
        }

        [Test]
        public void Test_PropertyValueString_ValidDateTime()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);
            boProp.Value = expectedDateTime;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            string persistedPropertyValueString = boProp.PropertyValueString;

            //---------------Test Result -----------------------
            Assert.IsTrue(boProp.Value is string, "Value should be a expectedString");
            Assert.AreEqual(expectedDateTime.ToString(_standardDateTimeFormat), persistedPropertyValueString);
        }

        [Test]
        public void Test_PropertyValueString_ValidInt()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            int expectedInt = BOTestUtils.RandomInt;
            boProp.Value = expectedInt;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            string persistedPropertyValueString = boProp.PropertyValueString;
            //---------------Test Result -----------------------
            Assert.IsTrue(boProp.Value is string, "Value should be a expectedString");
            Assert.AreEqual(expectedInt.ToString(), persistedPropertyValueString);
        }
    }
}