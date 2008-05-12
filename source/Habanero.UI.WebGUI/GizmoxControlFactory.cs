using System.Collections;
using System.Collections.Generic;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class GizmoxControlFactory : IControlFactory
    {
        public IFilterControl CreateFilterControl()
        {
            return new FilterControlGiz(this);
        }

        public ITextBox CreateTextBox()
        {
            return new TextBoxGiz();
        }
        public IComboBox CreateComboBox()
        {
            return new ComboBoxGiz();
        }
        public IListBox CreateListBox()
        {
            return new ListBoxGiz();
        }

        public IMultiSelector<T> CreateMultiSelector<T>()
        {
            return new MultiSelectorGiz<T>();
        }

        public IButton CreateButton()
        {
            return new ButtonGiz();
        }

        public ICheckBox CreateCheckBox()
        {
            return new CheckBoxGiz();
        }

        public ILabel CreateLabel()
        {
            return CreateLabel("");
        }

        public ILabel CreateLabel(string labelText)
        {
            LabelGiz label = new LabelGiz(labelText);
            label.Width = label.Text.Length * 8;
            label.Height = 15;
            return label;
        }

        public IDateTimePicker CreateDateTimePicker()
        {
            return new DateTimePickerGiz();
        }

        public IChilliControl CreateControl()
        {
            return new ControlGiz();
        }
    }
}
