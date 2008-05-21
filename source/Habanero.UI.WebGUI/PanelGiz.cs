using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    class PanelGiz:Panel, IPanel
    {
        IControlCollection IControlChilli.Controls
        {
            get
            {
                
                return new ControlCollectionGiz(base.Controls);
            }
        }

        public IDockStyle Dock
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }
    }
}
