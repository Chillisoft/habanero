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
        public void TryParsePropValue_WhenGivenADate_OnTheBaseDate_ShouldBeTimeOfDay()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new TimeSpanDataMapper();
            var valueToParse = TimeSpanDataMapper.BaseDate.Add(new TimeSpan(0,23,59,59,999));
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
			Assert.AreEqual(valueToParse.TimeOfDay, parsedValue);
        }

        [Test]
        public void TryParsePropValue_WhenGivenADate_WithNumberOfDays_ShouldNotLooseNumberOfDays()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new TimeSpanDataMapper();
        	var days = TestUtil.GetRandomInt(1,500);
        	var valueToParse = TimeSpanDataMapper.BaseDate.Add(TestUtil.GetRandomTimeSpan(days));
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
        	var timeSpan = (TimeSpan) parsedValue;
        	Assert.AreEqual(days, timeSpan.Days);
			Assert.AreEqual(valueToParse.Hour, timeSpan.Hours);
			Assert.AreEqual(valueToParse.Minute, timeSpan.Minutes);
			Assert.AreEqual(valueToParse.Second, timeSpan.Seconds);
			Assert.AreEqual(valueToParse.Millisecond, timeSpan.Milliseconds);
        }

    	[Test]
        public void TryParsePropValue_WhenString_ShouldConvertCorrectly()
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
        public void TryParsePropValue_WhenString_WithDays_ShouldConvertCorrectly()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new TimeSpanDataMapper();
            var originalValue = TestUtil.GetRandomTimeSpan(TestUtil.GetRandomInt(1,500));
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