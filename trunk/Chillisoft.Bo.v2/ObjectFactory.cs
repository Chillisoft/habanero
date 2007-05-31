namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// A factory to create objects
    /// </summary>
    /// TODO ERIC - does this have any use?
    public class ObjectFactory
    {
        private static ObjectFactory mObjectFactory;

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
            if (mObjectFactory == null)
            {
                mObjectFactory = new ObjectFactory();
            }
            return mObjectFactory;
        }
    }
}