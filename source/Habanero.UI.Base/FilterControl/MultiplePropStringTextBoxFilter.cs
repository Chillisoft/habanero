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
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// A filter that comprises a TextBox and filters on a string type.
    /// </summary>
    public class MultiplePropStringTextBoxFilter : ICustomFilter
    {
        private readonly IControlFactory _controlFactory;
        private readonly List<string> _propertyNames;
        private readonly FilterClauseOperator _filterClauseOperator;
        private readonly ITextBox _textBox;

        ///<summary>
        /// Constructor for <see cref="MultiplePropStringTextBoxFilter"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="propertyNames"></param>
        ///<param name="filterClauseOperator"></param>
        public MultiplePropStringTextBoxFilter(IControlFactory controlFactory, List<string> propertyNames,
                                               FilterClauseOperator filterClauseOperator)
        {
            _controlFactory = controlFactory;
            _propertyNames = propertyNames;
            _propertyNames.Sort((s, s1) => s1.CompareTo(s));
            _filterClauseOperator = filterClauseOperator;
            _textBox = _controlFactory.CreateTextBox();
            _textBox.TextChanged += (sender, e) => FireValueChanged();
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
        public IControlHabanero Control
        {
            get { return _textBox; }
        }

        ///<summary>
        /// Returns the filter clause for this control
        ///</summary>
        ///<param name="filterClauseFactory"></param>
        ///<returns></returns>
        public IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory)
        {
            string filterValue = _textBox.Text;
            if (String.IsNullOrEmpty(filterValue)) return filterClauseFactory.CreateNullFilterClause();
            IFilterClause currentClause = filterClauseFactory.CreateStringFilterClause(_propertyNames[0], _filterClauseOperator, filterValue);
            for (int i = 1; i < _propertyNames.Count; i++)
            {
                IFilterClause rightClause = filterClauseFactory.CreateStringFilterClause(_propertyNames[i], _filterClauseOperator, filterValue);
                currentClause = filterClauseFactory.CreateCompositeFilterClause(currentClause, FilterClauseCompositeOperator.OpOr, rightClause);
            }
            return currentClause;
        }

        ///<summary>
        /// Clears the <see cref="IDateRangeComboBox"/> of its value
        ///</summary>
        public void Clear()
        {
            _textBox.Text = "";
        }

        /// <summary>
        /// Event handler that fires when the value in the Filter control changes
        /// </summary>
        public event EventHandler ValueChanged;

        ///<summary>
        /// The name of the property being filtered by.
        ///</summary>
        public string PropertyName
        {
            get
            {
                string propertyName = "";
                string name = propertyName;
                _propertyNames.ForEach(s => name = s + "/" + name);
                propertyName = name.Remove(name.Length - 1);
                return propertyName;
            }
        }

        ///<summary>
        /// Returns the operator <see cref="ICustomFilter.FilterClauseOperator"/> e.g.OpEquals to be used by for creating the Filter Clause.
        ///</summary>
        public FilterClauseOperator FilterClauseOperator
        {
            get { return _filterClauseOperator; }
        }
    }
}