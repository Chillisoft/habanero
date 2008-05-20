using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Util;

namespace Habanero.UI.Base
{
    public class DateRangeComboBoxManager
    {
        private IComboBox _comboBox;
        private Dictionary<DateRangeOptions, string> _dateRangePairs;
        private List<DateRangeOptions> _optionsToDisplay;

        private DateTime _startDate;
        private DateTime _endDate;

        private bool _ignoreTime;
        private TimeSpan _midnightOffset;
        private int _weekStartOffset;
        private int _monthStartOffset;
        private int _yearStartOffset;

        private Boolean _useFixedNowDate;
        private DateTime _fixedNow;

        /// <summary>
        /// Constructor to initialise a new ComboBox with a selection of
        /// date range options that are suited to a timeless system
        /// </summary>
        /// <param name="comboBox">The combobox to be managed</param>
        public DateRangeComboBoxManager(IComboBox comboBox)
        {
            _comboBox = comboBox;

            _optionsToDisplay = new List<DateRangeOptions>();
            _optionsToDisplay.Add(DateRangeOptions.Today);
            _optionsToDisplay.Add(DateRangeOptions.Yesterday);
            _optionsToDisplay.Add(DateRangeOptions.ThisWeek);
            _optionsToDisplay.Add(DateRangeOptions.PreviousWeek);
            _optionsToDisplay.Add(DateRangeOptions.Previous7Days);
            _optionsToDisplay.Add(DateRangeOptions.ThisMonth);
            _optionsToDisplay.Add(DateRangeOptions.PreviousMonth);
            _optionsToDisplay.Add(DateRangeOptions.Previous31Days);
            _optionsToDisplay.Add(DateRangeOptions.ThisYear);
            _optionsToDisplay.Add(DateRangeOptions.PreviousYear);
            _optionsToDisplay.Add(DateRangeOptions.Previous365Days);

            InitialiseValues();
        }

        /// <summary>
        /// Initialises the ComboBox with a list of values to display
        /// </summary>
        private void InitialiseValues()
        {
            SetDateRangePairs();
            BuildComboBoxList();

            _ignoreTime = false;
            _useFixedNowDate = false;
            _fixedNow = DateTime.Now;
            _weekStartOffset = 0;
            _monthStartOffset = 0;
            _yearStartOffset = 0;
        }

        /// <summary>
        /// Populates the ComboBox with the current set of date options
        /// </summary>
        private void BuildComboBoxList()
        {
            _comboBox.Items.Clear();
            _comboBox.Items.Add("(Date Ranges)");
            _comboBox.Text = _comboBox.Items[0].ToString();
            foreach (DateRangeOptions option in _optionsToDisplay)
            {
                _comboBox.Items.Add(_dateRangePairs[option]);
            }
        }

        /// <summary>
        /// Populates the ComboBox with all available DateOptions, since
        /// the default constructor only provides a standardised collection
        /// </summary>
        public void UseAllDateOptions()
        {
            _optionsToDisplay = new List<DateRangeOptions>();
            foreach (int constant in Enum.GetValues(typeof(DateRangeOptions)))
            {
                AddDateOption((DateRangeOptions)Enum.ToObject(typeof(DateRangeOptions), constant));
            }
        }

        /// <summary>
        /// Sets the item in the ComboBox that first appears to the user
        /// </summary>
        /// <param name="displayString">The string to display</param>
        public void SetTopComboBoxItem(string displayString)
        {
            string oldText = _comboBox.Items[0].ToString();
            _comboBox.Items[0] = displayString;
            if (_comboBox.Text == oldText)
            {
                _comboBox.Text = _comboBox.Items[0].ToString();
            }
        }

        /// <summary>
        /// Creates a dictionary of enum and string pairs to define
        /// how each option is displayed and to recognise the appropriate
        /// option from the user's selection
        /// </summary>
        private void SetDateRangePairs()
        {
            _dateRangePairs = new Dictionary<DateRangeOptions, string>();
            foreach (int constant in Enum.GetValues(typeof(DateRangeOptions)))
            {
                string displayString = StringUtilities.DelimitPascalCase(Enum.GetName(typeof(DateRangeOptions), constant), " ");
                _dateRangePairs.Add((DateRangeOptions)Enum.ToObject(typeof(DateRangeOptions), constant), displayString);
            }
        }

        /// <summary>
        /// Amends the display string for a given date option
        /// </summary>
        /// <param name="option">The date option to amend</param>
        /// <param name="newDisplayString">The display string to apply</param>
        public void SetDateRangeString(DateRangeOptions option, string newDisplayString)
        {
            if (_dateRangePairs.ContainsKey(option))
            {
                if (_dateRangePairs.ContainsValue(newDisplayString)
                    && _dateRangePairs[option] != newDisplayString)
                {
                    throw new ArgumentException("A date range display string " +
                        "is being assigned, but that display string has already " +
                        "been used.");
                }
                int comboBoxPos = _comboBox.Items.IndexOf(_dateRangePairs[option]);
                _comboBox.Items[comboBoxPos] = newDisplayString;
                _dateRangePairs[option] = newDisplayString;
            }
            else
            {
                throw new ArgumentException("A date range string is being changed, " +
                    "but the given option does not exist in the collection " +
                    "of date options.");
            }
        }

