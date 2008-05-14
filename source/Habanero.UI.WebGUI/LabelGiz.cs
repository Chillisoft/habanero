using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

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
        IList IControlChilli.Controls
        {
            get { return this.Controls; }
        }
        //List<IControlChilli> IControlChilli.Controls
        //{
        //    get
        //    {
        //        return new List<IControlChilli>();
        //    }
        //}
    }
}