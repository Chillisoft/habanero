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
    /// Represents a DateTimePicker
    /// </summary>
    public interface IDateTimePicker : IControlChilli
    {
        /// <summary>
        /// Gets or sets the date/time value assigned to the control.
        /// </summary>
        DateTime Value { get; set; }

        /// <summary>
        /// Gets or sets the date/time value assigned to the control, but returns
        /// null if there is no date set in the picker
        /// </summary>
        DateTime? ValueOrNull { get; set; }

        /// <summary>
        /// Gets or sets the custom date/time format string
        /// </summary>
        string CustomFormat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a spin button control
        /// (also known as an up-down control) is used to adjust the date/time value
        /// </summary>
        bool ShowUpDown { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a check box is
        /// displayed to the left of the selected date
        /// </summary>
        bool ShowCheckBox { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Value property has
        /// been set with a valid date/time value and the displayed value is able to be updated
        /// </summary>
        bool Checked { get; set; }

        /// <summary>
        /// Occurs when the control is entered
        /// </summary>
        event EventHandler Enter;

        /// <summary>
        /// Occurs when the Value property changes
        /// </summary>
        event EventHandler ValueChanged;
    }
}