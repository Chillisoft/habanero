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
using Habanero.UI.Forms;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// A super-class for mappers of the numeric up-down facility
    /// </summary>
    public abstract class NumericUpDownMapper : ControlMapper
    {
        protected NumericUpDown _numericUpDown;

        /// <summary>
        /// Constructor to initialise the mapper
        /// </summary>
        /// <param name="control">The control to map</param>
        /// <param name="propName">The property name</param>
		/// <param name="isReadOnly">Whether this control is read only</param>
		public NumericUpDownMapper(NumericUpDown control, string propName, bool isReadOnly)
            : base(control, propName, isReadOnly)
        {
            _numericUpDown = control;
        }

        /// <summary>
        /// Updates the interface when the value has been changed in the
        /// object being represented
        /// </summary>
        protected override void ValueUpdated()
        {
            object propValue = GetPropertyValue();
            if (propValue == null)
            {
                _numericUpDown.Value = new Decimal(0);
            }
            else if (!propValue.Equals(_numericUpDown.Value))
            {
                _numericUpDown.Value = Convert.ToDecimal(propValue);
            }
        }
    }
}