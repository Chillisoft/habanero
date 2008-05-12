using System;
using System.Collections;
using Habanero.Base;

namespace Habanero.UI.Base
{
    public interface IFilterControl : IChilliControl
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
        /// <param name="ignoreTime">Sets all times produced by the DateTimePicker
        /// to 12am before comparing dates</param>
        /// <param name="nullable">Must the date time picker be nullable</param>
        /// <returns>Returns the new DateTimePicker added</returns>
        IDateTimePicker AddDateFilterDateTimePicker(string label, string propertyName, DateTime defaultValue, FilterClauseOperator filterClauseOperator, bool ignoreTime, bool nullable);
    }
}