namespace Habanero.Base
{
    /// <summary>
    /// Represents a field used in a query. In a database context, this represents a field listed 
    /// in the SELECT clause of a sql statement
    /// </summary>
    public class QueryField
    {
        private readonly string _propertyName;
        private readonly string _fieldName;
        private readonly string _sourceName;

        /// <summary>
        /// Creates a QueryField with the given property name, field name and source name
        /// </summary>
        /// <param name="propertyName">The name of the property (as defined in the ClassDef) that this QueryField is for</param>
        /// <param name="fieldName">The name of the field in the data source that this QueryField is for</param>
        /// <param name="sourceName">The name of the source (such as a table name) that this QueryField is from.</param>
        public QueryField(string propertyName, string fieldName, string sourceName)
        {
            _propertyName = propertyName;
            _sourceName = sourceName;
            _fieldName = fieldName;
        }

        /// <summary>
        /// The name of the property (as defined in the ClassDef) that this QueryField is for
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }

        /// <summary>
        /// The name of the field in the data source that this QueryField is for
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
        }

        /// <summary>
        /// The name of the source (such as a table name) that this QueryField is from.
        /// </summary>
        public string SourceName
        {
            get { return _sourceName; }
        }
    }
}