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
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.UI.Base.FilterControl
{
    public interface IFilterControl : IControlChilli
    {
        /// <summary>
        /// Adds a TextBox filter in which users can specify text that
        /// a string-value column will be filtered on.  This uses a "like"
        /// operator and accepts any strings that contain the provided clause.
        /// </summary>
        /// <param name="labelText">The label to appear before the TextBox</param>
        /// <param name="propertyName">The column of data on which to do the
        /// filtering</param>
        /// <returns>Returns the new TextBox added</returns>
        ITextBox AddStringFilterTextBox(string labelText, string propertyName);
        /// <summary>
        /// Adds a TextBox filter in which users can specify text that
        /// a string-value column will be filtered on.
        /// </summary>
        /// <param name="labelText">The label to appear before the TextBox</param>
        /// <param name="propertyName">The column of data on which to do the
        /// filtering</param>
        /// <returns>Returns the new TextBox added</returns>
        /// <param name="filterClauseOperator">Operator To Use For the filter clause</param>
        ITextBox AddStringFilterTextBox(string labelText, string propertyName, FilterClauseOperator filterClauseOperator);

        /// <summary>
        /// Returns the filter clause as a composite of all the specific
        /// clauses in each filter control in the set
        /// </summary>
        /// <returns>Returns the filter clause</returns>
        IFilterClause GetFilterClause();

        /// <summary>
        /// Adds a combo box filter control.
        /// </summary>
        /// <param name="labelText"></param>
        /// <param name="propertyName">The property of the business object being filtered</param>
        /// <param name="options">The collection of items used to fill the combo box.</param>
        /// <param name="strictMatch"></param>
        /// <returns></returns>
        IComboBox AddStringFilterComboBox(string labelText, string propertyName, ICollection options, bool strictMatch);

        /// <summary>
        /// Adds a CheckBox filter that displays only rows whose boolean value
        /// matches the on-off state of the CheckBox. The column of data must
        /// have "true" or "false" as its values (boolean database fields are
        /// usually converted to true/false string values by the Habanero
        /// object manager).
        /// </summary>
        /// <param name="labelText">The text label to appear next to the CheckBox</param>
        /// <param name="propertyName">The column of data on which to do the
        /// filtering</param>
        /// <param name="defaultValue">Whether the CheckBox is checked</param>
        /// <returns>Returns the new CheckBox added</returns>
        ICheckBox AddBooleanFilterCheckBox(string labelText, string propertyName, bool defaultValue);

        /// <summary>
        /// Adds a date-time picker that filters a date column on the date
        /// chosen by the user.  The given operator compares the chosen date
        /// with the date shown in the given column name.
        /// </summary>
        /// <param name="label">The label to appear before the editor</param>
        /// <param name="propertyName">The column of data on which to do the
        /// filtering</param>
        /// <param name="defaultValue">The default date or null</param>
        /// <param name="filterClauseOperator">The operator used to compare
        /// with the date chosen by the user.  The chosen date is on the
        /// right side of the equation.</param>
        /// <param name="nullable">Must the date time picker be nullable</param>
        /// <returns>Returns the new DateTimePicker added</returns>
        IDateTimePicker AddDateFilterDateTimePicker(string label, string propertyName, DateTime defaultValue, FilterClauseOperator filterClauseOperator, bool nullable);

        /// <summary>
        /// The event that is fired with the filter is ready so that another control e.g. a grid can be filtered.
        /// </summary>
        event EventHandler Filter;

        /// <summary>
        ///Applies the filter that has been captured.
        ///This allows an external control e.g. another button click to be used as the event that causes the filter to fire.
        ///Typically used when the filter controls are being set manually
        /// </summary>
        void ApplyFilter();

        /// <summary>
        /// The header text that will be set above the filter defaults to 'Filter'
        /// </summary>
        string HeaderText { get; set;}

        /// <summary>
        /// The number of controls used for filtering that are on the filter control. <see cref="FilterControls"/>
        /// </summary>
        int CountOfFilters { get; }

        /// <summary>
        /// Returns the filter button (this is the button that when clicked applies the filter.
        /// </summary>
        IButton FilterButton { get; }

        /// <summary>
        /// Returns the clear button (this is the button that when clicked clears the filter.
        /// </summary>
        IButton ClearButton
        {
            get;
        }

        /// <summary>
        /// gets and sets the FilterMode <see cref="FilterModes"/>
        /// </summary>
        FilterModes FilterMode { get; set; }

        /// <summary>
        /// returns a collection of the controls used for filtering i.e. the textbox, combobox. This list excludes the labels etc.
        /// </summary>
        IList FilterControls { get; }


        /// <summary>
        /// returns the control used for filtering i.e. the textbox, combobox.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        IControlChilli GetChildControl(string propertyName);

        /// <summary>
        /// Clears all the values from the filter and calls <see cref="ApplyFilter"/>
        /// </summary>
        void ClearFilters();

        /// <summary>
        /// returns the layout manager used to lay the controls out on the filter control panel.
        /// The default layout manager is the FlowLayoutManager.
        /// </summary>
        LayoutManager LayoutManager { get; set; }

        /// <summary>
        /// returns the panel onto which the controls that will be used for filtering will be placed.
        /// </summary>
        IPanel FilterPanel { get; }




        /// <summary>
        /// Adds a ComboBox filter from which the user can choose an option, so that
        /// only rows with that option in the specified column will be shown
        /// </summary>
        /// <param name="labelText">The label to appear before the ComboBox</param>
        /// <param name="columnName">The column of data on which to do the
        /// filtering</param>
        /// <param name="includeStartDate">Includes all dates that match the start
        /// date exactly</param>
        /// <param name="includeEndDate">Includes all dates that match the end
        /// date exactly</param>
        /// <returns>Returns the new ComboBox added</returns>
        IDateRangeComboBox AddDateRangeFilterComboBox(string labelText, string columnName, bool includeStartDate,
                                                      bool includeEndDate);


        IDateRangeComboBox AddDateRangeFilterComboBox(string labelText, string columnName, List<DateRangeOptions> options, bool includeStartDate, bool includeEndDate);
    }
}
