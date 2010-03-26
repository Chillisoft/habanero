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
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides utilities to control how a DateTimePicker appears and behaves
    /// </summary>
    public class DateTimePickerControllerWin //: DateTimePickerController
    {
        //State Variables
        private Control _dateTimePicker;
        private bool _isNull;
        private Panel _displayBox;
        private Label _displayText;
        //Property Info
        private PropertyInfo _showCheckBoxPropInfo;
        private PropertyInfo _checkedPropInfo;
        private bool _supportsCheckBox;

        private event EventHandler _valueChanged;

        #region Setup Controller
        		///<summary>
		/// Initialises a new instance of a DateTimePickerController.
		///</summary>
		///<param name="dateTimePicker">The DateTimePicker control(can be any implementation)</param>
		public DateTimePickerControllerWin(Control dateTimePicker)
		{
            
			_dateTimePicker = dateTimePicker;
			_dateTimePicker.KeyDown += DateTimePicker_KeyDown;
			DateTimePickerUtilWinTemp.AddValueChangedHandler(_dateTimePicker, DateTimePicker_ValueChanged);
			_dateTimePicker.MouseUp += DateTimePicker_MouseUp;
			_dateTimePicker.GotFocus += DateTimePicker_GotFocus;
			_dateTimePicker.LostFocus += DateTimePicker_LostFocus;
			_dateTimePicker.Resize += DateTimePicker_Resize;
			_dateTimePicker.EnabledChanged += DateTimePicker_ColorChanged;
			_dateTimePicker.BackColorChanged += DateTimePicker_ColorChanged;
			_dateTimePicker.ForeColorChanged += DateTimePicker_ColorChanged;
			_showCheckBoxPropInfo = _dateTimePicker.GetType().GetProperty("ShowCheckBox", BindingFlags.Instance | BindingFlags.Public);
			_checkedPropInfo = _dateTimePicker.GetType().GetProperty("Checked", BindingFlags.Instance | BindingFlags.Public);
			_supportsCheckBox = _showCheckBoxPropInfo != null && _checkedPropInfo != null;
			SetupDisplayBox();
			NullDisplayValue = "";
			Value = null;
			UpdateFocusState();
		}
        /// <summary>
        /// Cleans up all events etc on the DateTimePicker.
        /// </summary>
        ~DateTimePickerControllerWin()
        {
            _dateTimePicker.KeyDown -= DateTimePicker_KeyDown;
            DateTimePickerUtilWinTemp.RemoveValueChangedHandler(_dateTimePicker, DateTimePicker_ValueChanged);
            _dateTimePicker.MouseUp -= DateTimePicker_MouseUp;
            _dateTimePicker.GotFocus -= DateTimePicker_GotFocus;
            _dateTimePicker.LostFocus -= DateTimePicker_LostFocus;
            _dateTimePicker.Resize -= DateTimePicker_Resize;
            _dateTimePicker.EnabledChanged -= DateTimePicker_ColorChanged;
            _dateTimePicker.BackColorChanged -= DateTimePicker_ColorChanged;
            _dateTimePicker.ForeColorChanged -= DateTimePicker_ColorChanged;
            ControlsHelper.SafeGui(_dateTimePicker, delegate()
                                                        {
                                                            try
                                                            {
                                                                _dateTimePicker.Controls.Clear();
                                                            }
                                                            catch
                                                            {
                                                            }
                                                        });
            _dateTimePicker = null;
        }

        private void SetupDisplayBox()
        {
            ControlsHelper.SafeGui(_dateTimePicker, delegate()
            {
                _displayBox = new Panel();
                _displayBox.BorderStyle = BorderStyle.None;
                _displayBox.Location = new Point(2, 2);
                ResizeDisplayBox();
                _displayBox.BackColor = _dateTimePicker.BackColor;
                _displayBox.ForeColor = _dateTimePicker.ForeColor;
                _displayBox.MouseUp += DateTimePicker_MouseUp;
                _displayBox.KeyDown += DateTimePicker_KeyDown;
                _displayText = new Label();
                _displayText.Location = new Point(0, 0);
                _displayText.AutoSize = true;
                _displayText.Text = "";
                _displayText.MouseUp += DateTimePicker_MouseUp;
                _displayText.KeyDown += DateTimePicker_KeyDown;
                _displayBox.Controls.Add(_displayText);
                _dateTimePicker.Controls.Add(_displayBox);
                _displayBox.Visible = false;
            });
        }

        private void ResizeDisplayBox()
        {
            _displayBox.Width = _dateTimePicker.Width - 22 - 2;
            _displayBox.Height = _dateTimePicker.Height - 7;
        }

        private void UpdateFocusState()
        {
            if (_dateTimePicker.Focused)
            {
                _displayBox.BackColor = SystemColors.Highlight;
                _displayBox.ForeColor = SystemColors.HighlightText;
            }
            else
            {
                _displayBox.BackColor = _dateTimePicker.BackColor;
                _displayBox.ForeColor = _dateTimePicker.BackColor;
            }
            _displayText.BackColor = _displayBox.BackColor;
            _displayText.ForeColor = _displayBox.ForeColor;
        }

        #endregion //Setup Controller

        #region Properties

        /// <summary>
        /// Gets the DateTimePicker control being controlled
        /// </summary>
        public Control DateTimePicker
        {
            get { return _dateTimePicker; }
        }

        /// <summary>
        /// Gets and sets the text that will be displayed when the Value is null
        /// </summary>
        public string NullDisplayValue
        {
            get { return _displayText.Text; }
            set { _displayText.Text = value ?? ""; }
        }

        /// <summary>
        /// Gets and sets the Value represented by the DateTimePicker
        /// </summary>
        public DateTime? Value
        {
            get
            {
                if (!IsNull())
                {
                    return (DateTime) DateTimePickerUtilWinTemp.GetValue(_dateTimePicker);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Value == value) return;
                if (value != null)
                {
                    DateTimePickerUtilWinTemp.SetValue(_dateTimePicker, (DateTime) value);
                    ApplyValueFormat();
                }
                else
                {
                    ApplyBlankFormat();
                }
                FireValueChanged();
            }
        }

        /// <summary>
        /// Occurs when the <see cref="Value"/> property changes.
        /// </summary>
        public event EventHandler ValueChanged
        {
            add { _valueChanged += value; }
            remove { _valueChanged -= value; }
        }

        #endregion //Properties

        #region State Control

        private bool IsNull()
        {
            if (CheckBoxVisible)
            {
                return !CheckBoxChecked;
            }
            else
            {
                return _isNull;
            }
        }

        private bool ApplyValueFormat()
        {
            _displayBox.Visible = false;
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
                _displayBox.Visible = true;
            }
            else
            {
                _displayBox.Visible = false;
                CheckBoxChecked = false;
            }
            _isNull = true;
            return true;
        }

        private void FireValueChanged()
        {
            if (_valueChanged != null)
            {
                _valueChanged(this, EventArgs.Empty);
            }
        }

        #endregion //State Control

        #region Control Events

        private void DateTimePicker_ColorChanged(object sender, EventArgs e)
        {
            UpdateFocusState();
        }

        private void DateTimePicker_LostFocus(object sender, EventArgs e)
        {
            UpdateFocusState();
        }

        private void DateTimePicker_GotFocus(object sender, EventArgs e)
        {
            UpdateFocusState();
        }

        private void DateTimePicker_Resize(object sender, EventArgs e)
        {
            ResizeDisplayBox();
        }

        private void DateTimePicker_MouseUp(object sender, MouseEventArgs e)
        {
            if (!CheckBoxVisible)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (ApplyValueFormat())
                    {
                        FireValueChanged();
                    }
                }
            }
        }

        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (!CheckBoxVisible)
            {
                ApplyValueFormat();
            }
            FireValueChanged();
        }

        private void DateTimePicker_KeyDown(object sender, KeyEventArgs e)
        {
            if (!CheckBoxVisible)
            {
                if ((e.KeyCode == Keys.Back
                     || e.KeyCode == Keys.Delete))
                {
                    e.SuppressKeyPress = true;
                    if (ApplyBlankFormat())
                    {
                        FireValueChanged();
                    }
                }
                else if (e.KeyCode != Keys.ShiftKey && IsNull())
                {
                    ApplyValueFormat();
                }
            }
        }

        #endregion //Control Events

        #region Control State Methods

        private void SetControlProp(PropertyInfo propertyInfo, object value)
        {
            propertyInfo.SetValue(_dateTimePicker, value, new object[] {});
        }

        private object GetControlProp(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetValue(_dateTimePicker, new object[] {});
        }

        private bool CheckBoxVisible
        {
            get
            {
                if (_supportsCheckBox)
                {
                    return (bool) GetControlProp(_showCheckBoxPropInfo);
                }
                else
                {
                    return false;
                }
            }
        }

        private bool CheckBoxChecked
        {
            get
            {
                if (_supportsCheckBox)
                {
                    return (bool) GetControlProp(_checkedPropInfo);
                }
                else
                {
                    return true;
                }
            }
            set
            {
                if (_supportsCheckBox)
                {
                    SetControlProp(_checkedPropInfo, value);
                }
            }
        }

        #endregion //Control State Methods
    }
}