namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// Provides a synchronisation controller that does not implement
    /// any synchronisation strategy
    /// </summary>
    public class NullSynchronisationController : SynchronisationController
    {
        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="syncObject">This can be set to null</param>
        public void UpdateSynchronisationProperties(Synchronisable syncObject)
        {
        }
    }
}