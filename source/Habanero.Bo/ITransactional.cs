namespace Habanero.BO
{
    ///<summary>
    ///</summary>
    public interface ITransactional
    {

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