using System;
using System.Collections;
using System.Windows.Forms;
using Chillisoft.Generic.v2;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Manages controls that filter values
    /// </summary>
    /// TODO ERIC - review these comments
    public class FilterControl : UserControl
    {
        private FilterInputBoxCollection itsFilterInputBoxCollection;
        private FlowLayoutManager itsManager;
        
        //public event EventHandler FilterClauseChanged;

        public event EventHandler<FilterControlEventArgs> FilterClauseChanged;
        
        /// <summary>
        /// Constructor to initialise a new filter control
        /// </summary>
        /// <param name="clauseFactory">The filter clause factory</param>
        public FilterControl(FilterClauseFactory clauseFactory)
        {
            itsManager = new FlowLayoutManager(this);
            itsFilterInputBoxCollection = new FilterInputBoxCollection(clauseFactory);
            itsFilterInputBoxCollection.FilterClauseChanged += new EventHandler(FilterControlValueChangedHandler);
            this.Height = new TextBox().Height + 10;
        }

        /// <summary>
        /// Adds a text box that filters strings
        /// </summary>
        /// <param name="label">The label</param>
        /// <param name="columnName">The column name</param>
        /// <returns>Returns the new TextBox added</returns>
        public TextBox AddStringFilterTextBox(string label, string columnName)
        {
            itsManager.AddControl(itsFilterInputBoxCollection.AddLabel(label));
            TextBox tb = itsFilterInputBoxCollection.AddStringFilterTextBox(columnName);
            itsManager.AddControl(tb);
            return tb;
        }

        /// <summary>
        /// Adds a ComboBox that filters strings
        /// </summary>
        /// <param name="label">The label</param>
        /// <param name="columnName">The column name</param>
        /// <param name="options">The list of options to appear</param>
        /// <returns>Returns the new ComboBox added</returns>
        public ComboBox AddStringFilterComboBox(string label, string columnName, ICollection options)
        {
            itsManager.AddControl(itsFilterInputBoxCollection.AddLabel(label));
            ComboBox cb = itsFilterInputBoxCollection.AddStringFilterComboBox(columnName, options);
            itsManager.AddControl(cb);
            return cb;
        }

        /// <summary>
        /// Adds a DateTime editor that filters strings
        /// </summary>
        /// <param name="label">The label</param>
        /// <param name="columnName">The column name</param>
        /// <param name="defaultValue">The default date</param>
        /// <param name="filterGreaterThan">Whether to handle greater-than
        /// operators</param>
        /// <returns>Returns the new DateTimePicker added</returns>
        public DateTimePicker AddStringFilterDateTimeEditor(string label, string columnName, object defaultValue,
                                                            bool filterGreaterThan)
        {
            itsManager.AddControl(itsFilterInputBoxCollection.AddLabel(label));
            DateTimePicker picker =
                itsFilterInputBoxCollection.AddStringFilterDateTimeEditor(columnName, defaultValue, filterGreaterThan);
            itsManager.AddControl(picker);
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
        /// Returns the filter clause
        /// </summary>
        /// <returns>Returns the filter clause</returns>
        public FilterClause GetFilterClause()
        {
            return itsFilterInputBoxCollection.GetFilterClause();
        }

        /// <summary>
        /// Sets the filter width
        /// </summary>
        /// <param name="width">The width to set to</param>
        public void SetFilterWidth(int width)
        {
            itsFilterInputBoxCollection.SetFilterWidth(width);
        }

        /// <summary>
        /// Specifies whether automatic updates are carried out
        /// </summary>
        /// <param name="auto">True if automatic, false if not</param>
        public void SetAutomaticUpdate(bool auto)
        {
            itsFilterInputBoxCollection.SetAutomaticUpdate(auto);
        }
    }
}