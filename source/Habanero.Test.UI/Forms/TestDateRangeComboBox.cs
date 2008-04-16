//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Collections.Generic;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    [TestFixture]
    public class TestDateRangeComboBox
    {
        private DateRangeComboBox _comboBox;
        private int _numDefaultOptions;
        private int _numAllOptions;

        [SetUp]
        public void SetUpTests()
        {
            _comboBox = new DateRangeComboBox();
            _numDefaultOptions = 11;
            _numAllOptions = Enum.GetNames(typeof(DateRangeComboBox.DateOptions)).Length;

            // Use a fixed date as the reference point, rather than DateTime.Now
            _comboBox.UseFixedNowDate = true;
        }

        [Test]
        public void TestDefaultConstructor()
        {
            Assert.AreEqual(_numDefaultOptions, _comboBox.OptionsToDisplay.Count);
            Assert.IsFalse(_comboBox.IgnoreTime);
            Assert.AreEqual(TimeSpan.Zero, _comboBox.MidnightOffset);
            Assert.AreEqual(0, _comboBox.WeekStartOffset);
            Assert.AreEqual(0, _comboBox.MonthStartOffset);
            Assert.AreEqual(0, _comboBox.YearStartOffset);

            Assert.AreEqual(_comboBox.OptionsToDisplay.Count, _comboBox.Items.Count - 1);
            Assert.AreEqual("(Date Ranges)", _comboBox.Items[0]);
            Assert.AreEqual("Today", _comboBox.Items[1].ToString());
        }

        [Test]
        public void TestLoadedConstructor()
        {
            List<DateRangeComboBox.DateOptions> options = new List<DateRangeComboBox.DateOptions>();
            options.Add(DateRangeComboBox.DateOptions.Previous365Days);
            options.Add(DateRangeComboBox.DateOptions.PreviousYear);
            _comboBox = new DateRangeComboBox(options);

            Assert.AreEqual(2, _comboBox.OptionsToDisplay.Count);
            Assert.IsFalse(_comboBox.IgnoreTime);
            Assert.AreEqual(TimeSpan.Zero, _comboBox.MidnightOffset);
            Assert.AreEqual(0, _comboBox.WeekStartOffset);
            Assert.AreEqual(0, _comboBox.MonthStartOffset);
            Assert.AreEqual(0, _comboBox.YearStartOffset);

            Assert.AreEqual(3, _comboBox.Items.Count);
            Assert.AreEqual("(Date Ranges)", _comboBox.Items[0]);
            Assert.AreEqual("Previous 365 Days", _comboBox.Items[1]);
            Assert.AreEqual("Previous Year", _comboBox.Items[2]);
        }

        [Test]
        public void TestUseAllDateOptions()
        {
            Assert.AreEqual(_numDefaultOptions, _comboBox.OptionsToDisplay.Count);
            _comboBox.UseAllDateOptions();
            Assert.AreEqual(_numAllOptions, _comboBox.OptionsToDisplay.Count);
        }

        [Test]
        public void TestSetTopComboBoxItem()
        {
            Assert.AreEqual("(Date Ranges)", _comboBox.Items[0]);

            _comboBox.SetTopComboBoxItem("Express options...");
            Assert.AreEqual("Express options...", _comboBox.Items[0]);
        }

        [Test]
        public void TestGetAndSetDateRangeString()
        {
            Assert.AreEqual("This Week", _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.ThisWeek));

            _comboBox.SetDateRangeString(DateRangeComboBox.DateOptions.ThisWeek, "That Week");
            Assert.AreEqual("That Week", _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.ThisWeek));
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSetDateRangeStringException()
        {
            _comboBox.SetDateRangeString(DateRangeComboBox.DateOptions.ThisWeek, "Previous Week");
        }

        [Test]
        public void TestSpecialPropertyAssignments()
        {
            Assert.IsFalse(_comboBox.IgnoreTime);
            Assert.AreEqual(TimeSpan.Zero, _comboBox.MidnightOffset);
            Assert.AreEqual(0, _comboBox.WeekStartOffset);
            Assert.AreEqual(0, _comboBox.MonthStartOffset);
            Assert.AreEqual(0, _comboBox.YearStartOffset);

            _comboBox.IgnoreTime = true;
            _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0);
            _comboBox.WeekStartOffset = 1;
            _comboBox.MonthStartOffset = 1;
            _comboBox.YearStartOffset = 1;

            Assert.IsTrue(_comboBox.IgnoreTime);
            Assert.AreEqual(_comboBox.MidnightOffset, new TimeSpan(0, 1, 0, 0));
            Assert.AreEqual(_comboBox.WeekStartOffset, 1);
            Assert.AreEqual(_comboBox.MonthStartOffset, 1);
            Assert.AreEqual(_comboBox.YearStartOffset, 1);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestMidnightOffsetExceptionDays()
        {
            _comboBox.MidnightOffset = new TimeSpan(-1, 0, 0, 0);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestMidnightOffsetExceptionHours()
        {
            _comboBox.MidnightOffset = new TimeSpan(0, -25, 0, 0);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestMonthStartOffsetException()
        {
            _comboBox.MonthStartOffset = -1;
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestYearStartOffsetException()
        {
            _comboBox.YearStartOffset = -1;
        }

        [Test]
        public void TestSetOptionsToDisplay()
        {
            List<DateRangeComboBox.DateOptions> options = new List<DateRangeComboBox.DateOptions>();
            options.Add(DateRangeComboBox.DateOptions.Previous365Days);
            options.Add(DateRangeComboBox.DateOptions.PreviousYear);
            _comboBox.OptionsToDisplay = options;

            Assert.AreEqual(2, _comboBox.OptionsToDisplay.Count);
            Assert.AreEqual(3, _comboBox.Items.Count);
            Assert.AreEqual("(Date Ranges)", _comboBox.Items[0]);
            Assert.AreEqual("Previous 365 Days", _comboBox.Items[1]);
            Assert.AreEqual("Previous Year", _comboBox.Items[2]);
        }

        [Test]
        public void TestAddAndRemoveOptions()
        {
            Assert.AreEqual(_numDefaultOptions, _comboBox.OptionsToDisplay.Count);
            Assert.AreEqual(_numDefaultOptions + 1, _comboBox.Items.Count);

            _comboBox.RemoveDateOption(DateRangeComboBox.DateOptions.Today);
            Assert.AreEqual(_numDefaultOptions - 1, _comboBox.OptionsToDisplay.Count);
            Assert.AreEqual(_numDefaultOptions, _comboBox.Items.Count);
            Assert.AreEqual(-1, _comboBox.Items.IndexOf("Today"));

            // This should not cause an exception
            _comboBox.RemoveDateOption(DateRangeComboBox.DateOptions.Today);

            _comboBox.AddDateOption(DateRangeComboBox.DateOptions.Today);
            Assert.AreEqual(_numDefaultOptions, _comboBox.OptionsToDisplay.Count);
            Assert.AreEqual(_numDefaultOptions + 1, _comboBox.Items.Count);
            Assert.AreEqual(_numDefaultOptions, _comboBox.Items.IndexOf("Today"));

            // This should not cause an exception
            _comboBox.AddDateOption(DateRangeComboBox.DateOptions.Today);
        }

        [Test]
        public void TestFixedNowAssignment()
        {
            Assert.IsTrue(_comboBox.UseFixedNowDate);
            
            Assert.AreEqual(DateTime.Now.Year, _comboBox.FixedNowDate.Year);
            _comboBox.FixedNowDate = new DateTime(2000, 2, 2);
            Assert.AreEqual(new DateTime(2000, 2, 2), _comboBox.FixedNowDate);

            // Setting to false shouldn't affect FixedNowDate property
            _comboBox.UseFixedNowDate = false;
            Assert.IsFalse(_comboBox.UseFixedNowDate);
            Assert.AreEqual(new DateTime(2000, 2, 2), _comboBox.FixedNowDate);
        }

        [Test]
        public void TestDatesDefault()
        {
            Assert.AreEqual(DateTime.MinValue, _comboBox.StartDate);
            Assert.AreEqual(DateTime.MaxValue, _comboBox.EndDate);
        }

        // Testing guidelines for date options
        //   - first test the standard case
        //   - test the boundary cases
        //   - do a full combination of all applicable offset cases
        //       - test with offsets that go both ways, where applicable
        //       - test dates on both sides of the above cases

        [Test]
        public void TestThisHour()
        {
            _comboBox.AddDateOption(DateRangeComboBox.DateOptions.ThisHour);
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.ThisHour);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 11, 13, 20, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
        }

        [Test]
        public void TestPreviousHour()
        {
            _comboBox.AddDateOption(DateRangeComboBox.DateOptions.PreviousHour);
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.PreviousHour);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 11, 13, 19, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 13, 20, 0, 0, 0), _comboBox.EndDate);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 11, 13, 19, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 13, 20, 0, 0, 0), _comboBox.EndDate);
        }

        [Test]
        public void TestCurrent60Minutes()
        {
            _comboBox.AddDateOption(DateRangeComboBox.DateOptions.Current60Minutes);
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.Current60Minutes);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 11, 13, 19, 38, 12, 10), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
        }

        [Test]
        public void TestToday()
        {
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.Today);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

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
        public void TestYesterday()
        {
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.Yesterday);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 11, 12, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 11, 12, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

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
        public void TestCurrent24Hours()
        {
            _comboBox.AddDateOption(DateRangeComboBox.DateOptions.Current24Hours);
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.Current24Hours);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 11, 12, 20, 38, 12, 10), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
        }

        [Test]
        public void TestThisWeek()
        {
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.ThisWeek);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 11, 12, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 0, 0, 0, 0);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.WeekStartOffset = -1;
            _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 11, 11, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.WeekStartOffset = -1;
            _comboBox.FixedNowDate = new DateTime(2007, 11, 11, 12, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 11, 11, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.WeekStartOffset = 1;
            _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 11, 6, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.WeekStartOffset = 0;
            _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 11, 5, 1, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.WeekStartOffset = 0;
            _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 11, 11, 23, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

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
        }

        [Test]
        public void TestLastWeek()
        {
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.PreviousWeek);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 11, 5, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 12, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 11, 5, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 12, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.WeekStartOffset = -1;
            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 11, 4, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 11, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.WeekStartOffset = -1;
            _comboBox.FixedNowDate = new DateTime(2007, 11, 11, 12, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 11, 4, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 11, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.WeekStartOffset = 1;
            _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 10, 30, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 6, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.WeekStartOffset = 1;
            _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 10, 30, 1, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 6, 1, 0, 0, 0), _comboBox.EndDate);

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
        public void TestPrevious7Days()
        {
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.Previous7Days);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 11, 6, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 11, 6, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

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
        }

        [Test]
        public void TestThisMonth()
        {
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.ThisMonth);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 11, 1, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 0, 0, 0, 0);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

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

            _comboBox.MonthStartOffset = 1;
            _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 11, 3, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 11, 2, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.MonthStartOffset = 1;
            _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 10, 2, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

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
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.PreviousMonth);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 10, 1, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 1, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 10, 1, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 1, 0, 0, 0, 0), _comboBox.EndDate);

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

            _comboBox.MonthStartOffset = 1;
            _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 10, 2, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 2, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.MonthStartOffset = 1;
            _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 11, 2, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 10, 2, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 2, 0, 0, 0, 0), _comboBox.EndDate);

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
            _comboBox.AddDateOption(DateRangeComboBox.DateOptions.Previous30Days);
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.Previous30Days);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 10, 14, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 10, 14, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

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
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.Previous31Days);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 10, 13, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 10, 13, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

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
        public void TestThisYear()
        {
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.ThisYear);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 0, 0, 0, 0);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

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

            _comboBox.MonthStartOffset = 1;
            _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 1, 3, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 1, 2, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.MonthStartOffset = 1;
            _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2006, 1, 2, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

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

            _comboBox.YearStartOffset = 1;
            _comboBox.MonthStartOffset = 0;
            _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2007, 2, 1, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.YearStartOffset = 1;
            _comboBox.MonthStartOffset = 0;
            _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 0, 0, 0, 0);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.YearStartOffset = 1;
            _comboBox.MonthStartOffset = 0;
            _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 1, 31, 23, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.YearStartOffset = 1;
            _comboBox.MonthStartOffset = 0;
            _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 1, 31, 23, 30, 0, 0);
            Assert.AreEqual(new DateTime(2007, 1, 31, 23, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.YearStartOffset = 1;
            _comboBox.MonthStartOffset = 0;
            _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2006, 2, 1, 1, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.YearStartOffset = 1;
            _comboBox.MonthStartOffset = 1;
            _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 2, 3, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2007, 2, 2, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

            _comboBox.YearStartOffset = 1;
            _comboBox.MonthStartOffset = 1;
            _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2006, 2, 2, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

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
        public void TestLastYear()
        {
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.PreviousYear);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2006, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2006, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.EndDate);

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

            _comboBox.MonthStartOffset = 1;
            _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2006, 1, 2, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 1, 2, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.MonthStartOffset = 1;
            _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 1, 2, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2006, 1, 2, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 1, 2, 0, 0, 0, 0), _comboBox.EndDate);

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

            _comboBox.YearStartOffset = 1;
            _comboBox.MonthStartOffset = 0;
            _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2006, 2, 1, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 2, 1, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.YearStartOffset = 1;
            _comboBox.MonthStartOffset = 0;
            _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2006, 2, 1, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 2, 1, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.YearStartOffset = 1;
            _comboBox.MonthStartOffset = 0;
            _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 2, 2, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2006, 2, 1, 1, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 2, 1, 1, 0, 0, 0), _comboBox.EndDate);

            _comboBox.YearStartOffset = 1;
            _comboBox.MonthStartOffset = 0;
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
            _comboBox.MonthStartOffset = 0;
            _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 1, 31, 23, 30, 0, 0);
            Assert.AreEqual(new DateTime(2006, 1, 31, 23, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 1, 31, 23, 0, 0, 0), _comboBox.EndDate);

            _comboBox.YearStartOffset = 1;
            _comboBox.MonthStartOffset = 1;
            _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2006, 2, 2, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 2, 2, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.YearStartOffset = 1;
            _comboBox.MonthStartOffset = 1;
            _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
            _comboBox.FixedNowDate = new DateTime(2007, 2, 2, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2006, 2, 2, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 2, 2, 0, 0, 0, 0), _comboBox.EndDate);

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
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.Previous365Days);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2006, 11, 13, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
            Assert.AreEqual(new DateTime(2006, 11, 13, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

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
            _comboBox.AddDateOption(DateRangeComboBox.DateOptions.Current2Years);
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.Current2Years);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2005, 11, 13, 20, 38, 12, 10), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
        }

        [Test]
        public void TestCurrent3Years()
        {
            _comboBox.AddDateOption(DateRangeComboBox.DateOptions.Current3Years);
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.Current3Years);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2004, 11, 13, 20, 38, 12, 10), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
        }

        [Test]
        public void TestCurrent5Years()
        {
            _comboBox.AddDateOption(DateRangeComboBox.DateOptions.Current5Years);
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.Current5Years);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2002, 11, 13, 20, 38, 12, 10), _comboBox.StartDate);
            Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
        }

        [Test]
        public void TestPrevious2Years()
        {
            _comboBox.AddDateOption(DateRangeComboBox.DateOptions.Previous2Years);
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.Previous2Years);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2005, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.EndDate);
        }

        [Test]
        public void TestPrevious3Years()
        {
            _comboBox.AddDateOption(DateRangeComboBox.DateOptions.Previous3Years);
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.Previous3Years);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2004, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.EndDate);
        }

        [Test]
        public void TestPrevious5Years()
        {
            _comboBox.AddDateOption(DateRangeComboBox.DateOptions.Previous5Years);
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.Previous5Years);

            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
            Assert.AreEqual(new DateTime(2002, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.EndDate);
        }


        [Test]
        public void TestRealNow()
        {
            _comboBox.UseFixedNowDate = false;
            DateTime now = DateTime.Now;
            DateTime yest = now.AddDays(-1);

            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.Yesterday);
            Assert.AreEqual(new DateTime(yest.Year, yest.Month, yest.Day, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0), _comboBox.EndDate);
        }

        [Test]
        public void TestIgnoreTime()
        {
            _comboBox.IgnoreTime = true;
            _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);

            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.Today);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.ThisWeek);
            Assert.AreEqual(new DateTime(2007, 11, 12, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.ThisMonth);
            Assert.AreEqual(new DateTime(2007, 11, 1, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.ThisYear);
            Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

            _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
            _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeComboBox.DateOptions.Today);
            Assert.AreEqual(new DateTime(2007, 11, 12, 1, 0, 0, 0), _comboBox.StartDate);
            Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);
        }
    }
}