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
using Habanero.Base;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{
    [TestFixture]
    public class TestDateTimeUtilities
        //:TestBase
    {
        [SetUp]
        public
            void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
        }

        [TestFixtureSetUp]
        public
            void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public
            void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }

        [Test]
        public void Test_FirstDayOfFinYear_MonthGTStartMonth()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime firstDay = DateTimeUtilities.FirstDayOFinancialYear(4, new DateTime(2007, 4, 12));

            //---------------Test Result -----------------------
            Assert.AreEqual(4, firstDay.Month);
            Assert.AreEqual(1, firstDay.Day);
            Assert.AreEqual(2007, firstDay.Year);
        }

        [Test]
        public void Test_FirstDayOfFinYear_MonthLTStartMonth()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime firstDay = DateTimeUtilities.FirstDayOFinancialYear(4, new DateTime(2007, 3, 12));
           
            //---------------Test Result -----------------------
            Assert.AreEqual(4, firstDay.Month);
            Assert.AreEqual(1, firstDay.Day);
            Assert.AreEqual(2006, firstDay.Year);
        }
        [Test]
        public void Test_FirstDayOfFinYear_MonthGTStartDecember()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime firstDay = DateTimeUtilities.FirstDayOFinancialYear(12, new DateTime(2007, 12, 12));

            //---------------Test Result -----------------------
            Assert.AreEqual(12, firstDay.Month);
            Assert.AreEqual(1, firstDay.Day);
            Assert.AreEqual(2007, firstDay.Year);
        }

        [Test]
        public void Test_FirstDayOfFinYear_MonthGTStartJanuary()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime firstDay = DateTimeUtilities.FirstDayOFinancialYear(01, new DateTime(2007, 12, 12));

            //---------------Test Result -----------------------
            Assert.AreEqual(1, firstDay.Month);
            Assert.AreEqual(1, firstDay.Day);
            Assert.AreEqual(2007, firstDay.Year);
        }

        [Test]
        public void Test_FirstDayOfFinYear_InvalidStartMonth()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                DateTimeUtilities.FirstDayOFinancialYear(13, new DateTime(2007, 12, 12));
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (ArgumentOutOfRangeException ex)
            {
                StringAssert.Contains("Year, Month, and Day parameters describe an un-representable DateTime", ex.Message);
            }
      
        }

        [Test]
        public void Test_LastDayOfFinYear()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime firstDay = DateTimeUtilities.LastDayOfFinancialYear(1, new DateTime(2007, 12, 11));
            
            //---------------Test Result -----------------------
            Assert.AreEqual(12, firstDay.Month);
            Assert.AreEqual(31, firstDay.Day);
            Assert.AreEqual(2007, firstDay.Year);
        }
        [Test]
        public void Test_LastDayOfFinYear_MonthGTStartDecember()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime firstDay = DateTimeUtilities.LastDayOfFinancialYear(12, new DateTime(2007, 12, 12));
            
            //---------------Test Result -----------------------
            Assert.AreEqual(11, firstDay.Month);
            Assert.AreEqual(30, firstDay.Day);
            Assert.AreEqual(2008, firstDay.Year);
        }

        [Test]
        public void Test_LastDayOfFinYear_CurrentDateOnStartDate()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime firstDay = DateTimeUtilities.LastDayOfFinancialYear(1, new DateTime(2007, 01, 01));
            //---------------Test Result -----------------------
            Assert.AreEqual(12, firstDay.Month);
            Assert.AreEqual(31, firstDay.Day);
            Assert.AreEqual(2007, firstDay.Year);
        }
        [Test]
        public void Test_LastDayOfFinYear_CurrentDateOnLastDay()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime firstDay = DateTimeUtilities.LastDayOfFinancialYear(1, new DateTime(2007, 12, 31));
            //---------------Test Result -----------------------
            Assert.AreEqual(12, firstDay.Month);
            Assert.AreEqual(31, firstDay.Day);
            Assert.AreEqual(2007, firstDay.Year);
        }

        [Test]
        public void Test_CloseToDateTimeNow_WhenBeforeNow_WhenWithinRange_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            int variant = TestUtil.GetRandomInt(10, 100);
            DateTime testValue = DateTime.Now.AddSeconds(-variant);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool result = DateTimeUtilities.CloseToDateTimeNow(testValue, variant + 1);
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_CloseToDateTimeNow_WhenAfterNow_WhenWithinRange_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            int variant = TestUtil.GetRandomInt(10, 100);
            DateTime testValue = DateTime.Now.AddSeconds(variant);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool result = DateTimeUtilities.CloseToDateTimeNow(testValue, variant + 1);
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_CloseToDateTimeNow_WhenBeforeNow_WhenOutOfRange_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            int variant = TestUtil.GetRandomInt(10, 100);
            DateTime testValue = DateTime.Now.AddSeconds(-variant);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool result = DateTimeUtilities.CloseToDateTimeNow(testValue, variant - 1);
            //---------------Test Result -----------------------
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_CloseToDateTimeNow_WhenAfterNow_WhenOutOfRange_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            int variant = TestUtil.GetRandomInt(10, 100);
            DateTime testValue = DateTime.Now.AddSeconds(variant);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool result = DateTimeUtilities.CloseToDateTimeNow(testValue, variant - 1);
            //---------------Test Result -----------------------
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_TryParseDate_WhenNull_ShouldRetTrueAndRetValueNull()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime? parsedValue;
            bool parsed = DateTimeUtilities.TryParseDate(null, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parsed);
            Assert.IsNull(parsedValue);
        } 
        [Test]
        public void Test_TryParseDate_WhenTodayString_ShouldRetTrueAndRetTodayValue()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime? parsedValue;
            bool parsed = DateTimeUtilities.TryParseDate("today", out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parsed);
            Assert.AreEqual(DateTime.Today, parsedValue);
        } 
        [Test]
        public void Test_TryParseDate_WhenYesterdayString_ShouldRetTrueAndRetTodayValue()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime? parsedValue;
            bool parsed = DateTimeUtilities.TryParseDate("yesterday", out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parsed);
            Assert.AreEqual(DateTime.Today.AddDays(-1), parsedValue);
        } 
        [Test]
        public void Test_TryParseValue_WhenTomorrowString_ShouldRetTrueAndRetTodayValue()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsed = DateTimeUtilities.TryParseValue("tomorrow", out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parsed);
            Assert.IsInstanceOf<DateTimeToday>(parsedValue);
            Assert.AreEqual(1, ((DateTimeToday)parsedValue).OffSet);
        }
        [Test]
        public void Test_TryParseDate_WhenTomorrowString_ShouldRetTrueAndRetTodayValue()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime? parsedValue;
            bool parsed = DateTimeUtilities.TryParseDate("tomorrow", out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parsed);
            Assert.AreEqual(DateTime.Today.AddDays(1), parsedValue);
        }
        [Test]
        public void Test_TryParseDate_WhenDateString_ShouldRetTrueAndRetDate()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime? parsedValue;
            bool parsed = DateTimeUtilities.TryParseDate("01/31/2010", out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parsed);
            Assert.AreEqual(new DateTime(2010, 01, 31), parsedValue);
        }
        [Test]
        public void Test_TryParseDate_WhenDateValue_ShouldRetTrueAndRetDateValue()
        {
            //---------------Set up test pack-------------------
            var dateTime = new DateTime(2010, 01, 31);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DateTime? parsedValue;
            bool parsed = DateTimeUtilities.TryParseDate(dateTime, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parsed);
            Assert.AreEqual(dateTime, parsedValue);
        }
        [Test]
        public void Test_TryParseDate_WhenNot_ShouldRetFalseAndNull()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DateTime? parsedValue;
            bool parsed = DateTimeUtilities.TryParseDate("Invalid", out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsFalse(parsed);
            Assert.IsNull(parsedValue);
        }
    }
}