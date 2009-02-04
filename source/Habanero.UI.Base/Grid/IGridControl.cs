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
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides a combination of grid, filter and buttons used to edit a
    /// collection of business objects
    /// </summary>
    public interface IGridControl : IControlHabanero
    {
        /// <summary>
        /// Initiliases the grid structure using the default UI class definition (implicitly named "default")
        /// </summary>
        /// <param name="classDef">The class definition of the business objects shown in the grid</param>
        void Initialise(IClassDef classDef);

        /// <summary>
        /// Initialises the grid structure using the specified UI class definition
        /// </summary>
        /// <param name="classDef">The class definition of the business objects shown in the grid</param>
        /// <param name="uiDefName">The UI definition with the given name</param>
        void Initialise(IClassDef classDef, string uiDefName);

        /// <summary>
        /// Gets and sets the UI definition used to initialise the grid structure (the UI name is indicated
        /// by the "name" attribute on the UI element in the class definitions
        /// </summary>
        string UiDefName { get; set; }

        /// <summary>
        /// Gets and sets the class definition used to initialise the grid structure
        /// </summary>
        IClassDef ClassDef { get; set;}

        /// <summary>
        /// Returns the grid object held. This property can be used to
        /// access a range of functionality for the grid
        /// (eg. myGridWithButtons.Grid.AddBusinessObject(...)).
        /// </summary>    
        IGridBase Grid { get; }

        /// <summary>
        /// Gets and sets the default order by clause used for loading the grid when the <see cref="FilterModes"/>
        /// is set to Search
        /// </summary>
        string OrderBy { get; set; }

        /// <summary>
        /// Gets and sets the standard search criteria used for loading the grid when the <see cref="FilterModes"/>
        /// is set to Search. This search criteria will be appended with an AND to any search criteria returned
        /// by the FilterControl.
        /// </summary>
        string AdditionalSearchCriteria { get; set; }

        ///<summary>
        /// The <see cref="IFilterControl"/> that is displayed for the grid.
        ///</summary>
        IFilterControl FilterControl { get; }
    }
}