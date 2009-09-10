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
namespace Habanero.UI.Base
{
    /// <summary>
    /// This manager groups common logic for IFileChooser objects.
    /// Do not use this object in working code - rather call CreateFileChooser
    /// in the appropriate control factory.
    /// </summary>
    public class FileChooserManager
    {
        private readonly IControlFactory _controlFactory;
        private readonly IFileChooser _fileChooser;
        private readonly ITextBox _fileTextBox;
        private readonly IButton _selectFileButton;

        ///<summary>
        /// Constructs the <see cref="FileChooserManager"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="fileChooser"></param>
        public FileChooserManager(IControlFactory controlFactory, IFileChooser fileChooser)
        {
            _controlFactory = controlFactory;
            _fileChooser = fileChooser;
            FlowLayoutManager manager = new FlowLayoutManager(_fileChooser, _controlFactory);
            _fileTextBox = _controlFactory.CreateTextBox();
            _selectFileButton = _controlFactory.CreateButton("Select...", null);
            manager.AddControl(_fileTextBox);
            manager.AddControl(_selectFileButton);
        }

        ///<summary>
        /// Gets and Sets the selected file path.
        ///</summary>
        public string SelectedFilePath
        {
            get { return _fileTextBox.Text; }
            set { _fileTextBox.Text = value; }
        }
    }
}
