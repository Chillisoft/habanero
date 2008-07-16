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
using Habanero.UI;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Provides a form in which a user can edit an integer value
    /// </summary>
    public class InputFormInteger : InputFormNumeric
    {
        /// <summary>
        /// Initialises the form with a message to display to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="defaultValue">The default integer value to display</param>
        public InputFormInteger(string message, int defaultValue) : base(message)
        {
            _numericUpDown.Value = defaultValue;
        }

        /// <summary>
        /// Returns the integer value from the form
        /// </summary>
        /// TODO ERIC - have a set here?
        public int Value
        {
            get { return Convert.ToInt32(_numericUpDown.Value); }
        }

        /// <summary>
        /// Creates a numeric up-down integer control for the form
        /// </summary>
        /// <returns>Returns the NumericUpDown control created</returns>
        protected override NumericUpDown CreateNumericUpDown()
        {
            return ControlFactory.CreateNumericUpDownInteger();
        }
    }
}