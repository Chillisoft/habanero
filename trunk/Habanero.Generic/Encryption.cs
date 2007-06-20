namespace Habanero.Generic
{
    /// <summary>
    /// An enumeration to designate which type of encryption is to be
    /// carried out.
    /// </summary>
    public enum Encryption
    {
        /// <summary>No encryption or decryption is carried out</summary>
        None,
        /// <summary>Encryption and decryption are done using the Blowfish algorithm</summary>
        Blowfish
    }
}