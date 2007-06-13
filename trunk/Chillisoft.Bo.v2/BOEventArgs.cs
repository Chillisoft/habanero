namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Provides arguments to attach for an event involving business objects
    /// </summary>
    public class BOEventArgs
    {
        private BusinessObject _bo;

        /// <summary>
        /// Constructor to initialise a new set of arguments
        /// </summary>
        /// <param name="bo">The related business object</param>
        public BOEventArgs(BusinessObject bo)
        {
            _bo = bo;
        }

        /// <summary>
        /// Returns the business object related to the event
        /// </summary>
        public BusinessObject BusinessObject
        {
            get { return _bo; }
        }
    }
}