using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Habanero.Ui.Grid
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