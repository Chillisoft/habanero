#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.Security.Cryptography;
using System.Text;

namespace Habanero.Base
{
    ///<summary>
    /// Hashes a UTF8 string
    ///</summary>
    public class Utf8Sha1Hasher : IHasher
    {
        /// <summary>
        /// Creates a Hash of a UTF8 string using Sha1
        /// </summary>
        /// <param name="value">The string to hash</param>
        /// <returns>The hashed string as a Base64 string</returns>
        public string HashString(string value)
        {
            if (value == null) value = "";
            var encrypter = SHA1.Create();
            while (value.Length % 4 > 0)
            {
                value = value + "+";
            }
            var valueByteArray = GetPasswordByteArray(value);
            var hash = encrypter.ComputeHash(valueByteArray);
            return Convert.ToBase64String(hash);
        }

        private static byte[] GetPasswordByteArray(string value)
        {
            return string.IsNullOrEmpty(value) ? new byte[0] : Encoding.UTF8.GetBytes(value);
        }
    }
}