using System;
using System.Drawing;
using System.Windows.Forms;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.Misc.v2
{
    /// <summary>
    /// Provides a form that contains a specified control and "OK" and
    /// "Cancel" buttons
    /// </summary>
    public class OKCancelDialog
    {
        private readonly Control itsControlToNest;
        private readonly string itsTitle;
        private readonly Point itsPosition;
        private Form itsForm;

        /// <summary>
        /// Constructor to initialise the dialog with a control to display
        /// </summary>
        /// <param name="controlToNest">The control to add to the dialog</param>
        public OKCancelDialog(Control controlToNest) : this(controlToNest, "")
        {
        }

        /// <summary>
        /// Constructor to initialise the dialog with a control to display
        /// </summary>
        /// <param name="controlToNest">The control to add to the dialog</param>
        /// <param name="title">The dialog title</param>
        public OKCancelDialog(Control controlToNest, string title) : this(controlToNest, title, new Point(0, 0))
        {
        }

        /// <summary>
        /// Constructor to initialise the dialog with a control to display
        /// </summary>
        /// <param name="controlToNest">The control to add to the dialog</param>
        /// <param name="title">The dialog title</param>
        /// <param name="position">The position at which to display the
        /// dialog</param>
        public OKCancelDialog(Control controlToNest, string title, Point position)
        {
            itsControlToNest = controlToNest;
            itsTitle = title;
            itsPosition = position;
        }

        /// <summary>
        /// Sets up the form and makes it visible to the user.  Calls the
        /// CreateDialog() method.
        /// </summary>
        /// <returns>Returns a DialogResult object which indicates the user's 
        /// response to the dialog. See System.Windows.Forms.DialogResult for 
        /// more detail.</returns>
        public DialogResult ShowDialog()
        {
            return CreateDialog().ShowDialog();
        }

        /// <summary>
        /// Sets up the form.  If you intend to call ShowDialog() immediately,
        /// there is no need to call this method.
        /// </summary>
        /// <returns>Returns the Form object created</returns>
        /// TODO ERIC - for consistency (cf. InputBox...) and to eliminate
        /// confusion (users think they need to create then show), 
        /// combine this method into ShowDialog()
        public Form CreateDialog()
        {
            itsForm = new Form();

            ButtonControl buttons = new ButtonControl();
            Button cancelButton = buttons.AddButton("Cancel", new EventHandler(CancelButtonClickHandler));
            Button okButton = buttons.AddButton("OK", new EventHandler(OKButtonClickHandler));

            itsForm.Width = Math.Max( itsControlToNest.Width, okButton.Width * 2 + 30);
            itsForm.Height = itsControlToNest.Height + buttons.Height + 40;
            GridLayoutManager manager = new GridLayoutManager(itsForm);
            manager.SetGridSize(2, 1);
            manager.FixAllRowsBasedOnContents();

            manager.AddControl(itsControlToNest);
            manager.AddControl(buttons);

            itsForm.Text = itsTitle;
            if (itsPosition.X != 0 && itsPosition.Y != 0)
            {
                itsForm.StartPosition = FormStartPosition.Manual;
                itsForm.Location = itsPosition;
            }
            else
            {
                itsForm.StartPosition = FormStartPosition.CenterScreen;
            }
            itsForm.AcceptButton = okButton;
            okButton.NotifyDefault(true);
            itsForm.CancelButton = cancelButton;
            return itsForm;
        }

        /// <summary>
        /// Handles the event of the Cancel button being pressed, which
        /// closes the dialog
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CancelButtonClickHandler(object sender, EventArgs e)
        {
            itsForm.DialogResult = DialogResult.Cancel;
            itsForm.Close();
        }

        /// <summary>
        /// Handles the event of the OK button being pressed, which
        /// closes the dialog
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void OKButtonClickHandler(object sender, EventArgs e)
        {
            itsForm.DialogResult = DialogResult.OK;
            itsForm.Close();
        }
    }
}