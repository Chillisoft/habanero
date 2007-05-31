namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// An interface to model a synchronisation controller
    /// </summary>
    public interface SynchronisationController
    {
        /// <summary>
        /// Updates an object's synchronisation properties
        /// </summary>
        /// <param name="syncObject">The synchronisable object whose
        /// synchronisation properties need to be updated</param>
        void UpdateSynchronisationProperties(Synchronisable syncObject);
    }
}