//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;
using Habanero.UI.Win;
using DataGridViewColumnSortMode=Habanero.UI.Base.DataGridViewColumnSortMode;

namespace Habanero.UI.Win
{
    public abstract class GridBaseWin : DataGridView, IGridBase
    {
        public event EventHandler<BOEventArgs> BusinessObjectSelected;
        public event EventHandler CollectionChanged;
        public event EventHandler FilterUpdated;

        private readonly GridBaseManager _manager;

        protected GridBaseWin()
        {
            _manager = new GridBaseManager(this);
            this.SelectionChanged += delegate { FireBusinessObjectSelected(); };
            _manager.CollectionChanged += delegate{ FireCollectionChanged(); };
        }
        /// <summary>
        /// Returns the grid base manager for this grid
        /// </summary>
        public GridBaseManager GridBaseManager
        {
            get { return _manager; }
        }

        /// <summary>
        /// Creates a dataset provider that is applicable to this grid. For example, a readonly grid would
        /// return a read only datasetprovider, while an editable grid would return an editable one.
        /// </summary>
        /// <param name="col">The collection to create the datasetprovider for</param>
        /// <returns></returns>
        public abstract IDataSetProvider CreateDataSetProvider(IBusinessObjectCollection col);

        /// <summary>
        /// gets and sets the currently selected cell for the datagridview.
        /// </summary>
        public new IDataGridViewCell CurrentCell
        {
            get
            {
                return base.CurrentCell == null ? null : new DataGridViewCellWin(base.CurrentCell);
            }
            set { base.CurrentCell = ((DataGridViewCellWin)value).DataGridViewCell; }
        }


        private void FireBusinessObjectSelected()
        {
            if (this.BusinessObjectSelected != null)
            {
                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
            }
        }

        public void SetBusinessObjectCollection(IBusinessObjectCollection col)
        {
            _manager.SetBusinessObjectCollection(col);
        }

        /// <summary>
        /// Returns the business object collection being displayed in the grid
        /// </summary>
        /// <returns>Returns a business collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection()
        {
            return _manager.GetBusinessObjectCollection();
        }

