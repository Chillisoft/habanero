using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    internal class ToolTipWin : ToolTip, IToolTip
    {
        public void SetToolTip(IControlChilli label, string toolTipText)
        {
            base.SetToolTip((Control)label, toolTipText);
        }

        public string GetToolTip(IControlChilli controlChilli)
        {
            return base.GetToolTip((Control) controlChilli);
        }
    }
}