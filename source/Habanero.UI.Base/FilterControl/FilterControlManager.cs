using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.UI.Base
{
    public class FilterControlManager
    {
        private readonly IControlFactory _controlFactory;
        private readonly IFilterClauseFactory _clauseFactory;
        private readonly List<FilterUI> _filterControls = new List<FilterUI>();

        public FilterControlManager(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
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

            ITextBox textBox = _controlFactory.CreateTextBox();
            _filterControls.Add(new FilterUIString(_clauseFactory, propertyName, textBox));
            return textBox;
        }

        public IComboBox AddStringFilterComboBox(string labelText, string columnName, ICollection options, bool strictMatch)
        {

            IComboBox cb = _controlFactory.CreateComboBox();
            ////cb.Width = _filterWidth;
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
        public ICheckBox AddStringFilterCheckBox(string labelText, string propertyName, bool isChecked)
        {
            ICheckBox cb = _controlFactory.CreateCheckBox();
            ////cb.Width = _filterWidth;
            //_filterControls.Add(new FilterUIStringOptions(_clauseFactory, propertyName, cb, isChecked));
            //cb.Width = _controlFactory.CreateTextBox().Width; ;
            //_filterControls.Add(new FilterUICheckBox(_clauseFactory, propertyName, cb, labelText, isChecked));
            //cb.CheckedChanged += FilterControlValueChangedHandler;
            //FireFilterClauseChanged(cb);
            //_controls.Add(cb);
            return cb;
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

        public void ClearFilters()
        {
            foreach (FilterUI filterUI in _filterControls)
            {
                filterUI.Clear();
            }
        }
    }
}