        /// <summary>
        /// Returns the business object at the row specified
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        public IBusinessObject GetBusinessObjectAtRow(int row)
        {
            return _manager.GetBusinessObjectAtRow(row);
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

        public void Clear()
        {
            _manager.Clear();
        }

        public new IDataGridViewSelectedRowCollection SelectedRows
        {
            get { return new DataGridViewSelectedRowCollectionWin(base.SelectedRows); }
        }

        public new IDataGridViewSelectedCellCollection SelectedCells
        {
            get { return new DataGridViewSelectedCellCollectionWin(base.SelectedCells); }
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

        public IBusinessObject SelectedBusinessObject
        {
            get { return _manager.SelectedBusinessObject; }
            set { _manager.SelectedBusinessObject = value; }
        }

        public IList<BusinessObject> SelectedBusinessObjects
        {
            get
            {
                //DataGridViewRow row = new DataGridViewRow();
                //row.DataBoundItem
                 return _manager.SelectedBusinessObjects;
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
        Base.DockStyle IControlChilli.Dock
        {
            get { return (Base.DockStyle)base.Dock; }
            set { base.Dock = (System.Windows.Forms.DockStyle)value; }
        }

        #region IGridBase Members

        int IGridBase.ItemsPerPage
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        int IGridBase.CurrentPage
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public GridLoaderDelegate GridLoader
        {
            get { return _manager.GridLoader; }
            set { _manager.GridLoader = value; }
        }

        public IDataSetProvider DataSetProvider
        {
            get { return _manager.DataSetProvider; }
        }

        public void SelectedBusinessObjectEdited(BusinessObject bo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles the event of the currently selected business object being edited.
        /// This is used only for internal testing
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectEdited;

        public void RefreshGrid()
        {
            _manager.RefreshGrid();
        }

        /// <summary>
        /// Not supported.  Does nothing.
        /// </summary>
        /// <param name="rowNum"></param>
        public void ChangeToPageOfRow(int rowNum)
        {
            
        }


        ///// <summary>
        ///// initiliase the grid with the 'default' UIdef.
        ///// </summary>
        //public void Initialise()
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        /// <summary>
        /// Sets the sort column and indicates whether
        /// it should be sorted in ascending or descending order
        /// </summary>
        /// <param name="columnName">The column number to set</param>
        /// object property</param>
        /// <param name="ascending">Whether sorting should be done in ascending
        /// order ("false" sets it to descending order)</param>
        public void Sort(string columnName, bool ascending)
        {
            _manager.SetSortColumn(columnName,  ascending);
        }

        /// <summary>
        /// Applies a filter clause to the data table and updates the filter.
        /// The filter allows you to determine which objects to display using
        /// some criteria.
        /// </summary>
        /// <param name="filterClause">The filter clause</param>
        public void ApplyFilter(IFilterClause filterClause)
        {
            _manager.ApplyFilter(filterClause);
            FireFilterUpdated();
        }

        /// <summary>
        /// Calls the FilterUpdated() method, passing this instance as the
        /// sender
        /// </summary>
        private void FireFilterUpdated()
        {
            if (this.FilterUpdated != null)
            {
                this.FilterUpdated(this, new EventArgs());
            }
        }

        //public void AddColumn(IDataGridViewColumn column)
        //{
        //    _manager.AddColumn(column);
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

            /// <summary>Adds a new row to the collection, and populates the cells with the specified objects.</summary>
            /// <returns>The index of the new row.</returns>
            /// <param name="values">A variable number of objects that populate the cells of the new <see cref="IDataGridViewRow"></see>.</param>
            /// <exception cref="T:System.InvalidOperationException">The associated <see cref="IDataGridView"></see> control is 
            /// performing one of the 
            /// following actions that temporarily prevents new rows from being added:Selecting all cells in the control. 
            /// Clearing the selection.-or-This method is being called from a handler for one of the following 
            /// <see cref="IDataGridView"></see> events:<see cref="IDataGridView.CellEnter"></see><see cref="IDataGridView.CellLeave"></see><see cref="IDataGridView.CellValidating">
            /// </see><see cref="IDataGridView.CellValidated"></see><see cref="IDataGridView.RowEnter"></see>
            /// <see cref="IDataGridView.RowLeave"></see><see cref="IDataGridView.RowValidated"></see><see cref="IDataGridView.RowValidating"></see>
            /// -or-The <see cref="IDataGridView.VirtualMode"></see> property of the <see cref="IDataGridView"></see> is set to true.- or 
            /// -The <see cref="IDataGridView.DataSource"></see> property of the <see cref="IDataGridView"></see> is not null.
            /// -or-The <see cref="IDataGridView"></see> has no columns. -or-The row returned by the <see cref="IDataGridView.RowTemplate"></see>
            ///  property has more cells than there are columns in the control.-or-This operation would add a frozen row after unfrozen rows.</exception>
            /// <exception cref="T:System.ArgumentNullException">values is null.</exception>
            /// <filterpriority>1</filterpriority>
            public int Add(params object[] values)
            {
                return _rows.Add(values);
            }

            /// <summary>Clears the collection. </summary>
            /// <exception cref="T:System.InvalidOperationException">The collection is data bound and the underlying data source does not support clearing the row data.-or-The associated <see cref="IDataGridView"></see> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="IDataGridView"></see> events:<see cref="IDataGridView.CellEnter"></see><see cref="IDataGridView.CellLeave"></see><see cref="IDataGridView.CellValidating"></see><see cref="IDataGridView.CellValidated"></see><see cref="IDataGridView.RowEnter"></see><see cref="IDataGridView.RowLeave"></see><see cref="IDataGridView.RowValidated"></see><see cref="IDataGridView.RowValidating"></see></exception>
            /// <filterpriority>1</filterpriority>
            public void Clear()
            {
                _rows.Clear();
            }

            /// <summary>Removes the row at the specified position from the collection.</summary>
            /// <param name="index">The position of the row to remove.</param>
            /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero and greater than the number of rows in the collection minus one. </exception>
            /// <exception cref="T:System.InvalidOperationException">The associated <see cref="IDataGridView"></see> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="IDataGridView"></see> events:<see cref="IDataGridView.CellEnter"></see><see cref="IDataGridView.CellLeave"></see><see cref="IDataGridView.CellValidating"></see><see cref="IDataGridView.CellValidated"></see><see cref="IDataGridView.RowEnter"></see><see cref="IDataGridView.RowLeave"></see><see cref="IDataGridView.RowValidated"></see><see cref="IDataGridView.RowValidating"></see>-or-index is equal to the number of rows in the collection and the <see cref="IDataGridView.AllowUserToAddRows"></see> property of the <see cref="IDataGridView"></see> is set to true.-or-The associated <see cref="IDataGridView"></see> control is bound to an <see cref="T:System.ComponentModel.IBindingList"></see> implementation with <see cref="P:System.ComponentModel.IBindingList.AllowRemove"></see> and <see cref="P:System.ComponentModel.IBindingList.SupportsChangeNotification"></see> property values that are not both true.</exception>
            /// <filterpriority>1</filterpriority>
            public void RemoveAt(int index)
            {
                _rows.RemoveAt(index);
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
                foreach (DataGridViewRow row in _rows)
                {
                    yield return new DataGridViewRowWin(row);
                }
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

            public void Add(IDataGridViewColumn dataGridViewColumn)
            {
                DataGridViewColumnWin col = (DataGridViewColumnWin)dataGridViewColumn;
                _columns.Add(col.DataGridViewColumn);
            }

            public int Add(string columnName, string headerText)
            {
                int addedColumn = _columns.Add(columnName, headerText);
                _columns[addedColumn].DataPropertyName = columnName;
                return addedColumn;
            }

            public IDataGridViewColumn this[int index]
            {
                get
                {
                    DataGridViewColumn column = _columns[index];
                    if (column == null) return null;
                    return new DataGridViewColumnWin(column);
                }
            }

            public IDataGridViewColumn this[string name]
            {
                get
                {
                    DataGridViewColumn column = _columns[name];
                    if (column == null) return null;
                    return new DataGridViewColumnWin(column);
                }
            }

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

            public int Index
            {
                get { return _dataGridViewRow.Index; }
            }

            public bool SetValues(params object[] values)
            {
                return this._dataGridViewRow.SetValues(values);
            }

            /// <summary>Gets the collection of cells that populate the row.</summary>
            /// <returns>A <see cref="IDataGridViewCellCollection"></see> that contains all of the cells in the row.</returns>
            /// <filterpriority>1</filterpriority>
            public IDataGridViewCellCollection Cells
            {
                get { return new DataGridViewCellCollectionWin(_dataGridViewRow.Cells); }
            }

            public object DataBoundItem
            {
                get { return _dataGridViewRow.DataBoundItem; }
            }
        }


        private class DataGridViewCellCollectionWin : IDataGridViewCellCollection
        {
            private readonly DataGridViewCellCollection _cells;

            public DataGridViewCellCollectionWin(DataGridViewCellCollection cells)
            {
                _cells = cells;
            }

            public int Count
            {
                get { return _cells.Count; }
            }

            /// <summary>Adds a cell to the collection.</summary>
            /// <returns>The position in which to insert the new element.</returns>
            /// <param name="dataGridViewCell">A <see cref="IDataGridViewCell"></see> to add to the collection.</param>
            /// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="IDataGridViewCellCollection"></see> already belongs to a <see cref="System.Windows.DataGridView"></see> control.-or-dataGridViewCell already belongs to a <see cref="System.Windows.DataGridViewRow"></see>.</exception>
            /// <filterpriority>1</filterpriority>
            public int Add(IDataGridViewCell dataGridViewCell)
            {
                return _cells.Add((DataGridViewCell) dataGridViewCell);
            }

            /// <summary>Clears all cells from the collection.</summary>
            /// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="IDataGridViewCellCollection"></see> already belongs to a <see cref="System.Windows.DataGridView"></see> control.</exception>
            /// <filterpriority>1</filterpriority>
            public void Clear()
            {
                _cells.Clear();
            }

            /// <summary>Determines whether the specified cell is contained in the collection.</summary>
            /// <returns>true if dataGridViewCell is in the collection; otherwise, false.</returns>
            /// <param name="dataGridViewCell">A <see cref="IDataGridViewCell"></see> to locate in the collection.</param>
            /// <filterpriority>1</filterpriority>
            public bool Contains(IDataGridViewCell dataGridViewCell)
            {
                return _cells.Contains((DataGridViewCell) dataGridViewCell);
            }

            ///// <summary>Copies the entire collection of cells into an array at a specified location within the array.</summary>
            ///// <param name="array">The destination array to which the contents will be copied.</param>
            ///// <param name="index">The index of the element in array at which to start copying.</param>
            ///// <filterpriority>1</filterpriority>
            //public void CopyTo(IDataGridViewCell[] array, int index)
            //{
            //    throw new NotImplementedException();
            //}

            /// <summary>Returns the index of the specified cell.</summary>
            /// <returns>The zero-based index of the value of dataGridViewCell parameter, if it is found in the collection; otherwise, -1.</returns>
            /// <param name="dataGridViewCell">The cell to locate in the collection.</param>
            /// <filterpriority>1</filterpriority>
            public int IndexOf(IDataGridViewCell dataGridViewCell)
            {
                return _cells.IndexOf((DataGridViewCell) dataGridViewCell);
            }

            ///// <summary>Inserts a cell into the collection at the specified index. </summary>
            ///// <param name="dataGridViewCell">The <see cref="IDataGridViewCell"></see> to insert.</param>
            ///// <param name="index">The zero-based index at which to place dataGridViewCell.</param>
            ///// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="IDataGridViewCellCollection"></see> already belongs to a <see cref="System.Windows.DataGridView"></see> control.-or-dataGridViewCell already belongs to a <see cref="System.Windows.DataGridViewRow"></see>.</exception>
            ///// <filterpriority>1</filterpriority>
            //public void Insert(int index, IDataGridViewCell dataGridViewCell)
            //{
            //    throw new NotImplementedException();
            //}

            ///// <summary>Removes the specified cell from the collection.</summary>
            ///// <param name="cell">The <see cref="IDataGridViewCell"></see> to remove from the collection.</param>
            ///// <exception cref="T:System.ArgumentException">cell could not be found in the collection.</exception>
            ///// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="IDataGridViewCellCollection"></see> already belongs to a <see cref="System.Windows.DataGridView"></see> control.</exception>
            ///// <filterpriority>1</filterpriority>
            //public void Remove(IDataGridViewCell cell)
            //{
            //    throw new NotImplementedException();
            //}

            ///// <summary>Removes the cell at the specified index.</summary>
            ///// <param name="index">The zero-based index of the <see cref="IDataGridViewCell"></see> to be removed.</param>
            ///// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="IDataGridViewCellCollection"></see> already belongs to a <see cref="System.Windows.DataGridView"></see> control.</exception>
            ///// <filterpriority>1</filterpriority>
            //public void RemoveAt(int index)
            //{
            //    throw new NotImplementedException();
            //}

            public IDataGridViewCell this[int index]
            {
                get { return new DataGridViewCellWin(_cells[index]); }
            }

            /// <summary>Gets or sets the cell in the column with the provided name. In C#, this property is the indexer for the <see cref="IDataGridViewCellCollection"></see> class.</summary>
            /// <returns>The <see cref="IDataGridViewCell"></see> stored in the column with the given name.</returns>
            /// <param name="columnName">The name of the column in which to get or set the cell.</param>
            /// <exception cref="T:System.InvalidOperationException">The specified cell when setting this property already belongs to a <see cref="System.Windows.DataGridView"></see> control.-or-The specified cell when setting this property already belongs to a <see cref="System.Windows.DataGridViewRow"></see>.</exception>
            /// <exception cref="T:System.ArgumentException">columnName does not match the name of any columns in the control.</exception>
            /// <exception cref="T:System.ArgumentNullException">The specified value when setting this property is null.</exception>
            /// <filterpriority>1</filterpriority>
            public IDataGridViewCell this[string columnName]
            {
                get { return new DataGridViewCellWin(_cells[columnName]); }
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
                foreach (DataGridViewRow row in _cells)
                {
                    yield return new DataGridViewRowWin(row);
                }
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
                get {return _selectedRows.Count; }
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
                    yield return new DataGridViewRowWin(row);
                }
            }
        }

        private class DataGridViewSelectedCellCollectionWin : IDataGridViewSelectedCellCollection
        {
            private readonly DataGridViewSelectedCellCollection _selectedCells;

            public DataGridViewSelectedCellCollectionWin(DataGridViewSelectedCellCollection selectedCells)
            {
                _selectedCells = selectedCells;
            }

            public int Count
            {
                get { return _selectedCells.Count; }
            }

            public IDataGridViewCell this[int index]
            {
                get { return new DataGridViewCellWin(_selectedCells[index]); }
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
                foreach (DataGridViewCell cell in _selectedCells)
                {
                    yield return new DataGridViewCellWin(cell);
                }
            }
        }
    }

    internal class DataGridViewCellStyleWin : IDataGridViewCellStyle
    {
        private readonly DataGridViewCellStyle _dataGridViewCellStyle;

        public DataGridViewCellStyleWin(DataGridViewCellStyle dataGridViewCellStyle)
        {
            _dataGridViewCellStyle = dataGridViewCellStyle;
        }

        public DataGridViewCellStyle DataGridViewCellStyle
        {
            get { return _dataGridViewCellStyle; }
        }

        /// <summary>Applies the specified <see cref="IDataGridViewCellStyle"></see> to the current <see cref="IDataGridViewCellStyle"></see>.</summary>
        /// <param name="dataGridViewCellStyle">The <see cref="IDataGridViewCellStyle"></see> to apply to the current <see cref="IDataGridViewCellStyle"></see>.</param>
        /// <exception cref="T:System.ArgumentNullException">dataGridViewCellStyle is null.</exception>
        /// <filterpriority>1</filterpriority>
        public void ApplyStyle(IDataGridViewCellStyle dataGridViewCellStyle)
        {
            _dataGridViewCellStyle.ApplyStyle(((DataGridViewCellStyleWin)dataGridViewCellStyle).DataGridViewCellStyle);
        }

        /// <summary>Creates an exact copy of this <see cref="IDataGridViewCellStyle"></see>.</summary>
        /// <returns>A <see cref="IDataGridViewCellStyle"></see> that represents an exact copy of this cell style.</returns>
        public IDataGridViewCellStyle Clone()
        {
            return new DataGridViewCellStyleWin(_dataGridViewCellStyle.Clone());
        }

        /// <summary>Gets or sets the format string applied to the textual content of a <see cref="IDataGridView"></see> cell.</summary>
        /// <returns>A string that indicates the format of the cell value. The default is <see cref="F:System.String.Empty"></see>.</returns>
        /// <filterpriority>1</filterpriority>
        public string Format
        {
            get { return _dataGridViewCellStyle.Format;  }
            set { _dataGridViewCellStyle.Format = value; }
        }
    }

    internal class DataGridViewCellWin : IDataGridViewCell
    {
        private readonly DataGridViewCell _cell;

        public DataGridViewCellWin(DataGridViewCell cell)
        {
            _cell = cell;
        }

        public DataGridViewCell DataGridViewCell
        {
            get { return _cell; }
        }

        /// <summary>Gets the column index for this cell. </summary>
        /// <returns>The index of the column that contains the cell; -1 if the cell is not contained within a column.</returns>
        /// <filterpriority>1</filterpriority>
        public int ColumnIndex
        {
            get { return _cell.ColumnIndex; }
        }

        /// <summary>Gets a value that indicates whether the cell is currently displayed on-screen. </summary>
        /// <returns>true if the cell is on-screen or partially on-screen; otherwise, false.</returns>
        public bool Displayed
        {
            get { return _cell.Displayed; }
        }

        /// <summary>Gets a value indicating whether the cell is frozen. </summary>
        /// <returns>true if the cell is frozen; otherwise, false.</returns>
        /// <filterpriority>1</filterpriority>
        public bool Frozen
        {
            get { return _cell.Frozen; }
        }

        /// <summary>Gets the value of the cell as formatted for display.</summary>
        /// <returns>The formatted value of the cell or null if the cell does not belong to a <see cref="IDataGridView"></see> control.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The row containing the cell is a shared row.-or-The cell is a column header cell.</exception>
        /// <exception cref="T:System.Exception">Formatting failed and either there is no handler for the <see cref="IDataGridView.DataError"></see> event of the <see cref="T:Gizmox.WebGUI.Forms.DataGridView"></see> control or the handler set the <see cref="P:Gizmox.WebGUI.Forms.DataGridViewDataErrorEventArgs.ThrowException"></see> property to true. The exception object can typically be cast to type <see cref="T:System.FormatException"></see>.</exception>
        /// <exception cref="T:System.InvalidOperationException"><see cref="IDataGridViewCell.ColumnIndex"></see> is less than 0, indicating that the cell is a row header cell.</exception>
        /// <filterpriority>1</filterpriority>
        public object FormattedValue
        {
            get { return _cell.FormattedValue; }
        }

        /// <summary>Gets a value indicating whether this cell is currently being edited.</summary>
        /// <returns>true if the cell is in edit mode; otherwise, false.</returns>
        /// <exception cref="T:System.InvalidOperationException">The row containing the cell is a shared row.</exception>
        public bool IsInEditMode
        {
            get { return _cell.IsInEditMode; }
        }

        /// <summary>Gets or sets a value indicating whether the cell's data can be edited. </summary>
        /// <returns>true if the cell's data can be edited; otherwise, false.</returns>
        /// <exception cref="T:System.InvalidOperationException">There is no owning row when setting this property. -or-The owning row is shared when setting this property.</exception>
        /// <filterpriority>1</filterpriority>
        public bool ReadOnly
        {
            get { return _cell.ReadOnly; }
            set { _cell.ReadOnly = value; }
        }

        /// <summary>Gets the index of the cell's parent row. </summary>
        /// <returns>The index of the row that contains the cell; -1 if there is no owning row.</returns>
        /// <filterpriority>1</filterpriority>
        public int RowIndex
        {
            get { return _cell.RowIndex; }
        }

        /// <summary>Gets or sets a value indicating whether the cell has been selected. </summary>
        /// <returns>true if the cell has been selected; otherwise, false.</returns>
        /// <exception cref="T:System.InvalidOperationException">There is no associated <see cref="IDataGridView"></see> when setting this property. -or-The owning row is shared when setting this property.</exception>
        /// <filterpriority>1</filterpriority>
        public bool Selected
        {
            get { return _cell.Selected; }
            set { _cell.Selected = value; }
        }

        /// <summary>Gets or sets the value associated with this cell. </summary>
        /// <returns>Gets or sets the data to be displayed by the cell. The default is null.</returns>
        /// <exception cref="T:System.InvalidOperationException"><see cref="System.Windows.Forms.DataGridViewCell.ColumnIndex"></see> is less than 0, indicating that the cell is a row header cell.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><see cref="System.Windows.Forms.DataGridViewCell.RowIndex"></see> is outside the valid range of 0 to the number of rows in the control minus 1.</exception>
        /// <filterpriority>1</filterpriority>
        public object Value
        {
            get { return _cell.Value; }
            set { _cell.Value = value; }
        }

        /// <summary>Gets or sets the data type of the values in the cell. </summary>
        /// <returns>A <see cref="T:System.Type"></see> representing the data type of the value in the cell.</returns>
        /// <filterpriority>1</filterpriority>
        public Type ValueType
        {
            get { return _cell.ValueType; }
            set { _cell.ValueType = value; }
        }

        /// <summary>Gets a value indicating whether the cell is in a row or column that has been hidden. </summary>
        /// <returns>true if the cell is visible; otherwise, false.</returns>
        /// <filterpriority>1</filterpriority>
        public bool Visible
        {
            get { return _cell.Visible; }
        }

        /// <summary>Gets the type of the cell's hosted editing control. </summary>
        /// <returns>A <see cref="T:System.Type"></see> representing the 
        /// DataGridViewTextBoxEditingControl type.</returns>
        /// <filterpriority>1</filterpriority>
        public Type EditType
        {
            get { return _cell.EditType; }
        }

        /// <summary>Gets the default value for a cell in the row for new records.</summary>
        /// <returns>An <see cref="T:System.Object"></see> representing the default value.</returns>
        /// <filterpriority>1</filterpriority>
        public object DefaultNewRowValue
        {
            get { return _cell.DefaultNewRowValue; }
        }
    }
    
}