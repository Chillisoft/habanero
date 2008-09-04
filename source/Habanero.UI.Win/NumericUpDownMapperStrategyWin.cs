using System;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a NumericUpDown
    /// depending on the environment
    /// </summary>
    internal class NumericUpDownMapperStrategyWin : INumericUpDownMapperStrategy
    {
        private NumericUpDownMapper _mapper;

        /// <summary>
        /// Handles the value changed event suitably for the UI environment
        /// </summary>
        /// <param name="mapper">The mapper for the NumericUpDown</param>
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