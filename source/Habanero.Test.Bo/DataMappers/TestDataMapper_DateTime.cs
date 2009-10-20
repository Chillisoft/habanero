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
    public class TestDataMapper_DateTime
    {
        private PropDef _propDef;
        private BOPropDateTimeDataMapper _dataMapper;
        private const string _standardDateTimeFormat = "dd MMM yyyy HH:mm:ss:fff";

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            _propDef = new PropDef("PropName", typeof (DateTime), PropReadWriteRule.ReadWrite, null);
            _dataMapper = new BOPropDateTimeDataMapper();
        }

        [Test]
        public void Test_PropDef_ParsePropValue_Null()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _propDef.TryParsePropValue(null, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsTrue(parseSucceed);
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
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _propDef.TryParsePropValue(expectedDateTime.ToString("d"), out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDateTime, parsedValue);
            Assert.IsTrue(parseSucceed);
        }

        [Test]
        public void Test_PropDef_ParsePropValue()
        {
            //---------------Set up test pack-------------------
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _propDef.TryParsePropValue(expectedDateTime, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDateTime, parsedValue);
            Assert.IsTrue(parseSucceed);
        }

        [Test]
        public void Test_PropDef_ParsePropValue_FromInvalidSring()
        {
            //---------------Set up test pack-------------------
            const string invalidString = "Invalid";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _propDef.TryParsePropValue(invalidString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsFalse(parseSucceed);
        }

        [Test]
        public void Test_PropDef_ParsePropValue_FromDBNull()
        {
            //---------------Set up test pack-------------------
            object dbNullValue = DBNull.Value;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _propDef.TryParsePropValue(dbNullValue, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsTrue(parseSucceed);
        }

        [Test]
        public void Test_PropDef_ParsePropValue_FromNowString()
        {
            //---------------Set up test pack-------------------
            const string text = "now";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _propDef.TryParsePropValue(text, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            TestUtil.AssertIsInstanceOf<DateTimeNow>(parsedValue);
        }

        [Test]
        public void Test_PropDef_ParsePropValue_FromDateTimeNowObject()
        {
            //---------------Set up test pack-------------------
            DateTimeNow dateTimeNow = new DateTimeNow();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _propDef.TryParsePropValue(dateTimeNow, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.AreSame(dateTimeNow, parsedValue);
        }

        [Test]
        public void Test_PropDef_ParsePropValue_FromTodayString()
        {
            //---------------Set up test pack-------------------
            const string text = "today";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _propDef.TryParsePropValue(text, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            TestUtil.AssertIsInstanceOf<DateTimeToday>(parsedValue);
        }

        [Test]
        public void Test_PropDef_ParsePropValue_FromDateTimeTodayObject()
        {
            //---------------Set up test pack-------------------
            DateTimeToday dateTimeToday = new DateTimeToday();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _propDef.TryParsePropValue(dateTimeToday, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.AreEqual(dateTimeToday, parsedValue);
        }

        [Test]
        public void Test_PropDef_ConvertValueToString_Null()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _propDef.ConvertValueToString(null);

            //---------------Test Result -----------------------
            Assert.AreEqual("", parsedValue);
        }

        [Test]
        public void Test_PropDef_ConvertValueToString_FromValidSring()
        {
            //---------------Set up test pack-------------------
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _propDef.ConvertValueToString(expectedDateTime.ToString("d"));

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDateTime.ToString(_standardDateTimeFormat), parsedValue);
        }

        [Test]
        public void Test_PropDef_ConvertValueToString()
        {
            //---------------Set up test pack-------------------
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _propDef.ConvertValueToString(expectedDateTime);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDateTime.ToString(_standardDateTimeFormat), parsedValue);
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
        public void Test_PropDef_ConvertValueToString_FromDBNull()
        {
            //---------------Set up test pack-------------------
            object dbNullValue = DBNull.Value;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _propDef.ConvertValueToString(dbNullValue);

            //---------------Test Result -----------------------
            Assert.AreEqual("", parsedValue);
        }
        [Test]
        public void Test_DataMapper_ParsePropValue_Null()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _dataMapper.TryParsePropValue(null, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.IsNull(parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring()
        {
            //---------------Set up test pack-------------------
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _dataMapper.TryParsePropValue(expectedDateTime.ToString("d"), out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.AreEqual(expectedDateTime, parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue()
        {
            //---------------Set up test pack-------------------
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _dataMapper.TryParsePropValue(expectedDateTime, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.AreEqual(expectedDateTime, parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromInvalidSring()
        {
            //---------------Set up test pack-------------------
            const string invalidString = "Invalid";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _dataMapper.TryParsePropValue(invalidString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsFalse(parseSucceed);
            Assert.IsNull(parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromDBNull()
        {
            //---------------Set up test pack-------------------
            object dbNullValue = DBNull.Value;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _dataMapper.TryParsePropValue(dbNullValue, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.IsNull(parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromNowString()
        {
            //---------------Set up test pack-------------------
            const string text = "now";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _dataMapper.TryParsePropValue(text, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            TestUtil.AssertIsInstanceOf<DateTimeNow>(parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromDateTimeNowObject()
        {
            //---------------Set up test pack-------------------
            DateTimeNow dateTimeNow = new DateTimeNow();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _dataMapper.TryParsePropValue(dateTimeNow, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.AreSame(dateTimeNow, parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromTodayString()
        {
            //---------------Set up test pack-------------------
            const string text = "today";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _dataMapper.TryParsePropValue(text, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            TestUtil.AssertIsInstanceOf<DateTimeToday>(parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromDateTimeTodayObject()
        {
            //---------------Set up test pack-------------------
            DateTimeToday dateTimeToday = new DateTimeToday();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _dataMapper.TryParsePropValue(dateTimeToday, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.AreEqual(dateTimeToday, parsedValue);
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
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedDateTime.ToString("d"));

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDateTime.ToString(_standardDateTimeFormat), parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString()
        {
            //---------------Set up test pack-------------------
            DateTime expectedDateTime = DateTime.MinValue.AddDays(1);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedDateTime);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDateTime.ToString(_standardDateTimeFormat), parsedValue);
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
    }
}