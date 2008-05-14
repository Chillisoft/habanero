using System.Collections;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    internal class PanelWin : Panel, IPanel
    {
        IList IChilliControl.Controls
        {
            get { return this.Controls; }
        }
    }
}