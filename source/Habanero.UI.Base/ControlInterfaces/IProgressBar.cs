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

namespace Habanero.UI.Base
{
    /// <summary>
    /// Represents a progress bar control.
    /// </summary>
    public interface IProgressBar : IControlHabanero
    {
        /// <summary>
        /// Advances the current position of the progress bar by the specified amount.
        /// </summary>
        ///	<param name="value">The amount by which to increment the progress bar's current position. </param>
        void Increment(int value);

        /// <summary>
        /// Advances the current position of the progress bar by the amount of the Step property.
        /// </summary>
        void PerformStep();

        /// <summary>
        /// Gets or sets the maximum value of the range of the control.
        /// </summary>
        ///	<returns>The maximum value of the range. The default is 100.</returns>
        ///	<exception cref="T:System.ArgumentException">The value specified is less than 0. </exception>
        int Maximum { get; set; }

        /// <summary>
        /// Gets or sets the minimum value of the range of the control.
        /// </summary>
        ///	<returns>The minimum value of the range. The default is 0.</returns>
        ///	<exception cref="T:System.ArgumentException">The value specified for the property is less than 0. </exception>
        int Minimum { get; set; }

        /// <summary>
        /// Gets or sets the current position of the progress bar.
        /// </summary>
        ///	<returns>The position within the range of the progress bar. The default is 0.</returns>
        ///	<exception cref="T:System.ArgumentException">The value specified is greater than the value of the Maximum property.-or- The value specified is less than the value of the Minimum property. </exception>
        int Value { get; set; }

        /// <summary>
        /// Gets or sets the amount by which a call to the PerformStep method increases the current position of the progress bar.
        /// </summary>
        ///	<returns>The amount by which to increment the progress bar with each call to the PerformStep method. The default is 10.</returns>
        int Step { get; set; }
    }
}