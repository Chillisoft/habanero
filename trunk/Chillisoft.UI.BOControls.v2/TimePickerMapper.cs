using System;
using System.Windows.Forms;
using MyTimePicker;

namespace Chillisoft.UI.BOControls.v2
{
    /// <summary>
    /// Provides mapping for a TimePicker control
    /// </summary>
    public class TimePickerMapper : ControlMapper
    {
        private TimePicker itsTimePicker;
        //PropertyInfo itsValuePropertyInfo;

        /// <summary>
        /// Constructor to initialise a new mapper
        /// </summary>
        /// <param name="dtp">The control to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnceOnly">Whether the object is read once only</param>
        public TimePickerMapper(Control dtp, string propName, bool isReadOnceOnly) : base(dtp, propName, isReadOnceOnly)
        {
            itsTimePicker = (TimePicker) dtp;
            itsTimePicker.ValueChanged += new EventHandler(ValueChangedHandler);
        }

        /// <summary>
        /// A handler to carry out changes to the business object when the
        /// value has changed in the user interface
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void ValueChangedHandler(object sender, EventArgs e)
        {
            if (itsBusinessObject != null && !itsIsReadOnceOnly)
            {
                //DateTime newValue = _dateTimePicker.Value;
                object newValue = itsTimePicker.Value;
                object oldValue = itsBusinessObject.GetPropertyValue(itsPropertyName);
                if (oldValue != null || newValue != null)
                {
                    if (oldValue == null || !oldValue.Equals(newValue))
                    {
                        TimeSpan newTime = (TimeSpan) newValue;
                        if (oldValue == null)
                        {
                            itsBusinessObject.SetPropertyValue(itsPropertyName,
                                                               new DateTime(1, 1, 1, newTime.Hours, newTime.Minutes,
                                                                            newTime.Seconds));
                        }
                        else
                        {
                            DateTime oldDateTime = (DateTime) oldValue;
                            DateTime newDateTime =
                                new DateTime(oldDateTime.Year, oldDateTime.Month, oldDateTime.Day, newTime.Hours,
                                             newTime.Minutes, newTime.Seconds);
                            itsBusinessObject.SetPropertyValue(itsPropertyName, newDateTime);
                        }
                    }
                }
            }
        }

//
//		private object GetValueOfDateTimePicker() 
//		{
//			return DateTimePickerUtil.GetValue(itsTimePicker); 
//		}

//		private void SetValueOfDateTimePicker(object value)
//		{
//				try 
//		 {
//			 object dtpValue = GetValueOfDateTimePicker();
//			 if (dtpValue != null && dtpValue.Equals(value)) return;
//			 if (dtpValue == null && value == null) return;
//			 DateTimePickerUtil.SetValue(itsTimePicker, value); 
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
                itsTimePicker.Text = "";
            }
            else
            {
                if (propValue is DateTime)
                {
                    itsTimePicker.Value = ((DateTime) propValue).TimeOfDay;
                }
                else if (propValue is TimeSpan)
                {
                    itsTimePicker.Value = (TimeSpan) propValue;
                }
            }
            //			if (itsBusinessObject.GetPropertyValue(itsPropertyName) == null) {
            //				itsDateTimePicker.Text = "";
            //			}
            //			else {
            //				//itsDateTimePicker.Value = (DateTime)itsBusinessObject.GetPropertyValue(itsPropertyName);
            //				SetValueOfDateTimePicker(itsBusinessObject.GetPropertyValue(itsPropertyName));
            //			}
        }
    }
}