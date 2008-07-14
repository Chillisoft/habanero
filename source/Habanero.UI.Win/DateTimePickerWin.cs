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
            DateTimePickerManager.ValueGetter valueGetter = delegate()
            {
                return base.Value;
            };
            DateTimePickerManager.ValueSetter valueSetter = delegate(DateTime value)
            {
                base.Value = value;
            };
            _manager = new DateTimePickerManager(controlFactory, this, valueGetter, valueSetter);
        }
        
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }
        Base.DockStyle IControlChilli.Dock
        {
            get { return (Base.DockStyle)base.Dock; }
            set { base.Dock = (System.Windows.Forms.DockStyle)value; }
        }

        DateTime IDateTimePicker.Value
        {
            get { return _manager.Value; }
            set { _manager.Value = value; }
        }

        //protected override void OnValueChanged(EventArgs eventargs)
        //{
        //    _manager.OnValueChanged(eventargs);
        //    base.OnValueChanged(eventargs);
        //}

        public DateTime? ValueOrNull
        {
            get { return _manager.ValueOrNull; }
            set { _manager.ValueOrNull = value; }
        }
    }
}