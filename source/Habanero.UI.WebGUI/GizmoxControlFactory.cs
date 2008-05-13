using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;

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

        public IReadOnlyGrid CreateReadOnlyGrid()
        {
            return new ReadOnlyGridGiz();
        }

        public IReadOnlyGridWithButtons CreateReadOnlyGridWithButtons()
        {
            return new ReadOnlyGridWithButtonsGiz();
        }

        public IButtonGroupControl CreateButtonGroupControl()
        {
            return new ButtonGroupControlGiz(this);
        }

        public IReadOnlyGridButtonsControl CreateReadOnlyGridButtonsControl()
        {
            return new ReadOnlyGridButtonsControlGiz(this);
        }

        public IChilliControl CreateControl()
        {
            return new ControlGiz();
        }
    }

    public class ReadOnlyGridButtonsControlGiz :ButtonGroupControlGiz, IReadOnlyGridButtonsControl
    {
        private readonly IButton _deleteButton;

        public ReadOnlyGridButtonsControlGiz(IControlFactory controlFactory) : base(controlFactory)
        {
            AddButton("Add");
            AddButton("Edit");
            _deleteButton = AddButton("Delete");
            _deleteButton.Visible = false;

        }
    }
}
