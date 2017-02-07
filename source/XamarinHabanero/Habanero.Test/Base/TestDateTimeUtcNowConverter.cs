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
    public class TestDateTimeUtcNowConverter
    {
        [Test]
        public void Test_Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DateTimeUtcNowConverter dateTimeUtcNowConverter = new DateTimeUtcNowConverter();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof (TypeConverter), dateTimeUtcNowConverter);
        }

        [Test]
        public void Test_CanConvertTo_WithDateTime_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            DateTimeUtcNowConverter dateTimeUtcNowConverter = new DateTimeUtcNowConverter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool result = dateTimeUtcNowConverter.CanConvertTo(typeof(DateTime));
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_ConvertTo_WithDateTime_ShouldReturnNowValue()
        {
            //---------------Set up test pack-------------------
            DateTimeUtcNowConverter dateTimeUtcNowConverter = new DateTimeUtcNowConverter();
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            DateTime snapshot = DateTime.UtcNow;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object result = dateTimeUtcNowConverter.ConvertTo(dateTimeUtcNow, typeof(DateTime));
            //---------------Test Result -----------------------
            DateTime dateTime = TestUtil.AssertIsInstanceOf<DateTime>(result);
            Assert.Greater(dateTime, snapshot.AddSeconds(-1));
            Assert.Less(dateTime, snapshot.AddSeconds(1));
            Assert.AreEqual(dateTime.Kind, DateTimeKind.Utc);
        }

        [Test]
        public void Test_DefaultTypeConverter_WithDateTime_ShouldReturnNowValue()
        {
            //---------------Set up test pack-------------------
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(DateTimeUtcNow));
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            DateTime snapshot = DateTime.UtcNow;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object result = typeConverter.ConvertTo(dateTimeUtcNow, typeof(DateTime));
            //---------------Test Result -----------------------
            DateTime dateTime = TestUtil.AssertIsInstanceOf<DateTime>(result);
            Assert.Greater(dateTime, snapshot.AddSeconds(-1));
            Assert.Less(dateTime, snapshot.AddSeconds(1));
            Assert.AreEqual(dateTime.Kind, DateTimeKind.Utc);
        }
    }
}