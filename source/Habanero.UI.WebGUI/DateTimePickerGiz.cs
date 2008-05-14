using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class DateTimePickerGiz : DateTimePicker, IDateTimePicker
    {
        IList IControlChilli.Controls
        {
            get { return base.Controls; }
        }
    }
}