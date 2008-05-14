using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class ControlGiz : Control, IControlChilli
    {
        IList IControlChilli.Controls
        {
            get { return this.Controls; }
        }
    }
}