using System.Security.Cryptography;

namespace Habanero.Base
{
    /// <summary>
    /// An interface representing the ability of a class to support the decryption of RSA Encrypted information.
    /// </summary>
    public interface ISupportsRSADecryption
    {
        /// <summary>
        /// Sets the private key to use to decrypt the encrypted information.  The private key is in xml format.   
        /// </summary>
        /// <param name="xmlPrivateKey">The xml format of the RSA key (RSA.ToXmlString(true))</param>
        void SetPrivateKeyFromXML(string xmlPrivateKey);

        /// <summary>
        /// Sets the private key to use to decrypt the encrypted information. The private key is an RSA object.
        /// </summary>
        /// <param name="privateKey">The RSA object which has the private key</param>
        void SetPrivateKey(RSA privateKey);
    }
}