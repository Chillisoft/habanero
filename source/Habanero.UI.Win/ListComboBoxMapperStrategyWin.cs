using System;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    internal class ListComboBoxMapperStrategyWin : IListComboBoxMapperStrategy
    {
        public void AddItemSelectedEventHandler(ListComboBoxMapper mapper)
        {
            IControlChilli control = mapper.Control;
            if (!(control is IComboBox)) return;
            ComboBoxWin comboBoxWin = (ComboBoxWin) control;
            comboBoxWin.SelectedIndexChanged += delegate(object sender, EventArgs e)
                                                    {
                                                        mapper.ApplyChangesToBusinessObject();
                                                        mapper.UpdateControlValueFromBusinessObject();
                                                    };
        }
    }
}