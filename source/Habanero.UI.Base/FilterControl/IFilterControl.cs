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

        IComboBox AddStringFilterComboBox(string labelText, string columnName, ICollection options, bool strictMatch);

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

        string HeaderText { get; set;}

        int CountOfFilters { get; }

        IButton FilterButton { get; }

        FilterModes FilterMode { get; set; }

        IList FilterControls { get; }
        IControlChilli GetChildControl(string propertyName);


        void ClearFilters();
    }
}
