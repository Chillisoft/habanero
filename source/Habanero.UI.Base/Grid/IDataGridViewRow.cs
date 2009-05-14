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

using System.ComponentModel;

namespace Habanero.UI.Base  
{
    /// <summary>
    /// Represents a row in a DataGridView control
    /// </summary>
    public interface IDataGridViewRow
    {
        ///// <summary>Gets the error text for the row at the specified index.</summary>
        ///// <returns>A string that describes the error of the row at the specified index.</returns>
        ///// <param name="rowIndex">The index of the row that contains the error.</param>
        ///// <exception cref="T:System.InvalidOperationException">The row belongs to a <see cref="IDataGridView"></see> control and is a shared row.</exception>
        ///// <exception cref="T:System.ArgumentOutOfRangeException">The row belongs to a <see cref="IDataGridView"></see> control and rowIndex is less than zero or greater than the number of rows in the control minus one. </exception>
        //string GetErrorText(int rowIndex);

        ///// <summary>Calculates the ideal height of the specified row based on the specified criteria.</summary>
        ///// <returns>The ideal height of the row, in pixels.</returns>
        ///// <param name="autoSizeRowMode">A <see cref="IDataGridViewAutoSizeRowMode"></see> that specifies an automatic sizing mode.</param>
        ///// <param name="rowIndex">The index of the row whose preferred height is calculated.</param>
        ///// <param name="fixedWidth">true to calculate the preferred height for a fixed cell width; otherwise, false.</param>
        ///// <exception cref="T:System.ArgumentOutOfRangeException">The rowIndex is not in the valid range of 0 to the number of rows in the control minus 1. </exception>
        ///// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">autoSizeRowMode is not a valid <see cref="IDataGridViewAutoSizeRowMode"></see> value. </exception>
        //int GetPreferredHeight(int rowIndex, DataGridViewAutoSizeRowMode autoSizeRowMode, bool fixedWidth);

        ///// <summary>Sets the values of the row's cells.</summary>
        ///// <returns>true if all values have been set; otherwise, false.</returns>
        ///// <param name="values">One or more objects that represent the cell values in the row.-or-An <see cref="T:System.Array"></see> of <see cref="T:System.Object"></see> values. </param>
        ///// <exception cref="T:System.ArgumentNullException">values is null. </exception>
        ///// <exception cref="T:System.InvalidOperationException">This method is called when the associated <see cref="IDataGridView"></see> is operating in virtual mode. -or-This row is a shared row.</exception>
        ///// <filterpriority>1</filterpriority>
        //bool SetValues(params object[] values);

        /// <summary>Gets the collection of cells that populate the row.</summary>
        /// <returns>A <see cref="IDataGridViewCellCollection"></see> that contains all of the cells in the row.</returns>
        /// <filterpriority>1</filterpriority>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        IDataGridViewCellCollection Cells { get; }

        /// <summary>Gets the data-bound object that populated the row.</summary>
        /// <returns>The data-bound <see cref="T:System.Object"></see>.</returns>
        /// <filterpriority>1</filterpriority>
        [EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false)]
        object DataBoundItem { get; }

        ///// <summary>Gets or sets a value indicating whether the row is frozen. </summary>
        ///// <returns>true if the row is frozen; otherwise, false.</returns>
        ///// <exception cref="T:System.InvalidOperationException">The row is in a <see cref="IDataGridView"></see> control and is a shared row.</exception>
        ///// <filterpriority>1</filterpriority>
        //[Browsable(false)]
        //bool Frozen { get; set; }

        ///// <summary>Gets or sets the current height of the row.</summary>
        ///// <returns>The height, in pixels, of the row. The default is the height of the default font plus 9 pixels.</returns>
        ///// <exception cref="T:System.InvalidOperationException">When setting this property, the row is in a <see cref="IDataGridView"></see> control and is a shared row.</exception>
        ///// <filterpriority>1</filterpriority>
        //int Height { get; set; }

        ///// <summary>Gets a value indicating whether the row is the row for new records.</summary>
        ///// <returns>true if the row is the last row in the <see cref="IDataGridView"></see>, which is used for the entry of a new row of data; otherwise, false.</returns>
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //bool IsNewRow { get; }

        ///// <summary>Gets or sets the minimum height of the row.</summary>
        ///// <returns>The minimum row height in pixels, ranging from 2 to <see cref="F:System.Int32.MaxValue"></see>. The default is 3.</returns>
        ///// <exception cref="T:System.InvalidOperationException">When setting this property, the row is in a <see cref="IDataGridView"></see> control and is a shared row.</exception>
        ///// <exception cref="T:System.ArgumentOutOfRangeException">The specified value when setting this property is less than 2.</exception>
        ///// <filterpriority>1</filterpriority>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        //int MinimumHeight { get; set; }

        /// <summary>Gets or sets a value indicating whether the row is read-only.</summary>
        /// <returns>true if the row is read-only; otherwise, false.</returns>
        /// <exception cref="T:System.InvalidOperationException">The row is in a <see cref="IDataGridView"></see> control and is a shared row.</exception>
        /// <filterpriority>1</filterpriority>
        bool ReadOnly { get; set; }

        /// <summary>Gets or sets a value indicating whether the row is selected. </summary>
        /// <returns>true if the row is selected; otherwise, false.</returns>
        /// <exception cref="T:System.InvalidOperationException">The row is in a <see cref="IDataGridView"></see>
        ///  control and is a shared row.</exception>
        bool Selected { get; set; }

        /// <summary>
        /// Gets the relative position of the row within the DataGridView control
        /// </summary>
        int Index { get; }

        /// <summary>Gets a value indicating whether this row is displayed on the screen.</summary>
        /// <returns>true if the row is currently displayed on the screen; otherwise, false.</returns>
        /// <exception cref="T:System.InvalidOperationException">The row is in a <see cref="IDataGridView"></see> control and is a shared row.</exception>
        [Browsable(false)]
        bool Displayed { get; }

 
        /// <summary>Gets or sets a value indicating whether the row is visible. </summary>
        /// <returns>true if the row is visible; otherwise, false.</returns>
        /// <exception cref="T:System.InvalidOperationException">The row is in a <see cref="IDataGridView"></see>
        ///  control and is a shared row.</exception>
        /// <filterpriority>1</filterpriority>
        [Browsable(false)]
        bool Visible { get; set; }

        /// <summary>Sets the values of the row's cells.</summary>
        /// <returns>true if all values have been set; otherwise, false.</returns>
        /// <param name="values">One or more objects that represent the cell values in the row.-or-An
        ///  <see cref="T:System.Array"></see> of <see cref="T:System.Object"></see> values. </param>
        /// <exception cref="T:System.ArgumentNullException">values is null. </exception>
        /// <exception cref="T:System.InvalidOperationException">This method is called when the associated 
        /// <see cref="IDataGridView"></see> is operating in virtual mode. -or-This row is a shared row.</exception>
        /// <filterpriority>1</filterpriority>
        bool SetValues(params object[] values);

        /// <summary>Gets and sets a tag value for this <see cref="IDataGridViewRow"/></summary>
        [Browsable(false)]
        object Tag { get; set; }
    }
}