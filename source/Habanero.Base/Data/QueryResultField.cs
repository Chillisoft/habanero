namespace Habanero.Base.Data
{
    public class QueryResultField : IQueryResultField
    {
        public QueryResultField(string propertyName, int index)
        {
            PropertyName = propertyName;
            Index = index;
        }

        public string PropertyName { get; private set; }
        public int Index { get; set; }
    }
}