namespace Habanero.BO
{
    /// <summary>
    /// An interface that models the ability to load a business object
    /// collection
    /// </summary>
    public interface IBusinessObjectCollectionLoader
    {
        /// <summary>
        /// Loads a business object collection
        /// </summary>
        /// <returns>Returns the collection loaded</returns>
        BusinessObjectCollection<BusinessObject> Load();
    }
}