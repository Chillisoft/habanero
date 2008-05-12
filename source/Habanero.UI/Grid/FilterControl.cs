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
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Forms;

namespace Habanero.UI.Grid
{
    /// <summary>
    /// Manages a collection of filter input controls, that allow rows of
    /// data to be filtered to display only data that complies with some
    /// setting in the controls (eg. a filter clause text-box could allow the
    /// user to type in "Smith" and cause all rows without "Smith" in the
    /// surname column to be hidden).
    /// <br/><br/>
    /// This control can contain any number of filter controls - the
    /// data will be filtered based on a criteria composed of all the individual
    /// controls' filter clauses.
    /// <br/><br/>
    /// FilterControl is very similar to FilterInputBoxCollection, except that
    /// this class is itself a control that manages the items in a layout, whereas
    /// the latter is just a collection of controls, and you would need to place
    /// the controls in the user interface yourself.  This class also lets you
    /// use the same method call to add a filter control and a label before it.
    /// <br/><br/>
    /// Controls are laid out in the order they are added, using a flow layout.
    /// To determine the visual width of each individual control, call 
    /// SetFilterWidth just before adding each control.
    /// <br/><br/>
    /// The automatic-updates setting determines whether filtering takes place
    /// as you type (into a text-box, for example) or only when you commit the
    /// change (by pressing Enter, for example).
    /// </summary>
    public class FilterControl : UserControl
    {
        private readonly FilterInputBoxCollection _filterInputBoxCollection;
        private readonly FlowLayoutManager _layoutManager;
        
        //public event EventHandler FilterClauseChanged;

        public event EventHandler<FilterControlEventArgs> FilterClauseChanged;
        
        /// <summary>
        /// Default constructor - uses the DataViewFilterClauseFactory as the filer clause factory.
        /// </summary>
        public FilterControl() : this(new DataViewFilterClauseFactory()) {}

        /// <summary>
        /// Constructor to initialise a new filter control
        /// </summary>
        /// <param name="clauseFactory">The filter clause factory</param>
        public FilterControl(IFilterClauseFactory clauseFactory)
        {
            _layoutManager = new FlowLayoutManager(this);
            _filterInputBoxCollection = new FilterInputBoxCollection(clauseFactory);
            _filterInputBoxCollection.FilterClauseChanged += FilterControlValueChangedHandler;
            this.Height = new TextBox().Height + 10;
        }

        /// <summary>
        /// Adds a TextBox filter in which users can specify text that
        /// a string-value column will be filtered on.  This uses a "like"
        /// operator and accepts any strings that contain the provided clause.
        /// </summary>
        /// <param name="label">The label to appear before the TextBox</param>
        /// <param name="columnName">The column of data on which to do the
        /// filtering</param>
        /// <returns>Returns the new TextBox added</returns>
        public TextBox AddStringFilterTextBox(string label, string columnName)
        {
            _layoutManager.AddControl(_filterInputBoxCollection.AddLabel(label));
            TextBox tb = _filterInputBoxCollection.AddStringFilterTextBox(columnName);
            _layoutManager.AddControl(tb);
            return tb;
        }

        /// <summary>
        /// Adds a ComboBox filter from which the user can choose an option, so that
        /// only rows with that option in the specified column will be shown
        /// </summary>
        /// <param name="label">The label to appear before the ComboBox</param>
        /// <param name="columnName">The column of data on which to do the
        /// filtering</param>
        /// <param name="options">The options to display in the ComboBox,
        /// which will be prefixed with a blank option</param>
        /// <param name="strictMatch">Whether the column should match the entire
        /// string or just contain the string</param>
        /// <returns>Returns the new ComboBox added</returns>
        public ComboBox AddStringFilterComboBox(string label, string columnName, ICollection options, bool strictMatch)
        {
            _layoutManager.AddControl(_filterInputBoxCollection.AddLabel(label));
            ComboBox cb = _filterInputBoxCollection.AddStringFilterComboBox(columnName, options, strictMatch);
            _layoutManager.AddControl(cb);
            return cb;
        }

