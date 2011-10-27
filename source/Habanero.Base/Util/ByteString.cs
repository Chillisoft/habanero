//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
using System.Text;
using Habanero.Base;

namespace Habanero.Util
{
    /// <summary>
    /// Manages a long string that, when stored in the database, will need 
    /// to be stored as a byte array.
    /// In MySql this type is usually reflected by the data type 'blob'.
    /// In SqlServer this type is usually reflected by the data type 'blob'.
    /// In Oracle this type is usually reflected by the data type 'blob'.
    /// In Access this type is usually reflected by the data type 'OLE Object'.
    /// </summary>
    public class ByteString : CustomProperty
    {
        private string _textValue;

        ///<summary>
        /// Constructor to initialise a new long text string
        ///</summary>
        ///<param name="value">The ByteString string data</param>
        public ByteString(string value)
            : this((object)value, false)
        {
        }

        /// <summary>
        /// Constructor to initialise a new Byte String from the database or not
        /// </summary>
        /// <param name="value">The Byte or String data</param>
        /// <param name="isLoading">Is this value being loaded from the database</param>
        public ByteString(object value, bool isLoading)
            : base(value, isLoading)
        {
            if (value is string)
            {
                _textValue = (string)value;
            }
            else if (value is byte[])
            {
                _textValue = Encoding.Unicode.GetString((byte[])value);
            } else 
            {
                _textValue = value.ToString();
            }
            
        }

        /// <summary>
        /// Returns the value of the string
        /// </summary>
        public string Value
        {
            get { return _textValue; }
            set { _textValue = value; }
        }

        /// <summary>
        /// Returns the hashcode of the Byte string
        /// </summary>
        /// <returns>Returns a hashcode integer</returns>
        public override int GetHashCode()
        {
            return _textValue.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the contents of the given ByteString object
        /// equal the contents of this object
        /// </summary>
        /// <param name="obj">The ByteString object to compare with</param>
        /// <returns>Returns true if equal</returns>
        public override bool Equals(object obj)
        {
            if (obj is ByteString)
            {
                ByteString compareTo = (ByteString)obj;
                return compareTo.Value.Equals(_textValue);
            }
            return false;
        }

        /// <summary>
        /// Returns the text string
        /// </summary>
        /// <returns>Returns a string</returns>
        public override string ToString()
        {
            return _textValue;
        }

        /// <summary>
        /// Returns the value that is to be persisted to the database.
        /// </summary>
        /// <returns>Returns the byte array value</returns>
        public override object GetPersistValue()
        {
            byte[] byteArr = Encoding.Unicode.GetBytes(_textValue);
            return byteArr;
        }

    }
}