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

using System.Collections;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Represents a column of DataGridViewComboBoxCell objects
    /// </summary>
    public class DataGridViewComboBoxColumnWin : DataGridViewColumnWin, IDataGridViewComboBoxColumn
    {
        private readonly DataGridViewComboBoxColumn _dataGridViewComboBoxColumn;

        ///<summary>
        /// Constructor for <see cref="DataGridViewComboBoxCell"/>
        ///</summary>
        ///<param name="dataGridViewComboBoxColumn"></param>
        public DataGridViewComboBoxColumnWin(DataGridViewComboBoxColumn dataGridViewComboBoxColumn)
            : base(dataGridViewComboBoxColumn)
        {
            _dataGridViewComboBoxColumn = dataGridViewComboBoxColumn;
        }

        //public IComboBoxObjectCollection Items
        //{
        //    get { return (IComboBoxObjectCollection)_dataGridViewComboBoxColumn.Items; }
        //}

        /// <summary>
        /// Gets or sets the data source that populates the selections for the combo boxes
        /// </summary>
        public object DataSource
        {
            get { return _dataGridViewComboBoxColumn.DataSource; }
            set { _dataGridViewComboBoxColumn.DataSource = value; }
        }

        /// <summary>
        /// Gets or sets a string that specifies the property or column from
        /// which to get values that correspond to the selections in the drop-down list.
        /// </summary>
        public string ValueMember
        {
            get { return _dataGridViewComboBoxColumn.ValueMember; }
            set { _dataGridViewComboBoxColumn.ValueMember = value; }
        }

        /// <summary>
        /// Gets or sets a string that specifies the property or column from which to
        ///  retrieve strings for display in the combo boxes.
        /// </summary>
        public string DisplayMember
        {
            get { return _dataGridViewComboBoxColumn.DisplayMember; }
            set { _dataGridViewComboBoxColumn.DisplayMember = value; }
        }

        /// <summary>Gets the collection of objects used as selections in the combo boxes.</summary>
        /// <returns>An <see cref="IList"></see> that represents the selections in the combo boxes. </returns>
        public IList Items
        {
            get { return _dataGridViewComboBoxColumn.Items; }
        }
    }
}