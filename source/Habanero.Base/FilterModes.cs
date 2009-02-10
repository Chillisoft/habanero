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
using System.Collections.Generic;
using System.Text;

namespace Habanero.Base
{
    /// <summary>
    /// Provides filter modes that can be set up for a grid.
    /// The default options is FilterModes.Filter.
    /// </summary>
    public enum FilterModes
    {
        /// <summary>
        /// Hides rows in a loaded collection that do not meet the filter criteria.  This is a
        /// preferred option if the size of the unfiltered collection is not expected to cause a
        /// deterioration in the performance of the system.
        /// </summary>
        Filter,
        /// <summary>
        /// Reloads the collection shown in the grid, using the criteria as set by the filter.
        /// This is a useful option if the collection in the grid is potentially large.
        /// </summary>
        Search
    }
}
