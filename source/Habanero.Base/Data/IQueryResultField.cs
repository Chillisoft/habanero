namespace Habanero.Base.Data
{
    public interface IQueryResultField
    {
        string PropertyName { get; }
        int Index { get; set; }
    }
}