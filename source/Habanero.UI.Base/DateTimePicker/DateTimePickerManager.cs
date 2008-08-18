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

namespace Habanero.UI.Base
{
    /// <summary>
    /// This manager groups common logic for IDateTimePicker control.
    /// Do not use this object in working code - rather call CreateDateTimePicker
    /// in the appropriate control factory.
    /// </summary>
    public class DateTimePickerManager
    {
        #region Delegates

        public delegate DateTime ValueGetter();

        public delegate void ValueSetter(DateTime value);

        #endregion

        private readonly IControlFactory _controlFactory;
        private readonly IDateTimePicker _dateTimePicker;
        private readonly ValueGetter _valueGetter;
        private readonly ValueSetter _valueSetter;

        //State Variables
        private bool _isNull;
        //private IPanel _displayBox;
        //private ILabel _displayText;

        //private event EventHandler _valueChanged;

        public DateTimePickerManager(IControlFactory controlFactory, IDateTimePicker dateTimePicker,
                                     ValueGetter valueGetter, ValueSetter valueSetter)
        {
            if (valueGetter == null) throw new ArgumentNullException("valueGetter");
            if (valueSetter == null) throw new ArgumentNullException("valueSetter");
            _controlFactory = controlFactory;
            _dateTimePicker = dateTimePicker;
            _valueGetter = valueGetter;
            _valueSetter = valueSetter;
            ApplyBlankFormat();
            //SetupDisplayBox();
        }

        #region Properties

        ///<summary>
        /// Gets the <see cref="IDateTimePicker"/> control
        ///</summary>
        public IDateTimePicker DateTimePicker
        {
            get { return _dateTimePicker; }
        }

        /////<summary>
        ///// The text that will be displayed when the Value is null
        /////</summary>
        //public string NullDisplayValue
        //{
        //    get { return _displayText.Text; }
        //    set { _displayText.Text = value ?? ""; }
        //}

        /// <summary>
        /// Gets or sets the nullable DateTime value in the control
        /// </summary>
        public DateTime? ValueOrNull
        {
            get
            {
                if (!IsNull())
                {
                    return GetDateTimePickerValue();
                }
                return null;
            }
            set
            {
                if (ValueOrNull == value) return;
                if (value != null)
                {
                    SetDateTimePickerValue((DateTime) value);
                    ApplyValueFormat();
                }
                else
                {
                    ApplyBlankFormat();
                }
                FireValueChanged();
            }
        }

        ///<summary>
        /// The non-nullable DateTime value represented by the DateTimePicker
        ///</summary>
        public DateTime Value
        {
            get { return GetDateTimePickerValue(); }
            set
            {
                if (ValueOrNull == value) return;
                SetDateTimePickerValue(value);

                //if (!CheckBoxVisible)
                //    {
                //        ApplyValueFormat();
                //    }
                //    FireValueChanged();

                ApplyValueFormat();
                FireValueChanged();
            }
        }

        private void SetDateTimePickerValue(DateTime value)
        {
            _valueSetter(value);
        }

        private DateTime GetDateTimePickerValue()
        {
            return _valueGetter();
        }

        ///// <summary>
        ///// Occurs when the <see cref="Value"/> property changes.
        ///// </summary>
        //public event EventHandler ValueChanged
        //{
        //    add { _valueChanged += value; }
        //    remove { _valueChanged -= value; }
        //}

        #endregion //Properties

        #region State Control

        private bool IsNull()
        {
            if (CheckBoxVisible)
            {
                return !CheckBoxChecked;
            }
            return _isNull;
        }

        private bool ApplyValueFormat()
        {
            //_displayBox.Visible = false;
            if (!IsNull()) return false;
            if (CheckBoxVisible)
            {
                CheckBoxChecked = true;
            }
            _isNull = false;
            return true;
        }

        private bool ApplyBlankFormat()
        {
            if (IsNull()) return false;
            if (!CheckBoxVisible)
            {
                //_displayBox.Visible = true;
            }
            else
            {
                //_displayBox.Visible = false;
                CheckBoxChecked = false;
            }
            _isNull = true;
            return true;
        }

        private void FireValueChanged()
        {
            //if (_valueChanged != null)
            //{
            //    _valueChanged(this, EventArgs.Empty);
            //}
        }

        #endregion //State Control

        #region Control State Methods

        private bool CheckBoxVisible
        {
            get { return _dateTimePicker.ShowCheckBox; }
        }

        private bool CheckBoxChecked
        {
            get { return _dateTimePicker.Checked; }
            set { _dateTimePicker.Checked = value; }
        }

        #endregion //Control State Methods

        #region Control Events

        //private void DateTimePicker_ColorChanged(object sender, EventArgs e)
        //{
        //    UpdateFocusState();
        //}

        //private void DateTimePicker_LostFocus(object sender, EventArgs e)
        //{
        //    UpdateFocusState();
        //}

        //private void DateTimePicker_GotFocus(object sender, EventArgs e)
        //{
        //    UpdateFocusState();
        //}

        //private void DateTimePicker_Resize(object sender, EventArgs e)
        //{
        //    ResizeDisplayBox();
        //}

        //private void DateTimePicker_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (!CheckBoxVisible)
        //    {
        //        if (e.Button == MouseButtons.Left)
        //        {
        //            if (ApplyValueFormat())
        //            {
        //                FireValueChanged();
        //            }
        //        }
        //    }
        //}

        //private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        //{
        //    if (!CheckBoxVisible)
        //    {
        //        ApplyValueFormat();
        //    }
        //    FireValueChanged();
        //}

