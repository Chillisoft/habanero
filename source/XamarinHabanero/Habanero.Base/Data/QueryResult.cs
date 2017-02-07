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

using System.Collections.Generic;

namespace Habanero.Base.Data
{
    /// <summary>
    /// Database result set container
    /// </summary>
    public class QueryResult : IQueryResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public QueryResult()
        {
            Fields = new List<IQueryResultField>();
            Rows = new List<IQueryResultRow>();
        }

        /// <summary>
        /// The rows in the result
        /// </summary>
        public List<IQueryResultRow> Rows { get; private set; }

        /// <summary>
        /// The fields/columns in the result
        /// </summary>
        public List<IQueryResultField> Fields { get; private set; }

        /// <summary>
        /// Adds a field to the list of fields.
        /// </summary>
        /// <param name="propertyName"></param>
        public void AddField(string propertyName)
        {
            Fields.Add(new QueryResultField(propertyName, Fields.Count));
        }

        /// <summary>
        /// Adds a result to the list of results.
        /// </summary>
        /// <param name="rawValues">the array of raw values</param>
        public void AddResult(object[] rawValues)
        {
            Rows.Add(new QueryResultRow(rawValues));   
        }

    }
}