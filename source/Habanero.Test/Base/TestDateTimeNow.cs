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
    public class TestDateTimeNow
    {
        [Test]
        public void Test_IResolvableToValue()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DateTimeNow dateTimeNow = new DateTimeNow();
            //---------------Test Result -----------------------
            TestUtil.AssertIsInstanceOf<IResolvableToValue>(dateTimeNow);
        }
        [Test]
        public void Test_IResolvableToValue_ResolveToValue()
        {
            //---------------Set up test pack-------------------
            DateTimeNow dateTimeNow = new DateTimeNow();
            IResolvableToValue resolvableToValue = dateTimeNow;
            DateTime dateTimeBefore = DateTime.Now;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object resolvedValue = resolvableToValue.ResolveToValue();
            //---------------Test Result -----------------------
            DateTime dateTimeAfter = DateTime.Now;
            DateTime dateTime = TestUtil.AssertIsInstanceOf<DateTime>(resolvedValue);
            Assert.GreaterOrEqual(dateTime, dateTimeBefore);
            Assert.LessOrEqual(dateTime, dateTimeAfter);
        }
        
        [Test, Ignore("This test fails intermittently due to the mutability of the DateTimeNow value. June 2008")]
        public void Test_Comparable_Equals_WithDateTimeNowValue()
        {
            //-------------Setup Test Pack ------------------
            DateTimeNow dateTimeNow = new DateTimeNow();
            IComparable comparable = dateTimeNow;

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(DateTimeNow.Value);

            //-------------Test Result ----------------------
            Assert.AreEqual(0, i);
        }

        [Test]
        public void Test_Comparable_Equals_WithDateTimeNowType()
        {
            //-------------Setup Test Pack ------------------
            DateTimeNow dateTimeNow = new DateTimeNow();
            IComparable comparable = dateTimeNow;

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(new DateTimeNow());

            //-------------Test Result ----------------------
            Assert.AreEqual(0, i);
        }

        [Test]
        public void Test_Comparable_GreaterThan()
        {
            //-------------Setup Test Pack ------------------
            DateTimeNow dateTimeNow = new DateTimeNow();
            IComparable comparable = dateTimeNow;
            DateTime dateTime = DateTimeNow.Value.AddDays(-1);
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
            DateTimeNow dateTimeNow = new DateTimeNow();
            IComparable comparable = dateTimeNow;
            DateTime dateTime = DateTimeNow.Value.AddDays(1);

            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(dateTime);

            //-------------Test Result ----------------------
            Assert.AreEqual(-1, i);
        }

        [Test, Ignore("This test fails intermittently due to the mutability of the DateTimeNow value. June 2008")]
        public void Test_Comparable_OfDateTime_Equals()
        {
            //-------------Setup Test Pack ------------------
            DateTimeNow dateTimeNow = new DateTimeNow();
            IComparable<DateTime> comparable = dateTimeNow;
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(DateTimeNow.Value);
            //-------------Test Result ----------------------
            Assert.AreEqual(0, i);
        }

        [Test]
        public void Test_Comparable_OfDateTime_GreaterThan()
        {
            //-------------Setup Test Pack ------------------
            DateTimeNow dateTimeNow = new DateTimeNow();
            IComparable<DateTime> comparable = dateTimeNow;
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(DateTimeNow.Value.AddDays(-1));
            //-------------Test Result ----------------------
            Assert.AreEqual(1, i);
        }

        [Test]
        public void Test_Comparable_OfDateTime_LessThan()
        {
            //-------------Setup Test Pack ------------------
            DateTimeNow dateTimeNow = new DateTimeNow();
            IComparable<DateTime> comparable = dateTimeNow;
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(DateTimeNow.Value.AddDays(1));
            //-------------Test Result ----------------------
            Assert.AreEqual(-1, i);
        }

        [Test]
        public void Test_Equals_WithNull_ShouldReturnFalse()
        {
            //-------------Setup Test Pack ------------------
            DateTimeNow dateTimeNow = new DateTimeNow();
            const DateTimeNow dateTimeNow2 = null;
            //-------------Execute test ---------------------
            bool result = dateTimeNow.Equals(dateTimeNow2);
            //-------------Test Result ----------------------
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_Equals_WithDateTimeValue_ShouldReturnFalse()
        {
            //-------------Setup Test Pack ------------------
            DateTimeNow dateTimeNow = new DateTimeNow();
            DateTime dateTime2 = DateTime.Now;
            //-------------Execute test ---------------------
            bool result = dateTimeNow.Equals(dateTime2);
            //-------------Test Result ----------------------
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_Equals_WithDateTimeNowType_ShouldReturnTrue()
        {
            //-------------Setup Test Pack ------------------
            DateTimeNow dateTimeNow = new DateTimeNow();
            DateTimeNow dateTimeNow2 = new DateTimeNow();
            //-------------Execute test ---------------------
            bool result = dateTimeNow.Equals(dateTimeNow2);
            //-------------Test Result ----------------------
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_Value_ShouldReturnNow()
        {
            //-------------Setup Test Pack ------------------
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            DateTime dateTimeBefore = DateTime.Now;
            DateTime dateTime = DateTimeNow.Value;
            DateTime dateTimeAfter = DateTime.Now;

            //-------------Test Result ----------------------
            Assert.GreaterOrEqual(dateTime, dateTimeBefore);
            Assert.LessOrEqual(dateTime, dateTimeAfter);
        }

        [Test]
        public void Test_ToString()
        {
            //---------------Set up test pack-------------------
            DateTimeNow dteNow = new DateTimeNow();
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
            object[] customAttributes = typeof(DateTimeNow).GetCustomAttributes(false);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, customAttributes.Length);
            TypeConverterAttribute typeConverterAttribute =
                TestUtil.AssertIsInstanceOf<TypeConverterAttribute>(customAttributes[0]);
            Assert.AreEqual(typeof(DateTimeNowConverter).AssemblyQualifiedName, typeConverterAttribute.ConverterTypeName);
        }

        [Test]
        public void Test_TypeConverter_ShouldBeDefaultTypeConverterForDateTimeNow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(DateTimeNow));
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(DateTimeNowConverter), typeConverter);
        }
    }
}