using Habanero.Base;

namespace Habanero.Bo
{
    /// <summary>
    /// A basic lookup-list content provider that stores a collection of
    /// string-Guid pairs as provided in the constructor.
    /// A lookup-list is typically used to populate features like a ComboBox,
    /// where the string would be displayed, but the Guid would be the
    /// value stored (for reasons of data integrity).
    /// </summary>
    public class SimpleLookupListSource : ILookupListSource
    {
        private StringGuidPairCollection _lookupListCollection;

        /// <summary>
        /// Constructor to initialise the provider with a specified
        /// collection of string-Guid pairs
        /// </summary>
        /// <param name="collection">The string-Guid pair collection</param>
        public SimpleLookupListSource(StringGuidPairCollection collection)
        {
            _lookupListCollection = collection;
        }

        /// <summary>
        /// Returns the lookup list contents being held
        /// </summary>
        /// <returns>Returns a StringGuidPairCollection object</returns>
        public StringGuidPairCollection GetLookupList()
        {
            return _lookupListCollection;
        }

        /// <summary>
        /// Returns the lookup list contents being held
        /// </summary>
        /// <param name="connection">This can be set to null as it plays
        /// no role</param>
        /// <returns>Returns a StringGuidPairCollection object</returns>
        public StringGuidPairCollection GetLookupList(IDatabaseConnection connection)
        {
            return _lookupListCollection;
        }
    }
}