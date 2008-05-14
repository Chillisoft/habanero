using System;
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    internal class TabPageGiz : TabPage, ITabPage
    {
        public TabPageGiz()
        {
   

        }
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }
    }
}