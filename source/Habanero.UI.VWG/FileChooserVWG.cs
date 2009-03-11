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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Provides a means to choose a file and makes the selected
    /// path available to a control that needs it
    /// </summary>
    public partial class FileChooserVWG : UserControlVWG, IFileChooser 
    {
        private readonly IControlFactory _controlFactory;
        private readonly FileChooserManager _fileChooserManager;
        /// <summary>
        /// Constructor for <see cref="FileChooserVWG"/>
        /// </summary>
        /// <param name="controlFactory"></param>
        public FileChooserVWG(IControlFactory controlFactory)
        {
            this._controlFactory = controlFactory;
            _fileChooserManager = new FileChooserManager(controlFactory,this);
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionVWG(base.Controls); }
        }

        /// <summary>
        /// Gets or sets the selected file path
        /// </summary>
        public string SelectedFilePath
        {
            get { return _fileChooserManager.SelectedFilePath; }
            set { _fileChooserManager.SelectedFilePath = value; }
        }
    }
}
