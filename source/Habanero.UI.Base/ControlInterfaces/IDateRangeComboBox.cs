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
using System.Text;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides a selection of common date range options which a user
    /// can select from, modifying a start date and end date depending
    /// on the selection.  The options to choose from, such as
    /// "Yesterday", "Last Week" or "This Year", can be modified by
    /// the developer.  When adding this control, add an event handler
    /// on the SelectionChangeCommitted event, and obtain the StartDate and
    /// EndDate properties at this time.
    /// <br/>
    /// NOTE: The start and end dates are the two boundaries of a
    /// potential range.  Whether they should be inclusive or exclusive
    /// depends on the developer and how they are implemented.  For
    /// instance, if today is Nov 13, Yesterday would provide a start
    /// date of Nov 12, 12am and an end date of Nov 13, 12am.
    /// <br/>
    /// To add additional menu options that aren't available, simply add the
    /// new text item to the ComboBox, using Items.Add.  When you pick up the
    /// SelectionChangeCommitted event, check if the new text string is selected
    /// in the Text property and use your own calculations instead of 
    /// fetching the StartDate and EndDate.
    /// </summary>
    public interface IDateRangeComboBox : IComboBox
    {
        /// <summary>
        /// Gets and sets the list of options to display.  If you intend
        /// to edit individual items in the list, either set the entire
        /// list once you have edited it, or use the Add and Remove methods
        /// provided by this class, otherwise the ComboBox list will not
        /// be refreshed.
        /// </summary>
        List<DateRangeOptions> OptionsToDisplay { get; set; }

        /// <summary>
        /// Sets the current date (eg. DateTime.Now or FixedNowDate) in all calculations to 12am.
        /// Use caution when using this together with a MidnightOffset, in which
        /// case you may rather want to manually edit the time just before calling
        /// StartDate and EditDate (use UseFixedNowDate and FixedNowDate).
        /// </summary>
        bool IgnoreTime { get; set; }

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
        TimeSpan MidnightOffset { get; set; }

        /// <summary>
        /// Gets and sets the number of days to add or subtract from
        /// Monday to redefine the first day of the week.  If Sunday
        /// is the first day of the week for the given application,
        /// then this property can be set with -1.  If Tuesday is the
        /// first day then use 1 (1+1=2).
        /// </summary>
        int WeekStartOffset { get; set; }

        /// <summary>
        /// Gets and sets the number of days to add or subtract from
        /// the first day of the month in order to adjust which day
        /// is typically the first of the month.  If the 5th is the typical start
        /// of a new month for the given application,
        /// then this property can be set to 4 (1+4=5).
        /// </summary>
        int MonthStartOffset { get; set; }

        /// <summary>
        /// Gets and sets the number of months to add or subtract from
        /// January to redefine the first month of the year.  For example,
        /// if March is the first month of the new year for the given application,
        /// then this property can be set with 2 (1+2=3).
        /// </summary>
        int YearStartOffset { get; set; }

        /// <summary>
        /// Gets and sets whether the date used to calculate date ranges
        /// should be DateTime.Now or a fixed date that is specified.
        /// When false, all date ranges are calculated based on DateTime.Now.
        /// Setting this property to true allows you to use an alternative
        /// fixed date as your "Now" value, using the FixedNow property.
        /// </summary>
        bool UseFixedNowDate { get; set; }

        /// <summary>
        /// Gets and sets a fixed date used to calculate date ranges, rather
        /// than DateTime.Now.  The UseFixedNowDate property must be set to
        /// true, otherwise this property will be ignored.
        /// </summary>
        DateTime FixedNowDate { get; set; }

        /// <summary>
        /// Populates the ComboBox with all available DateOptions, since
        /// the default constructor only provides a standardised collection
        /// </summary>
        void UseAllDateOptions();

        /// <summary>
        /// Sets the item in the ComboBox that first appears to the user
        /// </summary>
        /// <param name="displayString">The string to display</param>
        void SetTopComboBoxItem(string displayString);

        /// <summary>
        /// Returns the display string for the date range option supplied
        /// </summary>
        /// <param name="option">The date range enumeration</param>
        /// <returns>Returns the string if found, otherwise throws an
        /// ArgumentException</returns>
        string GetDateRangeString(DateRangeOptions option);

        /// <summary>
        /// Amends the display string for a given date option
        /// </summary>
        /// <param name="option">The date option to amend</param>
        /// <param name="newDisplayString">The display string to apply</param>
        void SetDateRangeString(DateRangeOptions option, string newDisplayString);

        /// <summary>
        /// Removes a date range option from the current list of options available
        /// </summary>
        /// <param name="option">The date range option to remove</param>
        void RemoveDateOption(DateRangeOptions option);

        /// <summary>
        /// Adds a date range option to the current list of options available
        /// </summary>
        /// <param name="option">The date range option to add</param>
        void AddDateOption(DateRangeOptions option);

        /// <summary>
        /// Returns the start date for the currently selected date range option,
        /// or DateTime.MinValue if no valid option is selected
        /// </summary>
        DateTime StartDate { get; }

        /// <summary>
        /// Returns the end date for the currently selected date range option,
        /// or DateTime.MaxValue if no valid option is selected
        /// </summary>
        DateTime EndDate { get; }
    }

    /// <summary>
    /// Provides options that can be added to or removed from the
    /// DateRangeComboBox
    /// </summary>
    public enum DateRangeOptions
    {
        /// <summary>
        /// The period from the start of the hour until now
        /// </summary>
        ThisHour,
        /// <summary>
        /// The period covering the previous hour before the start of the
        /// current one
        /// </summary>
        PreviousHour,
        /// <summary>
        /// The period covering the previous 60 minutes before this one
        /// </summary>
        Current60Minutes,
        /// <summary>
        /// The period from the start of the day until now
        /// </summary>
        Today,
        /// <summary>
        /// The period from the start of the previous day till
        /// the end of the previous day
        /// </summary>
        Yesterday,
        /// <summary>
        /// The period covering the last 24 hours up till now
        /// </summary>
        Current24Hours,
        /// <summary>
        /// The period from the first day of the week until now
        /// </summary>
        ThisWeek,
        /// <summary>
        /// The period from the first day of the previous week
        /// until the last day of the previous week
        /// </summary>
        PreviousWeek,
        /// <summary>
        /// The previous seven days, excluding today
        /// </summary>
        Previous7Days,
        /// <summary>
        /// The period from the start of the month until now
        /// </summary>
        ThisMonth,
        /// <summary>
        /// The period covering the previous month
        /// </summary>
        PreviousMonth,
        /// <summary>
        /// The period covering the previous 30 days, excluding today
        /// </summary>
        Previous30Days,
        /// <summary>
        /// The period covering the previous 31 days, excluding today
        /// </summary>
        Previous31Days,
        /// <summary>
        /// The period covering the start of the year until now
        /// </summary>
        ThisYear,
        /// <summary>
        /// The period covering the previous year
        /// </summary>
        PreviousYear,
        /// <summary>
        /// The period covering the last 365 days, excluding today
        /// </summary>
        Previous365Days,
        /// <summary>
        /// The period covering the 2 years up till now
        /// </summary>
        Current2Years,
        /// <summary>
        /// The period covering the 3 years up till now
        /// </summary>
        Current3Years,
        /// <summary>
        /// The period covering the 5 years up till now
        /// </summary>
        Current5Years,
        /// <summary>
        /// The period covering the 2 years before the current one
        /// </summary>
        Previous2Years,
        /// <summary>
        /// The period covering the 3 years before the current one
        /// </summary>
        Previous3Years,
        /// <summary>
        /// The period covering the 5 years before the current one
        /// </summary>
        Previous5Years
    }
}