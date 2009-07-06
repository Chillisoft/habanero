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

using Habanero.Base;

namespace Habanero.Util
{
    /// <summary>
    /// Manages a long string that, when stored in the database, will need 
    /// to be stored differently according to the database used.
    /// In MySql this type is usually reflected by the data type 'LongText'.
    /// In SqlServer this type is usually reflected by the data type 'Text'.
    /// In Oracle this type is usually reflected by the data type 'CLOB'.
    /// In Access this type is usually reflected by the data type 'Memo'.
    /// It is the Oracle data type in patricular that needs special treatment.
    /// </summary>
    public class LongText : CustomProperty
    {


        private string _longTextValue;

        ///<summary>
        /// Constructor to initialise a new long text string
        ///</summary>
        ///<param name="value">The long text string data</param>
        public LongText(string value)
            : this(value, false)
        {
        }

        /// <summary>
        /// Constructor to initialise a new long text string from the database or not
        /// </summary>
        /// <param name="value">The long text string data</param>
        /// <param name="isLoading">Is this value being loaded from the database</param>
        public LongText(object value, bool isLoading)
            : base(value, isLoading)
        {
            _longTextValue = (string) value;
        }

        /// <summary>
        /// Returns the value of the long text string
        /// </summary>
        public string Value
        {
            get { return _longTextValue; }
            set { _longTextValue = value; }
        }

        /// <summary>
        /// Returns the hashcode of the long text string
        /// </summary>
        /// <returns>Returns a hashcode integer</returns>
        public override int GetHashCode()
        {
            return _longTextValue.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the contents of the given LongText object
        /// equal the contents of this object
        /// </summary>
        /// <param name="obj">The LongText object to compare with</param>
        /// <returns>Returns true if equal</returns>
        public override bool Equals(object obj)
        {
            if (obj is LongText)
            {
                LongText compareTo = (LongText)obj;
                return compareTo.Value.Equals(_longTextValue);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the long text string
        /// </summary>
        /// <returns>Returns a string</returns>
        public override string ToString()
        {
            return _longTextValue;
        }

        /// <summary>
        /// Returns the value that is to be persisted to the database.
        /// </summary>
        /// <returns>Returns the long text value</returns>
        public override object GetPersistValue()
        {
            return _longTextValue;
        }

    }
}