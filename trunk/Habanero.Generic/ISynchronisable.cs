namespace Habanero.Base
{
    /// <summary>
    /// An interface to model objects that are synchronisable
    /// </summary>
    public interface ISynchronisable
    {
        /// <summary>
        /// Increments a version number used for synchronisation
        /// </summary>
        void IncrementVersionNumber();
    }
}