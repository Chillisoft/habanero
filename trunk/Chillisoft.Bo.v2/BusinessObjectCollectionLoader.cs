namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// An interface that models the ability to load a business object
    /// collection
    /// </summary>
    /// TODO ERIC - rename to IBusinessObjectCollectionLoader
    public interface BusinessObjectCollectionLoader
    {
        /// <summary>
        /// Loads a business object collection
        /// </summary>
        /// <returns>Returns the collection loaded</returns>
        BusinessObjectBaseCollection Load();
    }
}