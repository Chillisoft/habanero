using System;
using System.Windows.Forms;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.Misc.v2
{
    /// <summary>
    /// Provides a popup box that displays a message to the user
    /// </summary>
    /// TODO ERIC - rename to something like MessageBox or ScrollableMessageBox, 
    /// because textbox implies that you can enter text - or can it be edited?
    /// - add a constructor that provides a default width and height
    public class TextBoxMessageBox
    {
        private readonly string itsTitle;
        private readonly int itsHeight;
        private readonly int itsWidth;
        private readonly string itsMessage;
        private Form itsForm;

        /// <summary>
        /// Constructor to initialise the form with some given details
        /// </summary>
        /// <param name="title">The form title</param>
        /// <param name="message">The message to display</param>
        /// <param name="width">The width of the form</param>
        /// <param name="height">The height of the form</param>
        public TextBoxMessageBox(string title, string message, int width, int height)
        {
            itsMessage = message;
            itsWidth = width;
            itsHeight = height;
            itsTitle = title;
        }

        /// <summary>
        /// Sets up the form and makes it visible to the user
        /// </summary>
        public void ShowDialog()
        {
            itsForm = new Form();
            itsForm.Height = itsHeight;
            itsForm.Width = itsWidth;
            itsForm.Text = itsTitle;

            BorderLayoutManager manager = new BorderLayoutManager(itsForm);
            manager.AddControl(ControlFactory.CreateLabel(itsTitle, false), BorderLayoutManager.Position.North);
            TextBox tb = ControlFactory.CreateTextBox();
            tb.Multiline = true;
            tb.ScrollBars = ScrollBars.Vertical;
            tb.Text = itsMessage;
            manager.AddControl(tb, BorderLayoutManager.Position.Centre);

            ButtonControl buttons = new ButtonControl();
            buttons.AddButton("OK", new EventHandler(OKButtonClickHandler));
            manager.AddControl(buttons, BorderLayoutManager.Position.South);

            itsForm.ShowDialog();
        }

        /// <summary>
        /// Handles the event of the OK button being pressed, which closes
        /// the form
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void OKButtonClickHandler(object sender, EventArgs e)
        {
            itsForm.Close();
        }
    }
}