namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// An interface to implement a transaction log. There are many 
    /// strategies for implementing transaction logs, such as recording 
    /// to a database table, recording to a text file, etc.<br/><br/>
    /// This interface fulfils the roll of the Strategy Object in the 
    /// GOF Strategy pattern.<br/><br/>
    /// The combination of properties passed to the class in its 
    /// constructor, methods or properties should be able to provide 
    /// all required functionality to implement the strategy chosen.
    /// </summary>
    public interface ITransactionLog
    {
        /// <summary>
        /// Record a transaction log for the business object
        /// </summary>
        /// <param name="busObj">The business object the transaction log is 
        /// being recorded for</param>
        void RecordTransactionLog(BusinessObject busObj, string logonUserName);
    }
}