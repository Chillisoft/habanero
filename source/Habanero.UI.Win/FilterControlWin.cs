using System;
using System.Collections;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;
using Habanero.UI.Base.LayoutManagers;

namespace Habanero.UI.Win
{
    public class FilterControlWin : FlowLayoutPanel, IFilterControl
    {
        private readonly FilterControlManager _filterControlManager;
        //private readonly FlowLayoutManager _flowLayoutManager;

        public FilterControlWin(IControlFactory controlFactory)
        {
            FlowLayoutManager flowLayoutManager = new FlowLayoutManager(this);
            _filterControlManager = new FilterControlManager(controlFactory, flowLayoutManager);

            this.Height = 40;
        }

        public ITextBox AddStringFilterTextBox(string labelText, string propertyName)
        {
            ITextBox textBox = _filterControlManager.AddStringFilterTextBox(labelText, propertyName);
            return textBox;
        }

        public IFilterClause GetFilterClause()
        {
            return _filterControlManager.GetFilterClause();
        }

        IList IControlChilli.Controls
        {
            get { return this.Controls; }
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
        public IDateTimePicker AddDateFilterDateTimePicker(string label, string propertyName, DateTime defaultValue, FilterClauseOperator filterClauseOperator, bool ignoreTime, bool nullable)
        {
            //_layoutManager.AddControl(_filterInputBoxCollection.AddLabel(label));
            //DateTimePicker picker =
            //    _filterInputBoxCollection.AddDateFilterDateTimePicker(columnName, defaultValue, filterClauseOperator, ignoreTime, nullable);
            //_layoutManager.AddControl(picker);
            //return picker;
            IDateTimePicker dtPicker = _filterControlManager.AddDateFilterDateTimePicker(propertyName, defaultValue, filterClauseOperator, ignoreTime, nullable);
            return dtPicker;
        }
    }
}