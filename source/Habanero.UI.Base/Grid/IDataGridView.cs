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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace Habanero.UI.Base
{
    /// <summary>Describes how cells of a DataGridView control can be selected.</summary>
    /// <filterpriority>2</filterpriority>
    //[Serializable()]
    public enum DataGridViewSelectionMode
    {
        ///<summary>One or more individual cells can be selected.</summary>
        CellSelect = 1,

        ///<summary>The entire row will be selected by clicking its row's header or a cell contained in that row.</summary>
        FullRowSelect = 2,

        ///<summary>The entire column will be selected by clicking the column's header or a cell contained in that column.</summary>
        FullColumnSelect = 4,

        ///<summary>The row will be selected by clicking in the row's header cell. An individual cell can be selected by clicking that cell.</summary>
        RowHeaderSelect = 8,

        ///<summary>The column will be selected by clicking in the column's header cell. An individual cell can be selected by clicking that cell.</summary>
        ColumnHeaderSelect = 16
    }

    /// <summary>
    /// Displays data in a customizable grid
    /// </summary>
    public interface IDataGridView : IControlHabanero
    {
        /// <summary>Puts the current cell in edit mode.</summary>
        /// <returns>true if the current cell is already in edit mode or successfully enters edit mode; otherwise, false.</returns>
        /// <param name="selectAll">true to select all the cell's contents; false to not select any contents.</param>
        /// <exception cref="T:System.Exception">Initialization of the editing cell value failed and either
        /// there is no handler for the DataError event or the handler has set the DataGridViewDataErrorEventArgs.ThrowException
        /// property to true. The exception object can typically be cast to type FormatException</exception>
        /// <exception cref="T:System.InvalidCastException">The type indicated by the cell's EditType property
        /// does not derive from the Control type.-or-The type indicated by the cell's EditType property does
        /// not implement the IDataGridViewEditingControl interface.</exception>
        /// <exception cref="T:System.InvalidOperationException">CurrentCell is not set to a valid cell.-or-This
        /// method was called in a handler for the CellBeginEdit event.</exception>
        /// <filterpriority>1</filterpriority>
        bool BeginEdit(bool selectAll);

        /// <summary>Cancels edit mode for the currently selected cell and discards any changes.</summary>
        /// <returns>true if the cancel was successful; otherwise, false.</returns>
        /// <filterpriority>1</filterpriority>
        bool CancelEdit();

        /// <summary>Clears the current selection by unselecting all selected cells.</summary>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        /// <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        /// <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        void ClearSelection();

        /// <summary>Returns the number of columns displayed to the user.</summary>
        /// <returns>The number of columns displayed to the user.</returns>
        /// <param name="includePartialColumns">true to include partial columns in the displayed column count; otherwise, false. </param>
        /// <filterpriority>1</filterpriority>
        int DisplayedColumnCount(bool includePartialColumns);

        /// <summary>Returns the number of rows displayed to the user.</summary>
        /// <returns>The number of rows displayed to the user.</returns>
        /// <param name="includePartialRow">true to include partial rows in the displayed row count; otherwise, false. </param>
        /// <filterpriority>1</filterpriority>
        int DisplayedRowCount(bool includePartialRow);

        /// <summary>Commits and ends the edit operation on the current cell using the default error context.</summary>
        /// <returns>true if the edit operation is committed and ended; otherwise, false.</returns>
        /// <exception cref="T:System.Exception">The cell value could not be committed and either there
        /// is no handler for the DataError event or the handler has set the DataGridViewDataErrorEventArgs.ThrowException
        /// property to true.</exception>
        bool EndEdit();

        /// <summary>Refreshes the value of the current cell with the underlying cell value when the
        /// cell is in edit mode, discarding any previous value.</summary>
        /// <returns>true if successful; false if a DataError event occurred.</returns>
        /// <filterpriority>1</filterpriority>
        bool RefreshEdit();

        /// <summary>Selects all the cells in the <see cref="IDataGridView"></see>.</summary>
        /// <filterpriority>1</filterpriority>
        void SelectAll();

        /// <summary>Sorts the contents of the IDataGridView control using an implementation of the
        /// <see cref="T:System.Collections.IComparer"></see> interface.</summary>
        /// <param name="comparer">An implementation of <see cref="T:System.Collections.IComparer"></see> that
        /// performs the custom sorting operation. </param>
        /// <exception cref="T:System.InvalidOperationException">VirtualMode is set to true.-or-
        /// DataSource is not null.</exception>
        /// <exception cref="T:System.ArgumentNullException">comparer is null.</exception>
        /// <filterpriority>1</filterpriority>
        void Sort(IComparer comparer);

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
        void Sort(IDataGridViewColumn dataGridViewColumn, ListSortDirection direction);

        /// <summary>
        /// Sets the sort column and indicates whether
        /// it should be sorted in ascending or descending order
        /// </summary>
        /// <param name="columnName">The column number to sort on</param>
        /// object property</param>
        /// <param name="ascending">True for ascending order, false for descending order</param>
        void Sort(string columnName, bool ascending);

        /// <summary>Forces the control to update its display of the cell at the specified location based 
        /// on its new value, applying any automatic sizing modes currently in effect. </summary>
        /// <param name="columnIndex">The zero-based column index of the cell with the new value.</param>
        /// <param name="rowIndex">The zero-based row index of the cell with the new value.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">columnIndex is less than zero or greater
        /// than the number of columns in the control minus one.-or-rowIndex is less than zero or
        /// greater than the number of rows in the control minus one.</exception>
        void UpdateCellValue(int columnIndex, int rowIndex);

        //[EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false)]
        //IControlHabanero EditingControl { get; }

        /// <summary>Gets or sets a value indicating whether the option to add rows is displayed to the user.</summary>
        /// <returns>true if the add-row option is displayed to the user; otherwise false. The default is true.</returns>
        /// <filterpriority>1</filterpriority>
        bool AllowUserToAddRows { get; set; }

        /// <summary>Gets or sets a value indicating whether the user is allowed to delete rows from
        /// the <see cref="IDataGridView"></see>.</summary>
        /// <returns>true if the user can delete rows; otherwise, false. The default is true.</returns>
        /// <filterpriority>1</filterpriority>
        bool AllowUserToDeleteRows { get; set; }

        /// <summary>Gets or sets a value indicating whether manual column repositioning is enabled.</summary>
        /// <returns>true if the user can change the column order; otherwise, false. The default is false.</returns>
        /// <filterpriority>1</filterpriority>
        bool AllowUserToOrderColumns { get; set; }

        /// <summary>Gets or sets a value indicating whether users can resize columns.</summary>
        /// <returns>true if users can resize columns; otherwise, false. The default is true.</returns>
        bool AllowUserToResizeColumns { get; set; }

        /// <summary>Gets or sets a value indicating whether users can resize rows.</summary>
        /// <returns>true if all the rows are resizable; otherwise, false. The default is true.</returns>
        bool AllowUserToResizeRows { get; set; }

        /// <summary>Gets or sets a value indicating whether columns are created automatically when
        /// the DataSource or DataMember properties are set.</summary>
        /// <returns>true if the columns should be created automatically; otherwise, false. The default is true.</returns>
        /// <filterpriority>1</filterpriority>
        [EditorBrowsable(EditorBrowsableState.Advanced), DefaultValue(true), Browsable(false)]
        bool AutoGenerateColumns { get; set; }

        /// <summary>Gets or sets the number of columns displayed in the <see cref="IDataGridView"></see>.</summary>
        /// <returns>The number of columns displayed in the <see cref="IDataGridView"></see>.</returns>
        /// <exception cref="T:System.InvalidOperationException">When setting this property,
        /// the DataSource property has been set. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The specified value when setting this property is less than 0. </exception>
        /// <filterpriority>1</filterpriority>
        [DefaultValue(0), EditorBrowsable(EditorBrowsableState.Advanced),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        int ColumnCount { get; set; }

        /// <summary>Gets or sets a value indicating whether the column header row is displayed.</summary>
        /// <returns>true if the column headers are displayed; otherwise, false. The default is true.</returns>
        /// <exception cref="T:System.InvalidOperationException">The specified value when setting this
        /// property is false and one or more columns have an DataGridViewColumn.InheritedAutoSizeMode
        /// property value of DataGridViewAutoSizeColumnMode.ColumnHeader.</exception>
        /// <filterpriority>1</filterpriority>
        [DefaultValue(true)]
        bool ColumnHeadersVisible { get; set; }

        /// <summary>Gets a collection that contains all the columns in the control.</summary>
        /// <returns>The <see cref="IDataGridViewColumnCollection"></see> that contains all the columns
        /// in the <see cref="IDataGridView"></see> control.</returns>
        /// <filterpriority>1</filterpriority>
        [Editor(
            "Gizmox.WebGUI.Forms.Design.DataGridViewColumnCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
            , typeof (UITypeEditor)), MergableProperty(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        IDataGridViewColumnCollection Columns { get; }

        /// <summary>Gets or sets the currently active cell.</summary>
        /// <returns>The <see cref="IDataGridViewCell"></see> that represents the current cell, or null
        /// if there is no current cell. The default is the first cell in the first column or null if
        /// there are no cells in the control.</returns>
        /// <exception cref="T:System.ArgumentException">The specified cell when setting this property is not
        /// in the <see cref="IDataGridView"></see>.</exception>
        /// <exception cref="T:System.InvalidOperationException">The value of this property cannot be set
        /// because changes to the current cell cannot be committed or canceled.-or-The specified cell when 
        /// setting this property is in a hidden row or column. </exception>
        /// <filterpriority>1</filterpriority>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        IDataGridViewCell CurrentCell { get; set; }

        /// <summary>Gets the row and column indexes of the currently active cell.</summary>
        /// <returns>A <see cref="T:System.Drawing.Point"></see> that represents the row and column indexes 
        /// of the currently active cell.</returns>
        /// <filterpriority>1</filterpriority>
        [Browsable(false)]
        Point CurrentCellAddress { get; }

        /// <summary>Gets the row containing the current cell.</summary>
        /// <returns>The <see cref="IDataGridViewRow"></see> that represents the row containing the current 
        /// cell, or null if there is no current cell.</returns>
        [Browsable(false)]
        IDataGridViewRow CurrentRow { get; }

        /// <summary>Gets or sets the name of the list or table in the data source for which the 
        /// <see cref="IDataGridView"></see> is displaying data.</summary>
        /// <returns>The name of the table or list in the <see cref="DataSource"></see> for which the 
        /// <see cref="IDataGridView"></see> is displaying data. The default is <see cref="F:System.String.Empty"></see>.</returns>
        /// <exception cref="T:System.Exception">An error occurred in the data source and either 
        /// there is no handler for the DataError event or the handler has set the DataGridViewDataErrorEventArgs.ThrowException
        /// property to true. The exception object can typically be cast to type <see cref="T:System.FormatException"></see>.</exception>
        /// <filterpriority>1</filterpriority>
        [Editor(
            "Gizmox.WebGUI.Forms.Design.DataMemberListEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
            , typeof (UITypeEditor)), DefaultValue("")]
        string DataMember { get; set; }

        /// <summary>Gets or sets the data source that the <see cref="IDataGridView"></see> is displaying data for.</summary>
        /// <returns>The object that contains data for the <see cref="IDataGridView"></see> to display.</returns>
        /// <exception cref="T:System.Exception">An error occurred in the data source and either there is no handler 
        /// for the event or the handler has set the property to true. The exception object can typically be
        ///  cast to type <see cref="T:System.FormatException"></see>.</exception>
        /// <filterpriority>1</filterpriority>
        [ RefreshProperties(RefreshProperties.Repaint), DefaultValue((string) null)]
        object DataSource { get; set; }

        ///// <summary>Gets the panel that contains the <see cref="P:Gizmox.WebGUI.Forms.DataGridView.EditingControl"></see>.</summary>
        ///// <returns>The <see cref="T:Gizmox.WebGUI.Forms.Panel"></see> that contains the <see cref="P:Gizmox.WebGUI.Forms.DataGridView.EditingControl"></see>.</returns>
        ///// <filterpriority>1</filterpriority>
        //[EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false)]
        //IPanel EditingPanel { get; }

        ///// <summary>Gets or sets a value indicating how to begin editing a cell.</summary>
        ///// <returns>One of the <see cref="IDataGridViewEditMode"></see> values. The default is <see cref="F:Gizmox.WebGUI.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2"></see>.</returns>
        ///// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is not a valid <see cref="IDataGridViewEditMode"></see> value.</exception>
        ///// <exception cref="T:System.Exception">The specified value when setting this property would cause the control to enter edit mode, but initialization of the editing cell value failed and either there is no handler for the <see cref="E:Gizmox.WebGUI.Forms.DataGridView.DataError"></see> event or the handler has set the <see cref="P:Gizmox.WebGUI.Forms.DataGridViewDataErrorEventArgs.ThrowException"></see> property to true. The exception object can typically be cast to type <see cref="T:System.FormatException"></see>.</exception>
        ///// <filterpriority>1</filterpriority>
        //[DefaultValue(DataGridViewEditMode.EditOnKeystrokeOrF2), SRDescription("DataGridView_EditModeDescr"),
        // SRCategory("CatBehavior")]
        //DataGridViewEditMode EditMode { get; set; }

        /// <summary>
        /// Gets or sets the index of the row that is the first row displayed on the DataGridView
        /// </summary>
        int FirstDisplayedScrollingRowIndex { get; set; }

        /// <summary>Gets a value indicating whether the current cell has uncommitted changes.</summary>
        /// <returns>true if the current cell has uncommitted changes; otherwise, false.</returns>
        /// <filterpriority>1</filterpriority>
        [Browsable(false)]
        bool IsCurrentCellDirty { get; }

        /// <summary>Gets a value indicating whether the current row has uncommitted changes.</summary>
        /// <returns>true if the current row has uncommitted changes; otherwise, false.</returns>
        /// <filterpriority>1</filterpriority>
        [Browsable(false)]
        bool IsCurrentRowDirty { get; }

        // TODO: THESE TWO AREN'T IN THE WINFORMS ONE
        ///// <summary>Gets or sets the cell located at the intersection of the row with the specified 
        ///// index and the column with the specified name. </summary>
        ///// <returns>The <see cref="IDataGridViewCell"></see> at the specified location.</returns>
        ///// <param name="columnName">The name of the column containing the cell.</param>
        ///// <param name="rowIndex">The index of the row containing the cell.</param>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        //IDataGridViewCell this[string columnName, int rowIndex] { get; set; }

        ///// <summary>Gets or sets the cell located at the intersection of the row and column with the specified indexes. </summary>
        ///// <returns>The <see cref="IDataGridViewCell"></see> at the specified location.</returns>
        ///// <param name="columnIndex">The index of the column containing the cell.</param>
        ///// <param name="rowIndex">The index of the row containing the cell.</param>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        //IDataGridViewCell this[int columnIndex, int rowIndex] { get; set; }

        /// <summary>Gets or sets a value indicating whether the user is allowed to select more than one 
        /// cell, row, or column of the <see cref="IDataGridView"></see> at a time.</summary>
        /// <returns>true if the user can select more than one cell, row, or column at a time; otherwise, 
        /// false. The default is true.</returns>
        /// <filterpriority>1</filterpriority>
        [DefaultValue(true)]
        bool MultiSelect { get; set; }

        /// <summary>Gets the index of the row for new records.</summary>
        /// <returns>The index of the row for new records, or -1 if <see cref="AllowUserToAddRows"></see> is false.</returns>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        int NewRowIndex { get; }

        /// <summary>Gets a value indicating whether the user can edit the cells of the <see cref="IDataGridView"></see> control.</summary>
        /// <returns>true if the user cannot edit the cells of the <see cref="IDataGridView"></see> control; 
        /// otherwise, false. The default is false.</returns>
        /// <exception cref="T:System.InvalidOperationException">The specified value when setting this property 
        /// is true, the current cell is in edit mode, and the current cell contains changes that cannot be committed. </exception>
        /// <exception cref="T:System.Exception">The specified value when setting this property would cause the 
        /// control to enter edit mode, but initialization of the editing cell value failed and either there 
        /// is no handler for the DataError event or the handler has set the DataGridViewDataErrorEventArgs.ThrowException
        /// property to true. The exception object can typically be cast to type <see cref="T:System.FormatException"></see>.</exception>
        /// <filterpriority>1</filterpriority>
        [DefaultValue(false), Browsable(true)]
        bool ReadOnly { get; set; }

        /// <summary>
        /// Uses internal paging algorithem
        /// </summary>
        [System.ComponentModel.DefaultValue(true)]
        bool UseInternalPaging { get; set; }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value></value>
        [System.ComponentModel.DefaultValue(1)]
        int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value></value>
        [System.ComponentModel.DefaultValue(20)]
        int ItemsPerPage { get; set; }

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        /// <value></value>
        [System.ComponentModel.DefaultValue(1)]
        int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the total items.
        /// </summary>
        /// <value></value>
        [System.ComponentModel.DefaultValue(0)]
        int TotalItems { get; set; }

        /// <summary>
        /// When pagination is used, changes the current page to the one containing
        /// the given row number
        /// </summary>
        /// <param name="rowNum">The row that you wish to show the page of.  For example, if your grid has
        /// 30 rows and is set to 20 rows per page, calling ChangeToPageOfRow with an argument
        /// of 25 will set the page to page 2 since row 25 is on page 2.</param>
        void ChangeToPageOfRow(int rowNum);

        /// <summary>Gets or sets the number of rows displayed in the <see cref="IDataGridView"></see>.</summary>
        /// <returns>The number of rows to display in the <see cref="IDataGridView"></see>.</returns>
        /// <exception cref="T:System.ArgumentException">The specified value when setting this property is less 
        /// than 0.-or-The specified value is less than 1 and <see cref="AllowUserToAddRows"></see> is set to true. </exception>
        /// <exception cref="T:System.InvalidOperationException">When setting this property, the </see> property is set. </exception>
        /// <filterpriority>1</filterpriority>
        [DefaultValue(0), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
         EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false)]
        int RowCount { get; set; }

        /// <summary>Gets or sets a value indicating whether the column that contains row headers is displayed.</summary>
        /// <returns>true if the column that contains row headers is displayed; otherwise, false. The default is true.</returns>
        /// <exception cref="T:System.InvalidOperationException">The specified value when setting this property 
        /// is false and the </see> property is set to </see> or </see>.</exception>
        /// <filterpriority>1</filterpriority>
        [ DefaultValue(true)]
        bool RowHeadersVisible { get; set; }

        /// <summary>Gets or sets the width, in pixels, of the column that contains the row headers.</summary>
        /// <returns>The width, in pixels, of the column that contains row headers. The default is 43.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The specified value when setting this property 
        /// is less than the minimum width of 4 pixels or is greater than the maximum width of 32768 pixels.</exception>
        /// <filterpriority>1</filterpriority>
        [Localizable(true)]
        int RowHeadersWidth { get; set; }

        /// <summary>Gets a collection that contains all the rows in the <see cref="IDataGridView"></see> control.</summary>
        /// <returns>A <see cref="IDataGridViewRowCollection"></see> that contains all the rows in the
        ///  <see cref="IDataGridView"></see>.</returns>
        /// <filterpriority>1</filterpriority>
        [Browsable(false)]
        IDataGridViewRowCollection Rows { get; }

        /// <summary>Gets the column by which the <see cref="IDataGridView"></see> contents are currently sorted.</summary>
        /// <returns>The <see cref="IDataGridViewColumn"></see> by which the <see cref="IDataGridView"></see> 
        /// contents are currently sorted.</returns>
        /// <filterpriority>1</filterpriority>
        [Browsable(false)]
        IDataGridViewColumn SortedColumn { get; }

        ///// <summary>Gets a value indicating whether the items in the <see cref="IDataGridView"></see> 
        /// control are sorted in ascending or descending order, or are not sorted.</summary>
        ///// <returns>One of the <see cref="T:Gizmox.WebGUI.Forms.SortOrder"></see> values.</returns>
        ///// <filterpriority>1</filterpriority>
        //[Browsable(false)]
        //SortOrder SortOrder { get; }

        /// <summary>Gets or sets a value indicating whether the TAB key moves the focus to the next 
        /// control in the tab order rather than moving focus to the next cell in the control.</summary>
        /// <returns>true if the TAB key moves the focus to the next control in the tab order; otherwise, false.</returns>
        /// <filterpriority>1</filterpriority>
        [DefaultValue(false),
         EditorBrowsable(EditorBrowsableState.Advanced)]
        bool StandardTab { get; set; }

        /// <summary>
        /// Gets the collection of currently selected rows
        /// </summary>
        IDataGridViewSelectedRowCollection SelectedRows { get; }

        /// <summary>
        /// Gets the collection of currently selected cells
        /// </summary>
        IDataGridViewSelectedCellCollection SelectedCells { get; }

        /// <summary>
        /// Occurs when the current selection changes.
        /// </summary>
        /// <filterpriority>1</filterpriority>
        event EventHandler SelectionChanged;

        /// <summary>Gets or sets a value indicating how the cells of the DataGridView can be selected.</summary>
        /// <returns>One of the DataGridViewSelectionMode values. The default is DataGridViewSelectionMode.RowHeaderSelect.</returns>
        /// <exception cref="T:System.InvalidOperationException">The specified value when setting this property is DataGridViewSelectionMode.FullColumnSelect or DataGridViewSelectionMode.ColumnHeaderSelect and the DataGridViewColumn.SortMode property of one or more columns is set to DataGridViewColumnSortMode.Automatic.</exception>
        /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is not a valid DataGridViewSelectionMode value.</exception>
        DataGridViewSelectionMode SelectionMode { get; set; }
    }
}