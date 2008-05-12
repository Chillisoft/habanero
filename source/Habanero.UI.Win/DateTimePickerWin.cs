using System.Collections;
using System.Windows.Forms;
using Habanero.UI;

namespace Habanero.UI.Win
{
    public class DateTimePickerWin : DateTimePicker, IDateTimePicker
    {
        IList IChilliControl.Controls
        {
            get { return base.Controls; }
        }
    }
}