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

using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class DataGridViewCheckBoxColumnGiz : DataGridViewColumnGiz, IDataGridViewCheckBoxColumn
    {
        private readonly DataGridViewCheckBoxColumn _dataGridViewCheckBoxColumn;

        public DataGridViewCheckBoxColumnGiz(DataGridViewCheckBoxColumn dataGridViewColumn) : base(dataGridViewColumn)
        {
            _dataGridViewCheckBoxColumn = dataGridViewColumn;
        }

        /// <summary>Gets or sets the underlying value corresponding to a cell value of false, which appears as an unchecked box.</summary>
        /// <returns>An <see cref="T:System.Object"></see> representing a value that the cells in this column will treat as a false value. The default is null.</returns>
        /// <exception cref="T:System.InvalidOperationException">The value of the <see cref="IDataGridViewCheckBoxColumn.CellTemplate"></see> property is null. </exception>
        /// <filterpriority>1</filterpriority>
        public object FalseValue
        {
            get { return _dataGridViewCheckBoxColumn.FalseValue; }
            set { _dataGridViewCheckBoxColumn.FalseValue = value; }
        }

        /// <summary>Gets or sets the underlying value corresponding to an indeterminate or null cell value, which appears as a disabled checkbox.</summary>
        /// <returns>An <see cref="T:System.Object"></see> representing a value that the cells in this column will treat as an indeterminate value. The default is null.</returns>
        /// <exception cref="T:System.InvalidOperationException">The value of the <see cref="IDataGridViewCheckBoxColumn.CellTemplate"></see> property is null. </exception>
        /// <filterpriority>1</filterpriority>
        public object IndeterminateValue
        {
            get { return _dataGridViewCheckBoxColumn.IndeterminateValue; }
            set { _dataGridViewCheckBoxColumn.IndeterminateValue = value; }
        }

        /// <summary>Gets or sets a value indicating whether the hosted check box cells will allow three check states rather than two.</summary>
        /// <returns>true if the hosted DataGridViewCheckBoxCell" objects are able to have a third, indeterminate, state; otherwise, false. The default is false.</returns>
        /// <exception cref="T:System.InvalidOperationException">The value of the DataGridViewCheckBoxColumn.CellTemplate property is null.</exception>
        /// <filterpriority>1</filterpriority>
        public bool ThreeState
        {
            get { return _dataGridViewCheckBoxColumn.ThreeState; }
            set { _dataGridViewCheckBoxColumn.ThreeState = value; }
        }

        /// <summary>Gets or sets the underlying value corresponding to a cell value of true, which appears as a checked box.</summary>
        /// <returns>An <see cref="T:System.Object"></see> representing a value that the cell will treat as a true value. The default is null.</returns>
        /// <exception cref="T:System.InvalidOperationException">The value of the DataGridViewCheckBoxColumn.CellTemplate property is null.</exception>
        /// <filterpriority>1</filterpriority>
        public object TrueValue
        {
            get { return _dataGridViewCheckBoxColumn.TrueValue; }
            set { _dataGridViewCheckBoxColumn.TrueValue = value; }
        }
    }
}