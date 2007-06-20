using Habanero.Generic;

namespace Habanero.Bo
{
    /// <summary>
    /// Provides an empty lookup-list
    /// </summary>
    public class NullLookupListSource : ILookupListSource
    {
        /// <summary>
        /// Returns a new empty lookup-list
        /// </summary>
        /// <returns>Returns an empty lookup-list</returns>
        public StringGuidPairCollection GetLookupList()
        {
            return new StringGuidPairCollection();
        }

        /// <summary>
        /// Returns a new empty lookup-list
        /// </summary>
        /// <param name="connection">A parameter preserved for polymorphism.
        /// This can be set to null.</param>
        /// <returns>Returns an empty lookup-list</returns>
        public StringGuidPairCollection GetLookupList(IDatabaseConnection connection)
        {
            return new StringGuidPairCollection();
        }
    }
}