using System;
using System.Collections.Generic;
using System.Text;

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
