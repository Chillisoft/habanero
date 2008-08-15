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

namespace Habanero.UI.Base
{
    /// <summary>
    /// This manager groups common logic for IMultiSelector objects.
    /// Do not use this object in working code - rather call CreateMultiSelector
    /// in the appropriate control factory.
    /// </summary>
    public class MultiSelectorManager<T>
    {
        private readonly IMultiSelector<T> _multiSelector;
        private readonly MultiSelectorModel<T> _model;

        //TODO: add double click behaviour

        public MultiSelectorManager(IMultiSelector<T> multiSelector)
        {
            _multiSelector = multiSelector;
            _model = new MultiSelectorModel<T>();
            _model.OptionAdded +=
                delegate(object sender, MultiSelectorModel<T>.ModelEventArgs<T> e) { AvailableOptionsListBox.Items.Add(e.Item); };

            _model.OptionRemoved +=
                delegate(object sender, MultiSelectorModel<T>.ModelEventArgs<T> e) { AvailableOptionsListBox.Items.Remove(e.Item); };

            _model.OptionsChanged += delegate { UpdateListBoxes(); };

            _model.SelectionsChanged += delegate { UpdateListBoxes(); };

            _model.Selected += delegate(object sender, MultiSelectorModel<T>.ModelEventArgs<T> e)
                                   {
                                       AvailableOptionsListBox.Items.Remove(e.Item);
                                       SelectionsListBox.Items.Add(e.Item);
                                   };
            _model.Deselected += delegate(object sender, MultiSelectorModel<T>.ModelEventArgs<T> e)
                                     {
                                         AvailableOptionsListBox.Items.Add(e.Item);
                                         SelectionsListBox.Items.Remove(e.Item);
                                     };

            IButton selectButton = GetButton(MultiSelectorButton.Select);
            selectButton.Click += DoSelect;

            IButton deselectButton = GetButton(MultiSelectorButton.Deselect);
            deselectButton.Click += delegate
                                        {
                                            List<T> items = new List<T>();
                                            foreach (T item in SelectionsListBox.SelectedItems) items.Add(item);
                                            _model.Deselect(items);
                                        };

            IButton selectAllButton = GetButton(MultiSelectorButton.SelectAll);
            selectAllButton.Click += delegate { _model.SelectAll(); };

            IButton deselectAllButton = GetButton(MultiSelectorButton.DeselectAll);
            deselectAllButton.Click += delegate { _model.DeselectAll(); };

            //SelectionsListBox.DoubleClick += DoSelect;
        }

        private void DoSelect(object sender, EventArgs e)
        {
            List<T> items = new List<T>();
            foreach (T item in AvailableOptionsListBox.SelectedItems) items.Add(item);
            _model.Select(items);
        }


        private void UpdateListBoxes()
        {
            AvailableOptionsListBox.Items.Clear();
            _model.AvailableOptions.ForEach(delegate(T obj) { AvailableOptionsListBox.Items.Add(obj); });
            SelectionsListBox.Items.Clear();
            foreach (T obj in _model.SelectionsView)
            {
                SelectionsListBox.Items.Add(obj);
            }
        }

        public List<T> Options
        {
            get { return _model.Options; }
            set { _model.Options = value; }
        }

        private IListBox AvailableOptionsListBox
        {
            get { return _multiSelector.AvailableOptionsListBox; }
        }

        public MultiSelectorModel<T> Model
        {
            get { return _model; }
        }

        public List<T> Selections
        {
            get { return _model.Selections; }
            set { _model.Selections = value; }
        }

        private IListBox SelectionsListBox
        {
            get { return _multiSelector.SelectionsListBox; }
        }

        public ReadOnlyCollection<T> SelectionsView
        {
            get { return _model.SelectionsView; }
        }

        private IButton GetButton(MultiSelectorButton buttonType)
        {
            return _multiSelector.GetButton(buttonType);
        }
    }
}