using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Habanero.UI.Base;
using DataGridViewColumnSortMode=Habanero.UI.Base.DataGridViewColumnSortMode;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a Windows Forms version of the DataGridViewColumn
    /// </summary>
    public class DataGridViewColumnWin : IDataGridViewColumn
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

        /// <summary>Gets or sets the name of the data source property or database column to which the <see cref="IDataGridViewColumn"></see> is bound.</summary>
        /// <returns>The name of the property or database column associated with the <see cref="IDataGridViewColumn"></see>.</returns>
        /// <filterpriority>1</filterpriority>
        //Editor(
        //    "Gizmox.WebGUI.Forms.Design.DataGridViewColumnDataPropertyNameEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
        //    , typeof (UITypeEditor)), Gizmox.WebGUI.Forms.SRDescription("DataGridView_ColumnDataPropertyNameDescr"),
        //DefaultValue(""),
        //TypeConverter(
        //    "IForms.Design.DataMemberFieldConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
        //    ), Browsable(true)]
        public string DataPropertyName
        {
            get { return _dataGridViewColumn.DataPropertyName; }
            set { _dataGridViewColumn.DataPropertyName = value; }
        }

        /// <summary>Gets or sets the caption text on the column's header cell.</summary>
        /// <returns>A <see cref="T:System.String"></see> with the desired text. The default is an empty string ("").</returns>
        /// <filterpriority>1</filterpriority>
        public string HeaderText
        {
            get { return _dataGridViewColumn.HeaderText; }
            set { _dataGridViewColumn.HeaderText = value; }
        }

        /// <summary>Gets or sets the name of the column.</summary>
        /// <returns>A <see cref="T:System.String"></see> that contains the name of the column. The default is an empty string ("").</returns>
        /// <filterpriority>1</filterpriority>
        public string Name
        {
            get { return _dataGridViewColumn.Name; }
            set { _dataGridViewColumn.Name = value; }
        }

        /// <summary>Gets or sets a value indicating whether the user can edit the column's cells.</summary>
        /// <returns>true if the user cannot edit the column's cells; otherwise, false.</returns>
        /// <exception cref="T:System.InvalidOperationException">This property is set to false for a column that is bound to a read-only data source. </exception>
        /// <filterpriority>1</filterpriority>
        public bool ReadOnly
        {
            get { return _dataGridViewColumn.ReadOnly; }
            set { _dataGridViewColumn.ReadOnly = value; }
        }

        /// <summary>Gets or sets the sort mode for the column.</summary>
        /// <returns>A <see cref="Base.DataGridViewColumnSortMode"></see> that specifies the criteria used to order the rows based on the cell values in a column.</returns>
        /// <exception cref="System.InvalidOperationException">The value assigned to the property conflicts with <see cref="SelectionMode"></see>. </exception>
        /// <filterpriority>1</filterpriority>
        public DataGridViewColumnSortMode SortMode
        {
            get { return (DataGridViewColumnSortMode)_dataGridViewColumn.SortMode; }
            set { _dataGridViewColumn.SortMode = (System.Windows.Forms.DataGridViewColumnSortMode)value; }
        }

        /// <summary>Gets or sets the text used for ToolTips.</summary>
        /// <returns>The text to display as a ToolTip for the column.</returns>
        /// <filterpriority>1</filterpriority>
        public string ToolTipText
        {
            get { return _dataGridViewColumn.ToolTipText; }
            set { _dataGridViewColumn.ToolTipText = value; }
        }

        /// <summary>Gets or sets the data type of the values in the column's cells.</summary>
        /// <returns>A <see cref="T:System.Type"></see> that describes the run-time class of the values stored in the column's cells.</returns>
        /// <filterpriority>1</filterpriority>
        public Type ValueType
        {
            get { return _dataGridViewColumn.ValueType; }
            set { _dataGridViewColumn.ValueType = value; }
        }

        /// <summary>Gets or sets the current width of the column.</summary>
        /// <returns>The width, in pixels, of the column. The default is 100.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The specified value when setting this property is greater than 65536.</exception>
        /// <filterpriority>1</filterpriority>
        public int Width
        {
            get { return _dataGridViewColumn.Width; }
            set { _dataGridViewColumn.Width = value; }
        }

        public bool Visible
        {
            get { return _dataGridViewColumn.Visible; }
            set { _dataGridViewColumn.Visible = value; }
        }

        /// <summary>Gets or sets the column's default cell style.</summary>
        /// <returns>A <see cref="IDataGridViewCellStyle"></see> that represents the default style of the cells in the column.</returns>
        /// <filterpriority>1</filterpriority>
        public IDataGridViewCellStyle DefaultCellStyle
        {
            get { return new DataGridViewCellStyleWin(_dataGridViewColumn.DefaultCellStyle); }
            set { throw new NotImplementedException(); }
        }
    }
}
