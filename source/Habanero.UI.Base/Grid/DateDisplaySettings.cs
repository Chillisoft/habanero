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

namespace Habanero.UI.Base
{
    /// <summary>
    /// Stores date display settings that define how dates should
    /// be displayed to users in various user interfaces
    /// </summary>
    public class DateDisplaySettings
    {
        private string _gridDateFormat;

        /// <summary>
        /// Gets and sets the .Net style date format string that
        /// determines how a date is displayed in a grid.
        /// Set this value to null to use the short
        /// date format of the underlying user's environment.
        /// The format for this string is the same as that of
        /// DateTime.ToString(), including shortcuts such as
        /// "d" which use the short date format of the culture
        /// on the user's machine.
        /// </summary>
        public string GridDateFormat
        {
            get { return _gridDateFormat; }
            set { _gridDateFormat = value; }
        }
    }
}
