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
    /// Initialises the structure of a grid.  If a ClassDef is provided, the grid
    /// is initialised using the UI definition provided for that class.  If no
    /// ClassDef is provided, it is assumed that the grid will be set up in code
    /// by the developer.
    /// </summary>
    public interface IGridInitialiser
    {
        /// <summary>
        /// Initialises the grid without a ClassDef. This is typically used where the columns are set up manually
        /// for purposes such as adding a column with images to indicate the state of the object or adding a
        /// column with buttons/links.
        /// <br/>
        /// The grid must already have at least one column added. At least one column must be a column with the name
        /// "HABANERO_OBJECTID", which is used to synchronise the grid with the business objects.
        /// </summary>
        /// <exception cref="GridBaseInitialiseException">Thrown in the case where the columns
        /// have not already been defined for the grid</exception>
        /// <exception cref="GridBaseSetUpException">Thrown in the case where the grid has already been initialised</exception>
        void InitialiseGrid();

        /// <summary>
        /// Initialises the grid with the default UI definition for the class,
        /// as provided in the ClassDef
        /// </summary>
        /// <param name="classDef">The ClassDef used to initialise the grid</param>
        void InitialiseGrid(IClassDef classDef);

        /// <summary>
        /// Initialises the grid with a specified alternate UI definition for the class,
        /// as provided in the ClassDef
        /// </summary>
        /// <param name="classDef">The Classdef used to initialise the grid</param>
        /// <param name="uiDefName">The name of the UI definition</param>
        void InitialiseGrid(IClassDef classDef, string uiDefName);

        /// <summary>
        /// Initialises the grid with a specified alternate UI definition for the class
        /// </summary>
        /// <param name="classDef">The Classdef used to initialise the grid</param>
        /// <param name="uiGridDef">The UI Grid definition to use</param>
        ///         /// <param name="uiDefName">The name of the UI definition</param>
        void InitialiseGrid(IClassDef classDef, IUIGrid uiGridDef, string uiDefName);

        /// <summary>
        /// Gets the value indicating whether the grid has been initialised already
        /// </summary>
        bool IsInitialised { get; }

        /// <summary>
        /// Gets the grid that is being initialised
        /// </summary>
        IGridControl Grid { get; }
    }
}