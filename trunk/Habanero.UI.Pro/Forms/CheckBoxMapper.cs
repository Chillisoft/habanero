using System;
using System.Windows.Forms;

namespace Habanero.Ui.BoControls
{
    /// <summary>
    /// This mapper represents a CheckBox object in a user interface
    /// </summary>
    public class CheckBoxMapper : ControlMapper
    {
        private CheckBox _checkBox;

        /// <summary>
        /// Constructor to create a new CheckBox mapper object
        /// </summary>
        /// <param name="cb">The CheckBox object to be mapped</param>
        /// <param name="propName">A name for the property</param>
        /// <param name="isReadOnceOnly">Whether this object can be read once only</param>
        public CheckBoxMapper(CheckBox cb, string propName, bool isReadOnceOnly) : base(cb, propName, isReadOnceOnly)
        {
            _checkBox = cb;
            _checkBox.CheckedChanged += new EventHandler(ValueChangedHandler);
        }

        /// <summary>
        /// The event handler that is called when the user has checked or
        /// unchecked the CheckBox.
        /// </summary>
        /// <param name="sender">The object that sent the message about a
        /// change in value</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void ValueChangedHandler(object sender, EventArgs e)
        {
            if (_isReadOnceOnly)
            {
                return;
            }
            bool newValue = _checkBox.Checked;
            bool valueChanged = false;
            if (GetPropertyValue() == null)
            {
                valueChanged = true;
            }
            else
            {
                bool oldValue = Convert.ToBoolean(GetPropertyValue());
                if (newValue != oldValue)
                {
                    valueChanged = true;
                }
            }
            if (valueChanged)
            {
                //log.Debug("setting property value to " + newValue + " of type bool");
                _businessObject.SetPropertyValue(_propertyName, newValue);
            }
        }

        /// <summary>
        /// Updates the appearance of the object when the value of the
        /// property has changed internally
        /// </summary>
        protected override void ValueUpdated()
        {
            object propValue = GetPropertyValue();
            bool newValue;
            if (propValue != null)
            {
                newValue = Convert.ToBoolean(propValue);
            }
            else
            {
                newValue = false;
            }
            if (newValue != _checkBox.Checked)
            {
                _checkBox.Checked = newValue;
            }
        }
    }
}