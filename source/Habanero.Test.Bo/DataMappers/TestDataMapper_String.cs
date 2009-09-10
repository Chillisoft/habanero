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
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestDataMapper_String
    {
        private PropDef _propDef;
        private BOPropDataMapper _dataMapper;
        private const string _standardDateTimeFormat = "dd MMM yyyy HH:mm:ss:fff";

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            _propDef = new PropDef("PropName", typeof (string), PropReadWriteRule.ReadWrite, null);
            _dataMapper = new BOPropStringDataMapper();
        }

        [Test]
        public void Test_PropDef_ParsePropValue_Null()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _propDef.TryParsePropValue(null, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_PropDef_ParsePropValue_EmptyString()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _propDef.TryParsePropValue("", out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsTrue(parseSucceed);
        }
        [Test]
        public void Test_PropDef_ParsePropValue_FromValidSring()
        {
            //---------------Set up test pack-------------------
            const string validValue = "Valid";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _propDef.TryParsePropValue(validValue, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(validValue, parsedValue);
            Assert.IsInstanceOfType(typeof (String), parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_PropDef_ParsePropValue_FromValidGuidSring()
        {
            //---------------Set up test pack-------------------
            string validValue = Guid.NewGuid().ToString("B");

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _propDef.TryParsePropValue(validValue, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(validValue, parsedValue);
            Assert.IsInstanceOfType(typeof (string), parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_PropDef_ConvertValueToString_FromValidSring()
        {
            //---------------Set up test pack-------------------
            const string expectedString = "Valid string";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _propDef.ConvertValueToString(expectedString);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedString, parsedValue);
            Assert.IsInstanceOfType(typeof(string), parsedValue);
        }

        [Test]
        public void Test_PropDef_ConvertValueToString_GuidObject()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.NewGuid();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _propDef.ConvertValueToString(expectedGuid);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedGuid.ToString(), parsedValue);
            Assert.IsInstanceOfType(typeof(string), parsedValue);
        }

        [Test]
        public void Test_PropDef_ConvertValueToString_FromDBNull()
        {
            //---------------Set up test pack-------------------
            object dbNullValue = DBNull.Value;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _propDef.ConvertValueToString(dbNullValue);

            //---------------Test Result -----------------------
            Assert.AreEqual("", parsedValue);
            Assert.IsInstanceOfType(typeof(string), parsedValue);
        }
        [Test]
        public void Test_DataMapper_ParsePropValue_Null()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(null, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring()
        {
            //---------------Set up test pack-------------------
            const string validValue = "Valid";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(validValue, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(validValue, parsedValue);
            Assert.IsInstanceOfType(typeof(String), parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidGuidSring()
        {
            //---------------Set up test pack-------------------
            string validValue = Guid.NewGuid().ToString("B");

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(validValue, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(validValue, parsedValue);
            Assert.IsInstanceOfType(typeof(string), parsedValue);
            Assert.IsTrue(parsedSucceed);
        }


        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidGuid()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.NewGuid();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(expectedGuid, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedGuid.ToString("B").ToUpperInvariant(), parsedValue);
            Assert.IsTrue(parsedValue is string, "Value should be a string");
            Assert.IsInstanceOfType(typeof (string), parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_EmptyGuid()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(expectedGuid, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_EmptyGuidString()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(expectedGuid.ToString("B"), out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedGuid.ToString("B"), parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_ValidDateTime()
        {
            //---------------Set up test pack-------------------
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(expectedDateTime, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDateTime.ToString(_standardDateTimeFormat), parsedValue);
            Assert.IsInstanceOfType(typeof (string), parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_ValidDateTimeString()
        {
            //---------------Set up test pack-------------------
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(expectedDateTime.ToString("d"), out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDateTime.ToString("d"), parsedValue);
            Assert.IsInstanceOfType(typeof (string), parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_ValidInt()
        {
            //---------------Set up test pack-------------------
            int expectedInt = BOTestUtils.RandomInt;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(expectedInt, out parsedValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedInt.ToString(), parsedValue);
            Assert.IsTrue(parsedValue is string, "Value should be a string");
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_ValidInt_string()
        {
            //---------------Set up test pack-------------------
            int expectedInt = BOTestUtils.RandomInt;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(expectedInt.ToString(), out parsedValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedInt.ToString(), parsedValue);
            Assert.IsTrue(parsedValue is string, "Value should be a string");
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromDBNull()
        {
            //---------------Set up test pack-------------------
            object dbNullValue = DBNull.Value;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(dbNullValue, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        #region ConvertValueToString

        [Test]
        public void Test_DataMapper_ConvertValueToString_Null()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(null);

            //---------------Test Result -----------------------
            Assert.AreEqual("", parsedValue);
            Assert.IsInstanceOfType(typeof (string), parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_FromValidSring()
        {
            //---------------Set up test pack-------------------
            const string expectedString = "Valid string";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedString);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedString, parsedValue);
            Assert.IsInstanceOfType(typeof (string), parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_FromGuidValidSring()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.NewGuid();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedGuid.ToString("P"));

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedGuid.ToString("P"), parsedValue);
            Assert.IsInstanceOfType(typeof (string), parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_GuidObject()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.NewGuid();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedGuid);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedGuid.ToString(), parsedValue);
            Assert.IsInstanceOfType(typeof (string), parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_EmptyGuid()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedGuid);

            //---------------Test Result -----------------------
            Assert.AreEqual("", parsedValue);
            Assert.IsInstanceOfType(typeof (string), parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_EmptyGuidString()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedGuid.ToString("B"));

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedGuid.ToString("B"), parsedValue);
            Assert.IsInstanceOfType(typeof (string), parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_DateTimeString()
        {
            //---------------Set up test pack-------------------
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedDateTime.ToString("d"));

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDateTime.ToString("d"), parsedValue);
            Assert.IsInstanceOfType(typeof (string), parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_DateTime()
        {
            //---------------Set up test pack-------------------
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedDateTime);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDateTime.ToString(_standardDateTimeFormat), parsedValue);
            Assert.IsInstanceOfType(typeof (string), parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_int()
        {
            //---------------Set up test pack-------------------
            const int expectedInt = 3;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedInt);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedInt.ToString(), parsedValue);
            Assert.IsInstanceOfType(typeof (string), parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_intString()
        {
            //---------------Set up test pack-------------------
            const int expectedInt = 3;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedInt.ToString());

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedInt.ToString(), parsedValue);
            Assert.IsInstanceOfType(typeof (string), parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_FromDBNull()
        {
            //---------------Set up test pack-------------------
            object dbNullValue = DBNull.Value;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(dbNullValue);

            //---------------Test Result -----------------------
            Assert.AreEqual("", parsedValue);
            Assert.IsInstanceOfType(typeof (string), parsedValue);
        }

        #endregion
    }
}