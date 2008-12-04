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

using System;
using System.Windows.Forms;
using Habanero.UI.Base;
using Habanero.UI.Win;
using DataGridViewColumnSortMode=Habanero.UI.Base.DataGridViewColumnSortMode;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Hosts a collection of cells that support a DateTimePicker
    /// </summary>
    public class DataGridViewDateTimeColumnWin : DataGridViewColumnWin, IDataGridViewDateTimeColumn
    {
        public DataGridViewDateTimeColumnWin(DataGridViewDateTimeColumn dataGridViewColumn)
            : base(dataGridViewColumn)
        {
        }
    }

    /// <summary>
    /// Implements a column of cells that support a DateTimePicker
    /// </summary>
    public class DataGridViewDateTimeColumn : DataGridViewColumn
    {
        /// <summary>
        /// Constructor to initialise a new column
        /// </summary>
        public DataGridViewDateTimeColumn()
            : base(new CalendarCell())
        {
        }

        /// <summary>
        /// Gets and sets the cell template
        /// </summary>
        public override DataGridViewCell CellTemplate
        {
            get { return base.CellTemplate; }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(CalendarCell)))
                {
                    throw new InvalidCastException("Must be a CalendarCell");
                }
                base.CellTemplate = value;
            }
        }

        /// <summary>Gets or sets the sort mode for the column.</summary>
        /// <returns>A <see cref="DataGridViewColumnSortMode"></see> that specifies the criteria used 
        /// to order the rows based on the cell values in a column.</returns>
        /// <exception cref="System.InvalidOperationException">The value assigned to the property 
        /// conflicts with SelectionMode. </exception>
        /// <filterpriority>1</filterpriority>
        public DataGridViewColumnSortMode SortMode
        {
            get { return (DataGridViewColumnSortMode)base.SortMode; }
            set { base.SortMode = (System.Windows.Forms.DataGridViewColumnSortMode)value; }
        }

        /// <summary>Gets or sets the column's default cell style.</summary>
        /// <returns>A <see cref="IDataGridViewCellStyle"></see> that represents the default style of the cells in the column.</returns>
        /// <filterpriority>1</filterpriority>
        public IDataGridViewCellStyle DefaultCellStyle
        {
            get { return new DataGridViewCellStyleWin(base.DefaultCellStyle); }
            set { throw new NotImplementedException(); }
        }
    }



    /// <summary>
    /// Represents a cell that holds a calendar date
    /// </summary>
    public class CalendarCell : DataGridViewTextBoxCell
    {
        /// <summary>
        /// Constructor to initialise a new cell, using the short date format
        /// </summary>
        public CalendarCell()
            : base()
        {
            // Use the short date format.
            this.Style.Format = "d";
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
            CalendarEditingControl ctl =
                DataGridView.EditingControl as CalendarEditingControl;

            if (this.Value == null)
            {
                ctl.Checked = false;
            }
            else
            {
                if (this.Value.ToString() != "")
                    ctl.Value = DateTime.Parse(this.Value.ToString());
            }
        }

        /// <summary>
        /// Gets the type of editing control that is used
        /// </summary>
        public override Type EditType
        {
            get { return typeof(CalendarEditingControl); }
        }

        /// <summary>
        /// Gets the type of value contained in the cell
        /// </summary>
        public override Type ValueType
        {
            get { return typeof(DateTime); }
        }

        /// <summary>
        /// Gets the default value for a new row, which in this case is
        /// the current date and time
        /// </summary>
        public override object DefaultNewRowValue
        {
            get { return DateTime.Now; }
        }
    }


    /// <summary>
    /// A control for editing date and time values
    /// </summary>
    public class CalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        DataGridView _dataGridView;
        private bool _valueChanged = false;
        int _rowIndex;

        /// <summary>
        /// Constructor to initialise a new editing control with the short
        /// date format
        /// </summary>
        public CalendarEditingControl()
        {
            this.Format = System.Windows.Forms.DateTimePickerFormat.Short;
        }

        /// <summary>
        /// Gets and sets the value being held in the control
        /// </summary>
        public object EditingControlFormattedValue
        {
            get { return this.Value.ToShortDateString(); }
            set
            {
                if (value is String)
                {
                    this.Value = DateTime.Parse((String)value);
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
            this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
            this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
        }

        /// <summary>
        /// Gets and sets the row index number
        /// </summary>
        public int EditingControlRowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
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
                case Keys.PageDown:
                case Keys.PageUp:
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
        /// Gets the value that indicates whether the control should be repositioned when there
        /// is a value change
        /// </summary>
        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        /// <summary>
        /// Gets and sets the DataGridView object referenced in this
        /// control
        /// </summary>
        public DataGridView EditingControlDataGridView
        {
            get { return _dataGridView; }
            set { _dataGridView = value; }
        }

        /// <summary>
        /// Gets and sets the boolean which indicates whether the value
        /// held in the control has changed
        /// </summary>
        public bool EditingControlValueChanged
        {
            get { return _valueChanged; }
            set { _valueChanged = value; }
        }

        /// <summary>
        /// Gets the Cursor object from the editing panel
        /// </summary>
        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; }
        }

        /// <summary>
        /// A handler to carry out repercussions of a changed value
        /// </summary>
        /// <param name="eventargs">Arguments relating to the event</param>
        protected override void OnValueChanged(EventArgs eventargs)
        {
            // Notify the DataGridView that the contents of the cell
            // have changed.
            _valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
        }
    }
}