        /// <summary>
        /// Returns the display string for the date range option supplied
        /// </summary>
        /// <param name="option">The date range enumeration</param>
        /// <returns>Returns the string if found, otherwise throws an
        /// ArgumentException</returns>
        public string GetDateRangeString(DateRangeOptions option)
        {
            if (_dateRangePairs.ContainsKey(option))
            {
                return _dateRangePairs[option];
            }
            else
            {
                throw new ArgumentException("A date range option string is being " +
                    "accessed, but the given date range option does not exist.");
            }
        }

        /// <summary>
        /// Adds a date range option to the current list of options available
        /// </summary>
        /// <param name="option">The date range option to add</param>
        public void AddDateOption(DateRangeOptions option)
        {
            if (!_optionsToDisplay.Contains(option))
            {
                _optionsToDisplay.Add(option);
                _comboBox.Items.Add(_dateRangePairs[option]);
            }
        }

        /// <summary>
        /// Removes a date range option from the current list of options available
        /// </summary>
        /// <param name="option">The date range option to remove</param>
        public void RemoveDateOption(DateRangeOptions option)
        {
            if (_optionsToDisplay.Contains(option))
            {
                _optionsToDisplay.Remove(option);
                _comboBox.Items.Remove(_dateRangePairs[option]);
            }
        }

        /// <summary>
        /// Gets and sets whether the date used to calculate date ranges
        /// should be DateTime.Now or a fixed date that is specified.
        /// When false, all date ranges are calculated based on DateTime.Now.
        /// Setting this property to true allows you to use an alternative
        /// fixed date as your "Now" value, using the FixedNow property.
        /// </summary>
        public bool UseFixedNowDate
        {
            get { return _useFixedNowDate; }
            set { _useFixedNowDate = value; }
        }

        /// <summary>
        /// Gets and sets the list of options to display.  If you intend
        /// to edit individual items in the list, either set the entire
        /// list once you have edited it, or use the Add and Remove methods
        /// provided by this class, otherwise the ComboBox list will not
        /// be refreshed.
        /// </summary>
        public List<DateRangeOptions> OptionsToDisplay
        {
            get { return _optionsToDisplay; }
            set { _optionsToDisplay = value; }
        }

        /// <summary>
        /// Sets the current date (eg. DateTime.Now or FixedNowDate) in all calculations to 12am.
        /// Use caution when using this together with a MidnightOffset, in which
        /// case you may rather want to manually edit the time just before calling
        /// StartDate and EditDate (use UseFixedNowDate and FixedNowDate).
        /// </summary>
        public bool IgnoreTime
        {
            get { return _ignoreTime; }
            set { _ignoreTime = value; }
        }

        /// <summary>
        /// Gets and sets the amount of time to add or subtract from
        /// midnight when calculating date ranges.  This option will
        /// typically be used where a shift operates on a different
        /// pattern to 12am to 12am (the default).  If, for instance,
        /// an industry's operational day runs from 6am to 6am, this
        /// property can be set with a TimeSpan that adds 6 hours.
        /// Conversely, if the day starts 2 hours earlier, at 10pm the
        /// previous evening, set the property with a TimeSpan that
        /// subtracts 2 hours.
        /// </summary>
        public TimeSpan MidnightOffset
        {
            get { return _midnightOffset; }
            set { _midnightOffset = value; }
        }

        /// <summary>
        /// Gets and sets the number of days to add or subtract from
        /// Monday to redefine the first day of the week.  If Sunday
        /// is the first day of the week for the given application,
        /// then this property can be set with -1.  If Tuesday is the
        /// first day then use 1 (1+1=2).
        /// </summary>
        public int WeekStartOffset
        {
            get { return _weekStartOffset; }
            set { _weekStartOffset = value; }
        }

        /// <summary>
        /// Gets and sets the number of days to add or subtract from
        /// the first day of the month in order to adjust which day
        /// is typically the first of the month.  If the 5th is the typical start
        /// of a new month for the given application,
        /// then this property can be set to 4 (1+4=5).
        /// </summary>
        public int MonthStartOffset
        {
            get { return _monthStartOffset; }
            set { _monthStartOffset = value; }
        }

        /// <summary>
        /// Gets and sets the number of months to add or subtract from
        /// January to redefine the first month of the year.  For example,
        /// if March is the first month of the new year for the given application,
        /// then this property can be set with 2 (1+2=3).
        /// </summary>
        public int YearStartOffset
        {
            get { return _yearStartOffset; }
            set { _yearStartOffset = value; }
        }
    }
}
