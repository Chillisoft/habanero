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
using System;
using System.Security.Cryptography;
using System.Text;

namespace Habanero.Base
{
    /// <summary>
    /// This class can only encrypt short messages, so is really only useful for encrypting
    /// passwords or similar short phrases.
    /// </summary>
    public class RSAPasswordCrypter : ICrypter
    {
        private readonly string _keyContainerName;

        ///<summary>
        /// Constructor
        ///</summary>
        ///<param name="keyContainerName"></param>
        public RSAPasswordCrypter(string keyContainerName)
        {
            _keyContainerName = keyContainerName;
        }

        /// <summary>
        /// Returns the given string without carrying out any changes.
        /// </summary>
        /// <param name="value">The string to decrypt</param>
        /// <returns>Returns the unaltered string provided</returns>
        public string DecryptString(string data)
        {
            byte[] dataToDecrypt = Convert.FromBase64String(data);
            byte[] decryptedData = Decrypt(dataToDecrypt, _keyContainerName);
            return Encoding.ASCII.GetString(decryptedData, 0, decryptedData.Length);
        }

        /// <summary>
        /// Returns the given string without carrying out any changes.
        /// </summary>
        /// <param name="data">The string to encrypt</param>
        /// <returns>Returns the unaltered string provided</returns>
        public string EncryptString(string data)
        {
            var dataToEncrypt = Encoding.ASCII.GetBytes(data);
            var encryptedData = Encrypt(dataToEncrypt, _keyContainerName);
            return Convert.ToBase64String(encryptedData);
        }

        private byte[] Encrypt(byte[] data, string keyContainerName)
        {
            var csp = new CspParameters {KeyContainerName = keyContainerName};
            var rsa = new RSACryptoServiceProvider(csp);
            return rsa.Encrypt(data, false);
        }

        private byte[] Decrypt(byte[] data, string keyContainerName)
        {
            var csp = new CspParameters {KeyContainerName = keyContainerName};
            var rsa = new RSACryptoServiceProvider(csp);
            return rsa.Decrypt(data, false);
        }
    }
}
