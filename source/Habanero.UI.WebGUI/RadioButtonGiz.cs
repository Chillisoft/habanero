using System;
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class RadioButtonGiz : RadioButton, IRadioButton
    {
        public IList Controls
        {
            get { throw new NotImplementedException(); }
        }
    }
}