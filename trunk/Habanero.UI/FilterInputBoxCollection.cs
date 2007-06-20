using System;
using System.Collections;
using System.Windows.Forms;
using Habanero.Generic;

namespace Habanero.Ui.Generic
{
    /// <summary>
    /// Manages a collection of filter input controls, that allow rows of
    /// data to be filtered to display only data that complies with some
    /// setting in the controls (eg. a filter clause text-box could allow the
    /// user to type in "Smith" and cause all rows without "Smith" in the
    /// surname column to be hidden).
    /// <br/><br/>
    /// This collection can consist of any number of filter controls - the
    /// data will be filtered based on a criteria composed of all the individual
    /// controls' filter clauses.
    /// <br/><br/>
    /// FilterInputBoxCollection is very similar to FilterControl, except that
    /// the latter is itself a control which manages the items in a layout, whereas
    /// this class is just a collection of controls, and you would need to place
    /// the controls in the user interface yourself.  Once you have added the
    /// controls use the GetControls()[] indexing facility to gain access to
    /// each control in the collection
    /// <br/><br/>
    /// To determine the visual width of each individual control, call 
    /// SetFilterWidth just before adding each control.
    /// <br/><br/>
    /// The automatic-updates setting determines whether filtering takes place
    /// as you type (into a text-box, for example) or only when you commit the
    /// change (by pressing Enter, for example).
    /// </summary>
    public class FilterInputBoxCollection
    {
        private readonly FilterClauseFactory _clauseFactory;
        private IList _filterUIs;
        private IList _controls;
        private int _filterWidth;
        private bool _isAutomaticUpdate = true;

        public event EventHandler FilterClauseChanged;

        /// <summary>
        /// Constructor to initialise a new collection
        /// </summary>
        /// <param name="clauseFactory">The filter clause factory</param>
        public FilterInputBoxCollection(FilterClauseFactory clauseFactory)
        {
            _filterUIs = new ArrayList();
            _clauseFactory = clauseFactory;
            _controls = new ArrayList(8);
            _filterWidth = (new TextBox()).Width;
        }

        /// <summary>
        /// Adds a label to be displayed in the filter control collection. This
        /// is a convenient way of indicating to the user what the subsequent
        /// filter control is for.
        /// </summary>
        /// <param name="label">The text to appear in the label</param>
        /// <returns>Returns the new Label object added</returns>
        public Label AddLabel(string label)
        {
            Label labelControl = ControlFactory.CreateLabel(label, false);
            _controls.Add(labelControl);
            return labelControl;
        }

        /// <summary>
        /// Adds a TextBox filter in which users can specify text that
        /// a string-value column will be filtered on.  This uses a "like"
        /// operator and accepts any strings that contain the provided clause.
        /// </summary>
        /// <param name="columnName">The name of the string data column to be 
        /// filtered on</param>
        /// <returns>Returns the new TextBox added</returns>
        public TextBox AddStringFilterTextBox(string columnName)
        {
            TextBox tb = ControlFactory.CreateTextBox();
            tb.Width = _filterWidth;
            _filterUIs.Add(new FilterUIString(_clauseFactory, columnName, tb));
            tb.KeyPress += new KeyPressEventHandler(FilterControlKeyPressedHandler);
            tb.TextChanged += new EventHandler(FilterControlValueChangedHandler);
            FireFilterClauseChanged(tb);
            _controls.Add(tb);
            return tb;
        }

        /// <summary>
        /// Handles the event where the value in a filter TextBox has changed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        /// TODO ERIC - this is not used
        private void FilterTextBoxValueChangedHandler(object sender, EventArgs e)
        {
            if (_isAutomaticUpdate)
            {
                FireFilterClauseChanged(sender);
            }
        }
        
        /// <summary>
        /// Handles the event where a key has been pressed in a filter control
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void FilterControlKeyPressedHandler(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                FireFilterClauseChanged(sender);
            }
        }

