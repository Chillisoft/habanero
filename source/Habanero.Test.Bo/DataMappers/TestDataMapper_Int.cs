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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestDataMapper_Int
    {
        private PropDef _propDef;
        private BOPropIntDataMapper _dataMapper;

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            BusinessObjectManager.Instance.ClearLoadedObjects();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            
            _propDef = new PropDef("PropName", typeof (int), PropReadWriteRule.ReadWrite, null);

            _dataMapper = new BOPropIntDataMapper();
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
            const int expectedint = 1;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _propDef.TryParsePropValue(expectedint.ToString(), out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedint, parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_PropDef_ParsePropValue_FromInvalidSring()
        {
            //---------------Set up test pack-------------------
            const string invalidString = "Invalid";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _propDef.TryParsePropValue(invalidString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsFalse(parsedSucceed);
        
        }

        [Test]
        public void Test_PropDef_ParsePropValue_ValidBusinessObject()
        {
            //---------------Set up test pack-------------------
            TestAutoInc.LoadClassDefWithIntID();
            const int validIntID = 3;
            const string validvalue = "ValidValue";
            TestAutoInc validBusinessObject = new TestAutoInc { TestField = validvalue, TestAutoIncID = validIntID };
            //---------------Assert Precondition----------------
            Assert.AreEqual(validvalue, validBusinessObject.ToString());
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _propDef.TryParsePropValue(validBusinessObject, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsNotNull(parsedValue);
            Assert.IsTrue(parsedSucceed);
            Assert.AreEqual(validIntID, parsedValue);
        }

        [Test]
        public void Test_PropDef_ConvertValueToString_FromInValidSring()
        {
            //---------------Set up test pack-------------------
            const string invalidString = "Invalid";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            string parsedValue = _propDef.ConvertValueToString(invalidString);
            //---------------Test Result -----------------------
            Assert.AreEqual("", parsedValue);
        }

        [Test]
        public void Test_PropDef_ConvertValueToString()
        {
            //---------------Set up test pack-------------------
            const int expectedint = 4;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            string parsedValue = _propDef.ConvertValueToString(expectedint);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedint.ToString().ToUpperInvariant(), parsedValue);
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
        public void Test_DataMapper_ParsePropValue_ValidBusinessObject()
        {
            //---------------Set up test pack-------------------
            TestAutoInc.LoadClassDefWithIntID();
            const int validIntID = 3;
            const string validvalue = "ValidValue";
            TestAutoInc validBusinessObject = new TestAutoInc { TestField = validvalue, TestAutoIncID = validIntID };
            //---------------Assert Precondition----------------
            Assert.AreEqual(validvalue, validBusinessObject.ToString());
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(validBusinessObject, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsNotNull(parsedValue);
            Assert.IsTrue(parsedSucceed);
            Assert.AreEqual(validIntID, parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_ValidBusinessObject_ToStringZeroLength()
        {
            //---------------Set up test pack-------------------
            TestAutoInc.LoadClassDefWithIntID();
            const int validIntID = 3;
            const string zeroLengthString = "";
            TestAutoInc validBusinessObject = new TestAutoInc { TestField = zeroLengthString, TestAutoIncID = validIntID };
            //---------------Assert Precondition----------------
            Assert.AreEqual(zeroLengthString, validBusinessObject.ToString());
            Assert.AreEqual("", zeroLengthString);
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(validBusinessObject, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsNotNull(parsedValue);
            Assert.IsTrue(parsedSucceed);
            Assert.AreEqual(validIntID, parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring()
        {
            //---------------Set up test pack-------------------
            const int expectedint = 4;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(expectedint.ToString(), out parsedValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedint, parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue()
        {
            //---------------Set up test pack-------------------
            const int expectedint = 4;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(expectedint, out parsedValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedint, parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromInvalidSring()
        {
            //---------------Set up test pack-------------------
            const string invalidString = "Invalid";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(invalidString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsFalse(parsedSucceed);
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

        [Test]
        public void Test_DataMapper_ConvertValueToString_Null()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(null);

            //---------------Test Result -----------------------
            Assert.AreEqual("", parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_FromValidSring()
        {
            //---------------Set up test pack-------------------
            const int expectedint = 4;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedint.ToString());

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedint.ToString().ToUpperInvariant(), parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString()
        {
            //---------------Set up test pack-------------------
            const int expectedint = 4;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedint);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedint.ToString().ToUpperInvariant(), parsedValue);
        }


        [Test]
        public void Test_DataMapper_ConvertValueToString_FromInValidSring()
        {
            //---------------Set up test pack-------------------
            const string invalidString = "Invalid";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(invalidString);
            //---------------Test Result -----------------------
            Assert.AreEqual("", parsedValue);
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
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_ValidBusinessObject()
        {
            //---------------Set up test pack-------------------
            TestAutoInc.LoadClassDefWithIntID();
            const int validIntID = 3;
            const string validvalue = "ValidValue";
            TestAutoInc validBusinessObject = new TestAutoInc {TestField = validvalue, TestAutoIncID = validIntID};
            //---------------Assert Precondition----------------
            Assert.AreEqual(validvalue, validBusinessObject.ToString());
            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(validBusinessObject);
            //---------------Test Result -----------------------
            Assert.AreEqual(validIntID.ToString(), parsedValue);
        }
    }
}