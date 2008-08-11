using System;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.Base
{
    [TestFixture]
    public class TestDateTimeNow
    {
        [Test]
        public void TestComparable_Equals()
        {
            //-------------Setup Test Pack ------------------
            DateTimeNow dateTimeNow = new DateTimeNow();
            IComparable comparable = dateTimeNow;
            DateTime dateTime = DateTimeNow.Value;

            //-------------Execute test ---------------------
            int i = comparable.CompareTo(dateTime);

            //-------------Test Result ----------------------
            Assert.AreEqual(0, i);
        }

        [Test]
        public void TestComparable_GreaterThan()
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
        public void TestComparable_LessThan()
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

        [Test]
        public void TestComparable_OfDateTime_Equals()
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
        public void TestComparable_OfDateTime_GreaterThan()
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
        public void TestComparable_OfDateTime_LessThan()
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
        public void TestEquals_NullFails()
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
        public void TestValue()
        {
            //-------------Setup Test Pack ------------------
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            DateTime dateTimeBefore = DateTime.Today;
            DateTime dateTime = DateTimeNow.Value;
            DateTime dateTimeAfter = DateTime.Today.AddDays(1);

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
            Assert.AreEqual(dteNow.ToString(), dteParsedDateTime.ToString());
        }
    }
}