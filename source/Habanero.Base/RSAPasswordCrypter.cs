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
using Habanero.Base.Util;

namespace Habanero.Base
{
    /// <summary>
    /// This class can only encrypt short messages, so is really only useful for encrypting
    /// passwords or similar short phrases.
    /// </summary>
    public class RSAPasswordCrypter : ICrypter
    {
        private readonly RSAExtenderCF _rsa;

        ///<summary>
        /// Constructor
        ///</summary>
        ///<param name="rsa"></param>
        public RSAPasswordCrypter(RSA rsa)
        {
            _rsa = new RSAExtenderCF(rsa);
        }

        /// <summary>
        /// Returns the given string without carrying out any changes.
        /// </summary>
        /// <param name="value">The string to decrypt</param>
        /// <returns>Returns the unaltered string provided</returns>
        public string DecryptString(string value)
        {

            //throw new NotImplementedException("CF: Code commented out to get CF to compile");
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            var extendedProvider = new RSAExtenderCF(provider);
            extendedProvider.FromXmlString(_rsa.ToXmlString(true));
            byte[] passwordBytes = new byte[value.Length / 2];
            for (int i = 0; i < passwordBytes.Length; i++)
            {
                passwordBytes[i] = Convert.ToByte(value.Substring(i * 2, 2), 16);
            }
            byte[] encryptedBytes = provider.Decrypt(passwordBytes, false);
            return ASCIIEncoding.ASCII.GetString(encryptedBytes,0, encryptedBytes.Length);
        }

        /// <summary>
        /// Returns the given string without carrying out any changes.
        /// </summary>
        /// <param name="value">The string to encrypt</param>
        /// <returns>Returns the unaltered string provided</returns>
        public string EncryptString(string value)
        {

            //throw new NotImplementedException("CF: Code commented out to get CF to compile");
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            var extendedProvider = new RSAExtenderCF(provider);
            extendedProvider.FromXmlString(_rsa.ToXmlString(false));
            byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(value);
            byte[] encryptedByes = provider.Encrypt(passwordBytes, false);
            string text = "";
            foreach (byte bye in encryptedByes)
            {
                text += String.Format("{0:x2}", bye);
            }
            return text;
        }
    }
}
