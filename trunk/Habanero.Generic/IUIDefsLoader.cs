using System.Collections;

namespace Habanero.Generic
{
    /// <summary>
    /// An interface to model a loader of user interface definitions
    /// </summary>
    public interface IUIDefsLoader
    {
        /// <summary>
        /// Loads UI definitions and returns them in a list
        /// </summary>
        /// <returns>Returns an IList object</returns>
        IList LoadUIDefs();
    }
}