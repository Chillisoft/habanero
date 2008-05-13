using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class ControlGiz : Control, IChilliControl
    {
        IList IChilliControl.Controls
        {
            get { return this.Controls; }
        }
    }
}