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
using Habanero.UI.Base.FilterControl;

namespace Habanero.UI.Base
{
    /// <summary>
    /// The filter modes that can be set up for the readonly grid.
    /// FilteModes.Filter is a mode used where the grid is provided with a collection of all the 
    /// business objects and the user can limit this grid by entering one or more filter criteria. 
    /// This is very interactive and converts the collection provided to the grid into a <see cref="System.Data.DataView"/>.
    /// The FilterModes.Search is a mode where the grid reloads the collection of business objects 
    /// based on the criteria entered by the user. This is typically used where the number of items that could be 
    /// loaded is large and for performance reasons you want to only load the collection with the items matching the 
    /// search criteria (this can also be used where a custome load delegate implements an alternate loading mechanism <see cref="GridLoaderDelegate"/>
    /// </summary>
    public enum FilterModes
    {
        Filter,
        Search
    }

    /// <summary>
    /// Manages a read-only grid with buttons (ie. a grid whose objects are
    /// edited through an editing form rather than directly on the grid).
    /// By default, an "Edit" and "Add" are added at 
    /// the bottom of the grid, which open up dialogs to edit the selected
    /// business object.<br/>
    /// To supply the business object collection to display in the grid,
    /// instantiate a new BusinessObjectCollection and load the collection
    /// from the database using the Load() command.  After instantiating this
    /// grid with the parameterless constructor, pass the collection with
    /// SetBusinessObjectCollection().<br/>
    /// To have further control of particular aspects of the buttons or
    /// grid, access the standard functionality through the Grid and
    /// Buttons properties (eg. myGridWithButtons.Buttons.AddButton(...)).
    /// You can assign a non-default object editor or creator for the buttons,
    /// using *.Buttons.BusinessObjectEditor and *.Buttons.BusinessObjectCreator.
    /// </summary>
    public interface IReadOnlyGridControl : IGridControl 
    {
        IBusinessObject SelectedBusinessObject { get; set; }

        /// <summary>
        /// Returns the button control held. This property can be used
        /// to access a range of functionality for the button control
        /// (eg. myGridWithButtons.Buttons.AddButton(...)).
        /// </summary>
        IReadOnlyGridButtonsControl Buttons { get; }

        /// <summary>
        /// Gets and sets the business object editor used to edit the object when the edit button is clicked
        /// If no editor is set then the <see cref="DefaultBOEditor"/> is used.
        /// </summary>
        IBusinessObjectEditor BusinessObjectEditor { get; set; }

        /// <summary>
        /// Gets and sets the business object creator used to create the object when the add button is clicked.
        /// If no creator is set then the <see cref="DefaultBOCreator"/> is used.
        /// </summary>
        IBusinessObjectCreator BusinessObjectCreator { get; set; }

        /// <summary>
        /// Gets and sets the business object deletor used to delete the object when the delete button is clicked
        /// If no deletor is set then the <see cref="DefaultBODeletor"/> is used.
        /// </summary>
        IBusinessObjectDeletor BusinessObjectDeletor { get; set; }

        /// <summary>
        /// returns the filter control for the readonly grid
        /// </summary>
        IFilterControl FilterControl { get; }

        /// <summary>
        /// has one of the overloaded initialise methods been called for the grid.
        /// </summary>
        bool IsInitialised { get; }

        /// <summary>
        /// gets and sets the filter modes for the grid i.e. Filter or search <see cref="FilterModes"/>
        /// </summary>
        FilterModes FilterMode { get; set; }

        /// <summary>
        /// Gets and sets the default order by clause used for loading the grid when the <see cref="FilterMode"/>
        /// is Search see <see cref="FilterModes"/>
        /// </summary>
        string OrderBy { get; set; }

        /// <summary>
        /// Gets and sets the standard search criteria used for loading the grid when the <see cref="FilterMode"/>
        /// is Search see <see cref="FilterModes"/>. This search criteria will be And (ed) to any search criteria returned
        /// by the FilterControl.
        /// </summary>
        string AdditionalSearchCriteria { get; set; }

        /// <summary>
        /// Sets the business object collection to display.  Loading of
        /// the collection needs to be done before it is assigned to the
        /// grid.  This method assumes a default ui definition is to be
        /// used, that is a 'ui' element without a 'name' attribute.
        /// </summary>
        /// <param name="boCollection">The new business object collection
        /// to be shown in the grid</param>
        void SetBusinessObjectCollection(IBusinessObjectCollection boCollection);

        /// <summary>
        /// Initialises the grid based with no classDef. This is used where the columns are set up manually.
        /// A typical case of when you would want to set the columns manually would be when the grid
        ///  requires alternate columns e.g. images to indicate the state of the object or buttons/links.
        /// The grid must already have at least one column added. At least one column must be a column with the name
        /// "ID" This column is used to synchronise the grid with the business objects.
        /// </summary>
        /// <exception cref="GridBaseInitialiseException"> in the case where the columns have not already been defined for the grid</exception>
        void Initialise();
    }
}