        //private void DateTimePicker_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (!CheckBoxVisible)
        //    {
        //        if ((e.KeyCode == Keys.Back
        //             || e.KeyCode == Keys.Delete))
        //        {
        //            e.SuppressKeyPress = true;
        //            if (ApplyBlankFormat())
        //            {
        //                FireValueChanged();
        //            }
        //        }
        //        else if (e.KeyCode != Keys.ShiftKey && IsNull())
        //        {
        //            ApplyValueFormat();
        //        }
        //    }
        //}

        #endregion //Control Events

        //#region Setup Controller
        //    ///<summary>
        //    /// Initialises a new instance of a DateTimePickerController.
        //    ///</summary>
        //    ///<param name="dateTimePicker">The DateTimePicker control(can be any implementation)</param>
        //    public DateTimePickerController(IControlFactory controlFactory, IDateTimePicker dateTimePicker)
        //    {
        //        _controlFactory = controlFactory;
        //        _dateTimePicker = dateTimePicker;
        //        //_dateTimePicker.KeyDown += DateTimePicker_KeyDown;
        //        //_dateTimePicker.ValueChanged += DateTimePicker_ValueChanged;
        //        //_dateTimePicker.MouseUp += DateTimePicker_MouseUp;
        //        //_dateTimePicker.GotFocus += DateTimePicker_GotFocus;
        //        //_dateTimePicker.LostFocus += DateTimePicker_LostFocus;
        //        _dateTimePicker.Resize += DateTimePicker_Resize;
        //        //_dateTimePicker.EnabledChanged += DateTimePicker_ColorChanged;
        //        //_dateTimePicker.BackColorChanged += DateTimePicker_ColorChanged;
        //        //_dateTimePicker.ForeColorChanged += DateTimePicker_ColorChanged;
        //        //_showCheckBoxPropInfo = _dateTimePicker.ShowCheckBox;
        //        //_checkedPropInfo = _dateTimePicker.Checked;
        //        _supportsCheckBox = _showCheckBoxPropInfo != null && _checkedPropInfo != null;
        //        SetupDisplayBox();
        //        NullDisplayValue = "";
        //        Value = null;
        //        UpdateFocusState();
        //    }
        //    ~DateTimePickerController()
        //    {
        //        //_dateTimePicker.KeyDown -= DateTimePicker_KeyDown;
        //        //_dateTimePicker.ValueChanged -= DateTimePicker_ValueChanged;
        //        //_dateTimePicker.MouseUp -= DateTimePicker_MouseUp;
        //        //_dateTimePicker.GotFocus -= DateTimePicker_GotFocus;
        //        //_dateTimePicker.LostFocus -= DateTimePicker_LostFocus;
        //        _dateTimePicker.Resize -= DateTimePicker_Resize;
        //        //_dateTimePicker.EnabledChanged -= DateTimePicker_ColorChanged;
        //        //_dateTimePicker.BackColorChanged -= DateTimePicker_ColorChanged;
        //        //_dateTimePicker.ForeColorChanged -= DateTimePicker_ColorChanged;

        //        //ControlsHelper.SafeGui(_dateTimePicker, delegate()
        //        //                                            {
        //                                                        try
        //                                                        {
        //                                                            _dateTimePicker.Controls.Clear();
        //                                                        }
        //                                                        catch
        //                                                        {
        //                                                        }
        //        //                                            });
        //        _dateTimePicker = null;
        //    }
        //private void SetupDisplayBox()
        //{
        //    //ControlsHelper.SafeGui(_dateTimePicker, delegate()
        //    //{
        //    _displayBox = _controlFactory.CreatePanel();
        //    //_displayBox.BorderStyle = BorderStyle.None;
        //    _displayBox.Location = new Point(2, 2);
        //    ResizeDisplayBox();
        //    _displayBox.BackColor = _dateTimePicker.BackColor;
        //    _displayBox.ForeColor = _dateTimePicker.ForeColor;
        //    //_displayBox.MouseUp += DateTimePicker_MouseUp;
        //    //_displayBox.KeyDown += DateTimePicker_KeyDown;
        //    _displayText = _controlFactory.CreateLabel();
        //    _displayText.Location = new Point(0, 0);
        //    _displayText.AutoSize = true;
        //    _displayText.Text = "";
        //    //_displayText.MouseUp += DateTimePicker_MouseUp;
        //    //_displayText.KeyDown += DateTimePicker_KeyDown;
        //    //_displayBox.Controls.Add(_displayText);
        //    //_dateTimePicker.Controls.Add(_displayBox);
        //    _displayBox.Visible = false;
        //    //});
        //}

        //private void ResizeDisplayBox()
        //{
        //    _displayBox.Width = _dateTimePicker.Width - 22 - 2;
        //    _displayBox.Height = _dateTimePicker.Height - 7;
        //}

        //    private void UpdateFocusState()
        //    {
        //        //if (_dateTimePicker.Focused)
        //        //{
        //        //    _displayBox.BackColor = SystemColors.Highlight;
        //        //    _displayBox.ForeColor = SystemColors.HighlightText;
        //        //}
        //        //else
        //        //{
        //        //    _displayBox.BackColor = _dateTimePicker.BackColor;
        //        //    _displayBox.ForeColor = _dateTimePicker.BackColor;
        //        //}
        //        //_displayText.BackColor = _displayBox.BackColor;
        //        //_displayText.ForeColor = _displayBox.ForeColor;
        //    }

        //    #endregion //Setup Controller

        /// <summary>
        /// Handles the event of the DateTime value being changed
        /// </summary>
        public void OnValueChanged(EventArgs eventArgs)
        {
            if (!CheckBoxVisible)
            {
                ApplyValueFormat();
            }
            FireValueChanged();
        }
    }
}