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

using System;
using System.Collections;
using System.Collections.Generic;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;
using Habanero.UI.Base.LayoutManagers;

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

        public BorderLayoutManager CreateBorderLayoutManager(IChilliControl control)
        {
            return new BorderLayoutManagerGiz(control);
        }

        public IPanel CreatePanel()
        {
            return new PanelGiz();
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
