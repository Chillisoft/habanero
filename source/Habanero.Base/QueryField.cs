// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System;

namespace Habanero.Base
{
    /// <summary>
    /// Represents a field used in a query. In a database context, this represents a field listed 
    /// in the SELECT clause of a sql statement
    /// </summary>
    public class QueryField
    {
        /// <summary>
        /// Creates a QueryField with the given property name, field name and source name
        /// </summary>
        /// <param name="propertyName">The name of the property (as defined in the ClassDef) that this QueryField is for</param>
        /// <param name="fieldName">The name of the field in the data source that this QueryField is for</param>
        /// <param name="source">The source (such as a table) that this QueryField is from.</param>
        public QueryField(string propertyName, string fieldName, Source source)
        {
            PropertyName = propertyName;
            Source = source;
            FieldName = fieldName;
        }

        /// <summary>
        /// The name of the property (as defined in the ClassDef) that this QueryField is for
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// The name of the field in the data source that this QueryField is for
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// The name of the source (such as a table name) that this QueryField is from.
        /// </summary>
        public Source Source { get; set; }


        ///<summary>
        /// Creates a <see cref="QueryField"/> object by parsing a string in the correct format.
        /// The format is:
        /// <para>&lt;queryField&gt; => [&lt;source&gt;.]&lt;fieldName&gt; </para>
        /// <para>&lt;source&gt; => [&lt;source&gt;.]&lt;sourceName&gt; </para>
        /// For example: <code>Surname</code> or <code>ContactPerson.Company.Name</code>
        ///</summary>
        ///<param name="fieldString">The string in the correct format (see above)</param>
        ///<returns>A <see cref="QueryField"/> created from the string</returns>
        public static QueryField FromString(string fieldString)
        {
            if (fieldString == null) throw new ArgumentNullException("fieldString");
            string[] parts = fieldString.Trim().Split('.');
            string propertyName = parts[parts.Length - 1];
            Source source = Base.Source.FromString(String.Join(".", parts, 0, parts.Length - 1));
            return new QueryField(propertyName, propertyName, source);
        }
    }
}