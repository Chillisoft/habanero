using System;
using System.Windows.Forms;
using Chillisoft.Generic.v2;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.Application.v2
{
    /// <summary>
    /// Manages buttons in an editable grid
    /// </summary>
    public class EditableGridButtonControl : ButtonControl
    {
        private readonly IEditableGrid itsEditableGrid;

        /// <summary>
        /// Constructor to initialise a new control
        /// </summary>
        /// <param name="editableGrid">The editable grid</param>
        public EditableGridButtonControl(IEditableGrid editableGrid)
        {
            this.AddButton("Save", new EventHandler(SaveButtonClickHandler));
            this.AddButton("Cancel", new EventHandler(CancelButtonClickHandler));
            this.itsEditableGrid = editableGrid;
        }

        /// <summary>
        /// Handles the event of the "Cancel" button being pressed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CancelButtonClickHandler(object sender, EventArgs e)
        {
            if (itsEditableGrid == null) return;
            itsEditableGrid.RejectChanges();
        }

        /// <summary>
        /// Handles the event of the "Save" button being clicked
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void SaveButtonClickHandler(object sender, EventArgs e)
        {
            if (itsEditableGrid == null) return;
            try
            {
                itsEditableGrid.AcceptChanges();
            }
            catch (UserException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}