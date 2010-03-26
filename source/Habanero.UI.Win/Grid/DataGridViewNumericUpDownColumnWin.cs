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
using System;
using System.Windows.Forms;
using Habanero.UI.Base;
using Habanero.UI.Win;

namespace Habanero.UI
{

    /// <summary>
    /// Hosts a collection of cells that support a NumericUpDown
    /// </summary>
    public class DataGridViewNumericUpDownColumnWin : DataGridViewColumnWin, IDataGridViewNumericUpDownColumn
    {
        ///<summary>
        /// Initialises the DataGridView NumericUpDown Column with the windows implementation
        /// of the DataGridViewNumericUpDownColumn to wrap for this implementation.
        ///</summary>
        ///<param name="dataGridViewColumn">The DataGridViewNumericUpDownColumn to wrap</param>
        public DataGridViewNumericUpDownColumnWin(DataGridViewNumericUpDownColumn dataGridViewColumn)
            : base(dataGridViewColumn)
        {
        }
    }

    /// <summary>
    /// Represents a numeric column in data grid view
    /// </summary>
    public class DataGridViewNumericUpDownColumn : DataGridViewColumn
    {
        /// <summary>
        /// Constructor to initialise a new column
        /// </summary>
        public DataGridViewNumericUpDownColumn()
            : base(new NumericUpDownCell())
        {
        }

        /// <summary>
        /// Gets and sets the cell template
        /// </summary>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a NumericUpDownCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(NumericUpDownCell)))
                {
                    throw new InvalidCastException("Must be a NumericUpDownCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    /// <summary>
    /// Manages a cell that holds a numeric value
    /// </summary>
    public class NumericUpDownCell : DataGridViewTextBoxCell
    {
        /// <summary>
        /// Constructor to initialise a new cell
        /// </summary>
        public NumericUpDownCell() : base()
        {
            // Format with 2 decimal places
            this.Style.Format = "0.00";
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
        /// Returns the default value for a new row, which in this case is zero
        /// </summary>
        public override object DefaultNewRowValue
        {
            get
            {
                return (Decimal)0;
            }
        }
    }

    /// <summary>
    /// A control for editing numeric values
    /// </summary>
    class NumericUpDownEditingControl : NumericUpDown, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        /// <summary>
        /// Constructor to initialise a new editing control with the 
        /// default numeric format of 2 decimal places.
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
        /// Returns the value being held in the editing control
        /// </summary>
        /// <returns>Returns the current editing control value.</returns>
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
            // Let the NumericUpDown handle the keys listed.
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