        /// <summary>
        /// Handles the event where a value has been changed in a filter control
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void FilterControlValueChangedHandler(object sender, EventArgs e)
        {
            FireFilterClauseChanged(sender);
        }

        /// <summary>
        /// Calls the FilterClauseChanged() method, providing the attached sender
        /// </summary>
        /// <param name="sender">The sending object</param>
        private void FireFilterClauseChanged(object sender)
        {
            if (this.FilterClauseChanged != null)
            {
                this.FilterClauseChanged(sender, new EventArgs());
            }
        }

        /// <summary>
        /// Returns the filter clause as a composite of all the specific
        /// clauses in each filter control in the set
        /// </summary>
        /// <returns>Returns the composite filter clause</returns>
        public FilterClause GetFilterClause()
        {
            FilterUI filterUi = (FilterUI) _filterUIs[0];
            FilterClause clause = filterUi.GetFilterClause();
            for (int i = 1; i < _filterUIs.Count; i++)
            {
                filterUi = (FilterUI) _filterUIs[i];
                clause =
                    _clauseFactory.CreateCompositeFilterClause(clause, FilterClauseCompositeOperator.OpAnd,
                                                                 filterUi.GetFilterClause());
            }
            return clause;
        }

        /// <summary>
        /// Adds a ComboBox filter from which the user can choose an option, so that
        /// only rows with that option in the specified column will be shown
        /// </summary>
        /// <param name="columnName">The name of the data column to be 
        /// filtered on</param>
        /// <param name="options">The options to display in the ComboBox,
        /// which will be prefixed with a blank option</param>
        /// <returns>Returns the new ComboBox added</returns>
        public ComboBox AddStringFilterComboBox(string columnName, ICollection options)
        {
            ComboBox cb = ControlFactory.CreateComboBox();
            cb.Width = _filterWidth;
            _filterUIs.Add(new FilterUIStringOptions(_clauseFactory, columnName, cb, options));
            cb.SelectedIndexChanged += new EventHandler(FilterControlValueChangedHandler);
            cb.TextChanged += new EventHandler(FilterControlValueChangedHandler);
            FireFilterClauseChanged(cb);
            _controls.Add(cb);
            return cb;
        }

        /// <summary>
        /// Adds a CheckBox filter that displays only rows whose boolean value
        /// matches the on-off state of the CheckBox. The column of data must
        /// have "true" or "false" as its values (boolean database fields are
        /// usually converted to true/false string values by the Habanero
        /// object manager).
        /// </summary>
        /// <param name="columnName">The name of the boolean column to be 
        /// filtered on</param>
        /// <param name="text">The text label to appear next to the checkbox</param>
        /// <param name="isChecked">Whether the CheckBox is checked</param>
        /// <returns>Returns the new CheckBox added</returns>
        public CheckBox AddBooleanFilterCheckBox(string columnName, string text, bool isChecked)
        {
            CheckBox cb = ControlFactory.CreateCheckBox();
            cb.Width = _filterWidth;
            _filterUIs.Add(new FilterUICheckBox(_clauseFactory, columnName, cb, text, isChecked));
            cb.CheckedChanged += new EventHandler(FilterControlValueChangedHandler);
            FireFilterClauseChanged(cb);
            _controls.Add(cb);
            return cb;
        }

