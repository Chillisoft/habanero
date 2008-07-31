//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using System.Text;
using Habanero.UI.Base.ControlInterfaces;

namespace Habanero.UI.Base
{
    public class FileChooserManager
    {
        private readonly IControlFactory _controlFactory;
        private readonly IFileChooser _fileChooser;
        private readonly ITextBox _fileTextBox;
        private readonly IButton _selectFileButton;

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

        public string SelectedFilePath
        {
            get { return _fileTextBox.Text; }
            set { _fileTextBox.Text = value; }
        }
    }
}
