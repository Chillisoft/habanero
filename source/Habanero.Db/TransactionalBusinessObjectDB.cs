using Habanero.Base;
using Habanero.BO;
using log4net;

namespace Habanero.DB
{
    ///<summary>
    /// Utility class that wraps the business object and implements a database persistance strategy for the business object.
    /// This class is used with allong with the Transaction Committer to implement transactional support
    /// for multiple business objects.
    ///</summary>
    public class TransactionalBusinessObjectDB
        : TransactionalBusinessObject, ITransactionalDB
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.DB.TransactionalBusinessObjectDB");
        ///<summary>
        ///</summary>
        ///<param name="businessObject"></param>
        public TransactionalBusinessObjectDB(IBusinessObject businessObject) : base(businessObject)
        {
        }
        ///<summary>
        /// Returns the appropriate sql statement collection depending on the state of the object.
        /// E.g. Update SQL, InsertSQL or DeleteSQL.
        ///</summary>
        public virtual ISqlStatementCollection GetPersistSql()
        {
            if (IsNewAndDeleted()) return null;

            SqlStatementCollection sqlStatementCollection;
            if (IsNew())
            {
                sqlStatementCollection = GetInsertSql();
            } else if (IsDeleted)
            {
                sqlStatementCollection = GetDeleteSql();
            }
            else
            {
                sqlStatementCollection = GetUpdateSql();
            }
            IBOStatus boStatus = BusinessObject.Status;
            ITransactionalDB transactionLog = BusinessObject.TransactionLog as ITransactionalDB;
            if (transactionLog != null && (boStatus.IsNew || boStatus.IsDeleted || boStatus.IsDirty))
            {
                sqlStatementCollection.Add(transactionLog.GetPersistSql());
            }
            return sqlStatementCollection;
        }

        /// <summary>
        /// Returns an "insert" sql statement list for inserting this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private SqlStatementCollection GetInsertSql()
        {
            InsertStatementGenerator gen = new InsertStatementGenerator(BusinessObject, DatabaseConnection.CurrentConnection);
            return gen.Generate();
        }

        /// <summary>
        /// Builds a "delete" sql statement list for this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private SqlStatementCollection GetDeleteSql()
        {
            DeleteStatementGenerator generator = new DeleteStatementGenerator(BusinessObject, DatabaseConnection.CurrentConnection);
            return generator.Generate();
        }

        /// <summary>
        /// Returns an "update" sql statement list for updating this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private SqlStatementCollection GetUpdateSql()
        {
            UpdateStatementGenerator gen = new UpdateStatementGenerator(BusinessObject, DatabaseConnection.CurrentConnection);
            return gen.Generate();
        }

    }
}