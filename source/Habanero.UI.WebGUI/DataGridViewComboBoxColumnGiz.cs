using System;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;
using Habanero.UI.Base.Grid;
using DataGridViewColumnSortMode=Habanero.UI.Base.DataGridViewColumnSortMode;

namespace Habanero.UI.WebGUI
{
    public class DataGridViewComboBoxColumnGiz : DataGridViewColumnGiz, IDataGridViewComboBoxColumn
    {
        private readonly DataGridViewComboBoxColumn _dataGridViewComboBoxColumn;

        public DataGridViewComboBoxColumnGiz(DataGridViewComboBoxColumn dataGridViewComboBoxColumn) : base(dataGridViewComboBoxColumn)
        {
            _dataGridViewComboBoxColumn = dataGridViewComboBoxColumn;
        }
    }
}