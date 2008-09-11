using System;
using System.Collections.Generic;
using System.Text;
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
    public interface IStaticDataEditor: IControlHabanero
    {
        /// <summary>
        /// Adds a section to the treeview, under which individual items
        /// can be listed
        /// </summary>
        /// <param name="sectionName">The name of the section as it appears to the user</param>
        void AddSection(string sectionName);

        /// <summary>
        /// Adds an item to the treeview
        /// </summary>
        /// <param name="itemName">The name of the item as it appears to the user</param>
        /// <param name="classDef">The class definition holding a grid def used to
        /// construct the grid for that type</param>
        void AddItem(string itemName, ClassDef classDef);

        /// <summary>
        /// Selects an item with the given name in the treeview
        /// </summary>
        /// <param name="itemName">The name of the item to select</param>
        void SelectItem(string itemName);

        /// <summary>
        /// Saves the changes made to the grid
        /// </summary>
        /// <returns>Returns true if saved successfully</returns>
        bool SaveChanges();

        /// <summary>
        /// Rejects (restores) changes to the grid since the last save
        /// </summary>
        /// <returns>Returns true if restored successfully</returns>
        bool RejectChanges();
    }
}
