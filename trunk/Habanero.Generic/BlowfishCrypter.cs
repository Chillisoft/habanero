using Blowfish_NET;

namespace Habanero.Generic
{
    /// <summary>
    /// An encrypter that uses the Blowfish encryption algorithm to
    /// encrypt or decrypt a given string.
    /// </summary>
    public class BlowfishCrypter : Crypter
    {
        private BlowfishSimple _blowfish;

        /// <summary>
        /// Initialises the blowfish encrypter using a pre-determined key
        /// </summary>
        public BlowfishCrypter()
        {
            _blowfish = new BlowfishSimple("mistral");
        }

        /// <summary>
        /// Takes a string that is assumed to be encrypted and converts it
        /// back to its usual form.
        /// </summary>
        /// <param name="value">The string to be decrypted</param>
        /// <returns>Returns the decrypted form of the given string</returns>
        public string DecryptString(string value)
        {
            return _blowfish.Decrypt(value);
        }

        /// <summary>
        /// Takes a given string and encrypts it using the Blowfish
        /// algorithm.
        /// </summary>
        /// <param name="value">The string to be encrypted</param>
        /// <returns>Returns the encrypted form of the given string</returns>
        public string EncryptString(string value)
        {
            return _blowfish.Encrypt(value);
        }
    }
}