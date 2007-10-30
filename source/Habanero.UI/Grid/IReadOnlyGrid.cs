//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.Grid
{
    /// <summary>
    /// Handles the event of a user double-clicking on a row in the grid
    /// </summary>
    /// <param name="sender">The object that notified of the event</param>
    /// <param name="e">Attached arguments regarding the event</param>
    public delegate void RowDoubleClickedHandler(Object sender, BOEventArgs e);

    /// <summary>
    /// An interface to model a grid that cannot be edited directly
    /// </summary>
    public interface IReadOnlyGrid
    {
        /// <summary>
        /// Gets and sets the currently selected business object
        /// </summary>
        BusinessObject SelectedBusinessObject { set; get; }
        
        /// <summary>
        /// Adds a business object to the collection being represented
        /// </summary>
        /// <param name="bo">The business object to add</param>
        void AddBusinessObject(BusinessObject bo);

        /// <summary>
        /// The event of a row being double-clicked
        /// </summary>
        event RowDoubleClickedHandler RowDoubleClicked;

        /// <summary>
        /// Returns the name of the ui definition used, as specified in the
        /// 'name' attribute of the 'ui' element in the class definitions.
        /// By default, no 'name' attribute is specified and the ui name of
        /// "default" is used.  Having a name attribute allows you to choose
        /// between a multiple visual representations of a business object
        /// collection.
        /// </summary>
        /// <returns>Returns the name of the ui definition this grid is using
        /// </returns>
        string UIName { get; }
    }
}