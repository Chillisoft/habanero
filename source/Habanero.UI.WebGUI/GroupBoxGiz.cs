using System;
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class GroupBoxGiz : GroupBox, IGroupBox
    {
        public IList Controls
        {
            get { throw new NotImplementedException(); }
        }
    }
}