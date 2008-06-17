using Habanero.Base;

namespace Habanero.BO
{
    public class TransactionCommitterFactoryInMemory : ITransactionCommitterFactory
    {
        private readonly DataStoreInMemory _dataStoreInMemory;

        public TransactionCommitterFactoryInMemory(DataStoreInMemory dataStoreInMemory)
        {
            _dataStoreInMemory = dataStoreInMemory;
        }

        public ITransactionCommitter CreateTransactionCommitter()
        {
            return new TransactionCommitterInMemory(_dataStoreInMemory);
        }


    }
}