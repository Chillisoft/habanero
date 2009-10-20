// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
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
        /// <summary>
        /// Constructor for <see cref="MultiSelectorManager{T}"/>
        /// </summary>
        /// <param name="multiSelector"></param>
        public MultiSelectorManager(IMultiSelector<T> multiSelector)
        {
            _multiSelector = multiSelector;
            _model = new MultiSelectorModel<T>();
            _model.OptionAdded +=((sender, e) => AvailableOptionsListBox.Items.Add(e.Item));

            _model.OptionRemoved +=((sender, e) => AvailableOptionsListBox.Items.Remove(e.Item));

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
            deselectButton.Click += DoDeselect;

            IButton selectAllButton = GetButton(MultiSelectorButton.SelectAll);
            selectAllButton.Click += delegate { _model.SelectAll(); };

            IButton deselectAllButton = GetButton(MultiSelectorButton.DeselectAll);
            deselectAllButton.Click += delegate { _model.DeselectAll(); };
        }
        /// <summary>
        /// Event handler for Selecting an item 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DoSelect(object sender, EventArgs e)
        {
            List<T> items = new List<T>();
            foreach (T item in AvailableOptionsListBox.SelectedItems) items.Add(item);
            _model.Select(items);
        }
        /// <summary>
        /// Event handler for deselecting an item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DoDeselect(object sender, EventArgs e)
        {
            List<T> items = new List<T>();
            foreach (T item in SelectionsListBox.SelectedItems) items.Add(item);
            _model.Deselect(items);
        }

        private void UpdateListBoxes()
        {
            AvailableOptionsListBox.Items.Clear();
            foreach (T item in _model.AvailableOptions)
            {
                AvailableOptionsListBox.Items.Add(item);
            }
//            _model.AvailableOptions.ForEach(obj => AvailableOptionsListBox.Items.Add(obj));
            SelectionsListBox.Items.Clear();
            foreach (T obj in _model.SelectionsView)
            {
                SelectionsListBox.Items.Add(obj);
            }
        }

        /// <summary>
        /// See <see cref="IMultiSelector{T}.AllOptions"/>
        /// </summary>
        public IList<T> AllOptions
        {
            get { return _model.AllOptions; }
            set { _model.AllOptions = value; }
        }

        /// <summary>
        /// See <see cref="IMultiSelector{T}.AvailableOptionsListBox"/>
        /// </summary>
        private IListBox AvailableOptionsListBox
        {
            get { return _multiSelector.AvailableOptionsListBox; }
        }

        /// <summary>
        /// See <see cref="IMultiSelector{T}.Model"/>
        /// </summary>
        public MultiSelectorModel<T> Model
        {
            get { return _model; }
        }

        /// <summary>
        /// See <see cref="IMultiSelector{T}.SelectedOptions"/>
        /// </summary>
        public IList<T> SelectedOptions
        {
            get { return _model.SelectedOptions; }
            set { _model.SelectedOptions = value; }
        }

        /// <summary>
        /// See <see cref="IMultiSelector{T}.SelectedOptionsListBox"/>
        /// </summary>
        private IListBox SelectionsListBox
        {
            get { return _multiSelector.SelectedOptionsListBox; }
        }

        /// <summary>
        /// See <see cref="IMultiSelector{T}.SelectionsView"/>
        /// </summary>
        public ReadOnlyCollection<T> SelectionsView
        {
            get { return _model.SelectionsView; }
        }

        /// <summary>
        /// See <see cref="IMultiSelector{T}.GetButton"/>
        /// </summary>
        private IButton GetButton(MultiSelectorButton buttonType)
        {
            return _multiSelector.GetButton(buttonType);
        }
    }
}