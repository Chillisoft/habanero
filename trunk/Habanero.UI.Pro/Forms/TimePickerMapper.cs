using System;
using System.Windows.Forms;
using Habanero.Ui.Forms;
using MyTimePicker;

namespace Habanero.Ui.Forms
{
    /// <summary>
    /// Provides mapping for a TimePicker control
    /// </summary>
    public class TimePickerMapper : ControlMapper
    {
        private TimePicker _timePicker;
        //PropertyInfo _valuePropertyInfo;

        /// <summary>
        /// Constructor to initialise a new mapper
        /// </summary>
        /// <param name="dtp">The control to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnceOnly">Whether the object is read once only</param>
        public TimePickerMapper(Control dtp, string propName, bool isReadOnceOnly) : base(dtp, propName, isReadOnceOnly)
        {
            _timePicker = (TimePicker) dtp;
            _timePicker.ValueChanged += new EventHandler(ValueChangedHandler);
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
                object newValue = _timePicker.Value;
                object oldValue = _businessObject.GetPropertyValue(_propertyName);
                if (oldValue != null || newValue != null)
                {
                    if (oldValue == null || !oldValue.Equals(newValue))
                    {
                        TimeSpan newTime = (TimeSpan) newValue;
                        if (oldValue == null)
                        {
                            _businessObject.SetPropertyValue(_propertyName,
                                                             new DateTime(1, 1, 1, newTime.Hours, newTime.Minutes,
                                                                          newTime.Seconds));
                        }
                        else
                        {
                            DateTime oldDateTime = (DateTime) oldValue;
                            DateTime newDateTime =
                                new DateTime(oldDateTime.Year, oldDateTime.Month, oldDateTime.Day, newTime.Hours,
                                             newTime.Minutes, newTime.Seconds);
                            _businessObject.SetPropertyValue(_propertyName, newDateTime);
                        }
                    }
                }
            }
        }

//
//		private object GetValueOfDateTimePicker() 
//		{
//			return DateTimePickerUtil.GetValue(_timePicker); 
//		}

//		private void SetValueOfDateTimePicker(object value)
//		{
//				try 
//		 {
//			 object dtpValue = GetValueOfDateTimePicker();
//			 if (dtpValue != null && dtpValue.Equals(value)) return;
//			 if (dtpValue == null && value == null) return;
//			 DateTimePickerUtil.SetValue(_timePicker, value); 
//			
//		 }
//		 catch(TargetInvocationException  ex) 
//		 {
//			 MessageBox.Show(ExceptionUtil.GetExceptionString(ex.InnerException,0)   );   
//				
//		 }
//		    
//		}

        /// <summary>
        /// Updates the interface when the value has been changed in the
        /// object being represented
        /// </summary>
        protected override void ValueUpdated()
        {
            object propValue = GetPropertyValue();
            if (propValue == null)
            {
                _timePicker.Text = "";
            }
            else
            {
                if (propValue is DateTime)
                {
                    _timePicker.Value = ((DateTime) propValue).TimeOfDay;
                }
                else if (propValue is TimeSpan)
                {
                    _timePicker.Value = (TimeSpan) propValue;
                }
            }
            //			if (_businessObject.GetPropertyValue(_propertyName) == null) {
            //				itsDateTimePicker.Text = "";
            //			}
            //			else {
            //				//itsDateTimePicker.Value = (DateTime)_businessObject.GetPropertyValue(_propertyName);
            //				SetValueOfDateTimePicker(_businessObject.GetPropertyValue(_propertyName));
            //			}
        }
    }
}