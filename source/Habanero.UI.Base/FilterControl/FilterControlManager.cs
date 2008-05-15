using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.UI.Base;

namespace Habanero.UI.Base.FilterControl
{
    public class FilterControlManager
    {
        private readonly IControlFactory _controlFactory;
        private readonly IFilterClauseFactory _clauseFactory;
        private readonly List<FilterUI> _filterControls = new List<FilterUI>();
        private readonly FlowLayoutManager _flowLayoutManager;
        public FilterControlManager(IControlFactory controlFactory, FlowLayoutManager flowLayoutManager )
        {
            _controlFactory = controlFactory;
            _flowLayoutManager = flowLayoutManager;
            _clauseFactory = new DataViewFilterClauseFactory();
        }

        public ITextBox AddTextBox()
        {
            ITextBox tb = _controlFactory.CreateTextBox();
            return tb;
        }

        public IFilterClause GetFilterClause()
        {
            if (_filterControls.Count == 0) return _clauseFactory.CreateNullFilterClause();
            FilterUI filterUi = _filterControls[0];
            IFilterClause clause = filterUi.GetFilterClause();
            for (int i = 1; i < _filterControls.Count; i++)
            {
                filterUi = _filterControls[i];
                clause =
                    _clauseFactory.CreateCompositeFilterClause(clause, FilterClauseCompositeOperator.OpAnd,
                                                               filterUi.GetFilterClause());
            }
            return clause;

        }

        public ITextBox AddStringFilterTextBox(string labelText, string propertyName)
        {
            ILabel label = _controlFactory.CreateLabel(labelText);
            ITextBox textBox = _controlFactory.CreateTextBox();
            _flowLayoutManager.AddControl(label);
            _flowLayoutManager.AddGlue();
            _flowLayoutManager.AddControl(textBox);
            _filterControls.Add(new FilterUIString(_clauseFactory, propertyName, textBox));
            return textBox;
        }

        public IComboBox AddStringFilterComboBox(string labelText, string columnName, ICollection options, bool strictMatch)
        {

            IComboBox cb = _controlFactory.CreateComboBox();
            ////cb.Width = _filterWidth;
            ILabel label = _controlFactory.CreateLabel(labelText);
            _flowLayoutManager.AddControl(label);
            _flowLayoutManager.AddGlue();
            _flowLayoutManager.AddControl(cb);
            _filterControls.Add(new FilterUIStringOptions(_clauseFactory, columnName, cb, options, strictMatch));


            ////cb.SelectedIndexChanged += FilterControlValueChangedHandler;
            ////cb.TextChanged += FilterControlValueChangedHandler;
            ////FireFilterClauseChanged(cb);
            return cb;
        }

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
        /// <param name="isChecked">Whether the CheckBox is checked</param>
        /// <returns>Returns the new CheckBox added</returns>
        public ICheckBox AddBooleanFilterCheckBox(string labelText, string propertyName, bool isChecked)
        {
            ICheckBox cb = _controlFactory.CreateCheckBox();
            ////cb.Width = _filterWidth;
            //_filterControls.Add(new FilterUIStringOptions(_clauseFactory, propertyName, cb, isChecked));
            //cb.Width = _controlFactory.CreateTextBox().Width; ;
            _flowLayoutManager.AddControl(cb);
            _filterControls.Add(new FilterUICheckBox(_clauseFactory, propertyName, cb, labelText, isChecked));
            //cb.CheckedChanged += FilterControlValueChangedHandler;
            //FireFilterClauseChanged(cb);
            return cb;
        }

        /// <summary>
        /// Adds a date-time picker that filters a date column on the date
        /// chosen by the user.  The given operator compares the chosen date
        /// with the date shown in the given column name.
        /// </summary>
        /// <param name="columnName">The name of the date-time column to be 
        /// filtered on</param>
        /// <param name="defaultDate">The default date or null</param>
        /// <param name="filterClauseOperator">The operator used to compare
        /// with the date chosen by the user.  The chosen date is on the
        /// right side of the equation.</param>
        /// <param name="ignoreTime">Sets all times produced by the DateTimePicker
        /// to 12am before comparing dates</param>
        /// <param name="nullable">Must the date time picker be nullable</param>
        /// <returns>Returns the new DateTimePicker object added</returns>
        public IDateTimePicker AddDateFilterDateTimePicker(string columnName, DateTime defaultDate, FilterClauseOperator filterClauseOperator, bool ignoreTime, bool nullable)
        {
            IDateTimePicker dtPicker = _controlFactory.CreateDateTimePicker();
            _filterControls.Add(new FilterUIDate(_clauseFactory, columnName, dtPicker, filterClauseOperator, ignoreTime));
            dtPicker.Value = defaultDate;
            return dtPicker;
            //if (defaultDate == null)
            //{
            //    dte = (IDateTimePicker)ControlFactory.CreateDateTimePicker();
            //}
            //else
            //{
            //    dte = (IDateTimePicker)ControlFactory.CreateDateTimePicker((DateTime)defaultDate);
            //}
            //dte.Width = _filterWidth;
            //if (nullable)
            //{
            //    DateTimePickerController dateTimePickerController = new DateTimePickerController(dte);
            //    _filterUIs.Add(new FilterUIDateNullable(_clauseFactory, columnName, dateTimePickerController, filterClauseOperator, ignoreTime));
            //    dateTimePickerController.ValueChanged += delegate(object sender, EventArgs e)
            //                                                 {
            //                                                     FilterControlValueChangedHandler(dte, e);
            //                                                 };
            //}
            //else
            //{
            //    _filterUIs.Add(new FilterUIDate(_clauseFactory, columnName, dte, filterClauseOperator, ignoreTime));
            //    dte.ValueChanged += FilterControlValueChangedHandler;
            //}
            //FireFilterClauseChanged(dte);
            //_controls.Add(dte);
            //return dte;
        }



