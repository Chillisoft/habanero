using System;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.UI.Forms;
using log4net;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// A mapper for the numeric up-down facility where the values are monetary
    /// </summary>
    public class NumericUpDownMoneyMapper : NumericUpDownMapper
    {
        private static readonly new ILog log =
            LogManager.GetLogger("Habanero.UI.Forms.NumericUpDownMoneyMapper");

        /// <summary>
        /// Constructor to initialise the mapper
        /// </summary>
        /// <param name="control">The control to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnceOnly">Whether the object is read once only</param>
        public NumericUpDownMoneyMapper(NumericUpDown control, string propName, bool isReadOnceOnly)
            : base(control, propName, isReadOnceOnly)
        {
            _numericUpDown.DecimalPlaces = 2;
            _numericUpDown.Maximum = Int32.MaxValue;
            _numericUpDown.Minimum = Int32.MinValue;
            _numericUpDown.ValueChanged += new EventHandler(ValueChangedHandler);
            _numericUpDown.Leave += new EventHandler(ValueChangedHandler);
        }

        /// <summary>
        /// Sets up the control to the number of decimal places specified in
        /// the attributes
        /// </summary>
        protected override void InitialiseWithAttributes()
        {
            if (_attributes.ContainsKey("decimalPlaces"))
            {
                int decimalPlaces = 0;
                if (!Int32.TryParse((string)_attributes["decimalPlaces"], out decimalPlaces))
                {
                    throw new InvalidXmlDefinitionException("In a 'parameter' " +
                        "element in the class definitions, the 'decimalPlaces' " +
                        "attribute had an invalid integer value.");
                }
                _numericUpDown.DecimalPlaces = decimalPlaces;
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
            if (_businessObject != null && !_isReadOnceOnly)
            {
                decimal newValue = Convert.ToDecimal(_numericUpDown.Value);
                decimal oldValue = Convert.ToDecimal(_businessObject.GetPropertyValue(_propertyName));
                if (newValue != oldValue)
                {
                    //log.Debug("setting property value to " + _numericUpDown.Value + " of type " + _numericUpDown.Value.GetType().Name);
                    _businessObject.SetPropertyValue(_propertyName, newValue);
                }
            }
        }
    }
}