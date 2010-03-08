// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System;
using System.ComponentModel;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.Base
{
    [TestFixture]
    public class TestDateTimeTodayConverter
    {
        [Test]
        public void Test_Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DateTimeTodayConverter dateTimeTodayConverter = new DateTimeTodayConverter();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(TypeConverter), dateTimeTodayConverter);
        }

        [Test]
        public void Test_CanConvertTo_WithDateTime_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            DateTimeTodayConverter dateTimeTodayConverter = new DateTimeTodayConverter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool result = dateTimeTodayConverter.CanConvertTo(typeof(DateTime));
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_ConvertTo_WithDateTime_ShouldReturnTodayValue()
        {
            //---------------Set up test pack-------------------
            DateTimeTodayConverter dateTimeTodayConverter = new DateTimeTodayConverter();
            DateTimeToday dateTimeToday = new DateTimeToday();
            DateTime snapshot = DateTime.Today;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object result = dateTimeTodayConverter.ConvertTo(dateTimeToday, typeof(DateTime));
            //---------------Test Result -----------------------
            DateTime dateTime = TestUtil.AssertIsInstanceOf<DateTime>(result);
            Assert.AreEqual(snapshot, dateTime);
        }

        [Test]
        public void Test_DefaultTypeConverter_WithDateTime_ShouldReturnTodayValue()
        {
            //---------------Set up test pack-------------------
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(DateTimeToday));
            DateTimeToday dateTimeToday = new DateTimeToday();
            DateTime snapshot = DateTime.Today;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object result = typeConverter.ConvertTo(dateTimeToday, typeof(DateTime));
            //---------------Test Result -----------------------
            DateTime dateTime = TestUtil.AssertIsInstanceOf<DateTime>(result);
            Assert.AreEqual(snapshot, dateTime);
        }

        //[Test]
        //public void Test_Convert_ToDateTime_ShouldReturnTodayValue()
        //{
        //    //---------------Set up test pack-------------------
        //    TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(DateTimeToday));
        //    DateTimeToday dateTimeToday = new DateTimeToday();
        //    DateTime snapshot = DateTime.Today;
        //    //---------------Assert Precondition----------------
        //    //---------------Execute Test ----------------------
        //    DateTime dateTime = Convert.ToDateTime(dateTimeToday);
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(snapshot, dateTime);
        //}
    }
}