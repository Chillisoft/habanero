using System;
using Chillisoft.Generic.v2;
using log4net;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Provides a synchronisation controller for incrementing version numbers
    /// </summary>
    public class VersionNumberSynchronisationController : SynchronisationController
    {
        private static readonly ILog log =
            LogManager.GetLogger("Chillisoft.Bo.v2.VersionNumberSynchronisationController");

        /// <summary>
        /// Constructor to initialise a controller
        /// </summary>
        public VersionNumberSynchronisationController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Updates an object's synchronisation properties, by incrementing
        /// the version number
        /// </summary>
        /// <param name="syncObject">The synchronisable object whose
        /// synchronisation properties need to be updated</param>
        public void UpdateSynchronisationProperties(Synchronisable syncObject)
        {
            try
            {
                syncObject.IncrementVersionNumber();
            }
            catch (NotSupportedException ex)
            {
                log.Debug(ex.Message);
            }
        }
    }
}