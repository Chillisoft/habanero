using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base.FilterControl;
using Habanero.UI.Base.LayoutManagers;

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
        ILabel CreateLabel();
        IChilliControl CreateControl();
        ILabel CreateLabel(string labelText);
        IDateTimePicker CreateDateTimePicker();
        BorderLayoutManager CreateBorderLayoutManager(IChilliControl control);
        IPanel CreatePanel();
        IReadOnlyGrid CreateReadOnlyGrid();
        IReadOnlyGridWithButtons CreateReadOnlyGridWithButtons();
        IButtonGroupControl CreateButtonGroupControl();
        IReadOnlyGridButtonsControl CreateReadOnlyGridButtonsControl();
    }
}
