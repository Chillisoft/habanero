using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a lookup ComboBox
    /// depending on the environment
    /// </summary>
    internal class LookupComboBoxKeyPressMapperStrategyWin : ILookupComboBoxMapperStrategy
    {
        private LookupComboBoxMapper _mapper;

        /// <summary>
        /// Removes event handlers previously assigned to the ComboBox
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        public void RemoveCurrentHandlers(LookupComboBoxMapper mapper)
        {
            _mapper = mapper;
            ComboBoxWin comboBoxWin = this.ComboBox(mapper);
            if (comboBoxWin != null)
            {
                comboBoxWin.SelectedIndexChanged -= _mapper.SelectedIndexChangedHandler;
            }
        }

        /// <summary>
        /// Adds event handlers to the ComboBox that are suitable for the UI environment
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        public void AddHandlers(LookupComboBoxMapper mapper)
        {
            ComboBoxWin comboBoxWin = this.ComboBox(mapper);
            if (comboBoxWin != null)
            {
                comboBoxWin.KeyPress += delegate(object sender, System.Windows.Forms.KeyPressEventArgs e)
                {
                    if (e.KeyChar == 13)
                    {
                        mapper.ApplyChangesToBusinessObject();
                        mapper.UpdateControlValueFromBusinessObject();
                    }
                };
            }
        }

        private ComboBoxWin ComboBox(LookupComboBoxMapper mapper)
        {
            ComboBoxWin comboBoxWin = null;
            IControlHabanero control = mapper.Control;
            if (control is IComboBox)
            {
                comboBoxWin = (ComboBoxWin) control;
            }
            return comboBoxWin;
        }
    }
}