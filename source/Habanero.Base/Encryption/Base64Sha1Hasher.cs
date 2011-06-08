using System;
using System.Security.Cryptography;

namespace Habanero.Base
{
    ///<summary>
    /// Hashes a base64 string
    ///</summary>
    public class Base64Sha1Hasher : IHasher
    {
        public string HashString(string value)
        {
            if (value == null) value = "";
            var encrypter = SHA1.Create();
            while (value.Length % 4 > 0)
            {
                value = value + "+";
            }
            var valueByteArray = GetPasswordByteArray(value);
            return Convert.ToBase64String(encrypter.ComputeHash(valueByteArray));
        }

        private static byte[] GetPasswordByteArray(string value)
        {
            return string.IsNullOrEmpty(value) ? new byte[0] : Convert.FromBase64String(value);
        }
    }
}