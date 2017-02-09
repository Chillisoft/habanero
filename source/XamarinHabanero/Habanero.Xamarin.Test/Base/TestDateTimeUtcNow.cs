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
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class TestDateTimeUtcNow
    {
        [Test]
        public void Test_IResolvableToValue()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DateTimeUtcNow dateTimeNow = new DateTimeUtcNow();
            //---------------Test Result -----------------------
            TestUtil.AssertIsInstanceOf<IResolvableToValue>(dateTimeNow);
        }
        [Test]
        public void Test_IResolvableToValue_ResolveToValue()
        {
            //---------------Set up test pack-------------------
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            IResolvableToValue resolvableToValue = dateTimeUtcNow;
            DateTime dateTimeBefore = DateTime.UtcNow;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object resolvedValue = resolvableToValue.ResolveToValue();
            //---------------Test Result -----------------------
            DateTime dateTimeAfter = DateTime.UtcNow;
            DateTime dateTime = TestUtil.AssertIsInstanceOf<DateTime>(resolvedValue);
            Assert.GreaterOrEqual(dateTime, dateTimeBefore);
            Assert.LessOrEqual(dateTime, dateTimeAfter);
        }

        [Test]
        public void Test_Comparable_Equals_WithDatetimeUtcNowType()
        {
            //-------------Setup Test Pack ------------------
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            IComparable comparable = dateTimeUtcNow;

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(new DateTimeUtcNow());

            //-------------Test Result ----------------------
            Assert.AreEqual(0, i);
        }

        [Test]
        public void Test_Comparable_GreaterThan()
        {
            //-------------Setup Test Pack ------------------
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            IComparable comparable = dateTimeUtcNow;
            DateTime dateTime = DateTimeUtcNow.Value.AddDays(-1);
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
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            IComparable comparable = dateTimeUtcNow;
            DateTime dateTime = DateTimeUtcNow.Value.AddDays(1);

            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(dateTime);

            //-------------Test Result ----------------------
            Assert.AreEqual(-1, i);
        }

        [Test]
        public void Test_Comparable_OfDateTime_GreaterThan()
        {
            //-------------Setup Test Pack ------------------
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            IComparable<DateTime> comparable = dateTimeUtcNow;
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(DateTimeUtcNow.Value.AddDays(-1));
            //-------------Test Result ----------------------
            Assert.AreEqual(1, i);
        }

        [Test]
        public void Test_Comparable_OfDateTime_LessThan()
        {
            //-------------Setup Test Pack ------------------
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            IComparable<DateTime> comparable = dateTimeUtcNow;
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(DateTimeUtcNow.Value.AddDays(1));
            //-------------Test Result ----------------------
            Assert.AreEqual(-1, i);
        }

        [Test]
        public void Test_Equals_WithNull_ShouldReturnFalse()
        {
            //-------------Setup Test Pack ------------------
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            const DateTimeUtcNow dateTimeUtcNow2 = null;
            //-------------Execute test ---------------------
            bool result = dateTimeUtcNow.Equals(dateTimeUtcNow2);
            //-------------Test Result ----------------------
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_Equals_WithDateTimeValue_ShouldReturnFalse()
        {
            //-------------Setup Test Pack ------------------
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            DateTime utcDateTime2 = DateTime.UtcNow;
            //-------------Execute test ---------------------
            bool result = dateTimeUtcNow.Equals(utcDateTime2);
            //-------------Test Result ----------------------
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_Equals_WithDatetimeUtcNowType_ShouldReturnTrue()
        {
            //-------------Setup Test Pack ------------------
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            DateTimeUtcNow dateTimeUtcNow2 = new DateTimeUtcNow();
            //-------------Execute test ---------------------
            bool result = dateTimeUtcNow.Equals(dateTimeUtcNow2);
            //-------------Test Result ----------------------
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_Value_ShouldReturnNow()
        {
            //-------------Setup Test Pack ------------------
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            DateTime dateTimeBefore = DateTime.UtcNow;
            DateTime dateTime = DateTimeUtcNow.Value;
            DateTime dateTimeAfter = DateTime.UtcNow;

            //-------------Test Result ----------------------
            Assert.GreaterOrEqual(dateTime, dateTimeBefore);
            Assert.LessOrEqual(dateTime, dateTimeAfter);
        }

        [Test]
        public void Test_ToString()
        {
            //---------------Set up test pack-------------------
            DateTimeUtcNow dteNow = new DateTimeUtcNow();
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
            object[] customAttributes = typeof(DateTimeUtcNow).GetCustomAttributes(false);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, customAttributes.Length);
            TypeConverterAttribute typeConverterAttribute =
                TestUtil.AssertIsInstanceOf<TypeConverterAttribute>(customAttributes[0]);
            Assert.AreEqual(typeof(DateTimeUtcNowConverter).AssemblyQualifiedName, typeConverterAttribute.ConverterTypeName);
        }

        [Test]
        public void Test_TypeConverter_ShouldBeDefaultTypeConverterForDatetimeUtcNow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(DateTimeUtcNow));
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(DateTimeUtcNowConverter), typeConverter);
        }
    }
}