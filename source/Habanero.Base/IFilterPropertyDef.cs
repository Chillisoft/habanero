//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
using System.Collections.Generic;

namespace Habanero.Base
{
    /// <summary>
    /// Interface defining a filter property definition - i.e. a filter field, what property it is filtering on, 
    /// what <see cref="FilterType"/> to use.  This is implemented by <see cref="IFilterPropertyDef"/>.
    /// </summary>
    public interface IFilterPropertyDef
    {
        /// <summary>
        /// The name of the property this filter applies to.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// The text to use to label the filter control.  This shows up as a label to the left of the control.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// The type of filter to use.  This type must implement ICustomFilter.  The default is StringTextBoxFilter
        /// </summary>
        string FilterType { get; set; }

        /// <summary>
        /// The assembly to find the filter class in.  The default is Habanero.UI.Base
        /// </summary>
        string FilterTypeAssembly { get; set; }

        /// <summary>
        /// The operator to use in this filter, for those filters that allow the use of different operators.
        /// </summary>
        FilterClauseOperator FilterClauseOperator { get; set; }

        /// <summary>
        /// A set of parameters to apply to the instantiated filter.  These are applied using reflection.  For instance, if
        /// a parameter is specified in this dictionary called 'AllowNull' with a value of 'true', then once the filter
        /// (which must be of type ICustomFilter) is 
        /// instantiated , the AllowNull property will be set with a value of true via
        /// reflection.
        /// </summary>
        Dictionary<string, string> Parameters { get; set; }
    }
}