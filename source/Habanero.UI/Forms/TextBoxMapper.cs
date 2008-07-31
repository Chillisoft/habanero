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

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Maps a TextBox object in a user interface.  Note that there are some
    /// limitations with using a TextBox for numbers.  For greater control 
    /// of user input with numbers, you should consider using a NumericUpDown 
    /// control.
    /// </summary>
    public class TextBoxMapper : ControlMapper
    {
        private TextBox _textBox;
        private string _oldText;

        /// <summary>
        /// Constructor to initialise a new instance of the mapper
        /// </summary>
        /// <param name="tb">The TextBox to map</param>
        /// <param name="propName">The property name</param>
		/// <param name="isReadOnly">Whether this control is read only</param>
		public TextBoxMapper(TextBox tb, string propName, bool isReadOnly)
			: base(tb, propName, isReadOnly)
        {
            _textBox = tb;
            //_textBox.Enabled = false;
            _textBox.KeyPress += delegate(object sender, KeyPressEventArgs e)
                         {
                             if (IsIntegerType())
                             {
                                 if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != 8 && e.KeyChar != '-')
                                 {
                                     e.Handled = true;
                                 }
                                 if (e.KeyChar == '-' && _textBox.SelectionStart != 0)
                                 {
                                     e.Handled = true;
                                 }
                             }
                             if (IsDecimalType())
                             {
                                 if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '.' && e.KeyChar != 8 && e.KeyChar != '-')
                                 {
                                     e.Handled = true;
                                 }

                                 if (e.KeyChar == '.' && _textBox.Text.Contains("."))
                                 {
                                     e.Handled = true;
                                 }
                                 if (e.KeyChar == '.' && _textBox.SelectionStart == 0)
                                 {
                                     _textBox.Text = "0." + _textBox.Text;
                                     e.Handled = true;
                                     _textBox.SelectionStart = 2;
                                     _textBox.SelectionLength = 0;
                                 }
                                 if (e.KeyChar == '-' && _textBox.SelectionStart != 0)
                                 {
                                     e.Handled = true;
                                 }
                             }
                         };
            _textBox.TextChanged += new EventHandler(ValueChangedHandler);
            _oldText = "";
        }

        private bool IsDecimalType()
        {
            Type propertyType = GetPropertyType();
            if (propertyType == null) return false;
            return
                (propertyType == typeof (decimal) || propertyType == typeof (double) || propertyType == typeof (float));
        }

        private bool IsIntegerType()
        {
            Type propertyType = GetPropertyType();
            if (propertyType == null) return false;
            return
                (propertyType == typeof (int) || propertyType == typeof (long) ||
                 propertyType == typeof (short) || propertyType == typeof (uint) ||
                 propertyType == typeof (byte) || propertyType == typeof (ulong)
                 || propertyType == typeof (ushort) || propertyType == typeof(sbyte));
        }

        private Type GetPropertyType()
        {
            if (_businessObject == null || !_businessObject.Props.Contains(_propertyName))
            {
                 return null;
            }
            return _businessObject.Props[_propertyName].PropertyType;
        }

        /// <summary>
        /// A handler to carry out changes to the business object when the
        /// value has changed in the user interface
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
		private void ValueChangedHandler(object sender, EventArgs e)
        {
        	string value = _textBox.Text;


        	if (IsDecimalType())
        	{
        		if (value.EndsWith(".")) return;
        	}
        	if (IsDecimalType() || IsIntegerType())
        	{
        		if (value.EndsWith("-")) return;
        		if (value.Length == 0)
        		{
        			value = null;
        		}
        	}

			if (!_isEditable) return;

        	try
        	{
                SetPropertyValue(value);
                //_businessObject.SetPropertyValue(_propertyName, value);
        	}
        	catch (FormatException)
        	{
        		_textBox.Text = _oldText;
        	}
        	_oldText = _textBox.Text;
        }

    	/// <summary>
        /// Updates the interface when the value has been changed in the
        /// object being represented
        /// </summary>
        protected override void ValueUpdated()
        {
            _textBox.Text = Convert.ToString(this.GetPropertyValue());
        }
    }
}