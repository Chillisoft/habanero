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

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Represents a DateTimePicker control
    /// </summary>
    public class DateTimePickerVWG : DateTimePicker, IDateTimePicker
    {
        private readonly DateTimePickerManager _manager;

        public DateTimePickerVWG(IControlFactory controlFactory)
        {
            DateTimePickerManager.ValueGetter<DateTime> valueGetter = delegate()
            {
                return base.Value;
            };
            DateTimePickerManager.ValueSetter<DateTime> valueSetter = delegate(DateTime value)
            {
                base.Value = value;
            };
            _manager = new DateTimePickerManager(controlFactory, this, valueGetter, valueSetter);
            //Note: by default the Checkbox is shown because the VWG DateTimePicker does not support representing the null state visually without it.
            this.ShowCheckBox = true;
            _manager.ChangeToNullMode();
        }

        /// <summary>
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        Base.AnchorStyles IControlHabanero.Anchor
        {
            get { return (Base.AnchorStyles)base.Anchor; }
            set { base.Anchor = (Gizmox.WebGUI.Forms.AnchorStyles)value; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionVWG(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlHabanero.Dock
        {
            get { return (Base.DockStyle)base.Dock; }
            set { base.Dock = (Gizmox.WebGUI.Forms.DockStyle)value; }
        }

        /// <summary>
        /// Gets or sets the date/time value assigned to the control.
        /// </summary>
        DateTime IDateTimePicker.Value
        {
            get { return _manager.Value; }
            set { _manager.Value = value; }
        }



        /// <summary>
        /// Gets or sets a value indicating whether the Value property has
        /// been set with a valid date/time value and the displayed value is able to be updated
        /// </summary>
        bool IDateTimePicker.Checked
        {
            get { return base.Checked; }
            set
            {
                base.Checked = value;
                if (_manager != null) _manager.OnValueChanged(new EventArgs());
            }
        }

        protected override void OnValueChanged(EventArgs eventargs)
        {
            _manager.OnValueChanged(eventargs);
            base.OnValueChanged(eventargs);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            _manager.OnResize(eventargs);
        }

        protected override void OnClick(EventArgs eventargs)
        {
            base.OnClick(eventargs);
            _manager.ChangeToValueMode();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e == null) return;
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                if (_manager.ChangeToNullMode())
                {
                    e.Handled = true;
                    return;
                }
            }
            else
            {
                if (_manager.ChangeToValueMode())
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            _manager.UpdateFocusState();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            _manager.UpdateFocusState();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _manager.UpdateFocusState();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            _manager.UpdateFocusState();
        }

        /// <summary>
        /// Occurs when the Value property changes
        /// </summary>
        event EventHandler IDateTimePicker.ValueChanged
        {
            add { _manager.ValueChanged += value; }
            remove { _manager.ValueChanged -= value; }
        }

        /// <summary>
        /// Gets or sets the date/time value assigned to the control, but returns
        /// null if there is no date set in the picker
        /// </summary>
        public DateTime? ValueOrNull
        {
            get { return _manager.ValueOrNull; }
            set { _manager.ValueOrNull = value; }
        }

        ///<summary>
        /// The text that will be displayed when the Value is null
        ///</summary>
        public string NullDisplayValue
        {
            get { return _manager.NullDisplayValue; }
            set { _manager.NullDisplayValue = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a spin button control
        /// (also known as an up-down control) is used to adjust the date/time value
        /// </summary>
        public bool ShowUpDown
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the format of the date and time displayed in the control.
        /// </summary>
        ///	<returns>One of the <see cref="Base.DateTimePickerFormat"></see> values. The default is <see cref="Base.DateTimePickerFormat.Long"></see>.</returns>
        ///	<exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="Base.DateTimePickerFormat"></see> values. </exception>
        Base.DateTimePickerFormat IDateTimePicker.Format
        {
            get { return (Base.DateTimePickerFormat)base.Format; }
            set { base.Format = (Gizmox.WebGUI.Forms.DateTimePickerFormat)value; }
        }
    }
}