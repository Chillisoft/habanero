//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

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
            get { return new ControlCollectionGiz(base.Controls); }
        }
        Base.DockStyle IControlChilli.Dock
        {
            get { return (Base.DockStyle)base.Dock; }
            set { base.Dock = (Gizmox.WebGUI.Forms.DockStyle)value; }
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

        public bool ShowUpDown
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}