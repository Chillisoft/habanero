using System;
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class TabControlGiz : TabControl, ITabControl
    {
        public new IList Controls
        {
            get { throw new NotImplementedException(); }
        }
        //TODO: Convert dockstyles between Giz windows etc
        public new IDockStyle Dock
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public new ITabPageCollection TabPages
        {
            get { throw new NotImplementedException(); }
        }
    }
}