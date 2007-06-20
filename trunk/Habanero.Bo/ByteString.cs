using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Generic;

namespace Habanero.Bo
{
    /// <summary>
    /// Manages a long string that, when stored in the database, will need 
    /// to be stored as a byte array.
    /// In MySQL this type is usually reflected by the data type 'blob'.
    /// In SQLServer this type is usually reflected by the data type 'blob'.
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
            else
            {
                return false;
            }
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
