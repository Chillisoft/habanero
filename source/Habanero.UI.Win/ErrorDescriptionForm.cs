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
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class ErrorDescriptionForm : FormWin
    {
        private readonly ITextBox _errorDescriptionTextBox;
        public event EventHandler ErrorDescriptionFormClosing;
 
        public ErrorDescriptionForm()
        {

            IControlFactory controlFactory = GlobalUIRegistry.ControlFactory;
            ILabel label = controlFactory.CreateLabel("Please enter further details regarding the error : ");
            _errorDescriptionTextBox = controlFactory.CreateTextBox();
            ErrorDescriptionTextBox.Multiline = true;
            IButtonGroupControl buttonGroupControl = controlFactory.CreateButtonGroupControl();
            buttonGroupControl.AddButton("OK", delegate { this.Close(); });
            BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(label, BorderLayoutManager.Position.North);
            layoutManager.AddControl(ErrorDescriptionTextBox, BorderLayoutManager.Position.Centre);
            layoutManager.AddControl(buttonGroupControl, BorderLayoutManager.Position.South);
            this.Text = "Error Description";
            this.Width = 500;
            this.Height = 400;
        }

        public ITextBox ErrorDescriptionTextBox
        {
            get { return _errorDescriptionTextBox; }
        }
    }
}
