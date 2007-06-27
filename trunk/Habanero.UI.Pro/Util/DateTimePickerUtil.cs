using System;
using System.Reflection;
using System.Windows.Forms;

namespace Habanero.Ui.Util
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
        /// Specify the time format in the given date-time picker
        /// </summary>
        /// <param name="dateTimePicker">The date-time picker</param>
        public static void SetTimeFormat(Control dateTimePicker)
        {
            if (dateTimePicker.GetType().Name == "UltraDateTimeEditor")
            {
                PropertyInfo propInfo =
                    dateTimePicker.GetType().GetProperty("FormatString", BindingFlags.Instance | BindingFlags.Public);
                propInfo.SetValue(dateTimePicker, "hh:mm:ss", new object[] {});
            }
            else if (dateTimePicker is DateTimePicker)
            {
                DateTimePicker picker = (DateTimePicker) dateTimePicker;
                picker.Format = DateTimePickerFormat.Time;
            }
        }
    }
}