using System;

namespace Habanero.BO
{
    ///<summary>
    /// This is a base class which is used as a wrapper (decorator around a business object)
    /// This class along with the TransactionCommiter implement transactional and persistence 
    /// strategies for the business object
    ///</summary>
    public  class TransactionalBusinessObject
    {
        private readonly BusinessObject _businessObject;

        ///<summary>
        ///</summary>
        ///<param name="businessObject"></param>
        protected internal TransactionalBusinessObject(BusinessObject businessObject)
        {
            if (businessObject == null) throw new ArgumentNullException("businessObject");

            _businessObject = businessObject;
        }

        ///<summary>
        /// Returns the business object that this objects decorates.
        ///</summary>
        protected internal BusinessObject BusinessObject
        {
            get { return _businessObject; }
        }

        /// <summary>
        /// Whether the business object's state is deleted
        /// </summary>
        protected internal virtual bool IsDeleted
        {
            get { return _businessObject.State.IsDeleted; }
        }

        /// <summary>
        /// Whether the business object's state is new
        /// </summary>
        /// <returns></returns>
        protected internal bool IsNew()
        {
            return _businessObject.State.IsNew;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected internal  bool IsNewAndDeleted()
        {
            return _businessObject.State.IsNew && (_businessObject.State.IsDeleted);
        }

        ///<summary>
        /// Updates the business object as committed
        ///</summary>
        protected internal virtual void UpdateStateAsCommitted()
        {
            _businessObject.UpdateStateAsPersisted();
        }

        /// <summary>
        /// Indicates whether all of the property values are valid
        /// </summary>
        /// <param name="invalidReason">A string to modify with a reason
        /// for any invalid values</param>
        /// <returns>Returns true if all are valid</returns>
        protected internal virtual bool IsValid(out string invalidReason)
        {
            return _businessObject.IsValid(out invalidReason);
        }

        protected static string GetDuplicateObjectErrMsg(BOKey boKey, string classDisplayName)
        {
            string errMsg;
            string propNames = "";
            foreach (BOProp prop in boKey.GetBOPropCol())
            {
                if (propNames.Length > 0) propNames += ", ";
                propNames += string.Format("{0} = {1}", prop.PropertyName, prop.Value);
            }
            errMsg =
                string.Format("A '{0}' already exists with the same identifier: {1}.",
                              classDisplayName, propNames);
            return errMsg;
        }

        ///<summary>
        /// Executes any custom code required by the business object before it is persisted to the database.
        /// This has the additionl capability of creating or updating other business objects and adding these
        /// to the transaction committer.
        ///</summary>
        ///<param name="transactionCommitter">the transaction committer that is executing the transaction</param>
        protected internal virtual void UpdateObjectBeforePersisting(TransactionCommitter transactionCommitter)
        {
            this.BusinessObject.UpdateObjectBeforePersisting(transactionCommitter);
        }

        ///<summary>
        /// Checks the underlying business object for any concurrency control errors before trying to commit to 
        /// the datasource
        ///</summary>
        protected internal virtual void CheckForConcurrencyErrors()
        {
            this.BusinessObject.CheckConcurrencyBeforePersisting();
        }

        protected internal virtual bool HasDuplicateIdentifier(out string errMsg)
        {
            errMsg = "";
            return false;
        }

        protected internal virtual void UpdateAsRolledBack()
        {
            this.BusinessObject.UpdateAsTransactionRolledBack();
        }
    }
}