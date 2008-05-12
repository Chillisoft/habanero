using System;
using System.Collections.Generic;
using System.Data;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

namespace Habanero.UI.Base
{
    public class GridBaseManager
    {
        private readonly IGridBase _gridBase;
        //private BOCollectionDataSetProvider _dataSetProvider;
        //private DataView _dataTableDefaultView;
        private IBusinessObjectCollection _boCol;

        public GridBaseManager(IGridBase gridBase)
        {
            _gridBase = gridBase;

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
                IDataGridViewRowCollection gridRows = _gridBase.Rows;
                for (int i = 0; i < gridRows.Count; i++)
                {
                    IDataGridViewRow row = gridRows[i];
                    row.Selected = false;
                }
                if (value == null) return;
                int j = 0;
                foreach (BusinessObject businessObject in _boCol)
                {
                    if (businessObject == value)
                    {
                        gridRows[j].Selected = true;
                        break;
                    }
                    j++;
                }
                //foreach (DataRowView dataRowView in _dataTableDefaultView)
                //{
                //    if ((string)dataRowView.Row["ID"] == value.ID.ToString())
                //    {
                //        gridRows[j].Selected = true;
                //        break;
                //    }
                //    j++;
                //}

            }
        }

        public IList<BusinessObject> SelectedBusinessObjects
        {
            get
            {
                if (_boCol == null) return new List<BusinessObject>();
                BusinessObjectCollection<BusinessObject> busObjects = new BusinessObjectCollection<BusinessObject>(_boCol.ClassDef);
                foreach (IDataGridViewRow row in _gridBase.SelectedRows)
                {
                    busObjects.Add((BusinessObject) row.DataBoundItem);
                }
                return busObjects;
            }
        }

        public void SetCollection(IBusinessObjectCollection col)
        {
            _boCol = col;
            //_dataSetProvider = new BOCollectionReadOnlyDataSetProvider(col);
            //ClassDef classDef = col.ClassDef;
            //UIDef uiDef = classDef.GetUIDef("default");
            //UIGrid uiGrid = uiDef.UIGrid;
            //DataTable dataTable = _dataSetProvider.GetDataTable(uiGrid);
            //_dataTableDefaultView = dataTable.DefaultView;
            _gridBase.AllowUserToAddRows = false;
            //_gridBase.DataSource = dataTable;
            _gridBase.DataSource = col;
            if (_gridBase.Rows.Count > 0)
            {
                SelectedBusinessObject = null;
                _gridBase.Rows[0].Selected = true;
            }
 
        }

        /// <summary>
        /// Returns the business object at the row specified
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        private BusinessObject GetBusinessObjectAtRow(int row)
        {

            return _boCol[row];
            //int i = 0; 
            //foreach (DataRowView dataRowView in _dataTableDefaultView)
            //{
            //    if (i++ == row)
            //    {
            //        return this._dataSetProvider.Find((string)dataRowView.Row["ID"]);
            //    }
            //}
            //return null;
        }

        public void Clear()
        {
            SetCollection(null);
        }
    }
}