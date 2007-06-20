namespace Habanero.Bo
{
    /// <summary>
    /// A factory to create objects
    /// </summary>
    /// TODO ERIC - does this have any use?
    public class ObjectFactory
    {
        private static ObjectFactory _objectFactory;

        /// <summary>
        /// Constructor to initialise a new factory
        /// </summary>
        private ObjectFactory()
        {
        }

        /// <summary>
        /// Returns the object factory stored in this instance
        /// </summary>
        /// <returns>Returns an ObjectFactory object</returns>
        public static ObjectFactory GetObjectFactory()
        {
            if (_objectFactory == null)
            {
                _objectFactory = new ObjectFactory();
            }
            return _objectFactory;
        }
    }
}