//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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

using System.Drawing;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Hosts a collection of DataGridViewImageCell objects
    /// </summary>
    public class DataGridViewImageColumnWin : DataGridViewColumnWin, IDataGridViewImageColumn
    {
        private readonly DataGridViewImageColumn _dataGridViewColumn;
        /// <summary>
        /// Constructor for <see cref="DataGridViewImageColumnWin"/> 
        /// </summary>
        /// <param name="dataGridViewColumn"></param>
        public DataGridViewImageColumnWin(DataGridViewImageColumn dataGridViewColumn)
            : base(dataGridViewColumn)
        {
            _dataGridViewColumn = dataGridViewColumn;
        }

        //public DataGridViewColumn DataGridViewColumn
        //{
        //    get { return this.DataGridViewColumn; }
        //}

        /// <summary>Gets or sets a string that describes the column's image. </summary>
        /// <returns>The textual description of the column image. The default is <see cref="F:System.String.Empty"></see>.</returns>
        /// <exception cref="T:System.InvalidOperationException">The value of the CellTemplate property is null.</exception>
        /// <filterpriority>1</filterpriority>
        public string Description
        {
            get { return this._dataGridViewColumn.Description; }
            set { this._dataGridViewColumn.Description = value; }
        }

        /// <summary>Gets or sets the icon displayed in the cells of this column when the
        ///  cell's Value property is not set and the cell's ValueIsIcon property is set to true.</summary>
        /// <returns>The <see cref="T:System.Drawing.Icon"></see> to display. The default is null.</returns>
        public Icon Icon
        {
            get { return this._dataGridViewColumn.Icon; }
            set { this._dataGridViewColumn.Icon = value; }
        }

        /// <summary>Gets or sets the image displayed in the cells of this column when the 
        /// cell's Value property is not set and the cell's ValueIsIcon property is set to false.</summary>
        /// <returns>The <see cref="T:System.Drawing.Image"></see> to display. The default is null.</returns>
        /// <filterpriority>1</filterpriority>
        public Image Image
        {
            get { return this._dataGridViewColumn.Image; }
            set { this._dataGridViewColumn.Image = value; }
        }

        /// <summary>Gets or sets a value indicating whether cells in this column display 
        /// <see cref="T:System.Drawing.Icon"></see> values.</summary>
        /// <returns>true if cells display values of type <see cref="T:System.Drawing.Icon"></see>; false 
        /// if cells display values of type <see cref="T:System.Drawing.Image"></see>. The default is false.</returns>
        /// <exception cref="T:System.InvalidOperationException">The value of the CellTemplate property is null.</exception>
        public bool ValuesAreIcons
        {
            get { return this._dataGridViewColumn.ValuesAreIcons; }
            set { this._dataGridViewColumn.ValuesAreIcons = value; }
        }
    }
}