using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.UI.Grid;
using log4net;

namespace Habanero.UI.Grid
{
    /// <summary>
    /// Manages a read-only grid (a grid that cannot be edited directly).
    /// The business object collection to display in this grid must be
    /// pre-loaded.
    /// </summary>
    public class ReadOnlyGrid : GridBase, IReadOnlyGrid
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.UI.Grid.ReadOnlyGrid");

        public event RowDoubleClickedHandler RowDoubleClicked;

        /// <summary>
        /// Constructor to initialise a new grid
        /// </summary>
        public ReadOnlyGrid() : base()
        {
            this.ReadOnly = true;
            this.CollectionChanged += new EventHandler(CollectionChangedHandler);
            this.DoubleClick += new EventHandler(DoubleClickHandler);
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
        }

        /// <summary>
        /// Handles the event of a double-click
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void DoubleClickHandler(object sender, EventArgs e)
        {
            System.Drawing.Point pt = this.PointToClient(Cursor.Position);
            DataGridView.HitTestInfo hti = this.HitTest(pt.X, pt.Y);
            if (hti.Type == DataGridViewHitTestType.Cell)
            {
                this.FireRowDoubleClicked(this.SelectedBusinessObject);
            }
        }

        /// <summary>
        /// Handles the event of the collection being updated
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CollectionChangedHandler(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn column in this.Columns)
            {
                column.ReadOnly = true;
            }
        }

        /// <summary>
        /// Gets and sets the selected business object
        /// </summary>
        public BusinessObject SelectedBusinessObject
        {
            set
            {
                if (value == null)
                {
                    if (this.CurrentRow == null) return;
                    this.SetSelectedRowCore(this.CurrentRow.Index, false);
                    return;
                }
                int i = 0;
                foreach (DataRowView dataRowView in _dataTableDefaultView)
                {
                    if ((string) dataRowView.Row["ID"] == value.ID.ToString())
                    {
                        this.SetSelectedRowCore(i, true);
                        this.SetCurrentCellAddressCore(1, i, true, false, false);
                        return;
                    }
                    i++;
                }
            }
            get { return this.GetSelectedBusinessObject(); }
        }

        /// <summary>
        /// Returns a list of the business objects currently selected
        /// </summary>
        public IList SelectedBusinessObjects
        {
            get { return this.GetSelectedBusinessObjects(); }
        }

        /// <summary>
        /// Creates a new data set provider for the business object collection
        /// specified
        /// </summary>
        /// <param name="col">The business object collection</param>
        /// <returns>Returns a new data set provider</returns>
        protected override BOCollectionDataSetProvider CreateBusinessObjectCollectionDataSetProvider(
			IBusinessObjectCollection col)
        {
            return new BOCollectionReadOnlyDataSetProvider(col);
        }

        /// <summary>
        /// Removes the specified business object
        /// </summary>
        /// <param name="businessObject">The business object to remove</param>
        public void RemoveBusinessObject(BusinessObject businessObject)
        {
            _collection.Remove(businessObject);
        }

        /// <summary>
        /// Indicates whether any business objects are currently displayed
        /// in the grid
        /// </summary>
        public bool HasBusinessObjects
        {
            get { return _collection.Count > 0; }
        }

        /// <summary>
        /// Returns a cloned collection of the business objects in the grid
        /// </summary>
        /// <returns>Returns a business object collection</returns>
		public IBusinessObjectCollection GetCollectionClone()
        //public BusinessObjectCollection<BusinessObject> GetCollectionClone()
		{
            return _collection.Clone();
        }

        /// <summary>
        /// Returns a list of the filtered business objects
        /// </summary>
        public List<BusinessObject> FilteredBusinessObjects
        {
            get
            {
                List<BusinessObject> filteredBos = new List<BusinessObject>(_collection.Count);
                foreach (DataRowView dataRowView in _dataTableDefaultView)
                {
                    filteredBos.Add(this._dataSetProvider.Find((string) dataRowView.Row["ID"]));
                }
                return filteredBos;
            }
        }

        /// <summary>
        /// Creates an event for a row being double-clicked
        /// </summary>
        /// <param name="selectedBo">The business object to which the
        /// double-click applies</param>
        public void FireRowDoubleClicked(BusinessObject selectedBo)
        {
            if (this.RowDoubleClicked != null)
            {
                this.RowDoubleClicked(this, new BOEventArgs(selectedBo));
            }
        }
    }
}