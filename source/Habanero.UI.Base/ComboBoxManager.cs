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
        private readonly IComboBox _comboBox;
        /// <summary>
        /// Creates <see cref="ComboBoxManager"/>
        /// </summary>
        /// <param name="comboBox"></param>
        public ComboBoxManager(IComboBox comboBox)
        {
            _comboBox = comboBox;


        }

        ///<summary>
        /// Returns the Key for the Selected Item if it is a <see cref="ComboPair"/> else
        /// returns the selectedItem.
        ///</summary>
        ///<param name="selectedItem"></param>
        ///<returns></returns>
        public object GetSelectedItem(object selectedItem)
        {
            if (selectedItem is ComboPair)
            {
                return ((ComboPair)selectedItem).Key;
            }
            return selectedItem;
        }

        ///<summary>
        /// If the <see cref="IComboBox"/> contains <see cref="ComboPair"/>s ;<br/>
        ///   If the <paramref name="value"/> equals a <see cref="ComboPair.Key"/> for a <see cref="ComboPair"/> then return the <see cref="ComboPair"/>;<br/>
        ///   else returns null;<br/>
        /// else return the value;<br/>
        ///</summary>
        ///<param name="value"></param>
        ///<returns></returns>
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

        ///<summary>
        /// 
        /// If the item is a <see cref="ComboPair"/> then returns the <see cref="ComboPair.Value"/> for the <see cref="ComboPair"/>
        ///   else returns the item.
        ///</summary>
        ///<param name="item"></param>
        ///<returns></returns>
        public object GetSelectedValue(object item)
        {
            if (item is ComboPair)
            {
                return ((ComboPair)item).Value;
            }
            return item;
        }

        ///<summary>
        /// If the <see cref="IComboBox"/> contains <see cref="ComboPair"/>s <br/>
        ///   If the value is equal to the <see cref="ComboPair.Value"/> for a <see cref="ComboPair"/> then<br/>
        ///    returns the <see cref="ComboPair"/> else returns the null.<br/>
        /// Else<br/>
        ///  returns the value<br/>
        ///</summary>
        ///<param name="value"></param>
        ///<returns></returns>
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
