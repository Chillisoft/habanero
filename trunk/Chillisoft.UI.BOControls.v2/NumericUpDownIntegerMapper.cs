using System;
using System.Windows.Forms;
using log4net;

namespace Chillisoft.UI.BOControls.v2
{
    /// <summary>
    /// A mapper for the numeric up-down facility where the values are integers
    /// </summary>
    public class NumericUpDownIntegerMapper : NumericUpDownMapper
    {
        private static readonly new ILog log =
            LogManager.GetLogger("Chillisoft.UI.BOControls.v2.NumericUpDownIntegerMapper");

        /// <summary>
        /// Constructor to initialise the mapper
        /// </summary>
        /// <param name="control">The control to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnceOnly">Whether the object is read once only</param>
        public NumericUpDownIntegerMapper(NumericUpDown control, string propName, bool isReadOnceOnly)
            : base(control, propName, isReadOnceOnly)
        {
            itsNumericUpDown.DecimalPlaces = 0;
            itsNumericUpDown.Maximum = Int32.MaxValue;
            itsNumericUpDown.Minimum = Int32.MinValue;
            itsNumericUpDown.ValueChanged += new EventHandler(ValueChangedHandler);
            itsNumericUpDown.Leave += new EventHandler(ValueChangedHandler);
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
                int newValue = Convert.ToInt32(itsNumericUpDown.Value);
                int oldValue = Convert.ToInt32(itsBusinessObject.GetPropertyValue(itsPropertyName));
                if (newValue != oldValue)
                {
                    //log.Debug("setting property value to " + itsNumericUpDown.Value + " of type " + itsNumericUpDown.Value.GetType().Name);
                    itsBusinessObject.SetPropertyValue(itsPropertyName, newValue);
                }
            }
        }
    }
}