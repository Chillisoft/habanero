using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;
using KeyPressEventHandler=Habanero.UI.Base.ControlInterfaces.KeyPressEventHandler;

namespace Habanero.UI.WebGUI
{
    public class TextBoxGiz : TextBox, ITextBox
    {
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }
    }

}
