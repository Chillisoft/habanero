using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class LabelGiz : Label, ILabel
    {
        private const int WIDTH_FACTOR = 8;
        public int PreferredWidth
        {
            get { return this.Text.Length * WIDTH_FACTOR; }
        }
    }
}