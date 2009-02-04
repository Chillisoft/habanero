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
using System.ComponentModel;
using System.Text;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;
using IEnumerable=System.Collections.IEnumerable;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Displays data in a customizable grid
    /// </summary>
    public class DataGridViewVWG : DataGridView, IDataGridView
    {
        private readonly DataGridViewManager _manager;

        public DataGridViewVWG()
        {
            _manager = new DataGridViewManager(this);
        }

        /// <summary>
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        Base.AnchorStyles IControlHabanero.Anchor
        {
            get { return (Base.AnchorStyles)base.Anchor; }
            set { base.Anchor = (Gizmox.WebGUI.Forms.AnchorStyles)value; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionVWG(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlHabanero.Dock
        {
            get { return DockStyleVWG.GetDockStyle(base.Dock); }
            set { base.Dock = DockStyleVWG.GetDockStyle(value); }
        }

        /// <summary>
        /// Gets the collection of rows in the grid
        /// </summary>
        public new IDataGridViewRowCollection Rows
        {
            get { return new DataGridViewRowCollectionVWG(base.Rows); }
        }

        /// <summary>
        /// Gets a collection of columns set up for the grid
        /// </summary>
        public new IDataGridViewColumnCollection Columns
        {
            get { return new DataGridViewColumnCollectionVWG(base.Columns); }
        }

        /// <summary>Sorts the contents of the <see cref="IDataGridView"></see> control in ascending or
        /// descending order based on the contents of the specified column.</summary>
        /// <param name="direction">One of the <see cref="T:System.ComponentModel.ListSortDirection"></see> values. </param>
        /// <param name="dataGridViewColumn">The column by which to sort the contents of the <see cref="IDataGridView"></see>. </param>
        /// <exception cref="T:System.ArgumentException">The specified column is not part of this <see cref="IDataGridView"></see>
        /// .-or-The DataSource property has been set and the DataGridViewColumn.IsDataBound property of the specified
        /// column returns false.</exception>
        /// <exception cref="T:System.ArgumentNullException">dataGridViewColumn is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">The VirtualMode property is set to true
        /// and the DataGridViewColumn.IsDataBound property of the specified column returns false.-or-
        /// The object specified by the DataSource property does not implement the
        /// <see cref="T:System.ComponentModel.IBindingList"></see> interface.-or-
        /// The object specified by the DataSource property has a
        /// <see cref="P:System.ComponentModel.IBindingList.SupportsSorting"></see> property value of false.</exception>
        /// <filterpriority>1</filterpriority>
        public void Sort(IDataGridViewColumn dataGridViewColumn, ListSortDirection direction)
        {
            //TODO
            throw new System.NotImplementedException();
        }

        /// <summary>Gets the column by which the <see cref="IDataGridView"></see> contents are currently sorted.</summary>
        /// <returns>The <see cref="IDataGridViewColumn"></see> by which the <see cref="IDataGridView"></see> 
        /// contents are currently sorted.</returns>
        /// <filterpriority>1</filterpriority>
        public IDataGridViewColumn SortedColumn
        {
            //TODO
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Sets the sort column and indicates whether
        /// it should be sorted in ascending or descending order
        /// </summary>
        /// <param name="columnName">The column number to sort on</param>
        /// object property</param>
        /// <param name="ascending">True for ascending order, false for descending order</param>
        public void Sort(string columnName, bool ascending)
        {
            _manager.SetSortColumn(columnName, ascending);
        }

        /// <summary>
        /// Gets the collection of currently selected rows
        /// </summary>
        public new IDataGridViewSelectedRowCollection SelectedRows
        {
            get { return new DataGridViewSelectedRowCollectionVWG(base.SelectedRows); }
        }

        /// <summary>
        /// Gets the collection of currently selected cells
        /// </summary>
        public new IDataGridViewSelectedCellCollection SelectedCells
        {
            get { return new DataGridViewSelectedCellCollectionVWG(base.SelectedCells); }
        }

        /// <summary>Gets or sets a value indicating how the cells of the DataGridView can be selected.</summary>
        /// <returns>One of the DataGridViewSelectionMode values. The default is DataGridViewSelectionMode.RowHeaderSelect.</returns>
        /// <exception cref="T:System.InvalidOperationException">The specified value when setting this property is DataGridViewSelectionMode.FullColumnSelect or DataGridViewSelectionMode.ColumnHeaderSelect and the DataGridViewColumn.SortMode property of one or more columns is set to DataGridViewColumnSortMode.Automatic.</exception>
        /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is not a valid DataGridViewSelectionMode value.</exception>
        Habanero.UI.Base.DataGridViewSelectionMode IDataGridView.SelectionMode
        {
            get { return (Habanero.UI.Base.DataGridViewSelectionMode)base.SelectionMode; }
            set { base.SelectionMode = (Gizmox.WebGUI.Forms.DataGridViewSelectionMode)value; }
        }

        /// <summary>
        /// Provides an indexer to get or set the cell located at the intersection of the column and row with the specified indexes.
        /// </summary>
        /// <param name="columnIndex">The index of the column containing the cell.</param>
        /// <param name="rowIndex">The index of the row containing the cell</param>
        /// <returns>The DataGridViewCell at the specified location</returns>
        public new IDataGridViewCell this[int columnIndex, int rowIndex]
        {
            get { return new DataGridViewCellVWG(base[columnIndex, rowIndex]); }
            set { base[columnIndex, rowIndex] = value == null ? null : ((DataGridViewCellVWG)value).DataGridViewCell; }
        }

        /// <summary>
        /// Gets the currently selected row
        /// </summary>
        public new IDataGridViewRow CurrentRow
        {
            get
            {
                if (base.CurrentRow == null) return null;
                return new DataGridViewRowVWG(base.CurrentRow);
            }
        }

        /// <summary>
        /// Gets or sets the currently selected cell
        /// </summary>
        public new IDataGridViewCell CurrentCell
        {
            get { return base.CurrentCell == null ? null : new DataGridViewCellVWG(base.CurrentCell); }
            set { base.CurrentCell = value == null ? null : ((DataGridViewCellVWG)value).DataGridViewCell; }
        }

        /// <summary>
        /// When pagination is used, changes the current page to the one containing
        /// the given row number
        /// </summary>
        /// <param name="rowNum">The row that you wish to show the page of.  For example, if your grid has
        /// 30 rows and is set to 20 rows per page, calling ChangeToPageOfRow with an argument
        /// of 25 will set the page to page 2 since row 25 is on page 2.</param>
        public void ChangeToPageOfRow(int rowNum)
        {
            if (ItemsPerPage > 0)
                this.CurrentPage = (rowNum / this.ItemsPerPage) + 1;
            else this.CurrentPage = 1;
        }







        /// <summary>
        /// A collection of DataGridViewRow objects
        /// </summary>
        private class DataGridViewRowCollectionVWG : IDataGridViewRowCollection
        {
            private readonly DataGridViewRowCollection _rows;

            public DataGridViewRowCollectionVWG(DataGridViewRowCollection rows)
            {
                if (rows == null) throw new ArgumentNullException("rows");
                _rows = rows;
            }

            /// <summary>
            /// Gets the number of rows in the collection
            /// </summary>
            public int Count
            {
                get { return _rows.Count; }
            }

            /// <summary>Gets the <see cref="IDataGridViewRow"></see> at the specified index.</summary>
            /// <returns>The <see cref="IDataGridViewRow"></see> at the specified index. Accessing
            ///  a <see cref="IDataGridViewRow"></see> with this indexer causes the row to become unshared. 
            /// To keep the row shared, use the SharedRow method. 
            /// For more information, see Best Practices for Scaling the Windows Forms DataGridView Control.</returns>
            /// <param name="index">The zero-based index of the <see cref="IDataGridViewRow"></see> to get.</param>
            /// <filterpriority>1</filterpriority>
            public IDataGridViewRow this[int index]
            {
                get { return new DataGridViewRowVWG(_rows[index]); }
            }

            /// <summary>Adds a new row to the collection, and populates the cells with the specified objects.</summary>
            /// <returns>The index of the new row.</returns>
            /// <param name="values">A variable number of objects that populate the cells of the
            ///  new <see cref="IDataGridViewRow"></see>.</param>
            /// <exception cref="T:System.InvalidOperationException">The associated <see cref="IDataGridView"></see> 
            /// control is performing one of the following actions that temporarily prevents new rows from 
            /// being added:Selecting all cells in the control.Clearing the selection.-or-This method is 
            /// being called from a handler for one of the following <see cref="IDataGridView"></see>
            ///  events: CellEnter, CellLeave, CellValidating, CellValidated, RowEnter, RowLeave, RowValidated,
            /// RowValidating, -or-The VirtualMode property of the <see cref="IDataGridView"></see> is set to
            ///  true.- or -The <see cref="IDataGridView.DataSource"></see> property of the
            ///  <see cref="IDataGridView"></see> is not null.-or-The <see cref="IDataGridView"></see> has no
            ///  columns. -or-The row returned by the RowTemplate property has more cells than there are columns 
            /// in the control.-or-This operation would add a frozen row after unfrozen rows.</exception>
            /// <exception cref="T:System.ArgumentNullException">values is null.</exception>
            /// <filterpriority>1</filterpriority>
            public int Add(params object[] values)
            {
                return _rows.Add(values);
            }

            /// <summary>Clears the rows collection. </summary>
            /// <exception cref="T:System.InvalidOperationException">Thrown if 
            /// The rows collection is data bound and the 
            /// underlying data source does not support clearing the row data.-or-
            /// The associated <see cref="IDataGridView"></see> control is performing one of the 
            /// following actions that temporarily prevents new rows from being added:Selecting all 
            /// cells in the control.Clearing the selection.-or-This method is being called from a 
            /// handler for one of the following <see cref="IDataGridView">
            /// </see> events:CellEnter, CellLeave, CellValidating, CellValidated, RowEnter
            /// RowLeave, RowValidated, RowValidating</exception>
            /// <filterpriority>1</filterpriority>
            public void Clear()
            {
                _rows.Clear();
            }

            /// <summary>Removes the row from the collection.</summary>
            /// <param name="dataGridViewRow">The row to remove from the <see cref="IDataGridViewRowCollection"></see>.</param>
            /// <exception cref="T:System.InvalidOperationException">The associated <see cref="IDataGridView"></see> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="IDataGridView"></see> events:<see cref="IDataGridView.CellEnter"></see><see cref="IDataGridView.CellLeave"></see><see cref="IDataGridView.CellValidating"></see><see cref="IDataGridView.CellValidated"></see><see cref="IDataGridView.RowEnter"></see><see cref="IDataGridView.RowLeave"></see><see cref="IDataGridView.RowValidated"></see><see cref="IDataGridView.RowValidating"></see>-or-dataGridViewRow is the row for new records.-or-The associated <see cref="IDataGridView"></see> control is bound to an <see cref="T:System.ComponentModel.IBindingList"></see> implementation with <see cref="P:System.ComponentModel.IBindingList.AllowRemove"></see> and <see cref="P:System.ComponentModel.IBindingList.SupportsChangeNotification"></see> property values that are not both true. </exception>
            /// <exception cref="T:System.ArgumentException">dataGridViewRow is not contained in this collection.-or-dataGridViewRow is a shared row.</exception>
            /// <exception cref="T:System.ArgumentNullException">dataGridViewRow is null.</exception>
            /// <filterpriority>1</filterpriority>
            public void Remove(IDataGridViewRow dataGridViewRow)
            {
                if (dataGridViewRow == null) throw new ArgumentNullException("dataGridViewRow");
                _rows.RemoveAt(dataGridViewRow.Index);
            }

            /// <summary>Removes the row at the specified position from the collection.</summary>
            /// <param name="index">The position of the row to remove.</param>
            /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero and greater 
            /// than the number of rows in the collection minus one. </exception>
            /// <exception cref="T:System.InvalidOperationException">The associated <see cref="IDataGridView"></see> 
            /// control is performing one of the following actions that temporarily prevents new rows from 
            /// being added:Selecting all cells in the control.Clearing the selection.-or-
            /// This method is being called from a handler for one of the following <see cref="IDataGridView"></see> 
            /// events:CellEnter, CellLeave, CellValidating, CellValidated, RowEnter, RowLeave, RowValidated, RowValidating
            /// -or-index is equal to the number of rows in the collection and the 
            /// <see cref="IDataGridView.AllowUserToAddRows"></see> property of the 
            /// <see cref="IDataGridView"></see> is set to true.-or-The associated 
            /// <see cref="IDataGridView"></see> control is bound to an 
            /// <see cref="T:System.ComponentModel.IBindingList"></see> implementation with 
            /// <see cref="P:System.ComponentModel.IBindingList.AllowRemove"></see> and 
            /// <see cref="P:System.ComponentModel.IBindingList.SupportsChangeNotification"></see> 
            /// property values that are not both true.</exception>
            /// <filterpriority>1</filterpriority>
            public void RemoveAt(int index)
            {
                _rows.RemoveAt(index);
            }

            /// <summary>
            /// Returns the index of a specified item in the collection
            /// </summary>
            /// <param name="dataGridViewRow">The DataGridViewRow to locate in the DataGridViewRowCollection</param>
            /// <returns>The index of value if it is a DataGridViewRow found in the DataGridViewRowCollection; otherwise, -1.</returns>
            public int IndexOf(IDataGridViewRow dataGridViewRow)
            {
                if (dataGridViewRow == null) throw new ArgumentNullException("dataGridViewRow");

                DataGridViewRowVWG rowWin = (DataGridViewRowVWG)dataGridViewRow;
                return _rows.IndexOf(rowWin.DataGridViewRow);
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
                    yield return new DataGridViewRowVWG(row);
                }
            }
        }

        /// <summary>
        /// Represents a collection of DataGridViewColumn objects in a DataGridView control.
        /// </summary>
        protected class DataGridViewColumnCollectionVWG : IDataGridViewColumnCollection
        {
            private readonly DataGridViewColumnCollection _columns;

            public DataGridViewColumnCollectionVWG(DataGridViewColumnCollection columns)
            {
                if (columns == null) throw new ArgumentNullException("columns");
                _columns = columns;
            }

            #region IDataGridViewColumnCollection Members

            /// <summary>
            /// Gets the number of columns held in this collection
            /// </summary>
            public int Count
            {
                get { return _columns.Count; }
            }

            /// <summary>
            /// Clears the collection
            /// </summary>
            public void Clear()
            {
                _columns.Clear();
            }

            /// <summary>
            /// Adds a DataGridViewTextBoxColumn with the given column name and column header text to the collection
            /// </summary>
            /// <returns>The index of the column</returns>
            public int Add(string columnName, string headerText)
            {
                int colnum = _columns.Add(columnName, headerText);
                _columns[colnum].DataPropertyName = columnName;
                return colnum;
            }

            /// <summary>
            /// Adds a column to the collection where the column has been
            /// wrapped using the IDataGridViewColumn pattern
            /// </summary>
            public void Add(IDataGridViewColumn dataGridViewColumn)
            {
                DataGridViewColumnVWG col = (DataGridViewColumnVWG)dataGridViewColumn;
                _columns.Add(col.DataGridViewColumn);
            }

            /// <summary>
            /// Gets or sets the column at the given index in the collection
            /// </summary>
            public IDataGridViewColumn this[int index]
            {
                get
                {
                    DataGridViewColumn column = _columns[index];
                    if (column == null) return null;
                    return GetAppropriateColumnType(column);
                }
            }

            /// <summary>
            /// Gets or sets the column of the given name in the collection
            /// </summary>
            public IDataGridViewColumn this[string name]
            {
                get
                {
                    DataGridViewColumn column = _columns[name];
                    if (column == null) return null;
                    return GetAppropriateColumnType(column);
                }
            }

            private static IDataGridViewColumn GetAppropriateColumnType(DataGridViewColumn column)
            {
                if (column is DataGridViewImageColumn)
                {
                    return new DataGridViewImageColumnVWG((DataGridViewImageColumn)column);
                }
                return new DataGridViewColumnVWG(column);
            }

            #endregion

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
                    yield return new DataGridViewColumnVWG(column);
                }
            }

            ///<summary>
            ///Returns an enumerator that iterates through a collection.
            ///</summary>
            ///
            ///<returns>
            ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
            ///</returns>
            ///<filterpriority>2</filterpriority>
            System.Collections.IEnumerator IEnumerable.GetEnumerator()
            {
                foreach (DataGridViewColumn column in _columns)
                {
                    yield return new DataGridViewColumnVWG(column);
                }
            }
        }

        /// <summary>
        /// Represents a row in a DataGridView control
        /// </summary>
        private class DataGridViewRowVWG : IDataGridViewRow
        {
            private readonly DataGridViewRow _dataGridViewRow;

            public DataGridViewRow DataGridViewRow
            {
                get { return _dataGridViewRow; }
            }

            public DataGridViewRowVWG(DataGridViewRow dataGridViewRow)
            {
                _dataGridViewRow = dataGridViewRow;
            }

            /// <summary>Gets or sets a value indicating whether the row is selected. </summary>
            /// <returns>true if the row is selected; otherwise, false.</returns>
            /// <exception cref="T:System.InvalidOperationException">The row is in a <see cref="IDataGridView"></see>
            ///  control and is a shared row.</exception>
            public bool Selected
            {
                get { return _dataGridViewRow.Selected; }
                set { _dataGridViewRow.Selected = value; }
            }

            /// <summary>
            /// Gets the relative position of the row within the DataGridView control
            /// </summary>
            public int Index
            {
                get { return _dataGridViewRow.Index; }
            }

            /// <summary>Gets a value indicating whether this row is displayed on the screen.</summary>
            /// <returns>true if the row is currently displayed on the screen; otherwise, false.</returns>
            /// <exception cref="T:System.InvalidOperationException">The row is in a <see cref="IDataGridView"></see> control and is a shared row.</exception>
            public bool Displayed
            {
                get { return _dataGridViewRow.Displayed; }
            }

            /// <summary>Gets or sets a value indicating whether the row is visible. </summary>
            /// <returns>true if the row is visible; otherwise, false.</returns>
            /// <exception cref="T:System.InvalidOperationException">The row is in a <see cref="IDataGridView"></see>
            ///  control and is a shared row.</exception>
            /// <filterpriority>1</filterpriority>
            public bool Visible
            {
                get { return _dataGridViewRow.Visible; }
                set { _dataGridViewRow.Visible = value; }
            }

            /// <summary>Sets the values of the row's cells.</summary>
            /// <returns>true if all values have been set; otherwise, false.</returns>
            /// <param name="values">One or more objects that represent the cell values in the row.-or-An
            ///  <see cref="T:System.Array"></see> of <see cref="T:System.Object"></see> values. </param>
            /// <exception cref="T:System.ArgumentNullException">values is null. </exception>
            /// <exception cref="T:System.InvalidOperationException">This method is called when the associated 
            /// <see cref="IDataGridView"></see> is operating in virtual mode. -or-This row is a shared row.</exception>
            /// <filterpriority>1</filterpriority>
            public bool SetValues(params object[] values)
            {
                return this._dataGridViewRow.SetValues(values);
            }

            /// <summary>Gets the collection of cells that populate the row.</summary>
            /// <returns>A <see cref="IDataGridViewCellCollection"></see> that contains all of the cells in the row.</returns>
            /// <filterpriority>1</filterpriority>
            public IDataGridViewCellCollection Cells
            {
                get { return new DataGridViewCellCollectionVWG(_dataGridViewRow.Cells); }
            }

            /// <summary>Gets the data-bound object that populated the row.</summary>
            /// <returns>The data-bound <see cref="T:System.Object"></see>.</returns>
            /// <filterpriority>1</filterpriority>
            public object DataBoundItem
            {
                get { return _dataGridViewRow.DataBoundItem; }
            }

            /// <summary>Gets or sets a value indicating whether the row is read-only.</summary>
            /// <returns>true if the row is read-only; otherwise, false.</returns>
            /// <exception cref="T:System.InvalidOperationException">The row is in a <see cref="IDataGridView"></see> control and is a shared row.</exception>
            /// <filterpriority>1</filterpriority>
            public bool ReadOnly
            {
                get { return _dataGridViewRow.ReadOnly; }
                set { _dataGridViewRow.ReadOnly = value; }
            }
        }

        /// <summary>
        /// Represents a collection of cells in a DataGridViewRow
        /// </summary>
        private class DataGridViewCellCollectionVWG : IDataGridViewCellCollection
        {
            private readonly DataGridViewCellCollection _cells;

            public DataGridViewCellCollectionVWG(DataGridViewCellCollection cells)
            {
                _cells = cells;
            }

            //public int Count
            //{
            //    get { return _cells.Count; }
            //}

            /// <summary>Adds a cell to the collection.</summary>
            /// <returns>The position in which to insert the new element.</returns>
            /// <param name="dataGridViewCell">A <see cref="IDataGridViewCell"></see> to add to the collection.</param>
            /// <exception cref="T:System.InvalidOperationException">The row that owns this 
            /// <see cref="IDataGridViewCellCollection"></see> already belongs to a DataGridView control.-or-
            /// dataGridViewCell already belongs to a DataGridViewRow>.</exception>
            /// <filterpriority>1</filterpriority>
            public int Add(IDataGridViewCell dataGridViewCell)
            {
                return _cells.Add((DataGridViewCell)dataGridViewCell);
            }

            /// <summary>Clears all cells from the collection.</summary>
            /// <exception cref="T:System.InvalidOperationException">The row that owns this 
            /// <see cref="IDataGridViewCellCollection"></see> already belongs to a DataGridView control.</exception>
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
                return _cells.Contains((DataGridViewCell)dataGridViewCell);
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
                return _cells.IndexOf((DataGridViewCell)dataGridViewCell);
            }

            ///// <summary>Inserts a cell into the collection at the specified index. </summary>
            ///// <param name="dataGridViewCell">The <see cref="IDataGridViewCell"></see> to insert.</param>
            ///// <param name="index">The zero-based index at which to place dataGridViewCell.</param>
            ///// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="IDataGridViewCellCollection"></see> already belongs to a <see cref="T:Gizmox.WebGUI.Forms.DataGridView"></see> control.-or-dataGridViewCell already belongs to a <see cref="T:Gizmox.WebGUI.Forms.DataGridViewRow"></see>.</exception>
            ///// <filterpriority>1</filterpriority>
            //public void Insert(int index, IDataGridViewCell dataGridViewCell)
            //{
            //    throw new NotImplementedException();
            //}

            ///// <summary>Removes the specified cell from the collection.</summary>
            ///// <param name="cell">The <see cref="IDataGridViewCell"></see> to remove from the collection.</param>
            ///// <exception cref="T:System.ArgumentException">cell could not be found in the collection.</exception>
            ///// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="IDataGridViewCellCollection"></see> already belongs to a <see cref="T:Gizmox.WebGUI.Forms.DataGridView"></see> control.</exception>
            ///// <filterpriority>1</filterpriority>
            //public void Remove(IDataGridViewCell cell)
            //{
            //    throw new NotImplementedException();
            //}

            ///// <summary>Removes the cell at the specified index.</summary>
            ///// <param name="index">The zero-based index of the <see cref="IDataGridViewCell"></see> to be removed.</param>
            ///// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="IDataGridViewCellCollection"></see> already belongs to a <see cref="T:Gizmox.WebGUI.Forms.DataGridView"></see> control.</exception>
            ///// <filterpriority>1</filterpriority>
            //public void RemoveAt(int index)
            //{
            //    throw new NotImplementedException();
            //}

            /// <summary>Gets or sets the cell at the provided index location. In C#, this property is 
            /// the indexer for the <see cref="IDataGridViewCellCollection"></see> class.</summary>
            /// <returns>The <see cref="IDataGridViewCell"></see> stored at the given index.</returns>
            /// <param name="index">The zero-based index of the cell to get or set.</param>
            /// <exception cref="T:System.InvalidOperationException">The specified cell when setting this 
            /// property already belongs to a DataGridView control.-or-The specified cell when setting this 
            /// property already belongs to a DataGridViewRow.</exception>
            /// <exception cref="T:System.ArgumentNullException">The specified value when setting this property is null.</exception>
            /// <filterpriority>1</filterpriority>
            public IDataGridViewCell this[int index]
            {
                get { return new DataGridViewCellVWG(_cells[index]); }
            }

            /// <summary>Gets or sets the cell in the column with the provided name. In C#, this property is 
            /// the indexer for the <see cref="IDataGridViewCellCollection"></see> class.</summary>
            /// <returns>The <see cref="IDataGridViewCell"></see> stored in the column with the given name.</returns>
            /// <param name="columnName">The name of the column in which to get or set the cell.</param>
            /// <exception cref="T:System.InvalidOperationException">The specified cell when setting this 
            /// property already belongs to a DataGridView control.-or-The specified cell when setting this 
            /// property already belongs to a DataGridViewRow".</exception>
            /// <exception cref="T:System.ArgumentException">columnName does not match the name of any columns in the control.</exception>
            /// <exception cref="T:System.ArgumentNullException">The specified value when setting this property is null.</exception>
            /// <filterpriority>1</filterpriority>
            public IDataGridViewCell this[string columnName]
            {
                get { return new DataGridViewCellVWG(_cells[columnName]); }
            }

            /////<summary>
            /////Returns an enumerator that iterates through a collection.
            /////</summary>
            /////
            /////<returns>
            /////An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
            /////</returns>
            /////<filterpriority>2</filterpriority>
            //public IEnumerator GetEnumerator()
            //{
            //    foreach (DataGridViewRow row in _cells)
            //    {
            //        yield return new DataGridViewRowVWG(row);
            //    }
            //}
        }

        /// <summary>
        /// Represents a collection of DataGridViewRow objects that are selected in a DataGridView
        /// </summary>
        private class DataGridViewSelectedRowCollectionVWG : IDataGridViewSelectedRowCollection
        {
            private readonly DataGridViewSelectedRowCollection _selectedRows;

            public DataGridViewSelectedRowCollectionVWG(DataGridViewSelectedRowCollection selectedRows)
            {
                _selectedRows = selectedRows;
            }

            /// <summary>
            /// Gets the total number of rows in the collection
            /// </summary>
            public int Count
            {
                get { return _selectedRows.Count; }
            }

            /// <summary>
            /// Gets the row at the specified index.
            /// </summary>
            public IDataGridViewRow this[int index]
            {
                get { return new DataGridViewRowVWG(_selectedRows[index]); }
            }

            ///<summary>
            ///Returns an enumerator that iterates through a collection.
            ///</summary>
            ///
            ///<returns>
            ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
            ///</returns>
            ///<filterpriority>2</filterpriority>
            public System.Collections.IEnumerator GetEnumerator()
            {
                foreach (DataGridViewRow row in _selectedRows)
                {
                    yield return new DataGridViewRowVWG(row);
                }
            }
        }

        /// <summary>
        /// Represents a collection of cells that are selected in a DataGridView
        /// </summary>
        private class DataGridViewSelectedCellCollectionVWG : IDataGridViewSelectedCellCollection
        {
            private readonly DataGridViewSelectedCellCollection _selectedCells;

            public DataGridViewSelectedCellCollectionVWG(DataGridViewSelectedCellCollection selectedCells)
            {
                _selectedCells = selectedCells;
            }

            /// <summary>
            /// Gets the total number of cells in the collection
            /// </summary>
            public int Count
            {
                get { return _selectedCells.Count; }
            }

            /// <summary>
            /// Gets the cell at the specified index.
            /// </summary>
            public IDataGridViewCell this[int index]
            {
                get { return new DataGridViewCellVWG(_selectedCells[index]); }
            }

            ///<summary>
            ///Returns an enumerator that iterates through a collection.
            ///</summary>
            ///
            ///<returns>
            ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
            ///</returns>
            ///<filterpriority>2</filterpriority>
            public System.Collections.IEnumerator GetEnumerator()
            {
                foreach (DataGridViewCell cell in _selectedCells)
                {
                    yield return new DataGridViewCellVWG(cell);
                }
            }
        }
    }
}
