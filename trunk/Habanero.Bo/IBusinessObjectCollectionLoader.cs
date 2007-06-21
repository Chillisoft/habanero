namespace Habanero.Bo
{
    /// <summary>
    /// An interface that models the ability to load a business object
    /// collection
    /// </summary>
    /// TODO ERIC - rename to IBusinessObjectCollectionLoader
    public interface IBusinessObjectCollectionLoader
    {
        /// <summary>
        /// Loads a business object collection
        /// </summary>
        /// <returns>Returns the collection loaded</returns>
        BusinessObjectCollection Load();
    }
}