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
using System.Windows.Forms;
using Habanero.UI.Base;
using Habanero.UI.Base.Grid;
using DataGridViewColumnSortMode = Habanero.UI.Base.DataGridViewColumnSortMode;

namespace Habanero.UI.Win
{
    public class DataGridViewComboBoxColumnWin : DataGridViewColumnWin, IDataGridViewComboBoxColumn
    {
        private readonly DataGridViewComboBoxColumn _dataGridViewComboBoxColumn;

        public DataGridViewComboBoxColumnWin(DataGridViewComboBoxColumn dataGridViewComboBoxColumn)
            : base(dataGridViewComboBoxColumn)
        {
            _dataGridViewComboBoxColumn = dataGridViewComboBoxColumn;
        }

        //public IComboBoxObjectCollection Items
        //{
        //    get { return (IComboBoxObjectCollection)_dataGridViewComboBoxColumn.Items; }
        //}
        public object DataSource
        {
            get { return _dataGridViewComboBoxColumn.DataSource; }
            set { _dataGridViewComboBoxColumn.DataSource = value; }
        }

        public string ValueMember
        {
            get { return _dataGridViewComboBoxColumn.ValueMember; }
            set { _dataGridViewComboBoxColumn.ValueMember = value; }
        }

        public string DisplayMember
        {
            get { return _dataGridViewComboBoxColumn.DisplayMember; }
            set { _dataGridViewComboBoxColumn.DisplayMember = value; }
        }
    }
}