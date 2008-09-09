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


        public void AddSection(string sectionName)
        {
            TreeNode treeNode = new TreeNode(sectionName);
            _treeView.Nodes.Add(treeNode);
            if (_currentSectionNode == null) _treeView.TopNode = treeNode;
            _currentSectionNode = treeNode;
        }

        public void AddItem(string itemName, ClassDef classDef)
        {
            TreeNode treeNode = new TreeNode(itemName);
            _currentSectionNode.Nodes.Add(treeNode);
            _items.Add(itemName, classDef);
        }

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