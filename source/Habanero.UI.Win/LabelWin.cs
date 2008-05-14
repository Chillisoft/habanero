using System.Collections;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    internal class LabelWin : Label, ILabel
    {
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }
    }
}