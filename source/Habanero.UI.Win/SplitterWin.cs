using System;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class SplitterWin : Splitter, ISplitter
    {
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(this.Controls); }
        }
        Base.DockStyle IControlChilli.Dock
        {
            get { return (Base.DockStyle)base.Dock; }
            set { base.Dock = (System.Windows.Forms.DockStyle)value; }
        }
    }
}