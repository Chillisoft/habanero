// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
    ///<summary>
    /// A Filter controller for an enum type this allows you set set a comboBox filter for any enum data type.
    ///</summary>
    public class EnumComboBoxFilter : ICustomFilter
    {
        private Type EnumType { get; set; }
        private readonly IControlFactory _controlFactory;
        private readonly string _propertyName;
        private readonly FilterClauseOperator _filterClauseOperator;
        private readonly IComboBox _comboBox;

        ///<summary>
        /// Constructor with controlFactory, propertyName and filterClauseOperator
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="propertyName"></param>
        ///<param name="filterClauseOperator"></param>
        ///<param name="enumType"></param>
        public EnumComboBoxFilter(IControlFactory controlFactory, string propertyName
                , FilterClauseOperator filterClauseOperator, Type enumType)
        {
            EnumType = enumType;
            if (enumType == null) throw new ArgumentNullException("enumType");
            _controlFactory = controlFactory;
            _propertyName = propertyName;
            _filterClauseOperator = filterClauseOperator;
            _comboBox = _controlFactory.CreateComboBox();
            _comboBox.SelectedIndexChanged += (sender, e) => FireValueChanged();
            _comboBox.TextChanged += (sender,e) => FireValueChanged();
            var purchaseOrderStatusCol = Enum.GetValues(enumType);
            Options = purchaseOrderStatusCol;
        }
        ///<summary>
        /// Returns a collection of Items that can be sellection from the combo box.
        ///</summary>
        public ICollection Options
        {
            set
            {
                _comboBox.Items.Clear();
                _comboBox.Items.Add("");
                foreach (object option in value)
                {
                    _comboBox.Items.Add(option);
                }
            }
        }
        private void FireValueChanged()
        {
            if (ValueChanged != null) this.ValueChanged(this, new EventArgs());
        }
        /// <summary>
        /// Returns the underlying <see cref="IComboBox"/> conrol being controlled by the Filter Control
        /// </summary>
        public IControlHabanero Control { get { return _comboBox; } }

        ///<summary>
        /// Returns the filter clause for this control
        ///</summary>
        ///<param name="filterClauseFactory"></param>
        ///<returns></returns>
        public IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory) {
            if (_comboBox.SelectedIndex > 0)
            {
                if (_comboBox.SelectedIndex != -1 && _comboBox.SelectedItem.ToString().Length > 0)
                {
                    var selectedEnum = Enum.Parse(EnumType, _comboBox.SelectedItem.ToString());
                    return
                        filterClauseFactory.CreateEnumFilterClause(_propertyName, _filterClauseOperator,
                                                                selectedEnum);
                }
                return filterClauseFactory.CreateNullFilterClause();
            }
            return filterClauseFactory.CreateNullFilterClause();
        }
        ///<summary>
        /// Clears the <see cref="IComboBox"/>
        ///</summary>
        public void Clear() { _comboBox.SelectedIndex = -1;}

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
    }
}