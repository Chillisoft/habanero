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
using System.Data;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    /// <summary>
    /// Provides a super-class for data-set providers for business objects
    /// </summary>
    public abstract class DataSetProvider : IDataSetProvider
    {
        protected readonly IBusinessObjectCollection _collection;
        protected ICollection _uiGridProperties;
        protected DataTable _table;
        protected IBusinessObjectInitialiser _objectInitialiser;

        /// <summary>
        /// Constructor to initialise a provider with a specified business
        /// object collection
        /// </summary>
        /// <param name="collection">The business object collection</param>
        public DataSetProvider(IBusinessObjectCollection collection)
        {
            this._collection = collection;
        }

        /// <summary>
        /// Returns a data table with the UIGridDef provided
        /// </summary>
        /// <param name="uiGrid">The UIGridDef</param>
        /// <returns>Returns a DataTable object</returns>
        public DataTable GetDataTable(UIGrid uiGrid)
        {
            _table = new DataTable();
            this.InitialiseLocalData();

            //BusinessObject sampleBo = _collection.ClassDef.InstantiateBusinessObjectWithClassDef();
            _uiGridProperties = uiGrid; //sampleBo.GetUIDef().GetUIGridProperties();
            DataColumn column = _table.Columns.Add();
            column.Caption = "ID";
            column.ColumnName = "ID";
            ClassDef classDef = _collection.ClassDef;
            foreach (UIGridColumn uiProperty in _uiGridProperties)
            {
                AddColumn(uiProperty, classDef);
            }
            foreach (BusinessObject businessObjectBase in _collection)
            {
                object[] values = GetValues(businessObjectBase);
                _table.LoadDataRow(values, true);
            }
            this.AddHandlersForUpdates();
            return _table;
        }

        private void AddColumn(UIGridColumn uiProperty, ClassDef classDef)
        {
            DataColumn column;
            column = _table.Columns.Add();
            if (_table.Columns.Contains(uiProperty.PropertyName))
            {
                throw new DuplicateNameException(String.Format(
                    "In a grid definition, a duplicate column with " +
                    "the name '{0}' has been detected. Only one column " +
                    "per property can be specified.", uiProperty.PropertyName));
            }
            //TODO : Generalise this for properties that do not have PropDefs
            PropDef propDef = classDef.GetPropDef(uiProperty.PropertyName);
            if (propDef.LookupList is NullLookupList)
            {
                column.DataType = propDef.PropertyType;
            }
            column.ColumnName = uiProperty.PropertyName;
            column.Caption = uiProperty.GetHeading(classDef);
            column.ReadOnly = !uiProperty.Editable;
            column.ExtendedProperties.Add("LookupList", classDef.GetLookupList(uiProperty.PropertyName));
            column.ExtendedProperties.Add("Width", uiProperty.Width);
            column.ExtendedProperties.Add("Alignment", uiProperty.Alignment);
        }

        /// <summary>
        /// Gets a list of the property values to display to the user
        /// </summary>
        /// <param name="businessObject">The business object whose
        /// properties are to be displayed</param>
        /// <returns>Returns an array of values</returns>
        protected object[] GetValues(BusinessObject businessObject)
        {
            object[] values = new object[_uiGridProperties.Count + 1];
            values[0] = businessObject.ID.ToString();
            int i = 1;
            BOMapper mapper = new BOMapper(businessObject);
            foreach (UIGridColumn gridProperty in _uiGridProperties)
            {
                object val = mapper.GetPropertyValueToDisplay(gridProperty.PropertyName);
                ////TODO: Do for derived properties may need some logic for setting val
                /// see previous code for this method
                values[i++] = val;
            }
            return values;
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
        public IBusinessObject Find(int rowNum)
        {
            return _collection.Find(this._table.Rows[rowNum]["ID"].ToString());
        }

        /// <summary>
        /// Returns a business object that matches the ID provided
        /// </summary>
        /// <param name="strId">The ID</param>
        /// <returns>Returns a business object</returns>
        public IBusinessObject Find(string strId)
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
                string gridIDValue = _table.Rows[i][0].ToString();
                string valuePersisted = bo.ID.PersistedValueString();
                string valueBeforeLastEdit = bo.ID.PropertyValueStringBeforeLastEdit();
                string currentValue = bo.ID.ToString();
                if (gridIDValue == valueBeforeLastEdit ||
                    gridIDValue == valuePersisted ||
                    gridIDValue == currentValue)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Sets the object initialiser
        /// </summary>
        public IBusinessObjectInitialiser ObjectInitialiser
        {
            set { _objectInitialiser = value; }
        }
    }
}