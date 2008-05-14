using System.Collections;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    internal class PanelWin : Panel, IPanel
    {
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        public IDockStyle Dock
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }
    }
}