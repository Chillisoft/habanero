using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.Base
{
    [TestFixture]
    public class TestDateTimeToday
    {
        [Test]
        public void TestEquals()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday dateTimeToday = new DateTimeToday();
            DateTimeToday dateTimeToday2 = new DateTimeToday();
            
            //-------------Execute test ---------------------
            bool result = dateTimeToday.Equals(dateTimeToday2);

            //-------------Test Result ----------------------
            Assert.IsTrue(result);
        }

        [Test]
        public void TestEquals_NullFails()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday dateTimeToday = new DateTimeToday();
            DateTimeToday dateTimeToday2 = null;

            //-------------Execute test ---------------------
            bool result = dateTimeToday.Equals(dateTimeToday2);

            //-------------Test Result ----------------------
            Assert.IsFalse(result);
        }

        [Test]
        public void TestValue()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday dateTimeToday = new DateTimeToday();
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            DateTime dateTimeBefore = DateTime.Today;
            DateTime dateTime = DateTimeToday.Value;
            DateTime dateTimeAfter = DateTime.Today;

            //-------------Test Result ----------------------
            Assert.GreaterOrEqual(dateTimeBefore, dateTime);
            Assert.LessOrEqual(dateTimeAfter, dateTime);
        }

        [Test]
        public void TestComparable_OfDateTime_Equals()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday dateTimeToday = new DateTimeToday();
            IComparable<DateTime> comparable = dateTimeToday;
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(DateTimeToday.Value);
            //-------------Test Result ----------------------
            Assert.AreEqual(0, i);
        }

        [Test]
        public void TestComparable_OfDateTime_LessThan()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday dateTimeToday = new DateTimeToday();
            IComparable<DateTime> comparable = dateTimeToday;
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(DateTimeToday.Value.AddDays(1));
            //-------------Test Result ----------------------
            Assert.AreEqual(-1, i);
        }

        [Test]
        public void TestComparable_OfDateTime_GreaterThan()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday dateTimeToday = new DateTimeToday();
            IComparable<DateTime> comparable = dateTimeToday;
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(DateTimeToday.Value.AddDays(-1));
            //-------------Test Result ----------------------
            Assert.AreEqual(1, i);
        }

        [Test]
        public void TestComparable_Equals()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday dateTimeToday = new DateTimeToday();
            IComparable comparable = dateTimeToday;
            DateTime dateTime = DateTimeToday.Value;

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(dateTime);

            //-------------Test Result ----------------------
            Assert.AreEqual(0, i);
        }

        [Test]
        public void TestComparable_LessThan()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday dateTimeToday = new DateTimeToday();
            IComparable comparable = dateTimeToday;
            DateTime dateTime = DateTimeToday.Value.AddDays(1);

            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(dateTime);

            //-------------Test Result ----------------------
            Assert.AreEqual(-1, i);
        }

        [Test]
        public void TestComparable_GreaterThan()
        {
            //-------------Setup Test Pack ------------------
            DateTimeToday dateTimeToday = new DateTimeToday();
            IComparable comparable = dateTimeToday;
            DateTime dateTime = DateTimeToday.Value.AddDays(-1);
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(dateTime);
            //-------------Test Result ----------------------
            Assert.AreEqual(1, i);
        }

    }
}
