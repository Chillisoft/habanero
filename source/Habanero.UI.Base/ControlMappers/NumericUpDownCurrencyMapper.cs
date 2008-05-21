namespace Habanero.UI.Base
{
    public class NumericUpDownCurrencyMapper : NumericUpDownMapper
    {
        public NumericUpDownCurrencyMapper(IControlChilli ctl, string propName, bool isReadOnly)
            : base(ctl, propName, isReadOnly)
        {
        }

        public override void ApplyChangesToBusinessObject()
        {
            SetPropertyValue(_numericUpDown.Value);
        }
    }
}