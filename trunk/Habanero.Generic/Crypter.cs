namespace Habanero.Generic
{
    /// <summary>
    /// Interface for classes that provide an encryption facility.
    /// </summary>
    public interface Crypter
    {
        string DecryptString(string value);
        string EncryptString(string value);
    }
}