        /// <summary>
        /// Adds a CheckBox filter that displays only rows whose boolean value
        /// matches the on-off state of the CheckBox. The column of data must
        /// have "true" or "false" as its values (boolean database fields are
        /// usually converted to true/false string values by the Habanero
        /// object manager).
        /// </summary>
        /// <param name="label">The text label to appear next to the CheckBox</param>
        /// <param name="columnName">The column of data on which to do the
        /// filtering</param>
        /// <param name="isChecked">Whether the CheckBox is checked</param>
        /// <returns>Returns the new CheckBox added</returns>
        public CheckBox AddStringFilterCheckBox(string label, string columnName, bool isChecked)
        {
            CheckBox cb = _filterInputBoxCollection.AddBooleanFilterCheckBox(columnName, label, isChecked);
            _layoutManager.AddControl(cb);
            return cb;
        }

        /// <summary>
        /// DEPRECATED.
        /// Adds a date-time picker that filters a date column on the date
        /// chosen by the user.  If a date is chosen by the user, setting the 
        /// filter-greater-than argument to "true" displays only rows with 
        /// dates above or equal to that specified, while "false" will 
        /// show all rows with dates less than or equal to the specified date.
        /// This filter caters only for dates without times.  Rather use
        /// AddDateFilterDateTimePicker, which provides greater flexibility
        /// and accuracy.
        /// </summary>
        /// <param name="label">The label to appear before the editor</param>
        /// <param name="columnName">The column of data on which to do the
        /// filtering</param>
        /// <param name="defaultValue">The default date</param>
        /// <param name="filterGreaterThan">True if all dates above or equal to
        /// the given date should be accepted, or false if all dates below or
        /// equal should be accepted</param>
        /// <returns>Returns the new DateTimePicker added</returns>
        [Obsolete]
        public DateTimePicker AddStringFilterDateTimeEditor(string label, string columnName, object defaultValue,
                                                            bool filterGreaterThan)
        {
            _layoutManager.AddControl(_filterInputBoxCollection.AddLabel(label));
            DateTimePicker picker =
                _filterInputBoxCollection.AddStringFilterDateTimeEditor(columnName, defaultValue, filterGreaterThan);
            _layoutManager.AddControl(picker);
            return picker;
        }

        /// <summary>
        /// Adds a date-time picker that filters a date column on the date
        /// chosen by the user.  The given operator compares the chosen date
        /// with the date shown in the given column name.
        /// </summary>
        /// <param name="label">The label to appear before the editor</param>
        /// <param name="columnName">The column of data on which to do the
        /// filtering</param>
        /// <param name="defaultValue">The default date or null</param>
        /// <param name="filterClauseOperator">The operator used to compare
        /// with the date chosen by the user.  The chosen date is on the
        /// right side of the equation.</param>
        /// <param name="ignoreTime">Sets all times produced by the DateTimePicker
        /// to 12am before comparing dates</param>
        /// <returns>Returns the new DateTimePicker added</returns>
        public DateTimePicker AddDateFilterDateTimePicker(string label, string columnName, object defaultValue,
                                                            FilterClauseOperator filterClauseOperator, bool ignoreTime)
        {
            return AddDateFilterDateTimePicker(label, columnName, defaultValue, filterClauseOperator, ignoreTime, false);
        }

        /// <summary>
        /// Adds a date-time picker that filters a date column on the date
        /// chosen by the user.  The given operator compares the chosen date
        /// with the date shown in the given column name.
        /// </summary>
        /// <param name="label">The label to appear before the editor</param>
        /// <param name="columnName">The column of data on which to do the
        /// filtering</param>
        /// <param name="defaultValue">The default date or null</param>
        /// <param name="filterClauseOperator">The operator used to compare
        /// with the date chosen by the user.  The chosen date is on the
        /// right side of the equation.</param>
        /// <param name="ignoreTime">Sets all times produced by the DateTimePicker
        /// to 12am before comparing dates</param>
        /// <param name="nullable">Must the date time picker be nullable</param>
        /// <returns>Returns the new DateTimePicker added</returns>
        public DateTimePicker AddDateFilterDateTimePicker(string label, string columnName, object defaultValue,
                                                            FilterClauseOperator filterClauseOperator, bool ignoreTime, bool nullable)
        {
            _layoutManager.AddControl(_filterInputBoxCollection.AddLabel(label));
            DateTimePicker picker =
                _filterInputBoxCollection.AddDateFilterDateTimePicker(columnName, defaultValue, filterClauseOperator, ignoreTime, nullable);
            _layoutManager.AddControl(picker);
            return picker;
        }

