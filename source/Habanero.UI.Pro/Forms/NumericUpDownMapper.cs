using System;
using System.Windows.Forms;
using Habanero.UI.Forms;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// A super-class for mappers of the numeric up-down facility
    /// </summary>
    public abstract class NumericUpDownMapper : ControlMapper
    {
        protected NumericUpDown _numericUpDown;

        /// <summary>
        /// Constructor to initialise the mapper
        /// </summary>
        /// <param name="control">The control to map</param>
        /// <param name="propName">The property name</param>
		/// <param name="isReadOnly">Whether this control is read only</param>
		public NumericUpDownMapper(NumericUpDown control, string propName, bool isReadOnly)
            : base(control, propName, isReadOnly)
        {
            _numericUpDown = control;
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
                _numericUpDown.Value = new Decimal(0);
            }
            else if (!propValue.Equals(_numericUpDown.Value))
            {
                _numericUpDown.Value = Convert.ToDecimal(propValue);
            }
        }
    }
}