//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Forms
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