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
    public class TestDateTimeOptionsConverter
    {
        [Test]
        public void Test_ThisHour_ShouldSetStartDateToBeginHour_AndEndDateToEndOfHour()
        {
            //---------------Set up test pack-------------------
            var dateTimeCurrent = GetDateTimeCurrent(20);
            var dateTimeNowFake = new DateTimeNowFake(dateTimeCurrent);
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
            var dateTimeNowFake = new DateTimeNowFake(dateTimeCurrent);
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
            var dateTimeNowFake = new DateTimeNowFake(dateTimeCurrent);
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
            var dateTimeNowFake = new DateTimeNowFake(dateTimeCurrent);
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
            var dateTimeNowFake = new DateTimeNowFake(dateTimeCurrent);
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
            var optionsConverter = new DateRangeOptionsConverter(new DateTimeNowFake(dateTimeCurrent));
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
            var optionsConverter = new DateRangeOptionsConverter(new DateTimeNowFake(dateTimeCurrent));
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
            var dateTimeNowFake = new DateTimeNowFake(dateTimeCurrent);
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

        /*



        [Test]
        public void TestToday_WithMidnightOffset_Positive()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Today);

            //---------------Test Result -----------------------
            _comboBox.MidnightOffset = new TimeSpan(0, 6, 0, 0, 10);
            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 7, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 11, 13, 6, 0, 0, 10), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.MidnightOffset = new TimeSpan(0, 6, 0, 0, 10);
            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 11, 12, 6, 0, 0, 10), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.MidnightOffset = new TimeSpan(0, 6, 0, 0, 10);
            _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 3, 0, 0, 0);
            Assert.AreEqual(new DateTime(2006, 12, 31, 6, 0, 0, 10), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
        }
                [Test]
                public void TestToday_WithMidnightOffset_Negative()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Today);

                    //---------------Test Result -----------------------
                    _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 22, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 23, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 22, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 22, 0, 0, 10);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 22, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 21, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 11, 22, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

        

                [Test]
                public void TestYesterday_UsingMidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Yesterday);

                    //---------------Test Result -----------------------
                    _comboBox.MidnightOffset = new TimeSpan(0, 6, 0, 0, 10);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 11, 6, 0, 0, 10), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 6, 0, 0, 10), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 11, 22, 0, 0, 00), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 22, 0, 0, 00), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 23, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 22, 0, 0, 00), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 13, 22, 0, 0, 00), _comboBox.EndDate);
                }



                [Test]
                public void TestThisWeek_UsingWeekStartOffset_Negative()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.WeekToDate);

                    //---------------Test Result -----------------------
                    _comboBox.WeekStartOffset = -1;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 11, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.WeekStartOffset = -1;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 11, 12, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 11, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestThisWeek_UsingWeekStartOffset_Positive()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.WeekToDate);

                    //---------------Test Result -----------------------
                    _comboBox.WeekStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 12, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.WeekStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 6, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestThisWeek_UsingMidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.WeekToDate);

                    //---------------Test Result -----------------------
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 5, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 11, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestThisWeek_UsingWeekStartOffset_Positive_AndMidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.WeekToDate);

                    //---------------Test Result -----------------------
                    _comboBox.WeekStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 23, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 22, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.WeekStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 21, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 5, 22, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestThisWeek_UsingWeekStartOffset_Negative_AndMidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.WeekToDate);

                    //---------------Test Result -----------------------
                    _comboBox.WeekStartOffset = -1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 11, 23, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 10, 22, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.WeekStartOffset = -1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 2, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 11, 23, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 11, 2, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.WeekStartOffset = -1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 2, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 1, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 11, 2, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.WeekStartOffset = -1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 2, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 11, 3, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 11, 2, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.WeekStartOffset = -1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 2, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 11, 1, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 4, 2, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestLastWeek()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousWeek);

                    //---------------Test Result -----------------------
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2007, 11, 5, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 0, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 5, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 0, 0, 0, 0), _comboBox.EndDate);
                }


                [Test]
                public void TestLastWeek_WeekOffSet_Negative()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousWeek);

                    //---------------Test Result -----------------------
                    _comboBox.WeekStartOffset = -1;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2007, 11, 4, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 11, 0, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.WeekStartOffset = -1;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 11, 12, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 4, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 11, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestLastWeek_WeekOffSet_Positive()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousWeek);

                    //---------------Test Result -----------------------
                    _comboBox.WeekStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2007, 10, 30, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 6, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestLastWeek_WeekOffSet_Positive_MidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousWeek);

                    //---------------Test Result -----------------------
                    _comboBox.WeekStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 30, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 6, 1, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestLastWeek_WeekOffSet_Negative_MidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousWeek);

                    //---------------Test Result -----------------------
                    _comboBox.WeekStartOffset = -1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 4, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 11, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.WeekStartOffset = -1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 11, 23, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 3, 22, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 10, 22, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.WeekStartOffset = -1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 10, 23, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 3, 22, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 10, 22, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.WeekStartOffset = -1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 10, 21, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 27, 22, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 3, 22, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestLastWeek_MidnightOffSet()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousWeek);

                    //---------------Test Result -----------------------
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2007, 11, 5, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 11, 12, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 28, 22, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 4, 22, 0, 0, 0), _comboBox.EndDate);
                }


                [Test]
                public void TestPrevious7Days()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous7Days);
                    //---------------Test Result -----------------------

                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2007, 11, 6, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 6, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestPrevious7Days_WithMidnightOffSet()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous7Days);
                    //---------------Test Result -----------------------

                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 5, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 5, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 23, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 23, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 5, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 23, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 22, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 4, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 11, 23, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestMonthToDate()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.MonthToDate);

                    //---------------Test Result -----------------------
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2007, 11, 1, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 0, 0, 0, 0);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestMonthToDate_WithMidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.MonthToDate);

                    //---------------Test Result -----------------------
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 31, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 10, 31, 23, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 31, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 1, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestMonthToDate_WithMonthStartOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.MonthToDate);

                    //---------------Test Result -----------------------
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 3, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 2, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.MonthStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 2, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestMonthToDate_WithMonthStartOffset_MidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.MonthToDate);

                    //---------------Test Result -----------------------
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 3, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 2, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 3, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 11, 1, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 10, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 1, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestLastMonth()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousMonth);

                    //---------------Test Result -----------------------
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2007, 10, 1, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 1, 0, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 1, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 1, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestLastMonth_WithMidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousMonth);

                    //---------------Test Result -----------------------
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 1, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 1, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 0, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 9, 1, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 10, 1, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 9, 30, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 10, 31, 23, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 10, 31, 23, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 9, 30, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 10, 31, 23, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestLastMonth_WithMonthStartOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousMonth);

                    //---------------Test Result -----------------------
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2007, 10, 2, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 2, 0, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MonthStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 2, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 2, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 2, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestLastMonth_WithMonthStartOffset_MidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousMonth);

                    //---------------Test Result -----------------------
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 2, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 2, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 2, 0, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 9, 2, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 10, 2, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 1, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 1, 23, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 23, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 1, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 1, 23, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestPrevious30Days()
                {
                    //---------------Set up test pack-------------------
                    _comboBox.AddDateOption(DateRangeOptions.Previous30Days);

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous30Days);
                    //---------------Test Result -----------------------
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2007, 10, 14, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 14, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestPrevious30Days_WithMidnightOffset()
                {
                    //---------------Set up test pack-------------------
                    _comboBox.AddDateOption(DateRangeOptions.Previous30Days);

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous30Days);
                    //---------------Test Result -----------------------
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 13, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 13, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 23, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 23, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 13, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 23, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestPrevious31Days()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------

                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous31Days);
                    //---------------Test Result -----------------------
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2007, 10, 13, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 13, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestPrevious31Days_WithMidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------

                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous31Days);
                    //---------------Test Result -----------------------
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 12, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 12, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 23, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 23, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 10, 12, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 23, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestYearToDate()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.YearToDate);
                    //---------------Test Result -----------------------
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 0, 0, 0, 0);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestYearToDate_WithMidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.YearToDate);
                    //---------------Test Result -----------------------
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 12, 31, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2006, 12, 31, 23, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 12, 31, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 1, 1, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestYearToDate_WithMonthOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.YearToDate);
                    //---------------Test Result -----------------------
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 1, 3, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 1, 2, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.MonthStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 1, 2, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestYearToDate_WithMonthOffset_MidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.YearToDate);
                    //---------------Test Result -----------------------
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 1, 3, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 1, 2, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 1, 3, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 1, 1, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 10, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 1, 1, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestYearToDate_WithYearOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.YearToDate);
                    //---------------Test Result -----------------------
                    _comboBox.YearStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2007, 2, 1, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.YearStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 0, 0, 0, 0);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestYearToDate_YearOffset_MidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.YearToDate);
                    //---------------Test Result -----------------------
                    _comboBox.YearStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 1, 31, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.YearStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 1, 31, 23, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 1, 31, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.YearStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 2, 1, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestYearToDate_WithYearOffset_MonthOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.YearToDate);
                    //---------------Test Result -----------------------
                    _comboBox.YearStartOffset = 1;
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 2, 3, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 2, 2, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.YearStartOffset = 1;
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 2, 2, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestYearToDate_YearOffsetMonthOffset_MidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.YearToDate);
                    //---------------Test Result -----------------------
                    _comboBox.YearStartOffset = 1;
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 2, 3, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 2, 2, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.YearStartOffset = 1;
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 2, 3, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2007, 2, 1, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

                    _comboBox.YearStartOffset = 1;
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 10, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 2, 1, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }


                [Test]
                public void TestThisYear()
                {
                    //---------------Set up test pack-------------------
                    _comboBox.AddDateOption(DateRangeOptions.ThisYear);
                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.ThisYear);
                    //---------------Test Result -----------------------
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2008, 1, 1, 0, 0, 0, 0), _comboBox.EndDate);
                }


                [Test]
                public void TestLastYear()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousYear);
                    //---------------Test Result -----------------------
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2006, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestLastYear_WithMidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousYear);
                    //---------------Test Result -----------------------
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 1, 2, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 1, 1, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 1, 1, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 0, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2005, 1, 1, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2006, 1, 1, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 1, 3, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2005, 12, 31, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2006, 12, 31, 23, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2006, 12, 31, 23, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2005, 12, 31, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2006, 12, 31, 23, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestLastYear_WithMonthOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousYear);
                    //---------------Test Result -----------------------
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2006, 1, 2, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 1, 2, 0, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MonthStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 1, 2, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 1, 2, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 1, 2, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestLastYear_WithMonthOffset_MidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousYear);
                    //---------------Test Result -----------------------
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 1, 3, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 1, 2, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 1, 2, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 1, 2, 0, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2005, 1, 2, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2006, 1, 2, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 1, 3, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 1, 1, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 1, 1, 23, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 23, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 1, 1, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 1, 1, 23, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestLastYear_YearOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousYear);
                    //---------------Test Result -----------------------
                    _comboBox.YearStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2006, 2, 1, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 2, 1, 0, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.YearStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 2, 1, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 2, 1, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestLastYear_YearOffset_MidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousYear);
                    //---------------Test Result -----------------------
                    _comboBox.YearStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 2, 2, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 2, 1, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 2, 1, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.YearStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 0, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2005, 2, 1, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2006, 2, 1, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.YearStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 2, 3, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 1, 31, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 1, 31, 23, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.YearStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 1, 31, 23, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 1, 31, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 1, 31, 23, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestLastYear_WithYearOffset_MonthOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousYear);
                    //---------------Test Result -----------------------
                    _comboBox.YearStartOffset = 1;
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2006, 2, 2, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 2, 2, 0, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.YearStartOffset = 1;
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.FixedNowDate = new DateTime(2007, 2, 2, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 2, 2, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 2, 2, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestLastYear_WithYearOffset_MonthOffset_MidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousYear);
                    //---------------Test Result -----------------------
                    _comboBox.YearStartOffset = 1;
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 2, 3, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 2, 2, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 2, 2, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.YearStartOffset = 1;
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 2, 2, 0, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2005, 2, 2, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2006, 2, 2, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.YearStartOffset = 1;
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 2, 3, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 2, 1, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 2, 1, 23, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.YearStartOffset = 1;
                    _comboBox.MonthStartOffset = 1;
                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 23, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 2, 1, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 2, 1, 23, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestPrevious365Days()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------

                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous365Days);
                    //---------------Test Result -----------------------
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2006, 11, 13, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 11, 13, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestPrevious365Days_WithMidnightOffset()
                {
                    //---------------Set up test pack-------------------

                    //---------------Execute Test ----------------------

                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous365Days);

                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 11, 12, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 1, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 11, 12, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 23, 0, 0, 0), _comboBox.EndDate);

                    _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 23, 30, 0, 0);
                    Assert.AreEqual(new DateTime(2006, 11, 12, 23, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 12, 23, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestCurrent2Years()
                {
                    //---------------Set up test pack-------------------

                    _comboBox.AddDateOption(DateRangeOptions.Current2Years);
                    //---------------Execute Test ----------------------

                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Current2Years);
                    //---------------Test Result -----------------------
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2005, 11, 13, 20, 38, 12, 10), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestCurrent3Years()
                {
                    //---------------Set up test pack-------------------
                    _comboBox.AddDateOption(DateRangeOptions.Current3Years);
                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Current3Years);
                    //---------------Test Result -----------------------
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2004, 11, 13, 20, 38, 12, 10), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestCurrent5Years()
                {
                    //---------------Set up test pack-------------------
                    _comboBox.AddDateOption(DateRangeOptions.Current5Years);
                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Current5Years);
                    //---------------Test Result -----------------------
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2002, 11, 13, 20, 38, 12, 10), _comboBox.StartDate);
                    Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
                }

                [Test]
                public void TestPrevious2Years()
                {
                    //---------------Set up test pack-------------------
                    _comboBox.AddDateOption(DateRangeOptions.Previous2Years);
                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous2Years);
                    //---------------Test Result -----------------------
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2005, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestPrevious3Years()
                {
                    //---------------Set up test pack-------------------
                    _comboBox.AddDateOption(DateRangeOptions.Previous3Years);
                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous3Years);
                    //---------------Test Result -----------------------
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2004, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestPrevious5Years()
                {
                    //---------------Set up test pack-------------------
                    _comboBox.AddDateOption(DateRangeOptions.Previous5Years);
                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous5Years);
                    //---------------Test Result -----------------------
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    Assert.AreEqual(new DateTime(2002, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.EndDate);
                }


                [Test]
                public void TestRealNow()
                {
                    //---------------Set up test pack-------------------
                    _comboBox.UseFixedNowDate = false;
                    DateTime now = DateTime.Now;
                    DateTime yest = now.AddDays(-1);
                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Yesterday);

                    //---------------Test Result -----------------------
                    Assert.AreEqual(new DateTime(yest.Year, yest.Month, yest.Day, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestIgnoreTime_Today()
                {
                    //---------------Set up test pack-------------------
                    _comboBox.IgnoreTime = true;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Today);

                    //---------------Test Result -----------------------
                    Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestIgnoreTime_ThisWeek()
                {
                    //---------------Set up test pack-------------------
                    _comboBox.IgnoreTime = true;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.WeekToDate);
                    //---------------Test Result -----------------------

                    Assert.AreEqual(new DateTime(2007, 11, 12, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestIgnoreTime_ThisMonth()
                {
                    //---------------Set up test pack-------------------
                    _comboBox.IgnoreTime = true;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.MonthToDate);
                    //---------------Test Result -----------------------

                    Assert.AreEqual(new DateTime(2007, 11, 1, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);
                }

                [Test]
                public void TestIgnoreTime_ThisYear()
                {
                    //---------------Set up test pack-------------------
                    _comboBox.IgnoreTime = true;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    //---------------Execute Test ----------------------
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.YearToDate);
                    //---------------Test Result -----------------------

                    Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);
                }

                //This test works but does not make any sense because you should not be using MidnightOffset if using IgnoreTime
                [Test]
                public void TestIgnoreTime_Today_MidnightOffset()
                {
                    //---------------Set up test pack-------------------
                    _comboBox.IgnoreTime = true;
                    _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
                    //---------------Execute Test ----------------------

                    _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
                    _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Today);
                    //---------------Test Result -----------------------

                    Assert.AreEqual(new DateTime(2007, 11, 12, 1, 0, 0, 0), _comboBox.StartDate);
                    Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);
                }*/
    }

    public class DateTimeNowFake : DateTimeNow
    {
        private readonly DateTime _dateTimeCurrent;

        public DateTimeNowFake(DateTime dateTimeCurrent)
        {
            _dateTimeCurrent = dateTimeCurrent;
        }

        public override DateTime ResolveToValue()
        {
            return _dateTimeCurrent;
        }
    }
}
