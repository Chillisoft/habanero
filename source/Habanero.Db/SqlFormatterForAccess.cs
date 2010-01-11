using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

namespace Habanero.DB
{
    ///<summary>
    /// Used to store specific SQL formatting information for an Access database.
    /// Typically databases differ in the characters used to differentiate fields and tables e.g. [ and ] for ms sql and
    /// ` for MySQL.
    ///</summary>
    public class SqlFormatterForAccess : SqlFormatter
    {
        ///<summary>
        /// Constructor of a sql formatter
        ///</summary>
        ///<param name="leftFieldDelimiter">The left field delimiter to be used for formatting a sql statement</param>
        ///<param name="rightFieldDelimiter">The right field delimiter to be used for formatting a sql statement</param>
        ///<param name="limitClauseAtBeginning"></param>
        ///<param name="limitClauseAtEnd"></param>
        public SqlFormatterForAccess(string leftFieldDelimiter, string rightFieldDelimiter, string limitClauseAtBeginning, string limitClauseAtEnd) 
            : base(leftFieldDelimiter, rightFieldDelimiter, limitClauseAtBeginning, limitClauseAtEnd)
        {
        }

        /// <summary>
        /// Prepares the value to be converted to a format appropriate for sql
        /// </summary>
        /// <param name="objValue">The value to prepare</param>
        /// <returns>Returns the reformatted object</returns>
        public override object PrepareValue(object objValue)
        {
            if (objValue is bool)
            {
                return (bool)objValue ? -1 : 0;
            }
            return base.PrepareValue(objValue);
        }
    }
}
