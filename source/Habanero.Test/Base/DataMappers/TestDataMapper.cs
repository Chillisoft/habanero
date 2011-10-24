using System;
using Habanero.Base.DataMappers;
using Habanero.Util;
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
     
    }

    internal class DataMapperStub: DataMapper
    {
        
    }
}