using System;
using System.Collections;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Ui.Base;

namespace Habanero.Ui.Grid
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
        private FilterInputBoxCollection _filterInputBoxCollection;
        private FlowLayoutManager _layoutManager;
        
        //public event EventHandler FilterClauseChanged;

        public event EventHandler<FilterControlEventArgs> FilterClauseChanged;
        
        /// <summary>
        /// Constructor to initialise a new filter control
        /// </summary>
        /// <param name="clauseFactory">The filter clause factory</param>
        public FilterControl(IFilterClauseFactory clauseFactory)
        {
            _layoutManager = new FlowLayoutManager(this);
            _filterInputBoxCollection = new FilterInputBoxCollection(clauseFactory);
            _filterInputBoxCollection.FilterClauseChanged += new EventHandler(FilterControlValueChangedHandler);
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
        /// <returns>Returns the new ComboBox added</returns>
        public ComboBox AddStringFilterComboBox(string label, string columnName, ICollection options)
        {
            _layoutManager.AddControl(_filterInputBoxCollection.AddLabel(label));
            ComboBox cb = _filterInputBoxCollection.AddStringFilterComboBox(columnName, options);
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
        /// Adds a date-time picker that filters a date column on the date
        /// chosen by the user.  If a date is chosen by the user, setting the 
        /// filter-greater-than argument to "true" displays only rows with 
        /// dates above or equal to that specified, while "false" will 
        /// show all rows with dates less than or equal to the specified date.
        /// </summary>
        /// <param name="label">The label to appear before the editor</param>
        /// <param name="columnName">The column of data on which to do the
        /// filtering</param>
        /// <param name="defaultValue">The default date</param>
        /// <param name="filterGreaterThan">True if all dates above or equal to
        /// the given date should be accepted, or false if all dates below or
        /// equal should be accepted</param>
        /// <returns>Returns the new DateTimePicker added</returns>
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