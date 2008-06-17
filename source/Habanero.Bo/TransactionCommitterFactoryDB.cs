using Habanero.Base;

namespace Habanero.BO
{
    public class TransactionCommitterFactoryDB : ITransactionCommitterFactory
    {

        public ITransactionCommitter CreateTransactionCommitter()
        {
            return new TransactionCommitterDB();
        }

    }
}