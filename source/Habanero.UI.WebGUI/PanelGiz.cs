using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    class PanelGiz:Panel, IChilliControl
    {
        IList IChilliControl.Controls
        {
            get { return this.Controls; }
        }
    }
}
