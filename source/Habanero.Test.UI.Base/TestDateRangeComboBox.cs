using System;
using System.Collections.Generic;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public abstract class TestDateRangeComboBox
    {
        protected abstract IControlFactory GetControlFactory();

        //[TestFixture]
        //public class TestDateRangeComboBoxWin : TestDateRangeComboBox
        //{
        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new ControlFactoryWin();
        //    }
        //}

        [TestFixture]
        public class TestDateRangeComboBoxGiz : TestDateRangeComboBox
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }
        }

        private IDateRangeComboBox _comboBox;
        private int _numDefaultOptions;
        private int _numAllOptions;

        [SetUp]
        public void SetUpTests()
        {
            _comboBox = GetControlFactory().CreateDateRangeComboBox();
            _numDefaultOptions = 11;
            _numAllOptions = Enum.GetNames(typeof (DateRangeOptions)).Length;

            // Use a fixed date as the reference point, rather than DateTime.Now
            _comboBox.UseFixedNowDate = true;
        }

        [Test]
        public void TestDefaultDefaultContstructor()
        {
            //---------------Set up test pack-------------------

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            IDateRangeComboBox comboBox = GetControlFactory().CreateDateRangeComboBox();
            //---------------Test Result -----------------------
            Assert.IsNotNull(comboBox.OptionsToDisplay);
            Assert.AreEqual(_numDefaultOptions, comboBox.OptionsToDisplay.Count);
            Assert.IsFalse(comboBox.IgnoreTime);

            Assert.AreEqual(TimeSpan.Zero, comboBox.MidnightOffset);
            Assert.AreEqual(0, comboBox.WeekStartOffset);
            Assert.AreEqual(0, comboBox.MonthStartOffset);
            Assert.AreEqual(0, comboBox.YearStartOffset);

            Assert.AreEqual(comboBox.OptionsToDisplay.Count + 1, comboBox.Items.Count,
                            "There should b an extra item in the combo box for the user hint");
            Assert.AreEqual("(Date Ranges)", _comboBox.Items[0], "The user hint should be set as the first item");
            Assert.AreEqual("Today", _comboBox.Items[1].ToString(), "First selectable item");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestUserFixedNowDate()
        {
            //---------------Set up test pack-------------------

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(_numDefaultOptions, _comboBox.OptionsToDisplay.Count);

            //---------------Execute Test ----------------------
            _comboBox.UseAllDateOptions();
            //---------------Test Result -----------------------
            Assert.AreEqual(_numAllOptions, _comboBox.OptionsToDisplay.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSetTopComboBoxItem()
        {
            //--------------Assert PreConditions----------------            

            Assert.AreEqual("(Date Ranges)", _comboBox.Items[0]);
            //---------------Execute Test ----------------------

            _comboBox.SetTopComboBoxItem("Express options...");
            //---------------Test Result -----------------------
            Assert.AreEqual("Express options...", _comboBox.Items[0], "The user hint should have changed");
        }

        [Test]
        public void TestGetDateRangeString()
        {
            //---------------Set up test pack-------------------
            
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            string dateRangeString = _comboBox.GetDateRangeString(DateRangeOptions.ThisWeek);

            //---------------Test Result -----------------------
            Assert.AreEqual("This Week", dateRangeString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSetDateRangeString()
        {
            //You can rename the string that is shown for any option e.g. u can change this year to current year
            //---------------Set up test pack-------------------
            string expectedDateRangeString = "Another Week";
            //--------------Assert PreConditions----------------            
            Assert.AreEqual("This Week", _comboBox.GetDateRangeString(DateRangeOptions.ThisWeek));
            Assert.IsFalse(_comboBox.Items.Contains(expectedDateRangeString));
            //---------------Execute Test ----------------------
            _comboBox.SetDateRangeString(DateRangeOptions.ThisWeek, expectedDateRangeString);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDateRangeString, _comboBox.GetDateRangeString(DateRangeOptions.ThisWeek));
            Assert.IsTrue(_comboBox.Items.Contains(expectedDateRangeString));
            //---------------Tear Down -------------------------          
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSetDateRange_SetAnExistingOptionToTextThatIsAlreadyInList()
        {
            //---------------Set up test pack-------------------
            
            //--------------Assert PreConditions----------------            
            Assert.IsTrue(_comboBox.Items.Contains("Previous Week"));
            //---------------Execute Test ----------------------
            _comboBox.SetDateRangeString(DateRangeOptions.ThisWeek, "Previous Week");
            //---------------Test Result -----------------------
            //EpectedException
        }
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSetDateRangeString_OptionNonCurrentlyAvailable()
        {
            //---------------Set up test pack-------------------
            
            //--------------Assert PreConditions----------------            
            Assert.IsFalse(_comboBox.OptionsToDisplay.Contains(DateRangeOptions.Current60Minutes));
            //---------------Execute Test ----------------------
            _comboBox.SetDateRangeString(DateRangeOptions.Current60Minutes, "Last Hour");
            //---------------Test Result -----------------------

            //---------------Tear Down -------------------------          
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSetDateRangeString_RenameAnExistingOption_ToAnotherOption_ThatIsNotInList()
        {
            //---------------Set up test pack-------------------
            
            //--------------Assert PreConditions----------------            
            Assert.IsFalse(_comboBox.OptionsToDisplay.Contains(DateRangeOptions.ThisHour));
            //---------------Execute Test ----------------------
            _comboBox.SetDateRangeString(DateRangeOptions.ThisWeek, "This Hour");
            //---------------Test Result -----------------------

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSpecialPropertyAssignments()
        {
            //--------------Assert PreConditions----------------            

            Assert.IsFalse(_comboBox.IgnoreTime);
            Assert.AreEqual(TimeSpan.Zero, _comboBox.MidnightOffset);
            Assert.AreEqual(0, _comboBox.WeekStartOffset);
            Assert.AreEqual(0, _comboBox.MonthStartOffset);
            Assert.AreEqual(0, _comboBox.YearStartOffset);

            //---------------Execute Test ----------------------
            _comboBox.IgnoreTime = true;
            _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0);
            _comboBox.WeekStartOffset = 1;
            _comboBox.MonthStartOffset = 1;
            _comboBox.YearStartOffset = 1;

            //---------------Test Result -----------------------
            Assert.IsTrue(_comboBox.IgnoreTime);
            Assert.AreEqual(_comboBox.MidnightOffset, new TimeSpan(0, 1, 0, 0));
            Assert.AreEqual(_comboBox.WeekStartOffset, 1);
            Assert.AreEqual(_comboBox.MonthStartOffset, 1);
            Assert.AreEqual(_comboBox.YearStartOffset, 1);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestMidnightOffsetExceptionDays()
        {
            //---------------Execute Test ----------------------
            //Cannot offset to previous days
            _comboBox.MidnightOffset = new TimeSpan(-1, 0, 0, 0);
        }
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestMidnightOffsetExceptionDays_Add()
        {
            //---------------Execute Test ----------------------
            //Cannot offset to future days
            _comboBox.MidnightOffset = new TimeSpan(+1, 0, 0, 0);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestMidnightOffsetExceptionHours()
        {
            _comboBox.MidnightOffset = new TimeSpan(0, -25, 0, 0);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestMonthStartOffsetException_NegativeMonth()
        {
            _comboBox.MonthStartOffset = -1;
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestYearStartOffsetException_NegativeYear()
        {
            _comboBox.YearStartOffset = -1;
        }

        [Test]
        public void TestSetOptionsToDisplay()
        {
            //---------------Execute Test ----------------------
            List<DateRangeOptions> options = new List<DateRangeOptions>();
            options.Add(DateRangeOptions.Previous365Days);
            options.Add(DateRangeOptions.PreviousYear);
            _comboBox.OptionsToDisplay = options;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, _comboBox.OptionsToDisplay.Count);
            Assert.AreEqual(3, _comboBox.Items.Count);
            Assert.AreEqual("(Date Ranges)", _comboBox.Items[0]);
            Assert.AreEqual("Previous 365 Days", _comboBox.Items[1]);
            Assert.AreEqual("Previous Year", _comboBox.Items[2]);
        }

        [Test]
        public void TestRemoveOption()
        {
            //---------------Set up test pack-------------------
            
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(_numDefaultOptions, _comboBox.OptionsToDisplay.Count);
            Assert.AreEqual(_numDefaultOptions + 1, _comboBox.Items.Count);
            //---------------Execute Test ----------------------
            _comboBox.RemoveDateOption(DateRangeOptions.Today);
            //---------------Test Result -----------------------
            Assert.AreEqual(_numDefaultOptions - 1, _comboBox.OptionsToDisplay.Count);
            Assert.AreEqual(_numDefaultOptions, _comboBox.Items.Count);
            Assert.AreEqual(-1, _comboBox.Items.IndexOf("Today"));
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestRemoveItemTwice()
        {
            //---------------Set up test pack-------------------
            
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(_numDefaultOptions, _comboBox.OptionsToDisplay.Count);
            Assert.AreEqual(_numDefaultOptions + 1, _comboBox.Items.Count);
            //---------------Execute Test ----------------------
            _comboBox.RemoveDateOption(DateRangeOptions.Today);
            _comboBox.RemoveDateOption(DateRangeOptions.Today);
            //---------------Test Result -----------------------
            Assert.AreEqual(_numDefaultOptions - 1, _comboBox.OptionsToDisplay.Count);
            Assert.AreEqual(_numDefaultOptions, _comboBox.Items.Count);
            Assert.AreEqual(-1, _comboBox.Items.IndexOf("Today"));
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddOption()
        {
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(_numDefaultOptions, _comboBox.OptionsToDisplay.Count);
            Assert.AreEqual(_numDefaultOptions + 1, _comboBox.Items.Count);

            //---------------Execute Test ----------------------
            _comboBox.AddDateOption(DateRangeOptions.ThisHour);

            //---------------Test Result -----------------------
            Assert.AreEqual(_numDefaultOptions + 1, _comboBox.OptionsToDisplay.Count);
            Assert.AreEqual(_numDefaultOptions + 2, _comboBox.Items.Count);
            Assert.GreaterOrEqual( _comboBox.Items.IndexOf("This Hour"),0);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddOptionTwiceNoError()
        {
            //---------------Set up test pack-------------------
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(_numDefaultOptions, _comboBox.OptionsToDisplay.Count);
            Assert.AreEqual(_numDefaultOptions + 1, _comboBox.Items.Count);

            //---------------Execute Test ----------------------
            _comboBox.AddDateOption(DateRangeOptions.ThisHour);
            _comboBox.AddDateOption(DateRangeOptions.ThisHour);

            //---------------Test Result -----------------------
            Assert.AreEqual(_numDefaultOptions + 1, _comboBox.OptionsToDisplay.Count);
            Assert.AreEqual(_numDefaultOptions + 2, _comboBox.Items.Count);
            Assert.GreaterOrEqual(_comboBox.Items.IndexOf("This Hour"), 0);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestFixedNowAssignment()
        {   
            //--------------Assert PreConditions----------------            
            Assert.IsTrue(_comboBox.UseFixedNowDate);
            Assert.AreEqual(DateTime.Now.Year, _comboBox.FixedNowDate.Year);
            //---------------Execute Test ----------------------

            //---------------Execute Test ----------------------
            _comboBox.FixedNowDate = new DateTime(2000, 2, 2);

            //---------------Test Result -----------------------
            Assert.AreEqual(new DateTime(2000, 2, 2), _comboBox.FixedNowDate);

            // Setting to false shouldn't affect FixedNowDate property
            _comboBox.UseFixedNowDate = false;
            Assert.IsFalse(_comboBox.UseFixedNowDate);
            Assert.AreEqual(new DateTime(2000, 2, 2), _comboBox.FixedNowDate);
        }

        [Test]
        public void TestSettingFixedNowDate_False_DoesNotAffectTheFixedNowDate()
        {
            //---------------Set up test pack-------------------
            _comboBox.FixedNowDate = new DateTime(2000, 2, 2);
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(new DateTime(2000, 2, 2), _comboBox.FixedNowDate);
            Assert.IsTrue(_comboBox.UseFixedNowDate);
            //---------------Execute Test ----------------------
            // Setting to false shouldn't affect FixedNowDate property
            _comboBox.UseFixedNowDate = false;

            //---------------Test Result -----------------------
            Assert.IsFalse(_comboBox.UseFixedNowDate);
            Assert.AreEqual(new DateTime(2000, 2, 2), _comboBox.FixedNowDate);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_GetDefaultStartDate()
        {
            //---------------Set up test pack-------------------
            
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            DateTime expectedStartDate = _comboBox.StartDate;
            //---------------Test Result -----------------------
            Assert.AreEqual(DateTime.MinValue, expectedStartDate);

            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_GetDefaultEndDate()
        {
            //---------------Execute Test ----------------------
            DateTime expectedEndDate = _comboBox.EndDate;
            
            //---------------Test Result -----------------------
            Assert.AreEqual(DateTime.MaxValue, expectedEndDate);

            //---------------Tear Down -------------------------          
        }

        //    // Testing guidelines for date options
        //    //   - first test the standard case
        //    //   - test the boundary cases
        //    //   - do a full combination of all applicable offset cases
        //    //       - test with offsets that go both ways, where applicable
        //    //       - test dates on both sides of the above cases

        //    [Test]
        //    public void TestThisHour()
        //    {
        //        _comboBox.AddDateOption(DateRangeOptions.ThisHour);
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.ThisHour);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 20, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestPreviousHour()
        //    {
        //        _comboBox.AddDateOption(DateRangeOptions.PreviousHour);
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousHour);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 19, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 20, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 19, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 20, 0, 0, 0), _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestCurrent60Minutes()
        //    {
        //        _comboBox.AddDateOption(DateRangeOptions.Current60Minutes);
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Current60Minutes);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 19, 38, 12, 10), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestToday()
        //    {
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Today);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, 6, 0, 0, 10);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 7, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 6, 0, 0, 10), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, 6, 0, 0, 10);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 6, 0, 0, 10), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, 6, 0, 0, 10);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 3, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 12, 31, 6, 0, 0, 10), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 22, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 23, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 22, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 22, 0, 0, 10);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 22, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 21, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 11, 22, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestYesterday()
        //    {
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Yesterday);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, 6, 0, 0, 10);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 11, 6, 0, 0, 10), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 6, 0, 0, 10), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 11, 22, 0, 0, 00), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 22, 0, 0, 00), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 23, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 22, 0, 0, 00), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 22, 0, 0, 00), _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestCurrent24Hours()
        //    {
        //        _comboBox.AddDateOption(DateRangeOptions.Current24Hours);
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Current24Hours);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 20, 38, 12, 10), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestThisWeek()
        //    {
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.ThisWeek);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 0, 0, 0, 0);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = -1;
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 11, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = -1;
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 11, 12, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 11, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = 1;
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 6, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = 0;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 5, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = 0;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 11, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 23, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 22, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 21, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 5, 22, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = -1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 11, 23, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 10, 22, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = -1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 2, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 11, 23, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 11, 2, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = -1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 2, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 1, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 11, 2, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = -1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 2, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 11, 3, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 11, 2, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestLastWeek()
        //    {
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousWeek);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 11, 5, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 5, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = -1;
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 11, 4, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 11, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = -1;
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 11, 12, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 4, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 11, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = 1;
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 10, 30, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 6, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 30, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 6, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = -1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 4, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 11, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = -1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 11, 23, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 3, 22, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 10, 22, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = -1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 10, 23, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 3, 22, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 10, 22, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.WeekStartOffset = -1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -2, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 10, 21, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 27, 22, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 3, 22, 0, 0, 0), _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestPrevious7Days()
        //    {
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous7Days);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 11, 6, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 6, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 5, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 5, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 23, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 23, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 5, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 23, 0, 0, 0), _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestThisMonth()
        //    {
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.ThisMonth);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 11, 1, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 0, 0, 0, 0);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 31, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 10, 31, 23, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 31, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 1, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 3, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 2, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 2, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 3, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 2, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 3, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 11, 1, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 10, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 1, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestLastMonth()
        //    {
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousMonth);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 10, 1, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 1, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 1, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 1, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 1, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 1, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 0, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 9, 1, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 10, 1, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 9, 30, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 10, 31, 23, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 10, 31, 23, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 9, 30, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 10, 31, 23, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 10, 2, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 2, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 2, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 2, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 2, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 2, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 2, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 2, 0, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 9, 2, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 10, 2, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 1, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 1, 23, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 1, 23, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 1, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 1, 23, 0, 0, 0), _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestPrevious30Days()
        //    {
        //        _comboBox.AddDateOption(DateRangeOptions.Previous30Days);
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous30Days);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 10, 14, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 14, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 13, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 13, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 23, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 23, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 13, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 23, 0, 0, 0), _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestPrevious31Days()
        //    {
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous31Days);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 10, 13, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 13, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 12, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 12, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 23, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 23, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 10, 12, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 23, 0, 0, 0), _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestThisYear()
        //    {
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.ThisYear);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 0, 0, 0, 0);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 12, 31, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2006, 12, 31, 23, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 12, 31, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 1, 1, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 3, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 1, 2, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 1, 2, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 3, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 1, 2, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 3, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 1, 1, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 10, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 1, 1, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 0;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2007, 2, 1, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 0;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 0, 0, 0, 0);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 0;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 1, 31, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 0;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 31, 23, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 1, 31, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 0;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 2, 1, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 2, 3, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 2, 2, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 2, 2, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 2, 3, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 2, 2, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 2, 3, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2007, 2, 1, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 10, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 2, 1, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestLastYear()
        //    {
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.PreviousYear);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2006, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 2, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 1, 1, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 1, 1, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 0, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2005, 1, 1, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2006, 1, 1, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 3, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2005, 12, 31, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2006, 12, 31, 23, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2006, 12, 31, 23, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2005, 12, 31, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2006, 12, 31, 23, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2006, 1, 2, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 1, 2, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 2, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 1, 2, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 1, 2, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 3, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 1, 2, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 1, 2, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 2, 0, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2005, 1, 2, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2006, 1, 2, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 3, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 1, 1, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 1, 1, 23, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 1, 23, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 1, 1, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 1, 1, 23, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 0;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2006, 2, 1, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 2, 1, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 0;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 2, 1, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 2, 1, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 0;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 2, 2, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 2, 1, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 2, 1, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 0;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 0, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2005, 2, 1, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2006, 2, 1, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 2, 3, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 1, 31, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 1, 31, 23, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 0;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 1, 31, 23, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 1, 31, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 1, 31, 23, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2006, 2, 2, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 2, 2, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 0, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 2, 2, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 2, 2, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 2, 2, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 2, 3, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 2, 2, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 2, 2, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 2, 2, 0, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2005, 2, 2, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2006, 2, 2, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 2, 3, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 2, 1, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 2, 1, 23, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.YearStartOffset = 1;
        //        _comboBox.MonthStartOffset = 1;
        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 2, 1, 23, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 2, 1, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 2, 1, 23, 0, 0, 0), _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestPrevious365Days()
        //    {
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous365Days);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2006, 11, 13, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 11, 13, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 11, 12, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 1, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 0, 0, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 11, 12, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 23, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, -1, 0, 0, 0);
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 12, 23, 30, 0, 0);
        //        Assert.AreEqual(new DateTime(2006, 11, 12, 23, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 23, 0, 0, 0), _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestCurrent2Years()
        //    {
        //        _comboBox.AddDateOption(DateRangeOptions.Current2Years);
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Current2Years);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2005, 11, 13, 20, 38, 12, 10), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestCurrent3Years()
        //    {
        //        _comboBox.AddDateOption(DateRangeOptions.Current3Years);
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Current3Years);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2004, 11, 13, 20, 38, 12, 10), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestCurrent5Years()
        //    {
        //        _comboBox.AddDateOption(DateRangeOptions.Current5Years);
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Current5Years);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2002, 11, 13, 20, 38, 12, 10), _comboBox.StartDate);
        //        Assert.AreEqual(_comboBox.FixedNowDate, _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestPrevious2Years()
        //    {
        //        _comboBox.AddDateOption(DateRangeOptions.Previous2Years);
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous2Years);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2005, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestPrevious3Years()
        //    {
        //        _comboBox.AddDateOption(DateRangeOptions.Previous3Years);
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous3Years);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2004, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestPrevious5Years()
        //    {
        //        _comboBox.AddDateOption(DateRangeOptions.Previous5Years);
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Previous5Years);

        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);
        //        Assert.AreEqual(new DateTime(2002, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.EndDate);
        //    }


        //    [Test]
        //    public void TestRealNow()
        //    {
        //        _comboBox.UseFixedNowDate = false;
        //        DateTime now = DateTime.Now;
        //        DateTime yest = now.AddDays(-1);

        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Yesterday);
        //        Assert.AreEqual(new DateTime(yest.Year, yest.Month, yest.Day, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0), _comboBox.EndDate);
        //    }

        //    [Test]
        //    public void TestIgnoreTime()
        //    {
        //        _comboBox.IgnoreTime = true;
        //        _comboBox.FixedNowDate = new DateTime(2007, 11, 13, 20, 38, 12, 10);

        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Today);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.ThisWeek);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.ThisMonth);
        //        Assert.AreEqual(new DateTime(2007, 11, 1, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.ThisYear);
        //        Assert.AreEqual(new DateTime(2007, 1, 1, 0, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);

        //        _comboBox.MidnightOffset = new TimeSpan(0, 1, 0, 0, 0);
        //        _comboBox.SelectedItem = _comboBox.GetDateRangeString(DateRangeOptions.Today);
        //        Assert.AreEqual(new DateTime(2007, 11, 12, 1, 0, 0, 0), _comboBox.StartDate);
        //        Assert.AreEqual(new DateTime(2007, 11, 13, 0, 0, 0, 0), _comboBox.EndDate);
        //    }
        //}
    }
}