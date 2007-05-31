using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
using log4net;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Serves as a super-class for grid implementations
    /// </summary>
    /// TODO ERIC - clarify on STA threads?
    public abstract class GridBase : DataGridView
    {
        private delegate void SetGridDataProviderDelegate(IGridDataProvider provider);

        private delegate void SetSortColumnDelegate(string columnName, bool isAscending);

        private static readonly ILog log = LogManager.GetLogger("Chillisoft.UI.Generic.v2.GridBase");
        protected DataTable itsDataTable;
        protected BusinessObjectCollectionDataSetProvider itsDataSetProvider;
        protected BusinessObjectBaseCollection itsCollection;
        private IObjectInitialiser itsObjectInitialiser;
        private IGridDataProvider itsProvider;
        protected DataView itsDataTableDefaultView;
        private SetGridDataProviderDelegate setGridDataProvider;
        private SetSortColumnDelegate setSortColumn;
        //private DelayedMethodCall filterUpdatedMethodCaller;

        public event EventHandler DataProviderUpdated;
        public event EventHandler FilterUpdated;

        /// <summary>
        /// Constructor to initialise a new instance
        /// </summary>
        protected GridBase()
        {
            //filterUpdatedMethodCaller = new DelayedMethodCall(1000) ;
            setGridDataProvider = new SetGridDataProviderDelegate(SetGridDataProviderInSTAThread);
            setSortColumn = new SetSortColumnDelegate(SetSortColumnInSTAThread);
        }

        /// <summary>
        /// Sets the grid's data provider to that specified
        /// </summary>
        /// <param name="provider">The grid data provider</param>
        public void SetGridDataProvider(IGridDataProvider provider)
        {
            try
            {
                BeginInvoke(setGridDataProvider, new object[] {provider});
                // needed to do the call on the Forms thread.  See info about STA thread model.
            }
            catch (InvalidOperationException)
            {
                this.SetGridDataProviderInSTAThread(provider);
            }
        }

        /// <summary>
        /// Sets the grid's data provider to that specified, but using a
        /// STA thread.  This method is called by SetGridDataProvider().
        /// </summary>
        /// <param name="provider">The grid data provider</param>
        private void SetGridDataProviderInSTAThread(IGridDataProvider provider)
        {
            itsProvider = provider;
            itsCollection = itsProvider.GetCollection();
            itsDataSetProvider = CreateBusinessObjectCollectionDataSetProvider(itsCollection);
            itsDataSetProvider.ObjectInitialiser = itsObjectInitialiser;
            itsDataTable = itsDataSetProvider.GetDataTable(itsProvider.GetUIGridDef());
            itsDataTable.TableName = "Table";

            this.Columns.Clear();

            DataGridViewColumn col = new DataGridViewTextBoxColumn(); // DataGridViewTextBoxColumn();
            //col.Visible = false;
            col.Width = 0;            
            this.Columns.Add(col);
            this.Columns[0].Visible = false;
            int colNum = 1;
            foreach (UIGridProperty gridProp in provider.GetUIGridDef())
            {
                DataColumn dataColumn = itsDataTable.Columns[colNum];

                if (gridProp.GridControlType == typeof (DataGridViewComboBoxColumn))
                {
                    DataGridViewComboBoxColumn comboBoxCol = new DataGridViewComboBoxColumn();
                    ILookupListSource source =
                        (ILookupListSource) itsDataTable.Columns[colNum].ExtendedProperties["LookupListSource"];
                    DataTable table = new DataTable();
                    table.Columns.Add("id");
                    table.Columns.Add("str");

                    table.LoadDataRow(new object[] { "", "" }, true);            
                    foreach (StringGuidPair item in source.GetLookupList())
                    {
                        table.LoadDataRow(new object[] {item.Id.ToString("B"), item.Str}, true);
                    }
                    comboBoxCol.DataSource = table;
                    comboBoxCol.ValueMember = "str";
                    comboBoxCol.DisplayMember = "str";
                    comboBoxCol.DataPropertyName = dataColumn.ColumnName;
                    col = comboBoxCol;
                }
                else if (gridProp.GridControlType == typeof (DataGridViewCheckBoxColumn))
                {
                    DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
                    col = checkBoxCol;
                }
                else if (gridProp.GridControlType == typeof(DataGridViewDateTimeColumn))
                {
                    DataGridViewDateTimeColumn dateTimeCol = new DataGridViewDateTimeColumn();
                    col = dateTimeCol;
                    
                }
                else
                {
                    col = (DataGridViewColumn) Activator.CreateInstance(gridProp.GridControlType);
                }
                col.Width = (int) (dataColumn.ExtendedProperties["Width"]);
                col.ReadOnly = gridProp.IsReadOnly;
                col.HeaderText = dataColumn.Caption;
                col.Name = dataColumn.ColumnName;
                col.DataPropertyName = dataColumn.ColumnName;
                //col.MappingName = dataColumn.ColumnName;

                switch (gridProp.Alignment)
                {
                    case UIGridProperty.PropAlignment.centre:
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                    case UIGridProperty.PropAlignment.left:
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        break;
                    case UIGridProperty.PropAlignment.right:
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        break;
                }
                this.Columns.Add(col);
                colNum++;
            }

           itsDataTableDefaultView = itsDataTable.DefaultView;

            this.AutoGenerateColumns = false;
            this.DataSource = itsDataTableDefaultView;

            //this.DataSource = itsDataTable;
            FireDataProviderUpdated();
        }

        /// <summary>
        /// Calls the DataProviderUpdated() method, passing this instance
        /// as the sender
        /// </summary>
        private void FireDataProviderUpdated()
        {
            if (this.DataProviderUpdated != null)
            {
                this.DataProviderUpdated(this, new EventArgs());
            }
        }

        /// <summary>
        /// Adds a business object to the collection being represented
        /// </summary>
        /// <param name="bo">The business object</param>
        public void AddBusinessObject(BusinessObjectBase bo)
        {
            itsCollection.Add(bo);
            int row = GetRowOfBusinessObject(bo);
            this.SetSelectedRowCore(row, true);
            //this.CurrentRowIndex = row ;
            //this.Select(row) ;
        }

        /// <summary>
        /// Creates a data set provider for the business object collection
        /// provided
        /// </summary>
        /// <param name="col">The business object collection</param>
        /// <returns>Returns the new provider</returns>
        protected abstract BusinessObjectCollectionDataSetProvider CreateBusinessObjectCollectionDataSetProvider(
            BusinessObjectBaseCollection col);

        /// <summary>
        /// Sets the object initialiser
        /// </summary>
        public virtual IObjectInitialiser ObjectInitialiser
        {
            set { itsObjectInitialiser = value;
            if (itsDataSetProvider != null)
            {
                itsDataSetProvider.ObjectInitialiser = itsObjectInitialiser;
            }
        }
        }

        /// <summary>
        /// Returns the data table
        /// </summary>
        public DataTable DataTable
        {
            get { return itsDataTable; }
        }

        /// <summary>
        /// Clears the business object collection and the rows in the data table
        /// </summary>
        public void Clear()
        {
            if (itsCollection != null)
            {
                itsCollection.Clear();
                itsDataTable.Rows.Clear();
            }
        }

        /// <summary>
        /// Applies a filter clause to the data table and updates the filter
        /// </summary>
        /// <param name="filterClause">The filter clause</param>
        public void ApplyFilter(FilterClause filterClause)
        {
            itsDataTableDefaultView.RowFilter = filterClause.GetFilterClauseString();
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
        protected virtual BusinessObjectBase GetSelectedBusinessObject()
        {
            if (this.CurrentCell == null) return null;
            int rownum = this.CurrentCell.RowIndex;
            int i = 0;
            foreach (DataRowView dataRowView in itsDataTableDefaultView)
            {
                
                if (i++ == rownum)
                {
                    return this.itsDataSetProvider.Find((string) dataRowView.Row["ID"]);
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the selected business objects as an IList
        /// </summary>
        /// <returns>Returns an IList object</returns>
        protected virtual IList GetSelectedBusinessObjects()
        {
            IList busObjects = new ArrayList();
            foreach (DataGridViewRow row in this.SelectedRows)
            {
                busObjects.Add(this.itsDataSetProvider.Find((string) row.Cells["ID"].Value));
            }
            //for (int i = 0; i < itsDataTableDefaultView.Count; i++ ) {
            //    if (this.IsSelected(i)) {
            //        int j = 0;
            //        foreach (DataRowView dataRowView in itsDataTableDefaultView) 
            //        {
            //            if (j++ == i) 
            //            {
            //                busObjects.Add(this.itsDataSetProvider.Find((string)dataRowView.Row["ID"])) ;
            //            }
            //        }
            //    }						 
            //}
            return busObjects;
        }

        /// <summary>
        /// Returns the row number that contains the specified business
        /// object
        /// </summary>
        /// <param name="bo">The business object in question</param>
        /// <returns>Returns the row number if found, or -1 if not found</returns>
        public virtual int GetRowOfBusinessObject(BusinessObjectBase bo)
        {
            int rownum = 0;

            foreach (DataRowView dataRowView in itsDataTableDefaultView)
            {
                if (this.itsDataSetProvider.Find((string) dataRowView.Row["ID"]) == bo)
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
        public BusinessObjectBase GetBusinessObjectAtRow(int row)
        {
            int i = 0;
            foreach (DataRowView dataRowView in itsDataTableDefaultView)
            {
                if (i++ == row)
                {
                    return this.itsDataSetProvider.Find((string)dataRowView.Row["ID"]);
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
                BeginInvoke(setSortColumn, new object[] {columnName, isAscending});
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
                itsDataTableDefaultView.Sort = columnName + " ASC";
            }
            else
            {
                itsDataTableDefaultView.Sort = columnName + " DESC";
            }
        }
    }
}