namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// An interface to model optimistic or pessimistic concurrency
    /// control, as used by business objects.
    /// This interface fulfills the roll of the Strategy Object 
    /// in the GOF Strategy pattern.
    /// <br/><br/>
    /// Since this architecture/framework supports the storing of 
    /// objects in an object manager, it is possible to retrieve a 
    /// stale object that has since been edited by another user or process.
    /// This interface allows you to implement a concurrency control 
    /// strategy to deal with this by
    /// raising an error, automatically refreshing the object, putting 
    /// a read lock on the object or any other 
    /// strategy that you wish to implement.
    /// </summary>
    public interface IConcurrencyControl
    {
        /// <summary>
        /// Checks concurrency when retrieving an object from the object
        /// manager, in order to ensure that up-to-date information is
        /// displayed to the user
        /// </summary>
        /// <param name="busObj">The business object to be loaded</param>
        void CheckConcurrencyOnGettingObjectFromObjectManager(BusinessObjectBase busObj);

        /// <summary>
        /// Checks concurrency before the user begins editing an object, in
        /// order to avoid the user making changes to an object and then losing
        /// those changes when the committal process shows a concurrency
        /// failure
        /// </summary>
        /// <param name="busObj">The business object to be edited</param>
        void CheckConcurrencyBeforeBeginEditing(BusinessObjectBase busObj);

        /// <summary>
        /// Checks concurrency before persisting an object to the database
        /// in order to prevent one of two conflicting copies from being lost
        /// </summary>
        /// <param name="busObj">The business object to be persisted</param>
        void CheckConcurrencyBeforePersisting(BusinessObjectBase busObj);

        /// <summary>
        /// Many optimistic concurrency control strategies rely on updating 
        /// certain properties in the database, such as the version number,
        /// time last updated, etc. This method must be implemented in the 
        /// "concrete concurrency control strategy" to update the
        /// appropriate properties before the object is persisted to the 
        /// database.
        /// </summary>
        void UpdatePropertiesWithLatestConcurrencyInfo();

        /// <summary>
        /// If your concurrency control strategy involves read locks, then 
        /// this method must be implemented to release the read locks.
        /// </summary>
        void ReleaseReadLocks();

        /// <summary>
        /// If your concurrency control strategy involves write locks, then 
        /// this method must be implemented to release the write locks.
        /// </summary>
        void ReleaseWriteLocks();
    }
}