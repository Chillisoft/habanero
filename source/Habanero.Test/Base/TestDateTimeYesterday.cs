using System;
using System.ComponentModel;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.Base
{
    [TestFixture]
    public class TestDateTimeYesterday
    {
        [Test]
        public void Test_IResolvableToValue()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DateTimeYesterday dateTimeToday = new DateTimeYesterday();
            //---------------Test Result -----------------------
            TestUtil.AssertIsInstanceOf<IResolvableToValue>(dateTimeToday);
        }

        [Test]
        public void Test_IResolvableToValue_ResolveToValue()
        {
            //---------------Set up test pack-------------------
            DateTimeYesterday dateTimeToday = new DateTimeYesterday();
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
        public void Test_Comparable_Equals_WithDateTimeYesterdayValue()
        {
            //-------------Setup Test Pack ------------------
            DateTimeYesterday dateTimeToday = new DateTimeYesterday();
            IComparable comparable = dateTimeToday;
            //-------------Execute test ---------------------
            int i = comparable.CompareTo(DateTimeYesterday.Value);
            //-------------Test Result ----------------------
            Assert.AreEqual(0, i);
        }

        [Test]
        public void Test_Comparable_Equals_WithDateTimeYesterdayType()
        {
            //-------------Setup Test Pack ------------------
            DateTimeYesterday dateTimeToday = new DateTimeYesterday();
            IComparable comparable = dateTimeToday;
            //-------------Execute test ---------------------
            int i = comparable.CompareTo(new DateTimeYesterday());
            //-------------Test Result ----------------------
            Assert.AreEqual(0, i);
        }

        [Test]
        public void Test_Comparable_GreaterThan()
        {
            //-------------Setup Test Pack ------------------
            DateTimeYesterday dateTimeToday = new DateTimeYesterday();
            IComparable comparable = dateTimeToday;
            DateTime dateTime = DateTimeYesterday.Value.AddDays(-1);
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
            DateTimeYesterday dateTimeToday = new DateTimeYesterday();
            IComparable comparable = dateTimeToday;
            DateTime dateTime = DateTimeYesterday.Value.AddDays(1);

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
            DateTimeYesterday dateTimeToday = new DateTimeYesterday();
            IComparable<DateTime> comparable = dateTimeToday;
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(DateTimeYesterday.Value);
            //-------------Test Result ----------------------
            Assert.AreEqual(0, i);
        }

        [Test]
        public void Test_Comparable_OfDateTime_GreaterThan()
        {
            //-------------Setup Test Pack ------------------
            DateTimeYesterday dateTimeToday = new DateTimeYesterday();
            IComparable<DateTime> comparable = dateTimeToday;
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(DateTimeYesterday.Value.AddDays(-1));
            //-------------Test Result ----------------------
            Assert.AreEqual(1, i);
        }

        [Test]
        public void Test_Comparable_OfDateTime_LessThan()
        {
            //-------------Setup Test Pack ------------------
            DateTimeYesterday dateTimeToday = new DateTimeYesterday();
            IComparable<DateTime> comparable = dateTimeToday;
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(DateTimeYesterday.Value.AddDays(1));
            //-------------Test Result ----------------------
            Assert.AreEqual(-1, i);
        }

        [Test]
        public void Test_Equals_WithNull_ShouldReturnFalse()
        {
            //-------------Setup Test Pack ------------------
            DateTimeYesterday dateTimeToday = new DateTimeYesterday();
            const DateTimeYesterday dateTimeToday2 = null;
            //-------------Execute test ---------------------
            bool result = dateTimeToday.Equals(dateTimeToday2);
            //-------------Test Result ----------------------
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_Equals_WithDateTimeValue_ShouldReturnFalse()
        {
            //-------------Setup Test Pack ------------------
            DateTimeYesterday dateTimeToday = new DateTimeYesterday();
            DateTime dateTime2 = DateTime.Now;
            //-------------Execute test ---------------------
            bool result = dateTimeToday.Equals(dateTime2);
            //-------------Test Result ----------------------
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_Equals_WithDateTimeYesterdayType_ShouldReturnTrue()
        {
            //-------------Setup Test Pack ------------------
            DateTimeYesterday dateTimeToday = new DateTimeYesterday();
            DateTimeYesterday dateTimeToday2 = new DateTimeYesterday();
            //-------------Execute test ---------------------
            bool result = dateTimeToday.Equals(dateTimeToday2);
            //-------------Test Result ----------------------
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_Value_ShouldReturnToday()
        {
            //-------------Setup Test Pack ------------------
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------
            DateTime dateTime = DateTimeYesterday.Value;
            //-------------Test Result ----------------------
            Assert.AreEqual(DateTime.Today, dateTime);
        }
        [Test]
        public void Test_Value_WhenOffSet1_ShouldReturnTomorrow()
        {
            //-------------Setup Test Pack ------------------
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------
            DateTime dateTime = DateTimeYesterday.Value;
            //-------------Test Result ----------------------
            Assert.AreEqual(DateTime.Today, dateTime);
        }

        [Test]
        public void Test_ToString()
        {
            //---------------Set up test pack-------------------
            DateTimeYesterday dteNow = new DateTimeYesterday();
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
            object[] customAttributes = typeof(DateTimeYesterday).GetCustomAttributes(false);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, customAttributes.Length);
            TypeConverterAttribute typeConverterAttribute =
                TestUtil.AssertIsInstanceOf<TypeConverterAttribute>(customAttributes[0]);
            Assert.AreEqual(typeof(DateTimeTodayConverter).AssemblyQualifiedName, typeConverterAttribute.ConverterTypeName);
        }

        [Test]
        public void Test_TypeConverter_ShouldBeDefaultTypeConverterForDateTimeYesterday()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(DateTimeYesterday));
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(DateTimeTodayConverter), typeConverter);
        }
    }
}