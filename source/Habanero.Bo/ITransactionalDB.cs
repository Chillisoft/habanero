using Habanero.Base;

namespace Habanero.BO
{
    ///<summary>
    /// Interface that is to be implemented for transactional object that are being updated
    /// to the database.
    ///</summary>
    public interface ITransactionalDB:ITransactional
    {
        ///<summary>
        /// Returns the appropriate sql statement collection depending on the state of the object.
        /// E.g. Update SQL, InsertSQL or DeleteSQL.
        ///</summary>
        ISqlStatementCollection GetSql();
    }
}