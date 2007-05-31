using System;
using System.Windows.Forms;
using log4net;

namespace Chillisoft.UI.BOControls.v2
{
    /// <summary>
    /// A mapper for the numeric up-down facility where the values are monetary
    /// </summary>
    public class NumericUpDownMoneyMapper : NumericUpDownMapper
    {
        private static readonly new ILog log =
            LogManager.GetLogger("Chillisoft.UI.BOControls.v2.NumericUpDownMoneyMapper");

        /// <summary>
        /// Constructor to initialise the mapper
        /// </summary>
        /// <param name="control">The control to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnceOnly">Whether the object is read once only</param>
        public NumericUpDownMoneyMapper(NumericUpDown control, string propName, bool isReadOnceOnly)
            : base(control, propName, isReadOnceOnly)
        {
            itsNumericUpDown.DecimalPlaces = 2;
            itsNumericUpDown.Maximum = Int32.MaxValue;
            itsNumericUpDown.Minimum = Int32.MinValue;
            itsNumericUpDown.ValueChanged += new EventHandler(ValueChangedHandler);
            itsNumericUpDown.Leave += new EventHandler(ValueChangedHandler);
        }

        /// <summary>
        /// Sets up the control to the number of decimal places specified in
        /// the attributes
        /// </summary>
        protected override void InitialiseWithAttributes()
        {
            if (itsAttributes.ContainsKey("decimalPlaces"))
            {
                int decimalPlaces = Convert.ToInt32(itsAttributes["decimalPlaces"]);
                itsNumericUpDown.DecimalPlaces = decimalPlaces;
            }
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
                decimal newValue = Convert.ToDecimal(itsNumericUpDown.Value);
                decimal oldValue = Convert.ToDecimal(itsBusinessObject.GetPropertyValue(itsPropertyName));
                if (newValue != oldValue)
                {
                    //log.Debug("setting property value to " + itsNumericUpDown.Value + " of type " + itsNumericUpDown.Value.GetType().Name);
                    itsBusinessObject.SetPropertyValue(itsPropertyName, newValue);
                }
            }
        }
    }
}