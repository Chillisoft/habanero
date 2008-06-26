using System;
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

        private const int WIDTH_FACTOR = 6;
        public int PreferredWidth
        {
            get { return this.Text.Length * WIDTH_FACTOR; }
        }
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }
    }
}