        /// <summary>
        /// Adds a date-time picker that filters a date column on the date
        /// chosen by the user.  If a date is chosen by the user, setting the 
        /// filter-greater-than argument to "true" displays only rows with 
        /// dates above or equal to that specified, while "false" will 
        /// show all rows with dates less than or equal to the specified date.
        /// </summary>
        /// <param name="columnName">The name of the date-time column to be 
        /// filtered on</param>
        /// <param name="defaultDate">The default date</param>
        /// <param name="filterGreaterThan">True if all dates above or equal to
        /// the given date should be accepted, or false if all dates below or
        /// equal should be accepted</param>
        /// <returns>Returns the new DateTimePicker object added</returns>
        /// TODO ERIC - if user wants to show exact date, can they use this or
        /// should they use a text-box filter? (discuss this issue in the summary)
        /// - if changed, amend FilterControl too
        public DateTimePicker AddStringFilterDateTimeEditor(string columnName, Object defaultDate,
                                                            bool filterGreaterThan)
        {
            DateTimePicker dte;
            if (defaultDate == null)
            {
                dte = (DateTimePicker) ControlFactory.CreateDateTimePicker();
            }
            else
            {
                dte = (DateTimePicker) ControlFactory.CreateDateTimePicker((DateTime) defaultDate);
            }
            dte.Width = _filterWidth;

            _filterUIs.Add(new FilterUIDateString(_clauseFactory, columnName, dte, filterGreaterThan));
            dte.ValueChanged += new EventHandler(FilterControlValueChangedHandler);
            FireFilterClauseChanged(dte);
            _controls.Add(dte);
            return dte;
        }

        /// <summary>
        /// Returns the controls held in the collection of filters
        /// </summary>
        /// <returns>Returns the controls as an IList object</returns>
        public IList GetControls()
        {
            return _controls;
        }

        /// <summary>
        /// Enables all controls in the collection
        /// </summary>
        public void EnableControls()
        {
            foreach (Control control in _controls)
            {
                control.Enabled = true;
            }
        }

        /// <summary>
        /// Disables all controls in the collection
        /// </summary>
        public void DisableControls()
        {
            foreach (Control control in _controls)
            {
                control.Enabled = false;
            }
        }

        /// <summary>
        /// Sets the width of the next filter to be added.  If your filter
        /// controls have variable width, simply change this setting before
        /// you add each control.
        /// </summary>
        /// <param name="width">The width in pixels</param>
        public void SetFilterWidth(int width)
        {
            _filterWidth = width;
        }

        /// <summary>
        /// Returns the current width setting to be used for the next control
        /// added
        /// </summary>
        /// <returns>Returns the width setting in pixels</returns>
        public int GetFilterWidth()
        {
            return _filterWidth;
        }

        /// <summary>
        /// Sets whether automatic updates take place or not.  If so, then
        /// the data is filtered as you type, otherwise the filtering only
        /// takes place when Enter is pressed. The default is true.
        /// </summary>
        /// <param name="auto">True for automatic updates, false if not</param>
        public void SetAutomaticUpdate(bool auto)
        {
            _isAutomaticUpdate = auto;
        }

        /// <summary>
        /// Indicates whether automatic updates take place. See
        /// SetAutomaticUpdates for more detail.
        /// </summary>
        /// <returns>Returns true if automatic</returns>
        public bool GetAutomaticUpdate()
        {
            return _isAutomaticUpdate;
        }

        /// <summary>
        /// A super-class for user interface elements that provide filter clauses
        /// </summary>
        private abstract class FilterUI
        {
            protected readonly FilterClauseFactory _clauseFactory;
            protected readonly string _columnName;

            /// <summary>
            /// Constructor to initialise a new instance
            /// </summary>
            /// <param name="clauseFactory">The filter clause factory</param>
            /// <param name="columnName">The column name</param>
            protected FilterUI(FilterClauseFactory clauseFactory, string columnName)
            {
                _columnName = columnName;
                _clauseFactory = clauseFactory;
            }

            /// <summary>
            /// Returns the filter clause
            /// </summary>
            /// <returns>Returns the filter clause</returns>
            public abstract FilterClause GetFilterClause();
        }

        /// <summary>
        /// Manages a TextBox in which the user can type string filter clauses
        /// </summary>
        private class FilterUIString : FilterUI
        {
            private readonly TextBox _textBox;

            public FilterUIString(FilterClauseFactory clauseFactory, string columnName, TextBox textBox)
                : base(clauseFactory, columnName)
            {
                _textBox = textBox;
            }

