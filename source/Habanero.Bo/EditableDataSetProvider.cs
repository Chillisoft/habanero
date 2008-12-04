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

        private Hashtable _rowStates;
        private Hashtable _rowIDs;
        private Hashtable _deletedRowIDs;
        private IDatabaseConnection _connection;
        private bool _isBeingAdded = false;

        /// <summary>
        /// An enumeration to specify the state of a row
        /// </summary>
        private enum RowState
        {
            Added,
            Deleted,
            Edited
        }

        /// <summary>
        /// Constructor to initialise a new provider with the business object
        /// collection provided
        /// </summary>
        /// <param name="col">The business object collection</param>
		public EditableDataSetProvider(IBusinessObjectCollection col)
            : base(col)
        {
        }

        /// <summary>
        /// Adds handlers to be called when updates occur
        /// </summary>
        public override void AddHandlersForUpdates()
        {
            _table.TableNewRow += NewRowHandler;
            _table.RowChanged += RowChangedHandler;
            _table.RowDeleting += RowDeletedHandler;
            _collection.BusinessObjectAdded += BusinessObjectAddedToCollectionHandler;
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
            _table.RowChanged -= RowChangedHandler;
            _table.RowDeleted -= RowDeletedHandler;
            _collection.BusinessObjectAdded -= BusinessObjectAddedToCollectionHandler;
        }

        /// <summary>
        /// Initialises the local data
        /// </summary>
        public override void InitialiseLocalData()
        {
            _rowStates = new Hashtable();
            _rowIDs = new Hashtable();
            _deletedRowIDs = new Hashtable();
        }

        /// <summary>
        /// Gets and sets the database connection
        /// </summary>
        public IDatabaseConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        /// <summary>
        /// Handles the event of a row being deleted
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        protected void RowDeletedHandler(object sender, DataRowChangeEventArgs e)
        {
            BusinessObject changedBo;
            if (e.Row.HasVersion(DataRowVersion.Original))
            {
                changedBo = (BusinessObject) _collection.Find(e.Row["ID", DataRowVersion.Original].ToString());
            }
            else
            {
                changedBo = (BusinessObject) _collection.Find(e.Row["ID"].ToString());
            }
            if (changedBo != null)
            {
                changedBo.Delete();
                _rowStates[e.Row] = RowState.Deleted;
                _deletedRowIDs[e.Row] = changedBo.ID.ToString();
                _rowIDs.Remove(e.Row);
            }
        }

        /// <summary>
        /// Handles the event of a row being changed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        protected void RowChangedHandler(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action == DataRowAction.Add)
            {
                RowAdded(e);
            }
            else if (e.Action == DataRowAction.Change)
            {
                RowChanged(e);
            }
            else if (e.Action == DataRowAction.Commit)
            {
                RowCommitted(e);
            }
            else if (e.Action == DataRowAction.Rollback)
            {
                RowRollback(e);
            }
        }

        /// <summary>
        /// Restores a row to its previous state before changes were made
        /// </summary>
        /// <param name="e">Attached arguments regarding the event</param>
        private void RowRollback(DataRowChangeEventArgs e)
        {
            if (_rowStates[e.Row] != null)
            {
                IBusinessObject changedBo;
                if ((RowState) _rowStates[e.Row] == RowState.Edited)
                {
                    changedBo = _collection.Find(e.Row["ID"].ToString());
                    //changedBo = _collection.Find(_rowIDs[e.Row].ToString());
                    changedBo.Restore();
                    _rowStates.Remove(e.Row);
                }
                else if ((RowState)_rowStates[e.Row] == RowState.Added)
                {
                    //changedBo = _collection.Find(e.Row["ID"].ToString());
                    changedBo = _collection.Find(_rowIDs[e.Row].ToString());
                    changedBo.Delete();
                    _collection.Remove(changedBo);
                    _rowStates.Remove(e.Row);
                    // should deletedRowIDs be added to?
                }
                else if ((RowState)_rowStates[e.Row] == RowState.Deleted)
                {
                    changedBo = _collection.Find((string) _deletedRowIDs[e.Row]);
                    changedBo.Restore();
                    _rowStates.Remove(e.Row);
                    _deletedRowIDs.Remove(e.Row);
                }
            }
        }

        /// <summary>
        /// Handles the event of a row being committed to the database
        /// </summary>
        /// <param name="e">Attached arguments regarding the event</param>
        private void RowCommitted(DataRowChangeEventArgs e)
        {
            if (_rowStates[e.Row] != null)
            {
                IBusinessObject changedBo;
                try
                {
                    if ((RowState) _rowStates[e.Row] != RowState.Deleted)
                    {
                        //log.Debug("Saving...");
                        changedBo = _collection.Find(e.Row["ID"].ToString());

                        changedBo.Save();
                        _rowStates.Remove(e.Row);
                    }
                    else
                    {
                        changedBo = _collection.Find((string) _deletedRowIDs[e.Row]);
                        changedBo.Save();
                        _rowStates.Remove(e.Row);
                        _deletedRowIDs.Remove(e.Row);
                    }
                }
                catch (Exception ex)
                {
                    GlobalRegistry.UIExceptionNotifier.Notify(ex, "There was a problem saving.", "Problem Saving");
                    //log.Debug(ExceptionUtil.GetExceptionString(ex, 0, true) ) ;
                }
            }
            else
            {
                try
                {
                    log.Debug("RowCommitted:  Row state is null for row " + e.Row["ID"]);
                }
                catch (RowNotInTableException)
                {
                    log.Debug("RowCommitted:  Row has been removed from table");
                }
            }
        }

        /// <summary>
        /// Handles the event of a row being changed
        /// </summary>
        /// <param name="e">Attached arguments regarding the event</param>
        private void RowChanged(DataRowChangeEventArgs e)
        {
            //log.Debug("Row Changed " + e.Row["ID"]);
            IBusinessObject changedBo = _collection.Find(e.Row["ID"].ToString());
            if (changedBo != null && !_isBeingAdded)
            {
                foreach (UIGridColumn uiProperty in _uiGridProperties)
                {
					if (uiProperty.PropertyName.IndexOf(".") == -1 && uiProperty.PropertyName.IndexOf("-") == -1)
                    {
                        changedBo.SetPropertyValue(uiProperty.PropertyName, e.Row[uiProperty.PropertyName]);
                    }
                }
                this.AddToRowStates(e.Row, RowState.Edited);
            }
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
                BusinessObject newBo;
                newBo = (BusinessObject) _collection.CreateBusinessObject();

                //log.Debug("Initialising obj");
                if (this._objectInitialiser != null)
                {
                    this._objectInitialiser.InitialiseObject(newBo);
                }
                // set all the values in the grid to the bo's current prop values (defaults)
                // make sure the part entered to create the row is not changed.
                foreach (UIGridColumn uiProperty in _uiGridProperties)
                {
                    if (uiProperty.PropertyName.IndexOf(".") == -1)
                    {
                        if (DBNull.Value.Equals(e.Row[uiProperty.PropertyName]))
                        {
                            e.Row[uiProperty.PropertyName] = newBo.GetPropertyValueToDisplay(uiProperty.PropertyName);
                        }
//                        if (newBo.Props.Contains(uiProperty.PropertyName))
//                        {
//                            newBo.Props[uiProperty.PropertyName].DisplayName = uiProperty.Heading;
//                        }
                    }
                }

                //AddNewRowToCollection(newBo);
                //log.Debug(newBo.GetDebugOutput()) ;
                e.Row["ID"] = newBo.ID.ToString();
                if (!_rowIDs.ContainsKey(e.Row))
                {
                    _rowIDs.Add(e.Row, e.Row["ID"]);
                    AddToRowStates(e.Row, RowState.Added);
                }



                //log.Debug("Row added complete.") ;
                //log.Debug(newBo.GetDebugOutput()) ;
                _isBeingAdded = false;
                this.RowChanged(e);
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

        /// <summary>
        /// Handles the event of a new business object being added
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectAddedToCollectionHandler(object sender, BOEventArgs e)
        {
            IBusinessObject businessObject = e.BusinessObject;
            if (FindRow(businessObject) != -1) return;
            _table.RowChanged -= RowChangedHandler;
            _table.NewRow();
            object[] values = GetValues(businessObject);
            _table.LoadDataRow(values, false);

            DataRow newRow = _table.Rows[FindRow(businessObject)];
            if (!_rowIDs.ContainsKey(newRow))
            {
                _rowIDs.Add(newRow, newRow["ID"]);
                _rowStates.Add(newRow, RowState.Added);
            }
            
            _table.RowChanged += RowChangedHandler;

            //log.Debug("Done adding bo to collection " + e.BusinessObject.ID);
        }

        /// <summary>
        /// Sets the state for a particular row
        /// </summary>
        /// <param name="row">The row to set</param>
        /// <param name="rowState">The state to set to. See the RowState
        /// enumeration for more detail.</param>
        private void AddToRowStates(DataRow row, RowState rowState)
        {
            if (!this._rowStates.ContainsKey(row))
            {
                _rowStates.Add(row, rowState);
            }
        }
    }
}