using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class NumericUpDownWin : NumericUpDown, INumericUpDown
    {
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

    }
}