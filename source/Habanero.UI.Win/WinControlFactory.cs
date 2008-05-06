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
            return new TextBoxWin(this);
        }

        public IListBox CreateListBox()
        {
            return new ListBoxWin(this);
        }

        public IMultiSelector<T> CreateMultiSelector<T>()
        {
            return new MultiSelectorWin<T>(this);
        }
    }
}
