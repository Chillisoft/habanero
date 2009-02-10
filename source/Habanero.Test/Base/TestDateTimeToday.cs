//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using NUnit.Framework;

namespace Habanero.Test.Base
{
    [TestFixture]
    public class TestDateTimeToday
    {
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
            const DateTimeToday dateTimeToday2 = null;

            //-------------Execute test ---------------------
            bool result = dateTimeToday.Equals(dateTimeToday2);

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
            DateTime dateTime = DateTimeToday.Value;
            DateTime dateTimeAfter = DateTime.Today;

            //-------------Test Result ----------------------
            Assert.GreaterOrEqual(dateTime, dateTimeBefore);
            Assert.LessOrEqual(dateTime, dateTimeAfter);
        }
    }
}