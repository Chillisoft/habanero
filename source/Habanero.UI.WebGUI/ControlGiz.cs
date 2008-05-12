using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI;

namespace Habanero.UI.WebGUI
{
    internal class ControlGiz : Control, IChilliControl
    {
        IList IChilliControl.Controls
        {
            get { return this.Controls; }
        }
    }
}