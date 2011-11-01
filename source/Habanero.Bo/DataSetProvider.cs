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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Habanero.Base;
using Habanero.Base.Logging;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.BO
{
    /// <summary>
    /// Provides a super-class for data-set providers for business objects
    /// </summary>
    public abstract class DataSetProvider : IDataSetProvider
    {
        private const string _idColumnName = "HABANERO_OBJECTID";
        protected static readonly IHabaneroLogger _logger = GlobalRegistry.LoggerFactory.GetLogger(typeof(DataSetProvider));

        /// <summary>
        /// The <see cref="IBusinessObjectCollection"/> of <see cref="IBusinessObject"/>s that
        ///   are being mapped in this DataSetProvider (i.e. are being copied to the <see cref="DataTable"/>.
        /// </summary>
        protected readonly IBusinessObjectCollection _collection;

        /// <summary>
        /// The collection of <see cref="UIGridColumn"/>s that are being shown in this <see cref="DataTable"/>.
        /// </summary>
        protected ICollection _uiGridProperties;

        /// <summary>
        /// The <see cref="DataTable"/> that is set up to represent the items in this collection.
        /// </summary>
        protected DataTable _table;

        /// <summary>
        /// A handler for the <see cref="IBusinessObject"/> has been added to the <see cref="_collection"/>.
        /// </summary>
        protected EventHandler<BOEventArgs> _boAddedHandler;

        /// <summary>
        /// A handler for the <see cref="IBusinessObject"/> has had one of its properties updated
        /// </summary>
        protected readonly EventHandler<BOPropUpdatedEventArgs> _propUpdatedEventHandler;

        /// <summary>
        /// A handler for the <see cref="IBusinessObject"/> has been persisted.
        /// </summary>
        protected readonly EventHandler<BOEventArgs> _updatedHandler;

        /// <summary>
        /// A handler for the <see cref="IBusinessObject"/> has been removed from the <see cref="_collection"/>.
        /// </summary>
        protected readonly EventHandler<BOEventArgs> _removedHandler;

        ///<summary>
        /// Gets and sets whether the property update handler shold be set or not.
        /// This is used to 
        ///    change behaviour typically to differentiate behaviour
        ///    between windows and web.<br/>
        ///Typically in windows every time a business object property is changed
        ///   the grid is updated with Web the grid is updated only when the object
        ///    is persisted.
        /// </summary>
        public bool RegisterForBusinessObjectPropertyUpdatedEvents { get; set; }

        /// <summary>
        /// Constructor to initialise a provider with a specified business
        /// object collection
        /// </summary>
        /// <param name="collection">The business object collection</param>
        protected DataSetProvider(IBusinessObjectCollection collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            this._collection = collection;
            RegisterForBusinessObjectPropertyUpdatedEvents = true;
            _boAddedHandler = BOAddedHandler;
            _propUpdatedEventHandler = PropertyUpdatedHandler;
            _updatedHandler = UpdatedHandler;
            _removedHandler = RemovedHandler;
        }

        /// <summary>
        /// Returns a data table with the UIGridDef provided
        /// </summary>
        /// <param name="uiGrid">The UIGridDef</param>
        /// <returns>Returns a DataTable object</returns>
        [Obsolete("Please use GetDataView instead")]
        public DataTable GetDataTable(IUIGrid uiGrid)
        {
            if (uiGrid == null) throw new ArgumentNullException("uiGrid");
            _table = new DataTable();
            this.InitialiseLocalData();

            _uiGridProperties = uiGrid;
            DataColumn column = _table.Columns.Add();
            column.Caption = _idColumnName;
            column.ColumnName = _idColumnName;
            IClassDef classDef = _collection.ClassDef;
            foreach (UIGridColumn uiProperty in _uiGridProperties)
            {
                AddColumn(uiProperty, classDef);
            }
            foreach (BusinessObject businessObject in _collection.Clone())
            {
                LoadBusinessObject(businessObject);
            }
            this.RegisterForEvents();
            return _table;
        }

#pragma warning disable 612,618
//This should be moved to a private method when this method is removed from UI.
        /// <summary>
        /// Returns a data view for the UIGridDef provided
        /// </summary>
        /// <param name="uiGrid">The UIGridDef</param>
        /// <returns>Returns a DataTable object</returns>
        public IBindingListView GetDataView(IUIGrid uiGrid)
        {
            var dataTable = GetDataTable(uiGrid);
            return dataTable == null ? null : dataTable.DefaultView;
        }

        private void AddColumn(IUIGridColumn uiProperty, IClassDef classDef)
        {
            DataColumn column = _table.Columns.Add();
            if (_table.Columns.Contains(uiProperty.PropertyName))
            {
                throw new DuplicateNameException
                    (String.Format
                         ("In a grid definition, a duplicate column with "
                          + "the name '{0}' has been detected. Only one column " + "per property can be specified.",
                          uiProperty.PropertyName));
            }
            //ILookupList lookupList = classDef.GetLookupList(uiProperty.PropertyName);
            Type columnPropertyType = GetPropertyType(classDef, uiProperty.PropertyName);

            column.DataType = columnPropertyType;
            column.ColumnName = uiProperty.PropertyName;
            column.Caption = uiProperty.ClassDef == null 

                        ? uiProperty.GetHeading(classDef) 
                        : uiProperty.GetHeading();
            //TODO brett 06 Jul 2010: These extended properties are never used by faces any more
            // and can be removed from this column.
            column.ExtendedProperties.Add("LookupList", uiProperty.LookupList);
            column.ExtendedProperties.Add("Width", uiProperty.Width);
            column.ExtendedProperties.Add("Alignment", uiProperty.Alignment);
        }
#pragma warning restore 612,618
        private static Type GetPropertyType(IClassDef classDef, string propertyName)
        {
            IPropDef def = classDef.GetPropDef(propertyName, false);
            if (def == null) return classDef.GetPropertyType(propertyName);
            var lookupList = def.LookupList;
            Type propertyType = def.PropertyType;
            if (lookupList != null && !(lookupList is NullLookupList))
            {
                propertyType = typeof(object);
            }
            return propertyType;
        }

        ///<summary>
        /// Updates the row values for the specified <see cref="IBusinessObject"/>.
        ///</summary>
        ///<param name="businessObject">The <see cref="IBusinessObject"/> for which the row values need to updated.</param>
        public virtual void UpdateBusinessObjectRowValues(IBusinessObject businessObject)
        {
            try
            {
                if (businessObject == null) return;
                int rowNum = this.FindRow(businessObject);
                if (rowNum == -1)
                {
                    return;
                }
                try
                {
                    DeregisterForTableEvents();
                    object[] values = GetValues(businessObject);
                    DataRow row = _table.Rows[rowNum];
                    foreach (DataColumn column in _table.Columns)
                    {
                        column.ReadOnly = false;
                        string propName = column.ColumnName;
                        if (businessObject.Props.Contains(propName))
                        {
                            IBOProp prop = businessObject.Props[propName];
                            if (prop != null) row.SetColumnError(propName, prop.InvalidReason);
                        }
                    }
                    row.RowError = businessObject.Status.IsValidMessage;
                    row.ItemArray = values;

//                if (string.IsNullOrEmpty(row.RowError ))
//                {
//                    row.ClearErrors();
//                } 
//                row.EndEdit();
                }
                finally
                {
                    RegisterForTableEvents();
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        /// <summary>
        /// Deregisters for all events to the <see cref="_table"/>
        /// </summary>
        protected virtual void DeregisterForTableEvents()
        {
        }

        /// <summary>
        /// Registers for all events to the <see cref="_table"/>
        /// </summary>
        protected virtual void RegisterForTableEvents()
        {
        }

        /// <summary>
        /// Gets a list of the property values to display to the user
        /// </summary>
        /// <param name="businessObject">The business object whose
        /// properties are to be displayed</param>
        /// <returns>Returns an array of values</returns>
        protected object[] GetValues(IBusinessObject businessObject)
        {
            if (businessObject == null) throw new ArgumentNullException("businessObject");
            object[] values = new object[_uiGridProperties.Count + 1];
            values[0] = businessObject.ID.ObjectID;
            int i = 1;
            BOMapper mapper = new BOMapper(businessObject);
            foreach (UIGridColumn gridProperty in _uiGridProperties)
            {
                object val = mapper.GetPropertyValueToDisplay(gridProperty.PropertyName);
                values[i++] = val ?? DBNull.Value;
            }
            return values;
        }

/*        /// <summary>
        /// Adds handlers to be called when updates occur
        /// </summary>
        public virtual void DeregisterForEvents()
        {
            DeregisterForBOEvents();
            DeregisterForTableEvents();
        }*/
        /// <summary>
        /// Derigisters the Data Set Provider from all events raised by the BO collection.
        /// </summary>
        protected virtual void DeregisterForBOEvents()
        {
            try
            {
                if (RegisterForBusinessObjectPropertyUpdatedEvents)
                {
                    _collection.BusinessObjectPropertyUpdated -= _propUpdatedEventHandler;
                }
                else
                {
                    _collection.BusinessObjectUpdated -= _updatedHandler;
                }
                _collection.BusinessObjectIDUpdated -= IDUpdatedHandler;
                _collection.BusinessObjectAdded -= _boAddedHandler;
                _collection.BusinessObjectRemoved -= _removedHandler;
            }
            catch (KeyNotFoundException)
            {
                //Hide the exception that the event was not registerd for in the first place
            }
        }

        /// <summary>
        /// Adds handlers to be called when updates occur
        /// </summary>
        public virtual void RegisterForEvents()
        {
            RegisterForBOEvents();
            RegisterForTableEvents();
        }

        /// <summary>
        /// Registers for all events from the <see cref="_collection"/>
        /// </summary>
        protected virtual void RegisterForBOEvents()
        {
            this.DeregisterForBOEvents();
            if (RegisterForBusinessObjectPropertyUpdatedEvents)
            {
                _collection.BusinessObjectPropertyUpdated += _propUpdatedEventHandler;
            }
            _collection.BusinessObjectUpdated += _updatedHandler;
            _collection.BusinessObjectIDUpdated += IDUpdatedHandler;
            _collection.BusinessObjectAdded += _boAddedHandler;
            _collection.BusinessObjectRemoved += _removedHandler;
        }

        private void PropertyUpdatedHandler(object sender, BOPropUpdatedEventArgs propEventArgs)
        {
            BusinessObject businessObject = (BusinessObject) propEventArgs.BusinessObject;
            UpdateBusinessObjectRowValues(businessObject);
        }

        /// <summary>
        /// Handles the event of a <see cref="IBusinessObject"/> being updated
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void UpdatedHandler(object sender, BOEventArgs e)
        {
            BusinessObject businessObject = (BusinessObject) e.BusinessObject;
            UpdateBusinessObjectRowValues(businessObject);
        }

        /// <summary>
        /// Handles the event of a business object being removed. Removes the
        /// data row that contains the object.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        protected virtual void RemovedHandler(object sender, BOEventArgs e)
        {
            lock (_table.Rows)
            {
                int rowNum = this.FindRow(e.BusinessObject);
                if (rowNum == -1) return;

                try
                {
                    this._table.Rows.RemoveAt(rowNum);
                }
                catch (Exception)
                {
                    //IF you hit delete many times in succession then you get an issue with the events interfering and you get a wierd error
                    //This suppresses the error.
                    Console.Write(Xsds.There_was_an_error_in_DataSetProvider_MultipleDelesHit);
                }
            }
        }

        /// <summary>
        /// Handles the event of a business object being added. Adds a new
        /// data row containing the object.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        protected virtual void BOAddedHandler(object sender, BOEventArgs e)
        {
            BusinessObject businessObject = (BusinessObject) e.BusinessObject;
            int rowNum = this.FindRow(e.BusinessObject);
            if (rowNum >= 0) return; //If row already exists in the datatable then do not add it.
            LoadBusinessObject(businessObject);
        }

        /// <summary>
        /// Adds the Business Object to the DataTable
        /// </summary>
        private void LoadBusinessObject(IBusinessObject businessObject)
        {
            object[] values = GetValues(businessObject);
            try
            {
                DeregisterForTableEvents();
                DataRow row = _table.LoadDataRow(values, true);
                foreach (DataColumn column in _table.Columns)
                {
                    string propName = column.ColumnName;
                    if (businessObject.Props.Contains(propName))
                    {
                        IBOProp prop = businessObject.Props[propName];
                        if (prop != null) row.SetColumnError(propName, prop.InvalidReason);
                    }
                }
                if (businessObject != null) row.RowError = businessObject.Status.IsValidMessage;
            }
            finally
            {
                RegisterForTableEvents();
            }
        }

        /// <summary>
        /// Updates the grid ID column when the Business's ID is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IDUpdatedHandler(object sender, BOEventArgs e)
        {
            BusinessObject businessObject = (BusinessObject) e.BusinessObject;
            UpdateBusinessObjectRowValues(businessObject);
        }

        /// <summary>
        /// Initialises the local data
        /// </summary>
        public abstract void InitialiseLocalData();

        //TODO brett 06 Jul 2010: This method should be moved to a private
        // method once the obsolete Find on the interface is removed

        /// <summary>
        /// Returns the business object at the row number specified
        /// </summary>
        /// <param name="rowNum">The row number</param>
        /// <returns>Returns a business object</returns>
        [Obsolete("This is no longer used use Find(Guid objectID) instead 6/7/2010")]
        public IBusinessObject Find(int rowNum)
        {
            DataRow row;
            try
            {
                row = this._table.Rows[rowNum];
            }
            catch (IndexOutOfRangeException ex)
            {
                _logger.Log(ex);
                throw;

            }
            return this.Find(row);
        }

        //TODO brett 06 Jul 2010: This method should be moved to a private
        // method once the obsolete Find on the interface is removed

        /// <summary>
        /// Returns the business object at the row specified
        /// </summary>
        /// <param name="row">The row related to the business object</param>
        /// <returns>Returns a business object</returns>
        [Obsolete("This is no longer used use Find(Guid objectID) instead 6/7/2010")]
        public IBusinessObject Find(DataRow row)
        {
            try
            {
                string objectID = row[_idColumnName].ToString();
                Guid id = new Guid(objectID);
                return this.Find(id);
            }
            catch (DeletedRowInaccessibleException)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a business object that matches the ID provided
        /// </summary>
        /// <param name="objectID">The ID</param>
        /// <returns>Returns a business object</returns>
        public IBusinessObject Find(Guid objectID)
        {
            return _collection.Find(objectID);
        }

        /// <summary>
        /// Finds the row number in which a specified business object resides
        /// </summary>
        /// <param name="bo">The business object to search for</param>
        /// <returns>Returns the row number if found, or -1 if not found</returns>
        public int FindRow(IBusinessObject bo)
        {
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                DataRow dataRow = _table.Rows[i];
                if (dataRow.RowState == DataRowState.Deleted) continue;

                string rowID = dataRow[0].ToString();
                Guid objectID = bo.ID.ObjectID;
                if (rowID == objectID.ToString())
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Sets the object initialiser
        /// </summary>
        public IBusinessObjectInitialiser ObjectInitialiser { get; set; }

        ///<summary>
        /// The column name used for the <see cref="DataTable"/> column which stores the unique object identifier of the <see cref="IBusinessObject"/>.
        /// This column's values will always be the current <see cref="IBusinessObject"/>'s <see cref="IBusinessObject.ID"/> value.
        ///</summary>
        public string IDColumnName
        {
            get { return _idColumnName; }
        }
    }

}