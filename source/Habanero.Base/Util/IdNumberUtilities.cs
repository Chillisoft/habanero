//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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

namespace Habanero.Util
{
    /// <summary>
    /// Provides utilities to manipulate personal ID number data
    /// </summary>
    public class IdNumberUtilities
    {
        /// <summary>
        /// Calculates the date of birth of a person given a South African 
        /// ID number
        /// </summary>
        /// <param name="idNumber">The South African ID number, which must 
        /// include at least the first six characters</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by the
        /// constructor of DateTime if the month or day are out of range
        /// </exception>
        /// <exception cref="FormatException">Thrown if the ID number is less 
        /// than six characters</exception>
        public static DateTime GetDateOfBirth(string idNumber) {
            if (idNumber.Length < 6) throw new FormatException("An ID number must have at least 6 digits to calculate the date of birth."); 
            int year = Convert.ToInt32( idNumber.Substring(0, 2));
            year = (year > DateTime.Now.Year%100) ? year + 1900 : year + 2000;
            int month = Convert.ToInt32( idNumber.Substring(2, 2));
            int day = Convert.ToInt32(idNumber.Substring( 4, 2));
            return new DateTime(year, month, day);
        }
    }
}
