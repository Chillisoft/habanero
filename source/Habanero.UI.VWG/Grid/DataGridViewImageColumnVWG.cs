using System.Drawing;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Hosts a collection of DataGridViewImageCell objects
    /// </summary>
    public class DataGridViewImageColumnVWG : DataGridViewColumnVWG, IDataGridViewImageColumn
    {
        private readonly DataGridViewImageColumn _dataGridViewColumn;

        public DataGridViewImageColumnVWG(DataGridViewImageColumn dataGridViewColumn)
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