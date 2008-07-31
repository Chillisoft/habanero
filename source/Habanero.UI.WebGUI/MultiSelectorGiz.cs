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

#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Habanero.UI.Base;

#endregion

namespace Habanero.UI.WebGUI
{
    /// <summary>
    /// Provides a multiselector control. The type to be displayed in the 
    /// lists is set by the template type.
    /// </summary>
    public partial class MultiSelectorGiz<T> : UserControlGiz, IMultiSelector<T>
    {

        private readonly MultiSelectorManager<T> _manager;

        public MultiSelectorGiz()
        {
            InitializeComponent();
            _manager = new MultiSelectorManager<T>(this);
        }

        public List<T> Options
        {
            get { return _manager.Options; }
            set { _manager.Options = value; }
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
            set { _manager.Selections = value; }
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
            get { return this._manager.SelectionsView; }
        }

        private void _availableOptionsListbox_DoubleClick(object sender, EventArgs e)
        {

        }


    }
}