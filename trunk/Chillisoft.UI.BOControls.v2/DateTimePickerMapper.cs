using System;
using System.Reflection;
using System.Windows.Forms;
using Chillisoft.Generic.v2;
using Chillisoft.UI.Misc.v2;

namespace Chillisoft.UI.BOControls.v2
{
    /// <summary>
    /// Manages the facility to set dates and times in the user interface
    /// </summary>
    public class DateTimePickerMapper : ControlMapper
    {
        private Control itsDateTimePicker;
        //PropertyInfo itsValuePropertyInfo;

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
            itsDateTimePicker = dtp;
            EventInfo valueChangedEventInfo = dtp.GetType().GetEvent("ValueChanged");
            valueChangedEventInfo.AddEventHandler(itsDateTimePicker, new EventHandler(ValueChangedHandler));
            //itsValuePropertyInfo = _dateTimePicker.GetType().GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);			
            //itsDateTimePicker.ValueChanged += new EventHandler(ValueChangedHandler);
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
                //DateTime newValue = itsDateTimePicker.Value;
                object newValue = GetValueOfDateTimePicker();
                object oldValue = itsBusinessObject.GetPropertyValue(itsPropertyName);
                if (oldValue != null || newValue != null)
                {
                    if (oldValue == null || !oldValue.Equals(newValue))
                    {
                        itsBusinessObject.SetPropertyValue(itsPropertyName, newValue);
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
            return DateTimePickerUtil.GetValue(itsDateTimePicker);
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
                DateTimePickerUtil.SetValue(itsDateTimePicker, value);
            }
            catch (TargetInvocationException  ex)
            {
                MessageBox.Show(ExceptionUtil.GetExceptionString(ex.InnerException, 0));
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
                itsDateTimePicker.Text = "";
            }
            else
            {
                SetValueOfDateTimePicker(Convert.ToDateTime(propValue));
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