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
using System.Windows.Forms;
using Habanero.UI.Util;

namespace Habanero.UI.Forms
{
	/// <summary>
    /// Manages the facility to set dates and times in the user interface
    /// </summary>
	public class DateTimePickerNullableMapper : ControlMapper
	{
        private Control _dateTimePicker;
		private DateTimePickerController _dateTimePickerController;
		
		/// <summary>
        /// Constructor to initialise a new instance of the class and assign
        /// an event handler to deal with changed values
        /// </summary>
        /// <param name="dtp">The DateTimePicker control object</param>
        /// <param name="propName">The property name</param>
		/// <param name="isReadOnly">Whether this control is read only</param>
		public DateTimePickerNullableMapper(Control dtp, string propName, bool isReadOnly)
            : base(dtp, propName, isReadOnly)
        {
            _dateTimePicker = dtp;
			_dateTimePickerController = new DateTimePickerController(_dateTimePicker);
			_dateTimePickerController.ValueChanged += ValueChangedHandler;
		}

        /// <summary>
        /// Initialises the control using the attributes already provided
        /// </summary>
        protected override void InitialiseWithAttributes()
        {
            if (_attributes.Contains("dateFormat"))
            {
                String dateFormat = (string)_attributes["dateFormat"];
                DateTimePickerUtil.SetCustomFormat(_dateTimePicker, dateFormat);
            }
            if (_attributes.Contains("showUpDown"))
            {
                bool showUpDown = Convert.ToBoolean(_attributes["showUpDown"]);
                DateTimePickerUtil.SetShowUpDown(_dateTimePicker, showUpDown);
            }
            base.InitialiseWithAttributes();
        }

		///<summary>
		/// The DateTimePickerController that controls the DateTimePicker
		///</summary>
		public DateTimePickerController DateTimePickerController
		{
			get { return _dateTimePickerController; }
			set { _dateTimePickerController = value; }
		}

		private void ValueChangedHandler(object sender, EventArgs e)
        {
			if (!_isEditable) return;

        	DateTime? newValue = _dateTimePickerController.Value;
			object oldValue = GetPropertyValue();
			if ( oldValue == null || !oldValue.Equals(newValue))
			{
                SetPropertyValue(newValue);
                //_businessObject.SetPropertyValue(_propertyName, newValue);
			}
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
				_dateTimePickerController.Value = null;
            }
            else
            {
                _dateTimePickerController.Value = Convert.ToDateTime(propValue);
            }
        }
	}
}
