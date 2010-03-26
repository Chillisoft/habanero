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
using Habanero.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// This allows the developer to set a filter that is always applied and is not modifiable by or visible to the end user.
    /// For example this can be used to show only the Non Archived users.
    /// </summary>
    public class StringStaticFilter : ICustomFilter
    {
        private readonly string _propertyName;
        private readonly FilterClauseOperator _filterClauseOperator;
        private readonly string _constantvalue;

        ///<summary>
        /// Constructor for <see cref="StringStaticFilter"/>
        ///</summary>
        ///<param name="propertyName"></param>
        ///<param name="filterClauseOperator"></param>
        ///<param name="constantvalue">The Constant Value that is being used in this filter</param>
        public StringStaticFilter(string propertyName, FilterClauseOperator filterClauseOperator, string constantvalue)
        {
            _propertyName = propertyName;
            _filterClauseOperator = filterClauseOperator;
            _constantvalue = constantvalue;
        }

        ///<summary>
        /// The control that has been constructed by this Control Manager.
        ///</summary>
        public IControlHabanero Control { get { return null; } }

        ///<summary>
        /// Returns the filter clause for this control
        ///</summary>
        ///<param name="filterClauseFactory"></param>
        ///<returns></returns>
        public IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory) {
            return string.IsNullOrEmpty(_constantvalue)
                       ? filterClauseFactory.CreateNullFilterClause()
                       : filterClauseFactory.CreateStringFilterClause(_propertyName, _filterClauseOperator, _constantvalue);
        }

        ///<summary>
        /// Clears the <see cref="IDateRangeComboBox"/> of its value
        ///</summary>
        public void Clear() { }

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