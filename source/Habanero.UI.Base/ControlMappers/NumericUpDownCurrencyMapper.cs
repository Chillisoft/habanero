namespace Habanero.UI.Base
{
    public class NumericUpDownCurrencyMapper : NumericUpDownMapper
    {
        /// <summary>
        /// Constructor to initialise a new instance of the class
        /// </summary>
        /// <param name="numericUpDownControl">The numericUpDownControl object to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnly">Whether this control is read only</param>
        /// <param name="factory">the control factory to be used when creating the controlMapperStrategy</param>
        public NumericUpDownCurrencyMapper(IControlChilli numericUpDownControl, string propName, bool isReadOnly, IControlFactory factory)
            : base(numericUpDownControl, propName, isReadOnly, factory)
        {
        }

        public override void ApplyChangesToBusinessObject()
        {
            SetPropertyValue(_numericUpDown.Value);
        }
    }
}