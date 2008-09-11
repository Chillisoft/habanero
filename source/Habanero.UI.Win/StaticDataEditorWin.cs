using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
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
        private readonly IControlFactory _controlFactory;
        private TreeView _treeView;
        private TreeNode _currentSectionNode;
        private IEditableGridControl _gridControl;
        private Dictionary<string, ClassDef> _items;

        public StaticDataEditorWin(IControlFactory controlFactory)
        {
            this._controlFactory = controlFactory;
            _items = new Dictionary<string, ClassDef>();
            _treeView = (TreeView) _controlFactory.CreateTreeView("TreeView");
            _gridControl = _controlFactory.CreateEditableGridControl();
            BorderLayoutManager layoutManager = _controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(_gridControl, BorderLayoutManager.Position.Centre);
            layoutManager.AddControl((IControlHabanero) _treeView, BorderLayoutManager.Position.West);
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
                System.Windows.Forms.DialogResult result = MessageBox.Show("Do you want to save changes?",
                                                                           "Save?",
                                                                           MessageBoxButtons.YesNoCancel,
                                                                           MessageBoxIcon.Exclamation);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    e.Cancel = !SaveChanges();
                }
                else if (result == System.Windows.Forms.DialogResult.No)
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
            TreeNode treeNode = new TreeNode(sectionName);
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
            TreeNode treeNode = new TreeNode(itemName);
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
                    BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, ""));
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