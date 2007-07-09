using System;
using System.Reflection;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Ui.Forms;
using Habanero.Ui.Util;

namespace Habanero.Ui.Forms
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
        /// <param name="isReadOnceOnly">Whether the control is read once only</param>
        public DateTimePickerMapper(Control dtp, string propName, bool isReadOnceOnly)
            : base(dtp, propName, isReadOnceOnly)
        {
            _dateTimePicker = dtp;
            EventInfo valueChangedEventInfo = dtp.GetType().GetEvent("ValueChanged");
            valueChangedEventInfo.AddEventHandler(_dateTimePicker, new EventHandler(ValueChangedHandler));
            //_valuePropertyInfo = _dateTimePicker.GetType().GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);			
            //_dateTimePicker.ValueChanged += new EventHandler(ValueChangedHandler);
        }

        /// <summary>
        /// A handler to carry out changes to the business object when the
        /// value has changed in the user interface
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void ValueChangedHandler(object sender, EventArgs e)
        {
            if (_businessObject != null && !_isReadOnceOnly)
            {
                //DateTime newValue = _dateTimePicker.Value;
                object newValue = GetValueOfDateTimePicker();
                object oldValue = _businessObject.GetPropertyValue(_propertyName);
                if (oldValue != null || newValue != null)
                {
                    if (oldValue == null || !oldValue.Equals(newValue))
                    {
                        _businessObject.SetPropertyValue(_propertyName, newValue);
                    }
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

        /// <summary>
        /// Updates the interface when the value has been changed in the
        /// object being represented
        /// </summary>
        protected override void ValueUpdated()
        {
            object propValue = GetPropertyValue();
            if (propValue == null)
            {
                _dateTimePicker.Text = "";
            }
            else
            {
                SetValueOfDateTimePicker(Convert.ToDateTime(propValue));
            }
//			if (_businessObject.GetPropertyValue(_propertyName) == null) {
//				_dateTimePicker.Text = "";
//			}
//			else {
//				//_dateTimePicker.Value = (DateTime)_businessObject.GetPropertyValue(_propertyName);
//				SetValueOfDateTimePicker(_businessObject.GetPropertyValue(_propertyName));
//			}
        }
    }
}