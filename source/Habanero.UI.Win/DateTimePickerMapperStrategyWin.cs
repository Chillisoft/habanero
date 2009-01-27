using System;
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.Util;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a TextBox
    /// depending on the environment
    /// </summary>
    internal class DateTimePickerMapperStrategyWin : IDateTimePickerMapperStrategy
    {
        // Assumes that one strategy is created for each control.
        // These fields exist so that the IsValidCharacter method knows
        //   which prop and textbox it is dealing with

        public void AddUpdateBoPropOnValueChangedHandler(DateTimePickerMapper mapper)
        {
            if (mapper.Control is IDateTimePicker)
            {
                IDateTimePicker dtp = (IDateTimePicker)mapper.Control;
                dtp.ValueChanged +=delegate {
                                           mapper.ApplyChangesToBusinessObject();
                                       };
            }
        }
    }
}