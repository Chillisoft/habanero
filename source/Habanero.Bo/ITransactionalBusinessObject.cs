namespace Habanero.BO
{
    ///<summary>
    ///</summary>
    public interface ITransactionalBusinessObject
    {
        ///<summary>
        /// Returns the business object that this objects decorates.
        ///</summary>
        BusinessObject BusinessObject
        {
            get;
        }

        /// <summary>
        /// Whether the business object's state is deleted
        /// </summary>
        bool IsDeleted
        {
            get;
        }

        /// <summary>
        /// Whether the business object's state is new
        /// </summary>
        /// <returns></returns>
        bool IsNew();

        ///<summary>
        /// Updates the business object as committed
        ///</summary>
        void UpdateStateAsCommitted();

        /// <summary>
        /// Indicates whether all of the property values are valid
        /// </summary>
        /// <param name="invalidReason">A string to modify with a reason
        /// for any invalid values</param>
        /// <returns>Returns true if all are valid</returns>
        bool IsValid(out string invalidReason);

        /// <summary>
        /// Indicates whether there is a duplicate of this object in the data store based on
        /// an alternate identifier.
        /// eg. for a database this will select from the table to find an object
        /// that matches this object's alternate key. In this case this object would be
        /// a duplicate.
        /// </summary>
        /// <param name="errMsg">The description of the duplicate</param>
        /// <returns>Whether a duplicate of this object exists in the data store (based on the alternate key)</returns>
        bool HasDuplicateIdentifier(out string errMsg);

        ///<summary>
        /// Executes any custom code required by the business object before it is persisted to the database.
        /// This has the additionl capability of creating or updating other business objects and adding these
        /// to the transaction committer.
        ///</summary>
        ///<param name="transactionCommitter">the transaction committer that is executing the transaction</param>
        void UpdateObjectBeforePersisting(TransactionCommitter transactionCommitter);
    }
}