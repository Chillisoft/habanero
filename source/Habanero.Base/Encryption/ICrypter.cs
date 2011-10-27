// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
namespace Habanero.Base
{
    /// <summary>
    /// Interface for classes that provide an encryption facility.
    /// </summary>
    public interface ICrypter
    {
        /// <summary>
        /// Returns the decrypted form of the given string
        /// </summary>
        /// <param name="value">The string to decrypt</param>
        /// <returns>Returns the decrypted string</returns>
        string DecryptString(string value);

        /// <summary>
        /// Returns the encrypted form of the given string
        /// </summary>
        /// <param name="value">The string to encrypt</param>
        /// <returns>Returns the encrypted string</returns>
        string EncryptString(string value);
    }
}