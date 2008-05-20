using System;
using System.Data;

namespace Habanero.UI.Base
{
    public interface IComboBox : IControlChilli
    {
        event EventHandler SelectedIndexChanged;
        IComboBoxObjectCollection Items { get; }

        int SelectedIndex { get; set; }

        object SelectedItem { get; set; }

        int Height { get; set; }

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