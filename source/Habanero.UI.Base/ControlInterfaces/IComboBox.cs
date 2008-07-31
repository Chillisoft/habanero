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
using System.Data;

namespace Habanero.UI.Base
{
    //TODO: Comments
    public interface IComboBox : IControlChilli
    {
        event EventHandler SelectedIndexChanged;

        IComboBoxObjectCollection Items { get; }

        int SelectedIndex { get; set; }

        object SelectedItem { get; set; }

        int DropDownWidth { get; set; }

        string ValueMember { get; set; }

        string DisplayMember { get; set; }

        object DataSource { get; set; }

        object SelectedValue { get; set; }
    }

    public class ComboPair
    {
        private readonly string _key;
        private readonly object _value;

        public ComboPair(string key, object value)
        {
            _key = key;
            _value = value;
        }

        public string Key
        {
            get
            {
                return _key;
            }
        }
        public object Value
        {
            get
            {
                return _value;
            }
        }

        public override string ToString()
        {
            return _key;
        }

        public override bool Equals(object obj)
        {

            if (obj == null) return false;
            if (this.GetType() != obj.GetType()) return false;

            ComboPair other = obj as ComboPair;

            return String.Compare(other.Key, Key) == 0 && (other.Value == Value);
        }

        public override int GetHashCode()
        {

            return Key.GetHashCode() | Value.GetHashCode();
        }

        public static bool operator ==(ComboPair v1, ComboPair v2)
        {

            if ((object)v1 == null)
                if ((object)v2 == null)
                    return true;
                else
                    return false;

            return (v1.Equals(v2));
        }

        public static Boolean operator !=(ComboPair v1, ComboPair v2)
        {

            return !(v1 == v2);
        }

    }
}