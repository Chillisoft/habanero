using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    internal class LookupComboBoxKeyPressMapperStrategyWin : ILookupComboBoxMapperStrategy
    {
        private LookupComboBoxMapper _mapper;

        public void RemoveCurrentHandlers(LookupComboBoxMapper mapper)
        {
            _mapper = mapper;
            ComboBoxWin comboBoxWin = this.ComboBox(mapper);
            if (comboBoxWin != null)
            {
                comboBoxWin.SelectedIndexChanged -= _mapper.SelectedIndexChangedHandler;
            }
        }

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
            IControlChilli control = mapper.Control;
            if (control is IComboBox)
            {
                comboBoxWin = (ComboBoxWin) control;
            }
            return comboBoxWin;
        }
    }
}