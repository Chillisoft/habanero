using System;
using System.Collections;
using System.Data;
using Chillisoft.Generic.v2;
using log4net;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Provides an editable data-set for business objects
    /// </summary>
    /// TODO ERIC - some more clarity on what a data set is would be nice
    /// (and for other equivalents too)
    public class BusinessObjectCollectionEditableDataSetProvider : BusinessObjectCollectionDataSetProvider
    {
        private static readonly ILog log =
            LogManager.GetLogger("Chillisoft.Bo.v2.BusinessObjectCollectionEditableDataSetProvider");

        private Hashtable itsRowStates;
        private Hashtable itsDeletedRowIDs;
        private IDatabaseConnection itsConnection;
        private bool itsIsBeingAdded = false;

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
        public BusinessObjectCollectionEditableDataSetProvider(BusinessObjectBaseCollection col) : base(col)
        {
        }

        /// <summary>
        /// Adds handlers to be called when updates occur
        /// </summary>
        public override void AddHandlersForUpdates()
        {
            itsTable.TableNewRow += new DataTableNewRowEventHandler(NewRowHandler);
            itsTable.RowChanged += new DataRowChangeEventHandler(RowChangedHandler);
            itsTable.RowDeleted += new DataRowChangeEventHandler(RowDeletedHandler);
            itsCollection.BusinessObjectAdded += new BusinessObjectEventHandler(BusinessObjectAddedToCollectionHandler);
        }

        /// <summary>
        /// Handles the event of a new row being added
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        void NewRowHandler(object sender, DataTableNewRowEventArgs e)
        {
            if (itsObjectInitialiser != null)
            {
                itsObjectInitialiser.InitialiseDataRow(e.Row);
            }
        }

        /// <summary>
        /// Removes the handlers that are called in the event of updates
        /// </summary>
        public void RemoveHandlersForUpdates()
        {
            itsTable.RowChanged -= new DataRowChangeEventHandler(RowChangedHandler);
            itsTable.RowDeleted -= new DataRowChangeEventHandler(RowDeletedHandler);
            itsCollection.BusinessObjectAdded -= new BusinessObjectEventHandler(BusinessObjectAddedToCollectionHandler);
        }

        /// <summary>
        /// Initialises the local data
        /// </summary>
        public override void InitialiseLocalData()
        {
            itsRowStates = new Hashtable();
            itsDeletedRowIDs = new Hashtable();
        }

        /// <summary>
        /// Gets and sets the database connection
        /// </summary>
        public IDatabaseConnection Connection
        {
            get { return itsConnection; }
            set { itsConnection = value; }
        }

        /// <summary>
        /// Handles the event of a row being deleted
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        protected void RowDeletedHandler(object sender, DataRowChangeEventArgs e)
        {
            BusinessObjectBase changedBo = itsCollection.Find(e.Row["ID", DataRowVersion.Original].ToString());
            if (changedBo != null)
            {
                changedBo.Delete();
                itsRowStates[e.Row] = RowState.Deleted;
                itsDeletedRowIDs[e.Row] = changedBo.ID.ToString();
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
            if (itsRowStates[e.Row] != null)
            {
                BusinessObjectBase changedBo;
                if ((RowState) itsRowStates[e.Row] != RowState.Deleted)
                {
                    changedBo = itsCollection.Find(e.Row["ID"].ToString());
                    changedBo.CancelEdit();
                    itsRowStates.Remove(e.Row);
                }
                else
                {
                    changedBo = itsCollection.Find((string) itsDeletedRowIDs[e.Row]);
                    changedBo.CancelEdit();
                    itsRowStates.Remove(e.Row);
                    itsDeletedRowIDs.Remove(e.Row);
                }
            }
        }

        /// <summary>
        /// Handles the event of a row being committed to the database
        /// </summary>
        /// <param name="e">Attached arguments regarding the event</param>
        private void RowCommitted(DataRowChangeEventArgs e)
        {
            if (itsRowStates[e.Row] != null)
            {
                BusinessObjectBase changedBo;
                try
                {
                    if ((RowState) itsRowStates[e.Row] != RowState.Deleted)
                    {
                        //log.Debug("Saving...");
                        changedBo = itsCollection.Find(e.Row["ID"].ToString());
                        changedBo.ApplyEdit();
                        itsRowStates.Remove(e.Row);
                    }
                    else
                    {
                        changedBo = itsCollection.Find((string) itsDeletedRowIDs[e.Row]);
                        changedBo.ApplyEdit();
                        itsRowStates.Remove(e.Row);
                        itsDeletedRowIDs.Remove(e.Row);
                    }
                }
                catch (BaseApplicationException ex)
                {
                    GlobalRegistry.UIExceptionNotifier.Notify(ex, "There was a problem saving.", "Problem Saving");
                    //log.Debug(ExceptionUtil.GetExceptionString(ex, 0) ) ;
                }
            }
            else
            {
                try
                {
                    log.Debug("RowCommitted:  Row state is null for row " + e.Row["ID"]);
                }
                catch (System.Data.RowNotInTableException)
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
            BusinessObjectBase changedBo = itsCollection.Find(e.Row["ID"].ToString());
            if (changedBo != null && !itsIsBeingAdded)
            {
                foreach (UIGridProperty uiProperty in itsUIGridProperties)
                {
                    if (uiProperty.PropertyName.IndexOf(".") == -1)
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
                itsIsBeingAdded = true;
                BusinessObjectBase newBo;
                if (itsConnection != null)
                {
                    newBo = itsCollection.ClassDef.CreateNewBusinessObject(itsConnection);
                }
                else
                {
                    newBo = itsCollection.ClassDef.CreateNewBusinessObject();
                }
                //log.Debug("Initialising obj");
                if (this.itsObjectInitialiser != null)
                {
                    this.itsObjectInitialiser.InitialiseObject(newBo);
                }
                // set all the values in the grid to the bo's current prop values (defaults)
                // make sure the part entered to create the row is not changed.
                foreach (UIGridProperty uiProperty in itsUIGridProperties)
                {
                    if (uiProperty.PropertyName.IndexOf(".") == -1)
                    {
                        if (DBNull.Value.Equals(e.Row[uiProperty.PropertyName]))
                        {
                            e.Row[uiProperty.PropertyName] = newBo.GetPropertyValueForUser(uiProperty.PropertyName);
                        }
                    }
                }

                AddNewRowToCollection(newBo);
                //log.Debug(newBo.GetDebugOutput()) ;
                e.Row["ID"] = newBo.ID.ToString();
                AddToRowStates(e.Row, RowState.Added);

                //log.Debug("Row added complete.") ;
                //log.Debug(newBo.GetDebugOutput()) ;
                itsIsBeingAdded = false;
                this.RowChanged(e);
            }
            catch (Exception ex)
            {
                itsIsBeingAdded = false;
                throw ex;
            }
        }

        /// <summary>
        /// Adds a new row to the collection, containing the specified business
        /// object
        /// </summary>
        /// <param name="newBo">The new business object</param>
        private void AddNewRowToCollection(BusinessObjectBase newBo)
        {
            //log.Debug("Adding new row to col");
            itsCollection.BusinessObjectAdded -= new BusinessObjectEventHandler(BusinessObjectAddedToCollectionHandler);
            itsTable.RowChanged -= new DataRowChangeEventHandler(RowChangedHandler);

            //log.Debug("Disabled handler, adding obj to col") ;
            itsCollection.Add(newBo);
            //log.Debug("Done adding obj to col, enabling handler") ;
            itsTable.RowChanged += new DataRowChangeEventHandler(RowChangedHandler);
            itsCollection.BusinessObjectAdded += new BusinessObjectEventHandler(BusinessObjectAddedToCollectionHandler);
            //log.Debug("Done Adding new row to col");
        }

        /// <summary>
        /// Handles the event of a new business object being added
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectAddedToCollectionHandler(object sender, BusinessObjectEventArgs e)
        {
            //log.Debug("BO Added to collection " + e.BusinessObject.ID);
            itsTable.RowChanged -= new DataRowChangeEventHandler(RowChangedHandler);
            DataRow row = itsTable.NewRow();
            object[] values = new object[itsUIGridProperties.Count + 1];
            values[0] = e.BusinessObject.ID.ToString();
            int i = 1;
            BOMapper mapper = new BOMapper(e.BusinessObject);
            foreach (UIGridProperty gridProperty in itsUIGridProperties)
            {
                values[i++] = mapper.GetPropertyValueForUser(gridProperty.PropertyName);
            }
            itsTable.LoadDataRow(values, false);
            itsRowStates.Add(this.itsTable.Rows[this.FindRow(e.BusinessObject)], RowState.Added);
            itsTable.RowChanged += new DataRowChangeEventHandler(RowChangedHandler);

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
            if (!this.itsRowStates.ContainsKey(row))
            {
                itsRowStates.Add(row, rowState);
            }
        }
    }
}