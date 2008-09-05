namespace Habanero.Base
{
    ///<summary>
    /// Used to store specific SQL formatting information for any specified database.
    /// Typically databases differ in the characters used to differentiate fields and tables e.g. [ and ] for ms sql and
    /// ` for MySQL.
    ///</summary>
    public interface ISqlFormatter
    {
        ///<summary>
        /// using the field delimiters it delimites the field name e.g. MyField will be returned as [MyField]
        ///</summary>
        ///<param name="fieldName">The table name to delimited</param>
        ///<returns>The delimited field name</returns>
        string DelimitField(string fieldName);

        ///<summary>
        /// using the field delimiters it delimites the table name e.g. MyTable will be returned as [MyTable]
        ///</summary>
        ///<param name="tableName">The table name to delimited</param>
        ///<returns>The delimited table name</returns>
        string DelimitTable(string tableName);
    }
}