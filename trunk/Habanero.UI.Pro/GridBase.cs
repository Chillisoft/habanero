using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using CoreBiz.Bo;

namespace CoreBiz.UI.Application
{
	/// <summary>
	/// Summary description for GridBase.
	/// </summary>
	public abstract class GridBase : DataGrid
	{
		protected DataTable itsDataTable;
		protected BusinessObjectCollectionDataSetProvider itsDataSetProvider;
		protected BusinessObjectBaseCollection itsCollection;

		public void SetBusinessObjectCollection(BusinessObjectBaseCollection col) 
		{
			itsCollection = col;
			itsDataSetProvider = CreateBusinessObjectCollectionDataSetProvider(col);
			itsDataTable = itsDataSetProvider.GetDataTable() ;
			itsDataTable.TableName = "Table";
			
			this.DataSource = itsDataTable;
			
			this.TableStyles.Clear() ;
			DataGridTableStyle tableStyle = new DataGridTableStyle();
			tableStyle.MappingName = "Table";
			foreach (DataColumn gridColumn in itsDataTable.Columns) {
				DataGridColumnStyle style = new DataGridTextBoxColumn();
				style.HeaderText = gridColumn.Caption ;
				style.MappingName = gridColumn.ColumnName ;
				style.Width = 100;
				tableStyle.GridColumnStyles.Add(style);
			}
			
			this.TableStyles.Add(tableStyle);
			if (this.TableStyles["Table"].GridColumnStyles.Count > 0) 
			{
				this.TableStyles["Table"].GridColumnStyles[0].Width = 0;
			}
		}

		protected abstract BusinessObjectCollectionDataSetProvider CreateBusinessObjectCollectionDataSetProvider(BusinessObjectBaseCollection col);
	}
}
