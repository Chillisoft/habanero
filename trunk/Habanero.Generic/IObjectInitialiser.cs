using System.Data;

namespace Habanero.Generic
{
    /// <summary>
    /// An interface to model an object initialiser
    /// </summary>
    public interface IObjectInitialiser
    {
        /// <summary>
        /// Initialises the given object
        /// </summary>
        /// <param name="objToInitialise">The object to initialise</param>
        void InitialiseObject(object objToInitialise);

        /// <summary>
        /// Initialises a DataRow object
        /// </summary>
        /// <param name="row">The DataRow object to initialise</param>
        void InitialiseDataRow(DataRow row);
    }
}