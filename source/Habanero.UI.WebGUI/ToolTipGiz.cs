using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class ToolTipGiz : ToolTip, IToolTip
    {
        public void SetToolTip(IControlChilli control, string toolTipText)
        {
            base.SetToolTip((Control) control, toolTipText);
        }

        public string GetToolTip(IControlChilli controlChilli)
        {
            return base.GetToolTip((Control) controlChilli);
        }
    }
}