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
using System.Windows.Forms;

namespace Habanero.UI.Grid
{
    /// <summary>
    /// A control for editing date and time values
    /// </summary>
    class NumericUpDownEditingControl : NumericUpDown, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        /// <summary>
        /// Constructor to initialise a new editing control with the short
        /// date format
        /// </summary>
        public NumericUpDownEditingControl()
        {
            this.DecimalPlaces = 2;
            this.Minimum = Decimal.MinValue;
            this.Maximum = Decimal.MaxValue;
        }

        /// <summary>
        /// Gets and sets the value being held in the control
        /// </summary>
        public object EditingControlFormattedValue
        {
            get
            {
                return this.Value.ToString("#,##0.00");
            }
            set
            {
                if (value is String)
                {
                    this.Value = Decimal.Parse((String)value);
                }
            }
        }

        /// <summary>
        /// Returns the value being held in the control
        /// </summary>
        /// <returns>Returns the value being held</returns>
        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        /// <summary>
        /// Copy the styles from the object provided across to this editing
        /// control
        /// </summary>
        /// <param name="dataGridViewCellStyle">The source to copy from</param>
        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
        }

        /// <summary>
        /// Gets and sets the row index number
        /// </summary>
        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        /// <summary>
        /// Indicates if the editing control wants the input key specified
        /// </summary>
        /// <param name="key">The key in question</param>
        /// <param name="dataGridViewWantsInputKey">Whether the DataGridView
        /// wants the input key</param>
        /// <returns>Returns true if so, false if not</returns>
        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {
            // Let the DateTimePicker handle the keys listed.
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Prepares the editing control for editing
        /// </summary>
        /// <param name="selectAll">Whether to select all the content first,
        /// which can make it easier to replace as you type</param>
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            // No preparation needs to be done.
        }

        /// <summary>
        /// Indicates whether the control should be repositioned when there
        /// is a value change
        /// </summary>
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets and sets the DataGridView object referenced in this
        /// control
        /// </summary>
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        /// <summary>
        /// Gets and sets the boolean which indicates whether the value
        /// held in the control has changed
        /// </summary>
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        /// <summary>
        /// Returns the Cursor object from the editing panel
        /// </summary>
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        /// <summary>
        /// A handler to carry out repercussions of a changed value
        /// </summary>
        /// <param name="eventargs">Arguments relating to the event</param>
        protected override void OnValueChanged(EventArgs eventargs)
        {
            // Notify the DataGridView that the contents of the cell
            // have changed.
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
        }
    }
}