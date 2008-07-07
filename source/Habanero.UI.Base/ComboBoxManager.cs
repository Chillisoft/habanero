namespace Habanero.UI.Base
{
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
