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
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.Util;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a TextBox
    /// depending on the environment
    /// </summary>
    internal class TextBoxMapperStrategyWin : ITextBoxMapperStrategy
    {
        // Assumes that one strategy is created for each control.
        // These fields exist so that the IsValidCharacter method knows
        //   which prop and textbox it is dealing with
        private IBOProp _boProp;
        private TextBoxWin _textBox;
        private TextBoxMapper _mapper;

        /// <summary>
        /// Gets the BOProp being mapped through this control
        /// </summary>
        public IBOProp BoProp
        {
            get { return _boProp; }
        }

        /// <summary>
        /// Gets the textbox control for which the strategy is applied
        /// </summary>
        public TextBoxWin TextBoxControl
        {
            get { return _textBox; }
        }

        /// <summary>
        /// Adds key press event handlers that carry out actions like
        /// limiting the input of certain characters, depending on the type of the
        /// property
        /// </summary>
        /// <param name="mapper">The TextBox mapper</param>
        /// <param name="boProp">The property being mapped</param>
        public void AddKeyPressEventHandler(TextBoxMapper mapper, IBOProp boProp)
        {
            _boProp = boProp;
            if (mapper.Control is ITextBox)
            {
                TextBoxWin tb = (TextBoxWin) mapper.Control;
                tb.KeyPress += KeyPressEventHandler;
                _textBox = tb;
            }
        }

        public void AddUpdateBoPropOnTextChangedHandler(TextBoxMapper mapper, IBOProp boProp)
        {
            _boProp = boProp;
            _mapper = mapper;
            if(mapper.Control is ITextBox)
            {
                TextBoxWin tb = (TextBoxWin)mapper.Control;
                tb.TextChanged += UpdateBoPropWithTextFromTextBox;
                _textBox = tb;
            }
        }

        private void UpdateBoPropWithTextFromTextBox(object sender, EventArgs e)
        {
            try
            {
                _mapper.ApplyChangesToBusinessObject();
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        private void KeyPressEventHandler(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if (!IsValidCharacter(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        /// <summary>
        /// Indicates if the given character being typed is valid, based on the
        /// text already entered in the textbox.  For instance, if the property
        /// type is an integer, this method will return false for a non-numeric
        /// character (apart from a negative sign).
        /// </summary>
        /// <param name="character">The character being input</param>
        /// <returns>Returns true if valid</returns>
        internal bool IsValidCharacter(char character)
        {
            if (BoProp == null) return true;
            if (TextBoxControl == null) return true;

            if (TypeUtilities.IsInteger(BoProp.PropertyType))
            {
                if ((character < '0' || character > '9') && character != 8 && character != '-')
                {
                    return false;
                }
                if (character == '-' && TextBoxControl.SelectionStart != 0)
                {
                    return false;
                }
            }
            else if (TypeUtilities.IsDecimal(BoProp.PropertyType))
            {
                if ((character < '0' || character > '9') && character != '.' && character != 8 && character != '-')
                {
                    return false;
                }
                if (character == '.' && TextBoxControl.Text.Contains("."))
                {
                    return false;
                }
                // In fact the char is valid, but we want the event to get handled in order to prevent double dots
                if (character == '.' && TextBoxControl.SelectionStart == 0)
                {
                    TextBoxControl.Text = "0." + TextBoxControl.Text;
                    TextBoxControl.SelectionStart = 2;
                    TextBoxControl.SelectionLength = 0;
                    return false;
                }
                if (character == '-' && TextBoxControl.SelectionStart != 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}