using System;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.UI.Base;
using Habanero.UI.Forms;
using Habanero.UI.Grid;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Manages buttons in an editable grid.  By default, "Save" and "Cancel"
    /// buttons are added, which call the appropriate methods in the grid
    /// to accept or discard the changes made by the user.
    /// You can add other buttons with a command like: 
    /// "AddButton("buttonName", new EventHandler(handlerMethodToCall));".
    /// You can also manipulate the behaviour of this control by accessing it
    /// through the grid with an accessor like "myGrid.Buttons.someMethod".
    /// </summary>
    public class EditableGridButtonControl : ButtonControl
    {
        private readonly IEditableGrid _editableGrid;

        /// <summary>
        /// Constructor to initialise a new control
        /// </summary>
        /// <param name="editableGrid">The editable grid</param>
        public EditableGridButtonControl(IEditableGrid editableGrid)
        {
            this.AddButton("Cancel", new EventHandler(CancelButtonClickHandler));
            this.AddButton("Save", new EventHandler(SaveButtonClickHandler));
            this._editableGrid = editableGrid;
        }

        /// <summary>
        /// Handles the event of the "Cancel" button being pressed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CancelButtonClickHandler(object sender, EventArgs e)
        {
            if (_editableGrid == null) return;
            _editableGrid.RejectChanges();
        }

        /// <summary>
        /// Handles the event of the "Save" button being clicked
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void SaveButtonClickHandler(object sender, EventArgs e)
        {
            if (_editableGrid == null) return;
            try
            {
                _editableGrid.AcceptChanges();
            }
            catch (UserException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}