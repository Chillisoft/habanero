#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
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
