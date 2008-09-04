using System;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a CheckBox
    /// depending on the environment
    /// </summary>
    internal class CheckBoxStrategyWin : ICheckBoxMapperStrategy
    {
        /// <summary>
        /// Adds click event handler
        /// </summary>
        /// <param name="mapper">The checkbox mapper</param>
        public void AddClickEventHandler(CheckBoxMapper mapper)
        {
            if (mapper.Control is ICheckBox)
            {
                CheckBoxWin checkBox = (CheckBoxWin) mapper.Control;
                checkBox.Click += delegate(object sender, EventArgs e)
                {
                    mapper.ApplyChangesToBusinessObject();
                    mapper.UpdateControlValueFromBusinessObject();
                };
            }
        }
    }
}