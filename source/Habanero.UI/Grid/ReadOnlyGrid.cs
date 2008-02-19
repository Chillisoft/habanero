//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

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
            ReadOnly = true;
            CollectionChanged += new EventHandler(CollectionChangedHandler);
            DoubleClick += new EventHandler(DoubleClickHandler);
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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
        /// Gets and sets the selected business object in the grid
        /// </summary>
        public BusinessObject SelectedBusinessObject
        {
            set
            {
                ClearSelection();
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
                        break;
                    }
                    i++;
                }
                if (CurrentRow != null && !CurrentRow.Displayed)
                {
                    FirstDisplayedScrollingRowIndex = Rows.IndexOf(CurrentRow);
                }
            }
            get { return this.GetSelectedBusinessObject(); }
        }

        /// <summary>
        /// Returns a list of the business objects currently selected
        /// </summary>
        // TODO Eric: why is this an IList instead of IBusinessObjectCollection?
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