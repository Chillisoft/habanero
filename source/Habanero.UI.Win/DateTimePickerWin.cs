using System.Collections;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class DateTimePickerWin : DateTimePicker, IDateTimePicker
    {
        IList IControlChilli.Controls
        {
            get { return base.Controls; }
        }
    }
}