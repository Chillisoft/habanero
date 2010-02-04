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
using Habanero.Base;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides an editor for static data in an application.  Static data serves
    /// a number purposes including providing source data for lookup lists used in
    /// drop-downs.
    /// <br/>
    /// The editor typically consists of a TreeView on the left and an EditableGrid
    /// on the right, where data for the selected type in the TreeView can be edited.
    /// </summary>
    public class StaticDataEditorWin : PanelWin, IStaticDataEditor
    {
        private readonly StaticDataEditorManager _staticDataEditorManager;

        ///<summary>
        /// Constructor for <see cref="StaticDataEditorWin"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        public StaticDataEditorWin(IControlFactory controlFactory)
        {
            _staticDataEditorManager = new StaticDataEditorManager(this, controlFactory);
        }

        /// <summary>
        /// Adds a section to the treeview, under which individual items
        /// can be listed
        /// </summary>
        /// <param name="sectionName">The name of the section as it appears to the user</param>
        public void AddSection(string sectionName)
        {
            _staticDataEditorManager.AddSection(sectionName);
        }

        /// <summary>
        /// Adds an item to the treeview
        /// </summary>
        /// <param name="itemName">The name of the item as it appears to the user</param>
        /// <param name="classDef">The class definition holding a grid def used to
        /// construct the grid for that type</param>
        public void AddItem(string itemName, IClassDef classDef)
        {
            _staticDataEditorManager.AddItem(itemName, classDef);
        }

        /// <summary>
        /// Selects an item with the given name in the treeview
        /// </summary>
        /// <param name="itemName">The name of the item to select</param>
        public void SelectItem(string itemName)
        {
            _staticDataEditorManager.SelectItem(itemName);
        }

        /// <summary>
        /// Saves the changes made to the grid
        /// </summary>
        /// <returns>Returns true if saved successfully</returns>
        public bool SaveChanges()
        {
            return _staticDataEditorManager.SaveChanges();
        }

        /// <summary>
        /// Rejects (restores) changes to the grid since the last save
        /// </summary>
        /// <returns>Returns true if restored successfully</returns>
        public bool RejectChanges()
        {
            return _staticDataEditorManager.RejectChanges();
        }
    }
}