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
    }
}