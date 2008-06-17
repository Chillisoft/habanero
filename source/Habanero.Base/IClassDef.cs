using Habanero.Base.Exceptions;

namespace Habanero.Base
{
    public interface IClassDef
    {
        /// <summary>
        /// Searches the property definition collection and returns the 
        /// property definition for the property with the name provided.
        /// </summary>
        /// <param name="propertyName">The property name in question</param>
        /// <returns>Returns the property definition if found, or
        /// throws an error if not</returns>
        /// <exception cref="InvalidPropertyNameException">
        /// This exception is thrown if the property is not found</exception>
        IPropDef GetPropDef(string propertyName);

        /// <summary>
        /// Searches the property definition collection and returns 
        /// the lookup-list found under the property with the
        /// name specified.  Also checks the super-class.
        /// </summary>
        /// <param name="propertyName">The property name in question</param>
        /// <returns>Returns the lookup-list if the property is
        /// found, or a NullLookupList object if not</returns>
        ILookupList GetLookupList(string propertyName);


        /// <summary>
        /// Creates a new business object using this class definition
        /// </summary>
        /// <returns>Returns the new object</returns>
        IBusinessObject CreateNewBusinessObject();
    }
}