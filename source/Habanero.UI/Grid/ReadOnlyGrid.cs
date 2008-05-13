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
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Habanero.BO;
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
        private static readonly ILog log = LogManager.GetLogger("Habanero.UI.Grid.Grid");
        private GridSelectionController _gridSelectionController;

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
            _gridSelectionController = new GridSelectionController(this);
        }

        #region Public Interface to Selection methods.

        /// <summary>
        /// Gets or sets the single selected business object (null if none are selected)
        /// denoted by where the current selected cell is
        /// </summary>
        public BusinessObject SelectedBusinessObject
        {
            get { return _gridSelectionController.SelectedBusinessObject; }
            set { _gridSelectionController.SelectedBusinessObject = value; }
        }

        /// <summary>
        /// Reselects the current row and creates a new item selected event
        /// if the current row has changed
        /// </summary>
        public void ReselectSelectedRow()
        {
            //this.Grid.SelectedBusinessObject = null;
            //_oldRowNumber = -1;
            //FireItemSelectedIfCurrentRowChanged();

            // TODO Check how this method is used because the above code wouldn't have done the right thing ( - Mark ).
            // TODO Check if this line below correctly repaces the above code or fulfills the objectives of the method ( - Mark ).
            _gridSelectionController.Reselect();
        }

        /// <summary>
        /// Adds another delegate to those of the selected item
        /// </summary>
        /// <param name="boDelegate">The delegate to add</param>
        public void AddItemSelectedDelegate(SetBusinessObjectDelegate boDelegate)
        {
            _gridSelectionController.AddItemSelectedDelegate(boDelegate);
        }

        #endregion //Public Interface to Selection methods.

        /// <summary>
        /// Handles the event of a double-click
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void DoubleClickHandler(object sender, EventArgs e)
        {
            Point pt = this.PointToClient(Cursor.Position);
            HitTestInfo hti = this.HitTest(pt.X, pt.Y);
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