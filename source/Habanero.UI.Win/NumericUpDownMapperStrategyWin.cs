using System;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    internal class NumericUpDownMapperStrategyWin : INumericUpDownMapperStrategy
    {
        private NumericUpDownMapper _mapper;

        public void ValueChanged(NumericUpDownMapper mapper)
        {
            _mapper = mapper;
            NumericUpDownWin control = (NumericUpDownWin) mapper.Control;
            control.ValueChanged += ValueChangedHandler;
            control.Leave += ValueChangedHandler;
        }

        private void ValueChangedHandler(object sender, EventArgs e)
        {
            _mapper.ApplyChangesToBusinessObject();
            _mapper.UpdateControlValueFromBusinessObject();
        }
    }
}