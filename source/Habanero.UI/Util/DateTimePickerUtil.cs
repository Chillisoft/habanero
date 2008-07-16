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
using System.Reflection;
using System.Windows.Forms;

namespace Habanero.UI.Util
{
    /// <summary>
    /// Gets and sets date-time values in any given date-time picker
    /// </summary>
    public class DateTimePickerUtil
    {
        /// <summary>
        /// Gets a date-time value from the provided picker
        /// </summary>
        /// <param name="dateTimePicker">A date-time picker</param>
        /// <returns>Returns the DateTime value or null if none was chosen</returns>
        public static object GetValue(Control dateTimePicker)
        {
            PropertyInfo propInfo =
                dateTimePicker.GetType().GetProperty("Value", BindingFlags.Instance | BindingFlags.Public);
            object val = propInfo.GetValue(dateTimePicker, new object[] {});
            if (val != null)
            {
                return (DateTime) val;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the date-time value in the specified date-time picker
        /// </summary>
        /// <param name="dateTimePicker">The date-time picker</param>
        /// <param name="date">The date to set to</param>
        public static void SetValue(Control dateTimePicker, DateTime date)
        {
            PropertyInfo propInfo =
                dateTimePicker.GetType().GetProperty("Value", BindingFlags.Instance | BindingFlags.Public);
            propInfo.SetValue(dateTimePicker, date, new object[] {});
        }

        /// <summary>
        /// Sets the date-time value in the specified date-time picker
        /// </summary>
        /// <param name="dateTimePicker">The date-time picker</param>
        /// <param name="dateString">The date value as a string</param>
        public static void SetValue(Control dateTimePicker, string dateString)
        {
            object dateValue;
            if (dateString == null || dateString.Length == 0)
            {
                dateValue = null;
            }
            else
            {
                dateValue = Convert.ToDateTime(dateString);
            }
            PropertyInfo propInfo =
                dateTimePicker.GetType().GetProperty("Value", BindingFlags.Instance | BindingFlags.Public);
            propInfo.SetValue(dateTimePicker, dateValue, new object[] {});
        }

        /// <summary>
        /// Sets the date-time value in the specified date-time picker
        /// </summary>
        /// <param name="dateTimePicker">The date-time picker</param>
        /// <param name="dateValue">The date value as either a string or as
        /// a DateTime object</param>
        /// <exception cref="ArgumentException">Thrown if the date value is neither
        /// a string nor a DateTime object</exception>
        public static void SetValue(Control dateTimePicker, object dateValue)
        {
            if (dateValue is DateTime)
            {
                SetValue(dateTimePicker, (DateTime) dateValue);
            }
            else if (dateValue is string)
            {
                SetValue(dateTimePicker, (string) dateValue);
            }
            else if (dateValue == null)
            {
                SetValue(dateTimePicker, (string) dateValue);
            }
            else
            {
                throw new ArgumentException(
                    "Invalid type when setting the value of a datetimepicker. Only dateTime, string, and null supported.");
            }
        }

        /// <summary>
        /// Specify the custom format for the given date-time picker
        /// </summary>
        /// <param name="dateTimePicker">The date-time picker</param>
        /// <param name="customFormat">The custom format to set for the date-time picker</param>
        public static void SetCustomFormat(Control dateTimePicker, string customFormat)
        {
            if (dateTimePicker.GetType().Name == "UltraDateTimeEditor")
            {
                PropertyInfo propInfo =
                    dateTimePicker.GetType().GetProperty("FormatString", BindingFlags.Instance | BindingFlags.Public);
                propInfo.SetValue(dateTimePicker, customFormat, new object[] { });
            }
            else if (dateTimePicker is DateTimePicker)
            {
                DateTimePicker picker = (DateTimePicker)dateTimePicker;
                picker.Format = DateTimePickerFormat.Custom;
                picker.CustomFormat = customFormat;
            }
        }

        /// <summary>
        /// Specify the time format in the given date-time picker
        /// </summary>
        /// <param name="dateTimePicker">The date-time picker</param>
        public static void SetTimeFormat(Control dateTimePicker)
        {
            if (dateTimePicker.GetType().Name == "UltraDateTimeEditor")
            {
                PropertyInfo propInfo =
                    dateTimePicker.GetType().GetProperty("FormatString", BindingFlags.Instance | BindingFlags.Public);
                propInfo.SetValue(dateTimePicker, "hh:mm:ss", new object[] { });
            }
            else if (dateTimePicker is DateTimePicker)
            {
                DateTimePicker picker = (DateTimePicker)dateTimePicker;
                picker.Format = DateTimePickerFormat.Time;
            }
        }

        /// <summary>
        /// Specify the time format in the given date-time picker
        /// </summary>
        /// <param name="dateTimePicker">The date-time picker</param>
        /// <param name="showUpDown">Specifies if the Up/Down control must be shown or not</param>
        public static void SetShowUpDown(Control dateTimePicker, bool showUpDown)
        {
            if (dateTimePicker is DateTimePicker)
            {
                DateTimePicker picker = (DateTimePicker)dateTimePicker;
                picker.ShowUpDown = showUpDown;
            }
        }

        ///<summary>
		/// Adds a ValueChanged handler for the date-time picker
		///</summary>
		///<param name="dateTimePicker">The date-time picker</param>
		///<param name="eventHandler">The Handler to add</param>
		public static void AddValueChangedHandler(Control dateTimePicker, EventHandler eventHandler)
		{
			EventInfo valueChangedEventInfo = dateTimePicker.GetType().GetEvent("ValueChanged");
			valueChangedEventInfo.AddEventHandler(dateTimePicker, eventHandler);
		}

		///<summary>
		/// Removes a ValueChanged handler for the date-time picker
		///</summary>
		///<param name="dateTimePicker">The date-time picker</param>
		///<param name="eventHandler">The Handler to remove</param>
		public static void RemoveValueChangedHandler(Control dateTimePicker, EventHandler eventHandler)
		{
			EventInfo valueChangedEventInfo = dateTimePicker.GetType().GetEvent("ValueChanged");
			valueChangedEventInfo.RemoveEventHandler(dateTimePicker, eventHandler);
		}

    }
}