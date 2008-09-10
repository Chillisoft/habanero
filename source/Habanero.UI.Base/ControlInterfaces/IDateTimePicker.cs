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

    #region Enums

    /// <summary>
    /// Specifies the date and time format the <see cref="IDateTimePicker"/> control displays.
    /// </summary>
    //[Serializable()]
    public enum DateTimePickerFormat
    {
        /// <summary>
        /// The <see cref="IDateTimePicker"></see> control displays the date/time value in a custom format.
        /// </summary>
        Custom = 8,
        /// <summary>
        /// The <see cref="IDateTimePicker"></see> control displays the date/time value in the long date format set by the user's operating system.
        /// </summary>
        Long = 1,
        /// <summary>
        /// The <see cref="IDateTimePicker"></see> control displays the date/time value in the short date format set by the user's operating system.
        /// </summary>
        Short = 2,
        /// <summary>
        /// The <see cref="IDateTimePicker"></see> control displays the date/time value in the time format set by the user's operating system.
        /// </summary>
        Time = 4
    }


    #endregion Enums

    /// <summary>
    /// Represents a DateTimePicker
    /// </summary>
    public interface IDateTimePicker : IControlHabanero
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
        /// Gets or sets the format of the date and time displayed in the control.
        /// </summary>
        ///	<returns>One of the <see cref="DateTimePickerFormat"></see> values. The default is <see cref="DateTimePickerFormat.Long"></see>.</returns>
        ///	<exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="DateTimePickerFormat"></see> values. </exception>
        DateTimePickerFormat Format { get; set; }

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

        ///<summary>
        /// The text that will be displayed when the Value is null
        ///</summary>
        string NullDisplayValue { get; set; }

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