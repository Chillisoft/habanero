namespace Habanero.BO
{
    public class QueryField
    {
        private readonly string _propertyName;
        private readonly string _fieldName;
        private readonly string _sourceName;

        public QueryField(string propertyName, string fieldName, string sourceName)
        {
            _propertyName = propertyName;
            _sourceName = sourceName;
            _fieldName = fieldName;
        }


        public string PropertyName
        {
            get { return _propertyName; }
        }

        public string FieldName
        {
            get { return _fieldName; }
        }

        public string SourceName
        {
            get { return _sourceName; }
        }
    }
}