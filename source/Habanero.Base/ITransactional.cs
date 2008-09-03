namespace Habanero.Base
{
    ///<summary>
    ///</summary>
    public interface ITransactional
    {
        ///<summary>
        ///</summary>
        ///<returns>The ID that uniquelty identifies this item of the transaction. In the case of business objects the object Id.
        /// for non business objects that no natural id exists for the particular transactional item a guid that uniquely identifies 
        /// transactional item should be generated. This is used by the transaction committer to ensure that the transactional item
        /// is not added twice in error.</returns>
        string TransactionID();

        ///<summary>
        /// Updates the business object as committed
        ///</summary>
        void UpdateStateAsCommitted();

        ///<summary>
        /// updates the object as rolled back
        ///</summary>
        void UpdateAsRolledBack();

    }
}