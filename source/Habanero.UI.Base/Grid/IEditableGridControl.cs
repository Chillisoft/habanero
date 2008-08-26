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

using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides a combination of editable grid, filter and buttons used to edit a
    /// collection of business objects
    /// </summary>
    public interface IEditableGridControl : IGridControl
    {
        /// <summary>
        /// Sets the business object collection to display.  Loading of
        /// the collection needs to be done before it is assigned to the
        /// grid.  This method assumes a default UI definition is to be
        /// used, that is a 'ui' element without a 'name' attribute.
        /// </summary>
        /// <param name="boCollection">The new business object collection
        /// to be shown in the grid</param>
        void SetBusinessObjectCollection(IBusinessObjectCollection boCollection);

        /// <summary>
        /// Gets the buttons control used to save and cancel changes
        /// </summary>
        IEditableGridButtonsControl Buttons { get; }

        /// <summary>
        /// Gets the filter control used to filter which rows are shown in the grid
        /// </summary>
        IFilterControl FilterControl { get; }

        /// <summary>
        /// Gets and sets the filter modes for the grid (i.e. filter or search). See <see cref="FilterModes"/>.
        /// </summary>
        FilterModes FilterMode { get; set; }

    
    }
}