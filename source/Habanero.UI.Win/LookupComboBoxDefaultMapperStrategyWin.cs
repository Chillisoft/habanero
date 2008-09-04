using System;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a lookup ComboBox
    /// depending on the environment
    /// </summary>
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

        /// <summary>
        /// Removes event handlers previously assigned to the ComboBox
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        public void RemoveCurrentHandlers(LookupComboBoxMapper mapper)
        {
        }

        /// <summary>
        /// Adds event handlers to the ComboBox that are suitable for the UI environment
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
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