        /// <summary>
        /// Adds a ComboBox filter from which the user can choose an option, so that
        /// only rows with that option in the specified column will be shown
        /// </summary>
        /// <param name="label">The label to appear before the ComboBox</param>
        /// <param name="columnName">The column of data on which to do the
        /// filtering</param>
        /// <param name="includeStartDate">Includes all dates that match the start
        /// date exactly</param>
        /// <param name="includeEndDate">Includes all dates that match the end
        /// date exactly</param>
        /// <returns>Returns the new ComboBox added</returns>
        public DateRangeComboBox AddDateRangeFilterComboBox(string label, string columnName, bool includeStartDate, bool includeEndDate)
        {
            _layoutManager.AddControl(_filterInputBoxCollection.AddLabel(label));
            DateRangeComboBox cb = _filterInputBoxCollection.AddDateRangeFilterComboBox(columnName, includeStartDate, includeEndDate);
            _layoutManager.AddControl(cb);
            return cb;
        }

        /// <summary>
        /// Adds a ComboBox filter from which the user can choose an option, so that
        /// only rows with that option in the specified column will be shown.  This
        /// overload allows a given collection of DateOptions to be shown.
        /// </summary>
        /// <param name="label">The label to appear before the ComboBox</param>
        /// <param name="columnName">The column of data on which to do the
        /// filtering</param>
        /// <param name="options">The collection of DateOptions to show</param>
        /// <param name="includeStartDate">Includes all dates that match the start
        /// date exactly</param>
        /// <param name="includeEndDate">Includes all dates that match the end
        /// date exactly</param>
        /// <returns>Returns the new ComboBox added</returns>
        public DateRangeComboBox AddDateRangeFilterComboBox(string label, string columnName, List<DateRangeComboBox.DateOptions> options, bool includeStartDate, bool includeEndDate)
        {
            _layoutManager.AddControl(_filterInputBoxCollection.AddLabel(label));
            DateRangeComboBox cb = _filterInputBoxCollection.AddDateRangeFilterComboBox(columnName, options, includeStartDate, includeEndDate);
            _layoutManager.AddControl(cb);
            return cb;
        }

        /// <summary>
        /// Handles the event of a value changed in a filter control
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void FilterControlValueChangedHandler(object sender, EventArgs e)
        {
            FireFilterClauseChanged((Control)sender);
        }

        /// <summary>
        /// Calls the FilterClauseChanged() method
        /// </summary>
        /// <param name="sendingControl">The sending control</param>
        private void FireFilterClauseChanged(Control sendingControl)
        {
            if (this.FilterClauseChanged != null)
            {
                this.FilterClauseChanged(this, new FilterControlEventArgs(sendingControl));
            }
        }

        /// <summary>
        /// Returns the filter clause as a composite of all the specific
        /// clauses in each filter control in the set
        /// </summary>
        /// <returns>Returns the filter clause</returns>
        public IFilterClause GetFilterClause()
        {
            return _filterInputBoxCollection.GetFilterClause();
        }

        /// <summary>
        /// Sets the width of the next filter to be added.  If your filter
        /// controls have variable width, simply change this setting before
        /// you add each control.
        /// </summary>
        /// <param name="width">The width in pixels</param>
        public void SetFilterWidth(int width)
        {
            _filterInputBoxCollection.SetFilterWidth(width);
        }

        /// <summary>
        /// Sets whether automatic updates take place or not.  If so, then
        /// the data is filtered as you type, otherwise the filtering only
        /// takes place when Enter is pressed. The default is true.
        /// </summary>
        /// <param name="auto">Returns true if automatic, false if not</param>
        public void SetAutomaticUpdate(bool auto)
        {
            _filterInputBoxCollection.SetAutomaticUpdate(auto);
        }
    }
}