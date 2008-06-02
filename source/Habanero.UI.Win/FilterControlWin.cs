using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;

namespace Habanero.UI.Win
{
    // TODO: move this into FilterControl directory like Giz version
    public class FilterControlWin : Panel, IFilterControl
    {
        public event EventHandler Filter;
        private readonly FilterControlManager _filterControlManager;
        private FilterModes _filterModes;
        private FilterModes _filterMode;
        //private readonly FlowLayoutManager _flowLayoutManager;

        public FilterControlWin(IControlFactory controlFactory)
        {
            FlowLayoutManager flowLayoutManager = new FlowLayoutManager(this, controlFactory);
            _filterControlManager = new FilterControlManager(controlFactory, flowLayoutManager);

            this.Height = 40;
        }

        public ITextBox AddStringFilterTextBox(string labelText, string propertyName)
        {
            ITextBox textBox = _filterControlManager.AddStringFilterTextBox(labelText, propertyName);
            return textBox;
        }

        /// <summary>
        /// Adds a TextBox filter in which users can specify text that
        /// a string-value column will be filtered on.
        /// </summary>
        /// <param name="labelText">The label to appear before the TextBox</param>
        /// <param name="propertyName">The column of data on which to do the
        /// filtering</param>
        /// <returns>Returns the new TextBox added</returns>
        /// <param name="filterClauseOperator">Operator To Use For the filter clause</param>
        public ITextBox AddStringFilterTextBox(string labelText, string propertyName, FilterClauseOperator filterClauseOperator)
        {
            return _filterControlManager.AddStringFilterTextBox(labelText, propertyName, filterClauseOperator); ;
        }

        public IFilterClause GetFilterClause()
        {
            return _filterControlManager.GetFilterClause();
        }

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        public IComboBox AddStringFilterComboBox(string labelText, string columnName, ICollection options,
                                                 bool strictMatch)
        {
            IComboBox comboBox =
                _filterControlManager.AddStringFilterComboBox(labelText, columnName, options, strictMatch);
            return comboBox;
        }

        public ICheckBox AddBooleanFilterCheckBox(string labelText, string propertyName, bool defaultValue)
        {
            ICheckBox checkBox = _filterControlManager.AddBooleanFilterCheckBox(labelText, propertyName, defaultValue);
            return checkBox;
        }

        /// <summary>
        /// Adds a date-time picker that filters a date column on the date
        /// chosen by the user.  The given operator compares the chosen date
        /// with the date shown in the given column name.  The standard
        /// DateTimePicker does not support time picking, so any date supplied
        /// or chosen will have its time values set to zero.
        /// </summary>
        /// <param name="label">The label to appear before the editor</param>
        /// <param name="propertyName">The column of data on which to do the
        /// filtering</param>
        /// <param name="defaultValue">The default date or null.  The filter clause will
        /// set all times to zero.</param>
        /// <param name="filterClauseOperator">The operator used to compare
        /// with the date chosen by the user.  The chosen date is on the
        /// right side of the equation.</param>
        /// <param name="nullable">Must the date time picker be nullable</param>
        /// <returns>Returns the new DateTimePicker added</returns>
        public IDateTimePicker AddDateFilterDateTimePicker(string label, string propertyName, DateTime defaultValue, FilterClauseOperator filterClauseOperator, bool nullable)
        {
            //_layoutManager.AddControl(_filterInputBoxCollection.AddLabel(label));
            //DateTimePicker picker =
            //    _filterInputBoxCollection.AddDateFilterDateTimePicker(columnName, defaultValue, filterClauseOperator, ignoreTime, nullable);
            //_layoutManager.AddControl(picker);
            //return picker;
            IDateTimePicker dtPicker = _filterControlManager.AddDateFilterDateTimePicker(propertyName, defaultValue, filterClauseOperator, nullable);
            return dtPicker;
        }

        /// <summary>
        ///Applies the filter that has been captured.
        ///This allows an external control e.g. another button click to be used as the event that causes the filter to fire.
        ///Typically used when the filter controls are being set manually
        /// </summary>
        public void ApplyFilter()
        {
            throw new NotImplementedException("not implemented on win");
        }

        public string HeaderText
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int CountOfFilters
        {
            get { throw new NotImplementedException(); }
        }

        public IButton FilterButton
        {
            get { throw new NotImplementedException(); }
        }

        public FilterModes FilterMode
        {
            get { return _filterMode; }
            set { _filterMode = value; }
        }

        public IList FilterControls
        {
            get {return this._filterControlManager.FilterControls; }
        }

        public IControlChilli GetChildControl(string propertyName)
        {
            return this._filterControlManager.GetChildControl(propertyName);
        }

        public void ClearFilters()
        {
            throw new NotImplementedException();
        }
    }
}
