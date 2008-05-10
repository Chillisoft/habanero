using System;
using System.Data;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

namespace Habanero.UI.Base
{
    public class GridBaseManager
    {
        private readonly IGridBase _gridBase;
        private BOCollectionDataSetProvider _dataSetProvider;
        private DataView _dataTableDefaultView;

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
                for (int i = 0; i < _gridBase.Rows.Count; i++)
                    _gridBase.Rows[i].Selected = false;
                if (value == null) return;
                int j = 0;
                foreach (DataRowView dataRowView in _dataTableDefaultView)
                {
                    if ((string)dataRowView.Row["ID"] == value.ID.ToString())
                    {
                        _gridBase.Rows[j].Selected = true;
                        break;
                    }
                    j++;
                }

            }
        }

        public void SetCollection(IBusinessObjectCollection col)
        {
            _dataSetProvider = new BOCollectionReadOnlyDataSetProvider(col);
            ClassDef classDef = col.ClassDef;
            UIDef uiDef = classDef.GetUIDef("default");
            UIGrid uiGrid = uiDef.UIGrid;
            DataTable dataTable = _dataSetProvider.GetDataTable(uiGrid);
            _dataTableDefaultView = dataTable.DefaultView;
            _gridBase.AllowUserToAddRows = false;
            _gridBase.DataSource = dataTable;

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
            int i = 0;
            foreach (DataRowView dataRowView in _dataTableDefaultView)
            {
                if (i++ == row)
                {
                    return this._dataSetProvider.Find((string)dataRowView.Row["ID"]);
                }
            }
            return null;
        }
    }
}