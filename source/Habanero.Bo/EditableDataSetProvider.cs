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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using log4net;

namespace Habanero.BO
{
    /// <summary>
    /// Provides an editable data-set for business objects
    /// </summary>
    public class EditableDataSetProvider : DataSetProvider
    {
        private static readonly ILog log =
            LogManager.GetLogger("Habanero.BO.EditableDataSetProvider");

//        private Hashtable _rowStates;
//
//        private Hashtable _rowIDs;
//
//        private Hashtable _deletedRowIDs;
        private Dictionary<DataRow, IBusinessObject> _addedRows;
        private Dictionary<DataRow, IBusinessObject> _deletedRows;

        private bool _isBeingAdded;
        private readonly DataRowChangeEventHandler _rowChangedHandler;
        private readonly DataRowChangeEventHandler _rowDeletedHandler;
        private readonly DataTableNewRowEventHandler _newRowHandler;

        /// <summary>
        /// Gets and sets the database connection
        /// </summary>
        public IDatabaseConnection Connection { get; set; }

//        /// <summary>
//        /// An enumeration to specify the state of a row
//        /// </summary>
//        private enum RowState
//        {
//            Added,
//            Deleted,
//            Edited
//        }

        /// <summary>
        /// Constructor to initialise a new provider with the business object
        /// collection provided
        /// </summary>
        /// <param name="col">The business object collection</param>
		public EditableDataSetProvider(IBusinessObjectCollection col)
           : base(col)
        {
            _rowChangedHandler = RowChangedHandler;
            _rowDeletedHandler = RowDeletedHandler;
            _newRowHandler = NewRowHandler;
        }

        /// <summary>
        /// Adds handlers to be called when updates occur
        /// </summary>
        public override void AddHandlersForUpdates()
        {
            base.AddHandlersForUpdates();
            
            _table.TableNewRow += _newRowHandler;
            _table.RowChanged += _rowChangedHandler;
            _table.RowDeleting += _rowDeletedHandler;
        }
        /// <summary>
        /// Handles the event of a business object being added. Adds a new
        /// data row containing the object.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        protected override void AddedHandler(object sender, BOEventArgs e)
        {
            BusinessObject businessObject = (BusinessObject) e.BusinessObject;
            int rowNum = this.FindRow(e.BusinessObject);
            if (rowNum >= 0) return;//If row already exists in the datatable then do not add it.
            object[] values = GetValues(businessObject);
            _table.RowChanged -= _rowChangedHandler;
            _table.LoadDataRow(values, true);
            _table.RowChanged += _rowChangedHandler;
        }

        /// <summary>
        /// Handles the event of a new row being added
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        void NewRowHandler(object sender, DataTableNewRowEventArgs e)
        {
            if (_objectInitialiser != null)
            {
                _objectInitialiser.InitialiseDataRow(e.Row);
            }
        }

        /// <summary>
        /// Removes the handlers that are called in the event of updates
        /// </summary>
        public void RemoveHandlersForUpdates()
        {
            _table.RowChanged -= _rowChangedHandler;
            _table.RowDeleted -= _rowDeletedHandler;
//            _collection.BusinessObjectAdded -= BusinessObjectAddedToCollectionHandler;//TODO  08 Feb 2009: this seems crazy so have commented out.
        }

        /// <summary>
        /// Initialises the local data
        /// </summary>
        public override void InitialiseLocalData()
        {
//            _rowStates = new Hashtable();
//            _rowIDs = new Hashtable();
//            _deletedRowIDs = new Hashtable();
            _addedRows = new Dictionary<DataRow, IBusinessObject>();
            _deletedRows = new Dictionary<DataRow, IBusinessObject>();
        }

        /// <summary>
        /// Handles the event of a row being deleted
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        protected void RowDeletedHandler(object sender, DataRowChangeEventArgs e)
        {
            DataRow row = e.Row;
            BusinessObject changedBo = GetBusinessObjectForRow(row);
            if (changedBo == null) return;
            _collection.BusinessObjectRemoved -= RemovedHandler;
            changedBo.MarkForDelete();
            _deletedRows.Add(row, changedBo);
//            _rowStates[e.Row] = RowState.Deleted;
//            _deletedRowIDs[e.Row] = changedBo.ID.ToString();
//            _rowIDs.Remove(e.Row);
            _collection.BusinessObjectRemoved += RemovedHandler;
        }

