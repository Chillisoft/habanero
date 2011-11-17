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
using Habanero.Base;
using Habanero.Base.Util;
using Habanero.Test.Base;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{
    [TestFixture]
    public class TestDateTimeUtilities
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

        #region FirstDayofFinYear

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

        #endregion


        #region LastDayOfFinYear

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

        #endregion


        #region CloseToDateTimeNow

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

        #endregion


        #region TryParseDate

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

        [Test]
        public void TryParseDate_WhenStringInStandardFormat()
        {
            //---------------Set up test pack-------------------
            // we cant use DateTime.Now because our serialisation format loses the microseconds so we
            // need a date without microseconds to compare to.
            var expectedDateTime = DateTime.Today.AddHours(-4).AddMinutes(-2).AddSeconds(-20).AddMilliseconds(352);
            string dateAsString = expectedDateTime.ToString(DateTimeUtilities.StandardDateTimeFormat);
            //---------------Execute Test ----------------------
            DateTime? parsedDateTime ;
            var result = DateTimeUtilities.TryParseDate(dateAsString, out parsedDateTime);
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
            Assert.AreEqual(expectedDateTime, parsedDateTime);
        }

        [Test]
        public void Test_TryParseDate_WhenDateString_ShouldRetTrueAndRetDate()
        {
            //---------------Set up test pack-------------------
            const string dateTimeToBeParsed = "01/31/2010";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime? parsedValue;
            var parsed = DateTimeUtilities.TryParseDate(dateTimeToBeParsed, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parsed, dateTimeToBeParsed + " should be parsed but was not. Out Value was : " + parsedValue);
            Assert.AreEqual(new DateTime(2010, 01, 31), parsedValue);
        }

        //[Test]
        //public void Test_TryParseDate_WhenDateString_ddMMYYYY_ShouldRetTrueAndRetDate()
        //{
        //    //---------------Set up test pack-------------------
        //    const string dateTimeToBeParsed = "31/01/2010";
        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    DateTime? parsedValue;
        //    var parsed = DateTimeUtilities.TryParseDate(dateTimeToBeParsed, out parsedValue);
        //    //---------------Test Result -----------------------
        //    Assert.IsTrue(parsed, dateTimeToBeParsed + " should be parsed but was not. Out Value was : " + parsedValue);
        //    Assert.AreEqual(new DateTime(2010, 01, 31), parsedValue);
        //}
        #endregion

        #region DayEnd

        [Test]
        public void Test_DayEnd_WhenNoOffSet_ShouldReturnLastMillisecondOfTheday()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = DateTime.Now;
            DateTime expectedDayEnd = dateTimeCurrent.Date.AddDays(1).AddMilliseconds(-1);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualDayEnd  = DateTimeUtilities.DayEnd(dateTimeCurrent);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDayEnd, actualDayEnd);
        }

        [Test]
        public void Test_DayEnd_WithOffSet_ShouldReturnLastMillisecondOfThedayPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = DateTime.Now;
            TimeSpan sixHourOffSet = new TimeSpan(6, 0, 0);
            DateTime expectedDayEnd = dateTimeCurrent.Date.AddDays(1).AddMilliseconds(-1).Add(sixHourOffSet);
            if((dateTimeCurrent - DateTimeUtilities.DayStart(dateTimeCurrent)) < sixHourOffSet)
            {
                expectedDayEnd = dateTimeCurrent.Date.AddMilliseconds(-1).Add(sixHourOffSet);
            }
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualDayEnd = DateTimeUtilities.DayEnd(dateTimeCurrent, sixHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDayEnd, actualDayEnd);
        }

        [Test]
        public void Test_DayEnd_WithOffSet_WithFixedDate_ShouldReturnLastMillisecondOfThedayPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = DateTime.Parse("31-May-2010 01:07:53");
            TimeSpan sixHourOffSet = new TimeSpan(6, 0, 0);
            DateTime expectedDayEnd = DateTime.Parse("2010-05-31 05:59:59.999");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualDayEnd = DateTimeUtilities.DayEnd(dateTimeCurrent, sixHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDayEnd, actualDayEnd);
        }


        [Test]
        public void Test_DayEnd_WithOffSetGTNow_ShouldReturnLastMillisecondOfThedayPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 10, 27, 1, 0, 0);
            TimeSpan sixHourOffSet = new TimeSpan(6, 0, 0);
            //DateTime expectedDayEnd = dateTimeCurrent.Date.AddDays(1).AddMilliseconds(-1).Add(sixHourOffSet);
            DateTime expectedDayEnd = new DateTime(2010, 10, 27, 5, 59, 59, 999);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualDayEnd = DateTimeUtilities.DayEnd(dateTimeCurrent, sixHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDayEnd, actualDayEnd);
        }

        #endregion

        [Test]
        public void Test_DayStart_WhenNoOffSet_ShouldReturnStartOffday()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = DateTime.Now;
            DateTime expectedStartTime = dateTimeCurrent.Date;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualDayStart = DateTimeUtilities.DayStart(dateTimeCurrent);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualDayStart);
        }

        [Test]
        public void Test_DayStart_WithOffOffSetLTNow_ShouldReturnStartOffdayPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = DateTime.Now;
            TimeSpan sixHourOffSet = new TimeSpan(6, 0, 0);
            DateTime expectedStartTime = dateTimeCurrent.Date.Add(sixHourOffSet);
            if ((dateTimeCurrent - DateTimeUtilities.DayStart(dateTimeCurrent)) < sixHourOffSet)
            {
                expectedStartTime = dateTimeCurrent.Date.AddDays(-1).Add(sixHourOffSet);
            }
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualDayStart = DateTimeUtilities.DayStart(dateTimeCurrent, sixHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualDayStart);
        }
        [Test]
        public void Test_DayStart_WithOffSet_WithFixedDate_ShouldReturnStartOffdayPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = DateTime.Parse("31-May-2010 01:07:53");
            TimeSpan sixHourOffSet = new TimeSpan(6, 0, 0);
            DateTime expectedStartTime = DateTime.Parse("2010-05-30 06:00:00.000");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualDayEnd = DateTimeUtilities.DayStart(dateTimeCurrent, sixHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualDayEnd);
        }
        [Test]
        public void Test_DayStart_WithOffOffSetGTNow_ShouldReturnStartOffdayPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = GetDateTimeCurrent(3);
            TimeSpan sixHourOffSet = new TimeSpan(6, 0, 0);
            DateTime expectedStartTime = dateTimeCurrent.Date.AddDays(-1).Add(sixHourOffSet);
            //---------------Assert Precondition----------------
            Assert.Greater(dateTimeCurrent.Date.Add(sixHourOffSet), dateTimeCurrent);
            //---------------Execute Test ----------------------
            var actualDayStart = DateTimeUtilities.DayStart(dateTimeCurrent, sixHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualDayStart);
        }

        [Test]
        public void Test_OnOrPreviousDayOfWeek_WhenThursday_ShouldReturnPrevSunday()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 27);
            var expectedPreviousSunday = new DateTime(2010, 05, 23);
            //---------------Assert Precondition----------------
            Assert.AreEqual(DayOfWeek.Thursday, dateTimeCurrent.DayOfWeek);
            //---------------Execute Test ----------------------
            var actualPreviousSunday = DateTimeUtilities.OnOrPreviousDayOfWeek(dateTimeCurrent, DayOfWeek.Sunday);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedPreviousSunday, actualPreviousSunday);
            Assert.AreEqual(DayOfWeek.Sunday, actualPreviousSunday.DayOfWeek);
        }
        [Test]
        public void Test_OnOrPreviousDayOfWeek_WhenSunday_ShouldReturnPrevSunday()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 23);
            var expectedPreviousSunday = new DateTime(2010, 05, 23);
            //---------------Assert Precondition----------------
            Assert.AreEqual(DayOfWeek.Sunday, dateTimeCurrent.DayOfWeek);
            //---------------Execute Test ----------------------
            var actualPreviousSunday = DateTimeUtilities.OnOrPreviousDayOfWeek(dateTimeCurrent, DayOfWeek.Sunday);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedPreviousSunday, actualPreviousSunday);
            Assert.AreEqual(DayOfWeek.Sunday, actualPreviousSunday.DayOfWeek);
        }
        [Test]
        public void Test_WeekStart_WhenNoOffSet_ShouldReturnStartOffWeek()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 27, 12, 22, 34);
            var expectedStartTime = new DateTime(2010, 05, 23);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualDayStart = DateTimeUtilities.WeekStart(dateTimeCurrent);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualDayStart);
        }

        [Test]
        public void Test_WeekStart_WithOffSetLTNow_ShouldReturnStartOffWeekPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 27, 12, 22, 34);
            TimeSpan sixHourOffSet = new TimeSpan(6, 0, 0);
            DateTime expectedStartTime = new DateTime(2010, 05, 23).Date.Add(sixHourOffSet);
            //---------------Assert Precondition----------------
            Assert.Greater(dateTimeCurrent, expectedStartTime);
            //---------------Execute Test ----------------------
            var actualDayStart = DateTimeUtilities.WeekStart(dateTimeCurrent, sixHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualDayStart);
        }
        [Test]
        public void Test_WeekStart_WithOffSetGTNow_ShouldReturnStartOffWeekPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 23, 3, 22, 34);
            TimeSpan sixHourOffSet = new TimeSpan(6, 0, 0);
            DateTime expectedStartTime = new DateTime(2010, 05, 16).Add(sixHourOffSet);
            //---------------Assert Precondition----------------
            Assert.Greater(dateTimeCurrent, expectedStartTime);
            //---------------Execute Test ----------------------
            var actualDayStart = DateTimeUtilities.WeekStart(dateTimeCurrent, sixHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualDayStart);
        }

        [Test]
        public void Test_WeekEnd_WhenNoOffSet_ShouldReturnEndOffWeek()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 27, 12, 22, 34);
            var expectedEndTime = new DateTime(2010, 05, 30).AddMilliseconds(-1);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualDayEnd = DateTimeUtilities.WeekEnd(dateTimeCurrent);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedEndTime, actualDayEnd);
        }
        [Test]
        public void Test_WeekEnd_WithOffSetLTNow_ShouldReturnEndOffWeekPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 27, 12, 22, 34);
            TimeSpan sixHourOffSet = new TimeSpan(6, 0, 0);
            var expectedEndTimeWithoutOffSet = new DateTime(2010, 05, 30).AddMilliseconds(-1);
            DateTime expectedEndTime = expectedEndTimeWithoutOffSet.Add(sixHourOffSet);
            //---------------Assert Precondition----------------
            Assert.Less(dateTimeCurrent, expectedEndTime);
            //---------------Execute Test ----------------------
            var actualDayEnd = DateTimeUtilities.WeekEnd(dateTimeCurrent, sixHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedEndTime, actualDayEnd);
        }
        [Test]
        public void Test_WeekEnd_WithOffSetGTNow_ShouldReturnEndOffWeekPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 24, 12, 22, 34);
            TimeSpan sixDaySixHourOffSet = new TimeSpan(6, 6, 0, 0);
            var expectedEndTimeWithoutOffSet = new DateTime(2010, 05, 30).AddMilliseconds(-1);
            DateTime expectedEndTime = new DateTime(2010, 05, 23).AddMilliseconds(-1).Add(sixDaySixHourOffSet);
            //---------------Assert Precondition----------------
            Assert.Less(dateTimeCurrent, expectedEndTimeWithoutOffSet.Add(sixDaySixHourOffSet));
