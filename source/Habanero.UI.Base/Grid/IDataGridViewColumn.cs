// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System.ComponentModel;

namespace Habanero.UI.Base
{
    /// <summary>Defines how a <see cref="IDataGridView"></see> column can be sorted by the user.</summary>
    /// <filterpriority>2</filterpriority>
    //[Serializable()]
    public enum DataGridViewColumnSortMode
    {
        /// <summary>
        /// The column can only be sorted programmatically, but it is not intended for sorting, 
        /// so the column header will not include space for a sorting glyph.
        /// </summary>
        NotSortable,
        /// <summary>
        /// The user can sort the column by clicking the column header unless the
        /// column headers are used for selection. A sorting glyph will be displayed automatically.
        /// </summary>
        Automatic,
        /// <summary>
        /// The column can only be sorted programmatically, and the column header will include space for a sorting glyph.
        /// </summary>
        Programmatic
    }

    /// <summary>
    /// Represents a column in a DataGridView control
    /// </summary>
    public interface IDataGridViewColumn
    {
        ///<summary>
        /// Returns the underlying control being wrapped by this decorator.
        ///</summary>
        object Control { get; }

        /// <summary>Gets or sets the name of the data source property or database column to which 
        /// the <see cref="IDataGridViewColumn"></see> is bound.</summary>
        /// <returns>The name of the property or database column associated with the <see cref="IDataGridViewColumn"></see>.</returns>
        /// <filterpriority>1</filterpriority>
        [DefaultValue(""),TypeConverter("IForms.Design.DataMemberFieldConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), Browsable(true)]
        string DataPropertyName { get; set; }

        /// <summary>Gets or sets the caption text on the column's header cell.</summary>
        /// <returns>A <see cref="T:System.String"></see> with the desired text. The default is an empty string ("").</returns>
        /// <filterpriority>1</filterpriority>
        [RefreshProperties(RefreshProperties.Repaint), Localizable(true)]
        string HeaderText { get; set; }

        /// <summary>Gets or sets the name of the column.</summary>
        /// <returns>A <see cref="T:System.String"></see> that contains the name of the column. The default is an empty string ("").</returns>
        /// <filterpriority>1</filterpriority>
        [Browsable(false)]
        string Name { get; set; }

        /// <summary>Gets or sets a value indicating whether the user can edit the column's cells.</summary>
        /// <returns>true if the user cannot edit the column's cells; otherwise, false.</returns>
        /// <exception cref="T:System.InvalidOperationException">This property is set to false for 
        /// a column that is bound to a read-only data source. </exception>
        /// <filterpriority>1</filterpriority>
        bool ReadOnly { get; set; }

        /// <summary>Gets or sets the sort mode for the column.</summary>
        /// <returns>A <see cref="DataGridViewColumnSortMode"></see> that specifies the criteria used 
        /// to order the rows based on the cell values in a column.</returns>
        /// <exception cref="System.InvalidOperationException">The value assigned to the property 
        /// conflicts with SelectionMode. </exception>
        /// <filterpriority>1</filterpriority>
        [DefaultValue(0)]
        DataGridViewColumnSortMode SortMode { get; set; }

        /// <summary>Gets or sets the text used for ToolTips.</summary>
        /// <returns>The text to display as a ToolTip for the column.</returns>
        /// <filterpriority>1</filterpriority>
        string ToolTipText { get; set; }

        /// <summary>Gets or sets the data type of the values in the column's cells.</summary>
        /// <returns>A <see cref="T:System.Type"></see> that describes the run-time class of the values stored in the column's cells.</returns>
        /// <filterpriority>1</filterpriority>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false),
         DefaultValue((string) null)]
        System.Type ValueType { get; set; }

        /// <summary>Gets or sets the current width of the column.</summary>
        /// <returns>The width, in pixels, of the column. The default is 100.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The specified value when setting 
        /// this property is greater than 65536.</exception>
        /// <filterpriority>1</filterpriority>
        [RefreshProperties(RefreshProperties.Repaint), Localizable(true)]
        int Width { get; set; }

        /// <summary>Gets or sets a value indicating whether the column is visible.</summary>
        /// <returns>true if the column is visible; otherwise, false.</returns>
        bool Visible { get; set; }

        /// <summary>Gets or sets the column's default cell style.</summary>
        /// <returns>A <see cref="IDataGridViewCellStyle"></see> that represents the default style of the cells in the column.</returns>
        /// <filterpriority>1</filterpriority>
        [Browsable(true)]
        IDataGridViewCellStyle DefaultCellStyle { get; set; }
    }
}