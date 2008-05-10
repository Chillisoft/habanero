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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.Util;
using log4net;

namespace Habanero.UI.Grid
{
    /// <summary>
    /// Serves as a super-class for grid implementations
    /// </summary>
    public abstract class GridBase : DataGridView
    {
		private delegate void SetCollectionDelegate(IBusinessObjectCollection col, string uiName);
        private delegate void SetSortColumnDelegate(string columnName, bool isAscending);

        private static readonly ILog log = LogManager.GetLogger("Habanero.UI.Grid.GridBase");
        protected DataTable _dataTable;
        protected BOCollectionDataSetProvider _dataSetProvider;
		protected IBusinessObjectCollection _collection;
        private IObjectInitialiser _objectInitialiser;
        protected DataView _dataTableDefaultView;
        private SetCollectionDelegate _setCollection;
        private SetSortColumnDelegate _setSortColumn;
        private string _uiName;
        private bool _compulsoryColumnsBold;
        private Dictionary<int, string> _dateColumnIndices;
        private int _lastDataError;
        private IFilterClause _currentFilterClause;

        public event EventHandler CollectionChanged;
        public event EventHandler FilterUpdated;

        /// <summary>
        /// Constructor to initialise a new instance
        /// </summary>
        protected GridBase()
        {
            _setCollection = SetCollectionInSTAThread;
            _setSortColumn = SetSortColumnInSTAThread;
            _compulsoryColumnsBold = false;
			
            DataSourceChanged += Grid_DataSourceChanged;

            _dateColumnIndices = new Dictionary<int, string>();
            CellFormatting += CellFormattingHandler;
            DataError += DataErrorHandler;
        }

        


