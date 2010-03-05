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
using System.Collections.Generic;
using System.ComponentModel;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.UI.Base
{
    /// <summary>
    /// The delegate used for custom loading of the grid
    /// </summary>
    /// <param name="grid">The grid to be loaded</param>
    /// <param name="col">The collection to load into the grid</param>
    public delegate void GridLoaderDelegate(IGridBase grid, IBusinessObjectCollection col);

//    /// <summary>
//    /// Handles the event of a user double-clicking on a row in the grid
//    /// </summary>
//    /// <param name="sender">The object that notified of the event</param>
//    /// <param name="e">Attached arguments regarding the event</param>
//    public delegate void RowDoubleClickedHandler(Object sender, BOEventArgs e);

    /// <summary>
    /// Provides an Interface that is used by the Grid's in Habanero this extends the <see cref="IDataGridView"/> so that it is adapted to show business objects
    /// </summary>
    public interface IGridBase : IDataGridView, IBOColSelectorControl
    {
        /// <summary>
        /// Sets the business object collection displayed in the grid.  This
        /// collection must be pre-loaded using the collection's Load() command.
        /// The default UI definition will be used, that is a 'ui' element 
        /// without a 'name' attribute.
        /// </summary>
        /// <param name="col">The collection of business objects to display.  This
        /// collection must be pre-loaded.</param>
        [Obsolete("Should be replaced with 'BusinessObjectCollection' property")] //01 Mar 2009
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        void SetBusinessObjectCollection(IBusinessObjectCollection col);

//        /// <summary>
//        /// Gets and Sets the business object collection displayed in the grid.  This
//        /// collection must be pre-loaded using the collection's Load() command or from the
//        /// <see cref="IBusinessObjectLoader"/>.
//        /// The default UI definition will be used, that is a 'ui' element 
//        /// without a 'name' attribute.
//        /// </summary>
//        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        IBusinessObjectCollection BusinessObjectCollection { get; set; }
//
//        /// <summary>
//        /// Gets and sets the currently selected business object in the grid
//        /// </summary>
//        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        IBusinessObject SelectedBusinessObject { get; set; }

//        /// <summary>
//        /// Gets a List of currently selected business objects
//        /// </summary>
//        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        IBusinessObjectList SelectedBusinessObjects { get; }

        /// <summary>
        /// Gets a List of currently selected business objects
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        IList<BusinessObject> SelectedBusinessObjects { get; }
        /// <summary>
        /// Checks if th user wants to delete the selected Business Object (generally via a popup message
        /// </summary>
        /// <returns></returns>
        bool CheckUserWantsToDelete();
//
//        /// <summary>
//        /// Occurs when a business object is selected
//        /// </summary>
//        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        event EventHandler<BOEventArgs> BusinessObjectSelected;

//        /// <summary>
//        /// Occurs when the current selection in the grid is changed
//        /// </summary>
//        event EventHandler SelectionChanged;

        /// <summary>
        /// Occurs when the collection in the grid is changed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        event EventHandler CollectionChanged;

//        /// <summary>
//        /// Clears the business object collection and the rows in the data table
//        /// </summary>
//        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        void Clear();

        /// <summary>
        /// Returns the business object collection being displayed in the grid
        /// </summary>
        /// <returns>Returns a business collection</returns>
        [Obsolete("Should be replaced with 'BusinessObjectCollection' property")]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        IBusinessObjectCollection GetBusinessObjectCollection();

        ///// <summary>
        ///// Returns the business object at the specified row number
        ///// </summary>
        ///// <param name="row">The row number in question</param>
        ///// <returns>Returns the busines object at that row, or null
        ///// if none is found</returns>
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //IBusinessObject GetBusinessObjectAtRow(int row);

        ///<summary>
        /// Returns the row for the specified <see cref="IBusinessObject"/>.
        ///</summary>
        ///<param name="businessObject">The <see cref="IBusinessObject"/> to search for.</param>
        ///<returns>Returns the row for the specified <see cref="IBusinessObject"/>, 
        /// or null if the <see cref="IBusinessObject"/> is not found in the grid.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        IDataGridViewRow GetBusinessObjectRow(IBusinessObject businessObject);

        /// <summary>
        /// Applies a filter clause to the data table and updates the filter.
        /// The filter allows you to determine which objects to display using
        /// some criteria.  This is typically generated by an <see cref="IFilterControl"/>.
        /// </summary>
        /// <param name="filterClause">The filter clause</param>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        void ApplyFilter(IFilterClause filterClause);

        /// <summary>
        /// Gets and sets the delegated grid loader for the grid.
        /// <br/>
        /// This allows the user to implememt a custom
        /// loading strategy. This can be used to load a collection of business objects into a grid with images or buttons
        /// that implement custom code. (Grids loaded with a custom delegate generally cannot be set up to filter 
        /// (grid filters a dataview based on filter criteria),
        /// but can be set up to search (a business object collection loaded with criteria).
        /// For a grid to be filterable the grid must load with a dataview.
        /// <br/>
        /// If no grid loader is specified then the default grid loader is employed. This consists of parsing the collection into 
        /// a dataview and setting this as the datasource.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        GridLoaderDelegate GridLoader { get; set; }

        /// <summary>
        /// Gets the grid's DataSet provider, which loads the collection's
        /// data into a DataSet suitable for the grid
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        IDataSetProvider DataSetProvider { get; }
        
        ///<summary>
        /// Returns the name of the column being used for tracking the business object identity.
        /// If a <see cref="IDataSetProvider"/> is used then it will be the <see cref="IDataSetProvider.IDColumnName"/>
        /// Else it will be "HABANERO_OBJECTID".
        ///</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        string IDColumnName { get; }

        /// <summary>
        /// Fires an event indicating that the selected business object
        /// is being edited
        /// </summary>
        /// <param name="bo">The business object being edited</param>
        [Obsolete("Should use FireBusinessObjectEditedEvent")]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        void SelectedBusinessObjectEdited(BusinessObject bo);

        /// <summary>
        /// Fires an event indicating that the selected business object
        /// is being edited
        /// </summary>
        /// <param name="bo">The business object being edited</param>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        void FireBusinessObjectEditedEvent(BusinessObject bo);

        /// <summary>
        /// Occurs when a business object is being edited
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        event EventHandler<BOEventArgs> BusinessObjectEdited;

        /// <summary>
        /// Reloads the grid based on the collection returned by GetBusinessObjectCollection
        /// </summary>
        //[Obsolete("Should use 'RefreshSelector'")]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        void RefreshGrid();


        /// <summary>
        /// Occurs when a row is double-clicked by the user
        /// </summary>
        event RowDoubleClickedHandler RowDoubleClicked;

        /// <summary>
        /// Gets and sets the UI definition used to initialise the grid structure (the UI name is indicated
        /// by the "name" attribute on the UI element in the class definitions
        /// </summary>
        string UiDefName { get; set; }

        /// <summary>
        /// Gets and sets the class definition used to initialise the grid structure
        /// </summary>
        IClassDef ClassDef { get; set;}

        ///<summary>
        /// Refreshes the row values for the specified <see cref="IBusinessObject"/>.
        ///</summary>
        ///<param name="businessObject">The <see cref="IBusinessObject"/> for which the row must be refreshed.</param>
        void RefreshBusinessObjectRow(IBusinessObject businessObject);
        /// <summary>
        /// Creates a dataset provider that is applicable to this grid. For example, a readonly grid would
        /// return a <see cref="ReadOnlyDataSetProvider"/>, while an editable grid would return an editable one.
        /// </summary>
        /// <param name="col">The collection to create the datasetprovider for</param>
        /// <returns>Returns the data set provider</returns>
        IDataSetProvider CreateDataSetProvider(IBusinessObjectCollection col);

        /// <summary>
        /// Gets or sets the boolean value that determines whether to confirm
        /// deletion with the user when they have chosen to delete a row
        /// </summary>
        bool ConfirmDeletion { get; set; }

        /// <summary>
        /// Gets or sets the delegate that checks whether the user wants to delete selected rows
        /// </summary>
        CheckUserConfirmsDeletion CheckUserConfirmsDeletionDelegate { get; set; }
    }
}