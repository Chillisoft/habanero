using System;

namespace Habanero.UI.Base
{
    public class NumericUpDownIntegerMapper : NumericUpDownMapper
    {
        private INumericUpDownMapperStrategy _mapperStrategy;

        /// <summary>
        /// Constructor to initialise a new instance of the class
        /// </summary>
        /// <param name="numericUpDownControl">The numericUpDownControl object to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnly">Whether this control is read only</param>
        /// <param name="factory">the control factory to be used when creating the controlMapperStrategy</param>
        public NumericUpDownIntegerMapper(IControlChilli numericUpDownControl, string propName, bool isReadOnly, IControlFactory factory)
            : base(numericUpDownControl, propName, isReadOnly, factory)
        {
            _numericUpDown.DecimalPlaces = 0;
            _numericUpDown.Maximum = int.MaxValue;
            _numericUpDown.Minimum = int.MinValue;
            _mapperStrategy = factory.CreateNumericUpDownMapperStrategy();
            _mapperStrategy.ValueChanged(this);
        }

        public INumericUpDownMapperStrategy MapperStrategy
        {
            get { return _mapperStrategy; }
        }

        public override void ApplyChangesToBusinessObject()
        {
            SetPropertyValue(Convert.ToInt32(_numericUpDown.Value));
        }

    }
}