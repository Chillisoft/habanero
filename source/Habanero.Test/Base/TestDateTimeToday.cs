// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
    public class TestDateTimeToday
    {
        [Test]
        public void Test_IResolvableToValue()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DateTimeToday dateTimeToday = new DateTimeToday();
            //---------------Test Result -----------------------
            TestUtil.AssertIsInstanceOf<IResolvableToValue>(dateTimeToday);
        }

        [Test]
        public void Test_IResolvableToValue_ResolveToValue()
        {
            //---------------Set up test pack-------------------
            DateTimeToday dateTimeToday = new DateTimeToday();
            IResolvableToValue resolvableToValue = dateTimeToday;
            DateTime today = DateTime.Today;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object resolvedValue = resolvableToValue.ResolveToValue();
            //---------------Test Result -----------------------
            DateTime dateTime = TestUtil.AssertIsInstanceOf<DateTime>(resolvedValue);
            Assert.AreEqual(today, dateTime);
        }

        [Test]
        public void Test_Comparable_Equals_WithDateTimeTodayValue()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday DateTimeToday = new DateTimeToday();
            IComparable comparable = DateTimeToday;
            //-------------Execute test ---------------------
            int i = comparable.CompareTo(DateTimeToday.Value);
            //-------------Test Result ----------------------
            Assert.AreEqual(0, i);
        }

        [Test]
        public void Test_Comparable_Equals_WithDateTimeTodayType()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday DateTimeToday = new DateTimeToday();
            IComparable comparable = DateTimeToday;
            //-------------Execute test ---------------------
            int i = comparable.CompareTo(new DateTimeToday());
            //-------------Test Result ----------------------
            Assert.AreEqual(0, i);
        }

        [Test]
        public void Test_Comparable_GreaterThan()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday DateTimeToday = new DateTimeToday();
            IComparable comparable = DateTimeToday;
            DateTime dateTime = DateTimeToday.Value.AddDays(-1);
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(dateTime);
            //-------------Test Result ----------------------
            Assert.AreEqual(1, i);
        }

        [Test]
        public void Test_Comparable_LessThan()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday DateTimeToday = new DateTimeToday();
            IComparable comparable = DateTimeToday;
            DateTime dateTime = DateTimeToday.Value.AddDays(1);

            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(dateTime);

            //-------------Test Result ----------------------
            Assert.AreEqual(-1, i);
        }

        [Test]
        public void Test_Comparable_OfDateTime_Equals()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday DateTimeToday = new DateTimeToday();
            IComparable<DateTime> comparable = DateTimeToday;
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(DateTimeToday.Value);
            //-------------Test Result ----------------------
            Assert.AreEqual(0, i);
        }

        [Test]
        public void Test_Comparable_OfDateTime_GreaterThan()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday DateTimeToday = new DateTimeToday();
            IComparable<DateTime> comparable = DateTimeToday;
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(DateTimeToday.Value.AddDays(-1));
            //-------------Test Result ----------------------
            Assert.AreEqual(1, i);
        }

        [Test]
        public void Test_Comparable_OfDateTime_LessThan()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday DateTimeToday = new DateTimeToday();
            IComparable<DateTime> comparable = DateTimeToday;
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(DateTimeToday.Value.AddDays(1));
            //-------------Test Result ----------------------
            Assert.AreEqual(-1, i);
        }

        [Test]
        public void Test_Equals_WithNull_ShouldReturnFalse()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday DateTimeToday = new DateTimeToday();
            const DateTimeToday DateTimeToday2 = null;
            //-------------Execute test ---------------------
            bool result = DateTimeToday.Equals(DateTimeToday2);
            //-------------Test Result ----------------------
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_Equals_WithDateTimeValue_ShouldReturnFalse()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday DateTimeToday = new DateTimeToday();
            DateTime dateTime2 = DateTime.Now;
            //-------------Execute test ---------------------
            bool result = DateTimeToday.Equals(dateTime2);
            //-------------Test Result ----------------------
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_Equals_WithDateTimeTodayType_ShouldReturnTrue()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday DateTimeToday = new DateTimeToday();
            DateTimeToday DateTimeToday2 = new DateTimeToday();
            //-------------Execute test ---------------------
            bool result = DateTimeToday.Equals(DateTimeToday2);
            //-------------Test Result ----------------------
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_Value_ShouldReturnToday()
        {
            //-------------Setup Test Pack ------------------
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------
            DateTime dateTime = DateTimeToday.Value;
            //-------------Test Result ----------------------
            Assert.AreEqual(DateTime.Today, dateTime);
        }

        [Test]
        public void Test_ToString()
        {
            //---------------Set up test pack-------------------
            DateTimeToday dteNow = new DateTimeToday();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string toString = dteNow.ToString();
            //---------------Test Result -----------------------
            DateTime dteParsedDateTime;
            Assert.IsTrue(DateTime.TryParse(toString, out dteParsedDateTime));
            //            Assert.IsTrue(dteNow == dteParsedDateTime);
            Assert.AreEqual(toString, dteParsedDateTime.ToString());
        }

        [Test]
        public void Test_TypeConverter_TypeAttribute()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object[] customAttributes = typeof(DateTimeToday).GetCustomAttributes(false);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, customAttributes.Length);
            TypeConverterAttribute typeConverterAttribute =
                TestUtil.AssertIsInstanceOf<TypeConverterAttribute>(customAttributes[0]);
            Assert.AreEqual(typeof(DateTimeTodayConverter).AssemblyQualifiedName, typeConverterAttribute.ConverterTypeName);
        }

        [Test]
        public void Test_TypeConverter_ShouldBeDefaultTypeConverterForDateTimeToday()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(DateTimeToday));
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(DateTimeTodayConverter), typeConverter);
        }
    }
}