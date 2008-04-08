//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Habanero.UI.Grid
{
    /// <summary>
    /// Manages a cell that holds a calendar date
    /// </summary>
    public class NumericUpDownCell : DataGridViewTextBoxCell
    {
        /// <summary>
        /// Constructor to initialise a new cell, using the short date format
        /// </summary>
        public NumericUpDownCell()
            : base()
        {

        }

        /// <summary>
        /// Initialises the editing control
        /// </summary>
        /// <param name="rowIndex">The row index number</param>
        /// <param name="initialFormattedValue">The initial value</param>
        /// <param name="dataGridViewCellStyle">The cell style</param>
        public override void InitializeEditingControl(int rowIndex, object
                                                                        initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                                          dataGridViewCellStyle);
            NumericUpDownEditingControl ctl =
                DataGridView.EditingControl as NumericUpDownEditingControl;

            if (this.Value == null)
            {
                ctl.Value = 0;
            }
            else
            {
                if (this.Value.ToString() != "")
                    ctl.Value = Decimal.Parse(this.Value.ToString());
            }
        }

        /// <summary>
        /// Returns the type of editing control that is used
        /// </summary>
        public override Type EditType
        {
            get
            {
                return typeof(NumericUpDownEditingControl);
            }
        }

        /// <summary>
        /// Returns the type of value contained in the cell
        /// </summary>
        public override Type ValueType
        {
            get
            {
                return typeof(Decimal);
            }
        }

        /// <summary>
        /// Returns the default value for a new row, which in this case is
        /// the current date and time
        /// </summary>
        public override object DefaultNewRowValue
        {
            get
            {
                return 0D;
            }
        }
    }
}