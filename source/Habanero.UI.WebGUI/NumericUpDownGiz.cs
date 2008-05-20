using System;
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class NumericUpDownGiz :NumericUpDown, INumericUpDown
    {
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }

  
        public void Select(int i, object length)
        {
            throw new NotImplementedException();
        }
    }
}