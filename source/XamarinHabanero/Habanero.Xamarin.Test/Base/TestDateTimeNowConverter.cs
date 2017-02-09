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
using System.ComponentModel;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.Base
{
    [TestFixture]
    public class TestDateTimeNowConverter
    {
        [Test]
        public void Test_Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DateTimeNowConverter dateTimeNowConverter = new DateTimeNowConverter();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof (TypeConverter), dateTimeNowConverter);
        }

        [Test]
        public void Test_CanConvertTo_WithDateTime_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            DateTimeNowConverter dateTimeNowConverter = new DateTimeNowConverter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool result = dateTimeNowConverter.CanConvertTo(typeof(DateTime));
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_ConvertTo_WithDateTime_ShouldReturnNowValue()
        {
            //---------------Set up test pack-------------------
            DateTimeNowConverter dateTimeNowConverter = new DateTimeNowConverter();
            DateTimeNow dateTimeNow = new DateTimeNow();
            DateTime snapshot = DateTime.Now;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object result = dateTimeNowConverter.ConvertTo(dateTimeNow, typeof(DateTime));
            //---------------Test Result -----------------------
            DateTime dateTime = TestUtil.AssertIsInstanceOf<DateTime>(result);
            Assert.Greater(dateTime, snapshot.AddSeconds(-1));
            Assert.Less(dateTime, snapshot.AddSeconds(1));
        }

        [Test]
        public void Test_DefaultTypeConverter_WithDateTime_ShouldReturnNowValue()
        {
            //---------------Set up test pack-------------------
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(DateTimeNow));
            DateTimeNow dateTimeNow = new DateTimeNow();
            DateTime snapshot = DateTime.Now;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object result = typeConverter.ConvertTo(dateTimeNow, typeof(DateTime));
            //---------------Test Result -----------------------
            DateTime dateTime = TestUtil.AssertIsInstanceOf<DateTime>(result);
            Assert.Greater(dateTime, snapshot.AddSeconds(-1));
            Assert.Less(dateTime, snapshot.AddSeconds(1));
        }
    }
}