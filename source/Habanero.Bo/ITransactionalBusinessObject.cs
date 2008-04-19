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
    }
}