using Habanero.BO;

namespace Habanero.Test.BO
{
    public interface INumberGenerator
    {
        int NextNumber();
        void SetSequenceNumber(int newSequenceNumber);
        void AddToTransaction(TransactionCommitter transactionCommitter);
    }
}