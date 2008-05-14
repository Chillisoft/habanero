using System;
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class SplitterGiz : Splitter, ISplitter
    {
        public IList Controls
        {
            get { throw new NotImplementedException(); }
        }
    }
}