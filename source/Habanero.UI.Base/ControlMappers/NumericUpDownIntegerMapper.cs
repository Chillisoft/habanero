using System;

namespace Habanero.UI.Base
{
    public class NumericUpDownIntegerMapper : NumericUpDownMapper
    {
        public NumericUpDownIntegerMapper(IControlChilli ctl, string propName, bool isReadOnly)
            : base(ctl, propName, isReadOnly)
        {
            _numericUpDown.DecimalPlaces = 0;
            _numericUpDown.Maximum = int.MaxValue;
            _numericUpDown.Minimum = int.MinValue;
        }
        public override void ApplyChangesToBusinessObject()
        {
            SetPropertyValue(Convert.ToInt32(_numericUpDown.Value));
        }

    }
}