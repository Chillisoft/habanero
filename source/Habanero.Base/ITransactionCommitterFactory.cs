namespace Habanero.Base
{
    public interface ITransactionCommitterFactory
    {
        ITransactionCommitter CreateTransactionCommitter();
    }
}