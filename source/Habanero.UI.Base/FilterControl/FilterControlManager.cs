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
        private LayoutManager _layoutManager;

        public FilterControlManager(IControlFactory controlFactory, LayoutManager layoutManager)
        {
            _controlFactory = controlFactory;
            _layoutManager = layoutManager;
            _clauseFactory = new DataViewFilterClauseFactory();
        }

        public int CountOfFilters
        {
            get { return _filterControls.Count; }
        }

        public IList FilterControls
        {
            get { return _filterControls; }
        }

        public LayoutManager LayoutManager
        {
            get { return _layoutManager; }
            set { _layoutManager = value; }
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
            AddControlToLayoutManager(label, textBox);
            _filterControls.Add(new FilterUIString(_clauseFactory, propertyName, textBox));
            return textBox;
        }

        private void AddControlToLayoutManager(ILabel label, IControlChilli entryControl)
        {
            _layoutManager.AddControl(label);
            if (_layoutManager is FlowLayoutManager)
            {
                ((FlowLayoutManager)_layoutManager).AddGlue();                
            }
            _layoutManager.AddControl(entryControl);
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
            ILabel label = _controlFactory.CreateLabel(labelText);
            ITextBox textBox = _controlFactory.CreateTextBox();
            AddControlToLayoutManager(label, textBox);
            _filterControls.Add(new FilterUIString(_clauseFactory, propertyName, textBox, filterClauseOperator));
            return textBox;
        }
        public IComboBox AddStringFilterComboBox(string labelText, string columnName, ICollection options, bool strictMatch)
        {

            IComboBox cb = _controlFactory.CreateComboBox();
            ILabel label = _controlFactory.CreateLabel(labelText);
            AddControlToLayoutManager(label, cb);
            _filterControls.Add(new FilterUIStringOptions(_clauseFactory, columnName, cb, options, strictMatch));

            //TODO: Port for windows
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
            _layoutManager.AddControl(cb);
            _filterControls.Add(new FilterUICheckBox(_clauseFactory, propertyName, cb, labelText, isChecked));
            //TODO: Port for windows
            //cb.CheckedChanged += FilterControlValueChangedHandler;
            //FireFilterClauseChanged(cb);
            return cb;
        }

        /// <summary>
        /// Adds a date-time picker that filters a date column on the date
        /// chosen by the user.  The given operator compares the chosen date
        /// with the date shown in the given column name.  The standard
        /// DateTimePicker does not support time picking, so any date supplied
        /// or chosen will have its time values set to zero.
        /// </summary>
        /// <param name="columnName">The name of the date-time column to be 
        /// filtered on</param>
        /// <param name="defaultDate">The default date or null.  The filter clause will
        /// set all times to zero.</param>
        /// <param name="filterClauseOperator">The operator used to compare
        /// with the date chosen by the user.  The chosen date is on the
        /// right side of the equation.</param>
        /// <param name="nullable">Must the date time picker be nullable</param>
        /// <returns>Returns the new DateTimePicker object added</returns>
        /// <remarks>A future improvement could provide another overload where you can
        /// supply a timespan argument that is added onto any date taken from the picker.</remarks>
        public IDateTimePicker AddDateFilterDateTimePicker(string columnName, DateTime defaultDate, FilterClauseOperator filterClauseOperator, bool nullable)
        {
            IDateTimePicker dtPicker = _controlFactory.CreateDateTimePicker();
            _filterControls.Add(new FilterUIDate(_clauseFactory, columnName, dtPicker, filterClauseOperator));
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
            //    _filterUIs.Add(new FilterUIDateNullable(_clauseFactory, columnName, dateTimePickerController, filterClauseOperator));
            //    dateTimePickerController.ValueChanged += delegate(object sender, EventArgs e)
            //                                                 {
            //                                                     FilterControlValueChangedHandler(dte, e);
            //                                                 };
            //}
            //else
            //{
            //    _filterUIs.Add(new FilterUIDate(_clauseFactory, columnName, dte, filterClauseOperator));
            //    dte.ValueChanged += FilterControlValueChangedHandler;
            //}
            //TODO: Port for windows
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

        public IControlChilli GetChildControl(string propertyName)
        {
            foreach (FilterUI filterUI in this._filterControls)
            {
                if (filterUI.PropertyName  == propertyName)
                {
                    return filterUI.FilterControl;
                }
            }
            return null;
        }


        public IDateRangeComboBox AddDateRangeFilterComboBox(string labelText, string columnName, bool includeStartDate, bool includeEndDate)
        {

            IDateRangeComboBox dateRangeComboBox = _controlFactory.CreateDateRangeComboBox();
            ConfigureDateRangeComboBox(labelText, columnName, dateRangeComboBox, includeStartDate, includeEndDate);
            return dateRangeComboBox;

        }


        public IDateRangeComboBox AddDateRangeFilterComboBox(string labelText, string columnName, List<DateRangeOptions> options, bool includeStartDate, bool includeEndDate)
        {
            IDateRangeComboBox dateRangeComboBox = _controlFactory.CreateDateRangeComboBox(options);
            return ConfigureDateRangeComboBox(labelText, columnName, dateRangeComboBox, includeStartDate, includeEndDate);
        }

        /// <summary>
        /// Configures the given DateRangeComboBox and sets up the filter control
        /// </summary>
        private IDateRangeComboBox ConfigureDateRangeComboBox(string labelText, string columnName, IDateRangeComboBox dateRangeComboBox, bool includeStartDate, bool includeEndDate)
        {
            //dateRangeComboBox.Width = _filterWidth;
            ILabel label = _controlFactory.CreateLabel(labelText);
            AddControlToLayoutManager(label, dateRangeComboBox);
            _filterControls.Add(new FilterUIDateRangeString(_clauseFactory, columnName, dateRangeComboBox, includeStartDate, includeEndDate));

            //TODO: Port for windows
            //dateRangeComboBox.SelectedIndexChanged += FilterControlValueChangedHandler;
            //dateRangeComboBox.TextChanged += FilterControlValueChangedHandler;
            //FireFilterClauseChanged(dateRangeComboBox);
            return dateRangeComboBox;
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

            public string PropertyName
            {
                get { return _columnName; }
            }

            public abstract IControlChilli FilterControl
            { get; }

            ///// <summary>
            ///// Returns the filter clause
            ///// </summary>
            ///// <returns>Returns the filter clause</returns>
            public abstract IFilterClause GetFilterClause();

            public abstract void Clear();

            //public abstract IFilterClause GetFilterClause(string stringLikeDelimiter, string dateTimeDelimiter);

        }

        /// <summary>
        /// Manages a TextBox in which the user can type string filter clauses
        /// </summary>
        private class FilterUIString : FilterUI
        {
            private readonly ITextBox _textBox;
            private readonly FilterClauseOperator _filterClauseOperator;

            public FilterUIString(IFilterClauseFactory clauseFactory, string propertyName, ITextBox textBox)
                : this(clauseFactory, propertyName, textBox, FilterClauseOperator.OpLike)
                
            {
                
            }

            public FilterUIString(IFilterClauseFactory clauseFactory, string propertyName, ITextBox textBox, FilterClauseOperator filterClauseOperator)
                : base(clauseFactory, propertyName)
            {
                _textBox = textBox;
                _filterClauseOperator = filterClauseOperator;
            }

            public override IControlChilli FilterControl
            {
                get { return _textBox; }
            }

            public override IFilterClause GetFilterClause()
            {
                if (_textBox.Text.Length > 0)
                {
                    return
                        _clauseFactory.CreateStringFilterClause(_columnName, _filterClauseOperator,
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

            public override IControlChilli FilterControl
            {
                get { return _comboBox; }
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
                _checkBox.Width = _checkBox.Width + text.Length*6;
            }

            public override IControlChilli FilterControl
            {
                get { return _checkBox; }
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

            public FilterUIDate(IFilterClauseFactory clauseFactory, string columnName, IDateTimePicker dtp,
                                      FilterClauseOperator op)
                : base(clauseFactory, columnName)
            {
                _dateTimePicker = dtp;
                _filterClauseOperator = op;
            }

            public override IControlChilli FilterControl
            {
                get { return _dateTimePicker; }
            }

            public override IFilterClause GetFilterClause()
            {
                DateTime date = _dateTimePicker.Value;
                date = date.Date;
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
                //TODO Critical Urgent NNB FIx :
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Manages a Date-Time Picker through which the user can select a date
        /// to serve as either a greater-than or less-than watershed, depending
        /// on the boolean set in the constructor
        /// </summary>
        private class FilterUIDateRangeString : FilterUI
        {
            private readonly IDateRangeComboBox _dateRangeComboBox;
            private readonly bool _filterIncludeStart;
            private readonly bool _filterIncludeEnd;

            public FilterUIDateRangeString(IFilterClauseFactory clauseFactory, string columnName, IDateRangeComboBox dateRangeComboBox,
                                      bool filterIncludeStart, bool filterIncludeEnd)
                : base(clauseFactory, columnName)
            {
                _dateRangeComboBox = dateRangeComboBox;
                _filterIncludeStart = filterIncludeStart;
                _filterIncludeEnd = filterIncludeEnd;
            }

            public override IFilterClause GetFilterClause()
            {
                if (_dateRangeComboBox.SelectedIndex > 0)
                {
                    FilterClauseOperator op;
                    if (_filterIncludeStart)
                    {
                        op = FilterClauseOperator.OpGreaterThanOrEqualTo;
                    }
                    else
                    {
                        op = FilterClauseOperator.OpGreaterThan;
                    }
                    IFilterClause startClause = _clauseFactory.CreateDateFilterClause(_columnName, op, _dateRangeComboBox.StartDate);
                    if (_filterIncludeEnd)
                    {
                        op = FilterClauseOperator.OpLessThanOrEqualTo;
                    }
                    else
                    {
                        op = FilterClauseOperator.OpLessThan;
                    }
                    IFilterClause endClause = _clauseFactory.CreateDateFilterClause(_columnName, op, _dateRangeComboBox.EndDate);

                    return
                        _clauseFactory.CreateCompositeFilterClause(startClause, FilterClauseCompositeOperator.OpAnd,
                                                                   endClause);
                }
                else
                {
                    return _clauseFactory.CreateNullFilterClause();
                }
            }

            public override IControlChilli FilterControl
            {
                get { return _dateRangeComboBox; }
            }

            public override void Clear()
            {
                //TODO Critical Urgent NNB FIx :
               // throw new NotImplementedException();
                _dateRangeComboBox.SelectedIndex = -1;
            }
        }


    }


}