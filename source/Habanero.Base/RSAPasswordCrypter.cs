//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Habanero.Base
{
    /// <summary>
    /// This class can only encrypt short messages, so is really only useful for encrypting
    /// passwords or similar short phrases.
    /// </summary>
    public class RSAPasswordCrypter : ICrypter {
        private RSA _rsa;

        public RSAPasswordCrypter(RSA rsa)
        {
            _rsa = rsa;
        }

        public string DecryptString(string value)
        {

            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(_rsa.ToXmlString(true));
             byte[] passwordBytes = new byte[value.Length/2];
                    for (int i = 0; i < passwordBytes.Length; i++) {
                        passwordBytes[i] = Convert.ToByte(value.Substring(i * 2, 2), 16);
                    }
                    byte[] encryptedByes = provider.Decrypt(passwordBytes, false);
                   return ASCIIEncoding.ASCII.GetString(encryptedByes);
        }

        public string EncryptString(string value)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(_rsa.ToXmlString(false));
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
