//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.UI.Grid;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Provides an editor that contains a treeview on the left and a corresponding
    /// editable grid on the right for each treeview entry.  This editor is useful
    /// for editing simple information such as lookup lists or contacts.
    /// <br/>
    /// Simply supply a data source that implements ITreeViewDataSource, which
    /// includes GetTreeViewData().  This supplies an IList with DictionaryEntry objects
    /// containing the text to appear in the treeview and the BusinessObject to model
    /// in the grid.  The grid design is drawn from the "ui" and "grid" elements in
    /// the object's class definition (XML).
    /// </summary>
    public class TreeViewTableEditor : UserControl
    {

        private ITableDataSource _tableDataSource;
        private SplitContainer _splitContainer1;
        protected TreeView _treeView;
        private GroupBox _groupBox1;
        private EditableGridWithButtons _gridAndButtons;
        private ITreeViewDataSource _treeViewDataSource;

        /// <summary>
        /// Constructor to initialise a new editor
        /// </summary>
        public TreeViewTableEditor()
        {
            //BorderLayoutManager mainManager = new BorderLayoutManager(this);
            //_treeView = ControlFactory.CreateTreeView("TreeView");
            //mainManager.AddControl(_treeView, BorderLayoutManager.Position.West, true);
            InitializeComponent();

            _tableDataSource = new DatabaseTableDataSource();
            _treeViewDataSource = new NullTreeViewDataSource();
            _treeView.BeforeSelect += delegate(object sender, TreeViewCancelEventArgs e)
                  {
                      DirtyCheckHandler(sender, e);
                  };
        }

        /// <summary>
        /// Gets and sets the data source for the table
        /// </summary>
        public ITableDataSource TableDataSource
        {
            get { return _tableDataSource; }
            set { _tableDataSource = value; }
        }

        /// <summary>
        /// Gets and sets the data source for the tree view
        /// </summary>
        public ITreeViewDataSource TreeViewDataSource
        {
            get { return _treeViewDataSource; }
            set { _treeViewDataSource = value; }
        }

        /// <summary>
        /// Gets the editable grid in which values are edited
        /// </summary>
        public EditableGridWithButtons DataGrid
        {
            get { return _gridAndButtons; }
        }

        /// <summary>
        /// Fills the tree view with data
        /// </summary>
        public void PopulateTreeView()
        {
            IList data = _treeViewDataSource.GetTreeViewData();
            TreeNode currentParentNode = null;
            foreach (DictionaryEntry entry in data)
            {
                if (entry.Value == null)
                {
                    if (currentParentNode != null)
                    {
                        _treeView.Nodes.Add(currentParentNode);
                    }
                    currentParentNode = new TreeNode(entry.Key.ToString());
                }
                else
                {
                    TreeNode childNode = new TreeNode(entry.Key.ToString());
                    childNode.Tag = entry.Value;
                    currentParentNode.Nodes.Add(childNode);
                }
            }
            _treeView.Nodes.Add(currentParentNode);
            _treeView.AfterSelect += new TreeViewEventHandler(AfterSelectHandler);
        }

        /// <summary>
        /// Handles instructions to carry out a select event
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void AfterSelectHandler(object sender, TreeViewEventArgs e)
        {
            NodeSelected(e);
        }

        /// <summary>
        /// Handles the event of a node being selected (this is called by
        /// AfterSelectHandler())
        /// </summary>
        /// <param name="e">Attached arguments regarding the event</param>
        protected virtual void NodeSelected(TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                IBusinessObject sampleBo = (BusinessObject) e.Node.Tag;
				IBusinessObjectCollection collectionForNode;
                try
                {
                    collectionForNode = _tableDataSource.GetCollection(sampleBo);
                    _gridAndButtons.Grid.SetCollection(collectionForNode);
                    //_grid.te = e.Node.Text;
                }
                catch (Exception ex)
                {
                    ExceptionUtilities.Display(ex);
                }
            }
        }

        private void DirtyCheckHandler(object sender, TreeViewCancelEventArgs e)
        {
            if (this.DataGrid != null
                && this.DataGrid.Grid != null
                && this.DataGrid.Grid.GetCollection() != null)
            {
                if (this.DataGrid.Grid.GetCollection().IsDirty)
                {
                    DialogResult result = MessageBox.Show("Do you want to save changes?",
                                                          "Save?",
                                                          MessageBoxButtons.YesNoCancel,
                                                          MessageBoxIcon.Exclamation);
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            ((EditableGrid) DataGrid.Grid).AcceptChanges();
                        }
                        catch (Exception)
                        {
                            e.Cancel = true;
                        }
                    }
                    if (result == DialogResult.No)
                    {
                        try
                        {
                            ((EditableGrid) DataGrid.Grid).RejectChanges();
                        }
                        catch (Exception)
                        {
                            e.Cancel = true;
                        }
                    }
                    if (result == DialogResult.Cancel) e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// An interface to model a data source for a table
        /// </summary>
        public interface ITableDataSource
        {
            /// <summary>
            /// Returns the business object collection
            /// </summary>
            /// <param name="sampleBo">A sample business object</param>
            /// <returns>Returns the business object collection</returns>
			IBusinessObjectCollection GetCollection(IBusinessObject sampleBo);
        }

        /// <summary>
        /// An interface to model a data source for a tree view
        /// </summary>
        public interface ITreeViewDataSource
        {
            /// <summary>
            /// Returns a list of the tree view data
            /// </summary>
            /// <returns>Returns an IList object</returns>
            IList GetTreeViewData();
        }

        /// <summary>
        /// Manages a data source for a table
        /// </summary>
        private class DatabaseTableDataSource : ITableDataSource
        {
            /// <summary>
            /// Returns the business object collection
            /// </summary>
            /// <param name="sampleBo">A sample business object</param>
            /// <returns>Returns the business object collection</returns>
			public IBusinessObjectCollection GetCollection(IBusinessObject sampleBo)
            {
                return BOLoader.Instance.GetBusinessObjectCol(sampleBo.GetType(), "", "");
            }
        }

        /// <summary>
        /// Provides an empty data source for a tree view
        /// </summary>
        private class NullTreeViewDataSource : ITreeViewDataSource
        {
            /// <summary>
            /// Returns an empty list
            /// </summary>
            /// <returns>Returns a new, empty IList object</returns>
            public IList GetTreeViewData()
            {
                return new ArrayList();
            }
        }

        /// <summary>
        /// Initialises the user interface components
        /// </summary>
        private void InitializeComponent()
        {
            this._splitContainer1 = new SplitContainer();
            this._treeView = new TreeView();
            _treeView.Name = "TreeView";
            this._groupBox1 = new GroupBox();
            this._gridAndButtons = new EditableGridWithButtons();
            this._splitContainer1.Panel1.SuspendLayout();
            this._splitContainer1.Panel2.SuspendLayout();
            this._splitContainer1.SuspendLayout();
            this._groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _splitContainer1
            // 
            this._splitContainer1.Dock = DockStyle.Fill;
            this._splitContainer1.Location = new Point(0, 0);
            this._splitContainer1.Name = "SplitContainer";
            // 
            // _splitContainer1.Panel1
            // 
            this._splitContainer1.Panel1.Name = "Panel1";
            this._splitContainer1.Panel1.Controls.Add(this._treeView);
            // 
            // _splitContainer1.Panel2
            // 
            this._splitContainer1.Panel2.Controls.Add(this._groupBox1);
            this._splitContainer1.Size = new Size(408, 231);
            this._splitContainer1.SplitterDistance = 136;
            this._splitContainer1.TabIndex = 0;
            // 
            // _treeView
            // 
            this._treeView.Dock = DockStyle.Fill;
            this._treeView.Location = new Point(0, 0);
            this._treeView.Name = "MyTreeView";
            this._treeView.Size = new Size(136, 231);
            this._treeView.TabIndex = 1;
            // 
            // _groupBox1
            // 
            this._groupBox1.Controls.Add(this._gridAndButtons);
            this._groupBox1.Dock = DockStyle.Fill;
            this._groupBox1.Location = new Point(0, 0);
            this._groupBox1.Name = "_groupBox1";
            this._groupBox1.Size = new Size(268, 231);
            this._groupBox1.TabIndex = 0;
            this._groupBox1.TabStop = false;
            // 
            // _gridAndButtons
            // 
            this._gridAndButtons.Dock = DockStyle.Fill;
            this._gridAndButtons.Location = new Point(3, 16);
            this._gridAndButtons.Name = "_gridAndButtons";
            this._gridAndButtons.Size = new Size(262, 212);
            this._gridAndButtons.TabIndex = 0;
            // 
            // TreeViewTableEditor
            // 
            this.Controls.Add(this._splitContainer1);
            this.Name = "TreeViewTableEditor";
            this.Size = new Size(408, 231);
            this._splitContainer1.Panel1.ResumeLayout(false);
            this._splitContainer1.Panel2.ResumeLayout(false);
            this._splitContainer1.ResumeLayout(false);
            this._groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        internal void Initialise()
        {
            this.InitializeComponent();
        }
    }
}