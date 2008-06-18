namespace Habanero.BO
{
    public class QueryField
    {
        private readonly string _propertyName;
        private readonly string _fieldName;

        public QueryField(string propertyName, string fieldName)
        {
            _propertyName = propertyName;
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
    }
}