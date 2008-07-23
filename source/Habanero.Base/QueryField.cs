//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

namespace Habanero.Base
{
    /// <summary>
    /// Represents a field used in a query. In a database context, this represents a field listed 
    /// in the SELECT clause of a sql statement
    /// </summary>
    public class QueryField
    {
        private readonly string _propertyName;
        private string _fieldName;
        private Source _source;

        /// <summary>
        /// Creates a QueryField with the given property name, field name and source name
        /// </summary>
        /// <param name="propertyName">The name of the property (as defined in the ClassDef) that this QueryField is for</param>
        /// <param name="fieldName">The name of the field in the data source that this QueryField is for</param>
        /// <param name="source">The source (such as a table) that this QueryField is from.</param>
        public QueryField(string propertyName, string fieldName, Source source)
        {
            _propertyName = propertyName;
            _source = source;
            _fieldName = fieldName;
        }

        /// <summary>
        /// The name of the property (as defined in the ClassDef) that this QueryField is for
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }

        /// <summary>
        /// The name of the field in the data source that this QueryField is for
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        /// <summary>
        /// The name of the source (such as a table name) that this QueryField is from.
        /// </summary>
        public Source Source
        {
            get { return _source; }
            set { _source = value; }
        }
    }
}