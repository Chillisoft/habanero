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
using System.Collections.Generic;
using System.Data;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Util;
//using log4net;

namespace Habanero.BO
{
    /// <summary>
    /// Provides an editable data-set for business objects
    /// </summary>
    public class EditableDataSetProvider : DataSetProvider
    {
//        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.EditableDataSetProvider");
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


        /// <summary>
        /// Constructor to initialise a new provider with the business object
        /// collection provided
        /// </summary>
        /// <param name="col">The business object collection</param>
        public EditableDataSetProvider(IBusinessObjectCollection col) : base(col)
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
            if (rowNum >= 0) return; //If row already exists in the datatable then do not add it.
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
        private void NewRowHandler(object sender, DataTableNewRowEventArgs e)
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
        }

        /// <summary>
        /// Initialises the local data
        /// </summary>
        public override void InitialiseLocalData()
        {
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
            try
            {
                DataRow row = e.Row;
                IBusinessObject changedBo = GetBusinessObjectForRow(row);
                if (changedBo == null) return;
                if (changedBo.Status.IsNew)
                {
                    _collection.BusinessObjectRemoved -= RemovedHandler;
                    _collection.Remove(changedBo);
                    _collection.BusinessObjectRemoved += RemovedHandler;
                    return;
                }
                _collection.BusinessObjectRemoved -= RemovedHandler;
                changedBo.MarkForDelete();
                _deletedRows.Add(row, changedBo);
                _collection.BusinessObjectRemoved += RemovedHandler;
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
            finally
            {
                _collection.BusinessObjectRemoved += RemovedHandler;
            }
        }

        private IBusinessObject GetBusinessObjectForRow(DataRow row)
        {
            IBusinessObject changedBo = null;
            if (row.RowState == DataRowState.Detached)
            {
                return null;
            }
            if (row.HasVersion(DataRowVersion.Original))
            {
                string originalRowID = row[IDColumnName, DataRowVersion.Original].ToString();
                changedBo = _collection.Find(new Guid(originalRowID));
                if (changedBo == null && _deletedRows.ContainsKey(row))
                {
                    changedBo = _deletedRows[row];
                }
            }
            if (changedBo == null)
            {
                string currencyRowID = row[IDColumnName].ToString();
                changedBo = _collection.Find(new Guid(currencyRowID));
                if (changedBo == null && _deletedRows.ContainsKey(row))
                {
                    changedBo = _deletedRows[row];
                }
            }
            return changedBo;
        }

        /// <summary>
        /// Handles the event of a row being changed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        protected void RowChangedHandler(object sender, DataRowChangeEventArgs e)
        {
            if (_isBeingAdded) return;

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
            IBusinessObject changedBo;
            if (row.RowState == DataRowState.Detached)
            {
                changedBo = _addedRows[row];
                _collection.Remove(changedBo);
                return;
            }
            changedBo = GetBusinessObjectForRow(row);
            if (changedBo != null) changedBo.Restore();
        }

        /// <summary>
        /// Handles the event of a row being committed to the database
        /// </summary>
        /// <param name="e">Attached arguments regarding the event</param>
        private void RowCommitted(DataRowChangeEventArgs e)
        {
            DataRow row = e.Row;
            try
            {
                IBusinessObject changedBo = GetBusinessObjectForRow(row);
                if (changedBo != null) changedBo.Save();
                row.RowError = "";
            }
            catch (Exception ex)
            {
                string message = "There was a problem saving. : " + ex.Message;
                row.RowError = message;
                GlobalRegistry.UIExceptionNotifier.Notify(ex, message, "Problem Saving");
            }
        }

        /// <summary>
        /// Handles the event of a row being changed
        /// </summary>
        /// <param name="e">Attached arguments regarding the event</param>
        private void RowChanged(DataRowChangeEventArgs e)
        {
            object objectID = e.Row[IDColumnName];
            Guid guidObjectID;
            if (objectID == null)
            {
                const string message =
                    "The Editable Dataset provider is not set up correctly since the Row ID is null for a row.";
                throw new HabaneroDeveloperException(message, message);
            }

            if (StringUtilities.GuidTryParse(objectID.ToString(), out guidObjectID))
            {
                if (guidObjectID == Guid.Empty)
                {
                    const string message =
                        "The Editable Dataset provider is not set up correctly since the Row ID is Empty for a row.";
                    throw new HabaneroDeveloperException(message, message);
                }
                IBusinessObject changedBo = _collection.Find(guidObjectID);
                if (changedBo == null || _isBeingAdded) return;
                foreach (UIGridColumn uiProperty in _uiGridProperties)
                {
                    //If not a related property and not a reflective property then update the property //TODO Brett 17 Feb 2009: Why?
                    if (uiProperty.PropertyName.IndexOf(".") == -1 && uiProperty.PropertyName.IndexOf("-") == -1)
                    {
                        changedBo.SetPropertyValue(uiProperty.PropertyName, e.Row[uiProperty.PropertyName]);
                    }
                }
            }
            else
            {
                const string message =
                    "The Editable Dataset provider is not set up correctly since the Row ID is not set as a guid for a row.";
                throw new HabaneroDeveloperException(message, message);
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
                row[IDColumnName] = newBo.ID.ObjectID;
                foreach (UIGridColumn uiProperty in _uiGridProperties)
                {
                    //If not a related Property then Update //TODO Brett 17 Feb 2009: Why?
                    if (uiProperty.PropertyName.IndexOf(".") == -1)
                    {
                        //If no value was typed into the grid then use the default value for the property if one exists.
                        if (DBNull.Value.Equals(row[uiProperty.PropertyName]))
                        {
                            object propertyValueToDisplay = newBo.GetPropertyValueToDisplay(uiProperty.PropertyName);
                            if (propertyValueToDisplay != null)
                            {
                                row[uiProperty.PropertyName] = propertyValueToDisplay;
                            }
                        }
                        else
                        {
                            newBo.SetPropertyValue(uiProperty.PropertyName, row[uiProperty.PropertyName]);
                        }
                    }
                }
                _addedRows.Add(row, newBo);
                if (newBo.ID.ObjectID == Guid.Empty)
                {
                    throw new HabaneroDeveloperException
                        ("Serious error The objectID is Emnpty", "Serious error The objectID is Emnpty");
                }
                _isBeingAdded = false;
            }
            catch (Exception ex)
            {
                _isBeingAdded = false;
                if (e.Row != null)
                {
                    e.Row.RowError = ex.Message;
                }
                throw;
            }
        }
    }
}
