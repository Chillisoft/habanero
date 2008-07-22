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
    public interface IGridControl : IControlChilli
    {
        /// <summary>
        /// initiliase the grid to the with the 'default' UIdef.
        /// </summary>
        void Initialise(IClassDef classDef);

        void Initialise(IClassDef classDef, string uiDefName);
        /// <summary>
        /// gets and sets the user interface defiition that will load the grid and will be used by the grid add and edit buttons.
        /// </summary>
        string UiDefName { get; set; }
        /// <summary>
        /// gets and sets the class definition that will load the grid and will be used by the grid add and edit buttons.
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
        /// is Search see <see cref="FilterModes"/>
        /// </summary>
        string OrderBy { get; set; }

        /// <summary>
        /// Gets and sets the standard search criteria used for loading the grid when the <see cref="FilterMode"/>
        /// is Search see <see cref="FilterModes"/>. This search criteria will be And (ed) to any search criteria returned
        /// by the FilterControl.
        /// </summary>
        string AdditionalSearchCriteria { get; set; }
    }
}