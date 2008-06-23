using System;
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class DateTimePickerGiz : DateTimePicker, IDateTimePicker
    {
        private readonly DateTimePickerManager _manager;

        public DateTimePickerGiz(IControlFactory controlFactory)
        {
            _manager = new DateTimePickerManager(controlFactory, this);
        }

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }

        public DateTime? ValueOrNull
        {
            get { return _manager.ValueOrNull; }
            set { _manager.ValueOrNull = value; }
        }

        public bool ShowUpDown
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }
    }
}