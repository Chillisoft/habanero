using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Grid;
using log4net;
using BusinessObject=Habanero.BO.BusinessObject;

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
        
        public event EventHandler CollectionChanged;
        public event EventHandler FilterUpdated;

        /// <summary>
        /// Constructor to initialise a new instance
        /// </summary>
        protected GridBase()
        {
            _setCollection = new SetCollectionDelegate(SetCollectionInSTAThread);
            _setSortColumn = new SetSortColumnDelegate(SetSortColumnInSTAThread);
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
                BeginInvoke(_setCollection, new object[] { col, uiName });
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
            UIGrid grid = collection.ClassDef.UIDefCol[uiName].UIGrid;
            _dataTable = _dataSetProvider.GetDataTable(grid);
            _dataTable.TableName = "Table";

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
            foreach (UIGridColumn gridColumn in grid)
            {
                dataColumn = _dataTable.Columns[colNum];

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

                }
                else
                {
                    col = (DataGridViewColumn)Activator.CreateInstance(gridColumn.GridControlType);
                }
                col.Width = (int)(dataColumn.ExtendedProperties["Width"]);
                col.ReadOnly = !gridColumn.Editable;
                col.HeaderText = dataColumn.Caption;
                col.Name = dataColumn.ColumnName;
                col.DataPropertyName = dataColumn.ColumnName;
                //col.MappingName = dataColumn.ColumnName;

                switch (gridColumn.Alignment)
                {
                    case UIGridColumn.PropAlignment.centre:
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                    case UIGridColumn.PropAlignment.left:
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        break;
                    case UIGridColumn.PropAlignment.right:
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        break;
                }
                this.Columns.Add(col);
                colNum++;
            }

            _dataTableDefaultView = _dataTable.DefaultView;

            this.AutoGenerateColumns = false;
            this.DataSource = _dataTableDefaultView;

            //this.DataSource = _dataTable;
            FireCollectionChanged();
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
            this.SetSelectedRowCore(row, true);
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
            _dataTableDefaultView.RowFilter = filterClause.GetFilterClauseString();
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

        /// <summary>
        /// Returns the currently selected business object
        /// </summary>
        /// <returns>Returns the business object</returns>
        protected virtual BusinessObject GetSelectedBusinessObject()
        {
            if (this.CurrentCell == null) return null;
            int rownum = this.CurrentCell.RowIndex;
            int i = 0;
            foreach (DataRowView dataRowView in _dataTableDefaultView)
            {
                
                if (i++ == rownum)
                {
                    return this._dataSetProvider.Find((string) dataRowView.Row["ID"]);
                }
            }
            return null;
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
    }
}
