using System.ComponentModel;

namespace Habanero.UI.Base
{
    public interface IDataGridViewImageColumn : IDataGridViewColumn
    {
        /// <summary>Gets or sets a string that describes the column's image. </summary>
        /// <returns>The textual description of the column image. The default is <see cref="F:System.String.Empty"></see>.</returns>
        /// <exception cref="T:System.InvalidOperationException">The value of the <see cref="IDataGridViewImageColumn.CellTemplate"></see> property is null.</exception>
        /// <filterpriority>1</filterpriority>
        [ Browsable(true),DefaultValue("")]
        string Description { get; set; }

        /// <summary>Gets or sets the icon displayed in the cells of this column when the cell's <see cref="IDataGridViewImageColumn.Value"></see> property is not set and the cell's <see cref="P:Gizmox.WebGUI.Forms.DataGridViewImageCell.ValueIsIcon"></see> property is set to true.</summary>
        /// <returns>The <see cref="T:System.Drawing.Icon"></see> to display. The default is null.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        System.Drawing.Icon Icon { get; set; }

        /// <summary>Gets or sets the image displayed in the cells of this column when the cell's <see cref="IDataGridViewImageColumn.Value"></see> property is not set and the cell's <see cref="P:Gizmox.WebGUI.Forms.DataGridViewImageCell.ValueIsIcon"></see> property is set to false.</summary>
        /// <returns>The <see cref="T:System.Drawing.Image"></see> to display. The default is null.</returns>
        /// <filterpriority>1</filterpriority>
        [DefaultValue((string) null)]
        System.Drawing.Image Image { get; set; }

//        /// <summary>Gets or sets the image layout in the cells for this column.</summary>
//        /// <returns>A <see cref="IDataGridViewImageCellLayout"></see> that specifies the cell layout. The default is <see cref="F:Gizmox.WebGUI.Forms.DataGridViewImageCellLayout.Normal"></see>.</returns>
//        /// <exception cref="T:System.InvalidOperationException">The value of the <see cref="IDataGridViewImageColumn.CellTemplate"></see> property is null. </exception>
//        /// <filterpriority>1</filterpriority>
//        [DefaultValue(1)]
//        DataGridViewImageCellLayout ImageLayout { get; set; }

        /// <summary>Gets or sets a value indicating whether cells in this column display <see cref="T:System.Drawing.Icon"></see> values.</summary>
        /// <returns>true if cells display values of type <see cref="T:System.Drawing.Icon"></see>; false if cells display values of type <see cref="T:System.Drawing.Image"></see>. The default is false.</returns>
        /// <exception cref="T:System.InvalidOperationException">The value of the <see cref="IDataGridViewImageColumn.CellTemplate"></see> property is null.</exception>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        bool ValuesAreIcons { get; set; }
    }
}