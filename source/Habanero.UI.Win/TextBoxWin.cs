using System;
using System.Collections;
using System.Windows.Forms;
using Habanero.UI.Base;
using KeyPressEventHandler=Habanero.UI.Base.ControlInterfaces.KeyPressEventHandler;

namespace Habanero.UI.Win
{
    public class TextBoxWin : TextBox, ITextBox
    {
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }
    }
}