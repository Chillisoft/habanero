using System;
using System.Windows.Forms;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Provides a text field with a file path and a button to choose 
    /// a new file path
    /// </summary>
    public class FileChooser : UserControl
    {
        private TextBox _fileTextBox;
        private Button _selectFileButton;

        /// <summary>
        /// Constructor to initialise a FileChooser, which consists of a
        /// text field with a file path, and a button labelled "Select", which
        /// allows the user to open a file dialog and choose a new file
        /// </summary>
        public FileChooser()
        {
            FlowLayoutManager manager = new FlowLayoutManager(this);
            _fileTextBox = ControlFactory.CreateTextBox();
            _selectFileButton = ControlFactory.CreateButton("Select...", new EventHandler(SelectButtonClickHandler));
            manager.AddControl(_fileTextBox);
            manager.AddControl(_selectFileButton);
        }

        /// <summary>
        /// A handler that responds to the user pressing the "Select" button.
        /// This opens a standard file dialog and the chosen file name is
        /// stored in this instance.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void SelectButtonClickHandler(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _fileTextBox.Text = dialog.FileName;
            }
        }

        /// <summary>
        /// Returns the current file path
        /// </summary>
        public string SelectedFilePath
        {
            get { return _fileTextBox.Text; }
        }
    }
}