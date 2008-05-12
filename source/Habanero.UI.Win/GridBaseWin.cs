using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class GridBaseWin : DataGridView, IGridBase
    {
        public event EventHandler<BOEventArgs> BusinessObjectSelected;

        public void Clear()
        {
            _mngr.Clear();
        }

        private readonly GridBaseManager _mngr;

        public GridBaseWin()
        {
            _mngr = new GridBaseManager(this);
            this.SelectionChanged += delegate { FireBusinessObjectSelected(); };
        }

        private void FireBusinessObjectSelected()
        {
            if (this.BusinessObjectSelected != null)
            {
                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
            }
        }

        public void SetCollection(IBusinessObjectCollection col)
        {
            _mngr.SetCollection(col);
        }

        public new IDataGridViewRowCollection Rows
        {
            get
            {
                
                return new DataGridViewRowCollectionWin(base.Rows);
            }
        }

        public new IDataGridViewSelectedRowCollection SelectedRows
        {
            get { return new DataGridViewSelectedRowCollectionWin(base.SelectedRows); }
        }

        public new IDataGridViewColumnCollection Columns
        {
            get
            {
                return new DataGridViewColumnCollectionWin(base.Columns);
            }
        }

        public BusinessObject SelectedBusinessObject
        {
            get { return _mngr.SelectedBusinessObject; }
            set { _mngr.SelectedBusinessObject = value; }
        }

        public IList<BusinessObject> SelectedBusinessObjects
        {
            get
            {
                //DataGridViewRow row = new DataGridViewRow();
                //row.DataBoundItem
                 return _mngr.SelectedBusinessObjects;
            }
        }

        //IList IGridBase.SelectedRows
        //{
        //    get { return base.SelectedRows; }
        //}

        IList IChilliControl.Controls
        {
            get { return this.Controls; }
        }

        private class DataGridViewRowCollectionWin : IDataGridViewRowCollection
        {
            private readonly DataGridViewRowCollection _rows;

            public DataGridViewRowCollectionWin(DataGridViewRowCollection rows)
            {
                if (rows == null) throw new ArgumentNullException("rows");
                _rows = rows;
            }

            public int Count
            {
                get { return _rows.Count; }
            }

            public IDataGridViewRow this[int index]
            {

                get { return new DataGridViewRowWin(_rows[index]); }
            }
        }

        private class DataGridViewColumnCollectionWin : IDataGridViewColumnCollection
        {
            private readonly DataGridViewColumnCollection _columns;

            public DataGridViewColumnCollectionWin(DataGridViewColumnCollection columns)
            {
                if (columns == null) throw new ArgumentNullException("columns");
                _columns = columns;
            }

            public int Count
            {
                get { return _columns.Count; }
            }
        }


        private class DataGridViewRowWin : IDataGridViewRow
        {
            private readonly DataGridViewRow _dataGridViewRow;

            public DataGridViewRowWin(DataGridViewRow dataGridViewRow)
            {
                _dataGridViewRow = dataGridViewRow;
            }

            public bool Selected
            {
                get { return _dataGridViewRow.Selected; }
                set { _dataGridViewRow.Selected = value; }
            }
            public object DataBoundItem
            {
                get { return _dataGridViewRow.DataBoundItem; }
            }
        }

        private class DataGridViewSelectedRowCollectionWin : IDataGridViewSelectedRowCollection
        {
            private readonly DataGridViewSelectedRowCollection _selectedRows;

            public DataGridViewSelectedRowCollectionWin(DataGridViewSelectedRowCollection selectedRows)
            {
                _selectedRows = selectedRows;
            }
            public int Count
            {
                get { return _selectedRows.Count; }
            }

            public IDataGridViewRow this[int index]
            {
                get { return new DataGridViewRowWin(_selectedRows[index]); }
            }
            ///<summary>
            ///Returns an enumerator that iterates through a collection.
            ///</summary>
            ///
            ///<returns>
            ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
            ///</returns>
            ///<filterpriority>2</filterpriority>
            public IEnumerator GetEnumerator()
            {
                foreach (DataGridViewRow row in _selectedRows)
                {
                    yield return new DataGridViewRowWin( row);
                }
            }
        }
    }

}