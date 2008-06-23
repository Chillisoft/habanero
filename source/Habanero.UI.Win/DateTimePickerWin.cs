using System;
using System.Collections;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class DateTimePickerWin : DateTimePicker, IDateTimePicker
    {
        private readonly DateTimePickerManager _manager;

        public DateTimePickerWin(IControlFactory controlFactory)
        {
            _manager = new DateTimePickerManager(controlFactory, this);
        }
        
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        public DateTime? ValueOrNull
        {
            get { return _manager.ValueOrNull; }
            set { _manager.ValueOrNull = value; }
        }
    }
}