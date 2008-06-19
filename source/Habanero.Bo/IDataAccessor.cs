using Habanero.Base;

namespace Habanero.BO
{
    public interface IDataAccessor
    {
        IBusinessObjectLoader BusinessObjectLoader { get; }

        ITransactionCommitterFactory TransactionCommiterFactory { get; }
    }
}