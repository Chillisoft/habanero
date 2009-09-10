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

namespace Habanero.Base
{
    /// <summary>
    /// An encrypter that carries out null change on the given string. 
    /// In other words, both the decrypt and encrypt methods simply return the
    /// string provided without changing it.
    /// </summary>
    public class NullCrypter : ICrypter
    {
        /// <summary>
        /// Returns the given string without carrying out any changes.
        /// </summary>
        /// <param name="value">The string to decrypt</param>
        /// <returns>Returns the unaltered string provided</returns>
        public string DecryptString(string value)
        {
            return value;
        }

        /// <summary>
        /// Returns the given string without carrying out any changes.
        /// </summary>
        /// <param name="value">The string to encrypt</param>
        /// <returns>Returns the unaltered string provided</returns>
        public string EncryptString(string value)
        {
            return value;
        }
    }
}