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

using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;
using Habanero.UI.Base.LayoutManagers;

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
            return new MultiSelectorWin<T>();
        }

        public IButton CreateButton()
        {
            return new ButtonWin();
        }

        public ICheckBox CreateCheckBox()
        {
            return new CheckBoxWin();
        }

        public ILabel CreateLabel()
        {
            return new LabelWin();
        }

        public ILabel CreateLabel(string labelText)
        {
            ILabel label = CreateLabel();
            label.Text = labelText;
            return label;
        }

        public IDateTimePicker CreateDateTimePicker()
        {
            return new  DateTimePickerWin();
        }

        public BorderLayoutManager CreateBorderLayoutManager(IChilliControl control)
        {
            return new BorderLayoutManagerWin(control);
        }

        public IPanel CreatePanel()
        {
            return new PanelWin();
        }
        public IReadOnlyGrid CreateReadOnlyGrid()
        {
            return new ReadOnlyGridWin();
        }

        public IReadOnlyGridWithButtons CreateReadOnlyGridWithButtons()
        {
            return  new ReadOnlyGridWithButtonsWin();
        }


        public IButtonGroupControl CreateButtonGroupControl()
        {
            return new ButtonGroupControlWin(this);
        }

        public IReadOnlyGridButtonsControl CreateReadOnlyGridButtonsControl()
        {
            throw new System.NotImplementedException();
        }

        public IChilliControl CreateControl()
        {
            return new ControlWin();
        }
    }
}
