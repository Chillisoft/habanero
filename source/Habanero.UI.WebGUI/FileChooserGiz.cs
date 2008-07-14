using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;
using Habanero.UI.Base.ControlInterfaces;

namespace Habanero.UI.WebGUI
{
    public partial class FileChooserGiz : UserControlGiz, IFileChooser 
    {
        private readonly IControlFactory _controlFactory;
        private readonly FileChooserManager _fileChooserManager;

        public FileChooserGiz(IControlFactory controlFactory)
        {
            this._controlFactory = controlFactory;
            _fileChooserManager = new FileChooserManager(controlFactory,this);
        }

        public string SelectedFilePath
        {
            get { return _fileChooserManager.SelectedFilePath; }
            set { _fileChooserManager.SelectedFilePath = value; }
        }

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }
    }
}
