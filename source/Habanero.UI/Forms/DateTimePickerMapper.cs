//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Reflection;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.UI.Forms;
using Habanero.UI.Util;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Manages the facility to set dates and times in the user interface
    /// </summary>
    public class DateTimePickerMapper : ControlMapper
    {
        private Control _dateTimePicker;
        //PropertyInfo _valuePropertyInfo;

        /// <summary>
        /// Constructor to initialise a new instance of the class and assign
        /// an event handler to deal with changed values
        /// </summary>
        /// <param name="dtp">The DateTimePicker control object</param>
        /// <param name="propName">The property name</param>
		/// <param name="isReadOnly">Whether this control is read only</param>
		public DateTimePickerMapper(Control dtp, string propName, bool isReadOnly)
            : base(dtp, propName, isReadOnly)
        {
            _dateTimePicker = dtp;
        	AddValueChangedHandler(new EventHandler(ValueChangedHandler));
        }

        /// <summary>
        /// Initialises the control using the attributes already provided
        /// </summary>
        protected override void InitialiseWithAttributes()
        {
            if (_attributes.Contains("dateFormat"))
            {
                String dateFormat = Convert.ToString(_attributes["dateFormat"]);
                DateTimePickerUtil.SetCustomFormat(_dateTimePicker, dateFormat);
            }
            if (_attributes.Contains("showUpDown"))
            {
                bool showUpDown = Convert.ToBoolean(_attributes["showUpDown"]);
                DateTimePickerUtil.SetShowUpDown(_dateTimePicker, showUpDown);
            }
            base.InitialiseWithAttributes();
        }

    	/// <summary>
        /// A handler to carry out changes to the business object when the
        /// value has changed in the user interface
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
		private void ValueChangedHandler(object sender, EventArgs e)
        {
			if (!_isEditable) return;

        	//DateTime newValue = _dateTimePicker.Value;
        	object newValue = GetValueOfDateTimePicker();
			object oldValue = GetPropertyValue();
        	if (oldValue != null || newValue != null)
        	{
        		if (oldValue == null || !oldValue.Equals(newValue))
        		{
                    SetPropertyValue(newValue);
        			//_businessObject.SetPropertyValue(_propertyName, newValue);
        		}
        	}
        }

    	/// <summary>
        /// Returns the value currently held by the picker
        /// </summary>
        /// <returns>Returns the value held</returns>
        private object GetValueOfDateTimePicker()
        {
            return DateTimePickerUtil.GetValue(_dateTimePicker);
        }

        /// <summary>
        /// Sets the value held by the picker interface
        /// </summary>
        /// <param name="value">The value to set to</param>
        private void SetValueOfDateTimePicker(object value)
        {
            try
            {
                object dtpValue = GetValueOfDateTimePicker();
                if (dtpValue != null && dtpValue.Equals(value)) return;
                if (dtpValue == null && value == null) return;
                DateTimePickerUtil.SetValue(_dateTimePicker, value);
            }
            catch (TargetInvocationException  ex)
            {
                MessageBox.Show(ExceptionUtilities.GetExceptionString(ex.InnerException, 0, true));
            }
        }

		private void AddValueChangedHandler(EventHandler eventHandler)
		{
			DateTimePickerUtil.AddValueChangedHandler(_dateTimePicker, eventHandler);
		}

        /// <summary>
        /// Updates the interface when the value has been changed in the
        /// object being represented
        /// </summary>
        protected override void ValueUpdated()
        {
            object propValue = GetPropertyValue();
            if (propValue == null || propValue == DBNull.Value)
            {
                _dateTimePicker.Text = "";
            }
            else
            {
                SetValueOfDateTimePicker(Convert.ToDateTime(propValue));
            }
        }

    }
}