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

        private IDataSetProvider _dataSetProvider;

        private IBusinessObjectCollection _boCol;

        private string _uiDefName;

        private DataView _dataTableDefaultView;

        public event EventHandler CollectionChanged;

        private GridLoaderDelegate _gridLoader;
        private IClassDef _classDef;

        public GridBaseManager(IGridBase gridBase, string uiDefName)
        {
            _gridBase = gridBase;
            _uiDefName = uiDefName;
            _gridBase.AutoGenerateColumns = false;
            _gridLoader = DefaultGridLoader;
            _gridBase.AllowUserToAddRows = false;
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
                _gridLoader(_gridBase, null);
                FireCollectionChanged();
                return;
            }
            //Hack: this is to overcome abug in Gizmox where the grid was freezing after delete
            // but should not cause a problem with win since it removed the currently selected item which is the deleted item
            col.BusinessObjectRemoved += delegate { SelectedBusinessObject = null; };

            _gridLoader(_gridBase, _boCol);

//            _gridBase.AllowUserToAddRows = false;

            if (_gridBase.Rows.Count > 0)
            {
                SelectedBusinessObject = null;
                _gridBase.Rows[0].Selected = true;
            }
            FireCollectionChanged();
        }

        public IDataSetProvider DataSetProvider
        {
            get { return _dataSetProvider; }
        }

        public void DefaultGridLoader(IGridBase gridBase, IBusinessObjectCollection boCol)
        {
            if (boCol == null)
            {
                gridBase.DataSource = null;
                return;
            }
            gridBase.DataSource = GetDataTable(boCol);
        }

        protected DataView GetDataTable(IBusinessObjectCollection boCol)
        {
            _dataSetProvider = _gridBase.CreateDataSetProvider(boCol);
            if (this.ClassDef == null)
            {
                this.ClassDef = _boCol.ClassDef;
            }  
            UIDef uiDef = ((ClassDef)this.ClassDef).GetUIDef(UiDefName);
            if (uiDef == null)
            {
                throw new ArgumentException(
                    String.Format(
                        "You cannot Get the data for the grid {0} since the uiDef {1} cannot be found for the classDef {2}",
                        this._gridBase.Name, UiDefName, ((ClassDef)this.ClassDef).ClassName));
            }
            DataTable dataTable = _dataSetProvider.GetDataTable(uiDef.UIGrid);
            _dataTableDefaultView = dataTable.DefaultView;
            return this._dataTableDefaultView;
        }

        public IBusinessObject SelectedBusinessObject
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
                BusinessObject bo = (BusinessObject) value;
                int rowNum = 0;
                foreach (IDataGridViewRow row in gridRows)
                {
                    if (row.Cells["ID"].Value.ToString() == bo.ID.ToString())
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
                    new BusinessObjectCollection<BusinessObject>(this.ClassDef);
                foreach (IDataGridViewRow row in _gridBase.SelectedRows)
                {
                    BusinessObject businessObject = (BusinessObject) GetBusinessObjectAtRow(row.Index);
                    busObjects.Add(businessObject);
                }
                return busObjects;
            }
        }

        public GridLoaderDelegate GridLoader
        {
            get { return this._gridLoader; }
            set { this._gridLoader = value; }
        }

        public string UiDefName
        {
            get { return _uiDefName; }
            set { _uiDefName = value; }
        }

        public IClassDef ClassDef
        {
            get { return _classDef; }
            set { _classDef = value; }
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
        /// <param name="rowIndex">The row Index in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        public IBusinessObject GetBusinessObjectAtRow(int rowIndex)
        {
            int i = 0;

            if (_gridBase.DataSource is DataView)
            {
                foreach (DataRowView dataRowView in _dataTableDefaultView)
                {
                    if (i++ == rowIndex)
                    {
                        return this._dataSetProvider.Find((string) dataRowView.Row["ID"]);
                    }
                }
            }else
            {
                IDataGridViewRow findRow =_gridBase.Rows[rowIndex];
                return this._boCol.Find(findRow.Cells["ID"].Value.ToString());
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
            if (_dataTableDefaultView == null)
            {
                throw new GridBaseInitialiseException(
                    "You cannot apply filters as the collection for the grid has not been set");
            }
            if (filterClause != null)
            {
                _dataTableDefaultView.RowFilter = filterClause.GetFilterClauseString();
            }
            else
            {
                _dataTableDefaultView.RowFilter = null;
            }
        }
        /// <summary>
        /// refreshes the grid with the collection returned by the associated grid.GetBusinessObjectCollection
        /// </summary>
        public void RefreshGrid()
        {
            IBusinessObjectCollection col = this._gridBase.GetBusinessObjectCollection();
            IBusinessObject bo = this._gridBase.SelectedBusinessObject;
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
        public GridDeveloperException(string message) : base(message, "")
        {
        }

        public GridDeveloperException(string message, Exception inner) : base(message, "", inner)
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

        public GridBaseInitialiseException(string message, Exception inner) : base(message, "", inner)
        {
        }

        public GridBaseInitialiseException(string message) : base(message, "")
        {
        }

        public GridBaseInitialiseException()
        {
        }
    }
}