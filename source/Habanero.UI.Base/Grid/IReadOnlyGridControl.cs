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
using Habanero.UI.Base;

namespace Habanero.UI.Base
{


    /// <summary>
    /// Provides a combination of read-only grid, filter and buttons used to edit a
    /// collection of business objects.
    /// <br/>
    /// Adding, editing and deleting objects is done by clicking the available
    /// buttons in the button control (accessed through the Buttons property).
    /// By default, this uses of a popup form for editing of the object, as defined
    /// in the "form" element of the class definitions for that object.  You can
    /// override the editing controls using the BusinessObjectEditor/Creator/Deletor
    /// properties in this class.
    /// <br/>
    /// A filter control is placed above the grid and is used to filter which rows
    /// are shown.
    /// </summary>
    public interface IReadOnlyGridControl : IGridControl 
    {
        ///<summary>
        /// R
        ///</summary>
        IBusinessObject SelectedBusinessObject { get; set; }

        /// <summary>
        /// Gets the button control, which contains a set of default buttons for
        /// editing the objects and can be customised
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
        /// If no deletor is set then the <see cref="DefaultBODeletor"/> is used.  The default delete button
        /// is hidden unless programmatically shown (using Buttons.ShowDefaultDeleteButton).
        /// </summary>
        IBusinessObjectDeletor BusinessObjectDeletor { get; set; }

        /// <summary>
        /// Gets the filter control for the readonly grid, which is used to filter
        /// which rows are shown in the grid
        /// </summary>
        IFilterControl FilterControl { get; }

        /// <summary>
        /// Gets the value indicating whether one of the overloaded initialise
        /// methods been called for the grid
        /// </summary>
        bool IsInitialised { get; }

        /// <summary>
        /// Gets and sets the filter modes for the grid (i.e. filter or search).  See <see cref="FilterModes"/>.
        /// </summary>
        FilterModes FilterMode { get; set; }

        /// <summary>
        /// Sets the business object collection to display.  Loading of
        /// the collection needs to be done before it is assigned to the
        /// grid.  This method assumes a default UI definition is to be
        /// used, that is a 'ui' element without a 'name' attribute.
        /// </summary>
        /// <param name="boCollection">The business object collection
        /// to be shown in the grid</param>
        void SetBusinessObjectCollection(IBusinessObjectCollection boCollection);

        /// <summary>
        /// Initialises the grid without a ClassDef. This is used where the columns are set up manually.
        /// A typical case of when you would want to set the columns manually would be when the grid
        /// requires alternate columns, such as images to indicate the state of the object or buttons/links.
        /// The grid must already have at least one column added with the name "ID". This column is used
        /// to synchronise the grid with the business objects.
        /// </summary>
        /// <exception cref="GridBaseInitialiseException">Occurs where the columns have not
        /// already been defined for the grid</exception>
        void Initialise();

        ///<summary>
        /// Enable or disable the default double click handler for the grid where the <see cref="IBusinessObjectEditor"/>
        /// is used to edit the <see cref="IBusinessObject"/> represented by the row that was double clicked.
        /// If you want to implement a custom handler on double click, you should set this to false so that 
        /// the default handler does not interfere with your custom handler. 
        ///</summary>
        bool DoubleClickEditsBusinessObject { get; set; }
    }
}