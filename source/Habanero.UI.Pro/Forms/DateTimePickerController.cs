using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Habanero.UI.Util;

namespace Habanero.UI.Forms
{
	///<summary>
	/// This controller can be used with any DateTimePicker in order to 
	/// allow support for nulls.
	///</summary>
	public class DateTimePickerController
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
		public DateTimePickerController(Control dateTimePicker)
		{
			_dateTimePicker = dateTimePicker;
			_dateTimePicker.KeyDown += DateTimePicker_KeyDown;
			DateTimePickerUtil.AddValueChangedHandler(_dateTimePicker, DateTimePicker_ValueChanged);
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

		~DateTimePickerController()
		{
			_dateTimePicker.KeyDown -= DateTimePicker_KeyDown;
			DateTimePickerUtil.RemoveValueChangedHandler(_dateTimePicker, DateTimePicker_ValueChanged);
			_dateTimePicker.MouseUp -= DateTimePicker_MouseUp;
			_dateTimePicker.GotFocus -= DateTimePicker_GotFocus;
			_dateTimePicker.LostFocus -= DateTimePicker_LostFocus;
			_dateTimePicker.Resize -= DateTimePicker_Resize;
			_dateTimePicker.EnabledChanged -= DateTimePicker_ColorChanged;
			_dateTimePicker.BackColorChanged -= DateTimePicker_ColorChanged;
			_dateTimePicker.ForeColorChanged -= DateTimePicker_ColorChanged;
            ControlsHelper.SafeGui(_dateTimePicker, delegate()
            {
                _dateTimePicker.Controls.Remove(_displayBox);
            });
			_dateTimePicker = null;
		}

		private void SetupDisplayBox()
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
			} else
			{
				_displayBox.BackColor = _dateTimePicker.BackColor;
				_displayBox.ForeColor = _dateTimePicker.BackColor;
			}
			_displayText.BackColor = _displayBox.BackColor;
			_displayText.ForeColor = _displayBox.ForeColor;
		}

		#endregion //Setup Controller

		#region Properties

		///<summary>
		/// The DateTimePicker control being controlled
		///</summary>
		public Control DateTimePicker
		{
			get { return _dateTimePicker; }
		}

		///<summary>
		/// The text that will be displayed when the Value is null
		///</summary>
		public string NullDisplayValue
		{
			get { return _displayText.Text; }
			set { _displayText.Text = value ?? ""; }
		}

		///<summary>
		/// The Value represented by the DateTimePicker
		///</summary>
		public DateTime? Value
		{
			get
			{
				if (!IsNull())
				{
					return (DateTime)DateTimePickerUtil.GetValue(_dateTimePicker);
				} else
				{
					return null;
				}
			}
			set
			{
				if (Value == value) return;
				if (value != null)
				{
					DateTimePickerUtil.SetValue(_dateTimePicker, (DateTime)value);
					ApplyValueFormat();
				} else
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
			} else
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
			} else
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
				} else if (e.KeyCode != Keys.ShiftKey && IsNull())
				{
					ApplyValueFormat();
				}
			}
		}

		#endregion //Control Events

		#region Control State Methods

		private void SetControlProp(PropertyInfo propertyInfo, object value)
		{
			propertyInfo.SetValue(_dateTimePicker, value, new object[] { });
		}

		private object GetControlProp(PropertyInfo propertyInfo)
		{
			return propertyInfo.GetValue(_dateTimePicker, new object[] { });
		}

		private bool CheckBoxVisible
		{
			get
			{
				if (_supportsCheckBox)
				{
					return (bool)GetControlProp(_showCheckBoxPropInfo);
				} else
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
					return (bool)GetControlProp(_checkedPropInfo);
				} else
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

