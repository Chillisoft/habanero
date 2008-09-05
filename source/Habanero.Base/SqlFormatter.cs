namespace Habanero.Base
{
    ///<summary>
    /// Used to store specific SQL formatting information for any specified database.
    /// Typically databases differ in the characters used to differentiate fields and tables e.g. [ and ] for ms sql and
    /// ` for MySQL.
    ///</summary>
    public class SqlFormatter : ISqlFormatter
    {
        private readonly string _leftFieldDelimiter;
        private readonly string _rightFieldDelimiter;

        ///<summary>
        /// Constructor of a sql formatter
        ///</summary>
        ///<param name="leftFieldDelimiter">The left field delimiter to be used for formatting a sql statement</param>
        ///<param name="rightFieldDelimiter">The right field delimiter to be used for formatting a sql statement</param>
        public SqlFormatter(string leftFieldDelimiter, string rightFieldDelimiter)
        {
            _leftFieldDelimiter = leftFieldDelimiter;
            _rightFieldDelimiter = rightFieldDelimiter;
        }

        ///<summary>
        /// using the field delimiters it delimites the field name e.g. MyField will be returned as [MyField]
        ///</summary>
        ///<param name="fieldName">The table name to delimited</param>
        ///<returns>The delimited field name</returns>
        public string DelimitField(string fieldName)
        {
            return _leftFieldDelimiter + fieldName + _rightFieldDelimiter;
        }

        ///<summary>
        /// using the field delimiters it delimites the table name e.g. MyTable will be returned as [MyTable]
        ///</summary>
        ///<param name="tableName">The table name to delimited</param>
        ///<returns>The delimited table name</returns>
        public string DelimitTable(string tableName)
        {
            return _leftFieldDelimiter + tableName + _rightFieldDelimiter;
        }

        ///<summary>
        /// The left field delimiter to be used for formatting a sql statement
        ///</summary>
        public string LeftFieldDelimiter
        {
            get { return _leftFieldDelimiter; }
        }

        ///<summary>
        /// The right field delimiter to be used for formatting a sql statement<
        ///</summary>
        public string RightFieldDelimiter
        {
            get { return _rightFieldDelimiter; }
        }

            
    }
}