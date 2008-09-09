using System;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a list ComboBox
    /// depending on the environment
    /// </summary>
    internal class ListComboBoxMapperStrategyWin : IListComboBoxMapperStrategy
    {
        /// <summary>
        /// Adds an ItemSelected event handler.
        /// For Windows Forms you may want the business object to be updated immediately, however
        /// for a web environment with low bandwidth you may choose to only update when the user saves.
        ///</summary>
        public void AddItemSelectedEventHandler(ListComboBoxMapper mapper)
        {
            IControlHabanero control = mapper.Control;
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