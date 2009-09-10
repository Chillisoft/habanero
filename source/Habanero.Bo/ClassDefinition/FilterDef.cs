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
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Defines a filter for a grid.  Consists of a set of <see cref="FilterPropertyDef"/>s, and a few settings, such as
    /// FilterMode (can be Filter or Search) and a number of columns that the filter controls will be layed out in
    /// using the ColumnLayoutManager (where 0 means a FlowLayoutManager is used).
    /// </summary>
    public class FilterDef : IFilterDef
    {
        /// <summary>
        /// The standard constructor, which takes the list of <see cref="FilterPropertyDef"/>s defining the fields of the filter.
        /// </summary>
        /// <param name="filterPropertyDefs">The fields defining what to filter on.</param>
        public FilterDef(IList<IFilterPropertyDef> filterPropertyDefs)
        {
            FilterPropertyDefs = filterPropertyDefs;
        }

        /// <summary>
        /// The list of <see cref="FilterPropertyDef"/>s which define each filter.
        /// </summary>
        public IList<IFilterPropertyDef> FilterPropertyDefs { get; set; }

        /// <summary>
        /// The mode of filtering, where <see cref="FilterModes.Filter"/> means to filter already loaded data, and 
        /// <see cref="FilterModes.Search"/> means to search via a query to the data store in the process of loading.
        /// </summary>
        public FilterModes FilterMode { get; set; }

        /// <summary>
        /// The number of columns to layout the filters in.  By default this is 0, and the layout used is that of a flow layout
        /// which simply adds controls until the right side of the container is reached and then moves to the next line.
        /// If this is set to a number greater than zero it will lay out the filter controls in that number of columns.
        /// </summary>
        public int Columns { get; set; }


    }
}