        private BusinessObject GetBusinessObjectForRow(DataRow row)
        {
            IBusinessObject changedBo = null;
            if (row.RowState == DataRowState.Detached)
            {
                return null;
            }
            if (row.HasVersion(DataRowVersion.Original))
            {
                string origionalRowID = row[IDColumnName, DataRowVersion.Original].ToString();
                changedBo = _collection.Find(origionalRowID);
                if (changedBo == null && _deletedRows.ContainsKey(row))
                {
                    changedBo = _deletedRows[row];
                }  
            }
            if (changedBo == null)
            {
                string currencyRowID = row[IDColumnName].ToString();
                changedBo = _collection.Find(currencyRowID);
                if (changedBo == null && _deletedRows.ContainsKey(row))
                {
                    changedBo = _deletedRows[row];
                }  
            }
            return (BusinessObject) changedBo;
        }

        /// <summary>
        /// Handles the event of a row being changed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        protected void RowChangedHandler(object sender, DataRowChangeEventArgs e)
        {
            switch (e.Action)
            {
                case DataRowAction.Add:
                    RowAdded(e);
                    break;
                case DataRowAction.Change:
                    RowChanged(e);
                    break;
                case DataRowAction.Commit:
                    RowCommitted(e);
                    break;
                case DataRowAction.Rollback:
                    RowRollback(e);
                    break;
            }
        }

        /// <summary>
        /// Restores a row to its previous state before changes were made
        /// </summary>
        /// <param name="e">Attached arguments regarding the event</param>
        private void RowRollback(DataRowChangeEventArgs e)
        {
            DataRow row = e.Row;
//            if (_rowStates[row] == null) return;
            IBusinessObject changedBo;
            if (row.RowState == DataRowState.Detached)
            {
                changedBo = _addedRows[row];
                _collection.Remove(changedBo);
                return;
            }
            changedBo = GetBusinessObjectForRow(row);
            if (changedBo != null) changedBo.Restore();
//            switch (row.RowState)
//            {
//                case DataRowState.Modified:
////                    changedBo = _collection.Find(row[IDColumnName].ToString());
//                    changedBo.Restore();
////                    _rowStates.Remove(row);
//                    break;
//                case DataRowState.Added:
////                    changedBo = _collection.Find(_rowIDs[row].ToString());
//                    if (changedBo != null) _collection.Remove(changedBo);
////                    _rowStates.Remove(row);
//                    break;
//                case DataRowState.Deleted:
////                    changedBo = _collection.Find((string) _deletedRowIDs[row]);
//                    if (changedBo != null) changedBo.Restore();
////                    _rowStates.Remove(row);
////                    _deletedRowIDs.Remove(row);
//                    break;
//            }
        }

        /// <summary>
        /// Handles the event of a row being committed to the database
        /// </summary>
        /// <param name="e">Attached arguments regarding the event</param>
        private void RowCommitted(DataRowChangeEventArgs e)
        {
//            e.Row.RowState == RowState.Deleted
//            if (_rowStates[row] != null)
//            {
            try
            {
                IBusinessObject changedBo = GetBusinessObjectForRow(e.Row);
                if (changedBo != null) changedBo.Save();
//                    if (row.RowState == DataRowState.Deleted)
//                    {
//                        changedBo = _collection.Find((string) _deletedRowIDs[row]);
//                        if (changedBo != null) changedBo.Save();
////                        _rowStates.Remove(row);
////                        _deletedRowIDs.Remove(row);
//                    }
//                    else
//                    {
//                        //log.Debug("Saving...");
//                        changedBo = _collection.Find(row[IDColumnName].ToString());
//
//                        if (changedBo != null) changedBo.Save();
////                        _rowStates.Remove(row);
//                    }
            }
            catch (Exception ex)
                {
                    GlobalRegistry.UIExceptionNotifier.Notify(ex, "There was a problem saving.", "Problem Saving");
                    //log.Debug(ExceptionUtil.GetExceptionString(ex, 0, true) ) ;
                }
//            }
//            else
//            {
//                try
//                {
//                    log.Debug("RowCommitted:  Row state is null for row " + row[IDColumnName]);
//                }
//                catch (RowNotInTableException)
//                {
//                    log.Debug("RowCommitted:  Row has been removed from table");
//                }
//            }
        }

        /// <summary>
        /// Handles the event of a row being changed
        /// </summary>
        /// <param name="e">Attached arguments regarding the event</param>
        private void RowChanged(DataRowChangeEventArgs e)
        {
            //log.Debug("Row Changed " + e.Row[_idColumnName]);
            IBusinessObject changedBo = _collection.Find(e.Row[IDColumnName].ToString());
            if (changedBo == null || _isBeingAdded) return;
            foreach (UIGridColumn uiProperty in _uiGridProperties)
            {
                if (uiProperty.PropertyName.IndexOf(".") == -1 && uiProperty.PropertyName.IndexOf("-") == -1)
                {
                    changedBo.SetPropertyValue(uiProperty.PropertyName, e.Row[uiProperty.PropertyName]);
                }
            }
//            this.AddToRowStates(e.Row, DataRowState.Modified);
        }

