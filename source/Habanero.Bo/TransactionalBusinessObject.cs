namespace Habanero.BO
{
    ///<summary>
    /// This is a base class which is used as a wrapper (decorator around a business object)
    /// This class along with the TransactionCommiter implement transactional and persistence 
    /// strategies for the business object
    ///</summary>
    public class TransactionalBusinessObject : ITransactionalBusinessObject
    {
        protected readonly BusinessObject _businessObject;

        ///<summary>
        ///</summary>
        ///<param name="businessObject"></param>
        protected internal TransactionalBusinessObject(BusinessObject businessObject)
        {
            _businessObject = businessObject;
        }

        ///<summary>
        /// Returns the business object that this objects decorates.
        ///</summary>
        public BusinessObject BusinessObject
        {
            get { return _businessObject; }
        }

        /// <summary>
        /// Whether the business object's state is deleted
        /// </summary>
        public virtual bool IsDeleted
        {
            get { return _businessObject.State.IsDeleted; }
        }

        /// <summary>
        /// Whether the business object's state is new
        /// </summary>
        /// <returns></returns>
        public bool IsNew()
        {
            return _businessObject.State.IsNew;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected internal bool IsNewAndDeleted()
        {
            return _businessObject.State.IsNew && (_businessObject.State.IsDeleted);
        }

        ///<summary>
        /// Updates the business object as committed
        ///</summary>
        public virtual void UpdateStateAsCommitted()
        {
            _businessObject.UpdateStateAsPersisted();
        }

        /// <summary>
        /// Indicates whether all of the property values are valid
        /// </summary>
        /// <param name="invalidReason">A string to modify with a reason
        /// for any invalid values</param>
        /// <returns>Returns true if all are valid</returns>
        public virtual bool IsValid(out string invalidReason)
        {
            return _businessObject.IsValid(out invalidReason);
        }
    }
}