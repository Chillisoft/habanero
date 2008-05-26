using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Habanero.UI.Base;
using Habanero.UI.Base.ControlInterfaces;

namespace Habanero.UI.Win
{
    public partial class FileChooserWin : UserControl, IFileChooser
    {
        private readonly IControlFactory _controlFactory;
        private readonly FileChooserManager _fileChooserManager;

        public FileChooserWin(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            _fileChooserManager = new FileChooserManager(controlFactory, this);
        }

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        public string SelectedFilePath
        {
            get { return _fileChooserManager.SelectedFilePath; }
            set { _fileChooserManager.SelectedFilePath = value; }
        }
    }
}
