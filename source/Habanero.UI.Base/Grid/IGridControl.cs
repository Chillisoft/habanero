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
using System;
using System.ComponentModel;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides a combination of grid, filter and buttons used to edit a
    /// collection of business objects
    /// </summary>
    public interface IGridControl : IBOSelectorAndEditor 
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
        /// Initialises the grid structure using the specified UI class definition
        /// </summary>
        /// <param name="classDef">The class definition of the business objects shown in the grid</param>
        /// <param name="gridDef">The grid definition to use to initialise</param>
        /// <param name="uiDefName">The name of the grid definition</param>
        void Initialise(IClassDef classDef, IUIGrid gridDef, string uiDefName);

        /// <summary>
        /// Gets and sets the name of the UI definition <see cref="UIDef"/> used to initialise the grid structure (the UI name is indicated
        /// by the "name" attribute on the UI element in the class definitions (<see cref="IClassDef"/>)
        /// </summary>
        string UiDefName { get; set; }

        /// <summary>
        /// Gets and sets the class definition ((<see cref="IClassDef"/>) used to initialise the grid structure
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
        /// The <see cref="IFilterControl"/> that is displayed for the grid which is used to filter the grid rows.
        ///</summary>
        IFilterControl FilterControl { get; }

        /// <summary>
        /// Gets and sets the filter modes for the grid (i.e. filter or search). See <see cref="FilterModes"/>.
        /// </summary>
        FilterModes FilterMode { get; set; }

        /// <summary>
        /// Gets the button control, which contains a set of default buttons for
        /// editing the objects and can be customised
        /// </summary>
        IButtonGroupControl Buttons { get; }

        ///<summary>
        /// Returns a Flag indicating whether this control has been initialised yet or not.
        /// Gets the value indicating whether one of the overloaded initialise
        /// methods been called for the grid
        ///</summary>
        bool IsInitialised { get; }


        /// <summary>
        /// Sets the business object collection to display.  Loading of
        /// the collection needs to be done before it is assigned to the
        /// grid.  This method assumes a default UI definition is to be
        /// used, that is a 'ui' element without a 'name' attribute.
        /// </summary>
        /// <param name="boCollection">The new business object collection
        /// to be shown in the grid</param>
        [Obsolete("Should be replaced with 'BusinessObjectCollection' property")] //01 Mar 2009
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        void SetBusinessObjectCollection(IBusinessObjectCollection boCollection);

        ///<summary>
        /// Returns the <see cref="IBusinessObjectCollection"/> that has been set for this <see cref="IGridControl"/>.
        ///</summary>
        ///<returns>Returns the <see cref="IBusinessObjectCollection"/> that has been set for this <see cref="IGridControl"/>.</returns>
        [Obsolete("Should be replaced with 'BusinessObjectCollection' property")] //01 Mar 2009
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        IBusinessObjectCollection GetBusinessObjectCollection();

        /// <summary>
        /// Gets and sets the currently selected business object in the grid
        /// </summary>
        new IBusinessObject SelectedBusinessObject { get; set; }

        void RefreshFilter();
    }
}
