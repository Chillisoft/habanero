namespace Habanero.Base
{
    /// <summary>
    /// Interface for classes that provide a hashing facility.
    /// </summary>
    public interface IHasher
    {
        /// <summary>
        /// Returns a hash created out the string given. Each time the string is Hashed it will return the same hash. This is a one way encryption mechanism.
        /// </summary>
        /// <param name="value">The string to hash</param>
        /// <returns>The hash</returns>
        string HashString(string value);
    }
}