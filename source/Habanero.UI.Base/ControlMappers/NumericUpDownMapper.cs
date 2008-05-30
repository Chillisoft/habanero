using System;

namespace Habanero.UI.Base
{
    public abstract class NumericUpDownMapper : ControlMapper
    {
        protected INumericUpDown _numericUpDown;

        /// <summary>
        /// Constructor to instantiate a new instance of the class
        /// </summary>
        /// <param name="ctl">The control object to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnly">Whether the control is read only.
        /// If so, it then becomes disabled.  If not,
        /// handlers are assigned to manage key presses.</param>
        /// <param name="factory">the control factory to be used when creating the controlMapperStrategy</param>
        protected NumericUpDownMapper(IControlChilli ctl, string propName, bool isReadOnly, IControlFactory factory)
            : base(ctl, propName, isReadOnly, factory)
        {
            _numericUpDown = (INumericUpDown)ctl;
        }
       


        /// <summary>
        /// Updates the value in the control from its business object.
        /// </summary>
        protected override void InternalUpdateControlValueFromBo()
        {
            _numericUpDown.Value = Convert.ToDecimal(GetPropertyValue());
        }
    }
}