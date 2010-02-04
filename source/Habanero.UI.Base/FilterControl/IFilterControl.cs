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
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.UI.Base
{


    /// <summary>
    /// Manages a group of filter controls that create a filter clause used to limit
    /// which rows of data to show on a DataGridView
    /// </summary>
    public interface IFilterControl : IControlHabanero
    {
        /// <summary>
        /// Adds a TextBox filter in which users can specify text that
        /// a string-value column will be filtered on.  This uses a "like"
        /// operator and accepts any strings that contain the provided clause.
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyName">The business object property on which to filter</param>
        /// <returns>Returns the new TextBox added</returns>
        ITextBox AddStringFilterTextBox(string labelText, string propertyName);

        /// <summary>
        /// Adds a TextBox filter in which users can specify text that
        /// a string-value column will be filtered on.
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyName">The business object property on which to filter</param>
        /// <param name="filterClauseOperator">The operator to use for the filter clause</param>
        /// <returns>Returns the new TextBox added</returns>
        ITextBox AddStringFilterTextBox(string labelText, string propertyName, FilterClauseOperator filterClauseOperator);

        /// <summary>
        /// Adds a TextBox filter in which users can specify text that
        /// multiple string-value properties will be filtered on. This list of properties is declared
        /// in propertyNames.  This uses a "like"
        /// operator and accepts any strings that contain the provided clause.
        /// <seealso cref="AddMultiplePropStringTextBox(string, List{string}, FilterClauseOperator)" />
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyNames">The business object properties on which to filter</param>
        /// <returns>Returns the new TextBox added</returns>
        ITextBox AddMultiplePropStringTextBox(string labelText, List<string> propertyNames);

        /// <summary>
        /// Adds a TextBox filter in which users can specify text that
        /// multiple string-value columns will be filtered on.
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyNames">The business object propertys on which to filter</param>
        /// <param name="filterClauseOperator">The operator to use for the filter clause</param>
        /// <returns>Returns the new TextBox added</returns>
        ITextBox AddMultiplePropStringTextBox(string labelText, List<string> propertyNames, FilterClauseOperator filterClauseOperator);

        /// <summary>
        /// Returns the filter clause as a composite of all the specific
        /// clauses in each filter control in the set
        /// </summary>
        /// <returns>Returns the filter clause</returns>
        IFilterClause GetFilterClause();

        /// <summary>
        /// Adds a ComboBox filter control
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyName">The business object property on which to filter</param>
        /// <param name="options">The collection of items used to fill the combo box.</param>
        /// <param name="strictMatch">Whether to filter the DataGridView column on a strict match or using a LIKE operator</param>
        /// <returns>Returns the new ComboBox added</returns>
        IComboBox AddStringFilterComboBox(string labelText, string propertyName, ICollection options, bool strictMatch);


        ///<summary>
        /// Adds a Filter Combo box for filtering an enum data type.
        ///</summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyName">The business object property on which to filter</param>
        ///<param name="enumType">The Type being showen in the ComboBox</param>
        ///<returns></returns>
        IComboBox AddEnumFilterComboBox(string labelText, string propertyName, Type enumType);

        /// <summary>
        /// Adds a ComboBox filter that displays only rows whose boolean value
        /// matches the true/false or null value in the ComboBox. The column (propertyName) of data must
        /// have "true" or "false" as its values (boolean database fields are
        /// usually converted to true/false string values by the Habanero
        /// object manager).
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyName">The business object property on which to filter</param>
        /// <param name="defaultValue">Whether the CheckBox is checked</param>
        /// <returns>Returns the new CheckBox added</returns>
        IComboBox AddBooleanFilterComboBox(string labelText, string propertyName, bool? defaultValue);

        /// <summary>
        /// Adds a CheckBox filter that displays only rows whose boolean value
        /// matches the on-off state of the CheckBox. The column of data must
        /// have "true" or "false" as its values (boolean database fields are
        /// usually converted to true/false string values by the Habanero
        /// object manager).
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyName">The business object property on which to filter</param>
        /// <param name="defaultValue">Whether the CheckBox is checked</param>
        /// <returns>Returns the new CheckBox added</returns>
        ICheckBox AddBooleanFilterCheckBox(string labelText, string propertyName, bool defaultValue);

        /// <summary>
        /// Adds a date-time picker that filters a date column on the date
        /// chosen by the user.  The given operator compares the chosen date
        /// with the date shown in the given column name.
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="propertyName">The business object property on which to filter</param>
        /// <param name="defaultValue">The default date or null</param>
        /// <param name="filterClauseOperator">The operator used to compare
        /// with the date chosen by the user.  The chosen date is on the
        /// right side of the equation.</param>
        /// <param name="nullable">Whether the datetime picker allows null values</param>
        /// <returns>Returns the new DateTimePicker added</returns>
        IDateTimePicker AddDateFilterDateTimePicker(string labelText, string propertyName, DateTime? defaultValue, FilterClauseOperator filterClauseOperator, bool nullable);

        /// <summary>
        /// The event that is fired with the filter is ready so that another control e.g. a grid can be filtered.
        /// </summary>
        event EventHandler Filter;

        /// <summary>
        /// Applies the filter that has been captured.
        /// This allows an external control (e.g. another button click) to be used as the event that causes the filter to fire.
        /// Typically used when the filter controls are being set manually.
        /// </summary>
        void ApplyFilter();

        /// <summary>
        /// The header text that will be set above the filter.  Defaults to 'Filter'.
        /// </summary>
        string HeaderText { get; set;}

        /// <summary>
        /// The number of controls used for filtering that are on the filter control. <see cref="FilterControls"/>
        /// </summary>
        [Obsolete("Please use FilterControls.Count")]
        int CountOfFilters { get; }

        /// <summary>
        /// Returns the filter button that when clicked applies the filter
        /// </summary>
        IButton FilterButton { get; }

        /// <summary>
        /// Returns the clear button that when clicked clears the filter
        /// </summary>
        IButton ClearButton { get; }

        /// <summary>
        /// Gets and sets the FilterMode <see cref="FilterModes"/>, which determines the
        /// behaviour of the filter control
        /// /// If the <see cref="FilterModes.Search"/> mode is chosen, the loading procedures can be customised
        /// by using an alternate loading mechanism (see <see cref="GridLoaderDelegate"/>).
        /// </summary>
        FilterModes FilterMode { get; set; }

        /// <summary>
        /// Gets the collection of individual filters
        /// </summary>
        List<ICustomFilter> FilterControls { get; }

        /// <summary>
        /// Returns the filter control used to filter the column for the given property name
        /// </summary>
        /// <param name="propertyName">The property name on the business object</param>
        IControlHabanero GetChildControl(string propertyName);

        /// <summary>
        /// Clears all the values from the filter and calls <see cref="ApplyFilter"/>
        /// </summary>
        void ClearFilters();

        /// <summary>
        /// Returns the layout manager used to lay the controls out on the filter control panel.
        /// The default layout manager is the FlowLayoutManager.
        /// </summary>
        LayoutManager LayoutManager { get; set; }

        /// <summary>
        /// Returns the panel onto which the filter controls will be placed
        /// </summary>
        IPanel FilterPanel { get; }

        /// <summary>
        /// Adds a DateRangeComboBox filter which provides common date ranges such as "Today" or "This Year",
        /// so that the grid will only show rows having a date property in the given range
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="columnName">The business object property on which to filter</param>
        /// <param name="includeStartDate">Includes all dates that match the start date exactly</param>
        /// <param name="includeEndDate">Includes all dates that match the end date exactly</param>
        /// <returns>Returns the new DateRangeComboBox added</returns>
        IDateRangeComboBox AddDateRangeFilterComboBox(string labelText, string columnName, bool includeStartDate,
                                                      bool includeEndDate);

        /// <summary>
        /// Adds a DateRangeComboBox filter which provides common date ranges such as "Today" or "This Year",
        /// so that the grid will only show rows having a date property in the given range
        /// </summary>
        /// <param name="labelText">The label to appear before the control</param>
        /// <param name="columnName">The business object property on which to filter</param>
        /// <param name="options">Provides a specific set of date range options to show</param>
        /// <param name="includeStartDate">Includes all dates that match the start date exactly</param>
        /// <param name="includeEndDate">Includes all dates that match the end date exactly</param>
        /// <returns>Returns the new DateRangeComboBox added</returns>
        IDateRangeComboBox AddDateRangeFilterComboBox(string labelText, string columnName, List<DateRangeOptions> options, bool includeStartDate, bool includeEndDate);


        ///<summary>
        /// Adds a custom filter which allows filtering using an ICustomFilter 
        ///</summary>
        ///<param name="labelText">The Label to appear before the control</param>
        ///<param name="propertyName">The property of the Business Object to filter</param>
        ///<param name="customFilter">The custom filter</param>
        ///<returns>Returns the new Custom Filter Control </returns>
        [Obsolete("Please use the overload without the propertyName parameter")]
        IControlHabanero AddCustomFilter(string labelText,string propertyName, ICustomFilter customFilter);

        ///<summary>
        /// Adds a custom filter which allows filtering using an ICustomFilter 
        ///</summary>
        ///<param name="labelText">The Label to appear before the control</param>
        ///<param name="customFilter">The custom filter</param>
        void AddCustomFilter(string labelText, ICustomFilter customFilter);

        /// <summary>
        /// Removes the default Click Event. 
        /// Enables the developer to add custom functionality on the button click 
        /// before the filter or search happens.
        /// </summary>
        void RemoveDefaultFilterClickEvent();

        /// <summary>
        /// Adds the default Click Event. 
        /// Enables the developer to add the default functionality back on the button click 
        /// if previously removed.
        /// </summary>
        void AddDefaultFilterClickEvent();

        /// <summary>
        /// Removes the default Click Event. 
        /// Enables the developer to add custom functionality on the button click 
        /// before the Clear or search happens.
        /// </summary>
        void RemoveDefaultClearClickEvent();

        /// <summary>
        /// Adds the default Click Event. 
        /// Enables the developer to add the default functionality back on the button click 
        /// if previously removed.
        /// </summary>
        void AddDefaultClearClickEvent();

        ///<summary>
        /// Adds a static string filter <see cref="StringStaticFilter"/> to the Filter Control.
        /// This allows the developer to set a filter that is always applied and is not modifiable by or visible to the end user.
        ///</summary>
        ///<param name="propertyName"></param>
        ///<param name="filterClauseOperator"></param>
        ///<param name="filterValue"></param>
        void AddStaticStringFilterClause(string propertyName, FilterClauseOperator filterClauseOperator, string filterValue);
    }
}
