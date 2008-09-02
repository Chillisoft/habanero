using System;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    internal class LookupComboBoxDefaultMapperStrategyWin : ILookupComboBoxMapperStrategy
    {
        private LookupComboBoxMapper _mapper;

        public void AddItemSelectedEventHandler(LookupComboBoxMapper mapper)
        {
            _mapper = mapper;
            IControlChilli control = mapper.Control;
            if (control is IComboBox)
            {
                ComboBoxWin comboBoxWin = (ComboBoxWin) control;
                comboBoxWin.SelectedIndexChanged += SelectIndexChangedHandler;
                _mapper.SelectedIndexChangedHandler = SelectIndexChangedHandler;
            }
        }

        private void SelectIndexChangedHandler(object sender, EventArgs e)
        {
            _mapper.ApplyChangesToBusinessObject();
           // _mapper.UpdateControlValueFromBusinessObject();
        }


        public void RemoveCurrentHandlers(LookupComboBoxMapper mapper)
        {
        }


        public void AddHandlers(LookupComboBoxMapper mapper)
        {
            AddItemSelectedEventHandler(mapper);
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