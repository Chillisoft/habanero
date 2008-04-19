using Habanero.Base;
using Habanero.BO.SqlGeneration;
using Habanero.DB;

namespace Habanero.BO
{
    ///<summary>
    /// Utility class that wraps the business object and implements a database persistance strategy for the business object.
    /// This class is used with allong with the Transaction Committer to implement transactional support
    /// for multiple business objects.
    ///</summary>
    public class TransactionalBusinessObjectDB
        : TransactionalBusinessObject
    {
        ///<summary>
        ///</summary>
        ///<param name="businessObject"></param>
        protected internal TransactionalBusinessObjectDB(BusinessObject businessObject) : base(businessObject)
        {
        }
        ///<summary>
        /// Returns the appropriate sql statement collection depending on the state of the object.
        /// E.g. Update SQL, InsertSQL or DeleteSQL.
        ///</summary>
        protected internal virtual ISqlStatementCollection GetSql()
        {
            if (IsNewAndDeleted()) return null;

            if (IsNew())
            {
                return GetInsertSql();
            }
            else if(IsDeleted)
            {
                return GetDeleteSql();
            }
            else
            {
                return GetUpdateSql();
            }
        }

        /// <summary>
        /// Returns an "insert" sql statement list for inserting this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private SqlStatementCollection GetInsertSql()
        {
            InsertStatementGenerator gen = new InsertStatementGenerator(_businessObject, _businessObject.GetDatabaseConnection());
            return gen.Generate();
        }
        /// <summary>
        /// Builds a "delete" sql statement list for this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private SqlStatementCollection GetDeleteSql()
        {
            DeleteStatementGenerator generator = new DeleteStatementGenerator(_businessObject, _businessObject.GetDatabaseConnection());
            return generator.Generate();
        }
        /// <summary>
        /// Returns an "update" sql statement list for updating this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private SqlStatementCollection GetUpdateSql()
        {
            UpdateStatementGenerator gen = new UpdateStatementGenerator(_businessObject, _businessObject.GetDatabaseConnection());
            return gen.Generate();
        }
    }
}
