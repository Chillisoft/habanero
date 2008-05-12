using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    internal class DateTimePickerGiz : DateTimePicker, IDateTimePicker
    {
        IList IChilliControl.Controls
        {
            get { return base.Controls; }
        }
    }
}