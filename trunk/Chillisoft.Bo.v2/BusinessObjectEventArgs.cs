namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Provides arguments to attach for an event involving business objects
    /// </summary>
    public class BusinessObjectEventArgs
    {
        private BusinessObjectBase _bo;

        /// <summary>
        /// Constructor to initialise a new set of arguments
        /// </summary>
        /// <param name="bo">The related business object</param>
        public BusinessObjectEventArgs(BusinessObjectBase bo)
        {
            _bo = bo;
        }

        /// <summary>
        /// Returns the business object related to the event
        /// </summary>
        public BusinessObjectBase BusinessObject
        {
            get { return _bo; }
        }
    }
}