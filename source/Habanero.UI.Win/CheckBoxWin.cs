using System.Collections;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    internal class CheckBoxWin : CheckBox, ICheckBox
    {
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }
    }
}