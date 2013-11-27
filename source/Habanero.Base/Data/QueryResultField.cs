namespace Habanero.Base.Data
{
    /// <summary>
    /// A field for a <see cref="QueryResult"/>.
    /// </summary>
    public class QueryResultField : IQueryResultField
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="index"></param>
        public QueryResultField(string propertyName, int index)
        {
            PropertyName = propertyName;
            Index = index;
        }

        /// <summary>
        /// The name of the field
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// The index of the field (position in the result set)
        /// </summary>
        public int Index { get; set; }
    }
}