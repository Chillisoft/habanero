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
using Habanero.Base.DataMappers;
using NUnit.Framework;

namespace Habanero.Test.Base.DataMappers
{
    [TestFixture]
    public class TestDataMapper
    {
        [Test]
        public void TryParsePropValue_ShouldSetReturnValueToNull_WhenValueToParseIsNull()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new DataMapperStub();
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(null, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.IsNull(parsedValue);
        }

        [Test]
        public void TryParsePropValue_ShouldSetReturnValueToNull_WhenValueToParseIsEmptyString()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new DataMapperStub();
            var valueToParse = "";
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.IsNull(parsedValue);
        }

        [Test]
        public void TryParsePropValue_ShouldFail_WhenRetainingEmptyStrings_AndValueToParseIsEmptyString()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new DataMapperStubRetainingEmptyStrings();
            var valueToParse = "";
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsFalse(parseSucceed);
            Assert.IsNull(parsedValue);
        }

        [Test]
        public void TryParsePropValue_ShouldSetReturnValueToNull_WhenValueToParseIsDBNull()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new DataMapperStub();
            var valueToParse = DBNull.Value;
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.IsNull(parsedValue);
        }

        [Test]
        public void TryParsePropValue_ShouldFail_ForOtherValues()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new DataMapperStub();
            const int valueToParse = 5;
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsFalse(parseSucceed);
            Assert.IsNull(parsedValue);
        }

        [Test]
        public void ConvertValueToString_ShouldReturnEmptyString_WhenValueIsNull()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new DataMapperStub();
            //---------------Execute Test ----------------------
            var strValue = dataMapper.ConvertValueToString(null);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, strValue.Length);
        }

        [Test]
        public void ConvertValueToString_ShouldReturnToStringOfValue_WhenValueIsNonNull()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new DataMapperStub();
            //---------------Execute Test ----------------------
            var strValue = dataMapper.ConvertValueToString(5);
            //---------------Test Result -----------------------
            Assert.AreEqual("5", strValue);
        }

        [TestCase(1, 1, true, "")]
        [TestCase(1, 2, false, "")]
        [TestCase(null, 1, false, "")]
        [TestCase(3, null, false, "")]
        [TestCase(null, null, true, "")]
        [TestCase(null, "", true, "A parsed null value equals an empty string")]
        [TestCase("", null, false, "A parsed empty string does not equal null")]
        [TestCase(1, "1", false, "This code does not do any parsing i.e. the '1' is not parsed to be an int and hence this will return false")]
        public void CompareValues_TestCases(object compareToValue, object value, bool areEqual, string message)
        {
            //---------------Set up test pack-------------------
            var dataMapper = new DataMapperStub();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = dataMapper.CompareValues(compareToValue, value);
            //---------------Test Result -----------------------
            var expectedMessage = string.Format("compareToValue : '{0}' - value : '{1}' CompareValues Should be : {2}",
                                                compareToValue, value, areEqual);
            expectedMessage += Environment.NewLine + message;
            Assert.AreEqual(areEqual, result, expectedMessage);
        }

        [TestCase(1, 1, true, "")]
        [TestCase(1, 2, false, "")]
        [TestCase(null, 1, false, "")]
        [TestCase(3, null, false, "")]
        [TestCase(null, null, true, "")]
        public void CompareValues_WithNullables_ShouldTreatAsBaseTypes(object compareToValue, int? value, bool areEqual, string message)
        {
            //---------------Set up test pack-------------------
            var dataMapper = new DataMapperStub();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = dataMapper.CompareValues(compareToValue, value);
            //---------------Test Result -----------------------
            var expectedMessage = string.Format("compareToValue : '{0}' - value : '{1}' CompareValues Should be : {2}",
                                                compareToValue, value, areEqual);
            expectedMessage += Environment.NewLine + message;
            Assert.AreEqual(areEqual, result, expectedMessage);
        }

        [TestCase(1, 1, true, "")]
        [TestCase(1, 2, false, "")]
        [TestCase(null, 1, false, "")]
        [TestCase(3, null, false, "")]
        [TestCase(null, null, true, "")]
        [TestCase(null, "", false, "A parsed null value does not equal an empty string")]
        [TestCase("", null, false, "A parsed empty string does not equal null")]
        [TestCase(1, "1", false, "This code does not do any parsing i.e. the '1' is not parsed to be an int and hence this will return false")]
        public void CompareValues_TestCases_WhenRetainingEmptyStrings(object compareToValue, object value, bool areEqual, string message)
        {
            //---------------Set up test pack-------------------
            var dataMapper = new DataMapperStubRetainingEmptyStrings();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = dataMapper.CompareValues(compareToValue, value);
            //---------------Test Result -----------------------
            var expectedMessage = string.Format("compareToValue : '{0}' - value : '{1}' CompareValues Should be : {2}",
                                                compareToValue, value, areEqual);
            expectedMessage += Environment.NewLine + message;
            Assert.AreEqual(areEqual, result, expectedMessage);
        }

        internal class DataMapperStubRetainingEmptyStrings : DataMapper
        {
            public DataMapperStubRetainingEmptyStrings()
            {
                _convertEmptyStringToNull = false;
            }
        }

        internal class DataMapperStub: DataMapper
        {
        
        }
     
    }
}