//            Assert.Less(dateTimeCurrent, expectedEndTime);
            //---------------Execute Test ----------------------
            var actualDayEnd = DateTimeUtilities.WeekEnd(dateTimeCurrent, sixDaySixHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedEndTime, actualDayEnd);
        }

        [Test]
        public void Test_MonthStart_WhenNoOffSet_ShouldReturnStartOffMonth()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 27, 12, 22, 34);
            var expectedStartTime = new DateTime(2010, 05, 01);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualMonthStart = DateTimeUtilities.MonthStart(dateTimeCurrent);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualMonthStart);
        }
/*        [Test]
        public void Test_MonthStart_WhenNoOffSet_WhenCurrentLastMillisecont_ShouldReturnStartOffMonth()
        {
            yield return new TestCaseData(new DateRangeTestCase(DateRangeOptions.PreviousMonth
        , "31 May 2010 23:59:59.999"
        , "01 May 2010"
        , "31 May 2010 23:59:59.999")).SetDescription("End EdgeCase");

            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 27, 12, 22, 34);
            var expectedStartTime = new DateTime(2010, 05, 01);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualMonthStart = DateTimeUtilities.MonthStart(dateTimeCurrent);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualMonthStart);
        }*/

        [Test]
        public void Test_MonthStart_WithOffSetLTNow_ShouldReturnStartOffMonthPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 27, 12, 22, 34);
            TimeSpan sixDayTwoHourOffSet = new TimeSpan(6, 2, 0, 0);
            DateTime expectedStartTime = new DateTime(2010, 05, 01).Date.Add(sixDayTwoHourOffSet);
            //---------------Assert Precondition----------------
            Assert.Greater(dateTimeCurrent, expectedStartTime);
            //---------------Execute Test ----------------------
            var actualMonthStart = DateTimeUtilities.MonthStart(dateTimeCurrent, sixDayTwoHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualMonthStart);
        }
        [Test]
        public void Test_MonthStart_WithOffSetGTNow_ShouldReturnStartOffMonthPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 05, 3, 22, 34);
            TimeSpan sixDayTwoHourOffSet = new TimeSpan(6, 2, 0, 0);
            DateTime expectedStartTime = new DateTime(2010, 04, 01).Add(sixDayTwoHourOffSet);
            //---------------Assert Precondition----------------
            Assert.Greater(dateTimeCurrent, expectedStartTime);
            //---------------Execute Test ----------------------
            var actualMonthStart = DateTimeUtilities.MonthStart(dateTimeCurrent, sixDayTwoHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualMonthStart);
        }
        [Test]
        public void Test_MonthStart_WithNegTimeSpanOffSetLTNow_ShouldReturnStartOffNextMonthAddOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 31, 23, 22, 34);
            TimeSpan negTwoHourOffSet = new TimeSpan(0, -2, 0, 0);
            DateTime expectedStartTime = new DateTime(2010, 06, 01).Add(negTwoHourOffSet);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualMonthStart = DateTimeUtilities.MonthStart(dateTimeCurrent, negTwoHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualMonthStart);
        }
        [Test]
        public void Test_MonthStart_WithNegTimeSpanOffSetLTNow_WhenCurrMonthDecember_ShouldReturnStartOffNextMonthAddOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 12, 31, 23, 22, 34);
            TimeSpan negTwoHourOffSet = new TimeSpan(0, -2, 0, 0);
            DateTime expectedStartTime = new DateTime(2011, 01, 01).Add(negTwoHourOffSet);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualMonthStart = DateTimeUtilities.MonthStart(dateTimeCurrent, negTwoHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualMonthStart);
        }

        [Test]
        public void Test_MonthEnd_WhenNoOffSet_ShouldReturnEndOffWeek()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 27, 12, 22, 34);
            var expectedEndTime = new DateTime(2010, 06, 1).AddMilliseconds(-1);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualMonthEnd = DateTimeUtilities.MonthEnd(dateTimeCurrent);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedEndTime, actualMonthEnd);
        }
        [Test]
        public void Test_MonthEnd_WithOffSetLTNow_ShouldReturnEndOffWeekPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 24, 12, 22, 34);
            TimeSpan twoDaySixHourOffSet = new TimeSpan(2, 6, 0, 0);
            var expectedEndTimeWithoutOffSet = new DateTime(2010, 06, 1).AddMilliseconds(-1);
            DateTime expectedEndTime = expectedEndTimeWithoutOffSet.Add(twoDaySixHourOffSet);
            //---------------Assert Precondition----------------
            Assert.Less(dateTimeCurrent, expectedEndTimeWithoutOffSet.Add(twoDaySixHourOffSet));
            //---------------Execute Test ----------------------
            var actualMonthEnd = DateTimeUtilities.MonthEnd(dateTimeCurrent, twoDaySixHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedEndTime, actualMonthEnd);
        }

        [Test]
        public void Test_MonthEnd_WithOffSetGTNow_ShouldReturnStartOffMonthPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 05, 3, 22, 34);
            TimeSpan sixDayTwoHourOffSet = new TimeSpan(6, 2, 0, 0);
            DateTime expectedStartTime = new DateTime(2010, 04, 01).Add(sixDayTwoHourOffSet);
            //---------------Assert Precondition----------------
            Assert.Greater(dateTimeCurrent, expectedStartTime);
            //---------------Execute Test ----------------------
            var actualMonthEnd = DateTimeUtilities.MonthStart(dateTimeCurrent, sixDayTwoHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualMonthEnd);
        }


        [Test]
        public void Test_MonthEnd_WithNegTimeSpanOffSetLTNow_ShouldReturnEndOffNextMonthAddOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 06, 30, 23, 22, 34);
            TimeSpan negTwoHourOffSet = new TimeSpan(0, -2, 0, 0);
            DateTime expectedEndTime = new DateTime(2010, 07, 31, 23, 59, 59, 999).Add(negTwoHourOffSet);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualMonthEnd = DateTimeUtilities.MonthEnd(dateTimeCurrent, negTwoHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedEndTime, actualMonthEnd);
        }

        [Test]
        public void Test_MonthEnd_WithNegTimeSpanOffSetLTNow_WhenCurrMonthDec_ShouldReturnEndOffNextMonthAddOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 12, 31, 23, 22, 34);
            TimeSpan negTwoHourOffSet = new TimeSpan(0, -2, 0, 0);
            DateTime expectedEndTime = new DateTime(2011, 01, 31, 23, 59, 59, 999).Add(negTwoHourOffSet);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualDayStart = DateTimeUtilities.MonthEnd(dateTimeCurrent, negTwoHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedEndTime, actualDayStart);
        }
        [Test]
        public void Test_MonthEnd_WithNegTimeSpanOffSetLTNow_WhenCurrMonthNov_ShouldReturnEndOffNextMonthAddOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 11, 30, 23, 22, 34);
            TimeSpan negTwoHourOffSet = new TimeSpan(0, -2, 0, 0);
            DateTime expectedEndTime = new DateTime(2010, 12, 31, 23, 59, 59, 999).Add(negTwoHourOffSet);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualDayStart = DateTimeUtilities.MonthEnd(dateTimeCurrent, negTwoHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedEndTime, actualDayStart);
        }

        [Test]
        public void Test_MonthEndWithOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 06, 02, 1, 1, 4);
            TimeSpan offSet = new TimeSpan(2, 3, 1, 2);
            DateTime expectedEndTime = new DateTime(2010, 06, 03, 3, 1, 1, 999);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualDayStart = DateTimeUtilities.MonthEnd(dateTimeCurrent, offSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedEndTime, actualDayStart);
        }
        [Test]
        public void Test_YearStart_WhenNoOffSet_ShouldReturnStartOffYear()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 27, 12, 22, 34);
            var expectedStartTime = new DateTime(2010, 01, 01);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualDayStart = DateTimeUtilities.YearStart(dateTimeCurrent);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualDayStart);
        }
        [Test]
        public void Test_YearStart_WithOffSetLTNow_ShouldReturnStartOffYearPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 27, 12, 22, 34);
            const int threeMonthOffSet = 3;
            DateTime expectedStartTime = new DateTime(2010, 01, 01).Date.AddMonths(threeMonthOffSet);
            //---------------Assert Precondition----------------
            Assert.Greater(dateTimeCurrent, expectedStartTime);
            //---------------Execute Test ----------------------
            var actualDayStart = DateTimeUtilities.YearStart(dateTimeCurrent, threeMonthOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualDayStart);
        }


        [Test]
        public void Test_YearStart_WithOffSetGTNow_ShouldReturnStartOffYearPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 02, 23, 3, 22, 34);
            const int threeMonthOffSet = 3;
            DateTime expectedStartTime = new DateTime(2009, 04, 01);
            //---------------Assert Precondition----------------
            Assert.Greater(new DateTime(2010, 04, 01), expectedStartTime);
            //---------------Execute Test ----------------------
            var actualDayStart = DateTimeUtilities.YearStart(dateTimeCurrent, threeMonthOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualDayStart);
        }

        [Test]
        public void Test_YearStart_WithMonthAndTimeOffSetLTNow_ShouldReturnStartOffYearPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 27, 12, 22, 34);
            const int threeMonthOffSet = 3;
            TimeSpan oneDayTwoHourOffSet = new TimeSpan(1, 2, 0, 0);
            DateTime expectedStartTime = new DateTime(2010, 01, 01).Date.AddMonths(threeMonthOffSet).Add(oneDayTwoHourOffSet);
            //---------------Assert Precondition----------------
            Assert.Greater(dateTimeCurrent, expectedStartTime);
            //---------------Execute Test ----------------------
            var actualDayStart = DateTimeUtilities.YearStart(dateTimeCurrent, threeMonthOffSet, oneDayTwoHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualDayStart);
        }

        [Test]
        public void Test_YearStart_WithMonthAndTimeOffSetGTNow_ShouldReturnStartOffYearPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 02, 23, 3, 22, 34);
            const int threeMonthOffSet = 3;
            TimeSpan oneDayTwoHourOffSet = new TimeSpan(1, 2, 0, 0);
            DateTime expectedStartTime = new DateTime(2009, 04, 01).Add(oneDayTwoHourOffSet);
            //---------------Assert Precondition----------------
            Assert.Greater(new DateTime(2010, 04, 01), expectedStartTime);
            //---------------Execute Test ----------------------
            var actualDayStart = DateTimeUtilities.YearStart(dateTimeCurrent, threeMonthOffSet, oneDayTwoHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualDayStart);
        }
        [Test]
        public void Test_YearStart_WithMonthAndTimeOffSetGTNow_NegOffSet_ShouldReturnStartOffYearPlusOffSet()
        {

            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2005, 12, 31, 23, 30, 0, 0);
            TimeSpan negOneHrOffset = new TimeSpan(0, -1, 0, 0);
            DateTime expectedStartTime = new DateTime(2005, 12, 31, 23, 0, 0, 0);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualYearStart = DateTimeUtilities.YearStart(dateTimeCurrent, 0, negOneHrOffset);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, actualYearStart);
        }

        [Test]
        public void Test_YearEnd_WhenNoOffSet_ShouldReturnEndOffYear()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 27, 12, 22, 34);
            var expectedEndTime = new DateTime(2011, 01, 01).AddMilliseconds(-1);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualDayEnd = DateTimeUtilities.YearEnd(dateTimeCurrent);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedEndTime, actualDayEnd);
        }
        [Test]
        public void Test_YearEnd_WithOffSetLTNow_ShouldReturnEndOffYearPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 27, 12, 22, 34);
            const int threeMonthOffSet = 3;
            var expectedEndTimeWithoutOffSet = new DateTime(2011, 01, 01).AddMilliseconds(-1);
            DateTime expectedEndTime = expectedEndTimeWithoutOffSet.AddMonths(threeMonthOffSet);
            //---------------Assert Precondition----------------
            Assert.Less(dateTimeCurrent, expectedEndTime);
            //---------------Execute Test ----------------------
            var actualDayEnd = DateTimeUtilities.YearEnd(dateTimeCurrent, threeMonthOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedEndTime, actualDayEnd);
        }
        [Test]
        public void Test_YearEnd_WithOffSetGTNow_ShouldReturnEndOffYearPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 02, 24, 12, 22, 34);
            const int threeMonthOffSet = 3;
            DateTime expectedEndTime = new DateTime(2010, 01, 01).AddMilliseconds(-1).AddMonths(threeMonthOffSet);
            //---------------Assert Precondition----------------
            Assert.Less(dateTimeCurrent, new DateTime(2011, 01, 01).AddMonths(threeMonthOffSet).AddYears(-1));
            //---------------Execute Test ----------------------
            var actualDayEnd = DateTimeUtilities.YearEnd(dateTimeCurrent, threeMonthOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedEndTime, actualDayEnd);
        }

        [Test]
        public void Test_YearEnd_WithMonthAndTimeOffSetLTNow_ShouldReturnEndOffYearPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 05, 27, 12, 22, 34);
            const int threeMonthOffSet = 3;
            TimeSpan oneDayTwoHourOffSet = new TimeSpan(1, 2, 0, 0);
            var expectedEndTimeWithoutOffSet = new DateTime(2011, 01, 01).AddMilliseconds(-1);
            DateTime expectedEndTime = expectedEndTimeWithoutOffSet.AddMonths(threeMonthOffSet).Add(oneDayTwoHourOffSet);
            //---------------Assert Precondition----------------
            Assert.Less(dateTimeCurrent, expectedEndTime);
            //---------------Execute Test ----------------------
            var actualDayEnd = DateTimeUtilities.YearEnd(dateTimeCurrent, threeMonthOffSet, oneDayTwoHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedEndTime, actualDayEnd);
        }

        [Test]
        public void Test_YearEnd_WithMonthAndTimeOffSetGTNow_ShouldReturnEndOffYearPlusOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2010, 02, 24, 12, 22, 34);
            const int threeMonthOffSet = 3;
            TimeSpan oneDayTwoHourOffSet = new TimeSpan(1, 2, 0, 0);
            DateTime expectedEndTime = new DateTime(2010, 01, 01).AddMilliseconds(-1).AddMonths(threeMonthOffSet).Add(oneDayTwoHourOffSet);
            //---------------Assert Precondition----------------
            Assert.Less(dateTimeCurrent, new DateTime(2011, 01, 01).AddMonths(threeMonthOffSet).AddYears(-1));
            //---------------Execute Test ----------------------
            var actualDayEnd = DateTimeUtilities.YearEnd(dateTimeCurrent, threeMonthOffSet, oneDayTwoHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedEndTime, actualDayEnd);
        }

        [Test] 
        public void Test_YearEnd_WhenCurrentLTFirstDayPlusHourOffSet()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2006, 1, 1, 0, 30, 0, 0);
            TimeSpan oneHourOffSet = new TimeSpan(0, 1, 0, 0);
            DateTime expectedEndTime = new DateTime(2006, 1, 1, 0, 59, 59, 999);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualDayEnd = DateTimeUtilities.YearEnd(dateTimeCurrent, 0, oneHourOffSet);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedEndTime, actualDayEnd);
        }
        private static DateTime GetDateTimeCurrent(int hour)
        {
            return GetDateTimeCurrent(hour, 12, 33, 0);
        }

        private static DateTime GetDateTimeCurrent(int hour, int minutes, int seconds, int millisecond)
        {
            const int year = 2007;
            const int month = 11;
            const int day = 13;
            return new DateTime(year, month, day, hour, minutes, seconds, millisecond);
        }
    }
}