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


using System.Windows.Forms;
using Habanero.BO;
using Habanero.UI.Base;
using Habanero.UI.Forms;

namespace Habanero.UI.Grid
{
    /// <summary>
    /// Manages an editable grid with buttons, that displays a business object
    /// collection that has been pre-loaded.  Note that changes are not
    /// immediately persisted to the database.
    /// By default, "Save" and "Cancel" buttons are added at 
    /// the bottom of the grid, which accept or discard changes made by the
    /// user.<br/>
    /// To supply the business object collection to display in the grid,
    /// instantiate a new BusinessObjectCollection and load the collection
    /// from the database using the Load() command.  After instantiating this
    /// grid with the parameterless constructor, pass the collection with
    /// SetCollection().<br/>
    /// To have further control of particular aspects of the buttons or
    /// grid, access the standard functionality through the Grid and
    /// Buttons properties (eg. myGridWithButtons.Buttons.AddButton(...)).<br/>
    /// Note that this grid does not warn the user when it is being closed
    /// with unsaved changes.  The Closing event on the parent form needs to
    /// be used to do a dirty check and prevent closing.
    /// </summary>
    public class EditableGridWithButtons : UserControl, IGridWithButtons
    {
        private EditableGrid _grid;
        private EditableGridButtonControl _buttons;

        /// <summary>
        /// Constructor to initialise a new grid
        /// </summary>
        public EditableGridWithButtons()
        {
            BorderLayoutManager manager = new BorderLayoutManager(this);
            _grid = new EditableGrid();
            _grid.Name = "GridControl";
            manager.AddControl(_grid, BorderLayoutManager.Position.Centre);
            _buttons = new EditableGridButtonControl(_grid);
            _buttons.Name = "ButtonControl";
            manager.AddControl(_buttons, BorderLayoutManager.Position.South);
        }

        /// <summary>
        /// Sets the business object collection displayed in the grid.  This
        /// collection must be pre-loaded using the collection's Load() command.
        /// </summary>
        /// <param name="boCol">The collection of business objects to display. This
        /// collection needs to be pre-loaded</param>
        /// <param name="uiName">The ui definition to use, as specified in the 'name'
        /// attribute of the 'ui' element</param>
		public void SetCollection(IBusinessObjectCollection boCol, string uiName)
        {
            _grid.SetCollection(boCol, uiName);
        }

        /// <summary>
        /// Sets the business object collection displayed in the grid.  This
        /// collection must be pre-loaded using the collection's Load() command.
        /// The default ui definition will be used, that is a 'ui' element 
        /// without a 'name' attribute.
        /// </summary>
        /// <param name="boCol">The collection of business objects to display.  This
        /// collection must be pre-loaded.</param>
		public void SetCollection(IBusinessObjectCollection boCol)
        {
            _grid.SetCollection(boCol, "default");
        }

        /// <summary>
        /// Returns the grid object.  You can use this property to have
        /// specific control over the grid using its associated properties and 
        /// methods.
        /// </summary>
        public EditableGrid Grid
        {
            get { return _grid; }
        }

        /// <summary>
        /// Gets or sets the boolean value that determines whether to confirm
        /// deletion with the user when they have chosen to delete a row
        /// </summary>
        public bool ConfirmDeletion
        {
            get { return _grid.ConfirmDeletion; }
            set { _grid.ConfirmDeletion = value; }
        }

        /// <summary>
        /// Saves all changes made to the grid
        /// </summary>
        public void SaveChanges()
        {
            _grid.AcceptChanges();
        }

        /// <summary>
        /// Returns the button control that holds the buttons.  You can use this 
        /// property to have specific control over the buttons using their 
        /// associated properties and methods.
        /// </summary>
        public EditableGridButtonControl Buttons
        {
            get { return _buttons; }
        }

        /// <summary>
        /// Returns the currently selected business object
        /// </summary>
        public BusinessObject SelectedBusinessObject
        {
            get { return this._grid.SelectedBusinessObject; }
        }
    }
}