using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public abstract class GridBaseWin : DataGridView, IGridBase
    {
        public event EventHandler<BOEventArgs> BusinessObjectSelected;
        public event EventHandler CollectionChanged;

        public void Clear()
        {
            _mngr.Clear();
        }

        private readonly GridBaseManager _mngr;

        public GridBaseWin()
        {
            _mngr = new GridBaseManager(this);
            this.SelectionChanged += delegate { FireBusinessObjectSelected(); };
            _mngr.CollectionChanged += delegate{ FireCollectionChanged(); };
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

        /// <summary>
        /// Returns the business object collection being displayed in the grid
        /// </summary>
        /// <returns>Returns a business collection</returns>
        public IBusinessObjectCollection GetCollection()
        {
            return _mngr.GetCollection();
        }

        /// <summary>
        /// Returns the business object at the row specified
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        public BusinessObject GetBusinessObjectAtRow(int row)
        {
            return _mngr.GetBusinessObjectAtRow(row);
        }

        private void FireCollectionChanged()
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, EventArgs.Empty);
            }
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

        public new IDataGridViewRow CurrentRow
        {
            get
            {
                if (base.CurrentRow == null) return null;
                return new DataGridViewRowWin(base.CurrentRow);
            }
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

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        /// <summary>
        /// Sets the sort column and indicates whether
        /// it should be sorted in ascending or descending order
        /// </summary>
        /// <param name="columnName">The column number to set</param>
        /// <param name="isBoProperty">Whether the property is a business
        /// object property</param>
        /// <param name="ascending">Whether sorting should be done in ascending
        /// order ("false" sets it to descending order)</param>
        public void SetSortColumn(string columnName, bool isBoProperty, bool ascending)
        {
            _mngr.SetSortColumn(columnName, isBoProperty, ascending);
        }

        //public void AddColumn(IDataGridViewColumn column)
        //{
        //    _mngr.AddColumn(column);
        //}

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

        internal class DataGridViewColumnWin : IDataGridViewColumn
        {
            private readonly DataGridViewColumn _dataGridViewColumn;

            public DataGridViewColumnWin(DataGridViewColumn dataGridViewColumn)
            {
                _dataGridViewColumn = dataGridViewColumn;
            }

            public DataGridViewColumn DataGridViewColumn
            {
                get { return _dataGridViewColumn; }
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

            #region IDataGridViewColumnCollection Members

            public int Count
            {
                get { return _columns.Count; }
            }

            public void Clear()
            {
                _columns.Clear();
            }
            public int Add(string columnName, string headerText)
            {
                return _columns.Add(columnName, headerText);
            }
            //public void Add(IDataGridViewColumn dataGridViewColumn)
            //{
            //    DataGridViewColumnWin dataGridViewColumnWin = dataGridViewColumn as DataGridViewColumnWin;
            //    _columns.Add(dataGridViewColumnWin.DataGridViewColumn);
            //}

            #endregion

            #region IEnumerable<IDataGridViewColumn> Members

            ///<summary>
            ///Returns an enumerator that iterates through the collection.
            ///</summary>
            ///
            ///<returns>
            ///A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
            ///</returns>
            ///<filterpriority>1</filterpriority>
            IEnumerator<IDataGridViewColumn> IEnumerable<IDataGridViewColumn>.GetEnumerator()
            {
                foreach (DataGridViewColumn column in _columns)
                {
                    yield return new DataGridViewColumnWin(column);
                }
            }

            #endregion

            #region IEnumerable Members

            ///<summary>
            ///Returns an enumerator that iterates through a collection.
            ///</summary>
            ///
            ///<returns>
            ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
            ///</returns>
            ///<filterpriority>2</filterpriority>
            IEnumerator IEnumerable.GetEnumerator()
            {
                foreach (DataGridViewColumn column in _columns)
                {
                    yield return new DataGridViewColumnWin(column);
                }
            }

            #endregion
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