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
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Provides a multiselector control. The type to be displayed in the 
    /// lists is set by the template type.  The multiselector helps the user to
    /// select from an available list of options.  Unselected options appear on the
    /// left and selected ones appear on the right.  The AvailableOptions consists
    /// of all options, both selected and unselected - no object may appear in the
    /// selected list if it is not also in the AvailableOptions list.  All list
    /// control is managed through the Model object.
    /// </summary>
    public partial class MultiSelectorVWG<T> : UserControlVWG, IMultiSelector<T>
    {
        private readonly IControlFactory _controlFactory;
        private readonly MultiSelectorManager<T> _manager;
        private GridLayoutManager _gridLayoutManager;

        public MultiSelectorVWG(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            InitializeComponent();
            _gridLayoutManager = new GridLayoutManager(this, controlFactory);
            PanelVWG optionsPanel = new PanelVWG();
            groupBox1.Dock = Gizmox.WebGUI.Forms.DockStyle.Fill;
            optionsPanel.Controls.Add(groupBox1);
            PanelVWG buttonPanel = new PanelVWG();
            GridLayoutManager buttonPanelManager = new GridLayoutManager(buttonPanel, controlFactory);
            buttonPanelManager.SetGridSize(6, 1);
            buttonPanelManager.AddControl(null);
            buttonPanelManager.AddControl(_btnSelect);
            buttonPanelManager.AddControl(_btnSelectAll);
            buttonPanelManager.AddControl(_btnDeselectAll);
            buttonPanelManager.AddControl(_btnDeselect);
            buttonPanelManager.AddControl(null);
            buttonPanelManager.FixRow(0, 25);
            buttonPanelManager.FixRow(1, 25);
            buttonPanelManager.FixRow(2, 25);
            buttonPanelManager.FixRow(3, 25);
            buttonPanelManager.FixRow(4, 25);
            buttonPanelManager.FixRow(5, 25);
            buttonPanelManager.FixColumnBasedOnContents(0);
            PanelVWG selectionsPanel = new PanelVWG();
            groupBox2.Dock = Gizmox.WebGUI.Forms.DockStyle.Fill;
            selectionsPanel.Controls.Add(groupBox2);
            _gridLayoutManager.SetGridSize(1, 3);
            _gridLayoutManager.FixColumn(1, 100);
            _gridLayoutManager.AddControl(optionsPanel);
            _gridLayoutManager.AddControl(buttonPanel);
            _gridLayoutManager.AddControl(selectionsPanel);
            _manager = new MultiSelectorManager<T>(this);
        }

        /// <summary>
        /// Gets and sets the complete list of options available to go in
        /// either panel
        /// </summary>
        public List<T> AllOptions
        {
            get { return _manager.AllOptions; }
            set { _manager.AllOptions = value; }
        }

        /// <summary>
        /// Gets the ListBox control that contains the available options that
        /// have not been selected
        /// </summary>
        public IListBox AvailableOptionsListBox
        {
            get { return _availableOptionsListbox; }
        }

        /// <summary>
        /// Gets the model that manages the options available or selected
        /// </summary>
        public MultiSelectorModel<T> Model
        {
            get { return _manager.Model; }
        }

        ///<summary>
        /// Gets or sets the list of items already selected (which is a subset of
        /// all available options).  This list typically appears on the right-hand side.
        ///</summary>
        public List<T> SelectedOptions
        {
            get { return _manager.SelectedOptions; }
            set { _manager.SelectedOptions = value; }
        }

        /// <summary>
        /// Gets the ListBox control that contains the options that have been
        /// selected from those available
        /// </summary>
        public IListBox SelectedOptionsListBox
        {
            get { return _selectionsListbox; }
        }

        /// <summary>
        /// Gets the button control as indicated by the <see cref="MultiSelectorButton"/> enumeration.
        /// </summary>
        /// <param name="buttonType">The type of button</param>
        /// <returns>Returns a button</returns>
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

        /// <summary>
        /// Gets a view of the SelectedOptions collection
        /// </summary>
        public ReadOnlyCollection<T> SelectionsView
        {
            get { return this._manager.SelectionsView; }
        }

        private void _availableOptionsListbox_DoubleClick(object sender, EventArgs e)
        {

        }


    }
}