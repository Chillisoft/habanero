//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.UI.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// This manager groups common logic for IFilterControl objects.
    /// Do not use this object in working code - rather call CreateFilterControl
    /// in the appropriate control factory.
    /// </summary>
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

        /// <summary>
        /// See <see cref="IFilterControl.CountOfFilters"/>
        /// </summary>
        public int CountOfFilters
        {
            get { return _filterControls.Count; }
        }

        /// <summary>
        /// See <see cref="IFilterControl.FilterControls"/>
        /// </summary>
        public IList FilterControls
        {
            get { return _filterControls; }
        }

        /// <summary>
        /// See <see cref="IFilterControl.LayoutManager"/>
        /// </summary>
        public LayoutManager LayoutManager
        {
            get { return _layoutManager; }
            set { _layoutManager = value; }
        }

        /// <summary>
        /// See <see cref="IFilterControl.GetFilterClause"/>
        /// </summary>
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

        /// <summary>
        /// See <see cref="IFilterControl.AddStringFilterTextBox(string,string)"/>
        /// </summary>
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
        /// See <see cref="IFilterControl.AddStringFilterTextBox(string,string,FilterClauseOperator)"/>
        /// </summary>
        public ITextBox AddStringFilterTextBox(string labelText, string propertyName, FilterClauseOperator filterClauseOperator)
        {
            ILabel label = _controlFactory.CreateLabel(labelText);
            ITextBox textBox = _controlFactory.CreateTextBox();
            AddControlToLayoutManager(label, textBox);
            _filterControls.Add(new FilterUIString(_clauseFactory, propertyName, textBox, filterClauseOperator));
            return textBox;
        }

        /// <summary>
        /// See <see cref="IFilterControl.AddStringFilterComboBox"/>
        /// </summary>
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
        /// See <see cref="IFilterControl.AddBooleanFilterCheckBox"/>
        /// </summary>
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
        /// See <see cref="IFilterControl.AddDateFilterDateTimePicker"/>
        /// </summary>
        public IDateTimePicker AddDateFilterDateTimePicker(string columnName, FilterClauseOperator filterClauseOperator, bool nullable, DateTime defaultDate)
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
        /// See <see cref="IFilterControl.ClearFilters"/>
        /// </summary>
        public void ClearFilters()
        {
            foreach (FilterUI filterUI in _filterControls)
            {
                filterUI.Clear();
            }
        }

        /// <summary>
        /// See <see cref="IFilterControl.GetChildControl"/>
        /// </summary>
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

        /// <summary>
        /// See <see cref="IFilterControl.AddDateRangeFilterComboBox(string,string,bool,bool)"/>
        /// </summary>
        public IDateRangeComboBox AddDateRangeFilterComboBox(string labelText, string columnName, bool includeStartDate, bool includeEndDate)
        {

            IDateRangeComboBox dateRangeComboBox = _controlFactory.CreateDateRangeComboBox();
            ConfigureDateRangeComboBox(labelText, columnName, dateRangeComboBox, includeStartDate, includeEndDate);
            return dateRangeComboBox;

        }

        /// <summary>
        /// See <see cref="IFilterControl.AddDateRangeFilterComboBox(string,string,List{T},bool,bool)"/>
        /// </summary>
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
                return _textBox.Text.Length > 0 
                    ? _clauseFactory.CreateStringFilterClause(_columnName, _filterClauseOperator,_textBox.Text) 
                    : _clauseFactory.CreateNullFilterClause();
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
                    FilterClauseOperator op = _strictMatch ? FilterClauseOperator.OpEquals : FilterClauseOperator.OpLike;
                    return
                        _clauseFactory.CreateStringFilterClause(_columnName, op,
                                                                _comboBox.SelectedItem.ToString());
                }
                return _clauseFactory.CreateNullFilterClause();
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
                return
                    _clauseFactory.CreateStringFilterClause(_columnName,
                                                            FilterClauseOperator.OpEquals, "false");
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
                return _clauseFactory.CreateDateFilterClause(_columnName, _filterClauseOperator, date);
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
                    FilterClauseOperator op = _filterIncludeStart ? FilterClauseOperator.OpGreaterThanOrEqualTo : FilterClauseOperator.OpGreaterThan;
                    IFilterClause startClause = _clauseFactory.CreateDateFilterClause(_columnName, op, _dateRangeComboBox.StartDate);
                    op = _filterIncludeEnd 
                        ? FilterClauseOperator.OpLessThanOrEqualTo 
                        : FilterClauseOperator.OpLessThan;
                    IFilterClause endClause = _clauseFactory.CreateDateFilterClause(_columnName, op, _dateRangeComboBox.EndDate);

                    return
                        _clauseFactory.CreateCompositeFilterClause(startClause, FilterClauseCompositeOperator.OpAnd,
                                                                   endClause);
                }
                return _clauseFactory.CreateNullFilterClause();
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