using System;
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class NumericUpDownGiz :NumericUpDown, INumericUpDown
    {
        public IList Controls
        {
            get { throw new NotImplementedException(); }
        }

        public void Select(int i, object length)
        {
            throw new NotImplementedException();
        }
    }
}