using System;
using Habanero.Generic;
using log4net;

namespace Habanero.Bo
{
    /// <summary>
    /// Provides a synchronisation controller for incrementing version numbers
    /// </summary>
    public class VersionNumberSynchronisationController : ISynchronisationController
    {
        private static readonly ILog log =
            LogManager.GetLogger("Habanero.Bo.VersionNumberSynchronisationController");

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
        public void UpdateSynchronisationProperties(ISynchronisable syncObject)
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