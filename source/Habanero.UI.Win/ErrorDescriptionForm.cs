using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
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
