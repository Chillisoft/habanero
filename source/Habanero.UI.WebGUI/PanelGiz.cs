using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    class PanelGiz:Panel, IPanel
    {
        IList IControlChilli.Controls
        {
            get { return this.Controls; }
        }

        public IDockStyle Dock
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }
    }
}
