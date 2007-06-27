using System;
using System.Drawing;
using System.Windows.Forms;
using Habanero.Ui.Base;
using Habanero.Ui.Forms;

namespace Habanero.Ui.Forms
{
    /// <summary>
    /// Provides a form that contains a specified control and "OK" and
    /// "Cancel" buttons
    /// </summary>
    public class OKCancelDialog
    {
        private readonly Control _controlToNest;
        private readonly string _title;
        private readonly Point _position;
        private Form _form;

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
            _controlToNest = controlToNest;
            _title = title;
            _position = position;
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
        /// Sets up the form
        /// </summary>
        /// <returns>Returns the Form object created</returns>
        private Form CreateDialog()
        {
            _form = new Form();

            ButtonControl buttons = new ButtonControl();
            Button cancelButton = buttons.AddButton("Cancel", new EventHandler(CancelButtonClickHandler));
            Button okButton = buttons.AddButton("OK", new EventHandler(OKButtonClickHandler));

            _form.Width = Math.Max( _controlToNest.Width, okButton.Width * 2 + 30);
            _form.Height = _controlToNest.Height + buttons.Height + 40;
            GridLayoutManager manager = new GridLayoutManager(_form);
            manager.SetGridSize(2, 1);
            manager.FixAllRowsBasedOnContents();

            manager.AddControl(_controlToNest);
            manager.AddControl(buttons);

            _form.Text = _title;
            if (_position.X != 0 && _position.Y != 0)
            {
                _form.StartPosition = FormStartPosition.Manual;
                _form.Location = _position;
            }
            else
            {
                _form.StartPosition = FormStartPosition.CenterScreen;
            }
            _form.AcceptButton = okButton;
            okButton.NotifyDefault(true);
            _form.CancelButton = cancelButton;
            return _form;
        }

        /// <summary>
        /// Handles the event of the Cancel button being pressed, which
        /// closes the dialog
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CancelButtonClickHandler(object sender, EventArgs e)
        {
            _form.DialogResult = DialogResult.Cancel;
            _form.Close();
        }

        /// <summary>
        /// Handles the event of the OK button being pressed, which
        /// closes the dialog
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void OKButtonClickHandler(object sender, EventArgs e)
        {
            _form.DialogResult = DialogResult.OK;
            _form.Close();
        }
    }
}