//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Habanero.Base
{
    /// <summary>
    /// An interface that models a sql statement
    /// </summary>
    public interface ISqlStatement
    {
        /// <summary>
        /// Gets and sets the sql statement
        /// </summary>
        StringBuilder Statement { get; set; }

        /// <summary>
        /// Adds a parameter value
        /// </summary>
        /// <param name="paramName">The parameter name</param>
        /// <param name="paramValue">The value to assign</param>
        /// <returns>Returns a IDbDataParameter object</returns>
        /// TODO ERIC - how does this compare with the similarly-worded
        /// AddParameterToStatement()? If it's behaviour (to assign a value?)
        /// is different, it should be named differently too
        IDbDataParameter AddParameter(string paramName, object paramValue);

        /// <summary>
        /// Returns the list of parameters
        /// </summary>
        IList Parameters { get; }

        /// <summary>
        /// Sets up the IDbCommand object
        /// </summary>
        /// <param name="command">The command</param>
        void SetupCommand(IDbCommand command);

        /// <summary>
        /// Adds a parameter to the sql statement
        /// </summary>
        /// <param name="obj">The parameter to add</param>
        void AddParameterToStatement(object obj);

        ///<summary>
        /// Adds a join clause to the sql statement
        ///</summary>
        ///<param name="joinType">The type of join to be created. eg. 'LEFT JOIN'</param>
        ///<param name="joinTable">The table to be joined to this sql statement</param>
        ///<param name="joinCriteria">The criteria on which the join is created</param>
        void AddJoin(string joinType, string joinTable, string joinCriteria);

        /// <summary>
        /// Appends a criteria clause to the sql statement
        /// </summary>
        /// <param name="criteria">The criteria clause</param>
        void AppendCriteria(string criteria);

        /// <summary>
        /// Appends an order-by clause to the sql statement
        /// </summary>
        /// <param name="orderByCriteria">The order-by clause</param>
        void AppendOrderBy(string orderByCriteria);
        
        /// <summary>
        /// Appends a where statement to the sql statement
        /// </summary>
        void AppendWhere();

        /// <summary>
        /// Adds more fields to the select fields list in the statement.
        /// </summary>
        /// <param name="fields">The list of fields to add to the select statement</param>
        void AddSelectFields(List<string> fields);
    }
}