        /// <summary>
        /// Handles the event of a row being added
        /// </summary>
        /// <param name="e">Attached arguments regarding the event</param>
        private void RowAdded(DataRowChangeEventArgs e)
        {
            try
            {
                //log.Debug("Row Added");
                _isBeingAdded = true;
                _collection.BusinessObjectAdded -= AddedHandler;
                BusinessObject newBo = (BusinessObject) _collection.CreateBusinessObject();
                _collection.BusinessObjectAdded += AddedHandler;

                //log.Debug("Initialising obj");
                if (this._objectInitialiser != null)
                {
                    this._objectInitialiser.InitialiseObject(newBo);
                }
                // set all the values in the grid to the bo's current prop values (defaults)
                // make sure the part entered to create the row is not changed.
                DataRow row = e.Row;
                foreach (UIGridColumn uiProperty in _uiGridProperties)
                {
                    if (uiProperty.PropertyName.IndexOf(".") == -1)
                    {
                        if (DBNull.Value.Equals(row[uiProperty.PropertyName]))
                        {
                            row[uiProperty.PropertyName] = newBo.GetPropertyValueToDisplay(uiProperty.PropertyName);
                        }
                    }
                }
                _addedRows.Add(row, newBo);
                //AddNewRowToCollection(newBo);
                //log.Debug(newBo.GetDebugOutput()) ;
//                e.Row[IDColumnName] = newBo.ID.ToString();
//                if (!_rowIDs.ContainsKey(e.Row))
//                {
//                    _rowIDs.Add(e.Row, e.Row[IDColumnName]);
////                    AddToRowStates(e.Row, RowState.Added);
//                }
                //log.Debug("Row added complete.") ;
                //log.Debug(newBo.GetDebugOutput()) ;
                _isBeingAdded = false;
//                this.RowChanged(e);
            }
            catch (Exception)
            {
                _isBeingAdded = false;
                throw;
            }
        }

        ///// <summary>
        ///// Adds a new row to the collection, containing the specified business
        ///// object
        ///// </summary>
        ///// <param name="newBo">The new business object</param>
        //private void AddNewRowToCollection(IBusinessObject newBo)
        //{
        //    //log.Debug("Adding new row to col");
        //    _collection.BusinessObjectAdded -= BusinessObjectAddedToCollectionHandler;
        //    _table.RowChanged -= RowChangedHandler;

        //    //log.Debug("Disabled handler, adding obj to col") ;
        //    _collection.Add(newBo);
        //    //log.Debug("Done adding obj to col, enabling handler") ;
        //    _table.RowChanged += RowChangedHandler;
        //    _collection.BusinessObjectAdded += BusinessObjectAddedToCollectionHandler;
        //    //log.Debug("Done Adding new row to col");
        //}

//        /// <summary>
//        /// Handles the event of a new business object being added
//        /// </summary>
//        /// <param name="sender">The object that notified of the event</param>
//        /// <param name="e">Attached arguments regarding the event</param>
//        private void BusinessObjectAddedToCollectionHandler(object sender, BOEventArgs e)
//        {
//            IBusinessObject businessObject = e.BusinessObject;
//            if (FindRow(businessObject) != -1) return;
//            _table.RowChanged -= _rowChangedHandler;
//            _table.NewRow();
//            object[] values = GetValues(businessObject);
//            _table.LoadDataRow(values, false);
//
//            DataRow newRow = _table.Rows[FindRow(businessObject)];
//            if (!_rowIDs.ContainsKey(newRow))
//            {
//                _rowIDs.Add(newRow, newRow[IDColumnName]);
//                _rowStates.Add(newRow, RowState.Added);
//            }
//            
//            _table.RowChanged += _rowChangedHandler;
//        }

//        /// <summary>
//        /// Sets the state for a particular row
//        /// </summary>
//        /// <param name="row">The row to set</param>
//        /// <param name="rowState">The state to set to. See the RowState
//        /// enumeration for more detail.</param>
//        private void AddToRowStates(DataRow row, RowState rowState)
//        {
//            if (!this._rowStates.ContainsKey(row))
//            {
//                _rowStates.Add(row, rowState);
//            }
//        }
    }
}