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
using System;
using System.ComponentModel;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    ///<summary>
    /// A form that allows the user to capture additional information about what happened when the error occured and to
    ///  send this to the relevant person.
    ///</summary>
    public class ErrorDescriptionForm : FormWin
    {
        private readonly ITextBox _errorDescriptionTextBox;
        /// <summary>
        /// Event raised when this form is due to close.
        /// </summary>
        public event EventHandler ErrorDescriptionFormClosing;
 
        ///<summary>
        /// A constructor for the <see cref="ErrorDescriptionForm"/>
        ///</summary>
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
            this.Closing += delegate(object sender, CancelEventArgs e)
            {
                if (ErrorDescriptionFormClosing == null)
                {
                    return;
                }
                ErrorDescriptionFormClosing(sender, e);
            };
        }

        ///<summary>
        /// Returns the text box that contains the error description.
        ///</summary>
        public ITextBox ErrorDescriptionTextBox
        {
            get { return _errorDescriptionTextBox; }
        }
    }
}
