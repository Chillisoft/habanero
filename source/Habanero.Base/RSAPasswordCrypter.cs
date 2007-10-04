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
