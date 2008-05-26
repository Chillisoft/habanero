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
