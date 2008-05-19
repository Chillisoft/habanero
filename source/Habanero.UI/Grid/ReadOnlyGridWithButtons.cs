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


using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI;
using Habanero.UI.Forms;
using log4net;

namespace Habanero.UI.Grid
{
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
    public class ReadOnlyGridWithButtons : UserControl
    {
        private static ILog log = LogManager.GetLogger("Habanero.UI.Grid.ReadOnlyGridWithButtons");
        public event EventHandler ItemSelected;

        private readonly ReadOnlyGrid _grid;
        private readonly ReadOnlyGridButtonControl _buttons;
        private readonly GridSelectionController _gridSelectionController;
        
        private IBusinessObjectCollection _collection;

        /// <summary>
        /// Constructor to initialise a new grid
        /// </summary>
        public ReadOnlyGridWithButtons()
        {
            BorderLayoutManager manager = new BorderLayoutManager(this);
            _grid = new ReadOnlyGrid();
            _grid.Name = "GridControl";
            manager.AddControl(_grid, BorderLayoutManager.Position.Centre);
            _buttons = new ReadOnlyGridButtonControl(_grid);
            _buttons.Name = "ButtonControl";
            manager.AddControl(_buttons, BorderLayoutManager.Position.South);
                        
            _gridSelectionController = new GridSelectionController(_grid);
            _gridSelectionController.DelayedItemSelected = DelayedItemSelected;

            this.Buttons.BusinessObjectEditor = new DefaultBOEditor();
            //this.Buttons.BusinessObjectCreator = new DefaultBOCreator(_provider.ClassDef);
        }

        #region Public Interface to Selection methods.

        /// <summary>
        /// Gets or sets the single selected business object (null if none are selected)
        /// denoted by where the current selected cell is
        /// </summary>
        public BusinessObject SelectedBusinessObject
        {
            get { return _gridSelectionController.SelectedBusinessObject; }
            set { _gridSelectionController.SelectedBusinessObject = value; }
        }

        /// <summary>
        /// Reselects the current row and creates a new item selected event
        /// if the current row has changed
        /// </summary>
        public void ReselectSelectedRow()
        {
            //this.Grid.SelectedBusinessObject = null;
            //_oldRowNumber = -1;
            //FireItemSelectedIfCurrentRowChanged();

            // TODO Check how this method is used because the above code wouldn't have done the right thing ( - Mark ).
            // TODO Check if this line below correctly repaces the above code or fulfills the objectives of the method ( - Mark ).
            _gridSelectionController.Reselect();
        }

        /// <summary>
        /// Adds another delegate to those of the selected item
        /// </summary>
        /// <param name="boDelegate">The delegate to add</param>
        public void AddItemSelectedDelegate(SetBusinessObjectDelegate boDelegate)
        {
            _gridSelectionController.AddItemSelectedDelegate(boDelegate);
        }

        #endregion //Public Interface to Selection methods.

        /// <summary>
        /// Creates a new item selected event
        /// </summary>
        /// <param name="sender">The sender</param>
        private void DelayedItemSelected(object sender)
        {
            if (this.ItemSelected != null)
            {
                this.ItemSelected(sender, new EventArgs());
            }
        }

        /// <summary>
        /// Returns the grid object held. This property can be used to
        /// access a range of functionality for the grid
        /// (eg. myGridWithButtons.Grid.AddBusinessObject(...)).
        /// </summary>
        public ReadOnlyGrid Grid
        {
            get { return _grid; }
        }

        /// <summary>
        /// Returns the button control held. This property can be used
        /// to access a range of functionality for the button control
        /// (eg. myGridWithButtons.Buttons.AddButton(...)).
        /// </summary>
        public ReadOnlyGridButtonControl Buttons
        {
            get { return _buttons; }
        }

        /// <summary>
        /// Gets or sets the boolean value that determines whether to confirm
        /// deletion with the user when they have clicked the Delete button
        /// </summary>
        public bool ConfirmDeletion
        {
            get { return _buttons.ConfirmDeletion; }
            set { _buttons.ConfirmDeletion = value; }
        }

        /// <summary>
        /// Sets the business object collection to display.  Loading of
        /// the collection needs to be done before it is assigned to the
        /// grid.  This method assumes a default ui definition is to be
        /// used, that is a 'ui' element without a 'name' attribute.
        /// </summary>
        /// <param name="boCollection">The new business object collection
        /// to be shown in the grid</param>
		public void SetCollection(IBusinessObjectCollection boCollection)
        {
            this.SetCollection(boCollection, "default");
        }

        /// <summary>
        /// Sets the business object collection to display.  Loading of
        /// the collection needs to be done before it is assigned to the
        /// grid.
        /// </summary>
        /// <param name="boCollection">The new business object collection
        /// to be shown in the grid</param>
        /// <param name="uiName">The name of the ui definition used to 
        /// format the grid, as specified in the 'name' attribute of the 
        /// 'ui' element in the class definitions</param>
		public void SetCollection(IBusinessObjectCollection boCollection, string uiName)
        {
            _collection = boCollection;
            this.Grid.SetCollection(boCollection, uiName);
            this.Buttons.BusinessObjectEditor = new DefaultBOEditor();
            this.Buttons.BusinessObjectCreator = new DefaultBOCreator(_collection.ClassDef);
        }

        /// <summary>
        /// Removes the specified business object from the list
        /// </summary>
        /// <param name="objectToRemove">The business object to remove</param>
        public void RemoveBusinessObject(IBusinessObject objectToRemove)
        {
            this.Grid.RemoveBusinessObject(objectToRemove);
            if (this.Grid.HasBusinessObjects)
            {
                _gridSelectionController.RefreshSelection();
                //FireItemSelected();
            }
        }

        /// <summary>
        /// Adds the specified business object to the list
        /// </summary>
        /// <param name="objectToAdd">The business object to add</param>
        public void AddBusinessObject(IBusinessObject objectToAdd)
        {
            this.Grid.AddBusinessObject(objectToAdd);
        }

        /// <summary>
        /// Returns a cloned collection of the business objects in the grid
        /// </summary>
        /// <returns>Returns a business object collection</returns>
		public IBusinessObjectCollection GetCollectionClone()
        {
            return this.Grid.GetCollectionClone();
        }

        /// <summary>
        /// Returns a list of the filtered business objects
        /// </summary>
        public List<BusinessObject> FilteredBusinessObjects
        {
            get { return this.Grid.FilteredBusinessObjects; }
        }

    }
}