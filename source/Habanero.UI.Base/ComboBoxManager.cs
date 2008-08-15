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

namespace Habanero.UI.Base
{
    /// <summary>
    /// This manager groups common logic for IComboBox objects.
    /// Do not use this object in working code - rather call CreateComboBox
    /// in the appropriate control factory.
    /// </summary>
    public class ComboBoxManager
    {
        private IComboBox _comboBox;

        public ComboBoxManager(IComboBox comboBox)
        {
            _comboBox = comboBox;
        }

        public object GetSelectedItem(object selectedItem)
        {
            if (selectedItem is ComboPair)
            {
                return ((ComboPair)selectedItem).Key;
            }
            else
            {
                return selectedItem;
            }
        }

        public object GetItemToSelect(object value)
        {
            if (value is string && _comboBox.Items.Count > 0 && _comboBox.Items[0] is ComboPair)
            {
                foreach (ComboPair comboPair in _comboBox.Items)
                {
                    if (comboPair.Key == (string)value)
                    {
                        return comboPair;
                    }
                }
            }
            else
            {
               return value;
            }
            return null;
        }

        public object GetSelectedValue(object item)
        {
            if (item is ComboPair)
            {
                return ((ComboPair)item).Value;
            }
            else
            {
                return item;
            }
        }

        public object GetValueToSelect(object value)
        {
            if (_comboBox.Items.Count > 0 && _comboBox.Items[0] is ComboPair)
            {
                if (value is string)
                {
                    foreach (ComboPair comboPair in _comboBox.Items)
                    {
                        if (!(comboPair.Value is string)) continue;
                        if ((string)comboPair.Value == (string)value)
                        {
                            return comboPair;
                        }
                    } 
                }
                foreach (ComboPair comboPair in _comboBox.Items)
                {
                    if (comboPair.Value == value)
                    {
                       return comboPair;
                    }
                }
            } else
            {
                return value;
            }
            return null;
        }
    }
}