        /// <summary>
        /// Sets the business object collection displayed in the grid.  This
        /// collection must be pre-loaded using the collection's Load() command.
        /// </summary>
        /// <param name="col">The collection of business objects to display. This
        /// collection needs to be pre-loaded</param>
        /// <param name="uiName">The ui definition to use, as specified in the 'name'
        /// attribute of the 'ui' element</param>
		public void SetCollection(IBusinessObjectCollection col, string uiName)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(_setCollection, new object[] {col, uiName});
                }
                else
                {
                    this.SetCollectionInSTAThread(col, uiName);
                }
    	        // needed to do the call on the Forms thread.  See info about STA thread model.
            }
            catch (InvalidOperationException)
            {
                this.SetCollectionInSTAThread(col, uiName);
            }
        }

        /// <summary>
        /// Sets the business object collection displayed in the grid.  This
        /// collection must be pre-loaded using the collection's Load() command.
        /// The default ui definition will be used, that is a 'ui' element 
        /// without a 'name' attribute.
        /// </summary>
        /// <param name="col">The collection of business objects to display.  This
        /// collection must be pre-loaded.</param>
		public void SetCollection(IBusinessObjectCollection col)
        {
            SetCollection(col, "default");
        }

        /// <summary>
        /// Sets the grid's collection to the one specified, but using the
        /// STA thread
        /// </summary>
        /// <param name="collection">The collection to display in the grid</param>
        /// <param name="uiName">The name of the uidef to use</param>
        /// TODO: Refactor
		private void SetCollectionInSTAThread(IBusinessObjectCollection collection, string uiName)
        {
            _collection = collection;
            _dataSetProvider = CreateBusinessObjectCollectionDataSetProvider(_collection);
            _dataSetProvider.ObjectInitialiser = _objectInitialiser;
            _uiName = uiName;
            ClassDef classDef = collection.ClassDef;
            UIDef uiDef = classDef.GetUIDef(uiName);
            UIGrid uiGrid = uiDef.UIGrid;
            _dataTable = _dataSetProvider.GetDataTable(uiGrid);
            _dataTable.TableName = "Table";
            _dateColumnIndices.Clear();

            this.Columns.Clear();

            DataGridViewColumn col = new DataGridViewTextBoxColumn(); // DataGridViewTextBoxColumn();
            //col.Visible = false;
            col.Width = 0;
            
            col.Visible = false;
            col.ReadOnly = true;
            DataColumn dataColumn = _dataTable.Columns[0];
            col.HeaderText = dataColumn.Caption;
            col.Name = dataColumn.ColumnName;
            col.DataPropertyName = dataColumn.ColumnName;
            this.Columns.Add(col);
            int colNum = 1;
            foreach (UIGridColumn gridColumn in uiGrid)
            {
                dataColumn = _dataTable.Columns[colNum];
                PropDef propDef = null;
                if (classDef.PropDefColIncludingInheritance.Contains(gridColumn.PropertyName))
                {
                    propDef = classDef.PropDefColIncludingInheritance[gridColumn.PropertyName];
                }
                if (gridColumn.GridControlType == typeof(DataGridViewComboBoxColumn))
                {
                    DataGridViewComboBoxColumn comboBoxCol = new DataGridViewComboBoxColumn();
                    ILookupList source =
                        (ILookupList)_dataTable.Columns[colNum].ExtendedProperties["LookupList"];
                    if (source != null) {
                        DataTable table = new DataTable();
                        table.Columns.Add("id");
                        table.Columns.Add("str");

                        table.LoadDataRow(new object[] {"", ""}, true);
                        foreach (KeyValuePair<string, object> pair in source.GetLookupList()) {
                            table.LoadDataRow(new object[] {pair.Value, pair.Key}, true);
                        }
                        comboBoxCol.DataSource = table;
                        comboBoxCol.ValueMember = "str";
                        comboBoxCol.DisplayMember = "str";
                    }
                    comboBoxCol.DataPropertyName = dataColumn.ColumnName;
                    col = comboBoxCol;
                }
                else if (gridColumn.GridControlType == typeof(DataGridViewCheckBoxColumn))
                {
                    DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
                    col = checkBoxCol;
                }
                else if (gridColumn.GridControlType == typeof(DataGridViewDateTimeColumn))
                {
                    DataGridViewDateTimeColumn dateTimeCol = new DataGridViewDateTimeColumn();
                    col = dateTimeCol;
                    _dateColumnIndices.Add(colNum, (string)gridColumn.GetParameterValue("dateFormat"));
                }
                else
                {
                    col = (DataGridViewColumn)Activator.CreateInstance(gridColumn.GridControlType);
                }
                int width = (int)(dataColumn.ExtendedProperties["Width"]);
                col.Width = width;
                if (width == 0)
                {
                    col.Visible = false;
                }
                col.ReadOnly = !gridColumn.Editable;
                col.HeaderText = dataColumn.Caption;
                col.Name = dataColumn.ColumnName;
                col.DataPropertyName = dataColumn.ColumnName;
                //col.MappingName = dataColumn.ColumnName;
                col.SortMode = DataGridViewColumnSortMode.Automatic;

                SetAlignment(col, gridColumn);
                if (CompulsoryColumnsBold && propDef != null && propDef.Compulsory)
                {
                    Font newFont = new Font(DefaultCellStyle.Font, FontStyle.Bold);
                    col.HeaderCell.Style.Font = newFont;
                }

                if (propDef != null && propDef.PropertyType == typeof(DateTime)
                    && gridColumn.GridControlType != typeof(DataGridViewDateTimeColumn))
                {
                    _dateColumnIndices.Add(colNum, (string)gridColumn.GetParameterValue("dateFormat"));
                }

                //if (propDef != null && propDef.PropertyName != gridColumn.GetHeading(classDef))
                //{
                //    foreach (BusinessObject bo in _collection)
                //    {
                //        BOProp boProp = bo.Props[propDef.PropertyName];
                //        if (!boProp.HasDisplayName())
                //        {
                //            boProp.DisplayName = gridColumn.GetHeading(classDef);
                //        }
                //    }
                //}

                Columns.Add(col);
                colNum++;
            }

            _dataTableDefaultView = _dataTable.DefaultView;
            this.AutoGenerateColumns = false;
            this.DataSource = _dataTableDefaultView;
            //this.DataSource = _dataTable;
            foreach (DataGridViewColumn dataGridViewColumn in this.Columns)
            {
                if (!dataGridViewColumn.Visible)
                {
                    dataGridViewColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dataGridViewColumn.Resizable = DataGridViewTriState.False;
                }
            }
            SetSorting(uiGrid);
            if (_currentFilterClause != null)
            {
                ApplyFilter(_currentFilterClause);
            }
            FireCollectionChanged();
        }

        /// <summary>
        /// Sets the sort column as per the SortColumn property for
        /// the given grid definition
        /// </summary>
        private void SetSorting(UIGrid grid)
        {
            if (!String.IsNullOrEmpty(grid.SortColumn))
            {
                bool columnNameExists = false;
                string columnName = grid.SortColumn;
                if (grid.SortColumn.Contains(" "))
                {
                    columnName = StringUtilities.GetLeftSection(grid.SortColumn, " ");
                }
                foreach (UIGridColumn column in grid)
                {
                    if (column.PropertyName == columnName)
                    {
                        columnNameExists = true;
                    }
                }
                if (!columnNameExists)
                {
                    throw new InvalidXmlDefinitionException(String.Format(
                        "In a 'sortOrder' attribute on a 'grid' element, the " +
                        "column name '{0}' does not exist.", grid.SortColumn));
                }

                ListSortDirection direction = ListSortDirection.Ascending;
                if (grid.SortColumn.Contains(" "))
                {
                    string sortOrder = StringUtilities.GetRightSection(grid.SortColumn, columnName + " ");
                    if (sortOrder.ToLower() == "asc")
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else if (sortOrder.ToLower() == "desc" || sortOrder.ToLower() == "des")
                    {
                        direction = ListSortDirection.Descending;
                    }
                    else
                    {
                        throw new InvalidXmlDefinitionException(String.Format(
                            "In a 'sortOrder' attribute on a 'grid' element, the " +
                            "attribute given as '{0}' was not valid.  The correct " +
                            "definition has the form of 'columnName' or " +
                            "'columnName asc' or 'columnName desc'.", grid.SortColumn));
                    }
                }

                Sort(Columns[columnName], direction);
            }
        }

        /// <summary>
        /// This action is included due to a flaw in DataGridView that
        /// causes the hidden ID column to reappear when data is changed
        /// </summary>
        private void Grid_DataSourceChanged(object sender, EventArgs e)
		{
			if (Columns.Contains("ID"))
			{
				DataGridViewColumn col = Columns["ID"];
				col.Visible = false;
			}
		}

        /// <summary>
        /// Returns the business object collection being displayed in the grid
        /// </summary>
        /// <returns>Returns a business collection</returns>
        public IBusinessObjectCollection GetCollection()
        {
            return _collection;
        }

        /// <summary>
        /// Returns the name of the ui definition used, as specified in the
        /// 'name' attribute of the 'ui' element in the class definitions.
        /// By default, no 'name' attribute is specified and the ui name of
        /// "default" is used.  Having a name attribute allows you to choose
        /// between a multiple visual representations of a business object
        /// collection.
        /// </summary>
        /// <returns>Returns the name of the ui definition this grid is using
        /// </returns>
        public string UIName
        {
            get { return _uiName; }
        }

        /// <summary>
        /// Gets or sets the boolean value that determines whether column
        /// headers for compulsory values should be in bold type or not
        /// </summary>
        public bool CompulsoryColumnsBold
        {
            get { return _compulsoryColumnsBold; }
            set { _compulsoryColumnsBold = value; }
        }

        /// <summary>
        /// Calls CollectionChanged, passing this instance as the sender
        /// </summary>
        private void FireCollectionChanged()
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Adds a business object to the collection being represented
        /// </summary>
        /// <param name="bo">The business object</param>
        public void AddBusinessObject(BusinessObject bo)
        {
            _collection.Add(bo);
            int row = GetRowOfBusinessObject(bo);
            if (row != -1)
            {
                this.SetSelectedRowCore(row, true);
            }
        }

        /// <summary>
        /// Creates a data set provider for the business object collection
        /// provided
        /// </summary>
        /// <param name="col">The business object collection</param>
        /// <returns>Returns the new provider</returns>
        protected abstract BOCollectionDataSetProvider CreateBusinessObjectCollectionDataSetProvider(
			IBusinessObjectCollection col);

        /// <summary>
        /// Sets the object initialiser
        /// </summary>
        public virtual IObjectInitialiser ObjectInitialiser
        {
            set { _objectInitialiser = value;
                if (_dataSetProvider != null)
                {
                    _dataSetProvider.ObjectInitialiser = _objectInitialiser;
                }
            }
        }

        /// <summary>
        /// Returns the data table
        /// </summary>
        public DataTable DataTable
        {
            get { return _dataTable; }
        }

        /// <summary>
        /// Clears the business object collection and the rows in the data table
        /// </summary>
        public void Clear()
        {
            if (_collection != null)
            {
                _collection.Clear();
                _dataTable.Rows.Clear();
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
            _currentFilterClause = filterClause;
            if (_currentFilterClause != null)
            {
                _dataTableDefaultView.RowFilter = _currentFilterClause.GetFilterClauseString();
            }
            else
            {
                _dataTableDefaultView.RowFilter = null;
            }
            FireFilterUpdated();
            //filterUpdatedMethodCaller.Call(new VoidMethod(FireFilterUpdated)) ;
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

        protected internal virtual void SetSelectedBusinessObject(BusinessObject bo)
        {
            ClearSelection();
            if (bo == null)
            {
                if (this.CurrentRow == null) return;
                this.SetSelectedRowCore(this.CurrentRow.Index, false);
                this.CurrentCell = null;
                return;
            }
            int i = 0;
            string boID = bo.ID.ToString();
            foreach (DataRowView dataRowView in _dataTableDefaultView)
            {
                if ((string)dataRowView.Row["ID"] == boID)
                {
                    this.SetSelectedRowCore(i, true);
                    this.SetCurrentCellAddressCore(1, i, true, false, false);
                    break;
                }
                i++;
            }
            if (CurrentRow != null && !CurrentRow.Displayed)
            {
                FirstDisplayedScrollingRowIndex = Rows.IndexOf(CurrentRow);
            }
        }


        /// <summary>
        /// Returns the currently selected business object
        /// </summary>
        /// <returns>Returns the business object</returns>
        protected internal virtual BusinessObject GetSelectedBusinessObject()
        {
            if (this.CurrentCell == null) return null;
            if (!this.CurrentCell.Selected) return null;
            int rownum = this.CurrentCell.RowIndex;
            return GetBusinessObjectAtRow(rownum);
        }

        /// <summary>
        /// Returns the selected business objects as a BusinessObjectCollection
        /// </summary>
        /// <returns>Returns an IList object</returns>
        protected virtual BusinessObjectCollection<BusinessObject> GetSelectedBusinessObjects()
        {
            BusinessObjectCollection<BusinessObject> busObjects = new BusinessObjectCollection<BusinessObject>(_collection.ClassDef);
            foreach (DataGridViewRow row in this.SelectedRows)
            {
                busObjects.Add(this._dataSetProvider.Find((string) row.Cells["ID"].Value));
            }
            return busObjects;
        }

        /// <summary>
        /// Returns the row number that contains the specified business
        /// object
        /// </summary>
        /// <param name="bo">The business object in question</param>
        /// <returns>Returns the row number if found, or -1 if not found</returns>
        public virtual int GetRowOfBusinessObject(BusinessObject bo)
        {
            int rownum = 0;

            foreach (DataRowView dataRowView in _dataTableDefaultView)
            {
                if (this._dataSetProvider.Find((string) dataRowView.Row["ID"]) == bo)
                {
                    return rownum;
                }
                rownum++;
            }
            return -1;
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
                    return this._dataSetProvider.Find((string)dataRowView.Row["ID"]);
                }
            }
            return null;
        }

        /// <summary>
        /// Sets the sort column and indicates whether
        /// it should be sorted in ascending or descending order
        /// </summary>
        /// <param name="columnName">The column number to set</param>
        /// <param name="isAscending">Whether sorting should be done in ascending
        /// order ("false" sets it to descending order)</param>
        public void SetSortColumn(string columnName, bool isAscending)
        {
            try
            {
                BeginInvoke(_setSortColumn, new object[] {columnName, isAscending});
                // needed to do the call on the Forms thread.  See info about STA thread model.
            }
            catch (InvalidOperationException)
            {
                this.SetSortColumnInSTAThread(columnName, isAscending);
            }
        }

        /// <summary>
        /// Sets the sort column as before, but using a STA thread.  This
        /// method is called by SetSortColumn().
        /// </summary>
        private void SetSortColumnInSTAThread(string columnName, bool isAscending)
        {
            if (isAscending)
            {
                _dataTableDefaultView.Sort = columnName + " ASC";
            }
            else
            {
                _dataTableDefaultView.Sort = columnName + " DESC";
            }
        }

        private void DataErrorHandler(object sender, DataGridViewDataErrorEventArgs e)
        {
            DataGridViewColumn dataGridViewColumn = Columns[e.ColumnIndex];
            e.ThrowException = false;
            string dataErrorDescription = e.Exception.Message + " Row:" + e.RowIndex + " Column:" + e.ColumnIndex;
            int dataErrorHashCode = dataErrorDescription.GetHashCode();
            if (_lastDataError != dataErrorHashCode)
            {
                _lastDataError = dataErrorHashCode;
                GlobalRegistry.UIExceptionNotifier.Notify(e.Exception,
                      String.Format("Error with data in Grid at row {0} and column '{1}'. The context of the error was '{2}'.",
                      e.RowIndex, dataGridViewColumn.HeaderText, StringUtilities.DelimitPascalCase(e.Context.ToString(), " ")), "Grid data error");
            }
        }

        /// <summary>
        /// Handles the CellFormatting event.  For date formatting, this only
        /// serves as a special-case handler for text columns, which don't know
        /// how to apply date formats to a string.
        /// </summary>
        private void CellFormattingHandler(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (_dateColumnIndices.ContainsKey(e.ColumnIndex) && e.Value != null && e.Value != DBNull.Value)
            {
                string format = GetDateFormatString(_dateColumnIndices[e.ColumnIndex]);
                DateTime dt;
                if (e.Value is DateTime) dt = (DateTime)e.Value;
                else
                {
                    if (!DateTime.TryParse((string)e.Value, out dt)) return;
                }
                e.Value = dt.ToString(format, DateTimeFormatInfo.CurrentInfo);
                e.FormattingApplied = true;
            }
        }

        /// <summary>
        /// Calculates the appropriate date format string.  First preference
        /// is given to the date format for the column as given in the class
        /// defs, followed by the global grid date display format, and the
        /// short date format of the user's environment as the last option.
        /// </summary>
        private static string GetDateFormatString(string defDateFormatParameter)
        {
            string format = defDateFormatParameter;
            if (format == null && GlobalUIRegistry.DateDisplaySettings != null)
            {
                format = GlobalUIRegistry.DateDisplaySettings.GridDateFormat;
            }
            if (format == null) format = "d";
            return format;
        }

        /// <summary>
        /// Sets the alignment of the column
        /// </summary>
        private static void SetAlignment(DataGridViewColumn column, UIGridColumn columnDef)
        {
            switch (columnDef.Alignment)
            {
                case UIGridColumn.PropAlignment.centre:
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    break;
                case UIGridColumn.PropAlignment.left:
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    break;
                case UIGridColumn.PropAlignment.right:
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    break;
            }
        }
    }
}
