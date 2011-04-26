using System;
using Habanero.Base.DataMappers;
using NUnit.Framework;

namespace Habanero.Test.Base.DataMappers
{
    [TestFixture]
    public class TestTimeSpanDataMapper
    {
        [Test]
        public void TryParsePropValue_WorksForNull()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new TimeSpanDataMapper();
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(null, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.IsNull(parsedValue);
        }
        [Test]
        public void TryParsePropValue_WorksForTimeSpan()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new TimeSpanDataMapper();
            var valueToParse = new TimeSpan(TestUtil.GetRandomInt());
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.AreEqual(valueToParse, parsedValue);
        }

        [Test]
        public void TryParsePropValue_ShouldUseTimeOfDay_WhenGivenADateTime()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new TimeSpanDataMapper();
            var valueToParse = new DateTime(TestUtil.GetRandomInt());
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.AreEqual(valueToParse.TimeOfDay, parsedValue);
        }

        [Test]
        public void TryParsePropValue_ConvertsStringToTimeSpan()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new TimeSpanDataMapper();
            var originalValue = new TimeSpan(TestUtil.GetRandomInt());
            var valueToParse = dataMapper.ConvertValueToString(originalValue);
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.IsInstanceOf(typeof(TimeSpan), parsedValue);
            Assert.AreEqual(originalValue, parsedValue);
        }

        [Test]
        public void TryParsePropValue_FailsForOtherTypes()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new TimeSpanDataMapper();
            object parsedValue;
            //---------------Execute Test ----------------------
            var parsedSucceed = dataMapper.TryParsePropValue(3, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsFalse(parsedSucceed);
        }

        [Test]
        public void ConvertValueToString_UsesUniversalFormat()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new TimeSpanDataMapper();
            var originalValue = new TimeSpan(TestUtil.GetRandomInt());
            //---------------Execute Test ----------------------
            string strValue = dataMapper.ConvertValueToString(originalValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(originalValue.ToString(), strValue);
        }
     
    }
}