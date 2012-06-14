using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Base;
using Habanero.Base.Util;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Base
{
    [TestFixture]
    public class TestDateRangeOptionsConverter
    {

        [Test]
        public void Test_SetOffSets_ShouldSetOffsets()
        {
            //---------------Set up test pack-------------------
            var optionsConverter = new DateRangeOptionsConverter();
            var expectedMidnightOffSet = new TimeSpan(0, TestUtil.GetRandomInt(0, 7), TestUtil.GetRandomInt(0, 7), 0);
            var expectedWeekStartOffset = TestUtil.GetRandomInt(0, 7);
            var expectedMonthStartOffset = TestUtil.GetRandomInt(0, 20);
            var expectedYearStartOffset = TestUtil.GetRandomInt(0, 30);    
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            optionsConverter.MidnightOffset = expectedMidnightOffSet;
            optionsConverter.WeekStartOffset = expectedWeekStartOffset;
            optionsConverter.MonthStartOffset = expectedMonthStartOffset;
            optionsConverter.YearStartOffset = expectedYearStartOffset;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedMidnightOffSet, optionsConverter.MidnightOffset);
            Assert.AreEqual(expectedWeekStartOffset, optionsConverter.WeekStartOffset);
            Assert.AreEqual(expectedMonthStartOffset, optionsConverter.MonthStartOffset);
            Assert.AreEqual(expectedYearStartOffset, optionsConverter.YearStartOffset);
        }
        [Test]
        public void Test_Construct_ShouldSetAllOffSetsToZero()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var optionsConverter = new DateRangeOptionsConverter();
            //---------------Test Result -----------------------
            Assert.AreEqual(new TimeSpan(0), optionsConverter.MidnightOffset);
            Assert.AreEqual(0, optionsConverter.WeekStartOffset);
            Assert.AreEqual(0, optionsConverter.MonthStartOffset);
            Assert.AreEqual(0, optionsConverter.YearStartOffset);
        }

        [Test]
        public void Test_SetNow_ShouldChangeDateTimeBeingUsedForCalcs()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = new DateTime(2007,5, 11);
            var optionsConverter = new DateRangeOptionsConverter();
            //--------------Assert PreConditions----------------
            //---------------Execute Test ----------------------
            optionsConverter.SetNow(dateTimeCurrent);
            DateRange dateRange = optionsConverter.ConvertDateRange(DateRangeOptions.Current24Hours);
            //---------------Test Result -----------------------
            Assert.AreEqual(dateTimeCurrent, dateRange.EndDate);
        }
        [Test]
        public void Test_ThisHour_ShouldSetStartDateToBeginHour_AndEndDateToEndOfHour()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = GetDateTimeCurrent(20);
            var dateTimeNowFake = new DateTimeNowFixed(dateTimeCurrent);
            var optionsConverter = new DateRangeOptionsConverter(dateTimeNowFake);
            //---------------Execute Test ----------------------
            DateRange dateRange = optionsConverter.ConvertDateRange(DateRangeOptions.ThisHour);
            //---------------Test Result -----------------------
            var expectedStartDate = GetDateTimeCurrent(20,0,0,0);
            var expectedEndDate = GetDateTimeCurrent(20, 59, 59, 999);
            Assert.AreEqual(expectedStartDate, dateRange.StartDate, "Start Date should be exactly 24 hours ago.");
            Assert.AreEqual(expectedEndDate, dateRange.EndDate, "End Date should be now");
        } 
        [Test]
        public void Test_ThisHour_WhenBeginOfHour_ShouldSetStartDateToBeginHour_AndEndDateToEndOfHour()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = GetDateTimeCurrent(20,0,0,0);
            var dateTimeNowFake = new DateTimeNowFixed(dateTimeCurrent);
            var optionsConverter = new DateRangeOptionsConverter(dateTimeNowFake);
            //---------------Execute Test ----------------------
            DateRange dateRange = optionsConverter.ConvertDateRange(DateRangeOptions.ThisHour);
            //---------------Test Result -----------------------
            var expectedStartDate = GetDateTimeCurrent(20, 0, 0, 0);
            var expectedEndDate = GetDateTimeCurrent(20, 59, 59, 999);
            Assert.AreEqual(expectedStartDate, dateRange.StartDate, "Start Date should be exactly 24 hours ago.");
            Assert.AreEqual(expectedEndDate, dateRange.EndDate, "End Date should be now");
        }

        [Test]
        public void Test_Current24Hours_ShouldSetStartDateToNowLess24_AndEndDateToNow()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = DateTime.Now;
            var dateTimeNowFake = new DateTimeNowFixed(dateTimeCurrent);
            var optionsConverter = new DateRangeOptionsConverter(dateTimeNowFake);
            //--------------Assert PreConditions----------------
            //---------------Execute Test ----------------------
            DateRange dateRange = optionsConverter.ConvertDateRange(DateRangeOptions.Current24Hours);
            //---------------Test Result -----------------------
            var expectedStartDate = dateTimeCurrent.AddHours(-24);
            var expectedEndDate = dateTimeCurrent;
            Assert.AreEqual(expectedStartDate, dateRange.StartDate);
            Assert.AreEqual(expectedEndDate, dateRange.EndDate);
        }
        [Test]
        public void TestPreviousHour_ShouldReturnStartDateEQStartPrevHr_EndDate_EndPrevHr()
        {
            //---------------Set up test pack-------------------
            const int hour = 20;
            DateTime dateTimeCurrent = GetDateTimeCurrent(hour, 38, 12, 999);
            var dateTimeNowFake = new DateTimeNowFixed(dateTimeCurrent);
            var optionsConverter = new DateRangeOptionsConverter(dateTimeNowFake);
           //--------------Assert PreConditions----------------
           //---------------Execute Test ----------------------
           DateRange dateRange = optionsConverter.ConvertDateRange(DateRangeOptions.PreviousHour);
           //---------------Test Result -----------------------
           var expectedStartDate = GetDateTimeCurrent(hour -1, 0, 0);
           var expectedEndDate = GetDateTimeCurrent(hour - 1, 59, 59, 999);
           Assert.AreEqual(expectedStartDate, dateRange.StartDate);
           Assert.AreEqual(expectedEndDate, dateRange.EndDate);
       }

        [Test]
        public void TestCurrent60Minutes()
        {
            //---------------Set up test pack-------------------
            const int hour = 20;
            const int minutes = 38;
            const int seconds = 12;
            const int millisecond = 10;
            DateTime dateTimeCurrent = GetDateTimeCurrent(hour, minutes, seconds, millisecond);
            var dateTimeNowFake = new DateTimeNowFixed(dateTimeCurrent);
            var optionsConverter = new DateRangeOptionsConverter(dateTimeNowFake);
            //--------------Assert PreConditions----------------
            //---------------Execute Test ----------------------
            DateRange dateRange = optionsConverter.ConvertDateRange(DateRangeOptions.Current60Minutes);
            //---------------Test Result -----------------------
            var expectedStartDate = GetDateTimeCurrent(hour - 1, minutes, seconds, millisecond);
            var expectedEndDate = dateTimeCurrent;
            Assert.AreEqual(expectedStartDate, dateRange.StartDate);
            Assert.AreEqual(expectedEndDate, dateRange.EndDate);
        }

        [Test]
        public void TestToday()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = DateTime.Now;
            var expectedStartTime = DateTimeUtilities.DayStart(dateTimeCurrent);
            var expectedEndTime = DateTimeUtilities.DayEnd(dateTimeCurrent);
            var optionsConverter = new DateRangeOptionsConverter(new DateTimeNowFixed(dateTimeCurrent));
            //--------------Assert PreConditions----------------
            //---------------Execute Test ----------------------
            DateRange dateRange = optionsConverter.ConvertDateRange(DateRangeOptions.Today);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartTime, dateRange.StartDate);
            Assert.AreEqual(expectedEndTime, dateRange.EndDate);
        }
        [Test]
        public void Test_Yesterday()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = DateTime.Now;
            var expectedStartTime = DateTimeUtilities.DayStart(dateTimeCurrent).AddDays(-1);
            var expectedEndTime = DateTimeUtilities.DayEnd(dateTimeCurrent).AddDays(-1);
            var optionsConverter = new DateRangeOptionsConverter(new DateTimeNowFixed(dateTimeCurrent));
            //--------------Assert PreConditions----------------
            Assert.AreEqual(expectedStartTime.AddDays(1).AddMilliseconds(-1), expectedEndTime);
            var expectedTimeSpan = expectedEndTime - expectedStartTime;
            Assert.AreEqual(24, Math.Round(expectedTimeSpan.TotalHours,3));
            //---------------Execute Test ----------------------
            DateRange dateRange = optionsConverter.ConvertDateRange(DateRangeOptions.Yesterday);
            //---------------Test Result -----------------------
            var actualTimeSpan = dateRange.EndDate - dateRange.StartDate;
            Assert.AreEqual(24, Math.Round(actualTimeSpan.TotalHours));
            Assert.AreEqual(expectedStartTime, dateRange.StartDate);
            Assert.AreEqual(expectedEndTime, dateRange.EndDate);
        }

        [Test]
        public void Test_ThisWeek()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = DateTime.Now;
            var dateTimeNowFake = new DateTimeNowFixed(dateTimeCurrent);
            var optionsConverter = new DateRangeOptionsConverter(dateTimeNowFake);
            //--------------Assert PreConditions----------------
            //---------------Execute Test ----------------------
            DateRange dateRange = optionsConverter.ConvertDateRange(DateRangeOptions.ThisWeek);
            //---------------Test Result -----------------------
            var expectedStartDate = DateTimeUtilities.WeekStart(dateTimeCurrent);
            var expectedEndDate = DateTimeUtilities.WeekEnd(dateTimeCurrent);
            Assert.AreEqual(expectedStartDate, dateRange.StartDate);
            Assert.AreEqual(expectedEndDate, dateRange.EndDate);
        }

        [Test]
        public void TestPreviousWeek_ReturnsStartAndEndForPreviousWeek()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = DateTime.Now;
            var dateTimeNowFake = new DateTimeNowFixed(dateTimeCurrent);
            var optionsConverter = new DateRangeOptionsConverter(dateTimeNowFake);
            var expectedStartDate = DateTimeUtilities.WeekStart(dateTimeCurrent.AddDays(-7));
            var expectedEndDate = DateTimeUtilities.WeekEnd(dateTimeCurrent.AddDays(-7));
            //--------------Assert PreConditions----------------
            //---------------Execute Test ----------------------
            DateRange dateRange = optionsConverter.ConvertDateRange(DateRangeOptions.PreviousWeek);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartDate, dateRange.StartDate);
            Assert.AreEqual(expectedEndDate, dateRange.EndDate);
        }

        [Test]
        public void TestPrevious7Days()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = DateTime.Now;
            var dateTimeNowFake = new DateTimeNowFixed(dateTimeCurrent);
            var optionsConverter = new DateRangeOptionsConverter(dateTimeNowFake);
            var expectedStartDate = DateTimeUtilities.DayStart( dateTimeCurrent.AddDays(-7));
            var expectedEndDate = dateTimeCurrent;
            //--------------Assert PreConditions----------------
            //---------------Execute Test ----------------------
            DateRange dateRange = optionsConverter.ConvertDateRange(DateRangeOptions.Previous7Days);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartDate, dateRange.StartDate);
            Assert.AreEqual(expectedEndDate, dateRange.EndDate);
        }

        [Test]
        public void TestMonthToDate()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = DateTime.Now;
            var dateTimeNowFake = new DateTimeNowFixed(dateTimeCurrent);
            var optionsConverter = new DateRangeOptionsConverter(dateTimeNowFake);
            var expectedStartDate = DateTimeUtilities.FirstDayOfMonth(dateTimeCurrent);
            var expectedEndDate = dateTimeCurrent;
            //--------------Assert PreConditions----------------
            //---------------Execute Test ----------------------
            DateRange dateRange = optionsConverter.ConvertDateRange(DateRangeOptions.MonthToDate);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartDate, dateRange.StartDate);
            Assert.AreEqual(expectedEndDate, dateRange.EndDate);
        }

        [Test]
        public void TestWeekToDate()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = DateTime.Now;
            var dateTimeNowFake = new DateTimeNowFixed(dateTimeCurrent);
            var optionsConverter = new DateRangeOptionsConverter(dateTimeNowFake);
            var expectedStartDate = DateTimeUtilities.WeekStart(dateTimeCurrent);
            var expectedEndDate = dateTimeCurrent;
            //--------------Assert PreConditions----------------
            //---------------Execute Test ----------------------
            DateRange dateRange = optionsConverter.ConvertDateRange(DateRangeOptions.WeekToDate);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedStartDate, dateRange.StartDate);
            Assert.AreEqual(expectedEndDate, dateRange.EndDate);
        }

        [Test, TestCaseSource(typeof(DateRangeConvertTestCaseSource), "DateRangeTestDataWithNoOffsets")]
        public void Test_DateRangeWithoutOffSets(DateRangeTestCase dateRangeTestCase)
        {
            //---------------Set up test pack-------------------
            var dateTimeNowFake = new DateTimeNowFixed(dateRangeTestCase.CurrectDate);
            var optionsConverter = new DateRangeOptionsConverter(dateTimeNowFake);
            //--------------Assert PreConditions----------------
            //---------------Execute Test ----------------------
            DateRange dateRange = optionsConverter.ConvertDateRange(dateRangeTestCase.DateRangeOptions);
            //---------------Test Result -----------------------
            dateRangeTestCase.ShouldHaveStartAndEndDate(dateRange);
        }

        [Test, TestCaseSource(typeof(DateRangeConvertTestCaseSource), "DateRangeTestDataWithOffSets")]
        public void Test_DateRangeWithOffSets(DateRangeTestCaseWithOffSet dateRangeTestCase)
        {
            //---------------Set up test pack-------------------
            var dateTimeNowFake = new DateTimeNowFixed(dateRangeTestCase.CurrectDate);
            var optionsConverter = new DateRangeOptionsConverter(dateTimeNowFake);
            optionsConverter.MidnightOffset = dateRangeTestCase.MidNightOffset;
            optionsConverter.MonthStartOffset = dateRangeTestCase.MonthOffSet;
            optionsConverter.WeekStartOffset = dateRangeTestCase.WeekOffSet;
            optionsConverter.YearStartOffset = dateRangeTestCase.YearOffSet;
            //--------------Assert PreConditions----------------
            //---------------Execute Test ----------------------
            DateRange dateRange = optionsConverter.ConvertDateRange(dateRangeTestCase.DateRangeOptions);
            //---------------Test Result -----------------------
            dateRangeTestCase.ShouldHaveStartAndEndDate(dateRange);
        }



        private static DateTime GetDateTimeCurrent(int hour)
        {
            return GetDateTimeCurrent(hour, 12, 33, 0);
        }

        private static DateTime GetDateTimeCurrent(int hour, int minutes, int seconds)
        {
            return GetDateTimeCurrent(hour, minutes, seconds, 0);
        }

        private static DateTime GetDateTimeCurrent(int hour, int minutes, int seconds, int millisecond)
        {
            const int year = 2007;
            const int month = 11;
            const int day = 13;
            return new DateTime(year, month, day, hour, minutes, seconds, millisecond);
        }


    }
    // ReSharper disable UnusedMember.Global
    public class DateRangeConvertTestCaseSource
    {


        // Ussed by the TestCaseSource
        public IEnumerable<TestCaseData> DateRangeTestDataWithNoOffsets
        {
            get
            {
                yield return new TestCaseData(new DateRangeTestCase(DateRangeOptions.PreviousMonth, "28 MAY 2010", "01 April 2010", "30 April 2010 23:59:59.999")).SetDescription("Middle Of the Month");

                var dateRangeOptions = DateRangeOptions.PreviousMonth;
                yield return new TestCaseData(new DateRangeTestCase(dateRangeOptions
                        , "31 May 2010 23:59:59.999"
                        , "01 Apr 2010"
                        , "30 Apr 2010 23:59:59.999")).SetDescription("End EdgeCase");
                //---------------------------
                dateRangeOptions = DateRangeOptions.Previous30Days;
                var currentDateTime = DateTime.Now;
                var expectedStartDateTime = DateTimeUtilities.DayStart(currentDateTime.AddDays(-30));
                var expectedEndDateTime = currentDateTime;
                yield return new TestCaseData(new DateRangeTestCase(dateRangeOptions, currentDateTime, expectedStartDateTime, expectedEndDateTime)).SetDescription("To Now");

                //Previous31 Days
                dateRangeOptions = DateRangeOptions.Previous31Days;
                expectedStartDateTime = DateTimeUtilities.DayStart(currentDateTime.AddDays(-31));
                yield return new TestCaseData(new DateRangeTestCase(dateRangeOptions, currentDateTime, expectedStartDateTime, expectedEndDateTime)).SetDescription("To Now");
                 //-----------------
                 dateRangeOptions = DateRangeOptions.YearToDate;
                 expectedStartDateTime = DateTimeUtilities.YearStart(currentDateTime);
                 yield return new TestCaseData(new DateRangeTestCase(dateRangeOptions, currentDateTime, expectedStartDateTime, expectedEndDateTime)).SetDescription("To Now");

                 //-----------------
                 dateRangeOptions = DateRangeOptions.Previous365Days;
                 // Changed to AddDays(-365) from AddYears(-1) as AddYears(-1) fails for leap years
                 expectedStartDateTime = DateTimeUtilities.DayStart(currentDateTime.AddDays(-365));
                 yield return new TestCaseData(new DateRangeTestCase(dateRangeOptions, currentDateTime, expectedStartDateTime, expectedEndDateTime));
                 //-----------------
                 dateRangeOptions = DateRangeOptions.Current2Years;
                 expectedStartDateTime = currentDateTime.AddYears(-2);
                 yield return new TestCaseData(new DateRangeTestCase(dateRangeOptions, currentDateTime, expectedStartDateTime, expectedEndDateTime));
                 //-----------------
                 dateRangeOptions = DateRangeOptions.Current3Years;
                 expectedStartDateTime = currentDateTime.AddYears(-3);
                 yield return new TestCaseData(new DateRangeTestCase(dateRangeOptions, currentDateTime, expectedStartDateTime, expectedEndDateTime));
                 //-----------------
                 dateRangeOptions = DateRangeOptions.Current5Years;
                 expectedStartDateTime = currentDateTime.AddYears(-5);
                 yield return new TestCaseData(new DateRangeTestCase(dateRangeOptions, currentDateTime, expectedStartDateTime, expectedEndDateTime));
                 //-----------------
                 dateRangeOptions = DateRangeOptions.ThisYear;
                 expectedStartDateTime = DateTimeUtilities.YearStart(currentDateTime);
                 expectedEndDateTime = DateTimeUtilities.YearEnd(currentDateTime);
                 yield return new TestCaseData(new DateRangeTestCase(dateRangeOptions, currentDateTime, expectedStartDateTime, expectedEndDateTime));

                 //-----------------
                 dateRangeOptions = DateRangeOptions.PreviousYear;
                 int noPreviouYears = 1;
                 expectedStartDateTime = DateTimeUtilities.YearStart(currentDateTime.AddYears(-noPreviouYears));
                 expectedEndDateTime = DateTimeUtilities.YearEnd(currentDateTime.AddYears(-noPreviouYears));
                 yield return new TestCaseData(new DateRangeTestCase(dateRangeOptions, currentDateTime, expectedStartDateTime, expectedEndDateTime));

                 //-----------------
                 dateRangeOptions = DateRangeOptions.Previous2Years;
                 noPreviouYears = 2;
                 expectedStartDateTime = DateTimeUtilities.YearStart(currentDateTime.AddYears(-noPreviouYears));
                 expectedEndDateTime = DateTimeUtilities.YearEnd(currentDateTime.AddYears(-noPreviouYears).AddYears(1));
                 yield return new TestCaseData(new DateRangeTestCase(dateRangeOptions, currentDateTime, expectedStartDateTime, expectedEndDateTime));
                 //-----------------
                 dateRangeOptions = DateRangeOptions.Previous2Years;
                 expectedStartDateTime = new DateTime(2005, 1, 1, 0, 0, 0, 0);
                 expectedEndDateTime = new DateTime(2006, 12, 31, 23, 59, 59, 999);
                 yield return new TestCaseData(new DateRangeTestCase(dateRangeOptions
                         , new DateTime(2007, 11, 13, 20, 38, 12, 10)
                         , expectedStartDateTime, expectedEndDateTime));



                 //-----------------
                 dateRangeOptions = DateRangeOptions.Previous3Years;
                 noPreviouYears = 3;
                 expectedStartDateTime = DateTimeUtilities.YearStart(currentDateTime.AddYears(-noPreviouYears));
                 expectedEndDateTime = DateTimeUtilities.YearEnd(currentDateTime.AddYears(-noPreviouYears)).AddYears(2);
                 yield return new TestCaseData(new DateRangeTestCase(dateRangeOptions, currentDateTime, expectedStartDateTime, expectedEndDateTime));
                 //-----------------
                 dateRangeOptions = DateRangeOptions.Previous5Years;
                 noPreviouYears = 5;
                 expectedStartDateTime = DateTimeUtilities.YearStart(currentDateTime.AddYears(-noPreviouYears));
                 expectedEndDateTime = DateTimeUtilities.YearEnd(currentDateTime.AddYears(-noPreviouYears)).AddYears(4);
                 yield return new TestCaseData(new DateRangeTestCase(dateRangeOptions, currentDateTime, expectedStartDateTime, expectedEndDateTime));

                 //-----------------
                dateRangeOptions = DateRangeOptions.Tommorrow;
                var testDescription = "Tommorrow";
                yield return new TestCaseData(new DateRangeTestCase(dateRangeOptions
                         , "02 June 2010 1:1:4"
                         , "03 June 2010"
                         , "03 June 2010 23:59:59.999")).SetDescription(testDescription);
                /**/

            }
        }
        // Ussed by the TestCaseSource
        public IEnumerable<TestCaseData> DateRangeTestDataWithOffSets
        {
            get
            {
                var testDescription = "Date Is after offset date";
                yield return new TestCaseData(new DateRangeTestCaseWithOffSet(DateRangeOptions.Today
                        , "28 MAY 2010 5:1:4", "28 May 2010 3:1:2", "29 May 2010 3:1:1.999"
                        , new TimeSpan(3,1,2), 0, 0, 0)).SetDescription(testDescription);
                testDescription = "Date Is B4 offset date";
                
                yield return new TestCaseData(new DateRangeTestCaseWithOffSet(DateRangeOptions.Today
                        , "28 MAY 2010 1:1:4", "27 May 2010 3:1:2", "28 May 2010 3:1:1.999"
                        , new TimeSpan(3, 1, 2), 0, 0, 0)).SetDescription(testDescription);
                //-----------------
                var dateRangeOptions = DateRangeOptions.Yesterday;
                testDescription = "Date Is B4 offset date";
                yield return new TestCaseData(new DateRangeTestCaseWithOffSet(dateRangeOptions
                        , "28 MAY 2010 1:1:4", "26 May 2010 3:1:2", "27 May 2010 3:1:1.999"
                        , new TimeSpan(3, 1, 2), 0, 0, 0)).SetDescription(testDescription);
                //-----------------
                dateRangeOptions = DateRangeOptions.ThisWeek;
                testDescription = "Date Is B4 offset date";
                yield return new TestCaseData(new DateRangeTestCaseWithOffSet(dateRangeOptions
                        , "24 MAY 2010 1:1:4", "17 May 2010 3:1:2", "24 May 2010 3:1:1.999"
                        , new TimeSpan(3, 1, 2), 1, 0, 0)).SetDescription(testDescription);
                //-----------------
                dateRangeOptions = DateRangeOptions.MonthToDate;
                testDescription = "Date Is B4 offset date";
                yield return new TestCaseData(new DateRangeTestCaseWithOffSet(dateRangeOptions
                        , "24 MAY 2010 1:1:4", "03 May 2010 3:1:2", "24 MAY 2010 1:1:4"
                        , new TimeSpan(3, 1, 2), 0, 2, 0)).SetDescription(testDescription);
                //-----------------
                dateRangeOptions = DateRangeOptions.PreviousMonth;
                testDescription = "Date Is B4 offset date";
                yield return new TestCaseData(new DateRangeTestCaseWithOffSet(dateRangeOptions
                        , "02 June 2010 1:1:4", "03 April 2010 3:1:2", "03 May 2010 3:1:1.999"
                        , new TimeSpan(3, 1, 2), 0, 2, 0)).SetDescription(testDescription);
                 //-----------------
                 dateRangeOptions = DateRangeOptions.ThisMonth;
                 testDescription = "Date Is B4 offset date";
                 yield return new TestCaseData(new DateRangeTestCaseWithOffSet(dateRangeOptions
                         , "02 June 2010 1:1:4", "03 May 2010 3:1:2", "03 June 2010 3:1:1.999"
                         , new TimeSpan(3, 1, 2), 0, 2, 0)).SetDescription(testDescription);
                 //-----------------
                 dateRangeOptions = DateRangeOptions.ThisYear;
                 testDescription = "Date Is B4 offset date";
                 yield return new TestCaseData(new DateRangeTestCaseWithOffSet(dateRangeOptions
                         , "02 Jan 2010 1:1:4", "03 Apr 2009 3:1:2", "03 Apr 2010 3:1:1.999"
                         , new TimeSpan(3, 1, 2), 0, 2, 3)).SetDescription(testDescription);
                 //-----------------
                 dateRangeOptions = DateRangeOptions.PreviousMonth;
                 testDescription = "Date Is B4 offset date";
                 yield return new TestCaseData(new DateRangeTestCaseWithOffSet(dateRangeOptions
                         , new DateTime(2007, 10, 31, 23, 30, 0, 0)
                         , new DateTime(2007, 9, 30, 23, 0, 0, 0)
                         , new DateTime(2007, 10, 31, 22, 59, 59, 999)
                         , new TimeSpan(0, -1, 0, 0, 0), 0, 0, 0)).SetDescription(testDescription);
                 //-----------------
                 dateRangeOptions = DateRangeOptions.PreviousYear;
                 testDescription = "Date Is B4 offset date";
                 yield return new TestCaseData(new DateRangeTestCaseWithOffSet(dateRangeOptions
                         , new DateTime(2007, 1, 1, 0, 30, 0, 0)
                         , new DateTime(2005, 1, 1, 1, 0, 0, 0)
                         , new DateTime(2006, 1, 1, 0, 59, 59, 999)
                         , new TimeSpan(0, 1, 0, 0, 0), 0, 0, 0)).SetDescription(testDescription);

                 //-----------------
                 dateRangeOptions = DateRangeOptions.PreviousYear;
                 testDescription = "Current Date After Negative OffSet Date";
                 yield return new TestCaseData(new DateRangeTestCaseWithOffSet(dateRangeOptions
                         , new DateTime(2006, 12, 31, 23, 30, 0, 0)
                         , new DateTime(2005, 12, 31, 23, 0, 0, 0)
                         , new DateTime(2006, 12, 31, 22, 59, 59, 999)
                         , new TimeSpan(0, -1, 0, 0, 0), 0, 0, 0)).SetDescription(testDescription);

                 //-----------------
                 dateRangeOptions = DateRangeOptions.Tommorrow;
                 testDescription = "Tommorrow";
                 yield return new TestCaseData(new DateRangeTestCaseWithOffSet(dateRangeOptions
                         , "02 June 2010 1:1:4"
                         , "02 June 2010 6:0:0"
                         , "03 June 2010 5:59:59.999"
                         , new TimeSpan(0, 6, 0, 0, 0), 0, 0, 0)).SetDescription(testDescription);
                 //-----------------
                 dateRangeOptions = DateRangeOptions.Next7Days;
                 testDescription = "Next7Days";
                 yield return new TestCaseData(new DateRangeTestCaseWithOffSet(dateRangeOptions
                         , "02 June 2010 1:1:4.011"
                         , "02 June 2010 1:1:4.011"
                         , "09 June 2010 5:59:59.999"
                         , new TimeSpan(0, 6, 0, 0, 0), 0, 0, 0)).SetDescription(testDescription);
                 //-----------------
                 dateRangeOptions = DateRangeOptions.Next30Days;
                 testDescription = "Next30Days";
                 yield return new TestCaseData(new DateRangeTestCaseWithOffSet(dateRangeOptions
                         , "02 June 2010 1:1:4.011"
                         , "02 June 2010 1:1:4.011"
                         , "02 July 2010 5:59:59.999"
                         , new TimeSpan(0, 6, 0, 0, 0), 0, 0, 0)).SetDescription(testDescription);
                /*

            _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2006, 12, 31, 23, 30, 0, 0);
            Assert.AreEqual(new DateTime(2005, 12, 31, 23, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2006, 12, 31, 22, 59, 59, 999), _comboBox.EndDate);*/
            }

        }
    }

    public class DateRangeTestCase
    {
        public DateRangeOptions DateRangeOptions { get; private set; }
        public DateTime CurrectDate { get; private set; }
        public DateTime ExpectedStartDate { get; private set; }
        public DateTime ExpectedEndDate { get; private set; }

        public DateRangeTestCase(DateRangeOptions dateRange, string currentDateStr, string expectedStartDateStr, string expectedEndDateStr)
        {
            DateRangeOptions = dateRange;
            CurrectDate = DateTimeUtilities.ParseToDate(currentDateStr);
            ExpectedStartDate = DateTimeUtilities.ParseToDate(expectedStartDateStr);
            ExpectedEndDate = DateTimeUtilities.ParseToDate(expectedEndDateStr);
        }
        public DateRangeTestCase(DateRangeOptions dateRange, DateTime currentDate, DateTime expectedStartDate, DateTime expectedEndDate)
        {
            DateRangeOptions = dateRange;
            CurrectDate = currentDate;
            ExpectedStartDate = expectedStartDate;
            ExpectedEndDate = expectedEndDate;
        }

        public void ShouldHaveStartAndEndDate(DateRange range)
        {
            ShouldEqualExpectedStartDate(range);
            ShouldEqualExpectedEndDate(range);
        }

        public virtual void ShouldEqualExpectedStartDate(DateTime actualStartDate)
        {
            var message = "The Start Date for Range '" + this.DateRangeOptions + "'" 
                      + Environment.NewLine + " with Current Date '" + CurrentDateString + "'" 
                      + Environment.NewLine + " Should be '" + ExpectedStartDateString + "'"
                      + Environment.NewLine + " But was '" + ToString(actualStartDate) + "'";
            Assert.AreEqual(ExpectedStartDate, actualStartDate, message );
        }

        protected string ExpectedStartDateString
        {
            get { return ToString(this.ExpectedStartDate); }
        }

        protected string CurrentDateString
        {
            get { return ToString(this.CurrectDate); }
        }

        protected static string ToString(DateTime date)
        {
            return date.ToString("dd MMM yyyy HH:mm:ss.fff");
        }

        public void ShouldEqualExpectedStartDate(DateRange dateRange)
        {
            ShouldEqualExpectedStartDate(dateRange.StartDate);

        }
        public virtual void ShouldEqualExpectedEndDate(DateTime actualEndDate)
        {
            Assert.AreEqual(ExpectedEndDate, actualEndDate, "The End Date for Range '"
                    + this.DateRangeOptions + "'" + Environment.NewLine
                    + " with Current Date '" + CurrentDateString + "'" + Environment.NewLine
                    + " Should be '" + ExpectedEndDateString + "'" + Environment.NewLine
                    + " But was '" + ToString(actualEndDate) + "'");
        }

        protected string ExpectedEndDateString
        {
            get { return ToString(this.ExpectedEndDate); }
        }

        public void ShouldEqualExpectedEndDate(DateRange dateRange)
        {
            ShouldEqualExpectedEndDate(dateRange.EndDate);
        }
    }

    public class DateRangeTestCaseWithOffSet : DateRangeTestCase
    {
        public int WeekOffSet { get; private set; }
        public int MonthOffSet { get; private set; }
        public int YearOffSet { get; private set; }
        public TimeSpan MidNightOffset { get; private set; }

        public DateRangeTestCaseWithOffSet(DateRangeOptions dateRange, string currentDateStr, string expectedStartDateStr, string expectedEndDateStr, TimeSpan midNightOffset, int weekOffSet, int monthOffSet, int yearOffSet)
            : base(dateRange, currentDateStr, expectedStartDateStr, expectedEndDateStr)
        {
            WeekOffSet = weekOffSet;
            MonthOffSet = monthOffSet;
            YearOffSet = yearOffSet;
            MidNightOffset = midNightOffset;
        }

        public DateRangeTestCaseWithOffSet(DateRangeOptions dateRange, DateTime currentDate, DateTime expectedStartDate, DateTime expectedEndDate, TimeSpan midNightOffset, int weekOffSet, int monthOffSet, int yearOffSet)
            : base(dateRange, currentDate, expectedStartDate, expectedEndDate)
        {
            MidNightOffset = midNightOffset;
            WeekOffSet = weekOffSet;
            MonthOffSet = monthOffSet;
            YearOffSet = yearOffSet;
        }
        public override void ShouldEqualExpectedEndDate(DateTime actualEndDate)
        {
            Assert.AreEqual(ExpectedEndDate, actualEndDate, "The End Date for Range '"
                    + this.DateRangeOptions + "'" + Environment.NewLine
                    + " with Current Date '" + CurrentDateString + "'" + Environment.NewLine
                    + " Should be '" + ExpectedEndDateString + "'" + Environment.NewLine
                    + " But was '" + ToString(actualEndDate) + "'"+ Environment.NewLine 
                    + OffSetMessage);
        }
        public override void ShouldEqualExpectedStartDate(DateTime actualStartDate)
        {
            var message = "The Start Date for Range '" + this.DateRangeOptions + "'" + Environment.NewLine
                    + "With Current Date '" + CurrentDateString + "'" + Environment.NewLine
                    + "Should be '" + ExpectedStartDateString + "'" + Environment.NewLine 
                    + "But was '"+ ToString(actualStartDate) + "'" + Environment.NewLine 
                    + OffSetMessage;
            Assert.AreEqual(ExpectedStartDate, actualStartDate, message);
        }
        private string OffSetMessage
        {
            get
            {
                return "OffSets :-" + Environment.NewLine
                       + "MidnightOffSet = " + this.MidNightOffset + Environment.NewLine
                       + "MonthOffSet = " + this.MonthOffSet + Environment.NewLine
                       + "YearOffSet = " + this.YearOffSet + Environment.NewLine
                       + "WeekOffSet = " + this.WeekOffSet + Environment.NewLine;
            }
        }
    }

}
