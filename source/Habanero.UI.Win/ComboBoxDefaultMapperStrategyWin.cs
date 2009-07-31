//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.Base;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a lookup ComboBox
    /// depending on the environment
    /// </summary>
    internal class ComboBoxDefaultMapperStrategyWin : IComboBoxMapperStrategy
    {
        private IComboBoxMapper _mapper;

        public void AddItemSelectedEventHandler(IComboBoxMapper mapper)
        {
            _mapper = mapper;
            IControlHabanero control = mapper.Control;
            if (control is IComboBox)
            {
                ComboBoxWin comboBoxWin = (ComboBoxWin) control;
                comboBoxWin.SelectedIndexChanged += SelectIndexChangedHandler;
                _mapper.SelectedIndexChangedHandler = SelectIndexChangedHandler;
            }
        }

        private void SelectIndexChangedHandler(object sender, EventArgs e)
        {
            try
            {
                _mapper.ApplyChangesToBusinessObject();
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error");
            }
        }

        /// <summary>
        /// Removes event handlers previously assigned to the ComboBox
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        public void RemoveCurrentHandlers(IComboBoxMapper mapper)
        {
            _mapper = mapper;
            IControlHabanero control = mapper.Control;
            if (control is IComboBox)
            {
                ComboBoxWin comboBoxWin = (ComboBoxWin)control;
                comboBoxWin.SelectedIndexChanged -= SelectIndexChangedHandler;
                _mapper.SelectedIndexChangedHandler = null;
            }
        }

        /// <summary>
        /// Adds event handlers to the ComboBox that are suitable for the UI environment
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        public void AddHandlers(IComboBoxMapper mapper)
        {
            AddItemSelectedEventHandler(mapper);
        }
    }
}