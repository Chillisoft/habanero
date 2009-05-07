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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides an editor for static data in an application.  Static data serves
    /// a number purposes including providing source data for lookup lists used in
    /// drop-downs.
    /// <br/>
    /// The editor typically consists of a TreeView on the left and an EditableGrid
    /// on the right, where data for the selected type in the TreeView can be edited.
    /// </summary>
    public class StaticDataEditorManager
    {
        private readonly IStaticDataEditor _staticDataEditor;
        private readonly IControlFactory _controlFactory;
        private readonly ITreeView _treeView;
        private readonly IEditableGridControl _gridControl;
        private readonly Dictionary<string, ClassDef> _items;
        private ITreeNode _currentSectionNode;

        public StaticDataEditorManager(IStaticDataEditor staticDataEditor, IControlFactory controlFactory)
        {
            _staticDataEditor = staticDataEditor;
            this._controlFactory = controlFactory;
            _items = new Dictionary<string, ClassDef>();
            _treeView = _controlFactory.CreateTreeView("TreeView");
            _treeView.Width = 200;
            _gridControl = _controlFactory.CreateEditableGridControl();
            BorderLayoutManager layoutManager = _controlFactory.CreateBorderLayoutManager(_staticDataEditor);
            layoutManager.AddControl(_gridControl, BorderLayoutManager.Position.Centre);
            layoutManager.AddControl(_treeView, BorderLayoutManager.Position.West);
            _treeView.AfterSelect += delegate(object sender, TreeViewEventArgs e) { SelectItem(e.Node.Text); };
            _treeView.BeforeSelect += _treeView_OnBeforeSelect;
            _gridControl.Enabled = false;
            _gridControl.FilterControl.Visible = false;

        }

        private void _treeView_OnBeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (_gridControl.Grid == null || _gridControl.Grid.GetBusinessObjectCollection() == null) return;
            if (_gridControl.Grid.GetBusinessObjectCollection().IsDirty)
            {
                DialogResult result = _controlFactory.ShowMessageBox("Do you want to save changes?",
                    "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Yes)
                {
                    e.Cancel = !SaveChanges();
                }
                else if (result == DialogResult.No)
                {
                    e.Cancel = !RejectChanges();
                }
                else e.Cancel = true;
            }
        }


        /// <summary>
        /// Adds a section to the treeview, under which individual items
        /// can be listed
        /// </summary>
        /// <param name="sectionName">The name of the section as it appears to the user</param>
        public void AddSection(string sectionName)
        {
            ITreeNode treeNode = _controlFactory.CreateTreeNode(sectionName);
            _treeView.Nodes.Add(treeNode);
            if (_currentSectionNode == null) _treeView.TopNode = treeNode;
            _currentSectionNode = treeNode;
        }

        /// <summary>
        /// Adds an item to the treeview
        /// </summary>
        /// <param name="itemName">The name of the item as it appears to the user</param>
        /// <param name="classDef">The class definition holding a grid def used to
        /// construct the grid for that type</param>
        public void AddItem(string itemName, ClassDef classDef)
        {
            ITreeNode treeNode = _controlFactory.CreateTreeNode(itemName);
            _currentSectionNode.Nodes.Add(treeNode);
            _items.Add(itemName, classDef);
        }

        /// <summary>
        /// Selects an item with the given name in the treeview
        /// </summary>
        /// <param name="itemName">The name of the item to select</param>
        public void SelectItem(string itemName)
        {
            ClassDef classDef;
            if (_items.ContainsKey(itemName)) classDef = _items[itemName];
            else
            {
                _gridControl.Enabled = false;
                return;
            }

            try
            {
                _gridControl.Initialise(classDef);
                _gridControl.Grid.SetBusinessObjectCollection(
                    BORegistry.GetDataAccessor(classDef.ClassType).BusinessObjectLoader.GetBusinessObjectCollection(classDef, ""));
                _gridControl.Enabled = true;
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "There was a problem loading a collection of " + classDef.ClassName,
                                                          "Problem loading");
            }
        }

        /// <summary>
        /// Saves the changes made to the grid
        /// </summary>
        /// <returns>Returns true if saved successfully</returns>
        public bool SaveChanges()
        {
            try
            {
                this._gridControl.Grid.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "There was a problem saving:", "Problem saving");
                return false;
            }
        }

        /// <summary>
        /// Rejects (restores) changes to the grid since the last save
        /// </summary>
        /// <returns>Returns true if restored successfully</returns>
        public bool RejectChanges()
        {
            try
            {
                this._gridControl.Grid.RejectChanges();
                return true;
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "There was a problem cancelling changes:", "Problem cancelling");
                return false;
                ;
            }
        }
    }
}