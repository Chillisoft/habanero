using System;
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class TreeViewGiz : TreeView, ITreeView
    {
        public new IList Controls
        {
            get { throw new NotImplementedException(); }
        }
    }
}