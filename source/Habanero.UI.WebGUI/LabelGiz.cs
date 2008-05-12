using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI;

namespace Habanero.UI.WebGUI
{
    public class LabelGiz : Label, ILabel
    {
        public LabelGiz(string labelText)
        {
            base.Text = labelText;
        }

        public LabelGiz()
        {
        }

        private const int WIDTH_FACTOR = 8;
        public int PreferredWidth
        {
            get { return this.Text.Length * WIDTH_FACTOR; }
        }
        IList IChilliControl.Controls
        {
            get { return this.Controls; }
        }
        //List<IChilliControl> IChilliControl.Controls
        //{
        //    get
        //    {
        //        return new List<IChilliControl>();
        //    }
        //}
    }
}