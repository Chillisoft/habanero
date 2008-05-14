using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class ButtonGiz : Button, IButton
    {
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }
    }
}