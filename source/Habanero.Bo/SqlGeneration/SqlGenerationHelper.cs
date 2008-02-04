//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

namespace Habanero.BO.SqlGeneration
{
    /// <summary>
    /// Provides some additional utilities for sql statement generators
    /// </summary>
    public class SqlGenerationHelper
    {
        /// <summary>
        /// Formats the table name correctly using field delimiters
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="connection">The connection used</param>
        /// <returns>Returns the formatted string</returns>
        public static string FormatTableName(string tableName, IDatabaseConnection connection)
        {
            return connection.LeftFieldDelimiter + tableName + connection.RightFieldDelimiter;
        }

        /// <summary>
        /// Formats the field name correctly using field delimiters
        /// </summary>
        /// <param name="fieldName">The name of the table</param>
        /// <param name="connection">The connection used</param>
        /// <returns>Returns the formatted string</returns>
        public static string FormatFieldName(string fieldName, IDatabaseConnection connection)
        {
            return connection.LeftFieldDelimiter + fieldName + connection.RightFieldDelimiter;
        }

        /// <summary>
        /// Formats the table and field name correctly using field delimiters if necessary
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="fieldName">The name of the field</param>
        /// <param name="connection">The connection used</param>
        /// <returns>Returns the formatted string</returns>
        public static string FormatTableAndFieldName(string tableName, string fieldName, IDatabaseConnection connection)
        {
            return String.Format("{0}.{1}",
                FormatTableName(tableName, connection),
                FormatFieldName(fieldName, connection));
        }
    }
}
