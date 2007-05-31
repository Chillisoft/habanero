using System.Collections;
using System.Windows.Forms;
using Chillisoft.Bo.v2;
using Chillisoft.UI.Generic.v2;
using Chillisoft.Util.v2;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Chillisoft.UI.Application.v2
{
    /// <summary>
    /// Manages an editor that uses a tree view
    /// </summary>
    public class TreeViewTableEditor : UserControl
    {

        private ITableDataSource itsTableDataSource;
        private SplitContainer splitContainer1;
        protected TreeView itsTreeView;
        private GroupBox groupBox1;
        private SimpleGridWithButtons gridAndButtons;
        private ITreeViewDataSource itsTreeViewDataSource;

        /// <summary>
        /// Constructor to initialise a new editor
        /// </summary>
        public TreeViewTableEditor()
        {
            //BorderLayoutManager mainManager = new BorderLayoutManager(this);
            //itsTreeView = ControlFactory.CreateTreeView("TreeView");
            //mainManager.AddControl(itsTreeView, BorderLayoutManager.Position.West, true);
            InitializeComponent();

            itsTableDataSource = new DatabaseTableDataSource();
            itsTreeViewDataSource = new NullTreeViewDataSource();
        }

        /// <summary>
        /// Gets and sets the data source for the table
        /// </summary>
        public ITableDataSource TableDataSource
        {
            get { return itsTableDataSource; }
            set { itsTableDataSource = value; }
        }

        /// <summary>
        /// Gets and sets the data source for the tree view
        /// </summary>
        public ITreeViewDataSource TreeViewDataSource
        {
            get { return itsTreeViewDataSource; }
            set { itsTreeViewDataSource = value; }
        }

        /// <summary>
        /// Fills the tree view with data
        /// </summary>
        public void PopulateTreeView()
        {
            IList data = itsTreeViewDataSource.GetTreeViewData();
            TreeNode currentParentNode = null;
            foreach (DictionaryEntry entry in data)
            {
                if (entry.Value == null)
                {
                    if (currentParentNode != null)
                    {
                        itsTreeView.Nodes.Add(currentParentNode);
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
            itsTreeView.Nodes.Add(currentParentNode);
            itsTreeView.AfterSelect += new TreeViewEventHandler(AfterSelectHandler);
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
                BusinessObjectBase sampleBo = (BusinessObjectBase) e.Node.Tag;
                BusinessObjectBaseCollection collectionForNode;
                try
                {
                    collectionForNode = itsTableDataSource.GetCollection(sampleBo);
                    gridAndButtons.Grid .SetGridDataProvider(
                        new SimpleGridDataProvider(collectionForNode,
                                                   sampleBo.GetUserInterfaceMapper().GetUIGridProperties()));
                    //itsGrid.te = e.Node.Text;
                }
                catch (BaseApplicationException ex)
                {
                    ExceptionUtilities.Display(ex);
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
            BusinessObjectBaseCollection GetCollection(BusinessObjectBase sampleBo);
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
            public BusinessObjectBaseCollection GetCollection(BusinessObjectBase sampleBo)
            {
                return sampleBo.GetBusinessObjectCol("", "");
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.itsTreeView = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gridAndButtons = new Chillisoft.UI.Application.v2.SimpleGridWithButtons();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.itsTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(408, 231);
            this.splitContainer1.SplitterDistance = 136;
            this.splitContainer1.TabIndex = 0;
            // 
            // itsTreeView
            // 
            this.itsTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itsTreeView.Location = new System.Drawing.Point(0, 0);
            this.itsTreeView.Name = "itsTreeView";
            this.itsTreeView.Size = new System.Drawing.Size(136, 231);
            this.itsTreeView.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gridAndButtons);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 231);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // gridAndButtons
            // 
            this.gridAndButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridAndButtons.Location = new System.Drawing.Point(3, 16);
            this.gridAndButtons.Name = "gridAndButtons";
            this.gridAndButtons.Size = new System.Drawing.Size(262, 212);
            this.gridAndButtons.TabIndex = 0;
            // 
            // TreeViewTableEditor
            // 
            this.Controls.Add(this.splitContainer1);
            this.Name = "TreeViewTableEditor";
            this.Size = new System.Drawing.Size(408, 231);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}