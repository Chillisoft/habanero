#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

namespace Habanero.BO
{
    /// <summary>
    /// Provides an editable data-set for business objects
    /// </summary>
    public class EditableDataSetProvider : DataSetProvider
    {
        private readonly DataTableNewRowEventHandler _newRowHandler;
        private readonly DataRowChangeEventHandler _rowChangedHandler;
        private readonly DataRowChangeEventHandler _rowDeletedHandler;
        private Dictionary<DataRow, IBusinessObject> _addedRows;
        private Dictionary<DataRow, IBusinessObject> _deletedRows;

        private bool _isBeingAdded;
        private bool _registeredForTableEvents = true;


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
            _boAddedHandler = BOAddedHandler;
        }

        /// <summary>
        /// Gets and sets the database connection
        /// </summary>
        public IDatabaseConnection Connection { get; set; }

        /// <summary>
        /// Deregisters for all events to the <see cref="DataSetProvider._table"/>
        /// </summary>
        protected override void DeregisterForTableEvents()
        {
            try
            {
                _registeredForTableEvents = false;
                _table.TableNewRow -= _newRowHandler;
                _table.RowChanged -= _rowChangedHandler;
                _table.RowDeleting -= _rowDeletedHandler;
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex);
                //Hide exception that cannot remove event handler
            }
        }

        /// <summary>
        /// Registers for all events to the <see cref="DataSetProvider._table"/>
        /// </summary>
        protected override void RegisterForTableEvents()
        {
            DeregisterForTableEvents();
            _table.TableNewRow += _newRowHandler;
            _table.RowChanged += _rowChangedHandler;
            _table.RowDeleting += _rowDeletedHandler;
            _registeredForTableEvents = true;
        }

        /// <summary>
        /// Handles the event of a new row being added
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void NewRowHandler(object sender, DataTableNewRowEventArgs e)
        {
            if (ObjectInitialiser != null)
            {
                ObjectInitialiser.InitialiseDataRow(e.Row);
            }
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
        private void RowDeletedHandler(object sender, DataRowChangeEventArgs e)
        {
            DataRow row = e.Row;
            try
            {
                IBusinessObject changedBo = GetBusinessObjectForRow(row);
                if (changedBo == null) return;
                try
                {
                    string message;
                    if (changedBo.IsDeletable(out message))
                    {
                        DeregisterForBOEvents();
                        changedBo.MarkForDelete();
                        changedBo.Save();
                    }
                    else
                    {
                        row.RejectChanges();
                    }
                }
                finally
                {
                   RegisterForBOEvents();
                }
            }
            catch (Exception ex)
            {
                string message = "There was a problem saving. : " + ex.Message;
                row.RowError = message;
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        /// <summary>
        /// Returns the business object mapped to a particular row.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private IBusinessObject GetBusinessObjectForRow(DataRow row)
        {
            IBusinessObject changedBo = null;
            if (row.RowState == DataRowState.Detached)
            {
                if (_deletedRows.ContainsKey(row))
                {
                    changedBo = _deletedRows[row];
                }
            }
            else
            {
                changedBo = _collection.Find(GetRowID(row));
            }

            return changedBo;
        }


        /// <summary>
        /// Restores a row to its previous state before changes were made
        /// </summary>
        /// <param name="e">Attached arguments regarding the event</param>
        private void RowRollback(DataRowChangeEventArgs e)
        {
            DataRow row = e.Row;
            try
            {
                IBusinessObject changedBo;
                if (row.RowState == DataRowState.Detached)
                {
                    changedBo = _addedRows[row];
                    _collection.Remove(changedBo);
                }
                else
                {
                    changedBo = GetBusinessObjectForRow(row);
                    if (changedBo != null)
                    {
                        try
                        {
                            DeregisterForBOEvents();
                            changedBo.CancelEdits();
                        }
                        finally
                        {
                            RegisterForBOEvents();
                        }
                        row.RowError = changedBo.Status.IsValidMessage;
                    }
                    else
                    {
                        row.RowError = "";
                    }
                }
            }
            catch (Exception ex)
            {
                string message = "There was a problem saving. : " + ex.Message;
                row.RowError = message;
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        /// <summary>
        /// Handles the event of a row being changed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void RowChangedHandler(object sender, DataRowChangeEventArgs e)
        {
            if (_isBeingAdded) return;
            if( !_registeredForTableEvents) return;

            DataRow row = e.Row;
            try
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
            catch (Exception ex)
            {
                string message = "There was a problem saving. : " + ex.Message;
                row.RowError = message;
                
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
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
                if (changedBo != null)
                {
                    try
                    {
                        DeregisterForBOEvents();
                        changedBo.Save();
                    }
                    finally
                    {
                        RegisterForBOEvents();
                    }
                    row.RowError = changedBo.Status.IsValidMessage;
                }
                else
                {
                    row.RowError = "";
                }
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
            DataRow row = e.Row;
            try
            {
                DeregisterForBOEvents();


                Guid guidObjectID = GetRowID(row);
                IBusinessObject changedBo = _collection.Find(guidObjectID);
                if (changedBo == null || _isBeingAdded) return;
                foreach (UIGridColumn uiProperty in _uiGridProperties)
                {
                    ApplyRowCellValueToBOProperty(row, uiProperty, changedBo);
                }
                row.RowError = changedBo.Status.IsValidMessage;
            }
            catch (Exception ex)
            {
                string message = "There was a problem saving. : " + ex.Message;
                row.RowError = message;
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
            finally
            {
                RegisterForBOEvents();
            }
        }

        private void ApplyRowCellValueToBOProperty(DataRow row, UIGridColumn uiProperty, IBusinessObject changedBo)
        {
            string columnName = uiProperty.PropertyName;
            if (!uiProperty.Editable) return;
            IBOPropertyMapper boPropertyMapper = BOPropMapperFactory.CreateMapper(changedBo, columnName);
            boPropertyMapper.SetPropertyValue(row[columnName]);
            row.SetColumnError(columnName, boPropertyMapper.InvalidReason);
        }

        private Guid GetRowID(DataRow row)
        {
            object objectID = row[IDColumnName];
            CheckRowIDNotNull(objectID);
            return GetGuidID(objectID);
        }

        private static Guid GetGuidID(object objectID)
        {
            Guid guidObjectID;
            if (!StringUtilities.GuidTryParse(objectID.ToString(), out guidObjectID))
            {
                const string message =
                    "The Editable Dataset provider is not set up correctly since the Row ID is not set as a guid for a row.";
                throw new HabaneroDeveloperException(message, message);
            }
            if (guidObjectID == Guid.Empty)
            {
                const string message =
                    "The Editable Dataset provider is not set up correctly since the Row ID is Empty for a row.";
                throw new HabaneroDeveloperException(message, message);
            }
            return guidObjectID;
        }

        private static void CheckRowIDNotNull(object objectID)
        {
            if (objectID != null) return;

            const string message =
                "The Editable Dataset provider is not set up correctly since the Row ID is null for a row.";
            throw new HabaneroDeveloperException(message, message);
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
                BusinessObject newBo;
                try
                {
                    DeregisterForBOEvents();
                    newBo = (BusinessObject) _collection.CreateBusinessObject();
                }
                finally
                {
                    RegisterForBOEvents();
                }

                if (ObjectInitialiser != null)
                {
                    try
                    {
                        DeregisterForBOEvents();
                        ObjectInitialiser.InitialiseObject(newBo);
                    }
                    finally
                    {
                        RegisterForBOEvents();
                    }
                }
                DataRow row = e.Row;
                try
                {
                    DeregisterForBOEvents();
                    // set all the values in the grid to the bo's current prop values (defaults)
                    // make sure the part entered to create the row is not changed.
                    row[IDColumnName] = newBo.ID.ObjectID;
                    foreach (UIGridColumn uiProperty in _uiGridProperties)
                    {
                        //If no value was typed into the grid then use the default value for the property if one exists.
                        if (DBNull.Value.Equals(row[uiProperty.PropertyName]))
                        {
                            var boMapper = new BOMapper(newBo);
                            var propertyValueToDisplay = boMapper.GetPropertyValueToDisplay(uiProperty.PropertyName);
                            if (propertyValueToDisplay != null)
                            {
                                _table.Columns[uiProperty.PropertyName].ReadOnly = false;
                                row[uiProperty.PropertyName] = propertyValueToDisplay;
                            }
                        }
                        else
                        {
                            ApplyRowCellValueToBOProperty(row, uiProperty, newBo);
                        }
                    }
                }
                finally
                {
                    RegisterForBOEvents();
                }
                string message;
                if (newBo.Status.IsValid(out message))
                {
                    newBo.Save();
                    row.AcceptChanges();
                }
                row.RowError = message;
                if (newBo.Status.IsNew)
                {
                    _addedRows.Add(row, newBo);
                }
                
                if (newBo.ID.ObjectID == Guid.Empty)
                {
                    throw new HabaneroDeveloperException
                        ("Serious error The objectID is Empty", "Serious error The objectID is Empty");
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