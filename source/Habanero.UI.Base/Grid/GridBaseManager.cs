using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    public class GridBaseManager
    {


        private readonly IGridBase _gridBase;

        private BOCollectionDataSetProvider _dataSetProvider;

        private IBusinessObjectCollection _boCol;
        private readonly string _uiDefName;
        private DataView _dataTableDefaultView;

        public event EventHandler CollectionChanged;

        public GridBaseManager(IGridBase gridBase, string uiDefName)
        {
            _gridBase = gridBase;
            _uiDefName = uiDefName;
            _gridBase.AutoGenerateColumns = false;
        }

        public GridBaseManager(IGridBase gridBase) : this(gridBase, "default")
        {
        }

        /// <summary>
        /// Sets the grid's collection to the one specified
        /// </summary>
        /// <param name="col">The collection to display in the grid</param>
        public void SetBusinessObjectCollection(IBusinessObjectCollection col)
        {
            if (_gridBase.Columns.Count <= 0)
            {
                throw new GridBaseSetUpException(
                    "You cannot call SetBusinessObjectCollection if the grid's columns have not been set up");
            }
            _boCol = col;
            if (_boCol == null)
            {
                SelectedBusinessObject = null;
                _gridBase.DataSource = null;
                FireCollectionChanged();
                return;
            }
            //Hack: this is to overcome abug in Gizmox where the grid was freezing after delete
            // but should not cause a problem with win
            col.BusinessObjectRemoved += delegate { SelectedBusinessObject = null; };

            _gridBase.DataSource = GetDataTable();

            _gridBase.AllowUserToAddRows = false;

            if (_gridBase.Rows.Count > 0)
            {
                SelectedBusinessObject = null;
                _gridBase.Rows[0].Selected = true;
            }
            FireCollectionChanged();
        }

        protected DataView GetDataTable()
        {
            _dataSetProvider = new BOCollectionReadOnlyDataSetProvider(this._boCol);
            ClassDef classDef = _boCol.ClassDef;
            UIDef uiDef = classDef.GetUIDef(_uiDefName);
            if (uiDef == null)
            {
                throw new ArgumentException(
                    String.Format(
                        "You cannot Get the data for the grid {0} since the uiDef {1} cannot be found for the classDef {2}",
                        this._gridBase.Name, _uiDefName, classDef.ClassName));
            }
            DataTable dataTable = _dataSetProvider.GetDataTable(uiDef.UIGrid);
            _dataTableDefaultView = dataTable.DefaultView;
            return this._dataTableDefaultView;
        }

        public BusinessObject SelectedBusinessObject
        {
            get
            {
                int rownum = -1;
                for (int i = 0; i < _gridBase.Rows.Count; i++)
                    if (_gridBase.Rows[i].Selected) rownum = i;
                if (rownum < 0) return null;
                return this.GetBusinessObjectAtRow(rownum);
            }
            set
            {
                //TODO_Port: RemovePreviouslySelectedBusinessObjectsEventHandler();
                if (_boCol == null && value != null)
                {
                    throw new GridBaseInitialiseException(
                        "You cannot call SelectedBusinessObject if the collection is not set");
                }
                IDataGridViewRowCollection gridRows = _gridBase.Rows;
                ClearAllSelectedRows(gridRows);
                if (value == null) return;
                int rowNum = 0;
                foreach (DataRowView dataRowView in _dataTableDefaultView)
                {
                    if ((string) dataRowView.Row["ID"] == value.ID.ToString())
                    {
                        gridRows[rowNum].Selected = true;
                        _gridBase.ChangeToPageOfRow(rowNum);
                        break;
                    }
                    rowNum++;
                }
            }
        }


        //TODO_Port:
        //private void RemovePreviouslySelectedBusinessObjectsEventHandler()
        //{
        //    if (SelectedBusinessObject != null)
        //    {
        //        SelectedBusinessObject.Updated -= bo_Updated;
        //    }
        //}

        private static void ClearAllSelectedRows(IDataGridViewRowCollection gridRows)
        {
            for (int i = 0; i < gridRows.Count; i++)
            {
                IDataGridViewRow row = gridRows[i];
                row.Selected = false;
            }
        }

        ///// <summary>
        ///// Calls the BusinessObjectAdded() handler
        ///// </summary>
        ///// <param name="bo">The business object added</param>
        //private void FireBusinessObjectEdited(BusinessObject bo)
        //{
        //    _gridBase.SelectedBusinessObjectEdited(bo);
        //}

        public IList<BusinessObject> SelectedBusinessObjects
        {
            get
            {
                if (_boCol == null) return new List<BusinessObject>();
                BusinessObjectCollection<BusinessObject> busObjects =
                    new BusinessObjectCollection<BusinessObject>(_boCol.ClassDef);
                foreach (IDataGridViewRow row in _gridBase.SelectedRows)
                {
                    BusinessObject businessObject = GetBusinessObjectAtRow(row.Index);
                    busObjects.Add(businessObject);
                }
                return busObjects;
            }
        }

        private void FireCollectionChanged()
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Returns the business object collection being displayed in the grid
        /// </summary>
        /// <returns>Returns a business collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection()
        {
            return _boCol;
        }

        /// <summary>
        /// Returns the business object at the row specified
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        public BusinessObject GetBusinessObjectAtRow(int row)
        {
            int i = 0;
            foreach (DataRowView dataRowView in _dataTableDefaultView)
            {
                if (i++ == row)
                {
                    return this._dataSetProvider.Find((string) dataRowView.Row["ID"]);
                }
            }
            return null;
        }

        public void Clear()
        {
            SetBusinessObjectCollection(null);
        }

        /// <summary>
        /// Sets the sort column and indicates whether
        /// it should be sorted in ascending or descending order
        /// </summary>
        /// <param name="columnName">The column number to set</param>
        /// object property</param>
        /// <param name="isAscending">Whether sorting should be done in ascending
        /// order ("false" sets it to descending order)</param>
        public void SetSortColumn(string columnName, bool isAscending)
        {
            if (_gridBase.DataSource is DataView)
            {
                if (isAscending)
                {
                    ((DataView) _gridBase.DataSource).Sort = columnName + " ASC";
                }
                else
                {
                    ((DataView) _gridBase.DataSource).Sort = columnName + " DESC";
                }
            }
        }

        /// <summary>
        /// Applies a filter clause to the data table and updates the filter.
        /// The filter allows you to determine which objects to display using
        /// some criteria.
        /// </summary>
        /// <param name="filterClause">The filter clause</param>
        public void ApplyFilter(IFilterClause filterClause)
        {
            if (filterClause != null)
            {
                _dataTableDefaultView.RowFilter = filterClause.GetFilterClauseString();
            }
            else
            {
                _dataTableDefaultView.RowFilter = null;
            }
        }

        public void RefreshGrid()
        {
            IBusinessObjectCollection col = this._gridBase.GetBusinessObjectCollection();
            BusinessObject bo = this._gridBase.SelectedBusinessObject;
            SetBusinessObjectCollection(null);
            SetBusinessObjectCollection(col);
            SelectedBusinessObject = bo;
        }
    }

    public class GridBaseSetUpException : Exception
    {

        public GridBaseSetUpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public GridBaseSetUpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public GridBaseSetUpException(string message) : base(message)
        {
        }

        public GridBaseSetUpException()
        {
        }
    }
    public class GridDeveloperException : HabaneroDeveloperException
    {
        public GridDeveloperException(string message) : base(message)
        {
        }

        public GridDeveloperException(string message, Exception inner) : base(message, inner)
        {
        }

        public GridDeveloperException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public GridDeveloperException()
        {
        }
    }


    public class GridBaseInitialiseException : HabaneroDeveloperException
    {

        public GridBaseInitialiseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public GridBaseInitialiseException(string message, Exception inner) : base(message, inner)
        {
        }

        public GridBaseInitialiseException(string message) : base(message)
        {
        }

        public GridBaseInitialiseException()
        {
        }
    }
}