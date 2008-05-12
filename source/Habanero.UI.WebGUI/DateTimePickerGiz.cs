using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI;

namespace Habanero.UI.WebGUI
{
    public class DateTimePickerGiz : DateTimePicker, IDateTimePicker
    {
        IList IChilliControl.Controls
        {
            get { return base.Controls; }
        }
    }
}