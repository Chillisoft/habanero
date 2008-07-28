namespace Habanero.Base
{
    public class SqlFormatter : ISqlFormatter
    {
        private readonly string _leftFieldDelimiter;
        private readonly string _rightFieldDelimiter;

        public SqlFormatter(string leftFieldDelimiter, string rightFieldDelimiter)
        {
            _leftFieldDelimiter = leftFieldDelimiter;
            _rightFieldDelimiter = rightFieldDelimiter;
        }

        public string DelimitField(string fieldName)
        {
            return _leftFieldDelimiter + fieldName + _rightFieldDelimiter;
        }

        public string DelimitTable(string tableName)
        {
            return _leftFieldDelimiter + tableName + _rightFieldDelimiter;
        }

        public string LeftFieldDelimiter
        {
            get { return _leftFieldDelimiter; }
        }

        public string RightFieldDelimiter
        {
            get { return _rightFieldDelimiter; }
        }

            
    }
}