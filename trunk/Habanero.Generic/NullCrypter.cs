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