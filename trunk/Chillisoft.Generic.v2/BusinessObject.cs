namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// An interface to model a business object
    /// </summary>
    /// TODO ERIC - rename this IBusinessObject (delete existing one)
    /// - is this interface needed?
    public interface BusinessObject
    {
        /// <summary>
        /// Returns the value held under the named property
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <returns>Returns the value</returns>
        object GetPropertyValue(string propertyName);
    }
}