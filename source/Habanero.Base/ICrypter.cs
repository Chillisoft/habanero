namespace Habanero.Base
{
    /// <summary>
    /// Interface for classes that provide an encryption facility.
    /// </summary>
    public interface ICrypter
    {
        string DecryptString(string value);
        string EncryptString(string value);
    }
}