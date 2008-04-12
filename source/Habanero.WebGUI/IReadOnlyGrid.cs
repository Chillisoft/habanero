//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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

using System.Collections;
using Habanero.BO;

namespace Habanero.WebGUI
{

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
        /// Returns a list of the business objects currently selected
        /// </summary>
        IList SelectedBusinessObjects { get; }
        
        /// <summary>
        /// Adds a business object to the collection being represented
        /// </summary>
        /// <param name="bo">The business object to add</param>
        void AddBusinessObject(BusinessObject bo);

        /// <summary>
        /// The event of a row being double-clicked
        /// </summary>
        //event RowDoubleClickedHandler RowDoubleClicked;

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

        IBusinessObjectCollection BusinessObjectCollection { get; }
    }
}