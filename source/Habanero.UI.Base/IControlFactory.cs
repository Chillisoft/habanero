using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI.Base
{
    public interface IControlFactory
    {
        IFilterControl CreateFilterControl();
        ITextBox CreateTextBox();
        IComboBox CreateComboBox();
        IListBox CreateListBox();
        IMultiSelector<T> CreateMultiSelector<T>();
        IButton CreateButton();
        ICheckBox CreateCheckBox();
    }
}
