namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// An interface to model objects that are synchronisable
    /// </summary>
    /// TODO ERIC - rename to ISynchronisable
    public interface Synchronisable
    {
        /// <summary>
        /// Increments a version number used for synchronisation
        /// </summary>
        void IncrementVersionNumber();
    }
}