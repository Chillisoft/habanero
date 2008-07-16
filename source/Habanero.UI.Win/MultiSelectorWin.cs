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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public partial class MultiSelectorWin<T> : UserControlWin, IMultiSelector<T>
    {
        private readonly MultiSelectorManager<T> _manager;

        public MultiSelectorWin()
        {
            InitializeComponent();
            _manager = new MultiSelectorManager<T>(this);
            AvailableOptionsListBox.SelectedIndexChanged += delegate
            {
                GetButton(MultiSelectorButton.Select).Enabled = (AvailableOptionsListBox.SelectedIndex != -1);
            };

            SelectionsListBox.SelectedIndexChanged += delegate
            {
                GetButton(MultiSelectorButton.Deselect).Enabled = (SelectionsListBox.SelectedIndex != -1);
            };
        }

        public List<T> Options
        {
            get { return _manager.Options; }
            set
            {
                _manager.Options = value;
                GetButton(MultiSelectorButton.Select).Enabled = false;
            }
        }
        public IListBox AvailableOptionsListBox
        {
            get { return _availableOptionsListbox; }
        }

        public MultiSelectorModel<T> Model
        {
            get { return _manager.Model; }
        }

        public List<T> Selections
        {
            get { return _manager.Selections; }
            set
            {
                _manager.Selections = value;
                GetButton(MultiSelectorButton.Deselect).Enabled = false;
            }
        }
        public IListBox SelectionsListBox
        {
            get { return _selectionsListbox; }
        }

        public IButton GetButton(MultiSelectorButton buttonType)
        {
            switch (buttonType)
            {
                case MultiSelectorButton.Select:
                    return _btnSelect;
              
                case MultiSelectorButton.Deselect:
                    return _btnDeselect;
                case MultiSelectorButton.SelectAll:
                    return _btnSelectAll;
                case MultiSelectorButton.DeselectAll:
                    return _btnDeselectAll;
                default:
                    throw new ArgumentOutOfRangeException("buttonType");
            }
        }

        public ReadOnlyCollection<T> SelectionsView
        {
            //TODO Port: Fix and test this for windows.
            get { return this._manager.SelectionsView; }
        }
    }
}
