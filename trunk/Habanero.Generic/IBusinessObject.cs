namespace Habanero.Generic
{
    /// <summary>
    /// An interface to model a business object
    /// </summary>
    public interface IBusinessObject
    {
        /// <summary>
        /// Returns the value held under the named property
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <returns>Returns the value</returns>
        object GetPropertyValue(string propertyName);
    }
}