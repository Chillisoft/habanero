using System;
using System.Collections;
using System.Data;
using Chillisoft.Generic.v2;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Provides a super-class for data-set providers for business objects
    /// </summary>
    public abstract class BOCollectionDataSetProvider : IDataSetProvider
    {
        protected readonly BusinessObjectCollection _collection;
        protected ICollection _uiGridProperties;
        protected DataTable _table;
        protected IObjectInitialiser _objectInitialiser;

        /// <summary>
        /// Constructor to initialise a provider with a specified business
        /// object collection
        /// </summary>
        /// <param name="collection">The business object collection</param>
        public BOCollectionDataSetProvider(BusinessObjectCollection collection)
        {
            this._collection = collection;
        }

        /// <summary>
        /// Returns a data table with the UIGridDef provided
        /// </summary>
        /// <param name="uiGridDef">The UIGridDef</param>
        /// <returns>Returns a DataTable object</returns>
        public DataTable GetDataTable(UIGridDef uiGridDef)
        {
            _table = new DataTable();
            this.InitialiseLocalData();

            BusinessObject sampleBo = _collection.ClassDef.InstantiateBusinessObjectWithClassDef();
            _uiGridProperties = uiGridDef; //sampleBo.GetUserInterfaceMapper().GetUIGridProperties();
            DataColumn column = _table.Columns.Add();
            column.Caption = "ID";
            column.ColumnName = "ID";
            foreach (UIGridProperty uiProperty in _uiGridProperties)
            {
                column = _table.Columns.Add();
                // TODO: check that property exists in object.
                column.ColumnName = uiProperty.PropertyName;
                column.Caption = uiProperty.Heading;
                column.ReadOnly = uiProperty.IsReadOnly;
                column.ExtendedProperties.Add("LookupListSource",
                                              _collection.ClassDef.GetLookupListSource(uiProperty.PropertyName));
                column.ExtendedProperties.Add("Width", uiProperty.Width);
                column.ExtendedProperties.Add("Alignment", uiProperty.Alignment);
            }
            foreach (BusinessObject businessObjectBase in _collection)
            {
                object[] values = new object[_uiGridProperties.Count + 1];
                values[0] = businessObjectBase.ID.ToString();
                int i = 1;
                BOMapper mapper = new BOMapper(businessObjectBase);
                foreach (UIGridProperty gridProperty in _uiGridProperties)
                {
                    object val = mapper.GetPropertyValueToDisplay(gridProperty.PropertyName);
                    // object val = businessObjectBase.GetPropertyValue(gridProperty.PropertyName);

                    if (val != null && val is DateTime)
                    {
                        val = ((DateTime) val).ToString("yyyy/MM/dd");
                    }
                    else if (val == null)
                    {
                        val = "";
                    }
                    else if (val is Guid)
                    {
                        val = ((Guid) val).ToString("B");
                    }
                    values[i++] = val;
                    //values[i++] = mapper.GetPropertyValueToDisplay(gridProperty.PropertyName);
                    //values[i++] = businessObjectBase.GetPropertyValueToDisplay(gridProperty.PropertyName);
                }
                _table.LoadDataRow(values, true);
            }
            this.AddHandlersForUpdates();
            return _table;
        }

        /// <summary>
        /// Adds handlers to be called when updates occur
        /// </summary>
        public abstract void AddHandlersForUpdates();

        /// <summary>
        /// Initialises the local data
        /// </summary>
        public abstract void InitialiseLocalData();

        /// <summary>
        /// Returns the business object at the row number specified
        /// </summary>
        /// <param name="rowNum">The row number</param>
        /// <returns>Returns a business object</returns>
        public BusinessObject Find(int rowNum)
        {
            return _collection.Find(this._table.Rows[rowNum]["ID"].ToString());
        }

        /// <summary>
        /// Returns a business object that matches the ID provided
        /// </summary>
        /// <param name="strId">The ID</param>
        /// <returns>Returns a business object</returns>
        public BusinessObject Find(string strId)
        {
            return _collection.Find(strId);
        }

        /// <summary>
        /// Finds the row number in which a specified business object resides
        /// </summary>
        /// <param name="bo">The business object to search for</param>
        /// <returns>Returns the row number if found, or -1 if not found</returns>
        public int FindRow(BusinessObject bo)
        {
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                if (_table.Rows[i][0].ToString() == bo.ID.ToString())
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Sets the object initialiser
        /// </summary>
        public IObjectInitialiser ObjectInitialiser
        {
            set { _objectInitialiser = value; }
        }
    }
}