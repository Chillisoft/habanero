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