using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class WinControlFactory : IControlFactory
    {
        public IFilterControl CreateFilterControl()
        {
            return new FilterControlWin(this);
        }

        public ITextBox CreateTextBox()
        {
            return new TextBoxWin();
        }

        public IComboBox CreateComboBox()
        {
            return new ComboBoxWin();
        }

        public IListBox CreateListBox()
        {
            return new ListBoxWin();
        }

        public IMultiSelector<T> CreateMultiSelector<T>()
        {
            return new MultiSelectorWin<T>(this);
        }

        public IButton CreateButton()
        {
            return new ButtonWin();
        }
    }
}
