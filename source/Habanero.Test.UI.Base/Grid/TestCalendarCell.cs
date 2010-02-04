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

using System;
using System.ComponentModel;
using System.Drawing;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{

    public abstract class TestCalendarCell //: TestBase
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestCalendarCellWin : TestCalendarCell
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.Win.ControlFactoryWin();
            }
        }

        [TestFixture]
        public class TestCalendarCellVWG : TestCalendarCell
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.VWG.ControlFactoryVWG();
            }
        }


        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
        }

    

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }
    }
 
    public interface ICalendarCell
    {
        /// <summary>
        /// Initialises the editing control
        /// </summary>
        /// <param name="rowIndex">The row index number</param>
        /// <param name="initialFormattedValue">The initial value</param>
        /// <param name="dataGridViewCellStyle">The cell style</param>
        void InitializeEditingControl
            (int rowIndex, object initialFormattedValue, IDataGridViewCellStyle dataGridViewCellStyle);

        /// <summary>
        /// Returns the type of editing control that is used
        /// </summary>
        Type EditType { get; }

        /// <summary>
        /// Returns the type of value contained in the cell
        /// </summary>
        Type ValueType { get; }

        /// <summary>
        /// Returns the default value for a new row, which in this case is
        /// the current date and time
        /// </summary>
        object DefaultNewRowValue { get; }
    }

    //TODO: Implement CalendarEditingControl needs IDataGridViewEditingControl
    ///// <summary>
    ///// A control for editing date and time values
    ///// </summary>
    //class CalendarEditingControl : DateTimePickerVWG, IDataGridViewEditingControl
    //{
    //    IDataGridView _dataGridView;
    //    private bool _valueChanged = false;
    //    int _rowIndex;

    //    /// <summary>
    //    /// Constructor to initialise a new editing control with the short
    //    /// date format
    //    /// </summary>
    //    public CalendarEditingControl()
    //    {
    //        this.Format = DateTimePickerFormat.Short;
    //    }

    //    /// <summary>
    //    /// Gets and sets the value being held in the control
    //    /// </summary>
    //    public object EditingControlFormattedValue
    //    {
    //        get
    //        {
    //            return this.Value.ToShortDateString();
    //        }
    //        set
    //        {
    //            if (value is String)
    //            {
    //                this.Value = DateTime.Parse((String)value);
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Returns the value being held in the control
    //    /// </summary>
    //    /// <returns>Returns the value being held</returns>
    //    public object GetEditingControlFormattedValue(
    //        IDataGridViewDataErrorContexts context)
    //    {
    //        return EditingControlFormattedValue;
    //    }

    //    /// <summary>
    //    /// Copy the styles from the object provided across to this editing
    //    /// control
    //    /// </summary>
    //    /// <param name="dataGridViewCellStyle">The source to copy from</param>
    //    public void ApplyCellStyleToEditingControl(
    //        IDataGridViewCellStyle dataGridViewCellStyle)
    //    {
    //        this.Font = dataGridViewCellStyle.Font;
    //        this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
    //        this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
    //    }

    //    /// <summary>
    //    /// Gets and sets the row index number
    //    /// </summary>
    //    public int EditingControlRowIndex
    //    {
    //        get
    //        {
    //            return _rowIndex;
    //        }
    //        set
    //        {
    //            _rowIndex = value;
    //        }
    //    }

    //    /// <summary>
    //    /// Indicates if the editing control wants the input key specified
    //    /// </summary>
    //    /// <param name="key">The key in question</param>
    //    /// <param name="dataGridViewWantsInputKey">Whether the DataGridView
    //    /// wants the input key</param>
    //    /// <returns>Returns true if so, false if not</returns>
    //    public bool EditingControlWantsInputKey(
    //        Keys key, bool dataGridViewWantsInputKey)
    //    {
    //        // Let the DateTimePicker handle the keys listed.
    //        switch (key & Keys.KeyCode)
    //        {
    //            case Keys.Left:
    //            case Keys.Up:
    //            case Keys.Down:
    //            case Keys.Right:
    //            case Keys.Home:
    //            case Keys.End:
    //            case Keys.PageDown:
    //            case Keys.PageUp:
    //                return true;
    //            default:
    //                return false;
    //        }
    //    }

    //    /// <summary>
    //    /// Prepares the editing control for editing
    //    /// </summary>
    //    /// <param name="selectAll">Whether to select all the content first,
    //    /// which can make it easier to replace as you type</param>
    //    public void PrepareEditingControlForEdit(bool selectAll)
    //    {
    //        // No preparation needs to be done.
    //    }

    //    /// <summary>
    //    /// Indicates whether the control should be repositioned when there
    //    /// is a value change
    //    /// </summary>
    //    public bool RepositionEditingControlOnValueChange
    //    {
    //        get
    //        {
    //            return false;
    //        }
    //    }

    //    /// <summary>
    //    /// Gets and sets the DataGridView object referenced in this
    //    /// control
    //    /// </summary>
    //    public IDataGridView EditingControlDataGridView
    //    {
    //        get
    //        {
    //            return _dataGridView;
    //        }
    //        set
    //        {
    //            _dataGridView = value;
    //        }
    //    }

    //    /// <summary>
    //    /// Gets and sets the boolean which indicates whether the value
    //    /// held in the control has changed
    //    /// </summary>
    //    public bool EditingControlValueChanged
    //    {
    //        get
    //        {
    //            return _valueChanged;
    //        }
    //        set
    //        {
    //            _valueChanged = value;
    //        }
    //    }

    //    /// <summary>
    //    /// Returns the Cursor object from the editing panel
    //    /// </summary>
    //    public Cursor EditingPanelCursor
    //    {
    //        get
    //        {
    //            return base.Cursor;
    //        }
    //    }

    //    /// <summary>
    //    /// A handler to carry out repercussions of a changed value
    //    /// </summary>
    //    /// <param name="eventargs">Arguments relating to the event</param>
    //    protected override void OnValueChanged(EventArgs eventargs)
    //    {
    //        // Notify the DataGridView that the contents of the cell
    //        // have changed.
    //        _valueChanged = true;
    //        this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
    //        base.OnValueChanged(eventargs);
    //    }
    //}

    //public class CalendarCellVWG : DataGridViewCellVWG, ICalendarCell
    //{
    //    /// <summary>
    //    /// Constructor to initialise a new cell, using the short date format
    //    /// </summary>
    //    public CalendarCellVWG()
    //    {
    //        // Use the short date format.
    //        this.Style.Format = "d";
    //    }

    //    /// <summary>
    //    /// Initialises the editing control
    //    /// </summary>
    //    /// <param name="rowIndex">The row index number</param>
    //    /// <param name="initialFormattedValue">The initial value</param>
    //    /// <param name="dataGridViewCellStyle">The cell style</param>
    //    public void InitializeEditingControl
    //        (int rowIndex, object initialFormattedValue, IDataGridViewCellStyle dataGridViewCellStyle)
    //    {
    //        // Set the value of the editing control to the current cell value.
    //        base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
    //        CalendarEditingControl ctl = DataGridView.EditingControl as CalendarEditingControl;

    //        if (this.Value == null)
    //        {
    //            ctl.Checked = false;
    //        }
    //        else
    //        {
    //            if (this.Value.ToString() != "")
    //                ctl.Value = DateTime.Parse(this.Value.ToString());
    //        }
    //    }

    //    /// <summary>
    //    /// Returns the type of editing control that is used
    //    /// </summary>
    //    public override Type EditType
    //    {
    //        get { return typeof(CalendarEditingControl); }
    //    }

    //    /// <summary>
    //    /// Returns the type of value contained in the cell
    //    /// </summary>
    //    public override Type ValueType
    //    {
    //        get { return typeof(DateTime); }
    //    }

    //    /// <summary>
    //    /// Returns the default value for a new row, which in this case is
    //    /// the current date and time
    //    /// </summary>
    //    public override object DefaultNewRowValue
    //    {
    //        get { return DateTime.Now; }
    //    }
    //}
    //public interface IDataGridViewTextBoxCell : IDataGridViewCell
    //{
    //    //        /// <filterpriority>1</filterpriority>
    //    //        [EditorBrowsable(EditorBrowsableState.Advanced)]
    //    //        void DetachEditingControl();

    //    /// <summary>Attaches and initializes the hosted editing control.</summary>
    //    /// <param name="initialFormattedValue">The initial value to be displayed in the control.</param>
    //    /// <param name="rowIndex">The index of the row being edited.</param>
    //    /// <param name="dataGridViewCellStyle">A cell style that is used to determine the appearance of the hosted control.</param>
    //    /// <filterpriority>1</filterpriority>
    //    void InitializeEditingControl
    //        (int rowIndex, object initialFormattedValue, IDataGridViewCellStyle dataGridViewCellStyle);

    //    //        ///// <summary>Determines if edit mode should be started based on the given key.</summary>
    //    //        ///// <returns>true if edit mode should be started; otherwise, false. </returns>
    //    //        ///// <param name="e">A <see cref="T:Gizmox.WebGUI.Forms.KeyEventArgs"></see> that represents the key that was pressed.</param>
    //    //        ///// <filterpriority>1</filterpriority>
    //    //        //bool KeyEntersEditMode(KeyEventArgs e);
    //    //        /// <filterpriority>1</filterpriority>
    //    //        void PositionEditingControl
    //    //            (bool setLocation, bool setSize, Rectangle cellBounds, Rectangle cellClip, IDataGridViewCellStyle cellStyle,
    //    //             bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn,
    //    //             bool isFirstDisplayedRow);

    //    ///// <summary>Gets the type of the formatted value associated with the cell.</summary>
    //    ///// <returns>A <see cref="T:System.Type"></see> representing the <see cref="T:System.String"></see> 
    //    ///// type in all cases.</returns>
    //    ///// <filterpriority>1</filterpriority>
    //    //Type FormattedValueType { get; }

    //    ///// <summary>Gets or sets the maximum number of characters that can be entered into the text box.</summary>
    //    ///// <returns>The maximum number of characters that can be entered into the text box; the default value is 32767.</returns>
    //    ///// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than 0.</exception>
    //    //[DefaultValue(0x7fff)]
    //    //int MaxInputLength { get; set; }
    //}
    
}