            public override FilterClause GetFilterClause()
            {
                if (_textBox.Text.Length > 0)
                {
                    return
                        _clauseFactory.CreateStringFilterClause(_columnName, FilterClauseOperator.OpLike,
                                                                  _textBox.Text);
                }
                else
                {
                    return _clauseFactory.CreateNullFilterClause();
                }
            }
        }

        /// <summary>
        /// Manages a Date-Time Picker through which the user can select a date
        /// to serve as either a greater-than or less-than watershed, depending
        /// on the boolean set in the constructor
        /// </summary>
        private class FilterUIDateString : FilterUI
        {
            private readonly DateTimePicker _dateTimePicker;
            private readonly bool _filterGreaterThan;

            public FilterUIDateString(FilterClauseFactory clauseFactory, string columnName, DateTimePicker dtp,
                                      bool filterGreaterThan)
                : base(clauseFactory, columnName)
            {
                _dateTimePicker = dtp;
                this._filterGreaterThan = filterGreaterThan;
            }

            public override FilterClause GetFilterClause()
            {
                if (_dateTimePicker.Value != null)
                {
                    FilterClauseOperator op;
                    if (_filterGreaterThan)
                    {
                        op = FilterClauseOperator.OpGreaterThanOrEqualTo;
                    }
                    else
                    {
                        op = FilterClauseOperator.OpLessThanOrEqualTo;
                    }
                    return
                        _clauseFactory.CreateStringFilterClause(_columnName, op,
                                                                  ((DateTime)_dateTimePicker.Value).ToString(
                                                                      "yyyy/MM/dd"));
                }
                else
                {
                    return _clauseFactory.CreateNullFilterClause();
                }
            }
        }

        /// <summary>
        /// Manages a ComboBox from which the user can select a string option
        /// on which values are filtered
        /// </summary>
        private class FilterUIStringOptions : FilterUI
        {
            private readonly ComboBox _comboBox;

            public FilterUIStringOptions(FilterClauseFactory clauseFactory, string columnName, ComboBox comboBox,
                                         ICollection options)
                : base(clauseFactory, columnName)
            {
                _comboBox = comboBox;
                _comboBox.Items.Add("");
                foreach (string optionString in options)
                {
                    _comboBox.Items.Add(optionString);
                }
            }

            public override FilterClause GetFilterClause()
            {
                if (_comboBox.SelectedIndex != -1 && _comboBox.SelectedItem.ToString().Length > 0)
                {
                    return
                        _clauseFactory.CreateStringFilterClause(_columnName, FilterClauseOperator.OpEquals,
                                                                  _comboBox.SelectedItem.ToString());
                }
                else
                {
                    return _clauseFactory.CreateNullFilterClause();
                }
            }
        }

        /// <summary>
        /// Manages a CheckBox which the user can select to filter boolean
        /// values
        /// </summary>
        private class FilterUICheckBox : FilterUI
        {
            private readonly CheckBox _checkBox;

            public FilterUICheckBox(FilterClauseFactory clauseFactory, string columnName, CheckBox checkBox,
                                         string text, bool isChecked)
                : base(clauseFactory, columnName)
            {
                _checkBox = checkBox;
                _checkBox.Checked = isChecked;
                _checkBox.Text = text;
            }

            public override FilterClause GetFilterClause()
            {
                if (_checkBox.Checked != null)
                {
                    if (_checkBox.Checked)
                    {
                        return
                            _clauseFactory.CreateStringFilterClause(_columnName,
                                                                       FilterClauseOperator.OpEquals, "true");
                    }
                    else
                    {
                        return
                            _clauseFactory.CreateStringFilterClause(_columnName,
                                                                       FilterClauseOperator.OpEquals, "false");
                    }
                }
                else
                {
                    return _clauseFactory.CreateNullFilterClause();
                }
            }
        }
    }
}