        /// <summary>
        /// Clears all the filters on the filter control (sets to null where it can or to the
        /// default value where null cannot be set (e.g. checkBox)
        /// </summary>
        public void ClearFilters()
        {
            foreach (FilterUI filterUI in _filterControls)
            {
                filterUI.Clear();
            }
        }

        /// <summary>
        /// A super-class for user interface elements that provide filter clauses
        /// </summary>
        private abstract class FilterUI
        {
            protected readonly IFilterClauseFactory _clauseFactory;
            protected readonly string _columnName;

            /// <summary>
            /// Constructor to initialise a new instance
            /// </summary>
            /// <param name="clauseFactory">The filter clause factory</param>
            /// <param name="columnName">The column name</param>
            protected FilterUI(IFilterClauseFactory clauseFactory, string columnName)
            {
                if (clauseFactory == null) throw new ArgumentNullException("clauseFactory");
                _columnName = columnName;
                _clauseFactory = clauseFactory;
            }

            /// <summary>
            /// Returns the filter clause
            /// </summary>
            /// <returns>Returns the filter clause</returns>
            public abstract IFilterClause GetFilterClause();

            public abstract void Clear();
       
        }

        /// <summary>
        /// Manages a TextBox in which the user can type string filter clauses
        /// </summary>
        private class FilterUIString : FilterUI
        {
            private readonly ITextBox _textBox;

            public FilterUIString(IFilterClauseFactory clauseFactory, string columnName, ITextBox textBox)
                : base(clauseFactory, columnName)
            {
                _textBox = textBox;
            }

            public override IFilterClause GetFilterClause()
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

            public override void Clear()
            {
                _textBox.Text = "";
            }
        }

        /// <summary>
        /// Manages a ComboBox from which the user can select a string option
        /// on which values are filtered
        /// </summary>
        private class FilterUIStringOptions : FilterUI
        {
            private readonly IComboBox _comboBox;
            private readonly bool _strictMatch;

            public FilterUIStringOptions(IFilterClauseFactory clauseFactory, string columnName, IComboBox comboBox,
                                         ICollection options, bool strictMatch)
                : base(clauseFactory, columnName)
            {
                if (comboBox == null) throw new ArgumentNullException("comboBox");
                if (options == null) throw new ArgumentNullException("options");
                _comboBox = comboBox;
                _comboBox.Items.Add("");
                foreach (object option in options)
                {
                    _comboBox.Items.Add(option);
                }
                _strictMatch = strictMatch;
            }

            public override IFilterClause GetFilterClause()
            {
                if (_comboBox.SelectedIndex != -1 && _comboBox.SelectedItem.ToString().Length > 0)
                {
                    FilterClauseOperator op;
                    if (_strictMatch) op = FilterClauseOperator.OpEquals;
                    else op = FilterClauseOperator.OpLike;
                    return
                        _clauseFactory.CreateStringFilterClause(_columnName, op,
                                                                _comboBox.SelectedItem.ToString());
                }
                else
                {
                    return _clauseFactory.CreateNullFilterClause();
                }
            }

            public override void Clear()
            {
                _comboBox.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Manages a CheckBox which the user can select to filter boolean
        /// values
        /// </summary>
        private class FilterUICheckBox : FilterUI
        {
            private readonly ICheckBox _checkBox;
            private readonly bool _isChecked;

            public FilterUICheckBox(IFilterClauseFactory clauseFactory, string columnName, ICheckBox checkBox,
                                    string text, bool isChecked)
                : base(clauseFactory, columnName)
            {
                _checkBox = checkBox;
                _isChecked = isChecked;
                _checkBox.Checked = isChecked;
                _checkBox.Text = text;
            }

            public override IFilterClause GetFilterClause()
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

            public override void Clear()
            {
                //Reset the check box to its default value
                _checkBox.Checked = _isChecked;
            }
 
        }
        /// <summary>
        /// Provides a filter clause from a given DateTime Picker, using the
        /// column name and filter clause operator provided
        /// </summary>
        private class FilterUIDate : FilterUI
        {
            private readonly IDateTimePicker _dateTimePicker;
            private readonly FilterClauseOperator _filterClauseOperator;
            private readonly bool _ignoreTime;

            public FilterUIDate(IFilterClauseFactory clauseFactory, string columnName, IDateTimePicker dtp,
                                      FilterClauseOperator op, bool ignoreTime)
                : base(clauseFactory, columnName)
            {
                _dateTimePicker = dtp;
                _filterClauseOperator = op;
                _ignoreTime = ignoreTime;
            }

            public override IFilterClause GetFilterClause()
            {
                DateTime date = _dateTimePicker.Value;
                if (_ignoreTime)
                {
                    date = date.Date;
                }
                if (_filterClauseOperator == FilterClauseOperator.OpLike)
                {
                    IFilterClause startClause = _clauseFactory.CreateDateFilterClause(
                        _columnName, FilterClauseOperator.OpGreaterThanOrEqualTo, date);
                    IFilterClause endClause = _clauseFactory.CreateDateFilterClause(
                        _columnName, FilterClauseOperator.OpLessThan, date.AddDays(1));
                    return _clauseFactory.CreateCompositeFilterClause(
                        startClause, FilterClauseCompositeOperator.OpAnd, endClause);
                }
                else
                {
                    return _clauseFactory.CreateDateFilterClause(_columnName, _filterClauseOperator, date);
                }
            }

            public override void Clear()
            {
                throw new NotImplementedException();
            }
        }
    }
}