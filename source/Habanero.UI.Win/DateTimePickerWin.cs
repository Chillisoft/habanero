using System.Collections;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    internal class DateTimePickerWin : DateTimePicker, IDateTimePicker
    {
        IList IChilliControl.Controls
        {
            get { return base.Controls; }
        }
    }
}