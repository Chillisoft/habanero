using System;
using System.Collections;
using System.Windows.Forms;
using Chillisoft.Generic.v2;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Manages a collection of filter input controls
    /// </summary>
    /// TODO ERIC - how do filter clauses fit in with input boxes?
    /// review all these comments
    public class FilterInputBoxCollection
    {
        private readonly FilterClauseFactory itsClauseFactory;
        private IList itsFilterUIs;
        private IList itsControls;
        private int itsFilterWidth;
        private bool itsIsAutomaticUpdate = false;

        public event EventHandler FilterClauseChanged;

        /// <summary>
        /// Constructor to initialise a new collection
        /// </summary>
        /// <param name="clauseFactory">The filter clause factory</param>
        public FilterInputBoxCollection(FilterClauseFactory clauseFactory)
        {
            itsFilterUIs = new ArrayList();
            itsClauseFactory = clauseFactory;
            itsControls = new ArrayList(8);
            itsFilterWidth = (new TextBox()).Width;
        }

        /// <summary>
        /// Adds a label to the collection
        /// </summary>
        /// <param name="label">The text to appear in the label</param>
        /// <returns>Returns the new Label object added</returns>
        public Label AddLabel(string label)
        {
            Label labelControl = ControlFactory.CreateLabel(label, false);
            itsControls.Add(labelControl);
            return labelControl;
        }

        /// <summary>
        /// Adds a TextBox that filters strings
        /// </summary>
        /// <param name="columnName">The column name</param>
        /// <returns>Returns the new TextBox added</returns>
        public TextBox AddStringFilterTextBox(string columnName)
        {
            TextBox tb = ControlFactory.CreateTextBox();
            tb.Width = itsFilterWidth;
            itsFilterUIs.Add(new FilterUIString(itsClauseFactory, columnName, tb));
            tb.KeyPress += new KeyPressEventHandler(FilterControlKeyPressedHandler);
            tb.TextChanged += new EventHandler(FilterControlValueChangedHandler);
            FireFilterClauseChanged(tb);
            itsControls.Add(tb);
            return tb;
        }

        /// <summary>
        /// Handles the event where the value in a filter TextBox has changed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        /// TODO ERIC - is that a spelling mistake or is it LeftValue?
        /// if left, then rename to Left
        private void FilterTextBoxlValueChangedHandler(object sender, EventArgs e)
        {
            if (itsIsAutomaticUpdate)
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
        /// Returns the filter clause
        /// </summary>
        /// <returns>Returns the filter clause</returns>
        public FilterClause GetFilterClause()
        {
            FilterUI filterUi = (FilterUI) itsFilterUIs[0];
            FilterClause clause = filterUi.GetFilterClause();
            for (int i = 1; i < itsFilterUIs.Count; i++)
            {
                filterUi = (FilterUI) itsFilterUIs[i];
                clause =
                    itsClauseFactory.CreateCompositeFilterClause(clause, FilterClauseCompositeOperator.OpAnd,
                                                                 filterUi.GetFilterClause());
            }
            return clause;
        }

        /// <summary>
        /// Adds a ComboBox that filters strings
        /// </summary>
        /// <param name="columnName">The column name</param>
        /// <param name="options">The options to display in the ComboBox</param>
        /// <returns>Returns the new ComboBox added</returns>
        public ComboBox AddStringFilterComboBox(string columnName, ICollection options)
        {
            ComboBox cb = ControlFactory.CreateComboBox();
            cb.Width = itsFilterWidth;
            itsFilterUIs.Add(new FilterUIStringOptions(itsClauseFactory, columnName, cb, options));
            cb.SelectedIndexChanged += new EventHandler(FilterControlValueChangedHandler);
            cb.TextChanged += new EventHandler(FilterControlValueChangedHandler);
            FireFilterClauseChanged(cb);
            itsControls.Add(cb);
            return cb;
        }

        /// <summary>
        /// Adds a date-time editor that filters strings
        /// </summary>
        /// <param name="columnName">The column name</param>
        /// <param name="defaultDate">The default date</param>
        /// <param name="filterGreaterThan">Whether to filter greater-than
        /// values</param>
        /// <returns>Returns the new DateTimePicker object added</returns>
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
            dte.Width = itsFilterWidth;

            itsFilterUIs.Add(new FilterUIDateString(itsClauseFactory, columnName, dte, filterGreaterThan));
            dte.ValueChanged += new EventHandler(FilterControlValueChangedHandler);
            FireFilterClauseChanged(dte);
            itsControls.Add(dte);
            return dte;
        }

        /// <summary>
        /// Returns the controls held in the collection
        /// </summary>
        /// <returns>Returns the controls as an IList object</returns>
        public IList GetControls()
        {
            return itsControls;
        }

        /// <summary>
        /// Enables all controls in the collection
        /// </summary>
        public void EnableControls()
        {
            foreach (Control control in itsControls)
            {
                control.Enabled = true;
            }
        }

        /// <summary>
        /// Disables all controls in the collection
        /// </summary>
        public void DisableControls()
        {
            foreach (Control control in itsControls)
            {
                control.Enabled = false;
            }
        }

        /// <summary>
        /// Set the width of the filter
        /// </summary>
        /// <param name="width">The width</param>
        public void SetFilterWidth(int width)
        {
            itsFilterWidth = width;
        }

        /// <summary>
        /// Sets whether automatic updates take place or not
        /// </summary>
        /// <param name="auto">True for automatic updates, false if not</param>
        public void SetAutomaticUpdate(bool auto)
        {
            itsIsAutomaticUpdate = auto;
        }

        /// <summary>
        /// A super-class for user interface elements with filters
        /// </summary>
        private abstract class FilterUI
        {
            protected readonly FilterClauseFactory itsClauseFactory;
            protected readonly string itsColumnName;

            /// <summary>
            /// Constructor to initialise a new instance
            /// </summary>
            /// <param name="clauseFactory">The filter clause factory</param>
            /// <param name="columnName">The column name</param>
            protected FilterUI(FilterClauseFactory clauseFactory, string columnName)
            {
                itsColumnName = columnName;
                itsClauseFactory = clauseFactory;
            }

            /// <summary>
            /// Returns the filter clause
            /// </summary>
            /// <returns>Returns the filter clause</returns>
            public abstract FilterClause GetFilterClause();
        }

        /// <summary>
        /// A filter user interface element for handling strings. Manages
        /// a TextBox object.
        /// </summary>
        private class FilterUIString : FilterUI
        {
            private readonly TextBox itsTextBox;

            public FilterUIString(FilterClauseFactory clauseFactory, string columnName, TextBox textBox)
                : base(clauseFactory, columnName)
            {
                itsTextBox = textBox;
            }

            public override FilterClause GetFilterClause()
            {
                if (itsTextBox.Text.Length > 0)
                {
                    return
                        itsClauseFactory.CreateStringFilterClause(itsColumnName, FilterClauseOperator.OpLike,
                                                                  itsTextBox.Text);
                }
                else
                {
                    return itsClauseFactory.CreateNullFilterClause();
                }
            }
        }

        /// <summary>
        /// A filter user interface element for handling date-formatted strings.
        /// Manages a DateTimePicker object and allows an option to be
        /// specified on whether to consider greater-than operators.
        /// </summary>
        private class FilterUIDateString : FilterUI
        {
            private readonly DateTimePicker itsDateTimePicker;
            private readonly bool itsFilterGreaterThan;

            public FilterUIDateString(FilterClauseFactory clauseFactory, string columnName, DateTimePicker dtp,
                                      bool filterGreaterThan)
                : base(clauseFactory, columnName)
            {
                itsDateTimePicker = dtp;
                this.itsFilterGreaterThan = filterGreaterThan;
            }

            public override FilterClause GetFilterClause()
            {
                if (itsDateTimePicker.Value != null)
                {
                    FilterClauseOperator op;
                    if (itsFilterGreaterThan)
                    {
                        op = FilterClauseOperator.OpGreaterThanOrEqualTo;
                    }
                    else
                    {
                        op = FilterClauseOperator.OpLessThanOrEqualTo;
                    }
                    return
                        itsClauseFactory.CreateStringFilterClause(itsColumnName, op,
                                                                  ((DateTime)itsDateTimePicker.Value).ToString(
                                                                      "yyyy/MM/dd"));
                }
                else
                {
                    return itsClauseFactory.CreateNullFilterClause();
                }
            }
        }

        /// <summary>
        /// A filter user interface element for handling a string list of options
        /// in a ComboBox
        /// </summary>
        private class FilterUIStringOptions : FilterUI
        {
            private readonly ComboBox itsComboBox;

            public FilterUIStringOptions(FilterClauseFactory clauseFactory, string columnName, ComboBox comboBox,
                                         ICollection options)
                : base(clauseFactory, columnName)
            {
                itsComboBox = comboBox;
                itsComboBox.Items.Add("");
                foreach (string optionString in options)
                {
                    itsComboBox.Items.Add(optionString);
                }
            }

            public override FilterClause GetFilterClause()
            {
                if (itsComboBox.SelectedIndex != -1 && itsComboBox.SelectedItem.ToString().Length > 0)
                {
                    return
                        itsClauseFactory.CreateStringFilterClause(itsColumnName, FilterClauseOperator.OpEquals,
                                                                  itsComboBox.SelectedItem.ToString());
                }
                else
                {
                    return itsClauseFactory.CreateNullFilterClause();
                }
            }
        }
    }
}