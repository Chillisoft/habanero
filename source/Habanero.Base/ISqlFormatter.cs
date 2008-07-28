namespace Habanero.Base
{
    public interface ISqlFormatter
    {
        string DelimitField(string fieldName);
        string DelimitTable(string tableName);
    }
}