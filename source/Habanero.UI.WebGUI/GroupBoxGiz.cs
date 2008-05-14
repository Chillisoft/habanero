using System;
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class GroupBoxGiz : GroupBox, IGroupBox
    {
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }
    }
}