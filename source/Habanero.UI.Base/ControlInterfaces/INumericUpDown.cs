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

namespace Habanero.UI.Base
{
    /// <summary>
    /// Represents a spin box (also known as an up-down control) that displays numeric values
    /// </summary>
    public interface INumericUpDown : IControlHabanero
    {
        /// <summary>
        /// Occurs when the control is entered
        /// </summary>
        event EventHandler Enter;

        /// <summary>
        /// Occurs when the <see cref="INumericUpDown"/>.<see cref="INumericUpDown.Value"/> property has been changed in some way.
        /// </summary>
        event EventHandler ValueChanged;

        /// <summary>
        /// Gets or sets the number of decimal places to display. The default is 0.
        /// </summary>
        int DecimalPlaces { get; set; }

        /// <summary>
        /// Gets or sets the minimum allowed value
        /// </summary>
        decimal Minimum { get; set; }

        /// <summary>
        /// Gets or sets the maximum value
        /// </summary>
        decimal Maximum { get; set; }

        /// <summary>
        /// Gets or sets the value assigned
        /// </summary>
        decimal Value { get; set; }

        /// <summary>
        /// Selects a range of text in the spin box (also known as an up-down control)
        /// specifying the starting position and number of characters to select
        /// </summary>
        /// <param name="i">The position of the first character to be selected</param>
        /// <param name="length">The total number of characters to be selected</param>
        void Select(int i, int length);

        /// <summary>
        /// Gets or sets the alignment of text in the up-down control
        /// </summary>
        HorizontalAlignment TextAlign { get; set; }
    }

}