namespace Habanero.Base
{
    public interface ITransactionCommitter
    {
        void AddBusinessObject(IBusinessObject businessObject);
        void CommitTransaction();
        void AddTransaction(ITransactional transactional);
    }
}