using Habanero.UI.Base;
using System.Windows.Forms;

namespace Habanero.UI.Win
{
    public class TabPageWin : TabPage, ITabPage
    {
        public TabPageWin()
        {
        }

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        Base.DockStyle IControlChilli.Dock
        {
            get { return (Base.DockStyle) base.Dock; }
            set { base.Dock = (System.Windows.Forms.DockStyle) value; }
        }
    }
}