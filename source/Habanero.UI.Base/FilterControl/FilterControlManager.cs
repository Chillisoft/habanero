// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;

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
        private readonly List<ICustomFilter> _filterControls = new List<ICustomFilter>();
        private LayoutManager _layoutManager;

        ///<summary>
        /// Constructor with controlFactory and layout manager.
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="layoutManager"></param>
        public FilterControlManager(IControlFactory controlFactory, LayoutManager layoutManager)
        {
            _controlFactory = controlFactory;
            _layoutManager = layoutManager;
            _clauseFactory = new DataViewFilterClauseFactory();
        }

        ///<summary>
        /// See <see cref="IFilterControl.CountOfFilters"/>
        ///</summary>
        [Obsolete("Please use FilterControls.Count")]
        public int CountOfFilters
        {
            get { return _filterControls.Count; }
        }

        /// <summary>
        /// See <see cref="IFilterControl.FilterControls"/>
        /// </summary>
        public List<ICustomFilter> FilterControls
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
            ICustomFilter filterUi = _filterControls[0];
            IFilterClause clause = filterUi.GetFilterClause(_clauseFactory);
            for (int i = 1; i < _filterControls.Count; i++)
            {
                filterUi = _filterControls[i];
                clause =
                    _clauseFactory.CreateCompositeFilterClause(clause, FilterClauseCompositeOperator.OpAnd,
                                                               filterUi.GetFilterClause(_clauseFactory));
            }
            return clause;
        }

        /// <summary>
        /// See <see cref="IFilterControl.AddStringFilterTextBox(string,string)"/>
        /// </summary>
        public ICustomFilter AddStringFilterTextBox(string labelText, string propertyName)
        {
            StringTextBoxFilter filter = new StringTextBoxFilter(_controlFactory, propertyName, FilterClauseOperator.OpLike);
            AddCustomFilter(labelText, filter);
            return filter;
        }        
        
        /// <summary>
        /// See <see cref="IFilterControl.AddStringFilterTextBox(string,string)"/>
        /// </summary>
        public ICustomFilter AddMultiplePropStringTextBox(string labelText, List<string> propertyNames)
        {
            MultiplePropStringTextBoxFilter filter = new MultiplePropStringTextBoxFilter(_controlFactory, propertyNames, FilterClauseOperator.OpLike);
            AddCustomFilter(labelText, filter);
            return filter;
        }

        private void AddControlToLayoutManager(IControlHabanero label, IControlHabanero entryControl)
        {
            if (label == null) throw new ArgumentNullException("label");
            if (entryControl == null) throw new ArgumentNullException("entryControl");
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
        public ICustomFilter AddStringFilterTextBox(string labelText, string propertyName, FilterClauseOperator filterClauseOperator)
        {
            StringTextBoxFilter filter = new StringTextBoxFilter(_controlFactory, propertyName, filterClauseOperator);
            AddCustomFilter(labelText,  filter);
            return filter;
        }

        /// <summary>
        /// See <see cref="IFilterControl.AddStringFilterTextBox(string,string,FilterClauseOperator)"/>
        /// </summary>
        public ICustomFilter AddMultiplePropStringTextBox(string labelText, List<string> propertyNames, FilterClauseOperator filterClauseOperator)
        {
            MultiplePropStringTextBoxFilter filter = new MultiplePropStringTextBoxFilter(_controlFactory, propertyNames, filterClauseOperator);
            AddCustomFilter(labelText,  filter);
            return filter;
        }

        /// <summary>
        /// See <see cref="IFilterControl.AddStringFilterComboBox"/>
        /// </summary>
        public ICustomFilter AddStringFilterComboBox(string labelText, string columnName, ICollection options, bool strictMatch)
        {
            StringComboBoxFilter filter = new StringComboBoxFilter(_controlFactory, columnName,
                                                                   strictMatch 
                                                                       ? FilterClauseOperator.OpEquals
                                                                       : FilterClauseOperator.OpLike);

            AddCustomFilter(labelText, filter);
            filter.Options = options;
            return filter;
        }
        /// <summary>
        /// See <see cref="IFilterControl.AddStringFilterComboBox"/>
        /// </summary>
        public ICustomFilter AddEnumFilterComboBox(string labelText, string columnName, Type enumType)
        {
            EnumComboBoxFilter filter = new EnumComboBoxFilter(_controlFactory, columnName, FilterClauseOperator.OpEquals, enumType);
            AddCustomFilter(labelText, filter);
            return filter;
        }

        /// <summary>
        /// See <see cref="IFilterControl.AddBooleanFilterCheckBox"/>
        /// </summary>
        public ICustomFilter AddBooleanFilterCheckBox(string labelText, string propertyName, bool isChecked)
        {
            BoolCheckBoxFilter filter = new BoolCheckBoxFilter(_controlFactory, propertyName, FilterClauseOperator.OpEquals);
            filter.IsChecked = isChecked;
            AddCustomFilter(labelText,  filter);
            return filter;
        }

        /// <summary>
        /// See <see cref="IFilterControl.AddDateFilterDateTimePicker"/>
        /// </summary>
        public ICustomFilter AddDateFilterDateTimePicker(string labelText, string propertyName, FilterClauseOperator filterClauseOperator, DateTime? defaultDate)
        {
            DateTimePickerFilter filter = new DateTimePickerFilter(_controlFactory, propertyName, filterClauseOperator);
            AddCustomFilter(labelText, filter);
            if (defaultDate != null)
            {
                filter.DefaultDate = defaultDate.Value;
            }
            

            return filter;
        }

        /// <summary>
        /// See <see cref="IFilterControl.AddCustomFilter(string,ICustomFilter)"/>
        /// </summary>
        public void AddCustomFilter(string labelText, ICustomFilter customFilter)
        {
            ILabel label = _controlFactory.CreateLabel(labelText);
            IControlHabanero control = customFilter.Control;
            AddControlToLayoutManager(label, control);
           _filterControls.Add(customFilter);
        }

        /// <summary>
        /// See <see cref="IFilterControl.ClearFilters"/>
        /// </summary>
        public void ClearFilters()
        {
            foreach (ICustomFilter filterUI in _filterControls)
            {
                filterUI.Clear();
            }
        }

        /// <summary>
        /// See <see cref="IFilterControl.GetChildControl"/>
        /// </summary>
        public IControlHabanero GetChildControl(string propertyName)
        {
            foreach (ICustomFilter customFilter in this._filterControls)
            {
                if (customFilter.PropertyName == propertyName)
                {
                    return customFilter.Control;
                }
            }
            return null;
        }

        /// <summary>
        /// See <see cref="IFilterControl.AddDateRangeFilterComboBox(string,string,List{DateRangeOptions},bool,bool)"/>
        /// </summary>
        public ICustomFilter AddDateRangeFilterComboBox(string labelText, string propertyName, List<DateRangeOptions> options, bool includeStartDate, bool includeEndDate)
        {
            DateRangeComboBoxFilter filter = new DateRangeComboBoxFilter(_controlFactory, propertyName, FilterClauseOperator.OpEquals);

            if (options !=null) filter.OptionsToDisplay = options;
            filter.IncludeStartDate = includeStartDate;
            filter.IncludeEndDate = includeEndDate;
            AddCustomFilter(labelText, filter);

            return filter;
        }

        ///<summary>
        /// Adds a static string filter <see cref="StringStaticFilter"/> to the Filter Control.
        /// This allows the developer to set a filter that is always applied and is not editable by or visible to the End user.
        ///</summary>
        ///<param name="propertyName"></param>
        ///<param name="filterClauseOperator"></param>
        ///<param name="filterValue"></param>
        public void AddStaticStringFilterClause(string propertyName, FilterClauseOperator filterClauseOperator, string filterValue)
        {
            StringStaticFilter filter = new StringStaticFilter(propertyName, filterClauseOperator, filterValue);
            _filterControls.Add(filter);
        }
    }

    ///<summary>
    /// A Filter controller for a <see cref="IDateRangeComboBox"/> that allows you to select a range of dates e.g. Yesterday, Last week etc
    ///</summary>
    public class DateRangeComboBoxFilter : ICustomFilter
    {
        private readonly IControlFactory _controlFactory;
        private readonly string _propertyName;
        private readonly FilterClauseOperator _filterClauseOperator;
        private readonly IDateRangeComboBox _dateRangeComboBox;
        private bool _includeStartDate = true;
        private bool _includeEndDate = true;

        ///<summary>
        /// Constructor with controlFactory, propertyName and filterClauseOperator
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="propertyName"></param>
        ///<param name="filterClauseOperator"></param>
        public DateRangeComboBoxFilter(IControlFactory controlFactory, string propertyName, FilterClauseOperator filterClauseOperator)
        {
            _controlFactory = controlFactory;
            _propertyName = propertyName;
            _filterClauseOperator = filterClauseOperator;
            _dateRangeComboBox = _controlFactory.CreateDateRangeComboBox();
            _dateRangeComboBox.SelectedIndexChanged += (sender, e) => FireValueChanged();
            _dateRangeComboBox.TextChanged += (sender,e) => FireValueChanged();
        }

        private void FireValueChanged()
        {
            if (ValueChanged != null) this.ValueChanged(this, new EventArgs());
        }
        /// <summary>
        /// Returns the underlying <see cref="IDateRangeComboBox"/> conrol being controlled by the Filter Control
        /// </summary>
        public IControlHabanero Control { get { return _dateRangeComboBox; } }

        ///<summary>
        /// Returns the filter clause for this control
        ///</summary>
        ///<param name="filterClauseFactory"></param>
        ///<returns></returns>
        public IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory) {
            if (_dateRangeComboBox.SelectedIndex > 0)
            {
                FilterClauseOperator op = _includeStartDate ? FilterClauseOperator.OpGreaterThanOrEqualTo : FilterClauseOperator.OpGreaterThan;
                IFilterClause startClause = filterClauseFactory.CreateDateFilterClause(_propertyName, op, _dateRangeComboBox.StartDate);
                op = _includeEndDate
                    ? FilterClauseOperator.OpLessThanOrEqualTo
                    : FilterClauseOperator.OpLessThan;
                IFilterClause endClause = filterClauseFactory.CreateDateFilterClause(_propertyName, op, _dateRangeComboBox.EndDate);

                return
                    filterClauseFactory.CreateCompositeFilterClause(startClause, FilterClauseCompositeOperator.OpAnd,
                                                               endClause);
            }
            return filterClauseFactory.CreateNullFilterClause();
        }
        ///<summary>
        /// Clears the <see cref="IDateRangeComboBox"/>
        ///</summary>
        public void Clear() { _dateRangeComboBox.SelectedIndex = -1;}

        /// <summary>
        /// Event handler that fires when the value in the Filter control changes
        /// </summary>
        public event EventHandler ValueChanged;
        ///<summary>
        /// The name of the property being filtered by.
        ///</summary>
        public string PropertyName { get { return _propertyName; } }
        ///<summary>
        /// Returns the operator <see cref="ICustomFilter.FilterClauseOperator"/> e.g.OpEquals to be used by for creating the Filter Clause.
        ///</summary>
        public FilterClauseOperator FilterClauseOperator { get { return _filterClauseOperator; } }
        ///<summary>
        /// Whether the start date is included in the Filter Clause or not
        ///</summary>
        public bool IncludeStartDate { get { return _includeStartDate; } set { _includeStartDate = value; } }
        ///<summary>
        /// Whether the End date should be included in the filter clause or not.
        ///</summary>
        public bool IncludeEndDate { get { return _includeEndDate; } set { _includeEndDate = value; } }
        ///<summary>
        /// Gets and Sets the List of <see cref="DateRangeOptions"/>.
        ///</summary>
        public List<DateRangeOptions> OptionsToDisplay { get { return _dateRangeComboBox.OptionsToDisplay; } set { _dateRangeComboBox.OptionsToDisplay = value; } }
    }

    ///<summary>
    /// For Filtering a combo box of String.
    ///</summary>
    public class StringComboBoxFilter : ICustomFilter
    {
        private readonly IControlFactory _controlFactory;
        private readonly string _propertyName;
        private readonly FilterClauseOperator _filterClauseOperator;
        private readonly IComboBox _comboBox;

        /// <summary>
        /// Event handler that fires when the value in the Filter control changes
        /// </summary>
        public event EventHandler ValueChanged;

        ///<summary>
        /// Constructor with the controlFactory, propertyName, filterClauseOperator.
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="propertyName"></param>
        ///<param name="filterClauseOperator"></param>
        public StringComboBoxFilter(IControlFactory controlFactory, string propertyName, FilterClauseOperator filterClauseOperator)
        {
            _controlFactory = controlFactory;
            _propertyName = propertyName;
            _filterClauseOperator = filterClauseOperator;
            _comboBox = _controlFactory.CreateComboBox();
            _comboBox.SelectedIndexChanged += (sender, e) => FireValueChanged();
            _comboBox.TextChanged += (sender,e) => FireValueChanged();
        }

        private void FireValueChanged()
        {
            if (ValueChanged != null)
            {
                this.ValueChanged(this, new EventArgs());
            }
        }

        ///<summary>
        /// The control that has been constructed by this Control Manager.
        ///</summary>
        public IControlHabanero Control { get { return _comboBox; } }

        ///<summary>
        /// Returns the filter clause for this control
        ///</summary>
        ///<param name="filterClauseFactory"></param>
        ///<returns></returns>
        public IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory) {
            if (_comboBox.SelectedIndex != -1 && _comboBox.SelectedItem.ToString().Length > 0)
            {
                return
                    filterClauseFactory.CreateStringFilterClause(_propertyName, _filterClauseOperator,
                                                            _comboBox.SelectedItem.ToString());
            }
            return filterClauseFactory.CreateNullFilterClause();
        }

        ///<summary>
        /// Clears the <see cref="IDateRangeComboBox"/> of its value
        ///</summary>
        public void Clear() { _comboBox.SelectedIndex = -1;  }

        ///<summary>
        /// The name of the property being filtered by.
        ///</summary>
        public string PropertyName { get { return _propertyName; } }
        ///<summary>
        /// Returns a collection of Items that can be sellection from the combo box.
        ///</summary>
        public ICollection Options { set {
            _comboBox.Items.Clear();
            _comboBox.Items.Add("");
            foreach (object option in value)
            {
                _comboBox.Items.Add(option);
            }
        } }

        ///<summary>
        /// Returns the operator <see cref="ICustomFilter.FilterClauseOperator"/> e.g.OpEquals to be used by for creating the Filter Clause.
        ///</summary>
        public FilterClauseOperator FilterClauseOperator { get { return _filterClauseOperator; } }
    }

    ///<summary>
    /// A <see cref="ICustomFilter"/> for Filtering using a CheckBox
    ///</summary>
    public class BoolCheckBoxFilter : ICustomFilter
    {
        private readonly IControlFactory _controlFactory;
        private readonly string _propertyName;
        private readonly FilterClauseOperator _filterClauseOperator;
        private readonly ICheckBox _checkBox;

        /// <summary>
        /// Event handler that fires when the value in the Filter control changes
        /// </summary>
        public event EventHandler ValueChanged;

        ///<summary>
        /// A Constructor for the BoolCheckBoxFilter
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="propertyName"></param>
        ///<param name="filterClauseOperator"></param>
        public BoolCheckBoxFilter(IControlFactory controlFactory, string propertyName, FilterClauseOperator filterClauseOperator)
        {
            _controlFactory = controlFactory;
            _propertyName = propertyName;
            _filterClauseOperator = filterClauseOperator;
            _checkBox = _controlFactory.CreateCheckBox();
            _checkBox.Text = "";
            _checkBox.CheckedChanged += (sender, e) => FireValueChanged();
        }

        private void FireValueChanged()
        {
            if (ValueChanged != null)
            {
                this.ValueChanged(this, new EventArgs());
            }
        }
        /// <summary>
        /// Returns the Control that is being used in the filter
        /// </summary>
        public IControlHabanero Control { get { return _checkBox; } }

        ///<summary>
        /// returns true of false Depending Whether the <see cref="ICheckBox"/> is checked or not>
        ///</summary>
        public bool IsChecked { get { return _checkBox.Checked; } set { _checkBox.Checked = value;} }

        ///<summary>
        /// Returns the filter clause for this control
        ///</summary>
        ///<param name="filterClauseFactory"></param>
        ///<returns></returns>
        public IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory) {
            if (_checkBox.Checked)
            {
                return
                    filterClauseFactory.CreateStringFilterClause(_propertyName,
                                                            FilterClauseOperator.OpEquals, "true");
            }
            return
                filterClauseFactory.CreateStringFilterClause(_propertyName,
                                                        FilterClauseOperator.OpEquals, "false");
        }

        ///<summary>
        /// Clears the <see cref="IDateRangeComboBox"/> of its value
        ///</summary>
        public void Clear() { _checkBox.Checked = false; }

        ///<summary>
        /// The name of the property being filtered by.
        ///</summary>
        public string PropertyName { get { return _propertyName; } }

        ///<summary>
        /// Returns the operator <see cref="ICustomFilter.FilterClauseOperator"/> e.g.OpEquals to be used by for creating the Filter Clause.
        ///</summary>
        public FilterClauseOperator FilterClauseOperator { get { return _filterClauseOperator; } }
    }

    ///<summary>
    /// A Custom Filter Control for DateTimePickerFilter.
    ///</summary>
    public class DateTimePickerFilter : ICustomFilter
    {
          private readonly IControlFactory _controlFactory;
        private readonly string _propertyName;
        private readonly FilterClauseOperator _filterClauseOperator;
        private readonly IDateTimePicker _dateTimePicker;
        private DateTime? _defaultDate;

        ///<summary>
        /// An overridden constructor for controlFactory, propertyName and filterClauseOperator.
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="propertyName"></param>
        ///<param name="filterClauseOperator"></param>
        public DateTimePickerFilter(IControlFactory controlFactory, string propertyName, FilterClauseOperator filterClauseOperator)
        {
            _controlFactory = controlFactory;
            _propertyName = propertyName;
            _filterClauseOperator = filterClauseOperator;
            _dateTimePicker = _controlFactory.CreateDateTimePicker();
            _dateTimePicker.ValueChanged += (sender, e) => FireValueChanged();     
        }

        private void FireValueChanged()
        {
            if (ValueChanged != null)
            {
                this.ValueChanged(this, new EventArgs());
            }
        }

        ///<summary>
        /// The control that has been constructed by this Control Manager.
        ///</summary>
        public IControlHabanero Control { get { return _dateTimePicker; } }

        ///<summary>
        /// Returns the filter clause for this control
        ///</summary>
        ///<param name="filterClauseFactory"></param>
        ///<returns></returns>
        public IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory) {
            DateTime date = _dateTimePicker.Value;
            date = date.Date;
            if (_filterClauseOperator == FilterClauseOperator.OpLike)
            {
                IFilterClause startClause = filterClauseFactory.CreateDateFilterClause(
                    _propertyName, FilterClauseOperator.OpGreaterThanOrEqualTo, date);
                IFilterClause endClause = filterClauseFactory.CreateDateFilterClause(
                    _propertyName, FilterClauseOperator.OpLessThan, date.AddDays(1));
                return filterClauseFactory.CreateCompositeFilterClause(
                    startClause, FilterClauseCompositeOperator.OpAnd, endClause);
            }
            return filterClauseFactory.CreateDateFilterClause(_propertyName, _filterClauseOperator, date);
        }

        ///<summary>
        /// Clears the <see cref="IDateRangeComboBox"/> of its value
        ///</summary>
        public void Clear() { _dateTimePicker.ValueOrNull = null; }

        /// <summary>
        /// Event handler that fires when the value in the Filter control changes
        /// </summary>
        public event EventHandler ValueChanged;

        ///<summary>
        /// The name of the property being filtered by.
        ///</summary>
        public string PropertyName { get { return _propertyName; } }
        ///<summary>
        /// Gets and Sets the DefaultDate that is used by the DateTimePicker.
        ///</summary>
        public DateTime? DefaultDate
        {
            get { return _defaultDate; }
            set
            {
                _defaultDate = value;
                _dateTimePicker.ValueOrNull = _defaultDate;
        } }

        ///<summary>
        /// Returns the operator <see cref="ICustomFilter.FilterClauseOperator"/> e.g.OpEquals to be used by for creating the Filter Clause.
        ///</summary>
        public FilterClauseOperator FilterClauseOperator { get { return _filterClauseOperator; } }
    }

    ///<summary>
    /// Provides a means to create custom filters inherits from <see cref="IControlManager"/>
    ///</summary>
    public interface ICustomFilter : IControlManager
    {
        ///<summary>
        /// Returns the filter clause for this control
        ///</summary>
        ///<param name="filterClauseFactory"></param>
        ///<returns></returns>
        IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory);
        ///<summary>
        /// Clears the <see cref="IDateRangeComboBox"/> of its value
        ///</summary>
        void Clear();
        /// <summary>
        /// Event handler that fires when the value in the Filter control changes
        /// </summary>
        event EventHandler ValueChanged;
        ///<summary>
        /// The name of the property being filtered by.
        ///</summary>
        string PropertyName { get; }
        ///<summary>
        /// Returns the operator <see cref="FilterClauseOperator"/> e.g.OpEquals to be used by for creating the Filter Clause.
        ///</summary>
        FilterClauseOperator FilterClauseOperator { get; }
    }

    /// <summary>
    /// A super-class for user interface elements that provide filter clauses
    /// </summary>
    public abstract class FilterUI
    {
        /// <summary>
        /// The <see cref="IFilterClauseFactory"/> used to create the filter clauses.
        /// </summary>
        protected readonly IFilterClauseFactory _clauseFactory;
        /// <summary>
        /// The name of the property (column) that is being used in creating this filter clause.
        /// </summary>
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

        ///<summary>
        /// The Property name being being used in creating the filter clause.
        ///</summary>
        public string PropertyName
        {
            get { return _columnName; }
        }

        ///<summary>
        /// Returns the actual control being controlled
        ///</summary>
        public abstract IControlHabanero FilterControl
        { get; }

        /// <summary>
        /// Returns the filter clause
        /// </summary>
        /// <returns>Returns the filter clause</returns>
        public abstract IFilterClause GetFilterClause();

        ///<summary>
        /// Clears The Filter.
        ///</summary>
        public abstract void Clear();
    }

}