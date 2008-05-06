using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Gizmox
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
            return new MultiSelectorGiz<T>(this);
        }

        public IButton CreateButton()
        {
            return new ButtonGiz();
        }
    }
}
